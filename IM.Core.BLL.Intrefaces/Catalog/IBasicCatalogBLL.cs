using InfraManager.DAL;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using InfraManager.BLL.CrudWeb;

namespace InfraManager.BLL.Catalog
{
    /// <summary>
    /// Предоставляет базовую реализацию основных бизнес правил для справочников.
    /// <para>Справочники обязаны иметь профиль маппинга, <see cref="IRepository{TCatalog}"/> и <see cref="IFinder{TCatalog}"/></para>
    /// <para>Если по умолчанию не настроить свойства которые отвечают за название и идентификатор каталога,
    /// то по-умолчанию будут использованы свойства с названием ID и Name</para>
    /// </summary>
    /// <typeparam name="TCatalog">Тип справочника</typeparam>
    /// <typeparam name="TCatalogDto">Представляет тип DTO справочника.</typeparam>
    public interface IBasicCatalogBLL<TCatalog, TCatalogDto, TKey, TTable>
        where TCatalog : Catalog<TKey>
        where TKey : struct
    {
        /// <summary>
        /// Сохраняет или обновляет справочник в БД.
        /// <para>Если справочник с таким именем уже имеется в БД, то будет возвращена ошибка '<see cref="BaseError.ExistsByName"/>'</para>
        /// </summary>
        /// <returns>Идентификатор добавленного или обновленного справочника.</returns>
        Task<TKey> SaveOrUpdateAsync(TCatalogDto catalogDetails,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Массово удаляет справочники.
        /// <para>В случае ошибки будут возвращены имена справочников, которые не были удалены</para>
        /// </summary>
        /// <param name="catalogIds">Идентификаторы справочников, которые необходимо удалить</param>
        /// <returns>Результат содержащий либо успешную операцию содержащий имена не удаленных справочников.</returns>
        Task<string[]> DeleteAsync(IEnumerable<DeleteModel<TKey>> deleteModels,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// Удаляет справочник.
        /// </summary>
        /// <param name="id">Идентификатор сущности</param>
        Task RemoveAsync(TKey id, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Возвращает справочники по указанным фильтрам.
        /// </summary>
        Task<TCatalogDto[]> GetByFilterAsync(BaseFilter filter,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает справочники по указанным фильтрам.
        /// </summary>
        Task<TCatalogDto> GetByIDAsync(TKey id,
            CancellationToken cancellationToken = default);

        void SetIncludeItems<TProperty>(Expression<Func<TCatalog, TProperty>> expression) where TProperty : class;
        
        Task<TCatalogDto[]> GetListAsync(string searchString=null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Обновляет
        /// <para>Если справочник с таким именем уже имеется в БД, то будет возвращена ошибка '<see cref="BaseError.ExistsByName"/>'</para>
        /// </summary>
        /// <returns>Идентификатор добавленного или обновленного справочника.</returns>
        Task<TKey> UpdateAsync(TKey id,TCatalogDto catalogDetails, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Добавляет новую сущность в базу данных
        /// <para>Если справочник с таким именем уже имеется в БД, то будет возвращена ошибка '<see cref="BaseError.ExistsByName"/>'</para>
        /// </summary>
        /// <returns>Идентификатор добавленного или обновленного справочника.</returns>
        public Task<TKey> InsertAsync(TCatalogDto catalogDetails, CancellationToken cancellationToken = default);
    }
}
