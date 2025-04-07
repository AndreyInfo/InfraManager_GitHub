using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace InfraManager.Core.Logging.Formatters
{
	[XmlType(ElementName)]
	public sealed class ExceptionFormatter : Formatter
	{
		#region fields
		public const string ElementName = "exceptionFormatter";
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
			if (logEvent.Exception != null)
				writer.WriteLine(logEvent.GetException());
			FormatFooter(writer, logEvent);
		}
		#endregion
	}
}
