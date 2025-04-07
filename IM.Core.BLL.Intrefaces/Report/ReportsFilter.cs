using System;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.Report;

public class ReportsFilter : BaseFilter
{
    public Guid? FolderID { get; init; }
}