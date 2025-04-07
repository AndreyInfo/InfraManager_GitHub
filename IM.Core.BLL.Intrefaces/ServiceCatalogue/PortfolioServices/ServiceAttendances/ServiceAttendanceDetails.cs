using InfraManager.DAL.ServiceCatalogue;
using System;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceAttendances;

public class ServiceAttendanceDetails : PortfolioServcieItemDetails
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

    public string ServiceName { get; init; }

    public Guid? FormID { get; init; }
}
