using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.BLL.ServiceDesk.DTOs;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk
{
    public class UrgencyBLL : IUrgencyBLL, ISelfRegisteredService<IUrgencyBLL>
    {
        private readonly IRepository<Urgency> _repository;
        private readonly IFinder<Urgency> _finder;
        private readonly IMapper _mapper;
        private readonly IPagingQueryCreator _paging;
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IRepository<Concordance> _concordanceRepository;
        private readonly IRepository<Problem> _problemRepository;
        private readonly IValidatePermissions<Urgency> _validatePermissions;
        private readonly ILogger<Urgency> _logger;
        private readonly ICurrentUser _currentUser;
        public UrgencyBLL(IMapper mapper,
                          IRepository<Urgency> repository,
                          IPagingQueryCreator paging,
                          IFinder<Urgency> finder,
                          IUnitOfWork saveChangesCommand,
                          IRepository<Concordance> concordanceRepository,
                          IRepository<Problem> problemRepository,
                          IValidatePermissions<Urgency> validatePermissions,
                          ILogger<Urgency> logger,
                          ICurrentUser currentUser
            )
        {
            _repository = repository;
            _mapper = mapper;
            _paging = paging;
            _finder = finder;
            _saveChangesCommand = saveChangesCommand;
            _concordanceRepository = concordanceRepository;
            _problemRepository = problemRepository;
            _validatePermissions = validatePermissions;
            _logger = logger;
            _currentUser = currentUser;
        }
        public async Task<UrgencyDTO[]> GetListForTableAsync(string searchName, int take, int skip, CancellationToken cancellationToken)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);
            var query = _repository.Query().Where(s => s.ID != Guid.Empty);

            if (!string.IsNullOrEmpty(searchName))
                query = query.Where(s => s.Name.ToLower().Contains(searchName.ToLower()));    

            var paggingQuery = _paging.Create(query.OrderBy(x => x.Sequence));

            var result = await paggingQuery.PageAsync(skip, take, cancellationToken);

            return _mapper.Map<UrgencyDTO[]>(result);
        }

        public async Task RemoveAsync(Guid id, CancellationToken cancellationToken)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Delete, cancellationToken);
            var urgency = await _repository.FirstOrDefaultAsync(c => c.ID == id, cancellationToken);

            if (urgency is null)
                throw new ObjectNotFoundException<Guid>(id, ObjectClass.Urgency);

            if (await _problemRepository.AnyAsync(c => c.UrgencyId == id, cancellationToken))
                throw new InvalidObjectException("Этот объект используется в системе");


            await DeleteCellPriorityMatrixAsync(id, cancellationToken);
            _repository.Delete(urgency);

            await _saveChangesCommand.SaveAsync(cancellationToken);
        }


        private async Task DeleteCellPriorityMatrixAsync(Guid urgencyID, CancellationToken cancellationToken)
        {
            var existingConcordances = await _concordanceRepository.ToArrayAsync(x => x.UrgencyId == urgencyID, cancellationToken);

            foreach (var el in existingConcordances)
                _concordanceRepository.Delete(el);
        }

        public async Task<Urgency> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails, cancellationToken);
            return await _finder.FindAsync(id, cancellationToken);
        }

        public async Task<Guid> SaveAsync(UrgencyDTO model, CancellationToken cancellationToken)
        {
            var isExistingWithSameName = await _repository.AnyAsync(x => x.ID != model.ID && x.Name.Equals(model.Name), cancellationToken);
            if (isExistingWithSameName)
                throw new InvalidObjectException("Срочность с таким именем уже существует"); // TODO locale

            var entity = _mapper.Map<Urgency>(model);

            if (model.ID == Guid.Empty)
            {
                await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Insert, cancellationToken);
                var addUrgency = new Urgency(model.Name, model.Sequence);
                _repository.Insert(addUrgency);
            }
            else
            {
                await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Update, cancellationToken);
                var existingEntity = await _finder.FindAsync(model.ID, cancellationToken);

                _mapper.Map(model, existingEntity);
            }
            await _saveChangesCommand.SaveAsync(cancellationToken);
            return entity.ID;
        }

        public async Task<UrgencyListItemModel[]> ListAsync(CancellationToken cancellationToken)
        {
            var urgencies = (await _repository
                .ToArrayAsync(u => u.ID != Guid.Parse(IMSystem.Global.NULL_Urgency_ID), cancellationToken))
                .OrderBy(x => x.Sequence);

            return _mapper.Map<UrgencyListItemModel[]>(urgencies);
        }
    }
}
