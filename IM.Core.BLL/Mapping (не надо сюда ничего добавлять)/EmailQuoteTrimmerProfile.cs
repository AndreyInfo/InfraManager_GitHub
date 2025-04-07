using AutoMapper;
using InfraManager.BLL.Settings;
using InfraManager.DAL.Settings;

namespace InfraManager.BLL.Mapping
{
    public class EmailQuoteTrimmerMap:Profile
    {
        public EmailQuoteTrimmerMap()
        {
            CreateMap<EmailQuoteTrimmer, EmailQuoteTrimmerData>().ReverseMap();
        }
    }
}
