using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Mysoft.Map.MVC
{

	/// <summary>
	/// 一个Json对象结果
	/// </summary>
	public sealed class JsonResult : IActionResult
	{
		/// <summary>
		/// 
		/// </summary>
		public object Model { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		public JsonResult(object model)
		{
			if( model == null )
				throw new ArgumentNullException("model");

			this.Model = model;
		}

		void IActionResult.Ouput(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string json = this.Model.ToJson();
			context.Response.Write(json);
		}

		/// <summary>
		/// 将一个对象序列化为JSON字符串
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static string ObjectToJson(object data)
		{
			if( data == null )
				throw new ArgumentNullException("data");

			return data.ToJson();
		}
	}


}
