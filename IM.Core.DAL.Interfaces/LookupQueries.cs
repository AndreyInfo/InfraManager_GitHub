namespace InfraManager.DAL
{
    public enum LookupQueries
    {
        None = 0,
        ReferencedProblemTypes,
        ProblemStateNames,
        ProblemUrgencies,
        ProblemInfluences,
        ProblemPriorities,
        ProblemBudget,
        ProblemBudgetUsageCause,

        CallReceiptType,
        CallSla,
        CallServiceName,
        CallUrgencies,
        CallInfluences,
        CallPriorities,
        CallBudget,
        CallBudgetUsageCause,
        CallStateName,
        CallType,

        WorkOrderBudget,
        WorkOrderBudgetUsageCause,
        WorkOrderPriorities,
        WorkOrderStateName,
        WorkOrderType,

        TaskCategory,
        TaskType,
        TaskPriority,
        TaskStateName,

        NegotiationStatus,
        NegotiationMode,
        IssueCategory,

        ChangeRequestCategory,
        ChangeRequestInfluence,
        ChangeRequestPriority,
        ChangeRequestStateName,
        ChangeRequestType,
        ChangeRequestUrgency,

        MassIncidentInformationChannels,
        MassIncidentTypes,
        MassIncidentCauses,
        MassIncidentServices,
        MassIncidentStates,
        MassIncidentPriorities,
        MassIncidentCriticalities,
        MassIncidentSLAs,
        
        Suppliers,
        Manufacturers,
        ProductCatalogTypes,
        ProductCatalogTemplates,
        ServiceContracts,
        ServiceContractStates,
        HardwareStates,
        IsWorkingLookup,
        LocationOnStoreLookup,
        HardwareModelNames,
        ProblemServiceName
    }
}
