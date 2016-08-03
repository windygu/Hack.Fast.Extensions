﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Mysoft.Map.MVC
{
	/// <summary>
	/// 以流的形式返回
	/// </summary>
	public sealed class StreamResult : IActionResult
	{
		private byte[] _buffer;
		private string _contentType;
		private string _filename;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer"></param>
		public StreamResult(byte[] buffer) : this(buffer, null)
		{
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="contentType"></param>
		public StreamResult(byte[] buffer, string contentType): this(buffer, contentType, null)
		{
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="contentType"></param>
		/// <param name="filename"></param>
		public StreamResult(byte[] buffer, string contentType, string filename)
		{
			if( buffer == null || buffer.Length == 0 )
				throw new ArgumentNullException("buffer");

			_buffer = buffer;
			_filename = filename;


			if( string.IsNullOrEmpty(contentType) )
				_contentType = "application/octet-stream";
			else
				_contentType = contentType;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		public void Ouput(HttpContext context)
		{
			context.Response.ContentType = _contentType;

			if( string.IsNullOrEmpty(_filename) == false ) {
				string downloadName = _filename;

				if( context.Request.Browser.Browser == "IE" )
					downloadName = HttpUtility.UrlPathEncode(downloadName);

				string headerValue = string.Format("attachment; filename=\"{0}\"", downloadName);
				context.Response.AddHeader("Content-Disposition", headerValue);
			}

			context.Response.OutputStream.Write(_buffer, 0, _buffer.Length);
		}

	}
}
