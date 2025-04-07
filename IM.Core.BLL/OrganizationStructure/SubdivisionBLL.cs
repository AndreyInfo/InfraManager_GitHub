using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.OrganizationStructure
{
    public class SubdivisionBLL : ISubdivisionBLL, ISelfRegisteredService<ISubdivisionBLL>
    {
        private readonly IRepository<Subdivision> _subdivisionsRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _saveChanges;
        private readonly IGetEntityArrayBLL<Guid, Subdivision, SubdivisionDetails, SubdivisionListFilter> _arrayBll;
        private readonly IGetEntityBLL<Guid, Subdivision, SubdivisionDetails> _detailsBll;
        private readonly ILogger<SubdivisionBLL> _logger;
        private readonly ICurrentUser _currentUser;
        private readonly IValidatePermissions<Subdivision> _validatePermissions;
        private readonly IGuidePaggingFacade<Subdivision, SubdivisionsList> _guidePaggingFacade;

        public SubdivisionBLL(
            IRepository<Subdivision> subdivisionsRepository,
            IMapper mapper,
            IUnitOfWork saveChanges,
            IGetEntityArrayBLL<Guid, Subdivision, SubdivisionDetails, SubdivisionListFilter> arrayBll,
            IGetEntityBLL<Guid, Subdivision, SubdivisionDetails> detailsBll,
            ILogger<SubdivisionBLL> logger,
            IGuidePaggingFacade<Subdivision, SubdivisionsList> guidePaggingFacade,
            ICurrentUser currentUser,
            IValidatePermissions<Subdivision> validatePermissions)
        {
            _subdivisionsRepository = subdivisionsRepository;
            _mapper = mapper;
            _saveChanges = saveChanges;
            _arrayBll = arrayBll;
            _detailsBll = detailsBll;
            _logger = logger;
            _currentUser = currentUser;
            _validatePermissions = validatePermissions;
            _guidePaggingFacade = guidePaggingFacade;
        }

        public Task<SubdivisionDetails> GetDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return _detailsBll.DetailsAsync(id, cancellationToken);
        }

        public Task<SubdivisionDetails[]> GetDetailsArrayAsync(SubdivisionListFilter filterBy, CancellationToken cancellationToken = default)
        {
            return _arrayBll.ArrayAsync(filterBy, cancellationToken);
        }

        public Task<SubdivisionDetails[]> GetDetailsPageAsync(SubdivisionListFilter filterBy, ClientPageFilter<Subdivision> pageFilter, CancellationToken cancellationToken = default)
        {
            return _arrayBll.PageAsync(filterBy, pageFilter, cancellationToken);
        }

        public async Task<SubdivisionDetails[]> GetTableAsync(BaseFilter filter, CancellationToken cancellationToken)
        {
            var entities = await _guidePaggingFacade.GetPaggingAsync(filter,
                _subdivisionsRepository.Query(),
                c => c.Name.ToLower().Contains(filter.SearchString.ToLower()),
                cancellationToken);

            return _mapper.Map<SubdivisionDetails[]>(entities);
        }

        public async Task DeleteByIDAsync(Guid subdivisionID, CancellationToken cancellationToken)
        {

            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Delete, cancellationToken);
            var foundEntity = await _subdivisionsRepository.FirstOrDefaultAsync(x => x.ID == subdivisionID, cancellationToken)
                                ?? throw new ObjectNotFoundException<Guid>(subdivisionID, ObjectClass.Division);

            _subdivisionsRepository.Delete(foundEntity);

            await _saveChanges.SaveAsync(cancellationToken);
        }
        public async Task UpdateAsync(Guid id, SubdivisionData subdivision, CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Update, cancellationToken);
            
            var foundEntity = await _subdivisionsRepository.FirstOrDefaultAsync(x=>x.ID == id, cancellationToken)
                                        ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.Division);

            _mapper.Map(subdivision, foundEntity);

            await _saveChanges.SaveAsync(cancellationToken);
            
        }
        public async Task<Guid> AddAsync(SubdivisionData subdivision, CancellationToken cancellationToken)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Insert, cancellationToken);

            var entity = _mapper.Map<Subdivision>(subdivision);
            _subdivisionsRepository.Insert(entity);
            await _saveChanges.SaveAsync(cancellationToken);

            return entity.ID;
        }

        public async Task<Subdivision[]> GetPathToSubdivisionAsync(Guid subdivisionID, CancellationToken cancellationToken)
        {
            List<Subdivision> subdivisions = new List<Subdivision>();

            var childrenSubdivision = await _subdivisionsRepository.FirstOrDefaultAsync(x => x.ID == subdivisionID, cancellationToken);

            subdivisions.Add(childrenSubdivision);
            while (childrenSubdivision.SubdivisionID.HasValue)
            {
                childrenSubdivision = await _subdivisionsRepository.FirstOrDefaultAsync(x => x.ID == childrenSubdivision.SubdivisionID.Value, cancellationToken);
                subdivisions.Add(childrenSubdivision);
            }

            return subdivisions.ToArray();
        }


        public async Task<SubdivisionDetails[]> GetAllSubSubdivisionsAsync(Guid parentID, CancellationToken cancellationToken)
        {
            var root = await _subdivisionsRepository.WithMany(c => c.ChildSubdivisions)
                        .FirstOrDefaultAsync(x => x.ID == parentID, cancellationToken)
                        ?? throw new ObjectNotFoundException<Guid>(parentID, ObjectClass.Division);

            var result = new Queue<Subdivision>();
            var nodes = new Queue<Subdivision>();
            do
            {
                result.Enqueue(root);

                foreach (var item in root.ChildSubdivisions)
                    nodes.Enqueue(item);

            } while (nodes.TryDequeue(out root));

            return _mapper.Map<SubdivisionDetails[]>(result);
        }
    }
}
