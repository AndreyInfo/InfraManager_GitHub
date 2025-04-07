using System;

namespace InfraManager.BLL.ServiceDesk.WorkOrderTemplates
{
    public class WorkOrderTemplateLookupListFilter : LookupListFilter
    {
        public Guid? WorkOrderTypeID { get; init; }
    }
}
