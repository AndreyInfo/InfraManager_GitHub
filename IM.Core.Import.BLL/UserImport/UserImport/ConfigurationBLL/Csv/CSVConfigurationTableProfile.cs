using AutoMapper;
using IM.Core.Import.BLL.Interface.Configurations.View;
using InfraManager.DAL.Import.CSV;

namespace IM.Core.Import.BLL.Import
{
    internal class CSVConfigurationTableProfile : Profile
    {
        public CSVConfigurationTableProfile()
        {
            CreateMap<UICSVConfiguration, CSVConfigurationTable>();
            CreateMap<UICSVConfiguration, ConfigurationCSVDetails>();
            CreateMap<ConfigurationCSVData, UICSVConfiguration>();

        }
    }
}
