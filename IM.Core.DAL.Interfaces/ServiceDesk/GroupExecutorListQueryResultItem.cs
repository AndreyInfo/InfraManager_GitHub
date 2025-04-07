using System;
using System.Collections.Generic;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.DAL.ServiceDesk;

public class GroupExecutorListQueryResultItem
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public string ResponsibleName { get; init; }
    public string Note { get; init; }
    public GroupType Type { get; init; }
    public Guid ResponsibleUserID { get; init; }
    public IEnumerable<User> QueueUserList { get; init; }
}