using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.ProductCatalogue.Tree;
using System;

namespace InfraManager.BLL.ProductCatalogue.Models;

public class ProductCatalogModelFilter : BaseFilter
{
    public Guid? ProductCatalogTypeID { get; init; }

    public Guid? ProductCatalogCategoryID { get; init; }

    public ProductCatalogTreeFilter GetTreeFilter() => new ProductCatalogTreeFilter()
    {
        ParentID = ProductCatalogTypeID,
        AvailableCategoryID = ProductCatalogCategoryID,
    };

}