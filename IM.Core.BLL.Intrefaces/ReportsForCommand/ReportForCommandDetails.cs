using InfraManager.BLL.Asset;
using System;

namespace InfraManager.BLL.ReportsForCommand;

public class ReportForCommandDetails
{
    public Guid? ReportID { get; set; }
    public string ReportName { get; set; }
    public OperationType OperationType { get; set; }
    public string StringFolder { get; set; }
}