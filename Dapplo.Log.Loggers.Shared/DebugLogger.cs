﻿//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2016 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.Log.Facade
// 
//  Dapplo.Log.Facade is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.Log.Facade is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.Log.Facade. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#define DEBUG

#region using

#if _XAMARIN_
using System;
#endif

using System.Diagnostics;

#endregion

namespace Dapplo.Log.Facade.Loggers
{
	/// <summary>
	///     A debug logger, the simplest implementation for logging debug messages
	/// </summary>
	public class DebugLogger : AbstractLogger
	{
		/// <summary>
		/// Write a message with parameters to Debug
		/// </summary>
		/// <param name="logInfo">LogInfo</param>
		/// <param name="messageTemplate">string with the message template</param>
		/// <param name="logParameters">object array with the parameters for the template</param>
		public override void Write(LogInfo logInfo, string messageTemplate, params object[] logParameters)
		{
#if _XAMARIN_
			messageTemplate =  messageTemplate.TrimEnd(Environment.NewLine.ToCharArray());
			Debug.WriteLine(Format(logInfo, messageTemplate, logParameters));
#else
			Debug.Write(Format(logInfo, messageTemplate, logParameters));
#endif
		}
	}
}