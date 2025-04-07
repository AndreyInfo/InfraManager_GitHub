using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.DataEntities.DTO;
using InfraManager.DAL.ConfigurationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.DataEntities
{
    public class DataEntityProfile : Profile
    {
        public DataEntityProfile()
        {
            CreateMap<DataEntity, DataEntityDetails>();
        }
    }
}
