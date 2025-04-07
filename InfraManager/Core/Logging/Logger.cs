using System;
using System.IO;
using System.Text;
using System.Xml;
using InfraManager.Core.Extensions;
using InfraManager.Core.Helpers;

namespace InfraManager.Core.Logging
{
	public static class Logger
	{
		#region fields
		private static Logging __logging;
		private static readonly FileSystemWatcher __fileSystemWatcher;
		#endregion


		#region properties
		private static bool VerboseIsEnabled { get { return __logging.VerboseIsEnabled; } }

		private static bool TraceIsEnabled { get { return __logging.TraceIsEnabled; } }

		private static bool DebugIsEnabled { get { return __logging.DebugIsEnabled; } }

		private static bool InfoIsEnabled { get { return __logging.InfoIsEnabled; } }

		private static bool WarningIsEnabled { get { return __logging.WarningIsEnabled; } }

		private static bool ErrorIsEnabled { get { return __logging.ErrorIsEnabled; } }

		private static bool CriticalIsEnabled { get { return __logging.CriticalIsEnabled; } }

		private static bool FatalIsEnabled { get { return __logging.FatalIsEnabled; } }
		#endregion


		#region constructors
		static Logger()
		{
            try
            {
                Reconfigure();
                //
                __fileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(ApplicationManager.Instance.ConfigurationFilePath), Path.GetFileName(ApplicationManager.Instance.ConfigurationFilePath));
                __fileSystemWatcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite;
                __fileSystemWatcher.Created += (sender, e) => Reconfigure();
                __fileSystemWatcher.Changed += (sender, e) => Reconfigure();
                __fileSystemWatcher.EnableRaisingEvents = true;
            }
            catch
            { }
		}
		#endregion


		#region method Verbose
		public static void Verbose(string message)
		{
			if (VerboseIsEnabled)
				Log(SeverityLevel.Verbose, message);
		}

		public static void Verbose(string format, object arg0)
		{
			if (VerboseIsEnabled)
				Log(SeverityLevel.Verbose, string.Format(format, arg0));
		}

		public static void Verbose(string format, object arg0, object arg1)
		{
			if (VerboseIsEnabled)
				Log(SeverityLevel.Verbose, string.Format(format, arg0, arg1));
		}

		public static void Verbose(string format, object arg0, object arg1, object arg2)
		{
			if (VerboseIsEnabled)
				Log(SeverityLevel.Verbose, string.Format(format, arg0, arg1, arg2));
		}

		public static void Verbose(string format, params object[] args)
		{
			if (VerboseIsEnabled)
				Log(SeverityLevel.Verbose, string.Format(format, args));
		}
		#endregion

		#region method Trace
		public static void Trace(string message)
		{
			if (TraceIsEnabled)
				Log(SeverityLevel.Trace, message);
		}

		public static void Trace(string format, object arg0)
		{
			if (TraceIsEnabled)
				Log(SeverityLevel.Trace, string.Format(format, arg0));
		}

		public static void Trace(string format, object arg0, object arg1)
		{
			if (TraceIsEnabled)
				Log(SeverityLevel.Trace, string.Format(format, arg0, arg1));
		}

		public static void Trace(string format, object arg0, object arg1, object arg2)
		{
			if (TraceIsEnabled)
				Log(SeverityLevel.Trace, string.Format(format, arg0, arg1, arg2));
		}

		public static void Trace(string format, params object[] args)
		{
			if (TraceIsEnabled)
				Log(SeverityLevel.Trace, string.Format(format, args));
		}
		#endregion

		#region method Debug
		public static void Debug(string message)
		{
			if (DebugIsEnabled)
				Log(SeverityLevel.Debug, message);
		}

		public static void Debug(string format, object arg0)
		{
			if (DebugIsEnabled)
				Log(SeverityLevel.Debug, string.Format(format, arg0));
		}

		public static void Debug(string format, object arg0, object arg1)
		{
			if (DebugIsEnabled)
				Log(SeverityLevel.Debug, string.Format(format, arg0, arg1));
		}

		public static void Debug(string format, object arg0, object arg1, object arg2)
		{
			if (DebugIsEnabled)
				Log(SeverityLevel.Debug, string.Format(format, arg0, arg1, arg2));
		}

