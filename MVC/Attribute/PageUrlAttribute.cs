﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mysoft.Map.MVC
{
	/// <summary>
	/// 用于描述一个Action可以处理哪些请求路径。
	/// 注意：这个Attribute可以多次使用，表示可以处理多个请求路径。
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class PageUrlAttribute : Attribute
	{
		/// <summary>
		/// 指示可以处理的请求路径。比如："/abc.aspx" 
		/// （Ajax请求【不使用】此参数）
		/// </summary>
		public string Url { get; set; }
	}


	/// <summary>
	/// 继承于PageUrlAttribute，指示Url是一个正则表达式
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public sealed class PageRegexUrlAttribute : PageUrlAttribute
	{
		// 使用方法：

		//[Action]
		//[OutputCache(Duration = 31536000)]
		//[PageRegexUrl(Url = @"/m/(?<id>[^/]{36})\.aspx")]
		//public object ShowMedia(string id)
		//{
		//    MediaObject media = _bll.GetMediaObject(id);

		//    return new StreamResult(media.bits, media.type);
		//}


		// 注意：正则表达式的匹配的次序未知。
	}
}



