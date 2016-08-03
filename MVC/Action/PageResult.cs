using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Mysoft.Map.MVC
{
	
	/// <summary>
	/// 表示一个页面结果（页面将由框架执行）
	/// </summary>
	public sealed class PageResult : IActionResult
	{
		/// <summary>
		/// 
		/// </summary>
		public string VirtualPath { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public object Model { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="virtualPath"></param>
		public PageResult(string virtualPath) : this(virtualPath, null)
		{
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="virtualPath"></param>
		/// <param name="model"></param>
		public PageResult(string virtualPath, object model)
		{
			this.VirtualPath = virtualPath;
			this.Model = model;
		}

		void IActionResult.Ouput(HttpContext context)
		{
			if( string.IsNullOrEmpty(this.VirtualPath) )
				this.VirtualPath = context.Request.FilePath;

			context.Response.ContentType = "text/html";
			string html = PageExecutor.Render(context, VirtualPath, Model);
			context.Response.Write(html);
		}
	}


}
