using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace InfraManager.Core.Logging.Formatters
{
	[XmlType(ElementName)]
    public sealed class BasicFormatter : Formatter
    {
        #region fields
        public const string ElementName = "basicFormatter";

        private string _old_mashineName;
        private string _old_operationSystem;
        private string _old_processName;
        private string _old_userLoguin;
        #endregion


        #region internal override method Format
        internal override void Format(TextWriter writer, LogEvent logEvent)
        {
            #region assertions
            Debug.Assert(writer != null, "writer is null.");
            Debug.Assert(logEvent != null, "logEvent is null.");
            #endregion
            //
            FormatHeader(writer, logEvent);
            writer.Write("Thread: ");
            writer.WriteLine(logEvent.CurrentThreadID);
            if (logEvent.Message != null)
            {
                writer.Write("Message: ");
                writer.WriteLine(logEvent.Message);
            }
            if (logEvent.Exception != null)
                writer.WriteLine(logEvent.GetException());
            //
            if (_old_mashineName != logEvent.MachineName)
            {
                writer.Write("Machine name: ");
                writer.WriteLine(logEvent.MachineName);
                _old_mashineName = logEvent.MachineName;
            }
            if (_old_operationSystem != logEvent.OperatingSystem)
            {
                writer.Write("Operating system: ");
                writer.WriteLine(logEvent.OperatingSystem);
                _old_operationSystem = logEvent.OperatingSystem;
            }
            if (_old_processName != logEvent.ProcessName)
            {
                writer.Write("Process name: ");
                writer.WriteLine(logEvent.ProcessName);
                _old_processName = logEvent.ProcessName;
            }
            if (_old_userLoguin != logEvent.UserLogin)
            {
                writer.Write("User login: ");
                writer.WriteLine(logEvent.UserLogin);
                _old_userLoguin = logEvent.UserLogin;
            }
            FormatFooter(writer, logEvent);
        }
        #endregion
    }
}
