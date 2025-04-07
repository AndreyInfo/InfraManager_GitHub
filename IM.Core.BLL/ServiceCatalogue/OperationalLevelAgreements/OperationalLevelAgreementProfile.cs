using AutoMapper;
using InfraManager.DAL.ServiceCatalogue.OperationalLevelAgreements;

namespace InfraManager.BLL.ServiceCatalogue.OperationalLevelAgreements;

public class OperationalLevelAgreementProfile : Profile
{
    public OperationalLevelAgreementProfile()
    {
        CreateMap<OperationalLevelAgreementData, OperationalLevelAgreement>()
            .ForMember(x => x.ConcludedWith, x => x.MapFrom(x => x.Concludes));

        CreateMap<OperationalLevelAgreementCloneData, OperationalLevelAgreement>();

        CreateMap<OperationalLevelAgreement, OperationalLevelAgreementDetails>()
            .ForMember(x => x.TimeZoneName, x => x.MapFrom(x => x.TimeZone.Name))
            .ForMember(x => x.CalendarWorkSchedule, x => x.MapFrom(x => x.CalendarWorkSchedule.Name))
            .ForMember(x => x.Concludes, x => x.MapFrom(x => x.ConcludedWith));
    }
}