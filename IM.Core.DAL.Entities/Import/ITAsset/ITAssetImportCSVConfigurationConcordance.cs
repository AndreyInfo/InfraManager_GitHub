using System;

namespace InfraManager.DAL.Import.ITAsset;
public class ITAssetImportCSVConfigurationConcordance
{
    public Guid ITAssetImportCSVConfigurationID { get; set; }
    public string Field { get; init; }
    public string Expression { get; set; }
    public virtual ITAssetImportCSVConfiguration Configuration { get; init; }
}
