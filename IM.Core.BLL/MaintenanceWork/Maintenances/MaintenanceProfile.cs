using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL.MaintenanceWork;

namespace InfraManager.BLL.MaintenanceWork.Maintenances;

internal sealed class MaintenanceProfile : Profile
{
    public MaintenanceProfile()
    {
        CreateMap<Maintenance, MaintenanceDetails>()
            .ForMember(p => p.MaintenanceFolderID, m => m.MapFrom(scr => scr.FolderID))
            .ForMember(p => p.WorkOrderTemplateName,
                m => m.MapFrom( scr => scr.WorkOrderTemplate != null
                                            ? scr.WorkOrderTemplate.Name 
                                            : string.Empty))
            .ForMember(p => p.MaintenanceFolderName,
                m => m.MapFrom(scr => scr.Folder != null 
                                        ? scr.Folder.Name 
                                        : string.Empty))
            .ForMember(p=> p.MultiplicityName , m => m.MapFrom<LocalizedEnumResolver<Maintenance, MaintenanceDetails, MaintenanceMultiplicity>,
                        MaintenanceMultiplicity>(
                        entity => entity.Multiplicity))
            .ForMember(p => p.StateName, m => m.MapFrom<LocalizedEnumResolver<Maintenance, MaintenanceDetails, MaintenanceState>,
                        MaintenanceState>(
                        entity => entity.State))
            .ReverseMap();


        CreateMap<MaintenanceData, Maintenance>()
            .ForMember(c => c.FolderID, m => m.MapFrom(scr => scr.MaintenanceFolderID))
            .ForMember(c => c.Folder, m => m.Ignore());
    }
}

