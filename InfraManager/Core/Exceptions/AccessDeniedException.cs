using System;
using System.Runtime.Serialization;

namespace InfraManager.Core.Exceptions
{
	[Serializable]
	public sealed class AccessDeniedException : ApplicationException
	{
		#region constructors
		public AccessDeniedException()
			: base()
		{ }

		public AccessDeniedException(string message)
			: base(message)
		{ }

		public AccessDeniedException(string message, Exception innerException)
			: base(message, innerException)
		{ }

		public AccessDeniedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
		#endregion
	}
}
