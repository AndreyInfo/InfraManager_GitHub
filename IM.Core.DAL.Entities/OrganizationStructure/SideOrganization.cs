using System;

namespace InfraManager.DAL.OrganizationStructure;

public class SideOrganization
{
    public Guid ID { get; init; }

    public string Name { get; init; }

    public string Phone { get; init; }

    public string Fax { get; init; }

    public string Mail { get; init; }

    public string Note { get; init; }

    public byte[] RowVersion { get; init; }

    public Guid ComplementaryID { get; init; }
}