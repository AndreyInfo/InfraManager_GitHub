namespace InfraManager.WebApi.Contracts.Models.ServiceDesk
{
    /// <summary>
    /// Этот класс описывает модель входных данных "На контроле"
    /// </summary>
    public class CustomControlDataModel
    {
        /// <summary>
        /// Возвращает признак нахождения объекта на контроле
        /// </summary>
        public bool UnderControl { get; init; }
    }
}
