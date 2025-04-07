using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.Localization;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.ResourcesArea;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace InfraManager.BLL.ProductCatalogue.Slots;
internal sealed class SlotBaseModifier<TEntity, TData> : IModifyObject<TEntity, TData>
    where TEntity : SlotBase
    where TData : SlotBaseData
{
    private readonly IMapper _mapper;
    private readonly ILocalizeText _localizeText;
    private readonly IRepository<TEntity> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public SlotBaseModifier(IMapper mapper
        , IUnitOfWork unitOfWork
        , ILocalizeText localizeText
        , IRepository<TEntity> repository)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _repository = repository;
        _localizeText = localizeText;
    }

    public async Task ModifyAsync(TEntity entity, TData data, CancellationToken cancellationToken = default)
    {

        if (entity.Number != data.Number)
        {
            if (_repository.Any(x => x.ObjectID == data.ObjectID && x.Number == data.Number))
                throw new InvalidObjectException(string.Format(_localizeText.Localize(nameof(Resources.ValidationErrorType_AlreadyExist)), data.ObjectID, data.Number));

            using (var transaction = TransactionScopeCreator.Create(IsolationLevel.ReadCommitted, TransactionScopeOption.Required))
            {
                _repository.Delete(entity);
                await _unitOfWork.SaveAsync(cancellationToken);

                _mapper.Map(data, entity);
                _repository.Insert(entity);

                transaction.Complete();
            }
        }
        else
        {
            _mapper.Map(data, entity);
        }
    }

    public void SetModifiedDate(TEntity entity)
    {
    }
}
