using AutoMapper;
using IM.Core.Import.BLL.Import;
using IM.Core.Import.BLL.Interface.Database;
using IM.Core.Import.BLL.Interface.Import;
using InfraManager.DAL;
using InfraManager.DAL.Database.Import;
using InfraManager.ServiceBase.ImportService.DBService;

namespace InfraManager.BLL.Database.Import
{
    internal class UIDBFieldsBLL :
        BaseEntityBLL<Guid, UIDBFields, UIDBFieldsData, UIDBFieldsOutputDetails, UIDBFieldsFilter>,
        IUIDBFieldsBLL, ISelfRegisteredService<IUIDBFieldsBLL>
    {
        public UIDBFieldsBLL(IRepository<UIDBFields> repository,
            IMapper mapper,
            IFilterEntity<UIDBFields, UIDBFieldsFilter> filterEntity,
            IFinderQuery<Guid, UIDBFields> finder,
            IUnitOfWork unitOfWork,
            IBuildModel<UIDBFields, UIDBFieldsOutputDetails> outputBuilder,
            IUpdateQuery<UIDBFieldsData, UIDBFields> updateQuery,
            IInsertQuery<UIDBFieldsData, UIDBFields> insertQuery,
            IRemoveQuery<Guid, UIDBFields> removeQuery
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