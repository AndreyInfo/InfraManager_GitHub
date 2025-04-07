using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Messages;

/// <summary>
/// Определяет интерфейс построителя шаблона из объекта <typeparamref name="TEntity"/>.
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
/// <typeparam name="TTemplate">Тип шаблона.</typeparam>
public interface IBuildEntityTemplate<in TEntity, TTemplate>
{
    /// <summary>
    /// Построить экземпляр <typeparamref name="TTemplate"/> для сущности <typeparamref name="TEntity"/> с заданным идентификатором.
    /// </summary>
    /// <param name="id">Уникальный идентификатор сущности.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <param name="userID">Идентификатор пользователя, для которого строится темплейт</param>
    /// <returns>Подготовленный шаблон.</returns>
    Task<TTemplate> BuildAsync(Guid id, CancellationToken cancellationToken = default, Guid? userID = null);
}