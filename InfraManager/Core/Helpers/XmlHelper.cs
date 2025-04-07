using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace InfraManager.Core.Helpers
{
    public static class XmlHelper
	{
		#region classes
		private static class _SerializerCache
		{
			#region fields
			private static readonly ConcurrentDictionary<string, XmlSerializer> __cache = new ConcurrentDictionary<string, XmlSerializer>();
			#endregion


			#region method GetSerializer
			public static XmlSerializer GetSerializer(Type type)
			{
				XmlSerializer result;
                var key = type.FullName;
				if (!__cache.TryGetValue(key, out result))
                {
                    result = new XmlSerializer(type);
                    __cache.TryAdd(key, result);
                }
				return result;
			}
			#endregion
		}
		#endregion


		#region method XmlStringIsValidAgainstXsd
		public static bool XmlStringIsValidAgainstXsd(string xmlString, string xsdFilePath, string ns, out Exception exception)
		{
			using (var xmlTextReader = new XmlTextReader(new StringReader(xmlString)))
				return XmlIsValidAgainstXsd(xmlTextReader, xsdFilePath, ns, out exception);
		}

		public static bool XmlStringIsValidAgainstXsd(string xmlString, string xsdFilePath, string ns)
		{
			using (var xmlTextReader = new XmlTextReader(new StringReader(xmlString)))
				return XmlIsValidAgainstXsd(xmlTextReader, xsdFilePath, ns);
		}
		#endregion

		#region method XmlFileIsValidAgainstXsd
		public static bool XmlFileIsValidAgainstXsd(string xmlFilePath, string xsdFilePath, string ns, out Exception exception)
		{
			using (var xmlTextReader = new XmlTextReader(xmlFilePath))
				return XmlIsValidAgainstXsd(xmlTextReader, xsdFilePath, ns, out exception);
		}

		public static bool XmlFileIsValidAgainstXsd(string xmlFilePath, string xsdFilePath, string ns)
		{
			using (var xmlTextReader = new XmlTextReader(xmlFilePath))
				return XmlIsValidAgainstXsd(xmlTextReader, xsdFilePath, ns);
		}
		#endregion

		#region method Object2XmlString
		public static string Object2XmlString(object @object, XmlSerializerNamespaces xmlSerializerNamespaces)
		{
			if (@object == null)
				return string.Empty;
			var xmlSerializer = _SerializerCache.GetSerializer(@object.GetType());
			using (var memoryStream = new MemoryStream())
			using (var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
			{
				xmlTextWriter.Formatting = Formatting.Indented;
				xmlTextWriter.Indentation = 2;
				xmlSerializer.Serialize(xmlTextWriter, @object, xmlSerializerNamespaces);
				xmlTextWriter.Flush();
				memoryStream.Seek(0, SeekOrigin.Begin);
				using (var streamReader = new StreamReader(memoryStream))
					return streamReader.ReadToEnd();
			}
		}
		#endregion

		#region method XmlNode2Object
		public static T XmlNode2Object<T>(XmlNode xmlNode)
		{
			if (xmlNode == null)
				return default(T);
			//
			using (var memoryStream = new MemoryStream())
			using (var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
			{
				xmlTextWriter.Formatting = Formatting.None;
				xmlTextWriter.Indentation = 0;
				xmlTextWriter.WriteStartDocument();
				xmlTextWriter.WriteRaw(xmlNode.OuterXml);
				xmlTextWriter.Flush();
				memoryStream.Seek(0, SeekOrigin.Begin);
				var xmlSerializer = _SerializerCache.GetSerializer(typeof(T));
				return (T)xmlSerializer.Deserialize(memoryStream);
			}
		}
		#endregion

		#region method XmlString2Object
		public static T XmlString2Object<T>(string xmlString)
		{
			if (xmlString == null)
				return default(T);
			if (xmlString == string.Empty)
				return (T)Activator.CreateInstance(typeof(T));
			//
			var stringReader = new StringReader(xmlString);
			var xmlSerializer = _SerializerCache.GetSerializer(typeof(T));
			return (T)xmlSerializer.Deserialize(stringReader);
		}
		#endregion

		#region method XmlString2XmlDom
		public static IXPathNavigable XmlString2XmlDom(string xmlString)
		{
			var xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xmlString);
			return xmlDocument.DocumentElement;
		}
		#endregion

		#region method Object2XmlDom
		public static IXPathNavigable Object2XmlDom(object @object, XmlSerializerNamespaces xmlSerializerNamespaces)
		{
			return XmlString2XmlDom(Object2XmlString(@object, xmlSerializerNamespaces));
		}
		#endregion

		#region private method XmlIsValidAgainstXsd
		private static bool XmlIsValidAgainstXsd(XmlTextReader xmlTextReader, string xsdFilePath, string ns, out Exception exception)
		{
			bool isValid = true;
			Exception tmp = null;
			try
			{
				var xmlReaderSettings = new XmlReaderSettings();
				xmlReaderSettings.ValidationType = ValidationType.Schema;
				xmlReaderSettings.ValidationEventHandler += (sender, e) => { isValid = false; tmp = e.Exception; };
				xmlReaderSettings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema | XmlSchemaValidationFlags.ReportValidationWarnings;
				xmlReaderSettings.Schemas.Add(ns, xsdFilePath);
				using (var xmlReader = XmlReader.Create(xmlTextReader, xmlReaderSettings))
					while (xmlReader.Read()) { }
				exception = tmp;
			}
			catch (Exception e)
			{
				isValid = false;
				exception = e;
			}
			return isValid;
		}

		private static bool XmlIsValidAgainstXsd(XmlTextReader xmlTextReader, string xsdFilePath, string ns)
		{
			bool isValid = true;
			try
			{
				var xmlReaderSettings = new XmlReaderSettings();
				xmlReaderSettings.ValidationType = ValidationType.Schema;
				xmlReaderSettings.ValidationEventHandler += (sender, e) => { isValid = false; };
				xmlReaderSettings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
				xmlReaderSettings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
				xmlReaderSettings.Schemas.Add(ns, xsdFilePath);
				using (var xmlReader = XmlReader.Create(xmlTextReader, xmlReaderSettings))
					while (xmlReader.Read()) { }
			}
			catch
			{
				isValid = false;
			}
			return isValid;
		}
		#endregion
	}
}
