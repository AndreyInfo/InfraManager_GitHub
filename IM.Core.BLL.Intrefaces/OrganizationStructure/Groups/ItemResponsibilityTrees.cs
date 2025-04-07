using InfraManager.BLL.AccessManagement;

namespace InfraManager.BLL.OrganizationStructure.Groups;

public class ItemResponsibilityTrees
{
    public AccessElementsDetails[] DeviceCatalogueElements { get; set; }
    public AccessElementsDetails[] ServiceCatalogueElements { get; set; }
    public AccessElementsDetails[] TTZ_sksElements { get; set; }
    public AccessElementsDetails[] TOZ_sksElements { get; set; }
    public AccessElementsDetails[] TOZ_orgElements { get; set; }
}

