using System.Diagnostics;
using System.Xml.Serialization;

namespace InfraManager.Core.Logging.Listeners
{
	[XmlType(ElementName)]
	public sealed class DebugListener : Listener
	{
		#region fields
		public const string ElementName = "debugListener";
		#endregion


		#region override method WriteLogEvent
		sealed public override void WriteLogEvent(LogEvent logEvent)
		{
			Debug.Write(RenderLogEvent(logEvent), logEvent.LoggerName);
			Debug.Flush();
		}
		#endregion
	}
}
