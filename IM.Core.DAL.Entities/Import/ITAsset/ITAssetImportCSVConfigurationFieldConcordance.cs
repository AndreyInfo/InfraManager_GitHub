using System;

namespace InfraManager.DAL.Import.ITAsset;
public class ITAssetImportCSVConfigurationFieldConcordance : ITAssetImportCSVConfigurationConcordance
{
    protected ITAssetImportCSVConfigurationFieldConcordance() { }

    public ITAssetImportCSVConfigurationFieldConcordance(Guid itAssetImportCSVConfigurationID, string field, string expression)
    {
        ITAssetImportCSVConfigurationID = itAssetImportCSVConfigurationID;
        Field = field;
        Expression = expression;
    }
}
