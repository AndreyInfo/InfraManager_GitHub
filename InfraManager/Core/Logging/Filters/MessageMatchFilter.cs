using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using InfraManager.Core.Extensions;

namespace InfraManager.Core.Logging.Filters
{
	[XmlType(ElementName)]
	public sealed class MessageMatchFilter : Filter
	{
		#region fields
		public const string ElementName = "messageMatch";
		private Regex _matchingRegex;
		#endregion


		#region properties
		[XmlAttribute("matchingString")]
		public string MatchingString { get; set; }

		[XmlAttribute("matchingRegex")]
		public string MatchingRegex { get; set; }
		#endregion


		#region interface IActivatable
		public override void Activate()
		{
			base.Activate();
			try
			{
				if (!this.MatchingRegex.IsNullOrEmpty())
					_matchingRegex = new Regex(this.MatchingRegex, RegexOptions.Compiled);
			}
			finally
			{
				this.IsSuccessful = false;
			}
		}
		#endregion


		#region internal override method Filtrate
		internal override FilterResult Filtrate(LogEvent logEvent)
		{
			#region assertions
			Debug.Assert(logEvent != null, "logEvent is null.");
			#endregion
			//
			if (this.MatchingString.IsNullOrEmpty() && _matchingRegex == null)
				return FilterResult.Ignore;
			var message = logEvent.Message;
			if (message.IsNullOrEmpty())
				return FilterResult.Ignore;
			if (!this.MatchingString.IsNullOrEmpty())
			{
				if (message.IndexOf(this.MatchingString) == -1)
					return FilterResult.Ignore;
				return this.AcceptOnMatch ? FilterResult.Accept : FilterResult.Reject;
			}
			if (_matchingRegex != null)
			{
				if (_matchingRegex.Match(message).Success == false)
					return FilterResult.Ignore;
				return this.AcceptOnMatch ? FilterResult.Accept : FilterResult.Reject;
			}
			return FilterResult.Ignore;
		}
		#endregion
	}
}
