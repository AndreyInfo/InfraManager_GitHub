using System;
using System.Collections.Generic;
using System.Linq;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.DAL;

public interface IProductModel : IGloballyIdentifiedEntity
{
    ProductCatalogType ProductCatalogType { get; }

    byte[] RowVersion { get; }

    Guid ProductCatalogTypeID { get; }

    string Name { get; }

    string ExternalID { get; }

    string Note { get; }
}

public interface IProductModel<TManufacturerID> : IProductModel
    where TManufacturerID : struct
{
    TManufacturerID ManufacturerID { get; }
}