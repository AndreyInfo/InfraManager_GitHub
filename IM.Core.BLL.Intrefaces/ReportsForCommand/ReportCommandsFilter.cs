using System;

namespace InfraManager.BLL.ReportsForCommand;

public class ReportCommandsFilter
{
    public string CommandName { get; set; }
    public Guid? ReportID { get; set; }
}