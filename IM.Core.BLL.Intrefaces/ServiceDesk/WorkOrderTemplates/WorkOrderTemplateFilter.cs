using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;

namespace InfraManager.BLL.ServiceDesk.WorkOrderTemplates;

public class WorkOrderTemplateFilter : BaseFilter
{
    public Guid FolderID { get; init; }
}
