using System;

namespace InfraManager.CrossPlatform.WebApi.Contracts.SoftwareInstallation.Models
{
    /// <summary>
    /// Модель элемента списка инсталляций
    /// </summary>
    public sealed class SoftwareInstallationListItem
    {
        /// <summary>
        /// Идентификатор инсталляции
        /// </summary>
        public  Guid ID { get; set; }

        /// <summary>
        /// Название технической модели ПО
        /// </summary>       
        public string SoftwareModelName { get; set; }

        /// <summary>
        /// Название коммерческой модели ПО, которая соответствует инсталляция (при наличии, иначе пустота)
        /// </summary>       
        public string CommercialModelName { get; set; }

        /// <summary>
        /// Название ИТ-актива - компьютера, на котором установлена инсталляция
        /// </summary>     
        public string DeviceName { get; set; }

        /// <summary>
        /// Строка - путь, по которому найдена инсталляция
        /// </summary>      
        public string InstallPath { get; set; }

        /// <summary>
        /// Дата, когда был установлена инсталляция
        /// </summary>        
        public string InstallDate { get; set; }        

        /// <summary>
        /// Дата последнего запуска задачи опроса по сети для поиска инсталляций
        /// </summary>
        public string DateLastSurvey { get; set; }

        /// <summary>
        /// Дата последнего запуска задачи последней проверки прав
        /// </summary>
        public string DateLastRightsCheck { get; set; }

        /// <summary>
        /// Дата, когда была создана запись
        /// </summary>        
        public string CreateDate { get; set; }

        /// <summary>
        /// Статус Инсталляции
        /// </summary>        
        public byte Status { get; set; }

        /// <summary>
        /// Статус Инсталляции
        /// </summary>        
        public string StatusName { get; set; }
    }
}
