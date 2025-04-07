using AutoMapper;
using InfraManager.Core;
using InfraManager.DAL;
using InfraManager.DAL.Messages;
using InfraManager.WebApi.Contracts.Models.EMailProtocol;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Messages
{
    public class EMailProtocolBLL : IEMailProtocolBLL, ISelfRegisteredService<IEMailProtocolBLL>
    {
        private readonly IEMailProtocolQuery _protocolQuery;
        private readonly IFinder<DAL.Notification.Notification> _finder;
        private readonly IMapper _mapper;

        public EMailProtocolBLL(
            IEMailProtocolQuery protocolQuery,
            IFinder<DAL.Notification.Notification> finder,
            IMapper mapper)
        {
            _protocolQuery = protocolQuery;
            _finder = finder;
            _mapper = mapper;
        }

        public async Task<NotificationReceiverDetails[]> GetEMAilsAsync(EMailListRequest request, CancellationToken cancellationToken)
        {
            // TODO: ЗАдейсвовать IServiceMapper на BusinessRole. Проанализровать исходные запрос в InfraManager.DAL.IM\Notifications\EmailProtocolDataManager.cs
            switch ((BusinessRole)request.BusinessRole)
            {
                case BusinessRole.SDAdministrator:
                    return _mapper.Map<NotificationReceiverDetails[]>(
                        await _protocolQuery.ExecuteForAdministratorAsync(request.Scope, request.NotificationID,
                            request.ObjectID, request.BusinessRole, cancellationToken));
                
                case BusinessRole.CallOwner:
                    var callNotification =
                        await _finder.FindOrRaiseErrorAsync(request.NotificationID, cancellationToken);
                    return _mapper.Map<NotificationReceiverDetails[]>(
                        await _protocolQuery.ExecuteForCallOwnerAsync(request.Scope, request.NotificationID,
                            request.ObjectID, request.BusinessRole, callNotification.ClassID, cancellationToken));
                
                case BusinessRole.CallClient:
                    return _mapper.Map<NotificationReceiverDetails[]>(
                        await _protocolQuery.ExecuteForCallClientAsync(request.Scope, request.NotificationID,
                            request.ObjectID, request.BusinessRole, cancellationToken));
                
                case BusinessRole.CallInitiator:
                    return _mapper.Map<NotificationReceiverDetails[]>(
                        await _protocolQuery.ExecuteForCallInitiatorAsync(request.Scope, request.NotificationID,
                            request.ObjectID, request.BusinessRole, cancellationToken));
                
                case BusinessRole.CallAccomplisher:
                    return _mapper.Map<NotificationReceiverDetails[]>(
                        await _protocolQuery.ExecuteForCallAccomplisherAsync(request.Scope, request.NotificationID,
                            request.ObjectID, request.BusinessRole, cancellationToken));
                
                case BusinessRole.CallExecutor:
                    return _mapper.Map<NotificationReceiverDetails[]>(
                        await _protocolQuery.ExecuteForCallExecutorAsync(request.Scope, request.NotificationID,
                            request.ObjectID, request.BusinessRole, cancellationToken));
                
                case BusinessRole.ProblemOwner:
                    var problemNotification =
                        await _finder.FindOrRaiseErrorAsync(request.NotificationID, cancellationToken);
                    return _mapper.Map<NotificationReceiverDetails[]>(
                        await _protocolQuery.ExecuteForProblemOwnerAsync(request.Scope, request.NotificationID,
                            request.ObjectID, request.BusinessRole, problemNotification.ClassID, cancellationToken));
                
                case BusinessRole.WorkOrderExecutor:
                    return _mapper.Map<NotificationReceiverDetails[]>(
                        await _protocolQuery.ExecuteForWorkOrderExecutorAsync(request.Scope, request.NotificationID,
                            request.ObjectID, request.BusinessRole, cancellationToken));
                
                case BusinessRole.WorkOrderAssignor:
                    return _mapper.Map<NotificationReceiverDetails[]>(
                        await _protocolQuery.ExecuteForWorkOrderAssignorAsync(request.Scope, request.NotificationID,
                            request.ObjectID, request.BusinessRole, cancellationToken));
                
                case BusinessRole.WorkOrderInitiator:
                    return _mapper.Map<NotificationReceiverDetails[]>(
                        await _protocolQuery.ExecuteForWorkOrderInitiatorAsync(request.Scope, request.NotificationID,
                            request.ObjectID, request.BusinessRole, cancellationToken));
                
                case BusinessRole.NegotiationParticipant:
                    return _mapper.Map<NotificationReceiverDetails[]>(
                        await _protocolQuery.ExecuteForNegotiationParticipantAsync(request.Scope,
                            request.NotificationID, request.ObjectID, request.BusinessRole, cancellationToken));
                
                case BusinessRole.ControllerParticipant:
                    return _mapper.Map<NotificationReceiverDetails[]>(
                        await _protocolQuery.ExecuteForControllerParticipantAsync(request.Scope, request.NotificationID,
                            request.ObjectID, request.BusinessRole, cancellationToken));
                
                case BusinessRole.RFCInitiator:
                    return _mapper.Map<NotificationReceiverDetails[]>(
                        await _protocolQuery.ExecuteForRFCInitiatorAsync(request.Scope, request.NotificationID,
                            request.ObjectID, request.BusinessRole, cancellationToken));
                
                case BusinessRole.RFCOwner:
                    var rfcNotification =
                        await _finder.FindOrRaiseErrorAsync(request.NotificationID, cancellationToken);
                    return _mapper.Map<NotificationReceiverDetails[]>(
                        await _protocolQuery.ExecuteForRFCOwnerAsync(request.Scope, request.NotificationID,
                            request.ObjectID, request.BusinessRole, rfcNotification.ClassID, cancellationToken));
                
                case BusinessRole.ReplacedUser:
                    return _mapper.Map<NotificationReceiverDetails[]>(
                        await _protocolQuery.ExecuteForReplacedUserAsync(request.Scope, request.NotificationID,
                            request.ObjectID, request.BusinessRole, cancellationToken));
                
                case BusinessRole.DeputyUser:
                    return _mapper.Map<NotificationReceiverDetails[]>(
                        await _protocolQuery.ExecuteForDeputyUserAsync(request.Scope, request.NotificationID,
                            request.ObjectID, request.BusinessRole, cancellationToken));
                
                case BusinessRole.ProblemExecutor:
                    return _mapper.Map<NotificationReceiverDetails[]>(
                        await _protocolQuery.ExecuteForProblemExecutorAsync(request.Scope, request.NotificationID,
                            request.ObjectID, request.BusinessRole, cancellationToken));
                
                case BusinessRole.ProblemInitiator:
                    return _mapper.Map<NotificationReceiverDetails[]>(
                        await _protocolQuery.ExecuteForProblemInitiatorAsync(request.Scope, request.NotificationID,
                            request.ObjectID, request.BusinessRole, cancellationToken));

                #region MassIncident

                case BusinessRole.MassIncidentInitiator:
                    return _mapper.Map<NotificationReceiverDetails[]>(
                        await _protocolQuery.ExecuteForMassIncidentInitiatorAsync(request.Scope, request.NotificationID,
                            request.ObjectID, request.BusinessRole, cancellationToken));

                case BusinessRole.MassIncidentOwner:
                {
                    return _mapper.Map<NotificationReceiverDetails[]>(
                        await _protocolQuery.ExecuteForMassIncidentOwnerAsync(request.Scope, request.NotificationID,
                            request.ObjectID, request.BusinessRole, cancellationToken));
                }

                case BusinessRole.MassIncidentExecutor:
                    return _mapper.Map<NotificationReceiverDetails[]>(
                        await _protocolQuery.ExecuteForMassIncidentExecutorAsync(request.Scope, request.NotificationID,
                            request.ObjectID, request.BusinessRole, cancellationToken));
                #endregion
            }

            throw new ApplicationException($"Бизнес роль {request.BusinessRole} ({(BusinessRole)request.BusinessRole}) не известна");
        }
    }
}
