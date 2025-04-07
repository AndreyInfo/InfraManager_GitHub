using System;
using System.Runtime.Serialization;

namespace InfraManager.Core.Exceptions
{
	[Serializable]
	public sealed class DeviceMonitorExceededException : ApplicationException
	{
		public DeviceMonitorExceededException()
			: base("Ограничение системы мониторинга")
		{ }

		public DeviceMonitorExceededException(string message)
			: base(message)
		{ }

		public DeviceMonitorExceededException(string message, Exception innerException)
			: base(message, innerException)
		{ }

		public DeviceMonitorExceededException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
	}
}
