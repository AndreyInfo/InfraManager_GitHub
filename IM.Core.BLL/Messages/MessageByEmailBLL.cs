using AutoMapper;
using InfraManager.BLL.Settings;
using InfraManager.BLL.Workflow;
using InfraManager.DAL;
using InfraManager.DAL.Message;
using InfraManager.DAL.Messages;
using InfraManager.DAL.Settings;
using InfraManager.DAL.WF;
using InfraManager.ServiceBase.MailService.WebAPIModels;
using InfraManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Inframanager.BLL;
using InfraManager.ResourcesArea;
using InfraManager.DAL.Documents;

namespace InfraManager.BLL.Messages
{
    public class MessageByEmailBLL : IMessageByEmailBLL, ISelfRegisteredService<IMessageByEmailBLL>
    {
        private readonly IMessageByEMailQuery _messageByEMailQuery;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly IFinder<Setting> _settingsFinder;
        private readonly IRepository<WorkflowRequest> _workflowRequests;
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IRepository<Message> _messageRepository;
        private readonly IRepository<MessageByEmail> _byEmailRepository;
        private readonly IRepository<Setting> _settingsRepository;
        private readonly IWorkflowServiceApi _workflowApi;
        private readonly IFindEntityByGlobalIdentifier<MessageByEmail> _finder;
        private readonly IModifyEntityBLL<Guid, MessageByEmail, MessageByEmailData, MessageByEmailDetails> _modifyEntity;
        private readonly IRepository<DocumentReference> _documentReferenceRepository;
        private readonly ICreateWorkflow<MessageByEmail> _workflow;

        public MessageByEmailBLL(
            IMessageByEMailQuery messageByEMailQuery,
            IMapper mapper,
            ICurrentUser currentUser,
            IFinder<Setting> settingsFinder,
            IRepository<WorkflowRequest> workflowRequests,
            IUnitOfWork saveChangesCommand,
            IRepository<Message> messageRepository,
            IRepository<MessageByEmail> byEmailRepository,
            IRepository<Setting> settingsRepository,
            IWorkflowServiceApi workflowApi,
            IFindEntityByGlobalIdentifier<MessageByEmail> finder,
            IModifyEntityBLL<Guid, MessageByEmail, MessageByEmailData, MessageByEmailDetails> modifyEntity,
            IRepository<DocumentReference> documentReferenceRepository,
            ICreateWorkflow<MessageByEmail> workflow)
        {
            _messageByEMailQuery = messageByEMailQuery;
            _mapper = mapper;
            _currentUser = currentUser;
            _settingsFinder = settingsFinder;
            _workflowRequests = workflowRequests;
            _saveChangesCommand = saveChangesCommand;
            _messageRepository = messageRepository;
            _byEmailRepository = byEmailRepository;
            _settingsRepository = settingsRepository;
            _workflowApi = workflowApi;
            _finder = finder;
            _modifyEntity = modifyEntity;
            _documentReferenceRepository = documentReferenceRepository;
            _workflow = workflow;
        }

