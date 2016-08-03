﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hack.Fast.Extensions.DAL
{
	/// <summary>
	/// 表示忽略指定的数据列。
	/// </summary>
	/// <example>
	///		<para>下面的代码演示了略指定的数据列的用法</para>
	///		<code>
	///		public class cbContract{
	///			//这个属性将不会被数据访问层处理
	///			[IgnoreColumn]
	///			public string MyContractCode { get; set; }
	///		}
	///		</code>
	/// </example>
	[AttributeUsageAttribute(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class IgnoreColumnAttribute: Attribute
	{

	}
}
