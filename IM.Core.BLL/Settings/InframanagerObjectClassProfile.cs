using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings
{
    public class InframanagerObjectClassProfile : Profile
    {
        public InframanagerObjectClassProfile()
        {
            CreateMap<DAL.Settings.InframanagerObjectClass, InframanagerObjectClassData>().ReverseMap();
        }
    }
}
