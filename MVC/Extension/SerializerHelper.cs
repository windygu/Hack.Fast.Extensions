using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Mysoft.Map.MVC
{
	internal static class SerializerHelper
	{
		/// <summary>
		/// 将对象执行JSON序列化
		/// </summary>
		/// <param name="obj">要序列化的对象</param>
		/// <returns>JSON序列化的结果</returns>
		internal static string ToJson(this object obj)
		{
			//JavaScriptSerializer jss = new JavaScriptSerializer();
			//return jss.Serialize(obj);
			return Hack.Fast.Extensions.Json.JsonHelper.JsonSerialize(obj);
		}


		/// <summary>
		/// 从JSON字符串中反序列化对象
		/// </summary>
		/// <typeparam name="T">反序列化的结果类型</typeparam>
		/// <param name="json">JSON字符串</param>
		/// <returns>反序列化的结果</returns>
		internal static T DeserializeFromJson<T>(this string json)
		{
			//JavaScriptSerializer jss = new JavaScriptSerializer();
			//return jss.Deserialize<T>(json);
			return Hack.Fast.Extensions.Json.JsonHelper.JsonDeserialize<T>(json);
		}

		/// <summary>
		/// 将对象执行XML序列化
		/// </summary>
		/// <param name="obj">要序列化的对象</param>
		/// <returns>XML序列化的结果</returns>
		internal static string ToXml(this object obj)
		{
			return Hack.Fast.Extensions.Xml.XmlHelper.XmlSerialize(obj, Encoding.UTF8);
		}


		/// <summary>
		/// 从XML字符串中反序列化对象
		/// </summary>
		/// <typeparam name="T">反序列化的结果类型</typeparam>
		/// <param name="xml">XML字符串</param>
		/// <returns>反序列化的结果</returns>
		internal static T DeserializeFromXml<T>(this string xml)
		{
			return Hack.Fast.Extensions.Xml.XmlHelper.XmlDeserialize<T>(xml, Encoding.UTF8);
		}

	}
}
