using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Collections.Specialized;
using System.IO;
using System.Web.Configuration;
using System.Configuration;
using System.IO.Compression;
using System.Reflection;
using Mysoft.Map.Tools;
using  Mysoft.Map.MVC;

namespace Hack.Fast.Extensions
{
	/// <summary>
	/// 使用Cookie实现SessionStateStoreProviderBase
	/// 注意：它只适合保存简单的基元类型数据。
	/// </summary>
	public class CookieSessionStateStore : SessionStateStoreProviderBase
	{
		internal static readonly string s_cookieName = "_sic";
		internal static readonly int s_timeout;



		static CookieSessionStateStore()
		{
			SessionStateSection configSection =
						ConfigurationManager.GetSection("system.web/sessionState") as SessionStateSection;
			s_timeout = (int)configSection.Timeout.TotalMinutes;
		}

		private void SaveToCookie(HttpContext context, SessionStateItemCollection items, int? timeout)
		{
			string cookieValue = Serialize(context, items);

			HttpCookie current = context.Request.Cookies[s_cookieName];
			if( current != null ) {
				if( current.Value == cookieValue )
					return;
			}


			HttpCookie cookie = new HttpCookie(s_cookieName, cookieValue);
			cookie.HttpOnly = true;

			// Cookie没有设置过期时间，表示为临时Cookie

			context.Response.AppendCookie(cookie);
		}

		private SessionStateItemCollection GetFromCookie()
		{
			HttpCookie cookie = HttpContext.Current.Request.Cookies[s_cookieName];
			if( cookie == null )
				return null;

			return Deserialize(cookie.Value);
		}

		// Cookie 限制
		// http://support.microsoft.com/kb/306070/zh-cn
		// http://support.microsoft.com/kb/941495/zh-cn
		// http://browsercookielimits.x64.me/
		private static string Serialize(HttpContext context, SessionStateItemCollection sessionItems)
		{
			if( sessionItems == null || sessionItems.Count == 0 )
				return null;

			using( MemoryStream ms = new MemoryStream() ) {
				using( BinaryWriter write = new BinaryWriter(ms) ) {
					sessionItems.Serialize(write);

					// 将Session集合序列化
					byte[] serializeBytes = ms.ToArray();

					// 用GZIP压缩序列化的结果
					byte[] gzipBytes = CompressHelper.CompressBytes(serializeBytes);

					// 将BYTE数组转成BASE64字符串
					string val = Convert.ToBase64String(gzipBytes);

					//string val = Convert.ToBase64String(ms.ToArray());

					// 如果“Set-Cookie”头的长度超过 5118 个字节，则 Internet Explorer 和 HTTP Wininet API 将忽略“Set-Cookie”头。
					// 但是其它的浏览器只支持到4K

					//if( string.Compare(context.Request.Browser.Browser, "IE", StringComparison.OrdinalIgnoreCase) == 0 ) {
					//    if( val.Length > 5118 )
					//        throw new InvalidDataException("Session内容太长，当前长度：" + val.Length);
					//}
					//else {
					//    if( val.Length > 4096 )
					//        throw new InvalidDataException("Session内容太长，当前长度：" + val.Length);
					//}

					if( val.Length > 4090 )
						throw new InvalidDataException("Session内容太长，当前长度：" + val.Length);

					// 对于较长的Cookie，可以拆分成多个Cookie，就可以突破这个限制，
					// 但是，如果全部Cookie的总长度太长，会给每个请求带来负担，

					return val;
				}
			}
		}

		private static SessionStateItemCollection Deserialize(string input)
		{
			if( string.IsNullOrEmpty(input) )
				return null;

			try {
				// 从BASE64字符串中还原GZIP压缩的结果
				byte[] gzipBytes = Convert.FromBase64String(input);

				// 解压缩GZIP结果，还原序列化前的Session字节数组
				byte[] serializeBytes = CompressHelper.DecompressBytes(gzipBytes);

				//byte[] serializeBytes = Convert.FromBase64String(input);

				// 根据字节数组还原Session集合。
				using( MemoryStream ms = new MemoryStream(serializeBytes) ) {
					using( BinaryReader reader = new BinaryReader(ms) ) {
						SessionStateItemCollection collection = SessionStateItemCollection.Deserialize(reader);
						collection.Dirty = false;
						return collection;
					}
				}
			}
			catch {
				return null;
			}
		}

