using System.Configuration;
using System.Xml;

namespace InfraManager.Core
{
	public sealed class StubConfigurationSection : IConfigurationSectionHandler
	{
		#region interface IConfigurationSectionHandler
		public object Create(object parent, object configContext, XmlNode section)
		{
			return null;
		}
		#endregion
	}
}
