using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using Hack.Fast.Extensions.Xml;


namespace Hack.Fast.Extensions.Utility
{
	/// <summary>
	/// 地图工具类
	/// </summary>
    public static class MapHelper
    {
        //GCJ-02(火星，高德) 坐标转换成 BD-09(百度) 坐标
        //@param bd_lon 百度经度
        //@param bd_lat 百度纬度
        public static Dictionary<string, double> ConvertToBaidu(double gg_lon, double gg_lat)
        {
            var x_pi = 3.14159265358979324 * 3000.0 / 180.0;
            var x = gg_lon;
            var y = gg_lat;
            var z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * x_pi);
            var theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * x_pi);
            Dictionary<string, double> dicData = new Dictionary<string, double>();
            dicData["lon"] = z * Math.Cos(theta) + 0.0065;
            dicData["lat"] = z * Math.Sin(theta) + 0.006;
            return dicData;
        }

        //BD-09(百度) 坐标转换成  GCJ-02(火星，高德) 坐标
        //@param bd_lon 百度经度
        //@param bd_lat 百度纬度
        public static Dictionary<string, double> ConvertToGoogle(double bd_lon, double bd_lat)
        {
            var x_pi = 3.14159265358979324 * 3000.0 / 180.0;
            var x = bd_lon - 0.0065;
            var y = bd_lat - 0.006;
            var z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * x_pi);
            var theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * x_pi);
            Dictionary<string, double> dicData = new Dictionary<string, double>();
            dicData["lon"] = z * Math.Cos(theta);
            dicData["lat"] = z * Math.Sin(theta);
            return dicData;
        }
    }
}
