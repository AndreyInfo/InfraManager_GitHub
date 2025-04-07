using InfraManager.BLL.ProductCatalogue.Models;
using InfraManager.DAL;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels;


internal class ProductObjectBLL<TEntity, TProductModel> :
    IProductObjectBLL
    where TEntity : class, IProduct<TProductModel>
    where TProductModel : class, IProductModel

{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<TEntity> _repository;

    public ProductObjectBLL(IUnitOfWork unitOfWork
        , IRepository<TEntity> repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task DeleteByModelIDAsync(Guid modelID, CancellationToken cancellationToken)
    {
        var objects = await _repository.ToArrayAsync(c => c.Model.IMObjID == modelID, cancellationToken);
        objects.ForEach(c=> _repository.Delete(c));
        await _unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task<bool> HasObjectsInModelAsync(Guid modelID, CancellationToken cancellationToken)
        => await _repository.AnyAsync(c => c.Model.IMObjID == modelID, cancellationToken);
}
