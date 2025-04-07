using System;

namespace InfraManager.DAL.Software;
public class SoftwareModelDetailsItem
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public SoftwareModelTemplate TemplateID { get; init; }
    public string TemplateText { get; init; }
    public string Version { get; init; }
    public Guid ManufacturerID { get; init; }
    public Guid SoftwareTypeID { get; init; }
    public string Note { get; init; }
    public string Code { get; init; }
    public string ExternalID { get; init; }
    public DateTime? SupportDate { get; init; }
    public DateTime CreateDate { get; init; }
    public string SoftwareModelUsingTypeName { get; init; }
    public bool IsCommercial { get; init; }
    public string ProcessNames { get; init; }
    public string ModelRedaction { get; init; }
    public string ModelDistribution { get; init; }
}
