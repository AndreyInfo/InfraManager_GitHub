namespace InfraManager.Web.Controllers.Models.LicenceSchemes
{
    /// <summary>
    /// Cхема лицензирования (для списка)
    /// </summary>
    public sealed class LicenceSchemeListItem
    {
        /// <summary>
        /// Идентификатор схемы лицензирования
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Объект лицензирования
        /// </summary>
        public string Object { get; set; }

        /// <summary>
        /// Вид схемы
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public string CreatedDate { get; set; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        public string UpdatedDate { get; set; }

        /// <summary>
        /// Признак удаленной схемы лицензирования
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// Признак пользовательской схемы лицензирования
        /// </summary>
        public bool IsUserLicenceScheme { get; set; }
    }
}