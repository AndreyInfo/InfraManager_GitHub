using AutoMapper;
using InfraManager.BLL.AutoMapper;
using WebUserSettingsEntity = InfraManager.DAL.Settings.WebUserSettings;

namespace InfraManager.BLL.Settings
{
    internal class WebUserSettingsProfile : Profile
    {
        public WebUserSettingsProfile()
        {
            CreateMap<WebUserSettingsEntity, WebUserSettings>()
                .ForMember(
                    details => details.AssetFiltrationField,
                    mapper => mapper.MapFrom(entity => entity.GetTreeSettings().FiltrationField))
                .ForMember(
                    details => details.AssetFiltrationObjectClassID,
                    mapper => mapper.MapFrom(entity => entity.GetTreeSettings().FiltrationObjectClassID))
                .ForMember(
                    details => details.AssetFiltrationObjectID,
                    mapper => mapper.MapFrom(entity => entity.GetTreeSettings().FiltrationObjectID))
                .ForMember(
                    details => details.AssetFiltrationObjectName,
                    mapper => mapper.MapFrom(entity => entity.GetTreeSettings().FiltrationObjectName))
                .ForMember(
                    details => details.AssetFiltrationTreeType,
                    mapper => mapper.MapFrom(entity => entity.GetTreeSettings().FiltrationTreeType))
                .ForMember(
                    details => details.ListView,
                    mapper => mapper.MapFrom(entity => new ListViewUserSettings
                    {
                        CompactMode = entity.ListViewCompactMode,
                        GridLines = entity.ListViewGridLines,
                        Multicolor = entity.ListViewMulticolor
                    }));

            CreateMap<WebUserSettings, WebUserSettingsEntity>().IgnoreNulls();
        }
    }
}
