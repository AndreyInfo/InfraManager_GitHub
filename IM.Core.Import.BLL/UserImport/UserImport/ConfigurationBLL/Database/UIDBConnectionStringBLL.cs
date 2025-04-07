using AutoMapper;
using IM.Core.Import.BLL.Import;
using IM.Core.Import.BLL.Interface.Database;
using IM.Core.Import.BLL.Interface.Import;
using InfraManager.DAL;
using InfraManager.DAL.Database.Import;
using InfraManager.ServiceBase.ImportService.DBService;

namespace InfraManager.BLL.Database.Import
{
    internal class UIDBConnectionStringBLL :
        BaseEntityBLL<Guid, UIDBConnectionString, UIDBConnectionStringData, UIDBConnectionStringOutputDetails,
            UIDBConnectionStringFilter>,
        IUIDBConnectionStringBLL, ISelfRegisteredService<IUIDBConnectionStringBLL>
    {
        public UIDBConnectionStringBLL(IRepository<UIDBConnectionString> repository,
            IMapper mapper,
            IFilterEntity<UIDBConnectionString, UIDBConnectionStringFilter> filterEntity,
            IFinderQuery<Guid, UIDBConnectionString> finder,
            IUnitOfWork unitOfWork,
            IBuildModel<UIDBConnectionString, UIDBConnectionStringOutputDetails> outputBuilder,
            IUpdateQuery<UIDBConnectionStringData, UIDBConnectionString> updateQuery,
            IInsertQuery<UIDBConnectionStringData, UIDBConnectionString> insertQuery,
            IRemoveQuery<Guid, UIDBConnectionString> removeQuery
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