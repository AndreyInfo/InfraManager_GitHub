using InfraManager.CrossPlatform.WebApi.Contracts.Common.Attributes;
using System;

namespace InfraManager.CrossPlatform.WebApi.Contracts.SoftwareInstallation.Models
{
    /// <summary>
    /// Модель данных Инсталляции
    /// </summary>
    [EntityCompare(71, "Инсталяция", AddOperationID = 86, EditOperationID = 245, DeleteOperationID = 88)]
    public sealed class SoftwareInstallationItem : ICustomValueManager
    {
        /// <summary>
        /// Идентификатор инсталляции
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// Наименование инсталляции
        /// </summary>       
        [FieldCompare("Наименование", 1, IsUseCustomValues = true)]
        public string SoftwareInstallationName { get; set; }

        /// <summary>
        /// Идентифтикатор технической модели ПО
        /// </summary>       
        [FieldCompare("Техническая модель ПО")]
        public Guid SoftwareModelID { get; set; }

        /// <summary>
        /// Название технической модели ПО
        /// </summary>       
        public string SoftwareModelName { get; set; }

        /// <summary>
        /// Название коммерческой модели ПО, которая соответствует инсталляция (при наличии, иначе пустота)
        /// </summary>       
        public string CommercialModelName { get; set; }

        /// <summary>
        /// Идентифткатор класса ИТ-актива
        /// </summary>     
        [FieldCompare("Класс Места установки", IsUseCustomValues = true)]
        public int DeviceClassID { get; set; }
        /// <summary>
        /// Идентифткатор ИТ-актива - компьютера, на котором установлена инсталляция
        /// </summary>     
        [FieldCompare("Место установки", IsUseCustomValues = true)]
        public Guid DeviceID { get; set; }
        /// <summary>
        /// Название ИТ-актива - компьютера, на котором установлена инсталляция
        /// </summary>     
        public string DeviceName { get; set; }

        /// <summary>
        /// Строка - путь, по которому найдена инсталляция
        /// </summary>      
        [FieldCompare("Путь установки")]
        public string InstallPath { get; set; }

        /// <summary>
        /// Дата, когда был установлена инсталляция
        /// </summary>        
        [FieldCompare("Дата установки")]
        public string InstallDate { get; set; }

        /// <summary>
        /// Дата последнего запуска задачи опроса по сети для поиска инсталляций
        /// </summary>
        public string DateLastSurvey { get; set; }

        /// <summary>
        /// Дата, когда был создана запись
        /// </summary>        
        public string CreateDate { get; set; }

        public void SetField(string filedName, string value)
        {
            if(filedName == nameof(DeviceID) || filedName == nameof(DeviceClassID))
            {
                var items = value.Split(':');
                if (items != null && items.Length == 2 && int.TryParse(items[0], out var classid) && Guid.TryParse(items[1], out var id))
                {
                    DeviceClassID = classid;
                    DeviceID = id;
                }
                else
                    throw new ArgumentException($"cannot parse value '{value}' as '{filedName}'");
            }
            else if(filedName == nameof(SoftwareInstallationName))
            {
                SoftwareInstallationName = value;
            }
        }
        public bool CheckSameFieldValue(string filedName, string value)
        {
            if(filedName == nameof(DeviceID) || filedName == nameof(DeviceClassID))
            {
                var items = value.Split(':');
                if (items != null && items.Length == 2 && int.TryParse(items[0], out var classid) && Guid.TryParse(items[1], out var id))
                {
                    return DeviceClassID == classid && DeviceID == id;
                }
            }
            else if(filedName == nameof(SoftwareInstallationName))
            {
                return (string.IsNullOrEmpty(SoftwareInstallationName) ? ID.ToString() : SoftwareInstallationName) == value;
            }
            return false;
        }
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
