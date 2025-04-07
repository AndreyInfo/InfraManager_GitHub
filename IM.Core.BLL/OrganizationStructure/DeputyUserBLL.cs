using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.Extensions.Logging;
using System;

namespace InfraManager.BLL.OrganizationStructure
{
    internal class DeputyUserBLL :
        StandardBLL<Guid, DeputyUser, DeputyUserData, DeputyUserDetails, DeputyUserListFilter>,
        IDeputyUserBLL, ISelfRegisteredService<IDeputyUserBLL>
    {
        public DeputyUserBLL(
            IRepository<DeputyUser> repository,
            ILogger<DeputyUserBLL> logger,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IBuildObject<DeputyUserDetails, DeputyUser> detailsBuilder,
            IInsertEntityBLL<DeputyUser, DeputyUserData> insertEntityBLL,
            IModifyEntityBLL<Guid, DeputyUser, DeputyUserData, DeputyUserDetails> modifyEntityBLL,
            IRemoveEntityBLL<Guid, DeputyUser> removeEntityBLL,
            IGetEntityBLL<Guid, DeputyUser, DeputyUserDetails> detailsBLL,
            IGetEntityArrayBLL<Guid, DeputyUser, DeputyUserDetails, DeputyUserListFilter> detailsArrayBLL)
            : base(repository, logger, unitOfWork, currentUser, detailsBuilder, insertEntityBLL, modifyEntityBLL, removeEntityBLL, detailsBLL, detailsArrayBLL)
        {
        }
    }
}