        public async Task<MessageByEmailDetails> AddAsync(MessageByEmailData messageByEmailData, CancellationToken cancellationToken = default)
        {
            if (messageByEmailData == null)
            {
                throw new InvalidObjectException(Resources.MessageByEmail_MissingData);
            }

            var messageMimeIDExists = !string.IsNullOrWhiteSpace(messageByEmailData.MessageMimeId) &&
                                      _messageByEMailQuery.Query(_currentUser.UserId,
                                          m => m.MessageMimeId.Equals(messageByEmailData.MessageMimeId)
                                          ).Any();
            var emailHasBeenReceivedEarlier = messageByEmailData.UtcDateReceived != null
                                              && _messageByEMailQuery.Query(_currentUser.UserId,
                                                  m => m.From == messageByEmailData.From &&
                                                       m.To == messageByEmailData.To &&
                                                       m.Title == messageByEmailData.Title &&
                                                       m.UtcDateReceived == messageByEmailData.UtcDateReceived
                                                       ).Any(); 
            if (messageMimeIDExists || emailHasBeenReceivedEarlier)
            {
                var result = _mapper.Map<MessageByEmailDetails>(_mapper.Map<MessageByEmail>(messageByEmailData));
                result.IsDuplicate = true;
                return result;
            }

            var messageID = Guid.NewGuid();
            var workflowRequest = new WorkflowRequest { Id = messageID };

            MessageByEmail messageByEMail;

            try
            {
                var message = new Message
                {
                    IMObjID = messageID,
                    UtcDateRegistered = DateTime.UtcNow,
                    SeverityType = messageByEmailData.SeverityType,
                    Type = (byte)MessageTypeEnum.Email,
                    WorkflowSchemeIdentifier = messageByEmailData.WorkflowSchemeIdentifier,
                };

                messageByEMail = new MessageByEmail(message);

                _mapper.Map(messageByEmailData, messageByEMail);

                if (string.IsNullOrWhiteSpace(messageByEMail.MessageMimeId))
                {
                    messageByEMail.MessageMimeId = messageID.ToString();
                }

                await _workflow.TryStartNewAsync(messageByEMail, cancellationToken);


                _messageRepository.Insert(message);
                _byEmailRepository.Insert(messageByEMail);

                if (messageByEmailData.AttachmentIDs != null)
                {
                    messageByEmailData.AttachmentIDs.ForEach(docID =>
                        _documentReferenceRepository.Insert(new DocumentReference(docID, messageID, ObjectClass.MessageByEmail)));
                }
            }
            catch
            {
                _workflowRequests.Delete(workflowRequest);
                await _saveChangesCommand.SaveAsync(cancellationToken);
                throw;
            }
            await _saveChangesCommand.SaveAsync(cancellationToken);

            return _mapper.Map<MessageByEmailDetails>(messageByEMail);

        }

        public async Task<MessageByEmailDetails> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _finder.With(x => x.Message);
            var message = await FindOrRaiseErrorAsync(id, cancellationToken);

            return _mapper.Map<MessageByEmailDetails>(message);
        }

        public async Task<byte[]> GetRulesSettingsValueAsync(CancellationToken cancellationToken = default)
        {
            var ruleSetValue = await _settingsFinder.FindAsync(SystemSettings.MessageByEmailRuleSet, cancellationToken);
            if (ruleSetValue == null)
                //TODO локализация, текст данной ошибки прилетает конечному пользователю
                throw new InvalidObjectException("Rules for MEssageby Email processing are missing");

            return ruleSetValue.Value;
        }

        
        private async Task<MessageByEmail> FindOrRaiseErrorAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var negotiation = await _finder.FindAsync(id, cancellationToken);

            if (negotiation == null)
            {
                throw new ObjectNotFoundException<Guid>(id, ObjectClass.Negotiation);
            }

