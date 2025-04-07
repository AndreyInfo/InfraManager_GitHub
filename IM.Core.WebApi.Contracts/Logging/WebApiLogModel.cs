using System;
using System.Collections.Generic;
using System.Text;

namespace InfraManager.CrossPlatform.WebApi.Contracts.Logging
{
    /// <summary>
    /// Модель логирвания данных запросво к API
    /// </summary>
    public class WebApiLogModel
    {
        /// <summary>
        /// Вермя события
        /// </summary>
        public DateTime EventTime { get; set; }
        /// <summary>
        /// Хотс, к которому обращение
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// Метод, к которому идет обращение
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Параметры строки запроса
        /// </summary>
        public string Query { get; set; }
        /// <summary>
        /// Идентификатор пользователя (если не авторизирован, то пусто)
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// Парамтеры ПОСТ метода (json представление)
        /// </summary>
        public string RequestData { get; set; }
        /// <summary>
        /// Идентификатоор устройва (браузера) пользователя (fingerprint)
        /// </summary>
        public string UserDeviceID { get; set; }
        /// <summary>
        /// идентификтор вызова
        /// </summary>
        public string CorrelationID { get; set; }
    }
}
