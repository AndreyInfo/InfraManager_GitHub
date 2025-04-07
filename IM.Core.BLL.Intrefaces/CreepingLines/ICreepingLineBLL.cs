using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.CreepingLines
{
    /// <summary>
    /// Интерфейс для бизнес-логики Бегущей строки
    /// </summary>
    public interface ICreepingLineBLL
    {
        /// <summary>
        /// Получение списка бегущих строк
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <param name="token"> токен отмены </param>
        /// <returns> список бегущих строк </returns>
        Task<CreepingLineDetails[]> ListAsync(CreepingLineFilter filter, CancellationToken token = default);
        
        /// <summary>
        /// Получение бегущей строки по идентификатору
        /// </summary>
        /// <param name="id"> идентификатор бегущей строки</param>
        /// <param name="token"> токен отмены </param>
        /// <returns> созданная бегущая строка </returns>
        Task<CreepingLineDetails> GetAsync(Guid id, CancellationToken token = default);
        
        /// <summary>
        /// Добавление бегущей строки в базу данных
        /// </summary>
        /// <param name="data"> модель бегущей строки</param>
        /// <param name="token"> токен отмены </param>
        /// <returns> идентификатор бегущей строки </returns>
        Task<Guid> InsertAsync(CreepingLineData data, CancellationToken token = default);

        /// <summary>
        /// Обновление бегущей строки в базе данных
        /// </summary>
        /// <param name="data"> модель бегущей строки</param>
        /// <param name="cancellationToken"> токен отмены </param>
        /// <returns> идентификатор бегущей строки </returns>
        Task<Guid> UpdateAsync(Guid id, CreepingLineData data, CancellationToken cancellationToken);
        
        /// <summary>
        /// Удаление бегущей строки из базы данных
        /// </summary>
        /// <param name="id"> идентификатор бегущей строки</param>
        /// <param name="token"> токен отмены </param>
        /// <returns> результат удаления </returns>
        Task DeleteAsync(Guid id, CancellationToken token = default);
    }
}