		public static void Debug(string format, params object[] args)
		{
			if (DebugIsEnabled)
				Log(SeverityLevel.Debug, string.Format(format, args));
		}

		public static void Debug(Exception exception)
		{
			if (DebugIsEnabled)
				Log(SeverityLevel.Debug, exception, null);
		}

		public static void Debug(Exception exception, string message)
		{
			if (DebugIsEnabled)
				Log(SeverityLevel.Debug, exception, message);
		}

		public static void Debug(Exception exception, string format, object arg0)
		{
			if (DebugIsEnabled)
				Log(SeverityLevel.Debug, exception, string.Format(format, arg0));
		}

		public static void Debug(Exception exception, string format, object arg0, object arg1)
		{
			if (DebugIsEnabled)
				Log(SeverityLevel.Debug, exception, string.Format(format, arg0, arg1));
		}

		public static void Debug(Exception exception, string format, object arg0, object arg1, object arg2)
		{
			if (DebugIsEnabled)
				Log(SeverityLevel.Debug, exception, string.Format(format, arg0, arg1, arg2));
		}

		public static void Debug(Exception exception, string format, params object[] args)
		{
			if (DebugIsEnabled)
				Log(SeverityLevel.Debug, exception, string.Format(format, args));
		}
		#endregion

		#region method Info
		public static void Info(string message)
		{
			if (InfoIsEnabled)
				Log(SeverityLevel.Info, message);
		}

		public static void Info(string format, object arg0)
		{
			if (InfoIsEnabled)
				Log(SeverityLevel.Info, string.Format(format, arg0));
		}

		public static void Info(string format, object arg0, object arg1)
		{
			if (InfoIsEnabled)
				Log(SeverityLevel.Info, string.Format(format, arg0, arg1));
		}

		public static void Info(string format, object arg0, object arg1, object arg2)
		{
			if (InfoIsEnabled)
				Log(SeverityLevel.Info, string.Format(format, arg0, arg1, arg2));
		}

		public static void Info(string format, params object[] args)
		{
			if (InfoIsEnabled)
				Log(SeverityLevel.Info, string.Format(format, args));
		}

		public static void Info(Exception exception)
		{
			if (InfoIsEnabled)
				Log(SeverityLevel.Info, exception, null);
		}

		public static void Info(Exception exception, string message)
		{
			if (InfoIsEnabled)
				Log(SeverityLevel.Info, exception, message);
		}

		public static void Info(Exception exception, string format, object arg0)
		{
			if (InfoIsEnabled)
				Log(SeverityLevel.Info, exception, string.Format(format, arg0));
		}

		public static void Info(Exception exception, string format, object arg0, object arg1)
		{
			if (InfoIsEnabled)
				Log(SeverityLevel.Info, exception, string.Format(format, arg0, arg1));
		}

		public static void Info(Exception exception, string format, object arg0, object arg1, object arg2)
		{
			if (InfoIsEnabled)
				Log(SeverityLevel.Info, exception, string.Format(format, arg0, arg1, arg2));
		}

		public static void Info(Exception exception, string format, params object[] args)
		{
			if (InfoIsEnabled)
				Log(SeverityLevel.Info, exception, string.Format(format, args));
		}
		#endregion

		#region method Warning
		public static void Warning(string message)
		{
			if (WarningIsEnabled)
				Log(SeverityLevel.Warning, message);
		}

		public static void Warning(string format, object arg0)
		{
			if (WarningIsEnabled)
				Log(SeverityLevel.Warning, string.Format(format, arg0));
		}

		public static void Warning(string format, object arg0, object arg1)
		{
			if (WarningIsEnabled)
				Log(SeverityLevel.Warning, string.Format(format, arg0, arg1));
		}

		public static void Warning(string format, object arg0, object arg1, object arg2)
		{
			if (WarningIsEnabled)
				Log(SeverityLevel.Warning, string.Format(format, arg0, arg1, arg2));
		}

		public static void Warning(string format, params object[] args)
		{
			if (WarningIsEnabled)
				Log(SeverityLevel.Warning, string.Format(format, args));
		}

