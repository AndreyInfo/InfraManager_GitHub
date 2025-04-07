using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace InfraManager.Core.Logging.Formatters
{
	[XmlType(ElementName)]
	public sealed class MessageFormatter : Formatter
	{
		#region fields
		public const string ElementName = "messageFormatter";
		#endregion


		#region internal override method Format
		internal override void Format(TextWriter writer, LogEvent logEvent)
		{
			#region assertions
			Debug.Assert(writer != null, "writer is null.");
			Debug.Assert(logEvent != null, "logEvent is null.");
			#endregion
			//
			FormatHeader(writer, logEvent);
			if (logEvent.Message != null)
			{
				writer.Write("Message: ");
				writer.WriteLine(logEvent.Message);
			}
			FormatFooter(writer, logEvent);
		}
		#endregion
	}
}
