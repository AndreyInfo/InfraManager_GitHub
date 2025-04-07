using InfraManager.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraManager.ComponentModel
{
    public interface IUser : IUserIdentifier
    {
        string FullName { get; }
        bool OperationIsGranted(int operationID);
    }
    public interface IUserWithRoles : IUser
    {
        BaseList<IRole> RoleList { get; }
    }

    public interface IUserIdentifier
    {
        Guid ID { get; }
    }

}
