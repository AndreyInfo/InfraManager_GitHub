using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Software
{
    /// <summary>
    /// Поставщик данных дял сущности "Схема лицензирования"
    /// </summary>
    public interface ISoftwareLicenceSchemeDataProvider
    {
        /// <summary>
        /// Получение схемы лицензирвоания по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="cancellationToken"></param>
        /// <returns>полную модлеь схемы лицензирвоания</returns>
        Task<SoftwareLicenceScheme> GetAsync(Guid id, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Получение списка схем лицензирования
        /// </summary>
        /// <param name="searchText">стрка поиска (по названию)</param>
        /// <param name="showDeleted">флаг включения в вывод и удаленных схем. Если не установлен - то только не удаленные выводятся</param>
        /// <param name="cancellationToken"></param>
        /// <returns>краткая модель схемы для отображения в списке</returns>
        Task<List<SoftwareLicenceScheme>> GetListAsync(string searchText = null, bool showDeleted = false, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// создание схемы лицензирвоания
        /// </summary>
        /// <param name="softwareLicenceScheme">полная модлеь схемы лицензирвоания для сохранения</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Базовый результат с идентификтаором сохаренной (или созданной) схемы и элементом перечисления бизнес правил, 
        /// нарушение которого обнаружено при сохранении. в этом случае ИД пустой
        /// </returns>
        Task<Guid> AddAsync(SoftwareLicenceScheme softwareLicenceScheme, CancellationToken cancellationToken = default);
        Task<SoftwareLicenceScheme> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Получение списка коэффициентов схемы лицензирования
        /// </summary>
        /// <param name="schemeID">Идентификтаор схемы, дял котрой получить список</param>
        /// <returns>список сущностей коэффициентов указанной схемы</returns>
        Task<List<SoftwareLicenceSchemeProcessorCoeff>> GetCoefficientListAsync(Guid schemeID, CancellationToken cancellationToken = default);
        Task AddAsync(SoftwareLicenceSchemeProcessorCoeff softwareLicenceSchemeProcessorCoeff, CancellationToken cancellationToken = default);
        void Delete(SoftwareLicenceSchemeProcessorCoeff softwareLicenceSchemeProcessorCoeff);
    }
}
