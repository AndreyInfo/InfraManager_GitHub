using AutoMapper;
using IM.Core.Import.BLL.Import;
using IM.Core.Import.BLL.Interface.Database;
using IM.Core.Import.BLL.Interface.Import;
using InfraManager.DAL;
using InfraManager.DAL.Database.Import;
using InfraManager.ServiceBase.ImportService.DBService;

namespace InfraManager.BLL.Database.Import
{
    internal class UIDBConfigurationBLL :
        BaseEntityBLL<Guid, UIDBConfiguration, UIDBConfigurationData, UIDBConfigurationOutputDetails,
            UIDBConfigurationFilter>,
        IUIDBConfigurationBLL, ISelfRegisteredService<IUIDBConfigurationBLL>
    {
        public UIDBConfigurationBLL(IRepository<UIDBConfiguration> repository,
            IMapper mapper,
            IFilterEntity<UIDBConfiguration, UIDBConfigurationFilter> filterEntity,
            IFinderQuery<Guid, UIDBConfiguration> finder,
            IUnitOfWork unitOfWork,
            IBuildModel<UIDBConfiguration, UIDBConfigurationOutputDetails> outputBuilder,
            IUpdateQuery<UIDBConfigurationData, UIDBConfiguration> updateQuery,
            IInsertQuery<UIDBConfigurationData, UIDBConfiguration> insertQuery,
            IRemoveQuery<Guid, UIDBConfiguration> removeQuery
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