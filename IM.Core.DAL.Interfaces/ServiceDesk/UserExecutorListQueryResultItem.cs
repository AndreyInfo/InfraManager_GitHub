using System;

namespace InfraManager.DAL.ServiceDesk;

public class UserExecutorListQueryResultItem
{
    public Guid IMObjID { get; init; }
    public string Name { get; init; }
    public Guid? SubdivisionID { get; init; }
    public string Details { get; init; }
}