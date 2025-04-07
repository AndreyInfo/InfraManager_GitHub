using AutoMapper;
using IM.Core.Import.BLL.Interface;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;
using IM.Core.Import.BLL.Interface.Import.Log;

namespace IM.Core.Import.BLL
{
    internal class PositionBLL : IPositionBLL, ISelfRegisteredService<IPositionBLL>
    {
        private readonly IReadonlyRepository<JobTitle> _positionReadonlyRepository;
        private readonly IUnitOfWork _saveChanges;

        private readonly IRepository<JobTitle> _positionRepository;
        private readonly ILocalLogger<PositionBLL> _logger;

        private readonly IMapper _mapper;
        public PositionBLL(IReadonlyRepository<JobTitle> positionReadonlyRepository,
            IRepository<JobTitle> positionRepository,
            IUnitOfWork saveChanges,
            ILocalLogger<PositionBLL> logger,
            IMapper mapper)
        {
            _positionReadonlyRepository = positionReadonlyRepository;
            _positionRepository = positionRepository;
            _saveChanges = saveChanges;
            _logger = logger;   
            _mapper = mapper;
        }

        public async Task<JobTitle> CreateAsync(PositionModel positionModel, CancellationToken cancellationToken)
        {
            try
            {
                var JobTitle = _mapper.Map<JobTitle>(positionModel);

                _positionRepository.Insert(JobTitle);

                await _saveChanges.SaveAsync(cancellationToken);
                _logger.Information($"Добавлена должность с именем: {JobTitle.Name}");

                return JobTitle;
            }
            catch (Exception e)
            {
                _logger.Error("Ошибка создания должности с именем", positionModel.Name, e);
                throw;
            }

        }
        public async Task<JobTitle> GetByNameAsync(PositionModel positionModel, CancellationToken cancellationToken)
        {
            return await _positionReadonlyRepository.FirstOrDefaultAsync(x => x.Name == positionModel.Name, cancellationToken);
        }
    }
}
