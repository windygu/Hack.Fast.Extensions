using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Mysoft.Map.MVC
{
	/// <summary>
	/// IActionResult 
	/// </summary>
	public interface IActionResult
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		void Ouput(HttpContext context);
	}
}
