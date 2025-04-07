using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk.Manhours
{
    public class ManhoursDetailsModel
    {
        public Guid ID { get; init; }
        public int Value { get; init; }
        public Guid WorkID { get; init; }
        public DateTime UtcDate { get; init; }
    }
}