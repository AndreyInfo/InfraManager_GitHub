namespace InfraManager.Web.Controllers.Models.LicenceObjects
{
    /// <summary>
    /// Объект лицензирования
    /// </summary>
    public sealed class LicenceObject
    {
        /// Идентификатор объекта лицензирвоания
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Название объекта лицензирования
        /// </summary>
        public string Name { get; set; }
    }
}