using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hack.Fast.Extensions.Utility
{
    public class Encreptor
    {
        public static string GetSign(List<string> paramList, string key)
        {
            paramList.Sort();
            string _params = string.Empty;
            foreach (string _param in paramList)
            {
                string[] arr = _param.Split('=');
                string str = arr[0].ToLower() + "=" + arr[1];
                _params += str + "&";
            }
            _params = _params.TrimEnd('&');
            return EncryptMd5(_params + key);
        }

        public static string EncryptMd5(string encryptStr)
        {
            byte[] result = Encoding.Default.GetBytes(encryptStr.Trim());
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return BitConverter.ToString(output).Replace("-", "").ToLower();
        }
        public static string Md5Hash(string input, string encod)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.GetEncoding(encod).GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
