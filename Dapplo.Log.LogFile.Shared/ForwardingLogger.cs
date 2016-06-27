﻿//  Dapplo - building blocks for .NET applications
//  Copyright (C) 2015-2016 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.Log
// 
//  Dapplo.Log is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.Log is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.Log. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#region Usings

using System;
using System.Collections.Concurrent;
using Dapplo.Log.Facade;

#if !_PCL_
using System.Diagnostics;
#endif

#endregion

namespace Dapplo.Log.LogFile
{
	/// <summary>
	/// This implements a logger which accepts log items, but only stores them and later forwards them to the replacement logger
	/// Can be used to setup before everything is taken care of, to later replace with the file logger.
	/// When disposed, everything in the queue is written to Trace.
	/// </summary>
    public class ForwardingLogger : AbstractLogger, IDisposable
	{
		private readonly ConcurrentQueue<Tuple<LogInfo, string, object[]>> _logItems = new ConcurrentQueue<Tuple<LogInfo, string, object[]>>();

		/// <summary>
		/// Enqueue the current information so it can be written to the file, formatting is done later.. (improves performance for the UI)
		/// Preferably do NOT pass huge objects which need to be garbage collected
		/// </summary>
		/// <param name="logInfo">LogInfo</param>
		/// <param name="messageTemplate">string</param>
		/// <param name="logParameters">params</param>
		public override void Write(LogInfo logInfo, string messageTemplate, params object[] logParameters)
		{
			_logItems.Enqueue(new Tuple<LogInfo, string, object[]>(logInfo, messageTemplate, logParameters));
		}

		/// <summary>
		/// Pass all the written items on to the next logger 
		/// </summary>
		/// <param name="newLogger">ILogger which replaces this</param>
		public override void ReplacedWith(ILogger newLogger)
		{
			Tuple<LogInfo, string, object[]> logItem;
			// Loop as long as there are items available
			while (_logItems.TryDequeue(out logItem))
			{
				newLogger.Write(logItem.Item1, logItem.Item2, logItem.Item3);
			}
		}

		#region IDisposable Support
		private bool _disposedValue; // To detect redundant calls

		private void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					Tuple<LogInfo, string, object[]> logItem;
					// Loop as long as there are items available
					while (_logItems.TryDequeue(out logItem))
					{
#if !_PCL_
						try
						{
							Trace.Write(Format(logItem.Item1, logItem.Item2, logItem.Item3));
						}
						catch (Exception ex)
						{
							Trace.WriteLine($"Couldn't format passed log information, maybe this was owned by the UI? {ex.Message}");
							Trace.WriteLine($"LogInfo and messagetemplate for the problematic log information: {logItem.Item1} {logItem.Item2}");
						}
#endif
					}
				}

				_disposedValue = true;
			}
		}

		// This code added to correctly implement the disposable pattern.
		void IDisposable.Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
		}
#endregion

	}
}
