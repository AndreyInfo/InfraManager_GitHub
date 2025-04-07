using AutoMapper;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.Models.ProductCatalogImportSettings;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.Configuration;
using IM.Core.Import.BLL.Interface.Import;
using InfraManager.DAL.Location;
using InfraManager.DAL.OrganizationStructure;
using Inframanager.DAL.ProductCatalogue.Import;
using InfraManager;
using InfraManager.ComponentModel;
using InfraManager.Core.Extensions;
using InfraManager.DAL.Import;
using InfraManager.DAL.Users;

namespace IM.Core.Import.BLL.Import.Importer;

public class ImportProfile : Profile
{
    public ImportProfile()
    {

        CreateMap<ConnectorType,ConnectorTypeOutputDetails>();
        CreateMap<TechnologyType,TechnologyTypeOutputDetails>().ReverseMap();
        CreateMap<ProductCatalogImportCSVConfigurationConcordance, ScriptDataDetails<ConcordanceObjectType>>()
            .ForMember(x => x.FieldName, x => x.MapFrom(x => x.Field))
            .ForMember(x => x.Script, x => x.MapFrom(y => y.Expression));
        CreateMap<ImportModel, UserDetails>()
            .ForMember(x => x.Name, x => x.MapFrom(x => x.UserFirstName))
            .ForMember(x => x.Surname, x => x.MapFrom(x => x.UserLastName))
            .ForMember(x => x.Patronymic, x => x.MapFrom(x => x.UserPatronymic))
            .ForMember(x => x.LoginName, x => x.MapFrom(x => x.UserLogin))
            .ForMember(x => x.Phone, x => x.MapFrom(x => x.UserPhone))
            .ForMember(x => x.Fax, x => x.MapFrom(x => x.UserFax))
            .ForMember(x => x.Pager, x => x.MapFrom(x => x.UserPager))
            .ForMember(x => x.Email, x => x.MapFrom(x => x.UserEmail))
            .ForMember(x => x.Note, x => x.MapFrom(x => x.UserNote))
            .ForMember(x => x.SID, x => x.MapFrom(x => x.UserSID))
            .ForMember(x => x.Number, x => x.MapFrom(x => x.UserNumber))
            .ForMember(x => x.ExternalID, x => x.MapFrom(x => x.UserExternalID ?? string.Empty))
            .ForMember(x => x.Phone1, x => x.MapFrom(x => x.UserPhoneInternal))
            .ForMember(x=>x.ImportModel, x=>x.MapFrom(x => x))
            .ForMember(x=>x.ManagerIdentifier,x=>x.MapFrom(y=>y.UserManager))
            .ForMember(x=>x.SubdivisionName,x=>x.MapFrom(y=>y.UserSubdivision))
            .ForMember(x=>x.ParentOrganizationName, x=>x.MapFrom(y=>y.UserOrganization))
            .ForMember(x=>x.ParentOrganizationExternalID, x=>x.MapFrom(y=>y.UserOrganizationExternalID))
            .ForMember(x=>x.SubdivisionExternalID,x=>x.MapFrom(y=>y.UserSubdivisionExternalID))
            .ForMember(x=>x.ManagerIdentifier,x=>x.MapFrom(y=>y.UserManager));
        CreateMap<UserDetails, User>()
            .ForMember(x=>x.Manager, x=>x.Condition(y=>y.Manager != null))
            .ForMember(x=>x.SubdivisionName, x=>x.MapFrom(y=>y.SubdivisionName.LastOrDefault()));
        CreateMap<IUserDetails, User>()
            .ForMember(x => x.Manager, x => x.Condition(y => y.Manager != null))
            .ForMember(x => x.SubdivisionName, x => x.MapFrom(y => y.SubdivisionName.LastOrDefault()));
        CreateMap<User,IUserDetails>().As<UserDetails>();
        CreateMap<User, UserDetails>()
            .ForMember(x => x.SubdivisionName, x => x.Ignore());
        CreateMap<UserDetails, WorkplaceModel>()
            .ForMember(x => x.ExternalId, x => x.MapFrom(x => x.WorkPlaceID));
        CreateMap<ImportModel, OrganizationDetails>()
            .ForMember(x => x.Name, x => x.MapFrom(x => x.OrganizationName))
            .ForMember(x => x.Note, x => x.MapFrom(x => x.OrganizationNote))
            .ForMember(x => x.ExternalId, x => x.MapFrom(x => x.OrganizationExternalID));
        CreateMap<ImportModel, SubdivisionDetails>()
            .ForMember(x => x.Name, x => x.MapFrom(y => y.SubdivisionName))
            .ForMember(x => x.Note, x => x.MapFrom(y => y.SubdivisionNote))
            .ForMember(x => x.ExternalID, x => x.MapFrom(y => y.SubdivisionExternalID))
            .ForMember(x=>x.ParentFullName, x=>x.MapFrom(y=>y.SubdivisionParent))
            .ForMember(x=>x.ParentExternalID, x=>x.MapFrom(y=>y.SubdivisionParentExternalID))
            .ForMember(x=>x.OrganisationExternalId,x=>x.MapFrom(y=>y.SubdivisionOrganizationExternalID))
            .ForMember(x=>x.OrganisationName, x=>x.MapFrom(y=>y.SubdivisionOrganization));
        CreateMap<SubdivisionDetails, Subdivision>()
            //todo:сделать отдельный details
            .ForMember(x=>x.ParentSubdivision, x=>x.Ignore())
            .ReverseMap();
        
        CreateMap<ISubdivisionDetails, Subdivision>()
            //todo:сделать отдельный details
            .ForMember(x=>x.ParentSubdivision, x=>x.Ignore())
            .ReverseMap();
       
        CreateMap<WorkplaceModel, Workplace>();
        CreateMap<ImportModel, WorkplaceModel>()
            .ForMember(x => x.Name, x => x.MapFrom(x => x.UserWorkplace));
        CreateMap<ImportModel, PositionModel>()
            .ForMember(x => x.Name, x => x.MapFrom(x => x.UserPosition));
        CreateMap<PositionModel, JobTitle>()
            .ForMember(x => x.IMObjID, x => x.MapFrom(x => Guid.NewGuid()));
        CreateMap<OrganizationDetails, Organization>()
            .ReverseMap();
        CreateMap<IUserDetails, FIO>();
        CreateMap<IUserDetails, NameSurname>();
        CreateMap<IUserDetails, User>();

    }

}