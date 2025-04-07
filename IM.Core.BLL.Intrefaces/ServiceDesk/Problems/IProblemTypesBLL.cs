using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace InfraManager.BLL.ServiceDesk.Problems
{
    /// <summary>
    /// Этот интерфейс описывает BLL сервис сущности "Тип проблемы"
    /// </summary>
    public interface IProblemTypesBLL
    {
        /// <summary>
        /// Получает список типов проблем
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Массив данных типов массовых инцидентов</returns>
        Task<ProblemTypeDetails[]> GetAllDetailsArrayAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Получает тип проблемы по problemTypeId
        /// </summary>
        /// <param name="id">идентификатор типа проблемы</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Тип проблемы, если problemTypeID != Guid.Empty. Первый родительский тип проблемы, если problemTypeID == Guid.Empty</returns>
        Task<ProblemTypeDetails> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получает путь до узла по problemTypeId
        /// </summary>
        /// <param name="id">идентификатор типа проблемы</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Массив данных типов массовых инцидентов</returns>
        Task<ProblemTypeDetails[]> GetPathByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получает дерево по родительскому идентификатору
        /// </summary>
        /// <param name="filterId"></param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <param name="id">идентификатор родительского элемента</param>
        /// <returns>Массив данных типов проблем</returns>
        Task<ProblemTypeDetails[]> GetTreeByIdAsync(Guid id, List<Guid> filterId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Создает новый тип проблемы
        /// </summary>
        /// <param name="data">входные данные для добавления типа проблемы</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Данные нового типа проблемы</returns>
        Task<ProblemTypeDetails> AddAsync(ProblemTypeData data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Изменяет тип проблемы
        /// </summary>
        /// <param name="id">идентификатор типа проблемы для изменения</param>
        /// <param name="data">Новые данные для типа проблемы</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Данные типа проблемы после изменения</returns>
        Task<ProblemTypeDetails> UpdateAsync(Guid id, ProblemTypeData data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаляет тип проблемы
        /// </summary>
        /// <param name="id">Идентификатор типа проблемы для удаления</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Получает изображение для типа проблемы
        /// </summary>
        /// <param name="id">Идентификатор типа проблемы для удаления</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// Массив byte содержащий картинку
        Task<byte[]> GetImageAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
