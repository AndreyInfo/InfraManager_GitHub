using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceCatalogue;
using System;
using System.Collections.Generic;
using InfraManager.BLL.Localization;

namespace InfraManager.BLL.ServiceCatalogue.Rules
{
    public enum FilterOperation
    {
        [FriendlyName("=")]
        Equal = 1,
        [FriendlyName("<>")]
        NotEqual = 2,
        [FriendlyName("Like")]
        Like = 4,
        [FriendlyName("<")]
        LT = 8,
        [FriendlyName("<=")]
        LTE = 16,
        [FriendlyName(">=")]
        GTE = 32,
        [FriendlyName(">")]
        GT = 64
    }

    /// <summary>
    /// Тип значения правила
    /// </summary>
    public enum RuleValueType : byte
    {
        Service = 0,
        ServiceAttendance = 1,
        ServiceItem = 2,
        DayOfWeek = 3,
        RegistrationTime = 4,
        Priority = 5,
        ClientPosition = 6,
        ClientOrgStructure = 11,
        CallType = 9,
        Urgency = 12,
        //
        PromiseTime = 7,
        Price = 8,
        ParameterTemplate = 10
    }

    public class RuleValue
    {

        public RuleValue()
        {
            ParameterValues = new();
            TimeRegistrations = new();
            DayOfWeeks = new();
        }


        public const string VERSION = "5.0.12";

        public string Price { get; set; }
        public long? PromiseTime { get; set; }
        public Service[] Services { get; set; }
        public ServiceAttendance[] ServiceAttendances { get; set; }
        public ServiceItem[] ServiceItems { get; set; }
        public Priority[] Priorities { get; set; }
        public Urgency[] Urgencies { get; set; }
        public CallType[] CallTypes { get; set; }
        public Tuple<string, int>[] ClientPositions { get; set; }
        public List<Tuple<string, ObjectClass, Guid, Guid[], Guid>> ClientOrgStructures { get; set; }
        public List<DayOfWeek> DayOfWeeks { get; set; }
        public Dictionary<Guid, RuleParameterValue[]> ParameterValues { get; set; }
        public Dictionary<FilterOperation, long> TimeRegistrations { get; set; }
    }
}