		/// <summary>
		/// 创建要用于当前请求的新 System.Web.SessionState.SessionStateStoreData 对象。
		/// </summary>
		/// <param name="context">当前请求的 System.Web.HttpContext。</param>
		/// <param name="timeout">新 System.Web.SessionState.SessionStateStoreData 的会话状态 System.Web.SessionState.HttpSessionState.Timeout值。</param>
		/// <returns>当前请求的新 System.Web.SessionState.SessionStateStoreData。</returns>
		public override SessionStateStoreData CreateNewStoreData(HttpContext context, int timeout)
		{
			return CreateLegitStoreData(context, null, null, timeout);
		}

		internal static SessionStateStoreData CreateLegitStoreData(HttpContext context, ISessionStateItemCollection sessionItems, HttpStaticObjectsCollection staticObjects, int timeout)
		{
			if( sessionItems == null )
				sessionItems = new SessionStateItemCollection();
			if( staticObjects == null && context != null )
				staticObjects = SessionStateUtility.GetSessionStaticObjects(context);
			return new SessionStateStoreData(sessionItems, staticObjects, timeout);
		}

		/// <summary>
		/// 将新的会话状态项添加到数据存储区中。
		/// </summary>
		/// <param name="context">当前请求的 System.Web.HttpContext。</param>
		/// <param name="id">当前请求的 System.Web.SessionState.HttpSessionState.SessionID。</param>
		/// <param name="timeout">当前请求的会话 System.Web.SessionState.HttpSessionState.Timeout。</param>
		public override void CreateUninitializedItem(HttpContext context, string id, int timeout)
		{
			SaveToCookie(context, null, timeout);
		}

		/// <summary>
		/// 实现销毁接口
		/// </summary>
		public override void Dispose()
		{
		}

		private SessionStateStoreData DoGet(HttpContext context, string id, bool exclusive, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actionFlags)
		{
			locked = false;
			lockId = null;
			lockAge = TimeSpan.Zero;
			actionFlags = SessionStateActions.None;

			SessionStateItemCollection sessionItems = GetFromCookie();
			if( sessionItems == null )
				return null;

			return CreateLegitStoreData(context, sessionItems, null, s_timeout);
		}

		/// <summary>
		///  在请求结束时由 System.Web.SessionState.SessionStateModule 对象调用。
		/// </summary>
		/// <param name="context">当前请求的 System.Web.HttpContext。</param>
		public override void EndRequest(HttpContext context)
		{
		}

		/// <summary>
		/// 从会话数据存储区中返回只读会话状态数据。
		/// </summary>
		/// <param name="context">当前请求的 System.Web.HttpContext。</param>
		/// <param name="id">当前请求的 System.Web.SessionState.HttpSessionState.SessionID。</param>
		/// <param name="locked">当此方法返回时，如果请求的会话项在会话数据存储区被锁定，请包含一个设置为 true 的布尔值；否则请包含一个设置为 false 的布尔值。</param>
		/// <param name="lockAge">当此方法返回时，请包含一个设置为会话数据存储区中的项锁定时间的 System.TimeSpan 对象。</param>
		/// <param name="lockId">当此方法返回时，请包含一个设置为当前请求的锁定标识符的对象。有关锁定标识符的详细信息，请参见 System.Web.SessionState.SessionStateStoreProviderBase 类摘要中的“锁定会话存储区数据”。</param>
		/// <param name="actionFlags">当此方法返回时，请包含 System.Web.SessionState.SessionStateActions 值之一，指示当前会话是否为未初始化的无Cookie 会话。</param>
		/// <returns>使用会话数据存储区中的会话值和信息填充的 System.Web.SessionState.SessionStateStoreData。</returns>
		public override SessionStateStoreData GetItem(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actionFlags)
		{
			return this.DoGet(context, id, false, out locked, out lockAge, out lockId, out actionFlags);
		}

