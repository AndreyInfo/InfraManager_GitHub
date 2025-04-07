using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.DAL;
using InfraManager.Expressions;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.ServiceCatalogue.OperationalLevelAgreements;

//TODO: Выпилить копипасту
public class ManyToManyReferenceBLL<TParent, TKey> : IManyToManyReferenceBLL<TParent, TKey> where TParent : class
{
    private readonly ILogger<ManyToManyReferenceBLL<TParent, TKey>> _logger;
    private readonly IFinder<TParent> _repository;
    private readonly ICurrentUser _currentUser;
    private readonly IValidatePermissions<TParent> _permissionsValidator;

    public ManyToManyReferenceBLL(
        IFinder<TParent> repository,
        ILogger<ManyToManyReferenceBLL<TParent, TKey>> logger,
        ICurrentUser currentUser,
        IValidatePermissions<TParent> permissionsValidator)
    {
        _repository = repository;
        _logger = logger;
        _currentUser = currentUser;
        _permissionsValidator = permissionsValidator;
    }

    public async Task<ManyToMany<TParent, TReference>> AddReferenceAsync<TReference, TReferenceKey>(
        Expression<Func<TParent, ICollection<ManyToMany<TParent, TReference>>>> collectionExpression,
        TKey id,
        Func<TReferenceKey, Task<TReference>> finder,
        TReferenceKey referenceID,
        CancellationToken cancellationToken = default) where TReference : class
    {
        _logger.LogTrace(
            $"User (ID = {_currentUser.UserId}) is adding {typeof(TReference).Name} (ID = {referenceID}) to {typeof(TParent).Name} (ID = {id}).");
        await ValidatePermissionAsync(cancellationToken);
        
        var referencedObject = await GetReferencedObjectAsync(id, collectionExpression, cancellationToken);
        var referencedEntity = await finder(referenceID) ??
                               throw new InvalidObjectException($"{typeof(TReference).Name} is either removed or not found.");

        var collection = collectionExpression.Compile()(referencedObject);
        var reference = new ManyToMany<TParent, TReference>(referencedEntity);
        collection.Add(reference);
        
        _logger.LogInformation(
            $"User (ID = {_currentUser.UserId}) successfully added {typeof(TReference).Name} (ID = {referenceID}) to {typeof(TParent).Name} (ID = {id})");

        return reference;
    }

    public async Task RemoveReferenceAsync<TReference, TReferenceKey>(
        TKey id,
        TReferenceKey referenceID,
        Expression<Func<TParent, ICollection<ManyToMany<TParent, TReference>>>> collectionExpression,
        Func<TReference, TReferenceKey> referenceIDAccessor,
        CancellationToken cancellationToken = default) where TReference : class where TReferenceKey : struct
    {
        _logger.LogTrace(
            $"User (ID = {_currentUser.UserId}) is removing {typeof(TReference).Name} (ID = {referenceID}) from {typeof(TParent).Name} (ID = {id}).");
        
        await ValidatePermissionAsync(cancellationToken);
        var referencedObject = await GetReferencedObjectAsync(id, collectionExpression, cancellationToken);

        var collection = collectionExpression.Compile()(referencedObject);
        var reference = collection.FirstOrDefault(x => referenceIDAccessor(x.Reference).Equals(referenceID))
                        ?? throw new ObjectNotFoundException(
                            $"Association of {typeof(TReference).Name} (ID = {referenceID}) and {typeof(TParent).Name} (ID = {id})");

        collection.Remove(reference);
        _logger.LogInformation(
            $"User (ID = {_currentUser.UserId}) successfully removed {typeof(TReference).Name} (ID = {referenceID}) from {typeof(TParent).Name} (ID = {id})");
    }
    
    public async Task<TReference[]> GetReferencesAsync<TReference>(
        TKey id,
        Expression<Func<TParent, ICollection<ManyToMany<TParent, TReference>>>> collectionExpression,
        CancellationToken cancellationToken = default) where TReference : class
    {
        _logger.LogTrace(
            $"User (ID = {_currentUser.UserId}) is removing {typeof(TReference).Name} from {typeof(TParent).Name} (ID = {id}).");
        
        var referencedObject = await GetReferencedObjectAsync(id, collectionExpression, cancellationToken);
        var collection = collectionExpression.Compile()(referencedObject);

        return collection.Select(x => x.Reference).ToArray();
    }
    

    private async Task<TParent> GetReferencedObjectAsync<TReference>(
        TKey id,
        Expression<Func<TParent, ICollection<ManyToMany<TParent, TReference>>>> include,
        CancellationToken cancellationToken) where TReference : class
    {
        var includableExpression = new ExpressionResultConverter<IEnumerable<ManyToMany<TParent, TReference>>>()
            .Convert(include);

        var parentObject = await _repository
                                  .WithMany(includableExpression)
                                  .FindAsync(id, cancellationToken)
                              ?? throw new ObjectNotFoundException<TKey>(id, nameof(TParent));

        _logger.LogTrace($"{nameof(TParent)} (ID = {id}) is loaded.");

        return parentObject;
    }
    
    private async Task ValidatePermissionAsync(CancellationToken cancellationToken = default)
    {
        await _permissionsValidator.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Update,
            cancellationToken);
    }
}