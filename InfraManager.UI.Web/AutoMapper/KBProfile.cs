using AutoMapper;
using InfraManager.BLL.KnowledgeBase;
using InfraManager.Web.BLL.SD.KB;
using System;

namespace InfraManager.UI.Web.AutoMapper
{
    public class KBProfile : Profile // TODO: Этого класса тут быть не должно
    {
        public KBProfile()
        {
            CreateMap<KbaInfo, KBArticleEditData>()
                .ForMember(dst => dst.Name, m => m.MapFrom(src => src.Name ?? string.Empty))
                .ForMember(dst => dst.HTMLDescription, m => m.MapFrom(src => string.Format(KBArticleEditData.DefaultDocument, src.HtmlDescription ?? string.Empty)))
                .ForMember(dst => dst.HTMLSolution, m => m.MapFrom(src => string.Format(KBArticleEditData.DefaultDocument, src.HtmlSolution ?? string.Empty)))
                .ForMember(dst => dst.HTMLAlternativeSolution, m => m.MapFrom(src => string.Format(KBArticleEditData.DefaultDocument, src.HtmlAltSolution ?? string.Empty)))
                .ForMember(dst => dst.Visible, m => m.MapFrom(src => src.AccessID == Guid.Empty))
                .ForMember(dst => dst.UtcDateValidUntil, m => m.MapFrom(src => !string.IsNullOrEmpty(src.DateValidUntil) ? DateTime.Parse(src.DateValidUntil) : (DateTime?)null));
        }
    }
}
