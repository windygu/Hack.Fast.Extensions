using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Mysoft.Map.MVC
{
	/// <summary>
	/// 表示一个重定向的结果
	/// </summary>
	public sealed class RedirectResult : IActionResult
	{
		/// <summary>
		/// 
		/// </summary>
		public string Url { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="url"></param>
		public RedirectResult(string url)
		{
			if( string.IsNullOrEmpty(url) )
				throw new ArgumentNullException("url");
			Url = url;
		}

		void IActionResult.Ouput(HttpContext context)
		{
			context.Response.Redirect(Url, true);
		}
	}

}
