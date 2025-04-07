using AutoMapper;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.Problems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.DAL;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.ServiceDesk.Problems
{
    // TODO убрать кривую логику в методах(переделать на LookupBLL)
    internal class ProblemTypesBLL :
        StandardBLL<Guid, ProblemType, ProblemTypeData, ProblemTypeDetails, ProblemTypeFilter>,
        IProblemTypesBLL,
        ISelfRegisteredService<IProblemTypesBLL>
    {
        private readonly IMapper _mapper;
        private readonly IReadonlyRepository<ProblemType> _readonlyRepository;
        private readonly IRepository<ProblemType> _repository;
        private readonly IProblemTypesDataProvider _problemTypesDataProvider;

        public ProblemTypesBLL(
            IMapper mapper,
            IReadonlyRepository<ProblemType> readonlyRepository,
            IRepository<ProblemType> repository,
            IProblemTypesDataProvider problemTypesDataProvider,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            ILogger<ProblemTypesBLL> logger,
            IBuildObject<ProblemTypeDetails, ProblemType> detailsBuilder,
            IInsertEntityBLL<ProblemType, ProblemTypeData> insertEntityBLL,
            IModifyEntityBLL<Guid, ProblemType, ProblemTypeData, ProblemTypeDetails> modifyEntityBLL,
            IRemoveEntityBLL<Guid, ProblemType> removeEntityBLL,
            IGetEntityBLL<Guid, ProblemType, ProblemTypeDetails> detailsBLL,
            IGetEntityArrayBLL<Guid, ProblemType, ProblemTypeDetails, ProblemTypeFilter> detailsArrayBLL
        )
            : base(
                repository,
                logger,
                unitOfWork,
                currentUser,
                detailsBuilder,
                insertEntityBLL,
                modifyEntityBLL,
                removeEntityBLL,
                detailsBLL,
                detailsArrayBLL)
        {
            _readonlyRepository = readonlyRepository;
            _repository = repository;
            _mapper = mapper;
            _problemTypesDataProvider = problemTypesDataProvider;
        }

        public async Task<ProblemTypeDetails> GetByIdAsync(Guid id,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                return _mapper.Map<ProblemTypeDetails>(await _problemTypesDataProvider.GetRootProblemTypeAsync());
            }

            return await DetailsAsync(id, cancellationToken);
        }

        public async Task<byte[]> GetImageAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var problemTypeDetails = await DetailsAsync(id, cancellationToken);

            if (problemTypeDetails.Image.Any())
            {
                return problemTypeDetails.Image;
            }

            if (problemTypeDetails.ParentID == null) return Array.Empty<byte>();

            var parentType = await DetailsAsync((Guid) problemTypeDetails.ParentID, cancellationToken);

            return parentType.Image;
        }

        public async Task<ProblemTypeDetails[]> GetPathByIdAsync(Guid id,
            CancellationToken cancellationToken = default)
        {
            await ThrowIfNoExists(id, cancellationToken);

            var data = await _problemTypesDataProvider.GetPathInTreeByIdAsync(id);
            
            var result = _mapper.Map<ProblemTypeDetails[]>(data);

            return result;
        }

        public async Task<ProblemTypeDetails[]> GetTreeByIdAsync(Guid id, List<Guid> filterId,
            CancellationToken cancellationToken = default)
        {
            await ThrowIfNoExists(id, cancellationToken);

            //TODO  убрать кривую логику с parentId
            var result = new List<ProblemTypeDetails>();

            if (id == Guid.Empty)
            {
                result.Add(_mapper.Map<ProblemTypeDetails>(await _problemTypesDataProvider.GetRootProblemTypeAsync()));
            }
            else
            {
                var models =
                    _mapper.Map<ProblemTypeDetails[]>(
                        await _problemTypesDataProvider.GetChildrenByIdAsync(id, filterId));
                
                result.AddRange(models);
            }
            
            return result.Distinct().ToArray();
        }

        private async Task ThrowIfNoExists(Guid id, CancellationToken cancellationToken = default)
        {
            var isExistsProblemType = await _readonlyRepository
                .AnyAsync(x => x.ID == id, cancellationToken);

            if (!isExistsProblemType && id != Guid.Empty)
                throw new ObjectNotFoundException<Guid>(id, "Problem Not Found");
        }
    }
}