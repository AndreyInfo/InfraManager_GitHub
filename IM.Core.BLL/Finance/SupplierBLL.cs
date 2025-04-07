using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Finance;
using Microsoft.Extensions.Logging;
using System;

namespace InfraManager.BLL.Finance
{
    internal class SupplierBLL :
        StandardBLL<Guid, Supplier, SupplierData, SupplierDetails, LookupListFilter>,
        ISupplierBLL,
        ISelfRegisteredService<ISupplierBLL>
    {
        public SupplierBLL(
            IRepository<Supplier> repository,
            ILogger<SupplierBLL> logger,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IBuildObject<SupplierDetails, Supplier> detailsBuilder,
            IInsertEntityBLL<Supplier, SupplierData> insertEntityBLL,
            IModifyEntityBLL<Guid, Supplier, SupplierData, SupplierDetails> modifyEntityBLL,
            IRemoveEntityBLL<Guid, Supplier> removeEntityBLL,
            IGetEntityBLL<Guid, Supplier, SupplierDetails> detailsBLL,
            IGetEntityArrayBLL<Guid, Supplier, SupplierDetails, LookupListFilter> detailsArrayBLL)
            : base(
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
