namespace InfraManager.Web.Controllers.Models.LicenceSchemes
{
    /// <summary>
    /// Запрос на получение списка лицензирования
    /// </summary>
    public sealed class LicenceSchemeFilterRequest
    {
        /// <summary>
        /// Название схемы лицензирования
        /// </summary>
        public string LicenceSchemeName { get; set; }

        /// <summary>
        /// Показывать удаленные
        /// </summary>
        public bool IsShowDeletedRecords { get; set; }


        public override string ToString()
        {
            return $"LicenceSchemeName: {LicenceSchemeName}, IsShowRecords: {IsShowDeletedRecords}";
        }
    }
}