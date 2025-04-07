using System;
using System.Collections.Generic;
using System.Text;

namespace InfraManager.CrossPlatform.WebApi.Contracts.Logging
{
    /// <summary>
    /// Данные по результату исполнения метода
    /// </summary>
    public class WebApiLogResultModel : WebApiLogModel
    {
        /// <summary>
        /// Результат исполнения - успех или ошибка
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Результат успешного исполнения (json представление)
        /// </summary>
        public string ResultData { get; set; }
        /// <summary>
        /// Информация об ошибке в случае ошибки
        /// </summary>
        public string ExceptionData { get; set; }
        /// <summary>
        /// Время исполнения метода
        /// </summary>
        public long ExecuteDuration { get; set; }
    }
}
