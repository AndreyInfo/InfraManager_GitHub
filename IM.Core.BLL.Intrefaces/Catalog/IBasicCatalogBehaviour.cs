using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System.Linq;
using System.Threading.Tasks;

namespace InfraManager.BLL.Catalog
{
    /// <summary>
    /// Определяет базовое поведение для <see cref="IBasicCatalogBLL{TCatalog, TCatalogDto, TId}"/>
    /// </summary>
    public interface IBasicCatalogBehaviour<TCatalog>
        where TCatalog : class
    {
        /// <summary>
        /// Проверяет, можно ли произвести вставку каталога в базу данных.
        /// </summary>
        /// <returns>Вернет <see cref="BaseError.Success"/> если вставку произвести можно; иначе код ошибки.</returns>
        ValueTask<BaseError> IsCanInsertAsync(TCatalog catalog);

        /// <summary>
        /// Проверяет, можно ли произвести обновление каталога в базе данных.
        /// </summary>
        /// <returns>Вернет <see cref="BaseError.Success"/> если обновление произвести можно; иначе код ошибки.</returns>
        ValueTask<BaseError> IsCanUpdateAsync(TCatalog catalog);

        /// <summary>
        /// Производит специфический способ удаления каталога.
        /// <para>Какие-то каталоги удаляются из базы данных напрямую, а какие-то могут помечаться как Removed.
        /// (Не исключено, что в будущем может быть новый кейс).</para>
        /// </summary>
        /// <param name="catalog"></param>
        /// <returns></returns>
        ValueTask<bool> DeleteCatalogAsync(TCatalog catalog);

        /// <summary>
        /// Возвращает <see cref="IQueryable{T}"/> настроенный с фильтрами по-умолчанию для получения каталога.
        /// <para>Какие-то каталоги необходимо возвращать с Removed = false, а какие-то по другим кейсам.</para>
        /// </summary>
        ValueTask<IQueryable<TCatalog>> GetDefaultCatalogQueryAsync(BaseFilter filter);
    }
}
