using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.Core.Exceptions
{
    public class ManhoursWorkException: ArithmeticException
    {
        #region properties
        public Guid WorkID { get; private set; }
        public String WorkName { get; private set; }
        public Guid UserID { get; private set; }
        public String UserFullName { get; private set; }
        #endregion

        #region constructors
        public ManhoursWorkException()
			: base()
		{ }

        public ManhoursWorkException(string message)
			: base(message)
		{ }

        public ManhoursWorkException(string message, Guid workID, string workName, Guid userID, string userFullName)
			: base(message)
		{
            this.WorkID = workID;
            this.WorkName = workName;
            this.UserID = userID;
            this.UserFullName = userFullName;
        }

        public ManhoursWorkException(string message, Exception innerException)
			: base(message, innerException)
		{ }

        public ManhoursWorkException(string message, Exception innerException, Guid workID, string workName, Guid userID, string userFullName)
            : base(message, innerException)
        {
            this.WorkID = workID;
            this.WorkName = workName;
            this.UserID = userID;
            this.UserFullName = userFullName;
        }
        #endregion
    }
}
