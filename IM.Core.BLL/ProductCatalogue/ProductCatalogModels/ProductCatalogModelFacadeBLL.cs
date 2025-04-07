using InfraManager.BLL.ProductCatalogue.Models;
using InfraManager.BLL.ProductCatalogue.ProductClass;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.DAL.ProductCatalogue.ProductCatalogTypes;
using InfraManager.DAL.ProductCatalogue.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels;

internal class ProductCatalogModelFacadeBLL : IProductCatalogModelBLLFacade
    , ISelfRegisteredService<IProductCatalogModelBLLFacade>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductClassBLL _productClassBLL;
    private readonly IReadonlyRepository<ProductCatalogTemplate> _templates;
    private readonly IProductCatalogTypeTemplateIDQuery _typeTemplateIDQuery;
    private readonly IServiceMapper<ObjectClass, IProductCatalogModelBLL> _serviceMapper;
    private readonly IClientSideFilterer<ProductModelOutputDetails, ProductModelColumns> _clientSideFilterer;
    private readonly ObjectClass[] objectModelClasses = new ObjectClass[]
    {
        ObjectClass.TerminalDeviceModel,
        ObjectClass.AdapterModel,
        ObjectClass.PeripherialModel,
        ObjectClass.NetworkDeviceModel,
        ObjectClass.SoftwareLicenseModel,
        ObjectClass.MaterialModel,
        ObjectClass.CabinetType,
    };

    public ProductCatalogModelFacadeBLL(IUnitOfWork unitOfWork
        , IProductClassBLL productClassBLL
        , IReadonlyRepository<ProductCatalogTemplate> templates
        , IProductCatalogTypeTemplateIDQuery typeTemplateIDQuery
        , IServiceMapper<ObjectClass, IProductCatalogModelBLL> serviceMapper
        , IClientSideFilterer<ProductModelOutputDetails, ProductModelColumns> clientSideFilterer)
    {
        _unitOfWork = unitOfWork;
        _productClassBLL = productClassBLL;
        _templates = templates;
        _typeTemplateIDQuery = typeTemplateIDQuery;
        _serviceMapper = serviceMapper;
        _clientSideFilterer = clientSideFilterer;
    }


    public async Task<ProductModelOutputDetails> GetAsync(Guid id
        , ProductTemplate templateID
        , CancellationToken cancellationToken = default)
    {
        var bll = await GetModelBLLByProductTemplateAsync(templateID, cancellationToken);

        var model = await bll.DetailsAsync(id, cancellationToken);

        InitAdditional(model);

        return model;
    }

    public async Task<ProductModelOutputDetails> InsertAsync(ProductCatalogModelData data,
        CancellationToken cancellationToken = default)
    {
        var template = await _typeTemplateIDQuery.ExecuteAsync(data.ProductCatalogTypeID, cancellationToken)
                       ?? throw new ObjectNotFoundException($"Не найден тип с ID={data.ProductCatalogTypeID}");

        var bll = await GetModelBLLByProductTemplateAsync(template, cancellationToken);

        var model = await bll.AddAsync(data, cancellationToken);
        InitAdditional(model);
        return model;
    }


    public async Task<ProductModelOutputDetails> UpdateAsync(Guid id, ProductTemplate template,
        ProductCatalogModelData data,
        CancellationToken cancellationToken = default)
    {
        var bll = await GetModelBLLByProductTemplateAsync(template, cancellationToken);

        var model = await bll.UpdateAsync(id, data, cancellationToken);
        InitAdditional(model);
        return model;
    }

    public async Task<ProductModelOutputDetails[]> GetModelsAsync(ProductCatalogModelFilter filter, CancellationToken cancellationToken = default)
    {
        var models = await GetModelsByTypeIDAsync(filter.GetTreeFilter(), cancellationToken);

        return await _clientSideFilterer.GetPaggingAsync(models
            , filter
            , c => c.Name.ToLower().Contains(filter.SearchString.ToLower())
                    || (c.Note is not null 
                            && c.Note.ToLower().Contains(filter.SearchString.ToLower()))
                    || (c.VendorName is not null 
                            && c.VendorName.ToLower().Contains(filter.SearchString.ToLower()))
            , cancellationToken);
    }

    private async Task<ProductModelOutputDetails[]> GetModelsByTypeIDAsync(ProductCatalogTreeFilter filter, CancellationToken cancellationToken)
    {
        var result = new List<ProductModelOutputDetails>();
        foreach (var modelClassID in objectModelClasses) 
        {
            var bll = GetModelBLL(modelClassID);

            var models = await bll.GetDetailsArrayAsync(filter, cancellationToken);

            models.ForEach(c => InitAdditional(c));
            result.AddRange(models);
        }
        return result.ToArray();
    }

    public async Task DeleteAsync(ProductTemplate template, Guid id, ProductCatalogDeleteFlags flags, CancellationToken token = default)
    {
        var bll = await GetModelBLLByProductTemplateAsync(template, token);

        await bll.DeleteByFlagsAsync(id, flags, token);
    }

    private async Task<IProductCatalogModelBLL> GetModelBLLByProductTemplateAsync(ProductTemplate templateClass, CancellationToken cancellationToken)
    {
        var objectClass = await GetObjectClassByProductTemplateAsync(templateClass, cancellationToken);

        return GetModelBLLByObjectCLassID(objectClass);
    }

    private async Task<ObjectClass> GetObjectClassByProductTemplateAsync(ProductTemplate templateID,
    CancellationToken cancellationToken)
    {
        var template = await _templates.FirstOrDefaultAsync(x => x.ID == templateID, cancellationToken)
            ?? throw new ObjectNotFoundException<ProductTemplate>(templateID, $"Не найден шаблон продукта с ID = {templateID}");

        return template.ClassID;
    }

    private IProductCatalogModelBLL GetModelBLLByObjectCLassID(ObjectClass objectClass)
    {
        var modelClass = _productClassBLL.GetModelClassByProductClass(objectClass)
            ?? throw new ObjectNotFoundException($"Не найден класс объектов для модели с ID {(int)objectClass}");

        return GetModelBLL(modelClass);
    }

    private IProductCatalogModelBLL GetModelBLL(ObjectClass modelClass)
    {
        if (!_serviceMapper.HasKey(modelClass))
            throw new InvalidObjectException("Указанный класс не поддерживается");

        return _serviceMapper.Map(modelClass);
    }

    private void InitAdditional(ProductModelOutputDetails output)
    {
        output.ModelClassID = _productClassBLL.GetModelClassByProductClass(output.TemplateClassID);
    }


    public async Task<bool> IsUseTypeAsync(ProductCatalogModelDeleteFilter filter, CancellationToken cancellationToken)
    {
        foreach (var modelClassID in objectModelClasses)
        {
            var bll = GetModelBLL(modelClassID);
            var models = await bll.GetDetailsArrayAsync(filter.ToTreeFilter(), cancellationToken);
            if(models.Any()) 
                return true;
        }
 
        return false;
    }

    public async Task DeleteModelsByFilterAsync(ProductCatalogModelDeleteFilter filter, bool withObject, CancellationToken cancellationToken)
    {
        var treeFilter = filter.ToTreeFilter();
        foreach (var modelClassID in objectModelClasses)
        {
            var bll = GetModelBLL(modelClassID);

            await bll.DeleteByFilterAsync(treeFilter, withObject, cancellationToken);
        }

        await _unitOfWork.SaveAsync(cancellationToken);
    }


    public async Task<ProductModelOutputDetails[]> GetModelsWithoutTTZAsync(ProductCatalogModelFilter filter, CancellationToken cancellationToken = default)
    {
        var result = new List<ProductModelOutputDetails>();

        foreach (var modelClassID in objectModelClasses)
        {
            var bll = GetModelBLL(modelClassID);

            var models = await bll.GetDetailsArrayWithoutTTZAsync(filter.GetTreeFilter(), cancellationToken);

            models.ForEach(c => InitAdditional(c));
            result.AddRange(models);
        }

        return await _clientSideFilterer.GetPaggingAsync(result.ToArray()
            , filter
            , c => c.Name.ToLower().Contains(filter.SearchString.ToLower())
                    || (c.Note is not null
                            && c.Note.ToLower().Contains(filter.SearchString.ToLower()))
                    || (c.VendorName is not null
                            && c.VendorName.ToLower().Contains(filter.SearchString.ToLower()))
            , cancellationToken);
    }

    public async Task<ProductModelOutputDetails> GetWithoutTTZAsync(Guid id, ProductTemplate modelClassID, CancellationToken cancellationToken = default)
    {
        var bll = await GetModelBLLByProductTemplateAsync(modelClassID, cancellationToken);

        var model = await bll.DetailsWithoutTTZAsync(id, cancellationToken);

        InitAdditional(model);

        return model;
    }

    public async Task<ProductModelOutputDetails> UpdateWithoutTTZAsync(Guid id, ProductTemplate modelClassID, ProductCatalogModelData data, CancellationToken cancellationToken = default)
    {
        var bll = await GetModelBLLByProductTemplateAsync(modelClassID, cancellationToken);

        var model = await bll.UpdateWithoutTTZAsync(id, data, cancellationToken);

        InitAdditional(model);

        return model;
    }
}