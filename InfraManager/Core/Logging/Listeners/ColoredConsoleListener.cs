using System;
using System.Xml.Serialization;

namespace InfraManager.Core.Logging.Listeners
{
	[XmlType(ElementName)]
	public sealed class ColoredConsoleListener : Listener
	{
		#region fields
		public const string ElementName = "coloredConsoleListener";
		#endregion


		#region override method WriteLogEvent
		sealed public override void WriteLogEvent(LogEvent logEvent)
		{
			var consoleColor = Console.ForegroundColor;
			if (logEvent.SeverityLevel == InfraManager.Core.Logging.SeverityLevel.Fatal ||
				logEvent.SeverityLevel == InfraManager.Core.Logging.SeverityLevel.Critical ||
				logEvent.SeverityLevel == InfraManager.Core.Logging.SeverityLevel.Error)
				Console.ForegroundColor = ConsoleColor.Red;
			else if (logEvent.SeverityLevel == InfraManager.Core.Logging.SeverityLevel.Warning)
				Console.ForegroundColor = ConsoleColor.Yellow;
			else if (logEvent.SeverityLevel == InfraManager.Core.Logging.SeverityLevel.Info)
				Console.ForegroundColor = ConsoleColor.Green;
			else if (logEvent.SeverityLevel == InfraManager.Core.Logging.SeverityLevel.Debug)
				Console.ForegroundColor = ConsoleColor.Cyan;
			else
				Console.ForegroundColor = ConsoleColor.White;
			Console.Out.WriteLine(RenderLogEvent(logEvent));
			Console.ForegroundColor = consoleColor;
		}
		#endregion
	}
}
