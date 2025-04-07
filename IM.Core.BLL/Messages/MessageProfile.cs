using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.BLL.Extensions;
using InfraManager.DAL.Message;
using InfraManager.DAL.ServiceDesk;
using InfraManager.WebApi.Contracts.Models.EMailProtocol;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Messages
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<MessageByEmailData, MessageByEmail>()
                .ConstructUsing((data, conext) =>
                {
                    Message message = new Message()
                    {
                    };
                    return new MessageByEmail(message);
                })
                .IgnoreNulls()
                ;

            CreateMap<MessageByEmail, MessageByEmailDetails>()
                .ForMember(dst => dst.EntityStateID, x => x.MapFrom(src => src.Message.EntityStateID))
                .ForMember(dst => dst.ID, x => x.MapFrom(src => src.Message.IMObjID))
                .ForMember(dst => dst.EntityStateName, x => x.MapFrom(src => src.Message.EntityStateName))
                .ForMember(dst => dst.WorkflowSchemeID, x => x.MapFrom(src => src.Message.WorkflowSchemeID))
                .ForMember(dst => dst.WorkflowSchemeIdentifier, x => x.MapFrom(src => src.Message.WorkflowSchemeIdentifier))
                .ForMember(dst => dst.WorkflowSchemeVersion, x => x.MapFrom(src => src.Message.WorkflowSchemeVersion))
                ;


            CreateMap<Message, MessageDetails>()
                .ForMember(dst => dst.TypeName, m=> m.MapFrom(scr => EnumExtensions.GetNameFromFriendlyName((MessageTypeEnum)scr.Type)))
                .ForMember(dst => dst.SeverityName, m=> m.MapFrom(scr => EnumExtensions.GetNameFromFriendlyName((SeverityType)scr.SeverityType)));

            CreateMap<MessageData, Message>()
                .ForMember(dst=>dst.IMObjID, opt=>opt.MapFrom(src=>src.ID))
                .IgnoreNulls()
                ;

            CreateMap<NotificationReceiver, NotificationReceiverDetails>()
                ;

        }
    }
}
