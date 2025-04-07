using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import.Slot;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using SlotEntity = InfraManager.DAL.Asset.Slot;

namespace IM.Core.Import.BLL.Import.Slot;
internal class SlotBLL : ISlotBLL, ISelfRegisteredService<ISlotBLL>
{
    private readonly IRepository<SlotEntity> _slotRepository;
    private readonly IRepository<SlotTemplate> _slotTemplateRepository;
    public SlotBLL(IRepository<SlotEntity> slotRepository, IRepository<SlotTemplate> slotTemplateRepository)
    {
        _slotRepository = slotRepository;
        _slotTemplateRepository = slotTemplateRepository;
    }

    public async Task CreateAsync(SlotEntity[] slots, IProtocolLogger protocolLogger, CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var slot in slots)
                _slotRepository.Insert(slot);
        }
        catch (Exception e)
        {
            protocolLogger.Information($"ERR Ошибка добавления слотов оборудования");
            protocolLogger.Error(e, $"Error Create slots");
            throw;
        }
    }

    public async Task<SlotTemplate[]> GetSlotTemplateAsync(Guid modelID, CancellationToken cancellationToken = default)
        => await _slotTemplateRepository.ToArrayAsync(x => x.ObjectID == modelID, cancellationToken);
}
