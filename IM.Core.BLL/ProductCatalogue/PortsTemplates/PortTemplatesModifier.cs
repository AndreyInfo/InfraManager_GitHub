using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.Localization;
using InfraManager.BLL.ProductCatalogue.PortTemplates;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.ResourcesArea;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace InfraManager.BLL.ProductCatalogue.PortsTemplates;

internal sealed class PortTemplatesModifier :
    IModifyObject<PortTemplate, PortTemplatesData>,
        ISelfRegisteredService<IModifyObject<PortTemplate, PortTemplatesData>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<PortTemplate> _repository;
    private readonly ILocalizeText _localizeText;

    public PortTemplatesModifier(IMapper mapper
        , IUnitOfWork unitOfWork
        , IRepository<PortTemplate> repository
        , ILocalizeText localizeText
        )
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _repository = repository;
        _localizeText = localizeText;
    }

    public async Task ModifyAsync(PortTemplate entity, PortTemplatesData data, CancellationToken cancellationToken = default)
    {
        if (entity.PortNumber != data.PortNumber)
        {
            if (_repository.Any(x => x.ObjectID == data.ObjectID && x.PortNumber == data.PortNumber))
                throw new InvalidObjectException(string.Format(_localizeText.Localize(nameof(Resources.ValidationErrorType_AlreadyExist)), data.ObjectID, data.PortNumber));

            using (var transaction = TransactionScopeCreator.Create(IsolationLevel.ReadCommitted, TransactionScopeOption.Required))
            {
                _repository.Delete(entity);
                await _unitOfWork.SaveAsync(cancellationToken);

                _mapper.Map(data, entity);
                _repository.Insert(entity);

                transaction.Complete();
            }
        }
        _mapper.Map(data, entity);
    }

    public void SetModifiedDate(PortTemplate entity)
    {
    }
}
