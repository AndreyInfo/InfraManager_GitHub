using IM.Core.WF.BLL.Interfaces.Models;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;
using WorkflowTracking = InfraManager.DAL.WF.WorkflowTracking;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.WF.BLL.Interfaces
{
    public interface IWorkflowTrackingBLL
    {
        void Delete();

        void Delete(Guid workflowTrackingID, bool isRepeatableRead);

        bool Exists(Guid id);

        void Insert(WorkflowTrackingModel workflowTracking, bool isRepeatableRead);

        void Update(Guid workflowTrackingID, WorkflowStateTrackingModel workflowStateTracking);

        void Update(Guid id, DateTime time);
        
        void UpdateStateTrackingDetail(Guid id, DateTime? nextUtcDate, int? timeSpanInWorkMinutes, int? stageTimeSpanInMinutes, int? stageTimeSpanInWorkMinutes);
        
        WorkflowTrackingModel Get(Guid id);

        WorkflowTracking GetNameDetailsBySchemeIdentifier(string schemeIdentifier);

        CalendarInfo GetEntityCalendarInfo(Guid entityID);

        void Insert(Guid workflowTrackingID, WorkflowStateTrackingModel model);
        /// <summary>
        /// Возвращает список рабочих процедур
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<WorkflowTrackingModel[]> GetListAsync(BaseFilter filter, CancellationToken cancellationToken);

        Task<WorkflowEventModel[]> GetEventsAsync(Guid id, CancellationToken cancellationToken = default);

        Task<WorkflowStateTrackingModel[]> GetStateTrackingsAsync(Guid id,
            CancellationToken cancellationToken = default);
    }
}
