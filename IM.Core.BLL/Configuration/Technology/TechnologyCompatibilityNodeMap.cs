using AutoMapper;
using InfraManager.BLL.Extensions;
using InfraManager.BLL.MaintenanceWork;
using InfraManager.DAL.MaintenanceWork;
using System.Linq;
using InfraManager.DAL.Configuration;
using InfraManager.BLL.Technologies;

namespace InfraManager.BLL.Configuration.Technology
{
    public class TechnologyCompatibilityNodeProfile : Profile
    {
        public TechnologyCompatibilityNodeProfile()
        {
            CreateMap<TechnologyCompatibilityNode, TechnologyCompatibilityNodeData>().ReverseMap();
        }
    }
}
