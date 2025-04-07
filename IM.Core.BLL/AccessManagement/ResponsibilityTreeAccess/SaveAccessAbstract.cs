using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess;


internal abstract class SaveAccessAbstract<TEntity> : ISaveAccess where TEntity : class
{
    protected abstract AccessTypes AccessType { get; }
    protected abstract ObjectClass ClassID { get; }
    protected abstract IReadOnlyCollection<ObjectClass> ChildClasses { get; }

    private readonly IRepository<ObjectAccess> _objectAccesses;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public SaveAccessAbstract(IRepository<ObjectAccess> objectAccesses
        , IMapper mapper
        , IUnitOfWork unitOfWork)
    {
        _objectAccesses = objectAccesses;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task SaveAccessAsync(AccessElementsData data, Guid? id = null, CancellationToken cancellationToken = default)
    {
        if (id.HasValue && await _objectAccesses.AnyAsync(c => c.ID == id, cancellationToken))
        {
            if (!data.IsSelectPart && !data.IsSelectFull)
                await DeleteAsync(id.Value, data.OwnerID, cancellationToken);
            else
                await UpdateAsync(id.Value, data, cancellationToken);
        }
        else if (data.IsSelectPart || data.IsSelectFull)
        {
            var objectAccess = await _objectAccesses.FirstOrDefaultAsync(c => c.OwnerID == data.OwnerID
                && c.ObjectID == data.ObjectID
                && c.ClassID == data.ObjectClassID
                && c.Type == data.AccessType
                , cancellationToken);

            if (objectAccess is not null)
                objectAccess.Propagate = data.IsSelectFull;
            else
                await AddAsync(data, cancellationToken);
        }
        else if (data.ObjectID != Guid.Empty && await _objectAccesses.AnyAsync(c => c.ObjectID == data.ObjectID, cancellationToken))
        {
            var objects = _objectAccesses.Where(c => c.OwnerID == data.OwnerID
                && c.ObjectID == data.ObjectID);

            var objectAccess = await _objectAccesses.FirstOrDefaultAsync(c => c.OwnerID == data.OwnerID
                && c.ObjectID == data.ObjectID
                && c.ClassID == data.ObjectClassID
                && c.Type == data.AccessType
                , cancellationToken);

            if (objectAccess is not null)
            {
                if (!data.IsSelectPart && !data.IsSelectFull)
                    await DeleteAsync(objectAccess.ID, data.OwnerID, cancellationToken);
            }
        }

        await _unitOfWork.SaveAsync(cancellationToken);
    }

    // Метод обновляет статус propagate для элемента и добавляет дочерние элементы.
    private async Task UpdateAsync(Guid id, AccessElementsData data, CancellationToken cancellationToken)
    {
        var access = await _objectAccesses.FirstOrDefaultAsync(c => c.ID == id, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(id, $"Not found {nameof(ObjectAccess)}");

        if (access.Propagate)
            await InsertSubObjectsAsync(data.ObjectID.GetValueOrDefault(), data.OwnerID, cancellationToken);

        _mapper.Map(data, access);
    }

    private async Task AddAsync(AccessElementsData data, CancellationToken cancellationToken)
    {
        await InsertItemAsync(data.OwnerID, data.ObjectID, data.ObjectClassID, data.IsSelectFull, cancellationToken);

        if (data.IsSelectFull)
            await InsertSubObjectsAsync(data.ObjectID.GetValueOrDefault(), data.OwnerID, cancellationToken);
    }

    protected async Task DeleteAsync(Guid id, Guid ownerID, CancellationToken cancellationToken)
    {
        var access = await _objectAccesses.FirstOrDefaultAsync(c => c.ID == id, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(id, $"Not found {nameof(ObjectAccess)}");

        _objectAccesses.Delete(access);

        await DeleteSubObjectsAsync(access.ObjectID.GetValueOrDefault(), ownerID, cancellationToken);
    }

    /// <summary>
    /// Добавление доступа с проверкой на его существование 
    /// </summary>
    /// <param name="ownerID">идентификатор собственника доступа</param>
    /// <param name="objectID">идентификатор объекта к которому дают доступ</param>
    /// <param name="classID">тип объекта</param>
    /// <param name="propagate">полный доступ</param>
    /// <param name="cancellationToken">токен отмены</param>
    protected async Task InsertItemAsync(Guid ownerID, Guid? objectID, ObjectClass classID, bool propagate = true, CancellationToken cancellationToken = default)
    {
        var isExists = await _objectAccesses.AnyAsync(v => v.OwnerID == ownerID
                                        && v.Type == AccessType
                                        && v.ClassID == classID
                                        && v.ObjectID == objectID
                                        , cancellationToken);
        if (!isExists)
        {
            var access = new ObjectAccess(ownerID, AccessType, classID, objectID, propagate);
            _objectAccesses.Insert(access);
        }
    }

    /// <summary>
    /// Удаление дочерних элементов узла в дереве
    /// </summary>
    /// <param name="parentID">идентификатор родительского элемента</param>
    /// <param name="ownerID">идентификатор владельца доступа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <exception cref="ArgumentNullException">если не инициализируют ChildClasses</exception>
    private async Task DeleteSubObjectsAsync(Guid parentID, Guid ownerID, CancellationToken cancellationToken)
    {
        if (ChildClasses is null)
            throw new ArgumentNullException($"{nameof(ChildClasses)} must not null");

        var subObjectAccess = Array.Empty<ObjectAccess>();
        if (!new ObjectClass[] { ObjectClass.Owner, ObjectClass.ProductCatalogue, ObjectClass.ServiceCatalogue }.Contains(ClassID))
        {
            var childrenIDs = await GetIDsSubObjectsAsync(parentID, cancellationToken);
            if (childrenIDs.Any())
                subObjectAccess = await _objectAccesses.ToArrayAsync(c => c.Type == AccessType
                                                              && c.OwnerID == ownerID
                                                              && ChildClasses.Contains(c.ClassID)
                                                              && childrenIDs.Contains(c.ObjectID.Value)
                                                              , cancellationToken);
        }
        else
        {
            subObjectAccess = await _objectAccesses.ToArrayAsync(c => c.Type == AccessType
                                                              && c.OwnerID == ownerID
                                                              && ChildClasses.Contains(c.ClassID)
                                                              , cancellationToken);
        }

        subObjectAccess.ForEach(c => _objectAccesses.Delete(c));

    }

    /// <summary>
    /// Получение id всех дочерних элементов для удаления
    /// </summary>
    /// <param name="parentID">идентификатор родительского элемента</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>массив id дочерних элементов</returns>
    protected abstract Task<Guid[]> GetIDsSubObjectsAsync(Guid parentID, CancellationToken cancellationToken);

    /// <summary>
    /// Добавление доступа для всех дочерних сущностей в дереве
    /// </summary>
    /// <param name="parentID">идентификатор родителя, для которого вставляем </param>
    /// <param name="ownerID">идентификатор для кого добавляем права доступа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    protected abstract Task InsertSubObjectsAsync(Guid parentID, Guid ownerID, CancellationToken cancellationToken);
}