using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;
using System.Collections.Specialized;
using Mysoft.Map.MVC.Serializer;
using Hack.Fast.Extensions;

namespace Mysoft.Map.MVC
{
	internal static class ActionExecutor
	{
		/// <summary>
		/// MyMVC的版本。（dll文件版本）
		/// </summary>
		private static readonly string MvcVersion
			= System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(ActionExecutor).Assembly.Location).FileVersion;


		private static void SetMvcVersionHeader(HttpContext context)
		{
			context.Response.AppendHeader("X-MysoftMVC", MvcVersion);
		}

		internal static void ExecuteAction(HttpContext context, InvokeInfo vkInfo)
		{
			if( context == null )
				throw new ArgumentNullException("context");
			if( vkInfo == null )
				throw new ArgumentNullException("vkInfo");

			SetMvcVersionHeader(context);

			// 验证请求是否允许访问（身份验证）
			AuthorizeAttribute authorize = vkInfo.GetAuthorize();
			if( authorize != null ) {
				if( authorize.AuthenticateRequest(context) == false )
					ExceptionHelper.Throw403Exception(context);
			}

			// ASP.NET的安全检查
			if( vkInfo.Action.Attr.NeedValidateRequest() )
				context.Request.ValidateInput();

			// 调用方法
			object result = ExecuteActionInternal(context, vkInfo);

			// 设置OutputCache
			OutputCacheAttribute outputCache = vkInfo.GetOutputCacheSetting();
			if( outputCache != null )
				outputCache.SetResponseCache(context);

			// 处理方法的返回结果
			IActionResult executeResult = result as IActionResult;
			if( executeResult != null ) {
				executeResult.Ouput(context);
			}
			else {
				if( result != null ) {
					// 普通类型结果
					context.Response.ContentType = "text/plain";
					context.Response.Write(result.ToString());
				}
			}
		}

		internal static void OutputActionResult(HttpContext context, object result)
		{
			IActionResult executeResult = result as IActionResult;
			if( executeResult != null ) {
				executeResult.Ouput(context);
			}
			else {
				if( result != null ) {
					// 普通类型结果
					context.Response.ContentType = "text/plain";
					context.Response.Write(result.ToString());
				}
			}
		}

		internal static object ExecuteActionInternal(HttpContext context, InvokeInfo info)
		{
			// 准备要传给调用方法的参数
			object[] parameters = GetActionCallParameters(context, info.Action);

			// 调用方法
			if( info.Action.HasReturn )
				//return info.Action.MethodInfo.Invoke(info.Instance, parameters);
				return info.Action.MethodInfo.FastInvoke(info.Instance, parameters);

			else {
				//info.Action.MethodInfo.Invoke(info.Instance, parameters);
				info.Action.MethodInfo.FastInvoke(info.Instance, parameters);
				return null;
			}
		}


		private static object[] GetActionCallParameters(HttpContext context, ActionDescription action)
		{
			if( action.Parameters == null || action.Parameters.Length == 0 )
				return null;

			IActionParametersProvider provider = ActionParametersProviderFactory.CreateActionParametersProvider(context);
			return provider.GetParameters(context, action);
		}

	}
}
