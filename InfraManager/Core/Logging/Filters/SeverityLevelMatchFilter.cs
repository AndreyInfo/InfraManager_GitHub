using System.Diagnostics;
using System.Xml.Serialization;

namespace InfraManager.Core.Logging.Filters
{
	[XmlType(ElementName)]
	public sealed class SeverityLevelMatchFilter : Filter
	{
		#region fields
		public const string ElementName = "severityLevelMatch";
		private SeverityLevel _severityLevel;
		#endregion


		#region properties
		[XmlAttribute("severityLevel")]
		public string SeverityLevel { get; set; }
		#endregion


		#region interface IActivatable
		public override void Activate()
		{
			base.Activate();
			_severityLevel = InfraManager.Core.Logging.SeverityLevel.GetSeverityLevel(this.SeverityLevel);
		}
		#endregion


		#region internal override method Filtrate
		internal override FilterResult Filtrate(LogEvent logEvent)
		{
			#region assertions
			Debug.Assert(logEvent != null, "logEvent is null.");
			#endregion
			//
			if (_severityLevel != null && _severityLevel == logEvent.SeverityLevel)
				return this.AcceptOnMatch ? FilterResult.Accept : FilterResult.Reject;
			return FilterResult.Ignore;
		}
		#endregion
	}
}
