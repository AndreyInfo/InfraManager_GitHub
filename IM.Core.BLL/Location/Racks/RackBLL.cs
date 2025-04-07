using Inframanager.BLL;
using InfraManager.DAL;
using Microsoft.Extensions.Logging;
using InfraManager.BLL.Asset.dto;
using InfraManager.BLL.Asset.Filters;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.Location.Racks
{
    internal class RackBLL :
        StandardBLL<int, Rack, RackData, RackDetails, RackListFilter>,
        IRackBLL,
        ISelfRegisteredService<IRackBLL>
    {
        public RackBLL(
            IRepository<Rack> repository,
            ILogger<RackBLL> logger,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IBuildObject<RackDetails, Rack> detailsBuilder,
            IInsertEntityBLL<Rack, RackData> insertEntityBLL,
            IModifyEntityBLL<int, Rack, RackData, RackDetails> modifyEntityBLL,
            IRemoveEntityBLL<int, Rack> removeEntityBLL,
            IGetEntityBLL<int, Rack, RackDetails> detailsBLL,
            IGetEntityArrayBLL<int, Rack, RackDetails, RackListFilter> detailsArrayBLL) :
            base(
                repository,
                logger,
                unitOfWork,
                currentUser,
                detailsBuilder,
                insertEntityBLL,
                modifyEntityBLL,
                removeEntityBLL,
                detailsBLL,
                detailsArrayBLL)
        { }
    }
}