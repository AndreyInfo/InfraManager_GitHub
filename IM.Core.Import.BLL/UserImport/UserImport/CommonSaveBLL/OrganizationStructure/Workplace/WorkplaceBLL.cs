using AutoMapper;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import.Log;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Location;
using Microsoft.Extensions.Logging;

namespace IM.Core.Import.BLL
{
    internal class WorkplaceBLL : IWorkplaceBLL, ISelfRegisteredService<IWorkplaceBLL>
    {
        private readonly IRepository<Workplace> _repository;
        private readonly IUnitOfWork _saveChanges;
        private readonly IMapper _mapper;

        private readonly ILocalLogger<WorkplaceBLL> _logger;

        public WorkplaceBLL(IRepository<Workplace> repository,
           IUnitOfWork saveChanges,
           ILocalLogger<WorkplaceBLL> logger,
           IMapper mapper)
        {
            _repository = repository;
            _saveChanges = saveChanges;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Workplace> CreateAsync(WorkplaceModel workplaceModel, CancellationToken cancellationToken)
        {
            try
            {
                var workplace = _mapper.Map<Workplace>(workplaceModel);

                _repository.Insert(workplace);

                await _saveChanges.SaveAsync(cancellationToken);
                _logger.Information($"Добавлено рабочее место: {workplace.Name}");

                return workplace;
            }
            catch (Exception e)
            {
                _logger.Error("Ошибка добавления рабочего места с именем", workplaceModel.Name, e);
                throw;
            }
        }

        public async Task<Workplace> GetOrCreateByNameAsync(WorkplaceModel workplaceModel, CancellationToken cancellationToken)
        {
            var workplace = await GetWorkplaceByName(workplaceModel.Name, cancellationToken);


            return (workplace == null)
                ?await CreateAsync(workplaceModel, cancellationToken)
                : workplace;
        }

        public async Task<Workplace> GetOrCreateByModelAsync(WorkplaceModel workplaceModel, CancellationToken token)
        {
            var workplace = await GetWorkplaceByModelAsync(workplaceModel, token) 
                            ?? await CreateAsync(workplaceModel, token);

            return workplace;
        }

        public async Task<Workplace?> GetWorkplaceByModelAsync(WorkplaceModel model, CancellationToken token)
        {
            var query = _repository.Query()
                .AsQueryable();
            
            if (model.OrganizationID.HasValue)
                query = query.Where(x =>
                  (!x.RoomID.HasValue || !x.Room.FloorID.HasValue || !x.Room.Floor.BuildingID.HasValue ||
                   (x.Room.Floor.Building.OrganizationID == model.OrganizationID)));
            
            if (!string.IsNullOrEmpty(model.Name))
            {
                query = query.Where(x => x.Name == model.Name);
            }

            if (!string.IsNullOrEmpty(model.Note))
            {
                query = query.Where(x => x.Note == model.Note);
            }

            if (!string.IsNullOrEmpty(model.ExternalId))
            {
                query = query.Where(x => x.ExternalID == model.ExternalId);
            }

            if (model.ComplementaryId.HasValue)
            {
                query = query.Where(x => x.ComplementaryID == model.ComplementaryId.Value);
            }

            if (model.PeripheralDatabaseId.HasValue)
            {
                query = query.Where(x => x.PeripheralDatabaseID == model.PeripheralDatabaseId.Value);
            }

            if (model.RoomID.HasValue)
            {
                query = query.Where(x => x.RoomID == model.RoomID.Value);
            }

            return  query.FirstOrDefault();
        }


        public async Task<Workplace> GetWorkplaceByName(string name, CancellationToken cancellationToken)
        {
            return await _repository.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
        }

        public async Task<Workplace> GetWorkplaceByExternalIDAsync(string id, CancellationToken cancellationToken)
        {
            return await _repository.FirstOrDefaultAsync(x => x.ExternalID == id, cancellationToken);
        }

        public async Task<Workplace> GetWorkplaceByExternalIDOrNameAsync(string nameOrId, CancellationToken cancellationToken)
        {
            return await _repository.FirstOrDefaultAsync(x => x.ExternalID == nameOrId || x.Name == nameOrId, cancellationToken);
        }

        public async Task<Workplace> GetAsync(Guid? imObjID, CancellationToken cancellationToken)
        {
            return await _repository.FirstOrDefaultAsync(x => x.IMObjID == imObjID, cancellationToken);
        }
    }
}
