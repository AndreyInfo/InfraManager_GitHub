using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL.ProductCatalogue.LifeCycles;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates;

internal sealed class LifeCycleStateColumnMapperSettings
    : IColumnMapperSetting<LifeCycleState, LifeCycleStateColumns>
    , ISelfRegisteredService<IColumnMapperSetting<LifeCycleState, LifeCycleStateColumns>>
{
    public void Configure(IColumnMapperSettingsBase<LifeCycleState, LifeCycleStateColumns> configurer)
    {
        configurer.ShouldBe(c=> c.Options, x => x.IsInRepair)
            .Then(x => x.IsApplied)  
            .Then(x => x.IsWrittenOff)  
            .Then(x => x.CanCreateAgreement)
            .Then(x => x.CanIncludeInPurchase)
            .Then(x => x.CanIncludeInActiveRequest)
            .Then(x => x.CanIncludeInInfrastructure);
    }
}
