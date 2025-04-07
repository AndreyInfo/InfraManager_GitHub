using AutoMapper;
using InfraManager.BLL.Asset.History;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset.LifeCycleCommands;
internal class LifeCycleCommandFacade : ILifeCycleCommandFacade, ISelfRegisteredService<ILifeCycleCommandFacade>
{
    private readonly IMapper _mapper;
    private readonly IServiceMapper<LifeCycleOperationCommandType, LifeCycleCommand> _commandBLLServiceMapper;
    private readonly IServiceMapper<LifeCycleOperationCommandType, LifeCycleCommandWithAlert> _commandBLLWithAlertServiceMapper;
    private readonly IRepository<LifeCycleStateOperation> _operationRepository;

    public LifeCycleCommandFacade(IMapper mapper
        , IServiceMapper<LifeCycleOperationCommandType, LifeCycleCommand> serviceMapper
        , IServiceMapper<LifeCycleOperationCommandType, LifeCycleCommandWithAlert> commandBLLWithAlertServiceMapper
        , IRepository<LifeCycleStateOperation> operationRepository)
    {
        _mapper = mapper;
        _commandBLLServiceMapper = serviceMapper;
        _operationRepository = operationRepository;
        _commandBLLWithAlertServiceMapper = commandBLLWithAlertServiceMapper;
    }

    public async Task ExecuteAsync(Guid id, Guid operationID, LifeCycleCommandBaseData data, CancellationToken cancellationToken)
    {
        var commandType = (await _operationRepository.FirstOrDefaultAsync(x => x.ID == operationID, cancellationToken)).CommandType;

        var command = _commandBLLServiceMapper.Map(commandType);
        var commandResult = await command.ExecuteAsync(id, operationID, data, cancellationToken);

        var assetHistoryData = _mapper.Map<AssetHistoryBaseData>(data);
        _mapper.Map(commandResult, assetHistoryData);
        assetHistoryData.OperationType = commandType;

        await command.SaveHistoryAsync(id, commandType, assetHistoryData, cancellationToken);
    }

    public async Task<LifeCycleCommandAlertDetails> ExecuteWithAlertAsync(Guid id, Guid operationID, LifeCycleCommandBaseData data, CancellationToken cancellationToken)
    {
        var bll = await GetBLLWithAlertAsync(operationID, cancellationToken);
        var command = await bll.ExecuteWithAlertAsync(id, operationID, data, cancellationToken);
        return _mapper.Map<LifeCycleCommandAlertDetails>(command);
    }

    private async Task<LifeCycleCommandWithAlert> GetBLLWithAlertAsync(Guid operationID, CancellationToken cancellationToken)
    {
        var command = (await _operationRepository.FirstOrDefaultAsync(x => x.ID == operationID, cancellationToken)).CommandType;
        return _commandBLLWithAlertServiceMapper.Map(command);
    }

}
