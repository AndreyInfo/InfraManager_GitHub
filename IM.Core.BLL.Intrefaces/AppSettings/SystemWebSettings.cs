namespace InfraManager.BLL.AppSettings;

public class SystemWebSettings
{
    public bool LdapAuthentication { get; init; }
    public bool HardToChooseButtonVisible { get; init; }
    public bool LoginHashAuthentication { get; init; }
    public bool LoginAuthentication { get; init; }
    public bool LoginPasswordAuthentication { get; init; }
    public bool VisibleNotAvailableServiceBySla { get; init; }
    
    public string ImagePathBrowser { get; init; }
    public string ImagePathMenu { get; init; }
    public string ImagePathLogin { get; init; }
    public bool WorkOrderRegistrationUseTypeSelectionDialog { get; init; }
}