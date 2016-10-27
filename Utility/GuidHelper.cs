using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Hack.Fast.Extensions.Utility
{
    public class GuidHelper
    {

        /// <summary>
        /// 生成有序GUID，为何如此实现
        /// 请参见如下网址说明。
        /// http://msdn.microsoft.com/zh-cn/library/ms189786%28v=sql.120%29.aspx
        /// http://www.pinvoke.net/default.aspx/rpcrt4/UuidCreateSequential.html
        /// </summary>
        /// <returns></returns>
        public static Guid NewSeqGuid()
        {
            Guid result;
            UuidCreateSequential(out result);
            return result;
        }

        [DllImport("rpcrt4.dll", SetLastError = true)]
        private static extern int UuidCreateSequential(out Guid guid);
    }
}
