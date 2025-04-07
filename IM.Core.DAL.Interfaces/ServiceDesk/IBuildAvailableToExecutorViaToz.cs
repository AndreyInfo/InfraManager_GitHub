using Inframanager;

namespace InfraManager.DAL.ServiceDesk;

/// <summary>
/// Определяет интерфейс-маркер построителя спецификации для проверки доступности объекта по ТОЗ.
/// </summary>
/// <typeparam name="TExecutor">Тип исполнителя.</typeparam>
/// <typeparam name="TEntity">Тип объекта.</typeparam>
public interface IBuildAvailableToExecutorViaToz<TExecutor, TEntity> : IBuildSpecification<TExecutor, TEntity>
{
}