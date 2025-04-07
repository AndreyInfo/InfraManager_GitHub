using System;

namespace InfraManager.DAL.Dashboards;

[ObjectClassMapping(ObjectClass.DevExpressDashboard)]
public class DashboardDevEx
{
    public DashboardDevEx(Guid dashboardID, string data)
    {
        DashboardID = dashboardID;
        Data = data;
    }

    public Guid DashboardID { get; set; }
    public string Data { get; set; }
    public virtual Dashboard Dashboard { get; }
}