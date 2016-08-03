using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Mysoft.Map.MVC.Serializer
{
	internal interface IActionParametersProvider
	{
		object[] GetParameters(HttpContext context, ActionDescription action);

	}
}
