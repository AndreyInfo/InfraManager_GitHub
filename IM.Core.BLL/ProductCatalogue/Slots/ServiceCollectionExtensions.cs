using Inframanager.BLL;
using InfraManager.BLL.ProductCatalogue.SlotTemplates;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using Microsoft.Extensions.DependencyInjection;

namespace InfraManager.BLL.ProductCatalogue.Slots;

public static class ServiceCollectionExtensions
{
    internal static IServiceCollection RegisterSlotServices(this IServiceCollection collection)
    {
        collection.AddScoped(typeof(ISlotBaseBLL<,,,>), typeof(SlotBaseBLL<,,,>));

        collection.AddScoped(typeof(IBuildEntityQuery<Slot, SlotDetails, SlotBaseFilter>), typeof(SlotBaseQueryBuilder<Slot, SlotDetails>));
        collection.AddScoped(typeof(IBuildEntityQuery<SlotTemplate, SlotTemplateDetails, SlotBaseFilter>), typeof(SlotBaseQueryBuilder<SlotTemplate, SlotTemplateDetails>));

        collection.AddScoped(typeof(IModifyObject<Slot, SlotData>), typeof(SlotBaseModifier<Slot, SlotData>));
        collection.AddScoped(typeof(IModifyObject<SlotTemplate, SlotTemplateData>), typeof(SlotBaseModifier<SlotTemplate, SlotTemplateData>));

        collection.AddScoped(typeof(IFinder<Slot>), typeof(SlotBaseFinder<Slot>));
        collection.AddScoped(typeof(IFinder<SlotTemplate>), typeof(SlotBaseFinder<SlotTemplate>));

        collection.AddScoped(typeof(IBuildObject<SlotTemplate, SlotTemplateData>), typeof(SlotBaseBuilder<SlotTemplate, SlotTemplateData>));
        collection.AddScoped(typeof(IBuildObject<Slot, SlotData>), typeof(SlotBaseBuilder<Slot, SlotData>));

        return collection;
    }
}