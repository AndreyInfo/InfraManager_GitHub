using System;
using InfraManager.DAL.ServiceCatalogue;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceAttendances;

public class ServiceAttendanceData : PortfolioServcieItemData
{
    public string Parameter { get; init; }

    public bool Agreement { get; init; }

    public AttendanceType Type { get; init; }

    public Guid ServiceID { get; init; }

    public byte[] RowVersion { get; init; }

    public string WorkflowSchemeIdentifier { get; init; }

    public string Note { get; init; }

    public string ExternalID { get; init; }

    public string Summary { get; init; }

    public Guid CategoryID { get; init; }

    public Guid? FormID { get; init; }
}
