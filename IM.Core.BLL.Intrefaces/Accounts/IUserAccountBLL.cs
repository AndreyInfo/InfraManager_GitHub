using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Accounts
{
    /// <summary>
    /// Бизнес логика для работы со справочником учетных записей, предназначеных для хранения данных об учетных записях, используемых при импорте, интеграции с внешними системами и при опросе сети.
    /// </summary>
    public interface IUserAccountBLL
    {
        /// <summary>
        /// Получение конкрентной учетной записи
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="isDecoded">Флаг получение паролей в расшифрованном виде</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<UserAccountDetails> DetailsAsync(int id, bool isDecoded, CancellationToken cancellationToken = default);

        /// <summary>
        /// Сохранение изменений конкрентной учетной записи
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="dataModel">Модель данных</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<UserAccountDetails> UpdateAsync(int id, UserAccountData dataModel, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удвление конкрентной учетной записи
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Добавление новой учетной записи
        /// </summary>
        /// <param name="dataModel">Модель данных</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>модель учетной записи</returns>
        Task<UserAccountDetails> AddAsync(UserAccountData dataModel, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение списка учетных записей 
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<UserAccountDetails[]> ListAsync(UserAccountFilter filter, CancellationToken cancellationToken = default);
    }
}
