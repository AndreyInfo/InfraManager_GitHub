using Inframanager;

namespace InfraManager.DAL.ServiceDesk;

/// <summary>
/// Определяет интерфейс-маркер построителя спецификации для проверки доступности объекта по ТТЗ.
/// </summary>
/// <typeparam name="TExecutor">Тип исполнителя.</typeparam>
/// <typeparam name="TEntity">Тип объекта.</typeparam>
public interface IBuildAvailableToExecutorViaTtz<TExecutor, TEntity> : IBuildSpecification<TExecutor, TEntity>
{
}