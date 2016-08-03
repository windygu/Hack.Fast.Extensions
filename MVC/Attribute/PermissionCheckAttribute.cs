using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using Hack.Fast.Extensions;

namespace Mysoft.Map.MVC
{
	/// <summary>
	/// 提供给明源Map使用,用于包含FunctionCode,ActionCode权限验证的通道请求
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public sealed class PermissionCheckAttribute : AuthorizeAttribute
	{
		/// <summary>
		/// 权限集合,格式为FunctionCode|ActionCode,ActionCode，例如:02010103|02,03。
		/// </summary>
		public string[] Permissions { get; private set; }

		/// <summary>
		/// 初始化权限集合
		/// </summary>
		/// <param name="permissions">权限集合,格式为FunctionCode|ActionCode,ActionCode，例如:02010103|02,03。</param>
		public PermissionCheckAttribute(params string[] permissions)
		{
			if( permissions == null ) 
				throw new ArgumentNullException("permissions未配置。");

			if( permissions.Length == 0 )
				throw new ArgumentNullException("permissions未配置。");

			Permissions = permissions;
		}

		/// <summary>
		/// 通过Mysoft.Map.Application.Security.User.CheckUserRight方法,验证当前登录用户是否有指定的FunctionCode,ActionCode权限
		/// </summary>
		/// <param name="context">Http上下文</param>
		/// <returns>是否拥有权限</returns>
		public override bool AuthenticateRequest(HttpContext context)
		{
			if (context.Session == null)
				throw new NotSupportedException("当前作用域不支持Session。");

			object obj = context.Session["UserGUID"];

			if( obj == null ) {
				throw new InvalidOperationException("UserGUID为空。");
			}

			if (DecouplingUtils.CheckUserRight == null)
				throw new InvalidOperationException("Mysoft.Map.Application.Security.User.CheckUserRight方法未找到。");

			string userid = obj.ToString();

			foreach( string permission in Permissions ) {
				string[] funcs = permission.Split('|');

				if (funcs.Length != 2){
					throw new InvalidOperationException("permission参数不正确,正确的值应该为:FunctionCode|ActionCode,ActionCode，例如:02010103|01,02。");
				}

				string funcId = funcs[0];
				string strActId = funcs[1];

				if( string.IsNullOrEmpty(funcId)  || string.IsNullOrEmpty(strActId) ) {
					throw new InvalidOperationException("permission参数不正确,正确的值应该为:FunctionCode|ActionCode,ActionCode，例如:02010103|02,03。");
				}

				string[] actIds = funcs[1].Split(',');

				foreach( string actId in actIds ) {
					bool result = DecouplingUtils.CheckUserRight(userid, funcId, actId);
					if( result ) 
						return true;
				}
			}

			return false;
		}
	}
}
