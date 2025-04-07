using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk
{
    public class DependencyProfile : Profile
    {
        public DependencyProfile()
        {
            CreateMap<Dependency, DependencyDetails>()
                .ForMember(dst => dst.IsLocked, m => m.MapFrom(src => src.Locked))
                ;
        }
    }
}
