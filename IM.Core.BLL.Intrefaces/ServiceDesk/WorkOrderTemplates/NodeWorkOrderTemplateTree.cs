using System;

namespace InfraManager.BLL.ServiceDesk.WorkOrderTemplates;

public class NodeWorkOrderTemplateTree
{
    public Guid ID { get; init; }

    public string Name { get; init; }

    public Guid? ParentId { get; set; }

    public bool HasChild { get; set; }

    public string IconName { get; set; }
}
