using AutoMapper;
using InfraManager.DAL;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using Microsoft.Extensions.Logging;
using Rule = InfraManager.DAL.ServiceCatalogue.Rule;

namespace InfraManager.BLL.ServiceCatalogue.Rules
{

    internal class RuleBLL : IRuleBLL, ISelfRegisteredService<IRuleBLL>
    {
        private readonly IMapper _mapper;
        private readonly IRuleValueConverter _ruleValueConverter;
        private readonly IRepository<Rule> _repository;
        private readonly IFinder<Rule> _ruleFinder;
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IPagingQueryCreator _pagingQuery;
        private readonly IValidatePermissions<Rule> _slaPermissions;
        private readonly ILogger<Rule> _logger;
        private readonly ICurrentUser _currentUser;
        private readonly IRemoveEntityBLL<Guid, Rule> _removeBLL;
        private readonly IModifyEntityBLL<Guid, Rule, RuleData, RuleDetails> _modifyEntityBLL;

        public RuleBLL(IMapper mapper,
                       IRuleValueConverter ruleValueConverter,
                       IFinder<Rule> ruleFinder,
                       IRepository<Rule> repository,
                       IUnitOfWork saveChangesCommand,
                       IPagingQueryCreator pagingQuery,
                       IValidatePermissions<Rule> slaPermissions,
                       ILogger<Rule> logger, ICurrentUser currentUser,
                       IRemoveEntityBLL<Guid, Rule> removeBLL,
                       IModifyEntityBLL<Guid, Rule, RuleData, RuleDetails> modifyEntityBLL)
        {
            _mapper = mapper;
            _ruleValueConverter = ruleValueConverter;
            _ruleFinder = ruleFinder;
            _repository = repository;
            _saveChangesCommand = saveChangesCommand;
            _pagingQuery = pagingQuery;
            _slaPermissions = slaPermissions;
            _logger = logger;
            _currentUser = currentUser;
            _removeBLL = removeBLL;
            _modifyEntityBLL = modifyEntityBLL;
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _removeBLL.RemoveAsync(id, cancellationToken);
            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

        public async Task<RuleDetails[]> ListAsync(RuleFilter filter, CancellationToken cancellationToken = default)
        {

            await _slaPermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray,
                cancellationToken);

            var query = _repository.Query();

            if (!string.IsNullOrWhiteSpace(filter.SearchString))
            {
                query = query.Where(x => x.Name.Contains(filter.SearchString));
            }

            if (filter.SLAID.HasValue)
            {
                query = query.Where(x => x.ServiceLevelAgreementID == filter.SLAID);
            }
            
            if (filter.OperationalLevelAgreementID.HasValue)
            {
                query = query.Where(x => x.OperationalLevelAgreementID == filter.OperationalLevelAgreementID);
            }

            var paging = _pagingQuery.Create(query.OrderBy(x => x.Sequence));

            var entities = await paging.PageAsync(filter.StartRecordIndex, filter.CountRecords, cancellationToken);

            _logger.LogTrace($"User ID = {_currentUser.UserId} got {filter.CountRecords} Rule items");
            return _mapper.Map<RuleDetails[]>(entities);
        }

        public async Task<RuleValueDetails> GetRuleValueAsync(Guid ruleID,
            CancellationToken cancellationToken = default)
        {
            await _slaPermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails,
                cancellationToken);

            var found = await FindAndThrowIfNoExists(ruleID, cancellationToken);

            var ruleValue = await _ruleValueConverter.ConvertFromBytesAsync(found.Value, cancellationToken);

            _logger.LogTrace($"User ID = {_currentUser.UserId} got Value Rule with ID = {ruleID}");
            return _mapper.Map<RuleValueDetails>(ruleValue);
        }

        public async Task<RuleDetails> InsertAsync(RuleData rule, CancellationToken cancellationToken = default)
        {
            await _slaPermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Insert,
                cancellationToken);

            var newRule = _mapper.Map<RuleData, Rule>(rule);

            _repository.Insert(newRule);

            await _saveChangesCommand.SaveAsync(cancellationToken);

            _logger.LogTrace($"User ID = {_currentUser.UserId} inserted new Rule");
            return _mapper.Map<RuleDetails>(newRule);
        }

        public async Task UpdateAsync(Guid ruleID, RuleData data, CancellationToken cancellationToken = default)
        {
            await _modifyEntityBLL.ModifyAsync(ruleID, data, cancellationToken);
            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

        public async Task InsertValueAsync(Guid ruleID, RuleValueDetails ruleValue,
            CancellationToken cancellationToken = default)
        {
            await _slaPermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Insert,
                cancellationToken);

            await SaveOrUpdateRuleAsync(ruleID, ruleValue, cancellationToken);
        }

        public async Task UpdateValueAsync(Guid ruleID, RuleValueDetails ruleValue,
            CancellationToken cancellationToken = default)
        {
            await _slaPermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Update,
                cancellationToken);

            await SaveOrUpdateRuleAsync(ruleID, ruleValue, cancellationToken);
        }

        private async Task SaveOrUpdateRuleAsync(Guid ruleID, RuleValueDetails ruleValue,
            CancellationToken cancellationToken = default)
        {
            var rule = await FindAndThrowIfNoExists(ruleID, cancellationToken);

            var ruleValueData = _mapper.Map<RuleValue>(ruleValue);
            rule.Value = await _ruleValueConverter.ConvertToBytesAsync(ruleValueData, cancellationToken);

            _logger.LogTrace($"User ID = {_currentUser.UserId} Updated Value Rule with ID = {ruleID}");
            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

        private async Task<Rule> FindAndThrowIfNoExists(Guid ruleID, CancellationToken cancellationToken = default)
        {
            return await _ruleFinder.FindAsync(ruleID, cancellationToken) ??
                             throw new ObjectNotFoundException($"Rule Not Found; ID = {ruleID}");
        }
    }
}
