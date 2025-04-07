using System;

namespace InfraManager.DAL.Users;

public interface IEmployeeWorkloadQueryBuilder
{
    IExecutableQuery<EmployeeWorkloadQueryResultItem> BuildQuery(DateTime utcDateTime, DateTime utcLastActivityTime);
}