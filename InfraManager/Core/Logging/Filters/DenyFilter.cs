using System.Xml.Serialization;

namespace InfraManager.Core.Logging.Filters
{
	[XmlType(ElementName)]
	public class DenyFilter : Filter
	{
		#region fields
		public const string ElementName = "deny";
		#endregion


		#region internal override method Filtrate
		internal override FilterResult Filtrate(LogEvent logEvent)
		{
			return FilterResult.Reject;
		}
		#endregion
	}
}
