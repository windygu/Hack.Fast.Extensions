using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Hack.Fast.Extensions.Xml;

namespace Mysoft.Map.MVC
{
	/// <summary>
	/// 表示以XML形式返回结果
	/// </summary>
	public sealed class XmlResult : IActionResult
	{
		/// <summary>
		/// 
		/// </summary>
		public object Model { get; private set; }

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="model"></param>
		public XmlResult(object model)
		{
			if( model == null )
				throw new ArgumentNullException("model");

			this.Model = model;
		}

		void IActionResult.Ouput(HttpContext context)
		{
			context.Response.ContentType = "application/xml";
			string xml = XmlHelper.XmlSerialize(Model, Encoding.UTF8);
			context.Response.Write(xml);
		}
	}
}
