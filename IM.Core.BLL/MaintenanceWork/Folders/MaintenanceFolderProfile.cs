using AutoMapper;
using InfraManager.DAL.MaintenanceWork;
using System.Linq;

namespace InfraManager.BLL.MaintenanceWork.Folders;

internal sealed class MaintenanceFolderProfile : Profile
{
    public MaintenanceFolderProfile()
    {
        CreateMap<MaintenanceFolderData, MaintenanceFolder>();

        CreateMap<MaintenanceFolder, MaintenanceFolderDetails>();
    }
}
