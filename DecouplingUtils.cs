using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hack.Fast.Extensions
{
	/// <summary>
	/// 与Map工程解耦帮助类
	/// </summary>
	internal static class DecouplingUtils
	{
		/// <summary>
		/// 解耦以下方法:
		/// Mysoft.Map.Application.Security.User.CheckUserRight
		/// </summary>
		internal static Func<string, string, string, bool> CheckUserRight;
	}
}
