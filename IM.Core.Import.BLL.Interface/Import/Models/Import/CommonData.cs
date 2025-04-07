using IM.Core.Import.BLL.Interface.Import.Models.SaveOrUpdateData;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using IM.Core.Import.BLL.Interface.Import.Models.UploadData;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using Inframanager.DAL.ProductCatalogue.Units;
using InfraManager.DAL.ProductCatalogue;

namespace IM.Core.Import.BLL.Interface.Import.Models.Import;

public interface ICommonData
{
    string ExternalID { get; set; }
    string Name { get; set; }
    string ModelNote { get; set; }
    string ModelProductNumber { get; set; }
    bool ModelCanBuy { get; set; }
    string TypeExternalID { get; set; }
    string TypeExternalName { get; set; }
    string UnitExternalID { get; set; }
    string UnitExternalName { get; set; }
    string ManufacturerExternalID { get; set; }
    string ManufacturerName { get; set; }
    string Parameters { get; set; }
}

public class CommonData : IImportEntity, ICommonData
{
    [FieldAssociateName(FieldName = nameof(IImportEntity.ExternalID), ClassName = nameof(IImportEntity))]
    [ImportField(Name = CommonFieldNames.ModelExternalID, Required = true)]
    public string ExternalID { get; set; }

    [FieldAssociateName(FieldName = nameof(IImportModel.Name), ClassName = nameof(IImportEntity))]
    [ImportField(Name = CommonFieldNames.ModelName, Required = true)]
    public string Name { get; set; }

    [FieldAssociateName(FieldName = nameof(IImportModel.Note), ClassName = nameof(IImportEntity))]
    [ImportField(Name = CommonFieldNames.ModelDescription)]
    public string ModelNote { get; set; }

    [FieldAssociateName(FieldName = nameof(IImportModel.ProductNumber), ClassName = nameof(IImportEntity))]
    [ImportField(Name = CommonFieldNames.ModelProductName)]
    public string ModelProductNumber { get; set; }

    [FieldAssociateName(FieldName = nameof(IImportModel.CanBuy), ClassName = nameof(IImportEntity))]
    [ImportField(Name = CommonFieldNames.ModelCanBuy)]
    public bool ModelCanBuy { get; set; }

    [FieldAssociateName(FieldName = nameof(ProductCatalogType.ExternalID), ClassName = nameof(ProductCatalogType))]
    [ImportField(Name = CommonFieldNames.TypeExternalID, Required = true)]
    public string TypeExternalID { get; set; }

    [FieldAssociateName(FieldName = nameof(ProductCatalogType.ExternalName), ClassName = nameof(ProductCatalogType))]
    [ImportField(Name = CommonFieldNames.TypeExternalName, Required = true)]
    public string TypeExternalName { get; set; }

    [FieldAssociateName(FieldName = nameof(Unit.ExternalID), ClassName = nameof(Unit))]
    [ImportField(Name = CommonFieldNames.UnitExternalID, Required = true)]
    public string UnitExternalID { get; set; }

    [FieldAssociateName(FieldName = nameof(Unit.Name), ClassName = nameof(Unit))]
    [ImportField(Name = CommonFieldNames.UnitName, Required = true)]
    public string UnitExternalName { get; set; }

    [FieldAssociateName(FieldName = nameof(Manufacturer.ExternalID), ClassName = nameof(Manufacturer))]
    [ImportField(Name = CommonFieldNames.ManufacturerExternalID, Required = false)]
    public string ManufacturerExternalID { get; set; }

    [FieldAssociateName(FieldName = nameof(Manufacturer.Name), ClassName = nameof(Manufacturer))]
    [ImportField(Name = CommonFieldNames.ManufacturerName, Required = true)]
    public string ManufacturerName { get; set; }

    public string Parameters { get; set; }
}