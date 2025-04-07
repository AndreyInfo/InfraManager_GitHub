using AutoMapper;
using InfraManager.DAL;

namespace IM.Core.Import.BLL.Import.Importer.UploadData;

//TODO:убрать и заменить на настоящий
public class InsertOrUpdate<TEntity>:IBulkInsertOrUpdate<TEntity>
where TEntity:class,IImportEntity
{
    private readonly IRepository<TEntity> _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public InsertOrUpdate(IRepository<TEntity> repository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(IEnumerable<TEntity> data, CancellationToken token)
    {
        foreach (var element in data)
        {
            if (await _repository.AnyAsync(x => x.ExternalID == element.ExternalID, token))
            {
                var oldValue = await _repository.FirstOrDefaultAsync(x => x.ExternalID == element.ExternalID, token);
                _mapper.Map(element, oldValue);
            }
            else
            {
                var newElement = _mapper.Map<TEntity>(element);
                _repository.Insert(newElement);
            }
        }

        await _unitOfWork.SaveAsync(token);
    }
}