using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.Notification;
using InfraManager.BLL.Workflow;
using InfraManager.DAL;
using InfraManager.DAL.Settings;
using Model = InfraManager.DAL.Notification;

namespace InfraManager.BLL.Settings
{
    [Obsolete("Use ISettingsBLL instead")]
    internal class SupportSettingsBLL : ISupportSettingsBll, ISelfRegisteredService<ISupportSettingsBll>
    {
        private readonly IRepository<Setting> _settings;
        private readonly IWorkflowServiceApi _workflowServiceApi;
        private readonly IReadonlyRepository<Model.Notification> _readOnlyNotificationRepository;
        private readonly INotificationBLL _notificationBLL;
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IEnumerable<IConvertSettingValue> _converters;

        private readonly IMapper _mapper;

        public SupportSettingsBLL(
            IUnitOfWork saveChangesCommand,
            IRepository<Setting> settings,
            IReadonlyRepository<Model.Notification> readOnlyNotificationRepository,
            IEnumerable<IConvertSettingValue> converters,
            IMapper mapper, IWorkflowServiceApi workflowServiceApi, INotificationBLL notificationBLL)
        {
            _saveChangesCommand = saveChangesCommand;
            _settings = settings;
            _readOnlyNotificationRepository = readOnlyNotificationRepository;
            _converters = converters;
            _mapper = mapper;
            _workflowServiceApi = workflowServiceApi;
            _notificationBLL = notificationBLL;
        }

        public T GetSetting<T>(SystemSettings setting)
        {
            var entity = _settings.FirstOrDefault(x => x.Id == setting);

            if (entity != null)
            {
                return GetValueFromByteArray<T>(entity.Value);
            }

            throw new NullReferenceException($"Настройка с ID=\"{(int)setting}\" отсутствует в бд.");
        }

