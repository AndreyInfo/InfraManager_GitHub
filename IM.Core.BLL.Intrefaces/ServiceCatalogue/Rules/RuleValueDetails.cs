using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.DTOs;
using System;
using System.Collections.Generic;
using InfraManager.BLL.Calls.DTO;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceItems;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceAttendances;

namespace InfraManager.BLL.ServiceCatalogue.Rules
{
    public class RuleValueDetails
    {
        public string Price { get; init; }
        public long? PromiseTime { get; init; }
        public ServiceDetails[] Services { get; init; }
        public ServiceAttendanceDetails[] ServiceAttendances { get; init; }
        public ServiceItemDetails[] ServiceItems { get; init; }
        public PriorityDetailsModel[] Priorities { get; init; }
        public UrgencyDTO[] Urgencies { get; init; }
        public CallTypeDetails[] CallTypes { get; init; }
        public List<Tuple<string, int>> ClientPositions { get; init; } = new();
        public List<Tuple<string, ObjectClass, Guid, Guid[], Guid>> ClientOrgStructures { get; init; }
        public List<DayOfWeek> DayOfWeeks { get; init; } = new();
        public Dictionary<Guid, RuleParameterValue[]> ParameterValues { get; init; } = new();
        public Dictionary<FilterOperation, long> TimeRegistrations { get; init; } = new();
    }
}
