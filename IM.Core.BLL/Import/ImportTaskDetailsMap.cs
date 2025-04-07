using AutoMapper;
using IM.Core.Import.BLL.Interface.Import;
using InfraManager.DAL.Import;

namespace InfraManager.BLL.Import
{
    internal class ImportTaskDetailsMap : Profile
    {
        public ImportTaskDetailsMap()
        {
            CreateMap<ImportTasksDetails, ImportTasksDetailsAPI>().ReverseMap();
        }
    }
}