        public async Task UpdateSettingAsync(SystemSettings setting, object value, CancellationToken cancellationToken = default)
        {
            var entity = _settings.FirstOrDefault(x => x.Id == setting);

            if (entity != null)
            {
                entity.Value = GetByteValue(value);
            }

            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

        public async Task UpdateFewSettingsAsync(Dictionary<SystemSettings, object> settingValueDict, CancellationToken cancellationToken = default)
        {
            var entities = _settings.Where(x => settingValueDict.Keys.Contains(x.Id));

            foreach (var entity in entities)
            {
                entity.Value = GetByteValue(settingValueDict[entity.Id]);
            }

            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

        public Dictionary<SystemSettings, dynamic> GetFewSettingsAsync(Dictionary<SystemSettings, Type> settingValueDict)
        {
            var entities = _settings.Where(x => settingValueDict.Keys.Contains(x.Id));
            return entities.ToDictionary(entity => entity.Id, entity => _converters.Convert(settingValueDict[entity.Id], entity.Value));
        }

        #region general

        public SupportSettingsGeneralModel GetGeneralSettings()
        {
            var generalSettings = GetFewSettingsAsync(
                new Dictionary<SystemSettings, Type>
                {
                    { SystemSettings.SupportLineCount, typeof(int) },
                    { SystemSettings.ObjectSearcherResultCount, typeof(int) },
                    { SystemSettings.SearchKBDuringRegisteringNewCall, typeof(bool) },
                    { SystemSettings.UseTTZ, typeof(bool) },
                    { SystemSettings.ManHoursInClosed, typeof(bool) },
                    { SystemSettings.ManHoursValueType, typeof(bool) },
                    { SystemSettings.ReportPeriodType, typeof(int) },
                    { SystemSettings.AutoTimeSheetSetStateToReview, typeof(bool) }
                });

            return new SupportSettingsGeneralModel
            {
                NumberSupportLines = generalSettings[SystemSettings.SupportLineCount],
                NumberSearchResults = generalSettings[SystemSettings.ObjectSearcherResultCount],
                UseDB = generalSettings[SystemSettings.SearchKBDuringRegisteringNewCall],
                UseTTZ = generalSettings[SystemSettings.UseTTZ],
                AllowAdd = generalSettings[SystemSettings.ManHoursInClosed],
                LaborCosts = generalSettings[SystemSettings.ManHoursValueType],
                ReportingPeriod = (PeriodType)generalSettings[SystemSettings.ReportPeriodType],
                AutoChange = generalSettings[SystemSettings.AutoTimeSheetSetStateToReview]
            };
        }

        public async Task UpdateGeneralSettings(SupportSettingsGeneralModel supportSettingsGeneralModel, CancellationToken cancellationToken = default)
        {
            await UpdateFewSettingsAsync(
                new Dictionary<SystemSettings, object>
                {
                    { SystemSettings.SupportLineCount, supportSettingsGeneralModel.NumberSupportLines },
                    { SystemSettings.ObjectSearcherResultCount, supportSettingsGeneralModel.NumberSearchResults },
                    { SystemSettings.SearchKBDuringRegisteringNewCall, supportSettingsGeneralModel.UseDB },
                    { SystemSettings.UseTTZ, supportSettingsGeneralModel.UseTTZ },
                    { SystemSettings.ManHoursInClosed, supportSettingsGeneralModel.AllowAdd },
                    { SystemSettings.ManHoursValueType, supportSettingsGeneralModel.LaborCosts },
                    { SystemSettings.ReportPeriodType, (int)supportSettingsGeneralModel.ReportingPeriod },
                    { SystemSettings.AutoTimeSheetSetStateToReview, supportSettingsGeneralModel.AutoChange}
                },
                cancellationToken);
        }

        #endregion

        #region tasks

        public SupportSettingsTasksModel GetTasksSettings()
        {
            var generalSettings = GetFewSettingsAsync(
                new Dictionary<SystemSettings, Type>
                {
                    { SystemSettings.DefaultWorkOrderWorkflowSchemeIdentifier, typeof(string) },
                    { SystemSettings.NotifyOnWorkOrderFinishDateViolation, typeof(bool) },
                    { SystemSettings.WorkOrderFinishDateDelta, typeof(long) },
                });

            //получам timespan по числу тактов
            var workOrderFinishDateDelta = new TimeSpan(generalSettings[SystemSettings.WorkOrderFinishDateDelta]);

            return new SupportSettingsTasksModel
            {
                DefaultWorkOrderWorkflowSchemeIdentifier = generalSettings[SystemSettings.DefaultWorkOrderWorkflowSchemeIdentifier],
                Warn = generalSettings[SystemSettings.NotifyOnWorkOrderFinishDateViolation],
                Hours = workOrderFinishDateDelta.Hours,
                Minutes = workOrderFinishDateDelta.Minutes
            }; // TODO: Это все через автомаппер надо делать
        }

        public async Task UpdateTasksSettings(SupportSettingsTasksModel supportSettingsGeneralModel, CancellationToken cancellationToken = default)
        {
            //число тактов для дельты {часы}:{минуты}
            var timeSpanWorkOrderFinishDateDelta = new TimeSpan(supportSettingsGeneralModel.Hours, supportSettingsGeneralModel.Minutes, 0).Ticks;

            await UpdateFewSettingsAsync(new Dictionary<SystemSettings, object>
            {
                { SystemSettings.DefaultWorkOrderWorkflowSchemeIdentifier, supportSettingsGeneralModel.DefaultWorkOrderWorkflowSchemeIdentifier },
                { SystemSettings.NotifyOnWorkOrderFinishDateViolation, supportSettingsGeneralModel.Warn},
                { SystemSettings.WorkOrderFinishDateDelta, timeSpanWorkOrderFinishDateDelta },
            }, cancellationToken);
        }

        #endregion

        #region problems

        public SupportSettingsProblemsModel GetProblemsSettings()
        {
            var generalSettings = GetFewSettingsAsync(
                new Dictionary<SystemSettings, Type>
                {
                    { SystemSettings.DefaultProblemWorkflowSchemeIdentifier, typeof(string) },
                    { SystemSettings.NotifyOnWorkOrderFinishDateViolation, typeof(bool) },
                    { SystemSettings.RecalculateProblemAdditionalParametersWithProblemTypeChange, typeof(bool) },
                    { SystemSettings.ProblemPromiseDateDelta, typeof(long) },
                });

            //получам timespan по числу тактов
            var workOrderFinishDateDelta = new TimeSpan(generalSettings[SystemSettings.ProblemPromiseDateDelta]);

            return new SupportSettingsProblemsModel
            {
                DefaultProblemWorkflowSchemeIdentifier = generalSettings[SystemSettings.DefaultProblemWorkflowSchemeIdentifier],
                UpdateComposition = generalSettings[SystemSettings.NotifyOnWorkOrderFinishDateViolation],
                Warn = generalSettings[SystemSettings.RecalculateProblemAdditionalParametersWithProblemTypeChange],
                Hours = workOrderFinishDateDelta.Hours,
                Minutes = workOrderFinishDateDelta.Minutes,
                Ticks = generalSettings[SystemSettings.ProblemPromiseDateDelta]
            };
        }

        public async Task UpdateProblemsSettings(SupportSettingsProblemsModel supportSettingsProblemsModel, CancellationToken cancellationToken = default)
        {
            //число тактов для дельты {часы}:{минуты}
            var timeSpanProblemPromiseDateDelta = new TimeSpan(supportSettingsProblemsModel.Hours, supportSettingsProblemsModel.Minutes, 0).Ticks;

            await UpdateFewSettingsAsync(new Dictionary<SystemSettings, object>
            {
                { SystemSettings.DefaultProblemWorkflowSchemeIdentifier, supportSettingsProblemsModel.DefaultProblemWorkflowSchemeIdentifier },
                { SystemSettings.NotifyOnWorkOrderFinishDateViolation, supportSettingsProblemsModel.UpdateComposition },
                { SystemSettings.RecalculateProblemAdditionalParametersWithProblemTypeChange, supportSettingsProblemsModel.Warn },
                { SystemSettings.ProblemPromiseDateDelta, timeSpanProblemPromiseDateDelta }
            }, cancellationToken);
        }

        #endregion

        #region agreements
        public SupportSettingsAgreementsModelDetails GetAgrementSettings()
        {
            var generalSettings = GetFewSettingsAsync(
                   new Dictionary<SystemSettings, Type>
                   {
                    { SystemSettings.NegotiatorDeleteMessageTemplate, typeof(string) },
                    { SystemSettings.NegotiationStartMessageTemplate, typeof(string) },
                    { SystemSettings.CommentNonPlacet, typeof(bool) },
                    { SystemSettings.CommentPlacet, typeof(bool) },
                   });

            Guid negotiatorDeleteMessageTemplateGuid = Guid.Parse(generalSettings[SystemSettings.NegotiatorDeleteMessageTemplate]);
            Guid negotiationStartMessageTemplateGuid = Guid.Parse(generalSettings[SystemSettings.NegotiationStartMessageTemplate]);

            var result = _readOnlyNotificationRepository.Where(x => x.ID == negotiatorDeleteMessageTemplateGuid || x.ID == negotiationStartMessageTemplateGuid);


            return new SupportSettingsAgreementsModelDetails
            {
                TemplateDeleteApprover = _mapper.Map<NotificationNameDetails>(result.FirstOrDefault(x => x.ID == negotiatorDeleteMessageTemplateGuid)),
                TemplateStartAgreement = _mapper.Map<NotificationNameDetails>(result.FirstOrDefault(x => x.ID == negotiationStartMessageTemplateGuid)),
                IsNoCommentNeeded = generalSettings[SystemSettings.CommentNonPlacet],
                IsYesCommentNeeded = generalSettings[SystemSettings.CommentPlacet]
            };
        }

        public async Task UpdateAgreementSettingsAsync(SupportSettingsAgreementsModelDetails model, CancellationToken cancellationToken = default)
        {
            await UpdateFewSettingsAsync(new Dictionary<SystemSettings, object>
            {
                { SystemSettings.NegotiatorDeleteMessageTemplate, model.TemplateDeleteApprover.ID.ToString() },
                { SystemSettings.NegotiationStartMessageTemplate, model.TemplateStartAgreement.ID.ToString() },
                { SystemSettings.CommentNonPlacet, model.IsNoCommentNeeded},
                { SystemSettings.CommentPlacet, model.IsYesCommentNeeded },
            }, cancellationToken);
        }


        #endregion

        #region requests

        public async Task<SupportSettingsRequestsModel> GetRequestsSettingsAsync(CancellationToken cancellationToken)
        {
            var generalSettings = GetFewSettingsAsync(new Dictionary<SystemSettings, Type>
            {
                {SystemSettings.DefaultCallWorkflowSchemeIdentifier, typeof(string)},
                {SystemSettings.RecalculateCallAdditionalParametersWithServiceChange, typeof(bool)},
                {SystemSettings.CallHidePlaceOfService, typeof(bool)},
                {SystemSettings.CallAddDependencyMode, typeof(int)},
                {SystemSettings.CallPromiseDateCalculationMode, typeof(byte)},
                {SystemSettings.CallPromiseDateDelta, typeof(long)},
                {SystemSettings.NotifyOnCallPromiseDateViolation, typeof(bool)},
                {SystemSettings.ClientCallRegistrationMessageTemplate,typeof(string)},
                {SystemSettings.CallEmailTemplateID, typeof(string)}
            });

            Guid callEmailTemplateID = Guid.Parse(generalSettings[SystemSettings.CallEmailTemplateID]);
            Guid clientCallRegistrationMessageTemplateID = Guid.Parse(generalSettings[SystemSettings.ClientCallRegistrationMessageTemplate]);

            var resultNotifications = _readOnlyNotificationRepository.Where(x => x.ID == clientCallRegistrationMessageTemplateID || x.ID == callEmailTemplateID);


            var wfSchemeIdentifier = (string)generalSettings[SystemSettings.DefaultCallWorkflowSchemeIdentifier];
            var wfScheme = await _workflowServiceApi.GetWorkFlowSchemeByIdentifierAsync(wfSchemeIdentifier, cancellationToken);

            return new SupportSettingsRequestsModel
            {
                Workflow = _mapper.Map<WorkflowSchemeNameDetails>(wfScheme),
                UpdateComposition = generalSettings[SystemSettings.RecalculateCallAdditionalParametersWithServiceChange],
                HideField = generalSettings[SystemSettings.CallHidePlaceOfService],
                ModeID = (CallAddDependencyMode)generalSettings[SystemSettings.CallAddDependencyMode],
                CountTime = (CallPromiseDateCalculationMode)generalSettings[SystemSettings.CallPromiseDateCalculationMode],
                Hours = generalSettings[SystemSettings.CallPromiseDateDelta] / 60,
                Minutes = generalSettings[SystemSettings.CallPromiseDateDelta] % 60,
                WarnSLA = generalSettings[SystemSettings.NotifyOnCallPromiseDateViolation],
                TemplateMail = _mapper.Map<NotificationNameDetails>(resultNotifications.FirstOrDefault(x => x.ID == callEmailTemplateID)),
                TemplateSignUp = _mapper.Map<NotificationNameDetails>(resultNotifications.FirstOrDefault(x => x.ID == clientCallRegistrationMessageTemplateID))
            };
        }

        public async Task UpdateRequestsSettingsAsync(SupportSettingsRequestsModel settings, CancellationToken cancellationToken) =>
            await UpdateFewSettingsAsync(new Dictionary<SystemSettings, object>
            {
                {SystemSettings.DefaultCallWorkflowSchemeIdentifier, settings.Workflow.Identifier},
                {SystemSettings.RecalculateCallAdditionalParametersWithServiceChange, settings.UpdateComposition},
                {SystemSettings.CallHidePlaceOfService, settings.HideField},
                {SystemSettings.CallAddDependencyMode, (int) settings.ModeID},
                {SystemSettings.CallPromiseDateCalculationMode, (byte) settings.CountTime},
                {SystemSettings.CallPromiseDateDelta, settings.Hours * 60 + settings.Minutes},
                {SystemSettings.NotifyOnCallPromiseDateViolation, settings.WarnSLA},
                {SystemSettings.ClientCallRegistrationMessageTemplate, settings.TemplateMail.ID.ToString()},
                {SystemSettings.CallEmailTemplateID, settings.TemplateSignUp.ID.ToString()}
            }, cancellationToken);

        

        #endregion

        #region notifications

        public SpecialNotificationOnAgreement GetNotificationOnAgreement()
        {
            var generalSettings = GetFewSettingsAsync(new Dictionary<SystemSettings, Type>
            {
                {SystemSettings.NegotiationStartMessageTemplate, typeof(string)},
                {SystemSettings.NegotiatorDeleteMessageTemplate, typeof(string)}
            });
            
            return new SpecialNotificationOnAgreement
            {
                StartMessageTemplate = new NotificationTemplateName
                {
                    Identifier = generalSettings[SystemSettings.NegotiationStartMessageTemplate],
                    Name = _notificationBLL.GetNameByID(Guid.Parse(generalSettings[SystemSettings.NegotiationStartMessageTemplate]))
                },
                DeleteMemberMessageTemplate = new NotificationTemplateName
                {
                    Identifier = generalSettings[SystemSettings.NegotiatorDeleteMessageTemplate],
                    Name = _notificationBLL.GetNameByID(Guid.Parse(generalSettings[SystemSettings.NegotiatorDeleteMessageTemplate]))
                }
            };
        }

        public async Task UpdateNotificationOnAgreementAsync(SpecialNotificationOnAgreement specialNotification, CancellationToken cancellationToken) =>
            await UpdateFewSettingsAsync(new Dictionary<SystemSettings, object>
            {
                {SystemSettings.NegotiationStartMessageTemplate, specialNotification.StartMessageTemplate.Identifier},
                {SystemSettings.NegotiatorDeleteMessageTemplate, specialNotification.DeleteMemberMessageTemplate.Identifier}
            }, cancellationToken);

        public SpecialNotificationOnControl GetNotificationOnControl()
        {
            var generalSettings = GetFewSettingsAsync(new Dictionary<SystemSettings, Type>
            {
                {SystemSettings.AddCustomControllers, typeof(string)},
                {SystemSettings.DeleteCustomControllers, typeof(string)}
            });
            
            return new SpecialNotificationOnControl
            {
                AddCustomControllers = new NotificationTemplateName
                {
                    Identifier = generalSettings[SystemSettings.AddCustomControllers],
                    Name = _notificationBLL.GetNameByID(Guid.Parse(generalSettings[SystemSettings.AddCustomControllers]))
                },
                DeleteCustomControllers = new NotificationTemplateName
                {
                    Identifier = generalSettings[SystemSettings.DeleteCustomControllers],
                    Name = _notificationBLL.GetNameByID(Guid.Parse(generalSettings[SystemSettings.DeleteCustomControllers]))
                }
            };
        }

        public async Task UpdateNotificationOnControlAsync(SpecialNotificationOnControl specialNotification,
            CancellationToken cancellationToken)
        {
            await UpdateFewSettingsAsync(new Dictionary<SystemSettings, object>
            {
                {SystemSettings.AddCustomControllers, specialNotification.AddCustomControllers.Identifier},
                {SystemSettings.DeleteCustomControllers, specialNotification.DeleteCustomControllers.Identifier}
            }, cancellationToken);
        }

        public SpecialNotificationOnReplacement GetNotificationOnReplacement()
        {
            var generalSettings = GetFewSettingsAsync(new Dictionary<SystemSettings, Type>
            {
                {SystemSettings.AddSubstitution, typeof(string)},
                {SystemSettings.DeleteSubstitution, typeof(string)},
                {SystemSettings.ChangeDatesSubstitution, typeof(string)}
            });
            
            return new SpecialNotificationOnReplacement
            {
                AddSubstitution = new NotificationTemplateName
                {
                    Identifier = generalSettings[SystemSettings.AddSubstitution],
                    Name = _notificationBLL.GetNameByID(Guid.Parse(generalSettings[SystemSettings.AddSubstitution]))
                },
                DeleteSubstitution = new NotificationTemplateName
                {
                    Identifier = generalSettings[SystemSettings.DeleteSubstitution],
                    Name = _notificationBLL.GetNameByID(Guid.Parse(generalSettings[SystemSettings.DeleteSubstitution]))
                },
                ChangeDatesSubstitution = new NotificationTemplateName
                {
                    Identifier = generalSettings[SystemSettings.ChangeDatesSubstitution],
                    Name = _notificationBLL.GetNameByID(Guid.Parse(generalSettings[SystemSettings.ChangeDatesSubstitution]))
                }
                
            };
        }

        public async Task UpdateNotificationOnReplacementAsync(
            SpecialNotificationOnReplacement specialNotification,
            CancellationToken cancellationToken)
        {
            await UpdateFewSettingsAsync(new Dictionary<SystemSettings, object>
            {
                {SystemSettings.AddSubstitution, specialNotification.AddSubstitution.Identifier},
                {SystemSettings.DeleteSubstitution, specialNotification.DeleteSubstitution.Identifier},
                {SystemSettings.ChangeDatesSubstitution,specialNotification.ChangeDatesSubstitution.Identifier}
            }, cancellationToken);
        }

        public SpecialNotificationOnRequest GetNotificationOnRequest()
        {
            var generalSettings = GetFewSettingsAsync(new Dictionary<SystemSettings, Type>
            {
                {SystemSettings.CallEmailTemplateID, typeof(string)},
                {SystemSettings.ClientCallRegistrationMessageTemplate, typeof(string)}
            });
            
            return new SpecialNotificationOnRequest
            {
                CallEmailTemplate = new NotificationTemplateName
                {
                    Identifier = generalSettings[SystemSettings.CallEmailTemplateID],
                    Name = _notificationBLL.GetNameByID(Guid.Parse(generalSettings[SystemSettings.CallEmailTemplateID]))
                },
                ClientCallRegistrationMessageTemplate = new NotificationTemplateName
                {
                    Identifier = generalSettings[SystemSettings.ClientCallRegistrationMessageTemplate],
                    Name = _notificationBLL.GetNameByID(Guid.Parse(generalSettings[SystemSettings.ClientCallRegistrationMessageTemplate]))
                }
            };
        }

        public async Task UpdateNotificationOnRequestAsync(SpecialNotificationOnRequest specialNotification,
            CancellationToken cancellationToken)
        {
            await UpdateFewSettingsAsync(new Dictionary<SystemSettings, object>
            {
                {SystemSettings.CallEmailTemplateID, specialNotification.CallEmailTemplate.Identifier},
                {SystemSettings.ClientCallRegistrationMessageTemplate, specialNotification.ClientCallRegistrationMessageTemplate.Identifier}
            }, cancellationToken);
        }

        #endregion
        


        #region private

        private byte[] GetByteValue(object value)
        {
            return _converters.ConvertBack(value);
        }

        private T GetValueFromByteArray<T>(byte[] array)
        {
            return _converters.Convert<T>(array);
        }


        #endregion

    }
}
