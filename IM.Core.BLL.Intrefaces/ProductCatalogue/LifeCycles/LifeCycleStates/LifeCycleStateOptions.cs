namespace InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates;

public class LifeCycleStateOptions
{
    public LifeCycleStateOptions(bool isWrittenOff
        , bool isApplied
        , bool isInRepair
        , bool canCreateAgreement
        , bool canIncludeInPurchase
        , bool canIncludeInActiveRequest
        , bool canIncludeInInfrastructure
        )
    {
        IsApplied = isApplied;
        IsInRepair = isInRepair;
        IsWrittenOff = isWrittenOff;
        CanCreateAgreement        = canCreateAgreement;
        CanIncludeInPurchase      = canIncludeInPurchase;
        CanIncludeInActiveRequest = canIncludeInActiveRequest;
        CanIncludeInInfrastructure = canIncludeInInfrastructure;
    }

    public bool IsWrittenOff { get; init; }
    public bool IsApplied { get; init; }
    public bool IsInRepair { get; init; }
    public bool CanIncludeInActiveRequest { get; init; }
    public bool CanIncludeInPurchase { get; init; }
    public bool CanCreateAgreement { get; init; }
    public bool CanIncludeInInfrastructure { get; init; }
}
