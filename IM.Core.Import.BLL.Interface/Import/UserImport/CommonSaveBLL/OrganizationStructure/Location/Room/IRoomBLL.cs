using InfraManager.DAL.Location;

namespace IM.Core.Import.BLL.Interface
{
    /// <summary>
    /// Интерфейс для сущности Комната
    /// </summary>
    public interface IRoomBLL
    {
        /// <summary>
        /// Метод производит получение комнаты из БД
        /// </summary>
        /// <param name="id">идентификатор комнаты</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task<Room> GetAsync(Guid? id, CancellationToken cancellationToken);
    }
}
