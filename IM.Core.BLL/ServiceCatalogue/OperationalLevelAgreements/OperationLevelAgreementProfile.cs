using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceCatalogue.OperationalLevelAgreements;

namespace InfraManager.BLL.ServiceCatalogue.OperationalLevelAgreements;

public class OperationLevelAgreementProfile : Profile
{
    public OperationLevelAgreementProfile()
    {
        CreateMap<OperationalLevelAgreementServiceListItem, OperationalLevelAgreementServiceDetails>()
            .ForMember(c => c.StateName, m =>
                m.MapFrom<LocalizedEnumResolver<OperationalLevelAgreementServiceListItem,
                    OperationalLevelAgreementServiceDetails, CatalogItemState>, CatalogItemState>(
                    entity => entity.State));
    }
}