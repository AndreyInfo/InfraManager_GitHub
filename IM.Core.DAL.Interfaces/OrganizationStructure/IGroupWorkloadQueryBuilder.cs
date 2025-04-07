using System;

namespace InfraManager.DAL.OrganizationStructure;

public interface IGroupWorkloadQueryBuilder
{
    IExecutableQuery<GroupWorkloadListQueryResultItem> BuildQuery(DateTime utcDateTime, Guid currentUserId, GroupType type);
}