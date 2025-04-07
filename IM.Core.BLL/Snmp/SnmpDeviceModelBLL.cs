using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Snmp;
using Microsoft.Extensions.Logging;
using System;

namespace InfraManager.BLL.Snmp
{
    internal class SnmpDeviceModelBLL : 
        StandardBLL<Guid, SnmpDeviceModel, SnmpDeviceModelData, SnmpDeviceModelDetails, SnmpDeviceModelFilter>, 
        ISnmpDeviceModelBLL, 
        ISelfRegisteredService<ISnmpDeviceModelBLL>
    {
        public SnmpDeviceModelBLL(
            IRepository<SnmpDeviceModel> repository,
            ILogger<SnmpDeviceModelBLL> logger,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IBuildObject<SnmpDeviceModelDetails, SnmpDeviceModel> detailsBuilder,
            IInsertEntityBLL<SnmpDeviceModel, SnmpDeviceModelData> insertEntityBLL,
            IModifyEntityBLL<Guid, SnmpDeviceModel, SnmpDeviceModelData, SnmpDeviceModelDetails> modifyEntityBLL,
            IRemoveEntityBLL<Guid, SnmpDeviceModel> removeEntityBLL,
            IGetEntityBLL<Guid, SnmpDeviceModel, SnmpDeviceModelDetails> detailsBLL,
            IGetEntityArrayBLL<Guid, SnmpDeviceModel, SnmpDeviceModelDetails, SnmpDeviceModelFilter> detailsArrayBLL) : base(
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
        {
        }
    }
}
