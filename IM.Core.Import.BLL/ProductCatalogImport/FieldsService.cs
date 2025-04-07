using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using InfraManager;

namespace IM.Core.Import.BLL.Import.Importer;

public class FieldsService : IFieldsService,ISelfRegisteredService<IFieldsService>
{
    private readonly HashSet<string> _fields = new()
    {
        CommonFieldNames.ModelExternalID,
        CommonFieldNames.ModelName,
        CommonFieldNames.ModelDescription,
        CommonFieldNames.ModelProductName,
        CommonFieldNames.ModelCanBuy,
        CommonFieldNames.TypeExternalID,
        CommonFieldNames.TypeExternalName,
        CommonFieldNames.UnitExternalID,
        CommonFieldNames.UnitName,
        CommonFieldNames.ManufacturerExternalID,
        CommonFieldNames.ManufacturerName
    };

    public IReadOnlyCollection<string> GetCommonFields() => _fields;
    
}