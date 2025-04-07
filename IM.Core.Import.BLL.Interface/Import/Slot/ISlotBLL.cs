using InfraManager.DAL.Asset;
using SlotEntity = InfraManager.DAL.Asset.Slot;

namespace IM.Core.Import.BLL.Interface.Import.Slot;
public interface ISlotBLL
{
    /// <summary>
    /// Метод создает слоты оборудования
    /// </summary>
    /// <param name="slots">объекты для создания</param>
    /// <param name="protocolLogger">протокол импорта</param>
    /// <param name="cancellationToken">отмена задачи</param>
    Task CreateAsync(SlotEntity[] slots, IProtocolLogger protocolLogger, CancellationToken cancellationToken = default);

    /// <summary>
    /// Метод получает массив слотов оборудования из шаблона
    /// </summary>
    /// <param name="modelID">идентификатор модели оборудования</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns>массив слотов оборудования из шаблона</returns>
    Task<SlotTemplate[]> GetSlotTemplateAsync(Guid modelID, CancellationToken cancellationToken = default);
}
