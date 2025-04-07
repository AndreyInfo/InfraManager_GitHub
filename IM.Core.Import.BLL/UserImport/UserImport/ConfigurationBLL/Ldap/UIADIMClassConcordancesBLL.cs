using AutoMapper;
using IM.Core.Import.BLL.Import;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager;
using InfraManager.DAL;
using Inframanager.DAL.ActiveDirectory.Import;

namespace IM.Core.Import.BLL.Ldap.Import
{
    internal class UIADIMClassConcordancesBLL :
        BaseEntityBLL<UIADIMClassConcordancesKey, UIADIMClassConcordance, UIADIMClassConcordancesDetails,
            UIADIMClassConcordancesOutputDetails, UIADIMClassConcordancesFilter>,
        IUIADIMClassConcordancesBLL, ISelfRegisteredService<IUIADIMClassConcordancesBLL>
    {
        public UIADIMClassConcordancesBLL(IRepository<UIADIMClassConcordance> entities,
            IMapper mapper,
            IFilterEntity<UIADIMClassConcordance, UIADIMClassConcordancesFilter> filterEntity,
            IFinderQuery<UIADIMClassConcordancesKey, UIADIMClassConcordance> finder,
            IUnitOfWork unitOfWork,
            IBuildModel<UIADIMClassConcordance, UIADIMClassConcordancesOutputDetails> outputBuilder,
            IUpdateQuery<UIADIMClassConcordancesDetails, UIADIMClassConcordance> updateQuery,
            IInsertQuery<UIADIMClassConcordancesDetails, UIADIMClassConcordance> insertQuery,
            IRemoveQuery<UIADIMClassConcordancesKey, UIADIMClassConcordance> removeQuery) : base(entities,
            mapper,
            filterEntity,
            finder,
            unitOfWork,
            outputBuilder,
            updateQuery,
            insertQuery,
            removeQuery)
        {
        }
    }
}