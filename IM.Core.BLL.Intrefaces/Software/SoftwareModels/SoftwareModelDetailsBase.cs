using InfraManager.BLL.Software.SoftwareModels.CommonDetails;
using InfraManager.DAL.Software;
using System;

namespace InfraManager.BLL.Software.SoftwareModels;

public class SoftwareModelDetailsBase
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public SoftwareModelTemplate TemplateID { get; init; }
    public string TemplateText { get; init; }
    public string Version { get; init; }
    public Guid ManufacturerID { get; init; }
    public ManufacturerDetails Manufacturer { get; set; }
    public Guid SoftwareTypeID { get; init; }
    public SoftwareTypeDetails SoftwareType { get; init; }
    public string Note { get; init; }
    public string Code { get; init; }
    public string ExternalID { get; init; }
}
