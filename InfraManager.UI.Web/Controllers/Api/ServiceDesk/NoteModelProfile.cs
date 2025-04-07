using AutoMapper;
using InfraManager.BLL.ServiceDesk;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk
{
    public class NoteModelProfile : Profile
    {
        public NoteModelProfile()
        {
            CreateMap<NoteDetails, NoteListItemModel>()
                .ForMember(dst => dst.DateForJs, m => m.MapFrom(src => src.UtcDate.ConvertToMillisecondsAfterMinimumDate()))
                ;
            CreateMap<NoteModel, NoteData>()
                ;
        }
    }
}
