using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.Core.Exceptions
{
    public sealed class FiltrationException : Exception
    {
        #region properties
        public string FilterName { get; private set; }
        #endregion

        #region constructors
        public FiltrationException()
			: base()
		{ }

        public FiltrationException(string message)
			: this(null, message)
		{ }

        public FiltrationException(string filterName, string message)
			: base(message)
		{
            this.FilterName = filterName;
        }

        public FiltrationException(string message, Exception innerException)
			: base(message, innerException)
		{ }

        public FiltrationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
        #endregion
    }
}
