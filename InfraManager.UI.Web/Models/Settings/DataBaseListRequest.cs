namespace InfraManager.UI.Web.Models.Settings;

public class DataBaseListRequest
{
    public string ServerName { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string AdditionalField { get; set; }
    public int Port { get; set; }
}