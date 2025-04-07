using System.IO;
using System.Text;
using System.Xml.Serialization;
using System;

namespace InfraManager.Core.Logging.Listeners
{
	[XmlType(ElementName)]
	public class FileListener : Listener
	{
		#region fields
		public const string ElementName = "fileListener";
		private string _filePath;
		#endregion


		#region properties
		[XmlAttribute("filePath")]
		public string FilePath { get; set; }

		[XmlAttribute("appendToFile")]
		public bool AppendToFile { get; set; }
		#endregion


		#region override method Activate
		public override void Activate()
		{
			base.Activate();
			try
			{
				if (!Path.IsPathRooted(_filePath = this.FilePath))
					_filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.FilePath);
				if (!Directory.Exists(Path.GetDirectoryName(_filePath)))
					Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
				if (!File.Exists(_filePath))
					File.Create(_filePath).Dispose();
			}
			catch
			{
				base.IsSuccessful = false;
			}
		}
		#endregion

		#region override method WriteLogEvent
		public override void WriteLogEvent(LogEvent logEvent)
		{
			const int attempts = 3;
			for (int i = 0; i < attempts; i++ )
				try
				{
					using (var streamWriter = new StreamWriter(_filePath, this.AppendToFile, Encoding.UTF8))
						RenderLogEvent(streamWriter, logEvent);
					break;
				}
				catch { }
		} 
		#endregion
	}
}
