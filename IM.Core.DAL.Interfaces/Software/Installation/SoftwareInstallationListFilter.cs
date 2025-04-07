using System;

namespace InfraManager.DAL.Software.Installation
{
    /// <summary>
    /// Модель фильтра поиска инсталляций
    /// </summary>
    public sealed class SoftwareInstallationListFilter
    {
        /// <summary>
        /// Начальная запись
        /// </summary>
        public int StartRecordIndex { get; set; }

        /// <summary>
        /// Коль-во записей
        /// </summary>
        public int CountRecords { get; set; }

        /// <summary>
        /// Идентификатор фильтра
        /// </summary>
        public Guid? CurrentFilterID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UserTreeSettings TreeSettings { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? ParentID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ObjectClass? ParentClassID { get; set; }

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
        public DateTime? InstallDate { get; set; }

        /// <summary>
        /// Дата последнего запуска задачи опроса по сети для поиска инсталляций
        /// </summary>
        public DateTime? DateLastSurvey { get; set; }

        /// <summary>
        /// Дата последнего запуска задачи последней проверки прав
        /// </summary>
        public DateTime? DateLastRightsCheck { get; set; }

        /// <summary>
        /// Дата, когда была создана запись
        /// </summary>        
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// Статус Инсталляции
        /// </summary>        
        public byte? Status { get; set; }
    }
}
