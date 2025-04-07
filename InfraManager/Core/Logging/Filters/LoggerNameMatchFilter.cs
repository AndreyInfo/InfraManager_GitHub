using System.Diagnostics;
using System.Xml.Serialization;
using InfraManager.Core.Extensions;

namespace InfraManager.Core.Logging.Filters
{
	[XmlType(ElementName)]
	public sealed class LoggerNameMatchFilter : Filter
	{
		#region fields
		public const string ElementName = "loggerNameMatch";
		#endregion


		#region properties
		[XmlAttribute("loggerName")]
		public string LoggerName { get; set; }
		#endregion


		#region internal override method Filtrate
		internal override FilterResult Filtrate(LogEvent logEvent)
		{
			#region assertions
			Debug.Assert(logEvent != null, "logEvent is null.");
			#endregion
			//
			if (!this.LoggerName.IsNullOrEmpty() && logEvent.LoggerName == this.LoggerName)
				return this.AcceptOnMatch ? FilterResult.Accept : FilterResult.Reject;
			return FilterResult.Ignore;
		}
		#endregion
	}
}
