using System;

namespace InfraManager.DAL.Users;

public class EmployeeWorkloadQueryResultItem
{
    public Guid ID { get; init; }
    public string FullName { get; init; }
    public bool IsOnWorkplace { get; init; }
    public int CallCount { get; init; }
    public int WorkOrderCount { get; init; }
}