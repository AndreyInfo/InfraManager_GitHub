using InfraManager.Services.WorkflowService;
using InfraManager.Web.BLL;
using InfraManager.Web.BLL.ProgressBar;
using InfraManager.Web.BLL.TimeManagement;
using InfraManager.Web.DTL;
using InfraManager.Web.DTL.Workflow;
using InfraManager.Web.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.SignalR
{
    public class HubWorker : IHostedService
    {
        private readonly IHubContext<EventHub> _hubContext;

        public HubWorker(IHubContext<EventHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //для обработки от сервера ИМ, от всех клиентов о добавлении, изменнеии, удалении объекта - если служба рабочих процедур доступна
            WorkflowWrapper.EntityModified += WorkflowWrapper_EntityModified;
            //для форм с workflow
            WorkflowWrapper.ExternalEventsCreated += WorkflowWrapper_ExternalEventsCreated;
            WorkflowWrapper.ExternalEventsProcessed += WorkflowWrapper_ExternalEventsProcessed;
            WorkflowWrapper.OnSaveError += WorkflowWrapper_OnSaveError;
            WorkflowWrapper.ObjectChanged += WorkflowWrapper_ObjectChanged;
            //для телефонии
            TelephonyWrapper.CallAnswered += TelephonyWrapper_CallAnswered;
            //для сообщений табеля (не сайт, а ICQ какое-то)
            TimeSheetNote.NoteChanged += TS_MessageInserted;
            //
            ProgressBarWrapper.ObjectChanged += ProgressBarWrapper_ObjectChanged;
            //
            Global.StartWatchdogServiceConnection();
        }

        private void ProgressBarWrapper_ObjectChanged(object sender, Core.EventArgs<ProgressBarInfo> e)
        {
            if (e.Value1 != null)
                EventHub.ProgressBarProcessed(_hubContext, e.Value1.ObjectClassID, e.Value1.ObjectID, e.Value1.ProgressMessage, e.Value1.Percentage, e.Value1.ConnectionID);
        }

        private void TS_MessageInserted(object sender, Core.EventArgs<InfraManager.Web.DTL.TimeManagement.TimeSheetNoteEventInfo> e)
        {
            if (e.Value1 != null)
            {
                switch (e.Value1.Mode)
                {
                    case EventMode.Insert:
                        EventHub.TimeSheet_MessageInserted(_hubContext, e.Value1.MessageID, e.Value1.TimesheetID, e.Value1.OwnerTimesheetID, e.Value1.AuthorID, e.Value1.ConnectionID);
                        break;
                }
            }
        }

        private void TelephonyWrapper_CallAnswered(string fromNumber, List<Guid> userIDs)
        {
            EventHub.CallAnswered(_hubContext, fromNumber, userIDs);
        }

        private void WorkflowWrapper_ObjectChanged(object sender, Core.EventArgs<BaseObjectEventInfo> e)
        {
            if (e.Value1 != null)
            {
                switch (e.Value1.Mode)
                {
                    case EventMode.Insert:
                        EventHub.ObjectInserted(_hubContext, e.Value1.EntityClassID, e.Value1.EntityID, e.Value1.ParentObjectID, e.Value1.ConnectionID);
                        break;
                    case EventMode.Update:
                        EventHub.ObjectUpdated(_hubContext, e.Value1.EntityClassID, e.Value1.EntityID, e.Value1.ParentObjectID, e.Value1.ConnectionID);
                        break;
                    case EventMode.Delete:
                        EventHub.ObjectDeleted(_hubContext, e.Value1.EntityClassID, e.Value1.EntityID, e.Value1.ParentObjectID, e.Value1.ConnectionID);
                        break;
                }
            }
        }

        private void WorkflowWrapper_OnSaveError(Guid entityID, int entityClassID, WorkflowResponse response)
        {
            EventHub.WorkflowOnSaveError(_hubContext, entityClassID, entityID, response);
        }

        private void WorkflowWrapper_ExternalEventsProcessed(object sender, Core.EventArgs<Guid> e)
        {
            EventHub.ExternalEventProcessed(_hubContext, e.Value1);
        }

        private void WorkflowWrapper_ExternalEventsCreated(object sender, Core.EventArgs<Guid> e)
        {
            EventHub.ExternalEventCreated(_hubContext, e.Value1);
        }

        private void WorkflowWrapper_EntityModified(object sender, NativeEventArgs e)
        {
            if (e.EntityID.HasValue && e.EntityClassID.HasValue && e.ApplicationID != Core.ApplicationManager.Instance.ApplicationID)
            {
                switch (e.EventType)
                {
                    case EventType.EntityCreated:
                        EventHub.ObjectInserted(_hubContext, e.EntityClassID.Value, e.EntityID.Value, null, null);
                        break;
                    case EventType.EntityModified:
                        EventHub.ObjectUpdated(_hubContext, e.EntityClassID.Value, e.EntityID.Value, null, null);
                        break;
                    case EventType.EntityDeleted:
                        EventHub.ObjectDeleted(_hubContext, e.EntityClassID.Value, e.EntityID.Value, null, null);
                        break;
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
        }
    }
}
