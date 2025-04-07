using InfraManager.BLL.Asset;
using System;

namespace InfraManager.BLL.ReportsForCommand;

public class ReportForCommandData
{
    public Guid ReportID { get; set; }
    public OperationType OperationType { get; set; }
}