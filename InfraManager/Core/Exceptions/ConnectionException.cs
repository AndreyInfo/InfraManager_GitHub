using System;
using System.Runtime.Serialization;

namespace InfraManager.Core.Exceptions
{
	[Serializable]
	public sealed class ConnectionException : ApplicationException
	{
		public ConnectionException()
			: base()
		{ }

		public ConnectionException(string message)
			: base(message)
		{ }

		public ConnectionException(string message, Exception innerException)
			: base(message, innerException)
		{ }

		public ConnectionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
	}
}
