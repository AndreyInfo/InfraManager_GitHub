namespace InfraManager.CrossPlatform.WebApi.Contracts.Common
{
    /// <summary>
    /// Ошибки исполнения запросов
    /// </summary>
    public enum BaseError
    {
        /// <summary>
        /// Нет ошибок
        /// </summary>
        Success = 0,

        /// <summary>
        /// Параметр null
        /// </summary>
        NullParamsError = 1,

        /// <summary>
        /// Ошибочный параметр
        /// </summary>
        BadParamsError = 2,

        /// <summary>
        /// Нет доступа
        /// </summary>
        AccessError = 3,

        /// <summary>
        /// Глобальная ошибка
        /// </summary>
        GlobalError = 4,

        /// <summary>
        /// Конкуренция за изменение данных
        /// </summary>
        ConcurrencyError = 5,

        /// <summary>
        /// Объект уже удален
        /// </summary>
        ObjectDeleted = 6,

        /// <summary>
        /// Ошибка операции
        /// </summary>
        OperationError = 7,

        /// <summary>
        /// Ошибка валидации
        /// </summary>
        ValidationError = 8,

        /// <summary>
        /// Объект уже существует
        /// </summary>
        ExistsByName = 13

    }
}
