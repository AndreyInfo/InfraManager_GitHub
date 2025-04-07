using IM.Core.DM.BLL.Interfaces;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using System;
using System.Threading.Tasks;

namespace IM.Core.DM.BLL
{
    internal class AssetServiceDataManagerBLL : IAssetServiceDataManagerBLL, ISelfRegisteredService<IAssetServiceDataManagerBLL>
    {
        private readonly IReadonlyRepository<Utilizer> _utilizerRepository;

        public AssetServiceDataManagerBLL(IReadonlyRepository<Utilizer> utilizerRepository)
        {
            _utilizerRepository = utilizerRepository;
        }

        public async Task<string> GetUtilizerFullNameAsync(Guid objectID)
        {
            var utilizer = await _utilizerRepository.FirstOrDefaultAsync(x => x.ID == objectID);
            if (utilizer == null)
            {
                return "Объект был удален";
            }

            return utilizer.Name;
        }
    }
}
