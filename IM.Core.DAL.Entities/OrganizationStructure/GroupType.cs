using System;

namespace InfraManager.DAL.OrganizationStructure
{
    [Flags]
    public enum GroupType : byte
    {
        None = 0,
        Call = 1,
        WorkOrder = 2,
        MassiveIncident = 1 << 2,
        Problem = 1 << 3,
        ChangeRequest = 1 << 4,
        All = Call | WorkOrder | MassiveIncident | Problem | ChangeRequest
    }
}