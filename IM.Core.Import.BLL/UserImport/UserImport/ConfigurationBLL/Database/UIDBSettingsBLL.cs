
	using AutoMapper;
	using IM.Core.Import.BLL.Import;
	using IM.Core.Import.BLL.Interface.Database;
	using IM.Core.Import.BLL.Interface.Import;
	using InfraManager.DAL;
	using InfraManager.DAL.Database.Import;
	using InfraManager.ServiceBase.ImportService.DBService;

	namespace InfraManager.BLL.Database.Import
	{
	    internal class UIDBSettingsBLL:
	        BaseEntityBLL<Guid,UIDBSettings,UIDBSettingsData, UIDBSettingsOutputDetails, UIDBSettingsFilter>,
	        IUIDBSettingsBLL,ISelfRegisteredService<IUIDBSettingsBLL>
	    {

	        public UIDBSettingsBLL(IRepository<UIDBSettings> repository,
	            IMapper mapper,
	            IFilterEntity<UIDBSettings,UIDBSettingsFilter> filterEntity,
	            IFinderQuery<Guid,UIDBSettings>  finder,
                IUnitOfWork unitOfWork, 
                IBuildModel<UIDBSettings, UIDBSettingsOutputDetails> outputBuilder, 
                IUpdateQuery<UIDBSettingsData, UIDBSettings> updateQuery,
                IInsertQuery<UIDBSettingsData, UIDBSettings> insertQuery, 
                IRemoveQuery<Guid, UIDBSettings> removeQuery
	            ) : base(repository,
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

	