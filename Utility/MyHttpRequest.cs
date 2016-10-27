using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hack.Fast.Extensions.Utility
{
    public class MyHttpRequest
    {
         static MyHttpRequest()
        {
            ServicePointManager.DefaultConnectionLimit = 128;
        }
        /// <summary>
        /// 
        /// </summary>
        public HttpWebRequest Request { get; private set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public MyHttpRequest(string url)
            : this(new Uri(url))
        {

        }

        #region 引入事件，为了拿到请求头
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<HttpEventArgs> GetResponseHeadCollection;
        internal void FireResponseHead(WebHeaderCollection headerCollection)
        {
            EventHandler<HttpEventArgs> handler = GetResponseHeadCollection;
            if (handler != null)
            {
                HttpEventArgs eventArgs = new HttpEventArgs(headerCollection);
                handler.Invoke(null, eventArgs);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public class HttpEventArgs : EventArgs
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="headerCollection"></param>
            public HttpEventArgs(WebHeaderCollection headerCollection)
            {
                HeaderCollection = headerCollection;
            }
            /// <summary>
            /// 
            /// </summary>
            public WebHeaderCollection HeaderCollection { get; set; }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        public MyHttpRequest(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            //var proxy = new WebProxy("127.0.0.1", 8888);
            //request.Proxy = proxy;

            this.Request = request;

            //request.Method = string.IsNullOrEmpty(method) ? "POST" : method;
            Request.UserAgent = " C# Client";
            //request.Credentials = CredentialCache.DefaultCredentials;
            Request.Headers.Add("x-charset", "utf-8");
            Request.ContentType = "application/x-www-form-urlencoded";


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cookieContainer"></param>
        public void BindCookie(CookieContainer cookieContainer)
        {
            if (cookieContainer == null)
                throw new ArgumentNullException("cookieContainer");

            this.Request.CookieContainer = cookieContainer;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Send()
        {
            return Send(new byte[0]);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        public string Send(byte[] postData)
        {
            if (postData != null && postData.Length > 0)
            {
                this.Request.Method = "POST";

                using (BinaryWriter bw = new BinaryWriter(this.Request.GetRequestStream()))
                {
                    bw.Write(postData);
                }
            }

            using (HttpWebResponse response = (HttpWebResponse)this.Request.GetResponse())
            {
                FireResponseHead(response.Headers);
                return ReadResponse(response);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Stream GetImage(string data)
        {
            byte[] postData = Encoding.UTF8.GetBytes(data);
            if (postData != null && postData.Length > 0)
            {
                this.Request.Method = "POST";

                using (BinaryWriter bw = new BinaryWriter(this.Request.GetRequestStream()))
                {
                    bw.Write(postData);
                }
            }

            using (HttpWebResponse response = (HttpWebResponse)this.Request.GetResponse())
            {
                Stream strem = null;
                if (response.Headers["Content-Encoding"] == "gzip")
                    strem = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                else
                    strem = response.GetResponseStream();
                return strem;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        public string Send(string postData)
        {
            if (string.IsNullOrEmpty(postData))
                return Send(new byte[0]);

            return Send(Encoding.UTF8.GetBytes(postData));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        public string Send(WebFormData postData)
        {
            if (postData == null)
                throw new ArgumentNullException("postData");

            return Send(postData.ToString());
        }

        /// <summary>
        /// 提供上传附件的功能
        /// </summary>
        /// <param name="postingData"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public string Send(Dictionary<string, string> postingData, List<HttpUploadingFile> files)
        {
            if (postingData != null && files.Count > 0)
            {
                this.Request.Method = "POST";
              
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    StreamWriter writer = new StreamWriter(memoryStream);
                    if (files.Count > 0)
                    {
                        string newLine = "\r\n";
                        string boundary = Guid.NewGuid().ToString().Replace("-", "");
                        this.Request.ContentType = "multipart/form-data; boundary=----" + boundary;

                        foreach (string key in postingData.Keys)
                        {
                            writer.Write("------" + boundary + newLine);
                            writer.Write(string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{1}", key,
                                newLine));
                            writer.Write(postingData[key] + newLine);
                        }

                        foreach (HttpUploadingFile file in files)
                        {
                            writer.Write("------" + boundary + newLine);
                            writer.Write(string.Format(
                                "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"{2}",
                                file.FieldName,
                                file.FileName,
                                newLine
                                ));
                            writer.Write("Content-Type: application/octet-stream" + newLine + newLine);
                            writer.Flush();
                            memoryStream.Write(file.Data, 0, file.Data.Length);
                            writer.Write(newLine);
                            writer.Write("------" + boundary + "--" + newLine);
                            writer.Flush();
                        }
                    }
                    using (Stream stream = this.Request.GetRequestStream())
                    {
                        memoryStream.WriteTo(stream);

                    }
                }

            }
            using (HttpWebResponse response = (HttpWebResponse)this.Request.GetResponse())
            {
                FireResponseHead(response.Headers);
                return ReadResponse(response);
            }
        }



        private string ReadResponse(HttpWebResponse response)
        {
            Stream strem = null;
            if (response.Headers["Content-Encoding"] == "gzip")
                strem = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
            else
                strem = response.GetResponseStream();


            Encoding encoding = (string.IsNullOrEmpty(response.CharacterSet) ? Encoding.UTF8 : Encoding.GetEncoding(response.CharacterSet));

            using (StreamReader reader = new StreamReader(strem, encoding))
            {
                return reader.ReadToEnd();
            }
        }


    }
    public class HttpUploadingFile
    {

        /// <summary>
        /// 
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        /// <param name="fieldName"></param>
        public HttpUploadingFile(byte[] data, string fileName, string fieldName)
        {
            this.Data = data;
            this.FileName = fileName;
            this.FieldName = fieldName;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public sealed class WebFormData
    {
        private StringBuilder _sb = new StringBuilder();
        private List<string>_list=new List<string>(); 
        private Dictionary<string, string> _parameters=new Dictionary<string, string>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public WebFormData(string key, string value)
        {
            
            this.Add(key, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebFormData Add(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            if (_sb.Length > 0)
                _sb.Append("&");

            _sb.Append(System.Web.HttpUtility.UrlEncode(key)).Append("=").Append(System.Web.HttpUtility.UrlEncode(value ?? string.Empty));
            _list.Add(System.Web.HttpUtility.UrlEncode(key)+"="+System.Web.HttpUtility.UrlEncode(value ?? string.Empty));
            _parameters.Add(key,value);
            return this;
        }

        /// <summary>
        /// 参数首字母排序
        /// </summary>
        public string ParamsData
        {
            get
            {
                _list.Sort();
                string paramstr = string.Empty;
                foreach (var param in _list)
                {
                    string[] arr = param.Split('=');
                    string str = arr[0].ToLower() + "=" + arr[1];
                    paramstr += str + "&";
                }
                return  paramstr.TrimEnd('&');
            }
        }

        /// <summary>
        /// 订单加密
        /// </summary>
        /// <param name="secret"></param>
        /// <returns></returns>
        public string SignRequest(string secret)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(_parameters, StringComparer.Ordinal);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder();
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                //if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                //{
                //    query.Append(key).Append(value);
                //}
                //参数为空，也参与签名
                query.Append(key).Append(value);
            }
            query.Append(secret);

            return query.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _sb.ToString();
        }
    }
    
}
