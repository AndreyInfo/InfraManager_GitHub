using System;
using System.Xml.Serialization;

namespace InfraManager.Core.Logging.Listeners
{
	[XmlType(ElementName)]
	public sealed class ConsoleListener : Listener
	{
		#region fields
		public const string ElementName = "consoleListener";
		#endregion


		#region properties
		[XmlAttribute("writeToStdOut")]
		public bool WriteToStdOut { get; set; }

		[XmlAttribute("writeToStdError")]
		public bool WriteToStdError { get; set; }
		#endregion


		#region override method WriteLogEvent
		sealed public override void WriteLogEvent(LogEvent logEvent)
		{
			if (this.WriteToStdOut)
				Console.Out.WriteLine(RenderLogEvent(logEvent));
			if (this.WriteToStdError)
				Console.Error.WriteLine(RenderLogEvent(logEvent));
		}
		#endregion
	}
}
