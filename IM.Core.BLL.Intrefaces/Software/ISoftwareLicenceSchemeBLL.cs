using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.CrossPlatform.WebApi.Contracts.Common.Models;
using InfraManager.CrossPlatform.WebApi.Contracts.SoftwareLicenceScheme.Models;

namespace InfraManager.BLL.Software
{
    /// <summary>
    /// Поставщик данных дял сущности "Схема лицензирования"
    /// </summary>
    public interface ISoftwareLicenceSchemeBLL
    {
        /// <summary>
        /// Получение схемы лицензирвоания по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="cancellationToken"></param>
        /// <returns>полную модлеь схемы лицензирвоания</returns>
        Task<BaseResult<SoftwareLicenceScheme, BaseError>> GetAsync(Guid id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Получение списка схем лицензирования
        /// </summary>
        /// <param name="searchText">стрка поиска (по названию)</param>
        /// <param name="showDeleted">флаг включения в вывод и удаленных схем. Если не установлен - то только не удаленные выводятся</param>
        /// <param name="cancellationToken"></param>
        /// <returns>краткая модель схемы для отображения в списке</returns>
        Task<BaseResult<List<SoftwareLicenceSchemeListItem>, BaseError>> GetListAsync(string searchText = null, bool showDeleted = false, SortModel sortModel = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// Сохранеие (создание или обновление) схемы лицензирвоания
        /// </summary>
        /// <param name="softwareLicenceScheme">полная модлеь схемы лицензирвоания для сохранения</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Базовый результат с идентификтаором сохраненной (или созданной) схемы и элементом перечисления бизнес правил, 
        /// нарушение которого обнаружено при сохранении. в этом случае ИД пустой
        /// </returns>
        Task<BaseResult<Guid, SoftwareLicenceSchemeRules>> SaveAsync(SoftwareLicenceScheme softwareLicenceScheme, CancellationToken cancellationToken = default);
        /// <summary>
        /// Отмечает схему как удаенную или снимает отметку
        /// </summary>
        /// <param name="id">Идентификатор схемы</param>
        /// <param name="doRestore">Флаг выполения восстановления схемы. Иначе - удаление</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Базовый результат с идентификтаором сохаренной (или созданной) схемы и элементом перечисления бизнес правил, 
        /// нарушение которого обнаружено при сохранении. в этом случае ИД пустой
        /// </returns>
        Task<BaseResult<SoftwareLicenceSchemeDeleteResponse, SoftwareLicenceSchemeRules>> DeleteRestoreAsync(SoftwareLicenceSchemeDeleteRequest request, bool doRestore, CancellationToken cancellationToken = default);
        Task<BaseResult<List<ListItem>, BaseError>> GetSchemeTypesAsync(CancellationToken cancellationToken);
        Task<BaseResult<List<ListItem>, BaseError>> GetLicenseObjectTypesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает перечень доступных операторов / функций / переменных, допустимых в выражении расчета к-ва лицензий и доп. лицензий
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Объект с перечислением строк с именами переменных, функций и переменных, допустимых в выражении расчета к-ва лицензий и доп. лицензий </returns>
        Task<BaseResult<ExpressionStatementsModel, BaseError>> GetLicenceExpressionStatements(CancellationToken cancellationToken);

        /// <summary>
        /// Проверяет значение атрибута схемы лицензирования
        /// </summary>
        /// <param name="fieldName">Наименование атрибута схемы лицензирования</param>
        /// <param name="value">Значение атрибута, которое необходимо проверить</param>
        /// <param name="token"></param>
        /// <returns>Результат выполнения операции</returns>
        Task<ExpressionValidationResponse> Validate(string fieldName, object value, CancellationToken token = default);
    }
}
