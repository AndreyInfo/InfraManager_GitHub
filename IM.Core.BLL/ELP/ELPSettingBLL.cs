using InfraManager.CrossPlatform.BLL.Intrefaces.ELP;
using InfraManager.CrossPlatform.WebApi.Contracts.ELP;
using InfraManager.DAL.Asset;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Inframanager.BLL;
using InfraManager.DAL;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.ELP
{
    internal class ELPSettingBLL : StandardBLL<Guid, ElpSetting, ELPItem, ELPSettingDetails, ELPListFilter>, IELPSettingBLL, ISelfRegisteredService<IELPSettingBLL>
    {
        private readonly IMapper _mapper;

        public ELPSettingBLL(
            IMapper mapper,
            IRepository<ElpSetting> repository,
            ILogger<ELPSettingBLL> logger,
            IUnitOfWork unitOfWork, ICurrentUser currentUser,
            IBuildObject<ELPSettingDetails, ElpSetting> detailsBuilder,
            IInsertEntityBLL<ElpSetting, ELPItem> insertEntityBLL,
            IModifyEntityBLL<Guid, ElpSetting, ELPItem, ELPSettingDetails> modifyEntityBLL,
            IRemoveEntityBLL<Guid, ElpSetting> removeEntityBLL,
            IGetEntityBLL<Guid, ElpSetting, ELPSettingDetails> detailsBLL,
            IGetEntityArrayBLL<Guid, ElpSetting, ELPSettingDetails, ELPListFilter> detailsArrayBLL) : base(repository,
            logger, unitOfWork, currentUser, detailsBuilder, insertEntityBLL, modifyEntityBLL, removeEntityBLL,
            detailsBLL, detailsArrayBLL)
        {
            _mapper = mapper;
        }

        public async Task<ELPListItem[]> GetListAsync(ELPListFilter listFilter, CancellationToken cancellationToken)
        {
            var details = await GetDetailsArrayAsync(listFilter, cancellationToken);
            var listItems = details.Select(d => _mapper.Map<ELPListItem>(d)).ToArray();
            // TODO: заполнить остальные поля ELPListItem после миграции ScheduleBLL
            return listItems;
        }
    }
}