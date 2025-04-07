using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using InfraManager.Core.Extensions;

namespace InfraManager.Core.Logging.Formatters
{
	[XmlType(ElementName)]
	public sealed class TemplatedFormatter : Formatter
	{
		#region fields
		public const string ElementName = "templatedFormatter";
		#endregion


		#region properties
		[XmlElement("template")]
		public string Template { get; set; }
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
			if (!this.Template.IsNullOrEmpty())
				writer.Write(this.Template.ObjectFormat(logEvent));
			FormatFooter(writer, logEvent);
		}
		#endregion
	}
}
