using InfraManager.DAL.ServiceCatalogue;

namespace IM.Core.Import.BLL.Interface.Import.ServiceCatalogue;
public class ImportSCData
{
    public List<SCImportDetail> ValidModelsWithoutExternalID { get; set; }
    public List<SCImportDetail> ValidModelsWithExternalID { get; set; }
    public Service[] FindServicesWithExternalIDByExternalID { get; set; }
    public Service[] FindServicesWithoutExternalIDByCategoryAndNameWithoutExternalID { get; set; }
    public Service[] FindServicesWithoutExternalIDByCategoryAndNameWithExternalID { get; set; }
    public Service[] FindServicesWithExternalIDByCategoryAndNameWithoutExternalID { get; set; }

}