		public static void Warning(Exception exception)
		{
			if (WarningIsEnabled)
				Log(SeverityLevel.Warning, exception, null);
		}

		public static void Warning(Exception exception, string message)
		{
			if (WarningIsEnabled)
				Log(SeverityLevel.Warning, exception, message);
		}

		public static void Warning(Exception exception, string format, object arg0)
		{
			if (WarningIsEnabled)
				Log(SeverityLevel.Warning, exception, string.Format(format, arg0));
		}

		public static void Warning(Exception exception, string format, object arg0, object arg1)
		{
			if (WarningIsEnabled)
				Log(SeverityLevel.Warning, exception, string.Format(format, arg0, arg1));
		}

		public static void Warning(Exception exception, string format, object arg0, object arg1, object arg2)
		{
			if (WarningIsEnabled)
				Log(SeverityLevel.Warning, exception, string.Format(format, arg0, arg1, arg2));
		}

		public static void Warning(Exception exception, string format, params object[] args)
		{
			if (WarningIsEnabled)
				Log(SeverityLevel.Warning, exception, string.Format(format, args));
		}
		#endregion

		#region method Error
		public static void Error(string message)
		{
			if (ErrorIsEnabled)
				Log(SeverityLevel.Error, message);
		}

		public static void Error(string format, object arg0)
		{
			if (ErrorIsEnabled)
				Log(SeverityLevel.Error, string.Format(format, arg0));
		}

		public static void Error(string format, object arg0, object arg1)
		{
			if (ErrorIsEnabled)
				Log(SeverityLevel.Error, string.Format(format, arg0, arg1));
		}

		public static void Error(string format, object arg0, object arg1, object arg2)
		{
			if (ErrorIsEnabled)
				Log(SeverityLevel.Error, string.Format(format, arg0, arg1, arg2));
		}

		public static void Error(string format, params object[] args)
		{
			if (ErrorIsEnabled)
				Log(SeverityLevel.Error, string.Format(format, args));
		}

		public static void Error(Exception exception)
		{
			if (ErrorIsEnabled)
				Log(SeverityLevel.Error, exception, null);
		}

		public static void Error(Exception exception, string message)
		{
			if (ErrorIsEnabled)
				Log(SeverityLevel.Error, exception, message);
		}

		public static void Error(Exception exception, string format, object arg0)
		{
			if (ErrorIsEnabled)
				Log(SeverityLevel.Error, exception, string.Format(format, arg0));
		}

		public static void Error(Exception exception, string format, object arg0, object arg1)
		{
			if (ErrorIsEnabled)
				Log(SeverityLevel.Error, exception, string.Format(format, arg0, arg1));
		}

		public static void Error(Exception exception, string format, object arg0, object arg1, object arg2)
		{
			if (ErrorIsEnabled)
				Log(SeverityLevel.Error, exception, string.Format(format, arg0, arg1, arg2));
		}

		public static void Error(Exception exception, string format, params object[] args)
		{
			if (ErrorIsEnabled)
				Log(SeverityLevel.Error, exception, string.Format(format, args));
		}
		#endregion

		#region method Critical
		public static void Critical(string message)
		{
			if (CriticalIsEnabled)
				Log(SeverityLevel.Critical, message);
		}

		public static void Critical(string format, object arg0)
		{
			if (CriticalIsEnabled)
				Log(SeverityLevel.Critical, string.Format(format, arg0));
		}

		public static void Critical(string format, object arg0, object arg1)
		{
			if (CriticalIsEnabled)
				Log(SeverityLevel.Critical, string.Format(format, arg0, arg1));
		}

		public static void Critical(string format, object arg0, object arg1, object arg2)
		{
			if (CriticalIsEnabled)
				Log(SeverityLevel.Critical, string.Format(format, arg0, arg1, arg2));
		}

		public static void Critical(string format, params object[] args)
		{
			if (CriticalIsEnabled)
				Log(SeverityLevel.Critical, string.Format(format, args));
		}

		public static void Critical(Exception exception)
		{
			if (CriticalIsEnabled)
				Log(SeverityLevel.Critical, exception, null);
		}

		public static void Critical(Exception exception, string message)
		{
			if (CriticalIsEnabled)
				Log(SeverityLevel.Critical, exception, message);
		}

