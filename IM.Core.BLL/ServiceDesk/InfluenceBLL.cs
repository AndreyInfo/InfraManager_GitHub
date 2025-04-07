using System;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.DAL.ServiceDesk;
using InfraManager.BLL.Asset.dto;
using InfraManager.DAL;
using System.Threading;
using System.Linq;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using Microsoft.Extensions.Logging;
using Inframanager;

namespace InfraManager.BLL.ServiceDesk
{
    internal class InfluenceBLL : IInfluenceBLL, ISelfRegisteredService<IInfluenceBLL>
    {
        private readonly IRepository<Influence> _repository;
        private readonly IRepository<Concordance> _concordanceRepository;
        private readonly IRepository<Problem> _problemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidatePermissions<Influence> _validatePermissions;
        private readonly ILogger<Influence> _logger;
        private readonly ICurrentUser _currentUser;

        private readonly IMapper _mapper;
        public InfluenceBLL(
            IMapper mapper,
            IRepository<Influence> repository,
            IUnitOfWork unitOfWork, 
            IRepository<Concordance> concordanceRepository,
            IRepository<Problem> problemRepository,
            IValidatePermissions<Influence> validatePermissions,
            ILogger<Influence> logger,
            ICurrentUser currentUser)
        {
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _concordanceRepository = concordanceRepository;
            _problemRepository = problemRepository;
            _validatePermissions = validatePermissions;
            _logger = logger;
            _currentUser = currentUser;
        }

        public async Task<InfluenceDetails[]> GetAllInfluenceAsync(CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);
            var InfluenceList = await _repository.ToArrayAsync(x => x.ID != default, cancellationToken);
            return _mapper.Map<InfluenceDetails[]>(InfluenceList);
        }

        public async Task RemoveByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Delete, cancellationToken);
            
            var existingInfluency = await _repository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken)
                ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.Influence);

            if (await _problemRepository.AnyAsync(c => c.InfluenceId == id, cancellationToken))
                throw new InvalidObjectException("Этот объект используется в системе");//TODO localization

            await DeleteCellPriorityMatrixAsync(id, cancellationToken);
            _repository.Delete(existingInfluency);

            await _unitOfWork.SaveAsync(cancellationToken);
            
        }

        private async Task DeleteCellPriorityMatrixAsync(Guid influenceID, CancellationToken cancellationToken)
        {
            var existingConcordances = await _concordanceRepository.ToArrayAsync(x => x.InfluenceId == influenceID, cancellationToken);

            foreach (var el in existingConcordances) 
                _concordanceRepository.Delete(el);
        }

        public async Task<bool> SaveAsync(InfluenceDetails influence, CancellationToken cancellationToken = default)
        {
            var isExistingWithSameName = await _repository.AnyAsync(x => x.ID != influence.ID && x.Name.Equals(influence.Name), cancellationToken);
            if (isExistingWithSameName)
                throw new InvalidObjectException("Влияние с таким именем уже существует");

            var model = _mapper.Map<Influence>(influence);

            if (model.ID == Guid.Empty)
            {
                await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Insert, cancellationToken);
                var addInfluence = new Influence(model.Name, model.Sequence);
                _repository.Insert(addInfluence);
            }
            else
            {
                await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Update, cancellationToken);
                var entity = await _repository.FirstOrDefaultAsync(x => x.ID == influence.ID, cancellationToken);

                if (entity is null)
                    throw new ObjectNotFoundException<Guid>(influence.ID, ObjectClass.Influence);

                _mapper.Map(model, entity);
            }

            await _unitOfWork.SaveAsync(cancellationToken);

            return true;
        }

        public async Task<InfluenceListItemModel[]> ListAsync(CancellationToken cancellationToken)
        {
            var influencies = (await _repository
                .ToArrayAsync(u => u.ID != Guid.Parse(IMSystem.Global.NULL_Urgency_ID), cancellationToken))
                .OrderBy(x => x.Sequence);

            return _mapper.Map<InfluenceListItemModel[]>(influencies);
        }

    }
}
