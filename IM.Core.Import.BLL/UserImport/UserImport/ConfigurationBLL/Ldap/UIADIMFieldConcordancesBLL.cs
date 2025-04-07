using AutoMapper;
using IM.Core.DM.BLL.Interfaces;
using IM.Core.Import.BLL.Import;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager;
using InfraManager.DAL;
using Inframanager.DAL.ActiveDirectory.Import;
using InfraManager.ServiceBase.ImportService.LdapModels;

namespace IM.Core.Import.BLL.Ldap.Import
{
    internal class UIADIMFieldConcordancesBLL :
        BaseEntityBLL<UIADIMFieldConcordancesKey, UIADIMFieldConcordance, UIADIMFieldConcordancesDetails,
            UIADIMFieldConcordancesOutputDetails, UIADIMFieldConcordancesFilter>,
        IUIADIMFieldConcordancesBLL, ISelfRegisteredService<IUIADIMFieldConcordancesBLL>
    {
        private readonly IInsertQuery<UIADIMFieldConcordancesDetails, UIADIMFieldConcordance> _insertQuery;
        private readonly IUpdateQuery<UIADIMFieldConcordancesDetails, UIADIMFieldConcordance> _updateQuery;
        private readonly IRemoveQuery<UIADIMFieldConcordancesKey, UIADIMFieldConcordance> _removeQuery;

        public UIADIMFieldConcordancesBLL(IRepository<UIADIMFieldConcordance> entities,
            IMapper mapper,
            IFilterEntity<UIADIMFieldConcordance, UIADIMFieldConcordancesFilter> filterEntity,
            IFinderQuery<UIADIMFieldConcordancesKey, UIADIMFieldConcordance> finder,
            IUnitOfWork unitOfWork,
            IBuildModel<UIADIMFieldConcordance, UIADIMFieldConcordancesOutputDetails> outputBuilder,
            IUpdateQuery<UIADIMFieldConcordancesDetails, UIADIMFieldConcordance> updateQuery,
            IInsertQuery<UIADIMFieldConcordancesDetails, UIADIMFieldConcordance> insertQuery,
            IRemoveQuery<UIADIMFieldConcordancesKey, UIADIMFieldConcordance> removeQuery) : base(entities,
            mapper,
            filterEntity,
            finder,
            unitOfWork,
            outputBuilder,
            updateQuery,
            insertQuery,
            removeQuery)
        {
            _removeQuery = removeQuery;
            _updateQuery = updateQuery;
            _insertQuery = insertQuery;
        }
        
        public async Task AddConfig(Guid configurationID, FieldConfiguration configuration, CancellationToken token)
        {
            if (!GetFieldEnum(configuration.FieldName, out ConcordanceObjectType concordanceEnum)) return;

            var details = new UIADIMFieldConcordanceInsertDetails(adConfigurationID: configurationID,
                imFieldID: (long) concordanceEnum, adClassID: configuration.ClassID, expression: configuration.Value);
            await _insertQuery.AddAsync(details, token);
        }

        public async Task UpdateConfig(UIADIMFieldConcordance source, string expression,
            CancellationToken token)
        {
            var details = new UIADIMFieldConcordancesDetails
            {
                Expression = expression
            };

            await _updateQuery.UpdateAsync(source, details, token);
        }

        public async Task RemoveConfig(UIADConfiguration entity, IEnumerable<(string name, Guid classId)> configs, CancellationToken token)
        {
            foreach (var config in configs)
            {
                if (!GetFieldEnum(config.name, out ConcordanceObjectType concordanceEnum)) return;
                var key = new UIADIMFieldConcordancesKey()
                {
                    ADClassID = config.classId,
                    ADConfigurationID = entity.ID,
                    IMFieldID = (long) concordanceEnum
                };
                await _removeQuery.RemoveAsync(key, token);
            }
       
        }

        private static bool GetFieldEnum(string fieldName, out  ConcordanceObjectType concordanceEnum)
        {
            return Enum.TryParse(fieldName, out concordanceEnum);
        }
    }
}