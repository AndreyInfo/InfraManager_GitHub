using InfraManager.DAL.ProductCatalogue;
using System;

namespace InfraManager.BLL.ProductCatalogue.Models.AdditionalFields;

public class MaterialModelAdditionalFields : IProductCatalogModelProperties
{
    public MaterialModelAdditionalFields(MaterialModel materialModel)
    {
        Gost = materialModel.Gost;
        Cost = materialModel.Cost;
        UnitID = materialModel.UnitID;
        UnitName = materialModel.Unit.Name;
        CanBuy = materialModel.CanBuy;
    }

    public string Gost { get; init; }
    public decimal? Cost { get; init; }
    public Guid? UnitID { get; init; }
    public string UnitName { get; init; }
    public bool CanBuy { get; set; }
}
