using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Mysoft.Map.MVC
{
	/// <summary>
	/// 表示 Http 404 状态码
	/// </summary>
	public sealed class Http404Result : IActionResult
	{
		/// <summary>
		/// 设置StatusCode
		/// </summary>
		/// <param name="context">Http上下文</param>
		public void Ouput(HttpContext context)
		{
			context.Response.StatusCode = 404;
		}

	}
}
