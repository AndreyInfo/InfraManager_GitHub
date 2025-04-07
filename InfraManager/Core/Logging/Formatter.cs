using System.IO;
using InfraManager.ComponentModel;
using System.Xml.Serialization;

namespace InfraManager.Core.Logging
{
	public abstract class Formatter : IActivatable
	{
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


		#region protected abstract method Format
		internal abstract void Format(TextWriter writer, LogEvent logEvent);
		#endregion


		#region protected virtual method FormatHeader
		protected virtual void FormatHeader(TextWriter writer, LogEvent logEvent)
		{
			writer.Write(logEvent.TimeStamp.ToString("yyyy.MM.dd HH:mm:ss.ffff"));
			writer.Write(" ");
			writer.Write(logEvent.SeverityLevel.DisplayName);
			writer.Write(" ");
			writer.Write(logEvent.ApplicationName);
			writer.Write(" ");
			writer.WriteLine(logEvent.ApplicationVersion);
		}
		#endregion

		#region protected virtual method FormatFooter
		protected virtual void FormatFooter(TextWriter writer, LogEvent logEvent)
		{
			writer.WriteLine();
		}
		#endregion
	}
}
