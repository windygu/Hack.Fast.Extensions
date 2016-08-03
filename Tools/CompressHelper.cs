﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.IO.Compression;

namespace Mysoft.Map.Tools
{
	/// <summary>
	/// 提供加密和解密的工具类
	/// </summary>
	public static class CompressHelper
	{
		private delegate void ZipCompress(Stream inStream, Stream outStream, bool isStreamOwner, int level);
		private delegate void ZipDecompress(Stream inStream, Stream outStream, bool isStreamOwner);
		
		private static readonly ZipCompress s_ZipCompressAction;
		private static readonly ZipDecompress s_ZipDecompressAction;

		static CompressHelper()
		{
			// 创建使用SharpZipLib的二个委托。
			Type t = Type.GetType("ICSharpCode.SharpZipLib.BZip2.BZip2, ICSharpCode.SharpZipLib", false);
			if( t != null ) {
				MethodInfo m1 = t.GetMethod("Compress", BindingFlags.Static | BindingFlags.Public, null,
					new Type[] { typeof(Stream), typeof(Stream), typeof(bool), typeof(int) }, null);

				MethodInfo m2 = t.GetMethod("Decompress", BindingFlags.Static | BindingFlags.Public, null,
					new Type[] { typeof(Stream), typeof(Stream), typeof(bool) }, null);

				if( m1 != null && m2 != null ) {
					s_ZipCompressAction = Delegate.CreateDelegate(typeof(ZipCompress), m1) as ZipCompress;
					s_ZipDecompressAction = Delegate.CreateDelegate(typeof(ZipDecompress), m2) as ZipDecompress;
				}
			}
		}


		/// <summary>
		/// 压缩字节数组，如果ICSharpCode.SharpZipLib存在，则使用zip算法压缩，否则使用Gzip算法压缩。
		/// </summary>
		/// <param name="input">要压缩的字节数组</param>
		/// <returns>压缩后的结果</returns>
		public static byte[] CompressBytes(byte[] input)
		{
			if( s_ZipCompressAction == null )
				return GzipCompress(input);
			else
				return SharpZipCompress(input);
		}

		/// <summary>
		/// 解压缩字节数组，如果ICSharpCode.SharpZipLib存在，则使用zip算法解压缩，否则使用Gzip算法解压缩。
		/// </summary>
		/// <param name="input">要解压的字节数组</param>
		/// <returns>解压后的结果</returns>
		public static byte[] DecompressBytes(byte[] input)
		{
			if( s_ZipDecompressAction == null )
				return GzipDecompress(input);
			else
				return SharpZipDecompress(input);
		}

		/// <summary>
		/// 使用Gzip压缩算法压缩字节数组
		/// </summary>
		/// <param name="input">要压缩的字节数组</param>
		/// <returns>压缩结果</returns>
		public static byte[] GzipCompress(byte[] input)
		{
			if( input == null )
				throw new ArgumentNullException("input");


			using( MemoryStream sourceStream = new MemoryStream(input) ) {
				using( MemoryStream resultStream = new MemoryStream() ) {
					using( GZipStream gZipStream = new GZipStream(resultStream, CompressionMode.Compress, true) ) {

						byte[] buffer = new byte[1024 * 16]; //缓冲区大小

						int sourceBytes = sourceStream.Read(buffer, 0, buffer.Length);
						while( sourceBytes > 0 ) {
							gZipStream.Write(buffer, 0, sourceBytes);
							sourceBytes = sourceStream.Read(buffer, 0, buffer.Length);
						}
						//gZipStream.Flush();
						gZipStream.Close();

						resultStream.Position = 0;
						return resultStream.ToArray();
					}
				}
			}
		}

		/// <summary>
		/// 使用Gzip算法解压缩字节数组
		/// </summary>
		/// <param name="input">要解压的字节数组</param>
		/// <returns>解压后的结果</returns>
		public static byte[] GzipDecompress(byte[] input)
		{
			if( input == null )
				throw new ArgumentNullException("input");


			using( MemoryStream sourceStream = new MemoryStream(input) ) {
				using( GZipStream gZipStream = new GZipStream(sourceStream, CompressionMode.Decompress, true) ) {
					using( MemoryStream resultStream = new MemoryStream() ) {
						byte[] buffer = new byte[1024 * 16]; //缓冲区大小
						int sourceBytes = gZipStream.Read(buffer, 0, buffer.Length);
						while( sourceBytes > 0 ) {
							resultStream.Write(buffer, 0, sourceBytes);
							sourceBytes = gZipStream.Read(buffer, 0, buffer.Length);
						}
						resultStream.Position = 0;
						return resultStream.ToArray();
					}
				}
			}
		}


		private static byte[] SharpZipCompress(byte[] input)
		{
			if( input == null )
				throw new ArgumentNullException("input");


			using( MemoryStream sourceStream = new MemoryStream(input) ) {
				using( MemoryStream resultStream = new MemoryStream() ) {
					s_ZipCompressAction(sourceStream, resultStream, false, 9);
					resultStream.Position = 0;
					return resultStream.ToArray();
				}
			}
		}

		private static byte[] SharpZipDecompress(byte[] input)
		{
			if( input == null )
				throw new ArgumentNullException("input");


			using( MemoryStream sourceStream = new MemoryStream(input) ) {
				using( MemoryStream resultStream = new MemoryStream() ) {
					s_ZipDecompressAction(sourceStream, resultStream, false);
					resultStream.Position = 0;
					return resultStream.ToArray();
				}
			}
		}




	}
}
