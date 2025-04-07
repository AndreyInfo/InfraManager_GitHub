using System;
using System.Collections.Generic;
using System.Text;

namespace InfraManager.CrossPlatform.WebApi.Contracts.Auth
{
    public sealed class UserContext
    {
        public UserContext(Guid userID)
        {
            UserID = userID;
        }
        public Guid UserID { get;}
    }
}
