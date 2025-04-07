namespace InfraManager.Web.Controllers.Models.LicenceSchemes
{
    /// <summary>
    /// Схема лицензирования
    /// </summary>
    public class LicenceScheme
    {
        /// <summary>
        /// Идентификатор схемы лицензирования
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Название схемы лицензирования
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание схемы лицензирования
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Объект лицензирования (ID)
        /// </summary>
        public int LicenceObjectId { get; set; }

        /// <summary>
        /// Объект лицензирования (ID)
        /// </summary>
        public string LicenceObjectName { get; set; }

        /// <summary>
        /// Требуемое кол-во прав на объект
        /// </summary>
        public string RequiredNumberOfRightsToObject { get; set; }
        
        /// <summary>
        /// Привязывать права к объектам
        /// </summary>
        public bool IsBindRightsToObjects { get; set; }

        /// <summary>
        /// Привязывать права к объектам
        /// </summary>
        public bool IsLicenseUserAccess { get; set; }

        /// <summary>
        /// Лицензировать все хосты кластера (для кластеров)
        /// </summary>
        public bool IsLicenseAllClusterHosts { get; set; }

        /// <summary>
        /// Допустимое число инсталляций на сервере бесконечно
        /// </summary>
        public bool AllowedNumberOfInstallationsOnTheServerUnlimit { get; set; }

        /// <summary>
        /// Кол-во  допустимых инсталляций на сервере
        /// </summary>
        public int AllowedNumberOfInstallationsOnTheServerCount { get; set; }

        /// <summary>
        /// Число инсталляций на виртуальных машинах (тип)
        /// </summary>
        public int NumberOfInstallationsOnVirtualMachinesType { get; set; }

        /// <summary>
        /// Число инсталляций на виртуальных машинах
        /// </summary>
        public int NumberOfInstallationsOnVirtualMachinesCount { get; set; }

        /// <summary>
        /// Кол-во дополнительных прав
        /// </summary>
        public string NumberOfAdditionalRights { get; set; }

        /// <summary>
        /// Размер увеличения числа инсталляций на виртуальных машинах
        /// </summary>
        public int SizeOfIncreaseInNumberOfInstallations { get; set; }

        /// <summary>
        /// Вид схемы лицензирования
        /// </summary>
        public int LicenceSchemeType { get; set; }

        /// <summary>
        /// Название вида схемы лицензирования
        /// </summary>
        public string LicenceSchemeTypeName { get; set; }
    }
}