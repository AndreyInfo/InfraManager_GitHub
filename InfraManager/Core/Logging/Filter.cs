using System.ComponentModel;
using System.Xml.Serialization;
using InfraManager.ComponentModel;

namespace InfraManager.Core.Logging
{
	public abstract class Filter : IActivatable
	{
		#region properties
		[XmlAttribute("acceptOnMatch"), DefaultValue(true)]
		public bool AcceptOnMatch { get; set; }
		#endregion


		#region interface IActivatable
		[XmlIgnore]
		public bool IsActivated { get; protected set; }

		[XmlIgnore]
		public bool IsSuccessful { get; protected set; }

		public virtual void Activate()
		{
			this.IsActivated = true;
			this.IsSuccessful = true;
		}
		#endregion


		#region internal abstract method Filtrate
		internal abstract FilterResult Filtrate(LogEvent logEvent);
		#endregion
	}
}
