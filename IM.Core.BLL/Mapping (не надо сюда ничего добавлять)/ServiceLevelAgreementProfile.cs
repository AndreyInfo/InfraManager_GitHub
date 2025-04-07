using AutoMapper;
using InfraManager.BLL.ServiceDesk.DTOs;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceCatalogue;
using System;
using System.Linq;
using InfraManager.BLL.ServiceCatalogue;

namespace InfraManager.BLL.Mapping
{
    public class ServiceLevelAgreementProfile : Profile
    {
        public ServiceLevelAgreementProfile()
        {
            CreateMap<ServiceLevelAgreement, ServiceLevelAgreement>();

            CreateMap<ServiceLevelAgreement, ServiceLevelAgreementDetails>()
                .ForMember(x => x.Concludeds, y => y.MapFrom(z => z.OrganizationItemGroups))
                .ForMember(x => x.CalendarWorkSchedule, y => y.MapFrom(y => y.CalendarWorkSchedule.Name))
                .ForMember(x => x.TimeZoneName, z => z.MapFrom(q => q.TimeZone.Name));

            CreateMap<ServiceLevelAgreementData, ServiceLevelAgreement>();
        }
    }
}
