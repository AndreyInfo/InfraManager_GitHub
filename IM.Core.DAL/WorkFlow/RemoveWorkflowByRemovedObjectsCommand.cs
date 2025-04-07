using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using InfraManager.DAL.Events;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.DAL.WF;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.WorkFlow;

internal class RemoveWorkflowByRemovedObjectsCommand : IRemoveWorkflowByRemovedObjectsCommand,
    ISelfRegisteredService<IRemoveWorkflowByRemovedObjectsCommand>
{
    private readonly DbContext _context;
    public RemoveWorkflowByRemovedObjectsCommand(CrossPlatformDbContext context)
    {
        _context = context;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var deleteFromExternalEventReference =
            (from externalEventReference in _context.Set<ExternalEventReference>()
                join callJoin in _context.Set<Call>().AsNoTracking()
                    on externalEventReference.WorkflowId equals callJoin.IMObjID into callLeftJoinResult
                from callJoin in callLeftJoinResult.DefaultIfEmpty()
                join workOrderJoin in _context.Set<WorkOrder>().AsNoTracking()
                    on externalEventReference.WorkflowId equals workOrderJoin.IMObjID into workOrderLeftJoinResult
                from workOrderJoin in workOrderLeftJoinResult.DefaultIfEmpty()
                join problemJoin in _context.Set<Problem>().AsNoTracking()
                    on externalEventReference.WorkflowId equals problemJoin.IMObjID into problemLeftJoinResult
                from problemJoin in problemLeftJoinResult.DefaultIfEmpty()
                join changeRequestJoin in _context.Set<ChangeRequest>().AsNoTracking()
                    on externalEventReference.WorkflowId equals changeRequestJoin.IMObjID into
                    changeRequestLeftJoinResult
                from changeRequestJoin in changeRequestLeftJoinResult.DefaultIfEmpty()
                join messageQuery in _context.Set<Message.Message>().AsNoTracking()
                    on externalEventReference.WorkflowId equals messageQuery.IMObjID into leftJoinResult
                from messageQuery in leftJoinResult.DefaultIfEmpty()
                where messageQuery.IMObjID == null && callJoin.IMObjID == null &&
                      workOrderJoin.IMObjID == null && problemJoin.IMObjID == null && changeRequestJoin.IMObjID == null
                select externalEventReference).ToArray();

        var externalEventReferenceContext = _context.Set<ExternalEventReference>();
        deleteFromExternalEventReference.ForEach(x => externalEventReferenceContext.Remove(x));
        
        var deleteWorkflowEvent = (from workflowEvent in _context.Set<WorkflowEvent>()
            join callJoin in _context.Set<Call>().AsNoTracking()
                on workflowEvent.WorkflowID equals callJoin.IMObjID into callLeftJoinResult
            from callJoin in callLeftJoinResult.DefaultIfEmpty()
            join workOrderJoin in _context.Set<WorkOrder>().AsNoTracking()
                on workflowEvent.WorkflowID equals workOrderJoin.IMObjID into workOrderLeftJoinResult
            from workOrderJoin in workOrderLeftJoinResult.DefaultIfEmpty()
            join problemJoin in _context.Set<Problem>().AsNoTracking()
                on workflowEvent.WorkflowID equals problemJoin.IMObjID into problemLeftJoinResult
            from problemJoin in problemLeftJoinResult.DefaultIfEmpty()
            join changeRequestJoin in _context.Set<ChangeRequest>().AsNoTracking()
                on workflowEvent.WorkflowID equals changeRequestJoin.IMObjID into changeRequestLeftJoinResult
            from changeRequestJoin in changeRequestLeftJoinResult.DefaultIfEmpty()
            join messageQuery in _context.Set<Message.Message>().AsNoTracking()
                on workflowEvent.WorkflowID equals messageQuery.IMObjID into leftJoinResult
            from messageQuery in leftJoinResult.DefaultIfEmpty()
            where messageQuery.IMObjID == null && callJoin.IMObjID == null &&
                  workOrderJoin.IMObjID == null && problemJoin.IMObjID == null && changeRequestJoin.IMObjID == null
            select workflowEvent).ToArray();
        
        var workflowEventContext = _context.Set<WorkflowEvent>();
        deleteWorkflowEvent.ForEach(x => workflowEventContext.Remove(x));

        var workflowTrachingDelete =
            (from workflowTracking in _context.Set<WorkflowTracking>().Include(x => x.StateTrackings)
                join callJoin in _context.Set<Call>().AsNoTracking()
                    on workflowTracking.ID equals callJoin.IMObjID into callLeftJoinResult
                from callJoin in callLeftJoinResult.DefaultIfEmpty()
                join workOrderJoin in _context.Set<WorkOrder>().AsNoTracking()
                    on workflowTracking.ID equals workOrderJoin.IMObjID into workOrderLeftJoinResult
                from workOrderJoin in workOrderLeftJoinResult.DefaultIfEmpty()
                join problemJoin in _context.Set<Problem>().AsNoTracking()
                    on workflowTracking.ID equals problemJoin.IMObjID into problemLeftJoinResult
                from problemJoin in problemLeftJoinResult.DefaultIfEmpty()
                join changeRequestJoin in _context.Set<ChangeRequest>().AsNoTracking()
                    on workflowTracking.ID equals changeRequestJoin.IMObjID into changeRequestLeftJoinResult
                from changeRequestJoin in changeRequestLeftJoinResult.DefaultIfEmpty()
                join messageQuery in _context.Set<Message.Message>().AsNoTracking()
                    on workflowTracking.ID equals messageQuery.IMObjID into leftJoinResult
                from messageQuery in leftJoinResult.DefaultIfEmpty()
                where messageQuery.IMObjID == null && callJoin.IMObjID == null &&
                      workOrderJoin.IMObjID == null && problemJoin.IMObjID == null && changeRequestJoin.IMObjID == null
                select workflowTracking).ToArray();
        
        var workflowTrackingContext = _context.Set<WorkflowTracking>();
        workflowTrachingDelete.ForEach(x => workflowTrackingContext.Remove(x));
        
        var workflowDelete = (from workflow in _context.Set<WF.Workflow>()
            join callJoin in _context.Set<Call>().AsNoTracking()
                on workflow.ID equals callJoin.IMObjID into callLeftJoinResult
            from callJoin in callLeftJoinResult.DefaultIfEmpty()
            join workOrderJoin in _context.Set<WorkOrder>().AsNoTracking()
                on workflow.ID equals workOrderJoin.IMObjID into workOrderLeftJoinResult
            from workOrderJoin in workOrderLeftJoinResult.DefaultIfEmpty()
            join problemJoin in _context.Set<Problem>().AsNoTracking()
                on workflow.ID equals problemJoin.IMObjID into problemLeftJoinResult
            from problemJoin in problemLeftJoinResult.DefaultIfEmpty()
            join changeRequestJoin in _context.Set<ChangeRequest>().AsNoTracking()
                on workflow.ID equals changeRequestJoin.IMObjID into changeRequestLeftJoinResult
            from changeRequestJoin in changeRequestLeftJoinResult.DefaultIfEmpty()
            join messageQuery in _context.Set<Message.Message>().AsNoTracking()
                on workflow.ID equals messageQuery.IMObjID into leftJoinResult
            from messageQuery in leftJoinResult.DefaultIfEmpty()
            where messageQuery.IMObjID == null && callJoin.IMObjID == null &&
                  workOrderJoin.IMObjID == null && problemJoin.IMObjID == null && changeRequestJoin.IMObjID == null
            select workflow).ToArray();

        var workflowContext = _context.Set<WF.Workflow>();
        workflowDelete.ForEach(x => workflowContext.Remove(x));
    }
}