using System;
using AutoMapper;
using InfraManager.BLL.Asset;
using InfraManager.DAL.ServiceDesk;
using InfraManager.Core.Helpers;

namespace InfraManager.BLL.ServiceDesk
{
    public class PriorityProfile : Profile
    {
        public string DefaultColor = "#f2fafd";
        public PriorityProfile()
        {
            CreateMap<Priority, PriorityDetailsModel>()
                .ForMember(dst => dst.ID, m => m.MapFrom(scr =>scr.ID))
                .ForMember(dst => dst.RowVersion, m => m.MapFrom(scr => Convert.ToBase64String(scr.RowVersion)))
                .ForMember(dst => dst.Color, m => m.MapFrom(scr => "#" + (scr.Color == 0 ? -1: scr.Color).ColorFromArgb().ToHtmlColor()));

            CreateMap<PriorityDetailsModel, Priority>() .ForMember(
                dst => dst.Color,
                m => m.MapFrom(                                                       // TODO исправить потом копипаст, используется эта модель тк нужен идентификатор и сущностей может быть > 1
                    scr => (!string.IsNullOrEmpty(scr.Color) ? scr.Color : DefaultColor) // TODO: Это логика предааставления данных. очевидно должно быть в Presentation Layer, а не в BusinessLayer
                        .ParseFromString()
                        .ToArgb()));
            
            
            CreateMap<PriorityModel, Priority>()
                .ConstructUsing(model => new Priority(model.Name, model.Sequence))
                .ForMember(dst => dst.RowVersion, m => m.MapFrom(scr => Convert.FromBase64String(scr.RowVersion)))                
                .ForMember(
                    dst => dst.Color,
                    m => m.MapFrom(
                        scr => (!string.IsNullOrEmpty(scr.Color) ? scr.Color : DefaultColor) // TODO: Это логика предааставления данных. очевидно должно быть в Presentation Layer, а не в BusinessLayer
                            .ParseFromString()
                            .ToArgb()));
        }
    }
}
