using AutoMapper;
using InfraManager.BLL.Settings;
using InfraManager.DAL.Settings;

namespace InfraManager.BLL.Mapping
{
    public class HtmlTagWorkerMap : Profile
    {
        public HtmlTagWorkerMap()
        {
            CreateMap<HtmlTagWorker, HtmlTagWorkerDetail>().ReverseMap();
        }
    }
}
