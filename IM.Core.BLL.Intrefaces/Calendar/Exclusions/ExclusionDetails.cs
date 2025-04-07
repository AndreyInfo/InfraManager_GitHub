using InfraManager.DAL.Calendar;
using System;

namespace InfraManager.BLL.Calendar.Exclusions;

public class ExclusionDetails
{
    public Guid ID { get; init; }

    public string Name { get; init; }

    public ExclusionType Type { get; init; }

    public string TypeName { get; init; }

    public byte[] RowVersion { get; init; }
}
