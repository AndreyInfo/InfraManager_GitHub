using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.BLL.Owners;

[Obsolete]
public class OwnerBLL : IOwnerBLL, ISelfRegisteredService<IOwnerBLL>
{
    private readonly IMapper _mapper;
    private readonly IRepository<Owner> _repository;
    private readonly IUnitOfWork _saveChanges;

    public OwnerBLL(IMapper mapper, 
        IRepository<Owner> repository,
        IUnitOfWork saveChanges)
    {
        _mapper = mapper;
        _repository = repository;
        _saveChanges = saveChanges;
    }
    
    public async Task<OwnerDetails> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var owner = await _repository.FirstOrDefaultAsync(x=>x.IMObjID == id, cancellationToken) 
                    ?? throw new ObjectNotFoundException("Подразделение не найдено");

        return _mapper.Map<OwnerDetails>(owner);
    }

    public async Task<OwnerDetails> GetFirstAsync(CancellationToken cancellationToken) =>
        _mapper.Map<OwnerDetails>(await _repository.FirstOrDefaultAsync(cancellationToken)); 
    
    
    
    public async Task UpdateAsync(OwnerDetails ownerDetails, CancellationToken cancellationToken)
    {
        var owner = await _repository.FirstOrDefaultAsync(x=> x.IMObjID == ownerDetails.IMObjID, cancellationToken) ?? throw new ObjectNotFoundException("Подразделение не найдено");;
        
        _mapper.Map(ownerDetails, owner);
        
        await _saveChanges.SaveAsync(cancellationToken);
    }   
}