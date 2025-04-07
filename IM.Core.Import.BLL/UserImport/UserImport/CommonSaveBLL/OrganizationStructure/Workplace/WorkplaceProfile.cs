using AutoMapper;
using IM.Core.Import.BLL.Interface;
using InfraManager.DAL.Location;

namespace IM.Core.Import.BLL
{
    public class WorkplaceProfile : Profile
    {
        public WorkplaceProfile()
        {
            CreateMap<WorkplaceModel, Workplace>();
        }
    }
}
