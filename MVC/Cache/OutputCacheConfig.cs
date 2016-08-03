using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Mysoft.Map.MVC
{
	/// <summary>
	/// 
	/// </summary>
	[XmlRoot("OutputCache")]
	public class OutputCacheConfig
	{
		/// <summary>
		/// 
		/// </summary>
		[XmlArrayItem("Setting")]
		public List<OutputCacheSetting> Settings = new List<OutputCacheSetting>();
	}

	/// <summary>
	/// 
	/// </summary>
	public class OutputCacheSetting : OutputCacheAttribute
	{
		/// <summary>
		/// 
		/// </summary>
		[XmlAttribute]
		public string FilePath { get; set; }
	}

}
