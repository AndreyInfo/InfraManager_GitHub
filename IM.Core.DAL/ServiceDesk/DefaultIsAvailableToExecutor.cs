using Inframanager;

namespace InfraManager.DAL.ServiceDesk;

/// <summary>
/// Представляет обощенный построитель предикатов для получения списка исполнителей по-умолчанию.
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
internal class DefaultIsAvailableToExecutor<TEntity> :
    IBuildAvailableToExecutorViaToz<User, TEntity>,
    IBuildAvailableToExecutorViaToz<Group, TEntity>,
    IBuildAvailableToExecutorViaTtz<User, TEntity>,
    IBuildAvailableToExecutorViaTtz<Group, TEntity>,
    IBuildAvailableToExecutorViaSupportLine<User, TEntity>,
    IBuildAvailableToExecutorViaSupportLine<Group, TEntity>
{
    Specification<User> IBuildSpecification<User, TEntity>.Build(TEntity filterBy)
    {
        return new Specification<User>(_ => true);
    }

    Specification<Group> IBuildSpecification<Group, TEntity>.Build(TEntity filterBy)
    {
        return new Specification<Group>(_ => true);
    }
}