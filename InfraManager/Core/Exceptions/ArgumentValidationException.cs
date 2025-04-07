using System;
using System.Runtime.Serialization;

namespace InfraManager.Core.Exceptions
{
	[Serializable]
	public sealed class ArgumentValidationException : ApplicationException
	{
		#region properties
		public string PropertyName { get; private set; }
        /// <summary>
        /// число или граница
        /// </summary>
        public string Value { get; private set; }
        /// <summary>
        /// тип отклонения от нормы
        /// </summary>
        public ValidationErrorType ErrorType { get; private set; }
        #endregion


        #region constructors
        public ArgumentValidationException()
			: base()
		{ }

		public ArgumentValidationException(string message)
			: this(null, message)
		{ }

        public ArgumentValidationException(string propertyName, string message)
            : this(propertyName, message, ValidationErrorType.None)
        { }

        public ArgumentValidationException(string propertyName, string message, ValidationErrorType type)
            : this(propertyName, message, type, null)
        { }

        public ArgumentValidationException(string propertyName, string message, ValidationErrorType type, string value)
            : base(message)
        {
            this.PropertyName = propertyName;
            this.ErrorType = type;
            this.Value = value;
        }

        public ArgumentValidationException(string message, Exception innerException)
			: base(message, innerException)
		{ }

		public ArgumentValidationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
		#endregion
	}

    public enum ValidationErrorType
    {
        None = 0,
        Greater,
        Lesser,
        NotEqual,
        Equal,
        NotInside,
        Inside,
        NotEmpty,
        Empty,
        AlreadyExist
    }
}
