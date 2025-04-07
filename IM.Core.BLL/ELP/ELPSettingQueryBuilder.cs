using Inframanager.BLL;
using InfraManager.CrossPlatform.WebApi.Contracts.ELP;
using InfraManager.DAL;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ELP
{
    internal class ELPSettingQueryBuilder :
        IBuildEntityQuery<ElpSetting, ELPSettingDetails, ELPListFilter>,
        ISelfRegisteredService<IBuildEntityQuery<ElpSetting, ELPSettingDetails, ELPListFilter>>
    {
        private readonly IReadonlyRepository<ElpSetting> _repository;

        public ELPSettingQueryBuilder(IReadonlyRepository<ElpSetting> repository)
        {
            _repository = repository;
        }

        public IExecutableQuery<ElpSetting> Query(ELPListFilter filterBy)
        {
            var query = _repository
                .With(x => x.Vendor)
                .Query();

            if (!string.IsNullOrEmpty(filterBy.SearchString))
            {
                query = query.Where(x => x.Name.Contains(filterBy.SearchString));
            }

            return query;
        }
    }
}