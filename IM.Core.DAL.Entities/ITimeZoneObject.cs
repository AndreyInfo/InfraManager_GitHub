namespace InfraManager.DAL;

public interface ITimeZoneObject
{
    public string TimeZoneID { get; set; }

    public void ChangeTimeZone(string timeZoneID)
    {
        TimeZoneID = timeZoneID;
    }
}
