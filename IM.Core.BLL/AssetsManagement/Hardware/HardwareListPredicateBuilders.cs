using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL;

namespace InfraManager.BLL.AssetsManagement.Hardware;

internal abstract class HardwareListPredicateBuilders<TEntity, TModelEntity, TListItem> :
    FilterBuildersAggregate<TEntity, TListItem>
    where TEntity : IProduct<TModelEntity>
    where TModelEntity : IProductModel
    where TListItem : IHardwareListItem 
{
    protected HardwareListPredicateBuilders()
    {
        AddPredicateBuilder(
            reportItem => reportItem.TypeName,
            product => product.Model.ProductCatalogTypeID);

        AddPredicateBuilder(
            reportItem => reportItem.ModelName,
            product => product.Model.IMObjID);

        AddPredicateBuilder(
            reportItem => reportItem.ProductCatalogTemplateName,
            product => product.Model.ProductCatalogType.ProductCatalogTemplateID);
    }
}