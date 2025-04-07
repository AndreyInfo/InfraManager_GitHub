using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.OrganizationStructure
{
    /// <summary>
    /// Этот интерфейс описывает BLL сервис сущности "Владелец"
    /// </summary>
    public interface IOwnerBLL
    {
        /// <summary>
        /// Возращает всех владельцев
        /// </summary>
        /// <param name="take">Максимальное количество элементов, которые необходимо вернуть</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Массив выходных контрактов с данными владельцев</returns>
        Task<OwnerDetails[]> AllAsync(int? take, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает данные владельца по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор владельца</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Выходной контракт с данными запрашиваемого владельца</returns>
        Task<OwnerDetails> DetailsAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Изменяет существующего владельца
        /// </summary>
        /// <param name="id">Идентификатор изменяемого владельца</param>
        /// <param name="data">Данные о новом состояния владельца</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Выходной контракт с данными владельца после применения изменений</returns>
        Task<OwnerDetails> ModifyAsync(Guid id, OwnerData data, CancellationToken cancellationToken);
    }
}
