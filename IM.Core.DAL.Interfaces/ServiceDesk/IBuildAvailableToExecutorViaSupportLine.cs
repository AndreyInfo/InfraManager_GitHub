using Inframanager;

namespace InfraManager.DAL.ServiceDesk;

/// <summary>
/// Определяет интерфейс-маркер построителя спецификации для проверки доступности объекта с учетом ответственности за сервис.
/// </summary>
/// <typeparam name="TExecutor">Тип исполнителя.</typeparam>
/// <typeparam name="TEntity">Тип объекта.</typeparam>
public interface IBuildAvailableToExecutorViaSupportLine<TExecutor, TEntity> : IBuildSpecification<TExecutor, TEntity>
{
}