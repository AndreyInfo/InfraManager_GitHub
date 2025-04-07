using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.ServiceCatalogue;

public class ServiceLevelAgreementReferenceBLL : IServiceLevelAgreementReference, ISelfRegisteredService<IServiceLevelAgreementReference>
{
    private readonly IRepository<SLAReference> _slaReferenceRepository;
    private readonly IReadonlyRepository<SLAReference> _slaReferenceReadonlyRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _saveChangesCommand;
    private readonly ILogger<ServiceLevelAgreementReferenceBLL> _logger;
    private readonly ICurrentUser _currentUser;

    public ServiceLevelAgreementReferenceBLL(IRepository<SLAReference> slaReferenceRepository,
        IReadonlyRepository<SLAReference> slaReferenceReadonlyRepository,
        IMapper mapper,
        IUnitOfWork saveChangesCommand,
        ILogger<ServiceLevelAgreementReferenceBLL> logger,
        ICurrentUser currentUser)
    {
        _slaReferenceRepository = slaReferenceRepository;
        _slaReferenceReadonlyRepository = slaReferenceReadonlyRepository;
        _saveChangesCommand = saveChangesCommand;
        _logger = logger;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task InsertAsync(SLAReferenceDetails slaReferenceDetails,
        CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<SLAReference>(slaReferenceDetails);

        _slaReferenceRepository.Insert(entity);

        await _saveChangesCommand.SaveAsync(cancellationToken);
        _logger.LogTrace(
            $"User ID = {_currentUser.UserId} inserted new SLAReference with sla id = {entity.SLAID} and object id =  {entity.ObjectID}");
    }

    public async Task UpdateAsync(Guid slaID, Guid objectID, SLAReferenceData slaReferenceDetails,
        CancellationToken cancellationToken = default)
    {
        var slaReference = await _slaReferenceReadonlyRepository.FirstOrDefaultAsync(
            x => x.SLAID == slaID && x.ObjectID == objectID,
            cancellationToken) ?? throw new ObjectNotFoundException("sla references not found");

        _mapper.Map(slaReferenceDetails, slaReference);

        await _saveChangesCommand.SaveAsync(cancellationToken);
        _logger.LogTrace(
            $"User ID = {_currentUser.UserId} Updated SLAReference with sla id = {slaID} and object id =  {objectID}");
    }

    public async Task DeleteAsync(Guid slaID, Guid objectID, CancellationToken cancellationToken = default)
    {
        var slaReference = await _slaReferenceReadonlyRepository.FirstOrDefaultAsync(x => x.SLAID == slaID && x.ObjectID == objectID, cancellationToken)
                           ?? throw new ObjectNotFoundException($"SLAReference not found; SLAID = {slaID}; objectID = {objectID}");

        _slaReferenceRepository.Delete(slaReference);

        await _saveChangesCommand.SaveAsync(cancellationToken);
        _logger.LogTrace(
            $"User ID = {_currentUser.UserId} Deleted SLAReference with sla id = {slaID} and object id =  {objectID}");
    }

    public async Task<SLAReferenceDetails> GetAsync(Guid slaID, Guid objectID, CancellationToken cancellationToken = default)
    {
        var entities = await _slaReferenceReadonlyRepository.FirstOrDefaultAsync(x => x.SLAID == slaID && x.ObjectID == objectID, cancellationToken);


        _logger.LogTrace(
            $"User ID = {_currentUser.UserId} Got SLAReference with sla id = {slaID} and object id =  {objectID}");
        return _mapper.Map<SLAReferenceDetails>(entities);
    }

    public async Task<SLAReferenceDetails[]> GetListAsync(Guid slaID, ObjectClass classID, CancellationToken cancellationToken = default)
    {
        var entities = await _slaReferenceReadonlyRepository.ToArrayAsync(x => x.SLAID == slaID && x.ClassID == classID, cancellationToken);

        _logger.LogTrace(
            $"User ID = {_currentUser.UserId} Got list of SLAReference with sla id = {slaID}");
        return _mapper.Map<SLAReferenceDetails[]>(entities);
    }
}