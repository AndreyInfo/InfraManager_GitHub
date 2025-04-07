using System;

namespace InfraManager.BLL.Location.Buildings;

public class BuildingData
{
    public string Name { get; init; }

    public string Index { get; init; }

    public string Region { get; init; }

    public string City { get; init; }

    public string Area { get; init; }

    public string Street { get; init; }

    public string HousePart { get; init; }

    public string Housing { get; init; }

    public string Note { get; init; }

    public Guid? SubdivisionID { get; init; }

    public Guid? OrganizationID { get; init; }

    public string House { get; init; }

    public string TimeZoneId { get; init; }

    public byte[] RowVersion { get; init; }

    public string ExternalID { get; init; }
}