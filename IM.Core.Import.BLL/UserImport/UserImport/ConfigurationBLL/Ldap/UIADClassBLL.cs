using AutoMapper;
using IM.Core.Import.BLL.Import;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager;
using InfraManager.DAL;
using Inframanager.DAL.ActiveDirectory.Import;

namespace IM.Core.Import.BLL.Ldap.Import
{
    internal class UIADClassBLL :
        BaseEntityBLL<Guid, UIADClass, UIADClassDetails, UIADClassOutputDetails, UIADClassFilter>,
        IUIADClassBLL, ISelfRegisteredService<IUIADClassBLL>
    {
        public UIADClassBLL(IRepository<UIADClass> entities,
            IMapper mapper,
            IFilterEntity<UIADClass, UIADClassFilter> filterEntity,
            IFinderQuery<Guid, UIADClass> finder,
            IUnitOfWork unitOfWork,
            IBuildModel<UIADClass, UIADClassOutputDetails> outputBuilder,
            IUpdateQuery<UIADClassDetails, UIADClass> updateQuery,
            IInsertQuery<UIADClassDetails, UIADClass> insertQuery,
            IRemoveQuery<Guid, UIADClass> removeQuery) : base(entities,
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