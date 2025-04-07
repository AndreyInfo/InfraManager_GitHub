using AutoMapper;
using InfraManager.DAL.Import;
using InfraManager.ServiceBase.ImportService.Log;

namespace IM.Core.Import.BLL
{
    internal class LoggerProfile : Profile
    {
        public LoggerProfile()
        {
            CreateMap<MainLog, LogTask>();
        }
    }
}
