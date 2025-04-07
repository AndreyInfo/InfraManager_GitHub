using System;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.AccessManagement.Operations;

public class OperationFilter : BaseFilter
{
    public Guid RoleID { get; init; }
    public bool OnlySelectedForRole { get; init; }
}