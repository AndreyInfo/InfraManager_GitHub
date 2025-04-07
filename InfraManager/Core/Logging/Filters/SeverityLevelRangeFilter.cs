using System.Diagnostics;
using System.Xml.Serialization;

namespace InfraManager.Core.Logging.Filters
{
	[XmlType(ElementName)]
	public sealed class SeverityLevelRangeFilter : Filter
	{
		#region fields
		public const string ElementName = "severityLevelRange";
		private SeverityLevel _minSeverityLevel;
		private SeverityLevel _maxSeverityLevel;
		#endregion


		#region properties
		[XmlAttribute("minSeverityLevel")]
		public string MinSeverityLevel { get; set; }

		[XmlAttribute("maxSeverityLevel")]
		public string MaxSeverityLevel { get; set; }
		#endregion


		#region interface IActivatable
		public override void Activate()
		{
			base.Activate();
			_minSeverityLevel = SeverityLevel.GetSeverityLevel(this.MinSeverityLevel);
			_maxSeverityLevel = SeverityLevel.GetSeverityLevel(this.MaxSeverityLevel);
		}
		#endregion


		#region internal override method Filtrate
		internal override FilterResult Filtrate(LogEvent logEvent)
		{
			#region assertions
			Debug.Assert(logEvent != null, "logEvent is null.");
			#endregion
			//
			if (_minSeverityLevel != null && _minSeverityLevel > logEvent.SeverityLevel)
				return FilterResult.Reject;
			if (_maxSeverityLevel != null && _maxSeverityLevel < logEvent.SeverityLevel)
				return FilterResult.Reject;
			return this.AcceptOnMatch ? FilterResult.Accept : FilterResult.Reject;
		}
		#endregion
	}
}
