using IM.Core.Import.BLL.Interface;
using InfraManager.DAL;
using InfraManager.DAL.Import;


namespace IM.Core.Import.BLL.Import.OSU
{
    public class ImportDeleteSettings<T> where T : UISettingBase
    {
        private readonly IRepository<T> _settingsRepository;

        public ImportDeleteSettings(IRepository<T> settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task DeleteUISettingAsync(Guid id, CancellationToken cancellationToken)
        {
            var setting = await _settingsRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
            if (setting != null)
            {
                _settingsRepository.Delete(setting);
            }
        }

        
    }
}
