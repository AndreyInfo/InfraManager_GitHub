using System;

namespace InfraManager.DAL.Import.ITAsset;
public class ITAssetImportCSVConfigurationClassConcordance : ITAssetImportCSVConfigurationConcordance
{
    protected ITAssetImportCSVConfigurationClassConcordance() { }

    public ITAssetImportCSVConfigurationClassConcordance(Guid itAssetImportCSVConfigurationID, string field, string expression)
    {
        ITAssetImportCSVConfigurationID = itAssetImportCSVConfigurationID;
        Field = field;
        Expression = expression;
    }
}
