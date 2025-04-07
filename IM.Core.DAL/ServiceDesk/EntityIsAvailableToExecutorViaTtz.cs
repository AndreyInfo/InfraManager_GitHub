using System.Linq;
using Inframanager;
using InfraManager.DAL.AccessManagement;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.ServiceDesk;

/// <summary>
/// Представляет обощенный предикат построителя предиката для определения доступности сущности исполнителю по ТТЗ.
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
/// <typeparam name="TExecutor">Тип исполнителя.</typeparam>
/// <typeparam name="TDependencyEntity">Тип связанного объекта-зависимости.</typeparam>
internal class EntityIsAvailableToExecutorViaTtz<TEntity, TExecutor, TDependencyEntity> :
    IBuildAvailableToExecutorViaTtz<TExecutor, TEntity>
    where TEntity : IGloballyIdentifiedEntity
    where TExecutor : IGloballyIdentifiedEntity
    where TDependencyEntity : Dependency
{
    private static readonly ObjectClass[] HardwareClasses =
    {
        ObjectClass.Adapter,
        ObjectClass.Peripherial,
        ObjectClass.ActiveDevice,
        ObjectClass.TerminalDevice,
    };

    private readonly IQueryable<TDependencyEntity> _dependencies;
    private readonly IObjectClassProvider<TEntity> _classProvider;

    public EntityIsAvailableToExecutorViaTtz(
        DbSet<TDependencyEntity> dependencies,
        IObjectClassProvider<TEntity> classProvider)
    {
        _dependencies = dependencies.AsNoTracking();
        _classProvider = classProvider;
    }

    public Specification<TExecutor> Build(TEntity filterBy)
    {
        var dependencies = _dependencies
            .Where(dependency => dependency.OwnerObjectID == filterBy.IMObjID && HardwareClasses.Contains(dependency.ObjectClassID));
        
        return new Specification<TExecutor>(user =>
            !dependencies.Any()
            || dependencies.All(dependency => DbFunctions.AccessIsGranted(
                dependency.ObjectClassID,
                dependency.ObjectID,
                user.IMObjID,
                _classProvider.GetObjectClass(),
                AccessTypes.TTZ_sks,
                false)));
    }
}