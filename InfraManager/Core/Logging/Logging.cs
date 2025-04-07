using System.Xml.Serialization;
using InfraManager.ComponentModel;

namespace InfraManager.Core.Logging
{
	[XmlRoot(ElementName)]
	public sealed class Logging : IActivatable
	{
		#region fields
		public const string ElementName = "logging";
		#endregion


		#region properties
		[XmlAttribute("version")]
		public string Version { get; set; }

		[XmlAttribute("verboseIsEnabled")]
		public bool VerboseIsEnabled { get; set; }

		[XmlAttribute("traceIsEnabled")]
		public bool TraceIsEnabled { get; set; }

		[XmlAttribute("debugIsEnabled")]
		public bool DebugIsEnabled { get; set; }

		[XmlAttribute("infoIsEnabled")]
		public bool InfoIsEnabled { get; set; }

		[XmlAttribute("warningIsEnabled")]
		public bool WarningIsEnabled { get; set; }

		[XmlAttribute("errorIsEnabled")]
		public bool ErrorIsEnabled { get; set; }

		[XmlAttribute("criticalIsEnabled")]
		public bool CriticalIsEnabled { get; set; }

		[XmlAttribute("fatalIsEnabled")]
		public bool FatalIsEnabled { get; set; }

		[XmlArray("listeners")]
		[XmlArrayItem(typeof(Listeners.ColoredConsoleListener))]
		[XmlArrayItem(typeof(Listeners.ConsoleListener))]
		[XmlArrayItem(typeof(Listeners.DebugListener))]
		[XmlArrayItem(typeof(Listeners.EventLogListener))]
		[XmlArrayItem(typeof(Listeners.FileListener))]
		[XmlArrayItem(typeof(Listeners.TraceListener))]
		public Listener[] Listeners { get; set; }
		#endregion


		#region interface IActivatable
		[XmlIgnore]
		public bool IsActivated { get; private set; }

		[XmlIgnore]
		public bool IsSuccessful { get; private set; }

		public void Activate()
		{
			if (this.Listeners != null)
				foreach (var listener in this.Listeners)
					listener.Activate();
			this.IsActivated = true;
			this.IsSuccessful = true;
		}
		#endregion
	}
}
