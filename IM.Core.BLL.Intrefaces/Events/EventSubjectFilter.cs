using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;
using InfraManager.BLL.UserUniqueFiltrations;

namespace InfraManager.BLL.Events
{
    public class EventSubjectFilter : BaseFilter
    {
        public DateTime? DateFrom { get; init; }
        public DateTime? DateTo { get; init; }
        public PersonalUserFiltrationItem[] EventSubjectSearchColumns { get; init; }
    }
}
