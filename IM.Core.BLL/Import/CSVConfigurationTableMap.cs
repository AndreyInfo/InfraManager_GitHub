using AutoMapper;
using InfraManager.DAL.Import.CSV;

namespace InfraManager.BLL.Import
{
    internal class CSVConfigurationTableMap : Profile
    {
        public CSVConfigurationTableMap()
        {
            CreateMap<CSVConfigurationTable, CSVConfigurationTableAPI>();
        }
    }
}
