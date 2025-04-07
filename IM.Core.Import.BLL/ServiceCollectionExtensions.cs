using System.Reflection;
using IM.Core.Import.BLL.Import;
using IM.Core.Import.BLL.Import.Importer;
using IM.Core.Import.BLL.Import.Importer.DownloadData;
using IM.Core.Import.BLL.Import.Importer.UploadData;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.Models.DownloadData;
using IM.Core.Import.BLL.Interface.Import.Models.FieldUpdater;
using IM.Core.Import.BLL.Interface.Import.Models.Import;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using IM.Core.Import.BLL.Interface.Import.Models.UploadData;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue;
using Inframanager.DAL.ProductCatalogue.Units;
using InfraManager.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using IM.Core.Import.BLL.Interface.Import.OSU;
using IM.Core.Import.BLL.Import.OSU;
using IM.Core.Import.BLL.Interface.Import.Log;
using IM.Core.Import.BLL.Interface.Ldap.Import;
using IM.Core.Import.BLL.Ldap.Import;
using IM.Core.Import.BLL.OrganizationStructure;
using IM.Core.Import.BLL.UserImport.Log;
using Inframanager.DAL.ActiveDirectory.Import;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ITAsset;

namespace IM.Core.Import.BLL;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddImportBLL(this IServiceCollection services)
    {
        
        var thisAssembly = Assembly.GetExecutingAssembly();
        services.AddSelfRegisteredServices(thisAssembly);
        
        services.AddScoped(typeof(IModelFieldUpdater<,>), typeof(ModelFieldCommonDataUpdater<,>));
        services.AddScoped<IModelFieldUpdater<TmpModelData,AdapterType>,ModelFieldExtendedCommonDataUpdater<TmpModelData,AdapterType>>();
        services.AddScoped<IModelFieldUpdater<TmpModelData,PeripheralType>,ModelFieldExtendedCommonDataUpdater<TmpModelData,PeripheralType>>();
        
        services.AddScoped(typeof(IModelTypeIDUpdater<,>),typeof(ModelTypeIDUpdater<,>));
         services.AddScoped(typeof(ICommonPropertyData<,>), typeof(CommonPropertyData<,>));
        services.AddScoped(typeof(IEntityUpdater<,>), typeof(EntityFieldModelDataCommonDataUpdater<,>));
        services.AddScoped(typeof(IFieldUpdater<,>), typeof(EntityFieldModelDataCommonDataUpdater<,>));
        services.AddScoped(typeof(IRemoveQuery<,>), typeof(CommonRemoveQuery<,>));
        services.AddScoped<IModelSaver<TmpModelData>,ModelSaverFacade<TmpModelData>>();
         services.AddScoped(typeof(IBulkInsertOrUpdate<>), typeof(InsertOrUpdate<>));
         services.AddScoped(typeof(IBlockSaver<,>), typeof(DefaultBlockSaver<,>));
        services.AddScoped(typeof(IForeignKeyUpdater<,>),typeof(ModelForeignKeyUpdater<,>));
        services.AddScoped(typeof(ITypeDataUploader<>), typeof(TypeDataUploader<>));
        services.AddScoped(typeof(IBuildModel<,>), typeof(MapModelBuilder<,>));
        services.AddScoped(typeof(IUpdateQuery<,>), typeof(CommonUpdateQuery<,>));
        services.AddScoped(typeof(IInsertQuery<,>), typeof(CommonInsertQuery<,>));
        services
            .AddScoped<IBuildModel<UIADConfiguration, UIADConfigurationsOutputDetails>, UIADFieldConcoranceBuilder>();

        services.AddScoped<IUpdateQuery<UIADConfigurationsDetails, UIADConfiguration>, UIADConfigurationUpdateQuery>();
        services.AddScoped<IValidator<TmpModelData, ProductCatalogType>, ProductCatalogTypeDataValidator<TmpModelData>>()
                .AddScoped<IValidator<TmpModelData, Unit>, UnitsValidator<TmpModelData>>();

        services.AddScoped<IScriptDataParser<ConcordanceObjectType>, PythonScriptDataParser<ConcordanceObjectType>>()
                .AddScoped<IScriptDataParser<ConcordanceSCObjectType>, PythonScriptDataParser<ConcordanceSCObjectType>>()
                .AddScoped<IScriptDataParser<ConcordanceITAssetObjectType>, PythonScriptDataParser<ConcordanceITAssetObjectType>>();

        services.AddScoped<IInsertQuery<UIADConfigurationsDetails, UIADConfiguration>, UIADConfigurationInsertQuery>();

        services.AddScoped<IRemoveQuery<Guid, UIADConfiguration>, UIADConfigurationRemoveQuery>();

        services.AddScoped<IFinderQuery<UIADIMFieldConcordancesKey, UIADIMFieldConcordance>, UIADIMFieldConcordanceFinderQuery>();
        
        services.AddScoped<IValidator<TmpModelData>, ValidatorFacade<TmpModelData>>(x =>
            new ValidatorFacade<TmpModelData>(
                x.GetRequiredService<IValidator<TmpModelData, ProductCatalogType>>(),
                x.GetRequiredService<IValidator<TmpModelData, Unit>>())
                );
        
        services.AddMappingScoped(new ServiceMapping<ObjectClass, ISaver<TmpModelData>>()
                 .Map<EntitySaver<TmpModelData, Manufacturer>>().To(ObjectClass.Manufacturer)
                 .Map<EntitySaver<TmpModelData, AdapterType>>().To(ObjectClass.Adapter)
                 .Map<EntitySaver<TmpModelData,PeripheralType>>().To(ObjectClass.Peripherial)
                .Map<EntitySaver<TmpModelData,TerminalDeviceModel>>().To(ObjectClass.TerminalDevice)
              .Map<EntitySaver<TmpModelData,MaterialModel>>().To(ObjectClass.Material)
              .Map<EntitySaver<TmpModelData, Unit>>().To(ObjectClass.Unit)
            );
        services.SetImportForModel<AdapterType>()
            .SetImportForModel<PeripheralType>()
            .SetImportForModel<TerminalDeviceModel>()
            .SetImportForModel<MaterialModel>();
        
        
         services.AddScoped<IDataFactory<TmpModelData>, DataFactory>();

         services.AddScoped(typeof(IFinderQuery<,>), typeof(FinderQuery<,>));
         
        services.AddScoped(typeof(IEntitySaver<,>), typeof(EntitySaver<,>));
        services.AddScoped<ISaver<TmpModelData>, SaverChain<TmpModelData>>(x=>
            new SaverChain<TmpModelData>(
                x.GetRequiredService<IEntitySaver<TmpModelData,Manufacturer>>(),
                 x.GetRequiredService<IModelSaver<TmpModelData>>(),
                 x.GetRequiredService<IEntitySaver<TmpModelData,Unit>>()
            )
        );
        services.AddScoped<IImporter, Importer<TmpModelData>>();

        services.AddMappingScoped(
            new ServiceMapping<int, IImportBase>()
            .Map<ImportCSVBLL>().To(CommonFieldNames.CSVObjectType)
            .Map <ImportLDAPBLL>().To(CommonFieldNames.LDAPObjectType)
            .Map<ImportDBBLL>().To(CommonFieldNames.DBObjectType));

        services.AddScoped(typeof(IImportConnectorBase<,>), typeof(ImportConnectorBase<,>));

        services.AddScoped<IImportContext, ImportContext>();
        services.AddTransient(typeof(ILocalLogger<>), typeof(ImLocalLoggerAdapter<>));
        return services;
    }

    private static IServiceCollection SetImportForModel<TModel>(this IServiceCollection services)
    {
        services.AddScoped<IFieldUpdater<TmpModelData, TModel>, FieldUpdaterChain<TmpModelData, TModel>>(x =>
            new FieldUpdaterChain<TmpModelData, TModel>(
                x.GetRequiredService<IEntityUpdater<TmpModelData, TModel>>(),
                x.GetRequiredService<IModelFieldUpdater<TmpModelData, TModel>>(),
                x.GetRequiredService<IModelTypeIDUpdater<TmpModelData, TModel>>()
            ));
        services.AddScoped<IBlockSaver<TmpModelData, TModel>, ModelBlockSaver<TmpModelData, TModel>>();
        return services;
    }
}