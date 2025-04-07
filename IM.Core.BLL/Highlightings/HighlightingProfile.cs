using AutoMapper;
using InfraManager.DAL.Highlightings;
using System.Collections.Generic;
using HighlightingEntity = InfraManager.DAL.Highlightings.Highlighting;

namespace InfraManager.BLL.Highlighting;

internal class HighlightingProfile : Profile
{
    public HighlightingProfile()
    {
        CreateMap<HighlightingEntity, HighlightingDetails>();

        CreateMap<HighlightingCondition, HighlightingConditionDetails>()
            .AfterMap((source, destination) => destination.GuidValues = new List<GuidDetail>());

        CreateMap<HighlightingConditionDetails, HighlightingCondition>();

        CreateMap<HighlightingData, HighlightingEntity>();

        CreateMap<HighlightingConditionData, HighlightingCondition>();
    }
}
