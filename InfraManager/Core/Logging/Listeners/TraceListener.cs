using System.Diagnostics;
using System.Xml.Serialization;

namespace InfraManager.Core.Logging.Listeners
{
	[XmlType(ElementName)]
	public sealed class TraceListener : Listener
	{
		#region fields
		public const string ElementName = "traceListener";
		#endregion


		#region override method WriteLogEvent
		sealed public override void WriteLogEvent(LogEvent logEvent)
		{
			Trace.Write(RenderLogEvent(logEvent), logEvent.LoggerName);
			Trace.Flush();
		}
		#endregion
	}
}
