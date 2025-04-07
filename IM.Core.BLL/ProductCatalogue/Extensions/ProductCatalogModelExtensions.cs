using Inframanager.BLL;
using InfraManager.BLL.ProductCatalogue.Models;
using InfraManager.BLL.ProductCatalogue.ProductCatalogModels;
using InfraManager.DAL.ProductCatalogue.Tree;
using InfraManager.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InfraManager.BLL.ProductCatalogue.Extensions;

public static class ProductCatalogModelExtensions
{
    public static IServiceCollection RegisterModels(this IServiceCollection collection,
        params (Type type, ObjectClass classID)[] datum)
    {
        var mapping = new ServiceMapping<ObjectClass, IProductCatalogModelBLL>();
        var mappingType = mapping.GetType();
        var keyMappingType = typeof(ServiceMapping<ObjectClass, IProductCatalogModelBLL>.KeyMapping);
        var genericServiceType = typeof(ProductCatalogModelBLL<>);
        foreach (var type in datum)
        {
            var keyMapping = RunMap(genericServiceType, type.type, mappingType, mapping);

            mapping = RunTo(keyMappingType, type.classID, keyMapping);
        }

        collection.AddMappingScoped(mapping);

        foreach (var data in datum)
        {
            AddLoader(collection, data.type);
            AddBuildQuery(collection, data.type);
            AddDetailsBuilder(collection, data.type);
        }

        return collection;
    }

    private static ServiceMapping<ObjectClass, IProductCatalogModelBLL> RunTo(Type keyMappingType, ObjectClass classID,
        ServiceMapping<ObjectClass, IProductCatalogModelBLL>.KeyMapping keyMapping)
    {
        ServiceMapping<ObjectClass, IProductCatalogModelBLL> mapping;
        var keyMethod = keyMappingType.GetMethod("To");


        return (ServiceMapping<ObjectClass, IProductCatalogModelBLL>) keyMethod!.Invoke(keyMapping,
            new object[] {classID});
    }

    private static ServiceMapping<ObjectClass, IProductCatalogModelBLL>.KeyMapping RunMap(Type genericServiceType, Type type, Type mappingType, ServiceMapping<ObjectClass, IProductCatalogModelBLL> mapping)
    {
        var serviceType = genericServiceType.MakeGenericType(type);

        var mapGenericMethod = mappingType.GetMethod("Map", new Type[] { });

        var mapMethod = mapGenericMethod!.MakeGenericMethod(serviceType);

        var keyMapping =
            (ServiceMapping<ObjectClass, IProductCatalogModelBLL>.KeyMapping) mapMethod.Invoke(mapping, new object[] { });
        return keyMapping;
    }

    private static void AddBuildQuery(IServiceCollection collection, Type type)
    {
        var serviceType = typeof(IBuildEntityQuery<,,>).MakeGenericType(type, typeof(ProductModelOutputDetails),
            typeof(ProductCatalogTreeFilter));
        var implementationType = typeof(ProductModelByTreeFilterQueryBuilder<>).MakeGenericType(type);
        collection.AddScoped(serviceType, implementationType);
    }

    private static void AddLoader(IServiceCollection collection, Type type)
    {
        var serviceType =
            typeof(ILoadEntity<,,>).MakeGenericType(typeof(Guid), type, typeof(ProductModelOutputDetails));

        var implementationType = typeof(ProductModelLoader<>).MakeGenericType(type);

        collection.AddScoped(serviceType, implementationType);
    }

    private static void AddDetailsBuilder(IServiceCollection collection, Type type)
    {
        var serviceType =
            typeof(IBuildObject<,>).MakeGenericType(typeof(ProductModelOutputDetails), type);

        var implementationType = typeof(ProductCatalogModelBuilder<>).MakeGenericType(type);

        collection.AddScoped(serviceType, implementationType);
    }
}