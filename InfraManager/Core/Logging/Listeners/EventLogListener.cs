using System.Diagnostics;
using System.Xml.Serialization;

namespace InfraManager.Core.Logging.Listeners
{
	[XmlType(ElementName)]
	public sealed class EventLogListener : Listener
	{
		#region fields
		public const string ElementName = "eventLogListener";
		#endregion


		#region properties
		[XmlAttribute("logName")]
		public string LogName { get; set; }
		#endregion


		#region sealed override method Activate
		sealed public override void Activate()
		{
			base.Activate();
			try
			{
				var machineName = ".";
				var eventSource = ApplicationManager.Instance.ApplicationName;
				var logName = this.LogName;
				var sourceAlreadyExists = EventLog.SourceExists(eventSource);
				if (sourceAlreadyExists)
					logName = EventLog.LogNameFromSourceName(eventSource, machineName);
				//
				if (sourceAlreadyExists && logName != this.LogName)
				{
					EventLog.DeleteEventSource(eventSource, machineName);
					EventLog.CreateEventSource(new EventSourceCreationData(eventSource, this.LogName));
				}
				else if (!sourceAlreadyExists)
				{
					EventLog.CreateEventSource(new EventSourceCreationData(eventSource, this.LogName));
				}
			}
			catch
			{
				base.IsSuccessful = false;
			}
		}
		#endregion

		#region sealed override method WriteLogEvent
		sealed public override void WriteLogEvent(LogEvent logEvent)
		{
			if (!base.IsSuccessful)
				return;
			//
			const int maxMessageSize = 31884;
			var message = RenderLogEvent(logEvent);
			if (message.Length > maxMessageSize)
				message = message.Substring(maxMessageSize);
			EventLog.WriteEntry(ApplicationManager.Instance.ApplicationName, message, GetEventLogEntryType(logEvent.SeverityLevel));
		}
		#endregion

		#region private method GetEventLogEntryType
		private EventLogEntryType GetEventLogEntryType(SeverityLevel severityLevel)
		{
			if (severityLevel >= InfraManager.Core.Logging.SeverityLevel.Error)
				return EventLogEntryType.Error;
			else if (severityLevel == InfraManager.Core.Logging.SeverityLevel.Warning)
				return EventLogEntryType.Warning;
			else
				return EventLogEntryType.Information;
		}
		#endregion
	}
}
