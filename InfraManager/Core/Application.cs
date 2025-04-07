using System.Xml.Serialization;

namespace InfraManager.Core
{
	[XmlRoot(ElementName)]
	public sealed class Application
	{
		#region fields
		public const string ElementName = "application";
		#endregion


		#region properties
		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlAttribute("version")]
		public string Version { get; set; }
		#endregion
	}
}
