using System;
using System.Runtime.Serialization;

namespace InfraManager.Core.Exceptions
{
	[Serializable]
	public sealed class DemoVersionException : ApplicationException
	{
		public DemoVersionException()
			: base("Ограничение демо-версии")
		{ }

		public DemoVersionException(string message)
			: base(message)
		{ }

		public DemoVersionException(string message, Exception innerException)
			: base(message, innerException)
		{ }

		public DemoVersionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
	}
}