		/// <summary>
		///  从会话数据存储区中返回只读会话状态数据。
		/// </summary>
		/// <param name="context">当前请求的 System.Web.HttpContext。</param>
		/// <param name="id">当前请求的 System.Web.SessionState.HttpSessionState.SessionID。</param>
		/// <param name="locked">当此方法返回时，如果成功获得锁定，请包含一个设置为 true 的布尔值；否则请包含一个设置为 false 的布尔值。</param>
		/// <param name="lockAge">当此方法返回时，请包含一个设置为会话数据存储区中的项锁定时间的 System.TimeSpan 对象。</param>
		/// <param name="lockId">当此方法返回时，请包含一个设置为当前请求的锁定标识符的对象。有关锁定标识符的详细信息，请参见 System.Web.SessionState.SessionStateStoreProviderBase类摘要中的“锁定会话存储区数据”。</param>
		/// <param name="actionFlags"> 当此方法返回时，请包含 System.Web.SessionState.SessionStateActions 值之一，指示当前会话是否为未初始化的无Cookie 会话。</param>
		/// <returns>使用会话数据存储区中的会话值和信息填充的 System.Web.SessionState.SessionStateStoreData。</returns>
		public override SessionStateStoreData GetItemExclusive(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actionFlags)
		{
			return this.DoGet(context, id, true, out locked, out lockAge, out lockId, out actionFlags);
		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">名称</param>
		/// <param name="config">配置集合</param>
		public override void Initialize(string name, NameValueCollection config)
		{
			if( string.IsNullOrEmpty(name) )
				name = "Cookie Session State Provider";
			base.Initialize(name, config);
		}

		/// <summary>
		/// 初始化请求
		/// </summary>
		/// <param name="context">Http上下文</param>
		public override void InitializeRequest(HttpContext context)
		{
		}

		/// <summary>
		///  释放对会话数据存储区中项的锁定。
		/// </summary>
		/// <param name="context">当前请求的 System.Web.HttpContext。</param>
		/// <param name="id">当前请求的会话标识符。</param>
		/// <param name="lockId">当前请求的锁定标识符。</param>
		public override void ReleaseItemExclusive(HttpContext context, string id, object lockId)
		{
		}

		/// <summary>
		/// RemoveItem
		/// </summary>
		/// <param name="context">当前请求的 System.Web.HttpContext。</param>
		/// <param name="id">当前请求的会话标识符。</param>
		/// <param name="lockId">当前请求的锁定标识符。</param>
		/// <param name="item"></param>
		public override void RemoveItem(HttpContext context, string id, object lockId, SessionStateStoreData item)
		{
			HttpCookie cookie = new HttpCookie(s_cookieName);
			cookie.HttpOnly = true;
			cookie.Expires = DateTime.MinValue;

			HttpContext.Current.Response.AppendCookie(cookie);
		}

		/// <summary>
		///  删除会话数据存储区中的项数据。
		/// </summary>
		/// <param name="context">当前请求的 System.Web.HttpContext。</param>
		/// <param name="id">当前请求的会话标识符。</param>
		public override void ResetItemTimeout(HttpContext context, string id)
		{
		}

		/// <summary>
		/// 使用当前请求中的值更新会话状态数据存储区中的会话项信息，并清除对数据的锁定。
		/// </summary>
		/// <param name="context">当前请求的 System.Web.HttpContext。</param>
		/// <param name="id">当前请求的会话标识符。</param>
		/// <param name="item"></param>
		/// <param name="lockId">当前请求的锁定标识符。</param>
		/// <param name="newItem"></param>
		public override void SetAndReleaseItemExclusive(HttpContext context, string id, SessionStateStoreData item, object lockId, bool newItem)
		{
			if( item.Items.Dirty == false )
				return;

			SessionStateItemCollection sessionItems = null;

			if( item.Items.Count > 0 ) {
				//目前只支持基元类型,String,DateTime,Guid等类型，防止存储的数据过大。
				for( int i = 0; i < item.Items.Count; i++ ) {
					if( item.Items[i] != null ) {
						Type t = item.Items[i].GetType();
						if(t.IsSupportableType()==false) {
							throw new NotSupportedException(item.Items.Keys[i] + "不是基元类型和String,DateTime，Guid等类型，目前Session存储只支持基元类型,String类型,DateTime类型，Guid类型\r\n 基元类型有：Boolean, Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64, IntPtr, UIntPtr, Char, Double, Single.");
						}
					}
				}
				sessionItems = (SessionStateItemCollection)item.Items;
			}


			SaveToCookie(context, sessionItems, item.Timeout);
		}

		/// <summary>
		/// 设置对 Global.asax 文件中定义的 Session_OnEnd 事件的 System.Web.SessionState.SessionStateItemExpireCallback 委托的引用。
		/// </summary>
		/// <param name="expireCallback"> 对 Global.asax 文件中定义的 Session_OnEnd 事件的 System.Web.SessionState.SessionStateItemExpireCallback委托。</param>
		/// <returns>如果会话状态存储提供程序支持调用 Session_OnEnd 事件，则为 true；否则为 false。</returns>
		public override bool SetItemExpireCallback(SessionStateItemExpireCallback expireCallback)
		{
			return true;
		}





	}
}
