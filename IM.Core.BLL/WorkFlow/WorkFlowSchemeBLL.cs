using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.Settings;
using InfraManager.BLL.Workflow;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.WorkFlow;

namespace InfraManager.BLL.WorkFlow;

internal class WorkFlowSchemeBLL : IWorkFlowShemeBLL, ISelfRegisteredService<IWorkFlowShemeBLL>
{
    private readonly IMapper _mapper;
    private readonly IClassIM _classIM;
    
    //TODO отказаться от DataProvider перейти на IRepository<WorkFlowScheme>
    //TODO удалить данный класс
    private readonly IWorkFlowShemeDataProvider _workFlowSchemeDataProvider;

    public WorkFlowSchemeBLL(IMapper mapper, 
        IClassIM classIm, 
        IWorkFlowShemeDataProvider workFlowSchemeDataProvider)
    {
        _mapper = mapper;
        _classIM = classIm;
        _workFlowSchemeDataProvider = workFlowSchemeDataProvider;
    }

    public async Task<WorkflowSchemeDetailsModel> FindByIdentifierAsync(string identifier, CancellationToken cancellationToken = default)
    {
        if (! await _workFlowSchemeDataProvider.IsExistByIdentifierAsync(identifier)) throw new ObjectNotFoundException($"WorkflowScheme (Identifier = {identifier})");
        
        return _mapper.Map<WorkflowSchemeDetailsModel>(await _workFlowSchemeDataProvider.GetActualVersionByIdentifierAsync(identifier, cancellationToken));
    }

    public WorkflowSchemeTypeModel[] GetAvailableTypes() =>
        AvailableTypes().Select(el => new WorkflowSchemeTypeModel {ID = (int) el, Name = _classIM.GetClassName(el)}).ToArray();
    

    private IEnumerable<ObjectClass> AvailableTypes() => new List<ObjectClass>()
    {
        ObjectClass.WorkOrder,
        ObjectClass.Call,
        ObjectClass.Problem,
        ObjectClass.ChangeRequest,
        ObjectClass.MessageByMonitoring,
        ObjectClass.MessageByInquiry,
        ObjectClass.MessageByInquiryTask,
        ObjectClass.MessageByEmail,
        ObjectClass.MessageByOrganizationStructureImport,
        ObjectClass.MessageByTaskForUsers,
        ObjectClass.MassIncident
    };
}