            return negotiation;
        }

        private string GetRuleSetDocument(List<MessageProcessingRule> messageProcessingRuleList)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement messageProcessingRuleSet = doc.CreateElement(string.Empty, "MessageProcessingRuleSet", string.Empty);
            doc.AppendChild(messageProcessingRuleSet);
            XmlElement messageProcessingRules = doc.CreateElement(string.Empty, "MessageProcessingRules", string.Empty);
            messageProcessingRuleSet.AppendChild(messageProcessingRules);
            foreach (var rule in messageProcessingRuleList)
            {
                XmlElement element = doc.CreateElement(string.Empty, nameof(MessageProcessingRule), string.Empty);
                XmlElement name = doc.CreateElement(string.Empty, nameof(MessageProcessingRule.Name), string.Empty);
                XmlText nameText = doc.CreateTextNode(rule.Name);
                name.AppendChild(nameText);
                XmlElement note = doc.CreateElement(string.Empty, nameof(MessageProcessingRule.Note), string.Empty);
                XmlText noteText = doc.CreateTextNode(rule.Note);
                note.AppendChild(noteText);
                XmlElement workflowSchemeIdentifier = doc.CreateElement(string.Empty, nameof(MessageProcessingRule.WorkflowSchemeIdentifier), string.Empty);
                XmlText workflowSchemeIdentifierText = doc.CreateTextNode(rule.WorkflowSchemeIdentifier);
                workflowSchemeIdentifier.AppendChild(workflowSchemeIdentifierText);

                XmlElement conditions = doc.CreateElement(string.Empty, "Conditions", string.Empty);
                foreach (var condition in rule.Conditions)
                {
                    XmlElement conditionElement = doc.CreateElement(string.Empty, condition.Key, string.Empty);
                    if (!string.IsNullOrEmpty(condition.Parameter))
                    {
                        XmlElement parameter = doc.CreateElement(string.Empty, nameof(Condition.Parameter), string.Empty);
                        XmlText parameterText = doc.CreateTextNode(condition.Parameter);
                        parameter.AppendChild(parameterText);
                        conditionElement.AppendChild(parameter);
                    }
                    conditions.AppendChild(conditionElement);
                }
                element.AppendChild(name);
                element.AppendChild(note);
                element.AppendChild(workflowSchemeIdentifier);
                element.AppendChild(conditions);
                messageProcessingRules.AppendChild(element);
            }
            return System.Xml.Linq.XElement.Parse(doc.OuterXml).ToString();
        }

        public async Task<OperationResult> SetRulesSettingsValueAsync(List<MessageProcessingRule> messageProcessingRuleList, CancellationToken cancellationToken = default)
        {
            var settings = Encoding.UTF8.GetBytes(GetRuleSetDocument(messageProcessingRuleList));
            var ruleSetValue = await _settingsFinder.FindAsync(SystemSettings.MessageByEmailRuleSet, cancellationToken);
            if (ruleSetValue == null)
            {
                var rules = new Setting(SystemSettings.MessageByEmailRuleSet, settings);
                _settingsRepository.Insert(rules);
            }
            else
            {
                ruleSetValue.Value = settings;
            }
            await _saveChangesCommand.SaveAsync(cancellationToken);
            return new OperationResult() { Type = OperationResultType.Success };
        }

        public async Task<OperationResult> SetMailSettingsValueAsync(MailServiceSettings mailSetting, CancellationToken cancellationToken = default)
        {
            var mailSettingString = JsonSerializer.Serialize(mailSetting);
            var settings = Encoding.UTF8.GetBytes(mailSettingString);
            var mailSettingValue = await _settingsFinder.FindAsync(SystemSettings.MailServiceSetting, cancellationToken);
            if (mailSettingValue == null)
            {
                var serviceSettings = new Setting(SystemSettings.MailServiceSetting, settings);
                _settingsRepository.Insert(serviceSettings);
            }
            else
            {
                mailSettingValue.Value = settings;
            }
            await _saveChangesCommand.SaveAsync(cancellationToken);
            return new OperationResult() { Type = OperationResultType.Success };
        }

        public async Task<bool> GetCitateTrimmerUsingAsync(CancellationToken cancellationToken = default)
        {
            var citateTrimmerUsingSetting = await _settingsFinder.FindAsync(SystemSettings.CitateTrimmerUsing, cancellationToken);
            return citateTrimmerUsingSetting.Value[0] == 1;
        }

        public async Task<OperationResult> SetCitateTrimmerUsingAsync(bool citateTrimmerUsing, CancellationToken cancellationToken = default)
        {
            byte ct = citateTrimmerUsing ? (byte)1 : (byte)0;
            var value = new byte[] { ct };
            var citateTrimmerUsingValue = await _settingsFinder.FindAsync(SystemSettings.CitateTrimmerUsing, cancellationToken);
            if (citateTrimmerUsingValue == null)
            {
                var rules = new Setting(SystemSettings.CitateTrimmerUsing, value);
                _settingsRepository.Insert(rules);
            }
            else
            {
                citateTrimmerUsingValue.Value = value;
            }
            await _saveChangesCommand.SaveAsync(cancellationToken);
            return new OperationResult() { Type = OperationResultType.Success };
        }

        public async Task<MessageByEmailDetails> UpdateAsync(Guid id, MessageByEmailData model, CancellationToken cancellationToken)
        {
            var message = await _modifyEntity.ModifyAsync(id, model, cancellationToken);
            await _saveChangesCommand.SaveAsync(cancellationToken);

            return _mapper.Map<MessageByEmailDetails>(message);
        }
    }
}
