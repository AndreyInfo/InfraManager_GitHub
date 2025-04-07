using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace InfraManager.DAL
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Сохранить изменения
        /// </summary>
        /// <param name="isolationLevel">Уровень изоляции транзакции</param>
        void Save(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// Сохранить изменения асинхронно
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <param name="isolationLevel">Уровень изоляции транзакции</param>
        /// <returns></returns>
        Task SaveAsync(CancellationToken cancellationToken = default, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// Определяет наличие изменений зарегистрированных Unit of work
        /// </summary>
        /// <returns>Признак того что в unit of work есть изменения</returns>
        bool HasChanges();
    }
}
