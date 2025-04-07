using AutoMapper;
using IM.Core.Import.BLL.Import;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager;
using InfraManager.DAL;
using Inframanager.DAL.ActiveDirectory.Import;

namespace IM.Core.Import.BLL.Ldap.Import
{
    internal class UIADPathsBLL :
        BaseEntityBLL<Guid, UIADPath, UIADPathsDetails, UIADPathsOutputDetails, UIADPathsFilter>,
        IUIADPathsBLL, ISelfRegisteredService<IUIADPathsBLL>
    {
        public UIADPathsBLL(IRepository<UIADPath> entities,
            IMapper mapper,
            IFilterEntity<UIADPath, UIADPathsFilter> filterEntity,
            IFinderQuery<Guid, UIADPath> finder,
            IUnitOfWork unitOfWork,
            IBuildModel<UIADPath, UIADPathsOutputDetails> outputBuilder,
            IUpdateQuery<UIADPathsDetails, UIADPath> updateQuery,
            IInsertQuery<UIADPathsDetails, UIADPath> insertQuery,
            IRemoveQuery<Guid, UIADPath> removeQuery) : base(entities,
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