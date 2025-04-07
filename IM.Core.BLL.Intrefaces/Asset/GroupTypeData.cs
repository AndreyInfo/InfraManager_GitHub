namespace InfraManager.BLL.Asset;

public class GroupTypeData
{
    public GroupTypeData(bool isCall, bool isWorkOrder, bool isProblem, bool isChangeRequest, bool isMassiveIncident)
    {
        IsCall = isCall;
        IsWorkOrder = isWorkOrder;
        IsProblem = isProblem;
        IsChangeRequest = isChangeRequest;
        IsMassiveIncident = isMassiveIncident;
    }

    public bool IsCall { get; init; }
    public bool IsWorkOrder { get; init; }
    public bool IsProblem { get; init; }
    public bool IsChangeRequest { get; init; }
    public bool IsMassiveIncident { get; init; }
}