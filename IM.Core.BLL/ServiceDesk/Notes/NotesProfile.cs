using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Notes
{
    public class NotesProfile : Profile
    {
        public NotesProfile()
        {
            CreateMap<NoteQueryResultItem, NoteDetails>()
                .ForMember(dst => dst.UserID, m => m.MapFrom(src => src.NoteEntity.UserID))
                .ForMember(dst => dst.ID, m => m.MapFrom(src => src.NoteEntity.ID))
                .ForMember(dst => dst.Type, m => m.MapFrom(src => src.NoteEntity.Type))
                .ForMember(dst => dst.Message, m => m.MapFrom(src => src.NoteEntity.HTMLNote))
                .ForMember(dst => dst.UserName, m => m.MapFrom(src => src.UserName))
                .ForMember(dst => dst.UtcDate, m => m.MapFrom(src => src.NoteEntity.UtcDate));

            CreateMap<NoteData, Note>()
                .ForMember(dst => dst.NoteText, m => m.MapFrom(src => src.Message.RemoveHtmlTags()))
                .ForMember(dst => dst.Type, m => m.MapFrom(src => src.Type))
                .ForMember(dst => dst.HTMLNote, m => m.MapFrom(src => src.Message))
                .IgnoreNulls();
        }
    }
}
