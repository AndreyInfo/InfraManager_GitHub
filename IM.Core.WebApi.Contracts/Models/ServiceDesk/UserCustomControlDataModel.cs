using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk
{
    /// <summary>
    /// Этот класс описывает модель входных данных "На контроле" у пользователя
    /// </summary>
    public class UserCustomControlDataModel : CustomControlDataModel
    {
        /// <summary>
        /// Возвращает идентификатор пользователя на контроле у которого находится (или не находится) объект
        /// </summary>
        public Guid UserID { get; init; }
    }
}
