using System;
using InfraManager.Core.Helpers;
using InfraManager.Core.Extensions;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace InfraManager.Core.Logging
{
	[Serializable]
	public sealed class LogEvent
	{
		#region fields
		private static readonly HashSet<string> __ignorablePropertyNames =
			new HashSet<string>() 
			{ 
				"Data",
				"HelpLink",
				"InnerException",
				"Message",
				"Source",
				"StackTrace",
				"TargetSite",
			};
		private static readonly Dictionary<Type, PropertyInfo[]> __propertyInfosDictionary = new Dictionary<Type, PropertyInfo[]>();
		#endregion


		#region properties
		public string LoggerName { get; private set; }

		public DateTime UtcTimeStamp { get; private set; }

		public DateTime TimeStamp { get; private set; }

		public SeverityLevel SeverityLevel { get; private set; }

		public Exception Exception { get; private set; }

		public string Message { get; private set; }

		public string MachineName { get; private set; }

		public string OperatingSystem { get; private set; }

		public string ProcessName { get; private set; }

		public DateTime ProcessStartTime { get; private set; }

		public string ApplicationName { get; private set; }

		public string ApplicationVersion { get; private set; }

		public int CurrentThreadID { get; private set; }

		public string UserLogin { get; private set; }

		public string NewLine { get { return Environment.NewLine; } }
		#endregion


		#region constructors
		public LogEvent(SeverityLevel severityLevel, string message) :
			this(null, severityLevel, null, message)
		{ }

		public LogEvent(string loggerName, SeverityLevel severityLevel, string message) :
			this(loggerName, severityLevel, null, message)
		{ }

		public LogEvent(SeverityLevel severityLevel, Exception exception, string message) :
			this(null, severityLevel, exception, message)
		{ }

		public LogEvent(string loggerName, SeverityLevel severityLevel, Exception exception, string message)
		{
			if (severityLevel == null)
				throw new ArgumentNullException("severityLevel", "severityLevel is null.");
			//
			this.LoggerName = loggerName;
			this.UtcTimeStamp = DateTime.UtcNow;
			this.TimeStamp = DateTime.Now;
			this.SeverityLevel = severityLevel;
			this.Exception = exception;
			this.Message = message;
			this.MachineName = ApplicationManager.Instance.MachineName;
			this.OperatingSystem = ApplicationManager.Instance.OperatingSystem;
			this.ProcessName = ApplicationManager.Instance.ProcessName;
			this.ProcessStartTime = ApplicationManager.Instance.ProcessStartTime;
			this.ApplicationName = ApplicationManager.Instance.ApplicationName;
			this.ApplicationVersion = ApplicationManager.Instance.ApplicationVersion;
			this.CurrentThreadID = ApplicationManager.Instance.CurrentThreadID;
			this.UserLogin = ApplicationManager.Instance.UserLogin;
		}
		#endregion


		#region method GetException
		public string GetException()
		{
			if (this.Exception == null)
			{
				return string.Empty;
			}
			else
			{
				var stringBuilder = new StringBuilder();
				RenderException(this.Exception, stringBuilder);
				return stringBuilder.ToString();
			}
		}
		#endregion

		#region method GetProperty
		public string GetProperty(string propertyName)
		{
			switch (propertyName.ToLower())
			{
				case "loggername":
					return this.LoggerName;
				case "utctimestamp":
					return this.UtcTimeStamp.ToString("yyyy.MM.dd HH:mm:ss.ffff");
				case "timestamp":
					return this.TimeStamp.ToString("yyyy.MM.dd HH:mm:ss.ffff");
				case "severitylevel":
					return this.SeverityLevel.DisplayName;
				case "exception":
					return this.Exception == null ? null : this.Exception.GetType().ToString();
				case "message":
					return this.Message;
				case "machinename":
					return this.MachineName;
				case "operatingsystem":
					return this.OperatingSystem;
				case "processname":
					return this.ProcessName;
				case "applicationname":
					return this.ApplicationName;
				case "applicationversion":
					return this.ApplicationVersion;
				case "userlogin":
					return this.UserLogin;
			}
			return null;
		}
		#endregion

		#region private method RenderException
		private void RenderException(Exception exception, StringBuilder stringBuilder)
		{
			#region assertios
			Debug.Assert(exception != null, "exception is null.");
			Debug.Assert(stringBuilder != null, "stringBuilder is null.");
			#endregion
			//
			var exceptionType = exception.GetType();
			stringBuilder.Append("Type: ");
			stringBuilder.AppendLine(exceptionType.ToString());
			stringBuilder.Append("Message: ");
			stringBuilder.AppendLine(exception.Message ?? string.Empty);
			PropertyInfo[] propertyInfos;
			if (!__propertyInfosDictionary.TryGetValue(exceptionType, out propertyInfos))
				__propertyInfosDictionary[exceptionType] = propertyInfos = exceptionType.GetProperties().Where(x => !__ignorablePropertyNames.Contains(x.Name)).ToArray();
			foreach (var propertyInfo in propertyInfos)
			{
				stringBuilder.Append(propertyInfo.Name);
				stringBuilder.Append(": ");
				try
				{
					stringBuilder.AppendLine((propertyInfo.GetValue(exception, null) ?? string.Empty).ToString());
				}
				catch
				{
					stringBuilder.AppendLine("[obtaining failed]");
				}
			}
			stringBuilder.Append("Target assembly: ");
			try
			{
				var assembly = exception.TargetSite.Maybe(x => x.DeclaringType).Maybe(x => x.Assembly);
				stringBuilder.AppendLine(assembly == null ? string.Empty : string.Concat(assembly.FullName, ", ", assembly.Location));
			}
			catch
			{
				stringBuilder.AppendLine("[obtaining failed]");
			}
			stringBuilder.Append("Target site: ");
			try
			{
				var targetSite = exception.TargetSite;
				stringBuilder.AppendLine(targetSite == null || targetSite.DeclaringType == null ? string.Empty : string.Concat(targetSite.DeclaringType.FullName, ".", targetSite.Name));
			}
			catch
			{
				stringBuilder.AppendLine("[obtaining failed]");
			}
			stringBuilder.AppendLine("Stack trace: ");
			try
			{
				stringBuilder.AppendLine(exception.StackTrace ?? string.Empty);
			}
			catch
			{
				stringBuilder.AppendLine("[obtaining failed]");
			}
			if (exception.InnerException != null)
			{
				stringBuilder.AppendLine("Inner exception:");
				RenderException(exception.InnerException, stringBuilder);
			}
		}
		#endregion
	}
}