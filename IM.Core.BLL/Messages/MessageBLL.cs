using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.Message;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using Inframanager.BLL;

namespace InfraManager.BLL.Messages;

//TODO узнать права на Message
internal class MessageBLL : IMessageBLL, ISelfRegisteredService<IMessageBLL>
{
    private readonly IReadonlyRepository<Message> _repositoryMessages;
    private readonly IMapper _mapper;
    private readonly IGuidePaggingFacade<Message, MessageColumns> _guidePaggingFacade;
    private readonly IModifyEntityBLL<Guid, Message, MessageData, MessageDetails> _modifyEntity;
    private readonly IUnitOfWork _saveChangesCommand;

    public MessageBLL(
        IReadonlyRepository<Message> repositoryMessages,
        IMapper mapper,
        IGuidePaggingFacade<Message, MessageColumns> guidePaggingFacade,
        IModifyEntityBLL<Guid, Message, MessageData, MessageDetails> modifyEntity,
        IUnitOfWork saveChangesCommand)
    {
        _repositoryMessages = repositoryMessages;
        _mapper = mapper;
        _guidePaggingFacade = guidePaggingFacade;
        _modifyEntity = modifyEntity;
        _saveChangesCommand = saveChangesCommand;
    }

    public async Task<MessageDetails> GetByIDAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _repositoryMessages.FirstOrDefaultAsync(c => c.IMObjID == id, cancellationToken);
        if (entity is null)
            throw new ObjectNotFoundException<Guid>(id, ObjectClass.Message);

        return _mapper.Map<MessageDetails>(entity);
    }

    public async Task<MessageDetails[]> GetListByFilterAsync(BaseFilter filter, CancellationToken cancellationToken)
    {
        var messages = await _guidePaggingFacade.GetPaggingAsync(filter,
            _repositoryMessages.Query(),
            x => x.EntityStateName.ToLower().Contains(filter.SearchString.ToLower()),
            cancellationToken);


        return _mapper.Map<MessageDetails[]>(messages);
    }

    public async Task<MessageDetails> UpdateAsync(Guid id, MessageData model, CancellationToken cancellationToken)
    {
        var message = await _modifyEntity.ModifyAsync(id, model, cancellationToken);

        await _saveChangesCommand.SaveAsync(cancellationToken);

        return _mapper.Map<MessageDetails>(message);
    }
}
