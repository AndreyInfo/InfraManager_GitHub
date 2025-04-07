using System.Xml;

namespace InfraManager.Core
{
	public static class XmlSchema
	{
		#region fields
		public const string XSIPrefix = "xsi";
		public const string XSINamespace = "http://www.w3.org/2001/XMLSchema-instance"; 
		public const string IMFPrefix = "imf";
		public const string IMFNamespace = "urn:InfraManager.Framework";
		#endregion


		#region method GetXmlQualifiedNames
		public static XmlQualifiedName[] GetXmlQualifiedNames()
		{
			return new XmlQualifiedName[] { new XmlQualifiedName(XSIPrefix, XSINamespace), new XmlQualifiedName(IMFPrefix, IMFNamespace) };
		}
		#endregion
	}
}
