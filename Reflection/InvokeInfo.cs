﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Mysoft.Map.MVC
{

	internal sealed class InvokeInfo
	{
		public ControllerDescription Controller;
		public ActionDescription Action;
		public object Instance;
		public Match RegexMatch;

		public OutputCacheAttribute GetOutputCacheSetting()
		{
			if( this.Action != null && this.Action.OutputCache != null )
				return this.Action.OutputCache;
			if( this.Controller != null && this.Controller.OutputCache != null )
				return this.Controller.OutputCache;			
			return null;
		}
		public SessionMode GetSessionMode()
		{
			if( this.Action != null && this.Action.SessionMode != null )
				return this.Action.SessionMode.SessionMode;
			if( this.Controller != null && this.Controller.SessionMode != null )
				return this.Controller.SessionMode.SessionMode;			
			return SessionMode.NotSupport;
		}
		public AuthorizeAttribute GetAuthorize()
		{
			if( this.Action != null && this.Action.Authorize != null )
				return this.Action.Authorize;
			if( this.Controller != null && this.Controller.Authorize != null )
				return this.Controller.Authorize;
			return null;
		}
	}


	
}
