using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using InfraManager.DAL.ServiceDesk;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;
using InfraManager.BLL.ServiceDesk.DTOs;
using InfraManager.DAL;
using System.Threading;
using InfraManager.Core.Helpers;

namespace InfraManager.BLL.ServiceDesk
{
    internal class PriorityMatrixBLL : IPriorityMatrixBLL, ISelfRegisteredService<IPriorityMatrixBLL>
    {
        private readonly IMapper _mapper;
        private readonly IFinder<Priority> _priorityFinder; 
        private readonly IFinder<Influence> _influenceFinder;
        private readonly IFinder<Concordance> _concordanceFinder;
        private readonly IRepository<Concordance> _concordanceRepository;
        private readonly IUnitOfWork _unitOfWork;
        const string DefaultColor = "#f2fafd";
        public PriorityMatrixBLL(
            IMapper mapper,
            IFinder<Priority> priorityFinder,
            IFinder<Influence> influenceFinder,
            IRepository<Concordance> concordanceRepository,
            IUnitOfWork unitOfWork,
            IFinder<Concordance> concordanceFinder)
        {
            _mapper = mapper;
            _priorityFinder = priorityFinder;
            _influenceFinder = influenceFinder;
            _concordanceRepository = concordanceRepository;
            _unitOfWork = unitOfWork;
            _concordanceFinder = concordanceFinder;
        }

        public async Task<ConcordanceDetails[]> GetTableAsync(CancellationToken cancellationToken = default)
        {
            var data = await _concordanceRepository.With(x => x.Priority).ToArrayAsync(cancellationToken);

            return _mapper.Map<ConcordanceDetails[]>(data);
        }

        public async Task<bool> RemoveCellAsync(Guid urgencyId, Guid influencyId, CancellationToken cancellationToken = default)
        {

            var existingCell = await _concordanceRepository.FirstOrDefaultAsync(x => x.InfluenceId == influencyId && x.UrgencyId == urgencyId, cancellationToken);

            if (existingCell == null)
            {
                throw new ObjectNotFoundException<Guid>(Guid.Empty,
                    $"PriorityMatrixCell not found, UrgencyID{urgencyId} | influencyID{influencyId}");
            }

            _concordanceRepository.Delete(existingCell);

            await _unitOfWork.SaveAsync(cancellationToken);

            return true;
        }

        //TODO разделить добавление и обновление 
        public async Task<bool> SaveCellAsync(ConcordanceDetails cell, CancellationToken cancellationToken = default)
        {
            var priority = await _priorityFinder.FindAsync(cell.PriorityId, cancellationToken);
            var influence = await _influenceFinder.FindAsync(cell.InfluenceId, cancellationToken);

            if (priority == null || influence == null)
            {
                throw new ObjectNotFoundException<Guid>(Guid.Empty,
                    $"priority or Influency not found, PriorityID{cell.Priority} | influencyID{cell.InfluenceId}");

            }
            var existingCell = await _concordanceFinder.FindAsync(new object[] { cell.UrgencyId, cell.InfluenceId }, cancellationToken);

            if (existingCell == null)
            {
                var saveModel = _mapper.Map<Concordance>(cell);
                _concordanceRepository.Insert(saveModel);
            }
            else
            {
                existingCell.PriorityID = cell.PriorityId;
            }

            await _unitOfWork.SaveAsync(cancellationToken);

            return true;
        }
    }
}
