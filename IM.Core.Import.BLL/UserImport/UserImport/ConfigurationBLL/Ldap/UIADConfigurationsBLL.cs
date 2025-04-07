using AutoMapper;
using IM.Core.Import.BLL.Import;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager;
using InfraManager.DAL;
using Inframanager.DAL.ActiveDirectory.Import;

namespace IM.Core.Import.BLL.Ldap.Import
{
    internal class UIADConfigurationsBLL :
        BaseEntityBLL<Guid, UIADConfiguration, UIADConfigurationsDetails, UIADConfigurationsOutputDetails,
            UIADConfigurationsFilter>,
        IUIADConfigurationsBLL, ISelfRegisteredService<IUIADConfigurationsBLL>
    {
        public UIADConfigurationsBLL(IRepository<UIADConfiguration> entities,
            IMapper mapper,
            IFilterEntity<UIADConfiguration, UIADConfigurationsFilter> filterEntity,
            IFinderQuery<Guid, UIADConfiguration> finder,
            IUnitOfWork unitOfWork,
            IBuildModel<UIADConfiguration, UIADConfigurationsOutputDetails> outputBuilder,
            IUpdateQuery<UIADConfigurationsDetails, UIADConfiguration> updateQuery,
            IInsertQuery<UIADConfigurationsDetails, UIADConfiguration> insertQuery,
            IRemoveQuery<Guid, UIADConfiguration> removeQuery) : base(entities,
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