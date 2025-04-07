using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.ConfigurationUnit.DTO;
using InfraManager.BLL.DataEntities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ConfigurationUnit
{
    public class ConfigurationUnitProfile : Profile
    {
        public ConfigurationUnitProfile()
        {
            CreateMap<DAL.Configuration.ConfigurationUnit, ConfigurationUnitDetails>();
        }
    }
}
