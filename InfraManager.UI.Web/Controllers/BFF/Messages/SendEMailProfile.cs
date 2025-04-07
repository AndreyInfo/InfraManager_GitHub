using AutoMapper;
using InfraManager.BLL.Messages;
using InfraManager.UI.Web.Models.Messages;
using InfraManager.Web.DTL.Repository;

namespace InfraManager.UI.Web.Controllers.BFF.Messages
{
    public class SendEMailProfile : Profile
    {
        public SendEMailProfile()
        {
            CreateMap<SendEMailRequest, SendEMailData>()
                .ForMember(dst => dst.ObjectID, opt => opt.MapFrom(src => src.KBArticleID))
                ;

            CreateMap<UploadFileInfo, SendEMailData.FileInfo>()
                ;
        }
    }
}
