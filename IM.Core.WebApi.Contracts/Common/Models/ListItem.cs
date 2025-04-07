namespace InfraManager.CrossPlatform.WebApi.Contracts.Common.Models
{
    /// <summary>
    /// Общий класс для представления элементов списокв для выбора
    /// </summary>
    public class ListItem
    {
        /// <summary>
        /// Значение для выбора
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование значения
        /// </summary>
        public string Name { get; set; }
    }
}