		public static void Critical(Exception exception, string format, object arg0)
		{
			if (CriticalIsEnabled)
				Log(SeverityLevel.Critical, exception, string.Format(format, arg0));
		}

		public static void Critical(Exception exception, string format, object arg0, object arg1)
		{
			if (CriticalIsEnabled)
				Log(SeverityLevel.Critical, exception, string.Format(format, arg0, arg1));
		}

		public static void Critical(Exception exception, string format, object arg0, object arg1, object arg2)
		{
			if (CriticalIsEnabled)
				Log(SeverityLevel.Critical, exception, string.Format(format, arg0, arg1, arg2));
		}

		public static void Critical(Exception exception, string format, params object[] args)
		{
			if (CriticalIsEnabled)
				Log(SeverityLevel.Critical, exception, string.Format(format, args));
		}
		#endregion

		#region method Fatal
		public static void Fatal(string message)
		{
			if (FatalIsEnabled)
				Log(SeverityLevel.Fatal, message);
		}

		public static void Fatal(string format, object arg0)
		{
			if (FatalIsEnabled)
				Log(SeverityLevel.Fatal, string.Format(format, arg0));
		}

		public static void Fatal(string format, object arg0, object arg1)
		{
			if (FatalIsEnabled)
				Log(SeverityLevel.Fatal, string.Format(format, arg0, arg1));
		}

		public static void Fatal(string format, object arg0, object arg1, object arg2)
		{
			if (FatalIsEnabled)
				Log(SeverityLevel.Fatal, string.Format(format, arg0, arg1, arg2));
		}

		public static void Fatal(string format, params object[] args)
		{
			if (FatalIsEnabled)
				Log(SeverityLevel.Fatal, string.Format(format, args));
		}

		public static void Fatal(Exception exception)
		{
			if (FatalIsEnabled)
				Log(SeverityLevel.Fatal, exception, null);
		}

		public static void Fatal(Exception exception, string message)
		{
			if (FatalIsEnabled)
				Log(SeverityLevel.Fatal, exception, message);
		}

		public static void Fatal(Exception exception, string format, object arg0)
		{
			if (FatalIsEnabled)
				Log(SeverityLevel.Fatal, exception, string.Format(format, arg0));
		}

		public static void Fatal(Exception exception, string format, object arg0, object arg1)
		{
			if (FatalIsEnabled)
				Log(SeverityLevel.Fatal, exception, string.Format(format, arg0, arg1));
		}

		public static void Fatal(Exception exception, string format, object arg0, object arg1, object arg2)
		{
			if (FatalIsEnabled)
				Log(SeverityLevel.Fatal, exception, string.Format(format, arg0, arg1, arg2));
		}

		public static void Fatal(Exception exception, string format, params object[] args)
		{
			if (FatalIsEnabled)
				Log(SeverityLevel.Fatal, exception, string.Format(format, args));
		}
		#endregion

		#region private method Reconfigure
		private static void Reconfigure()
		{
			try
			{
				var xmlDocument = new XmlDocument().Do(x => x.Load(ApplicationManager.Instance.ConfigurationFilePath));
				var loggingElement = xmlDocument.DocumentElement.SelectSingleNode(Logging.ElementName);
				var stringBuilder = new StringBuilder();
				var xmlWriter = XmlWriter.Create(stringBuilder, new XmlWriterSettings { Encoding = new UTF8Encoding(false) });
				xmlWriter.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
				xmlWriter.WriteRaw(loggingElement.OuterXml);
				xmlWriter.Flush();
				__logging = XmlHelper.XmlString2Object<Logging>(stringBuilder.ToString());
			}
			catch
			{
				__logging = new Logging();
			}
			__logging.Activate();
		}
		#endregion

		#region private method Log
		private static void Log(SeverityLevel severityLevel, string message)
		{
			Log(severityLevel, null, message);
		}

		private static void Log(SeverityLevel severityLevel, Exception exception, string message)
		{
			if (__logging.Listeners.Length == 0)
				return;
			var logEvent = new LogEvent(severityLevel, exception, message);
			foreach (var listener in __logging.Listeners)
				listener.WriteLogEvent(logEvent);
		}
		#endregion
	}
}
