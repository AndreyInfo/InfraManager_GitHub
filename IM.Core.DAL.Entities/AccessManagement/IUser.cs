using System;

namespace InfraManager.DAL.AccessManagement
{
    public interface IUser
    {
        int ID { get; }
        string Name { get; }
        string Patronymic { get; }
        string Surname { get; }
        string Initials { get; }
        Guid IMObjID { get; }
    }
}
