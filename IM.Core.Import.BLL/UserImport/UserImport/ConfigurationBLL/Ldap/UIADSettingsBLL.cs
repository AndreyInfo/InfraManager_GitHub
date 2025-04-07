using AutoMapper;
using IM.Core.Import.BLL.Import;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager;
using InfraManager.DAL;
using Inframanager.DAL.ActiveDirectory.Import;

namespace IM.Core.Import.BLL.Ldap.Import
{
    internal class UIADSettingsBLL :
        BaseEntityBLL<Guid, UIADSetting, UIADSettingsDetails, UIADSettingsOutputDetails, UIADSettingsFilter>,
        IUIADSettingsBLL, ISelfRegisteredService<IUIADSettingsBLL>
    {
        public UIADSettingsBLL(IRepository<UIADSetting> entities,
            IMapper mapper,
            IFilterEntity<UIADSetting, UIADSettingsFilter> filterEntity,
            IFinderQuery<Guid, UIADSetting> finder,
            IUnitOfWork unitOfWork,
            IBuildModel<UIADSetting, UIADSettingsOutputDetails> outputBuilder,
            IUpdateQuery<UIADSettingsDetails, UIADSetting> updateQuery,
            IInsertQuery<UIADSettingsDetails, UIADSetting> insertQuery,
            IRemoveQuery<Guid, UIADSetting> removeQuery) : base(entities,
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