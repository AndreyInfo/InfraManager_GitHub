using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.FormBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.BLL.AccessManagement;
using InfraManager.BLL.FormBuilder.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.BLL.FormBuilder
{
    public class FullFormBLL : IFullFormBLL, ISelfRegisteredService<IFullFormBLL>
    {
        private Regex _numberWithCirclesRegex = new(@"\([0-9]+\)");
        private Regex _onlyNumberRegex = new(@"[0-9]+");
        private readonly IRepository<Form> _formRepository;
        private readonly IMapper _mapper;
        private readonly IReadonlyRepository<Form> _formFinder;
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IUserAccessBLL _access;
        private readonly ICurrentUser _currentUser;
        private readonly ILogger<FullFormBLL> _logger;
        private readonly IValidatePermissions<Form> _validatePermissions;
        private readonly IRepository<ServiceItem> _serviceItemsRepository;
        private readonly IRepository<ServiceAttendance> _serviceAttendancesRepository;
        private readonly IRepository<ProblemType> _problemTypesRepository;
        private readonly IRepository<WorkOrderTemplate> _workOrderTemplatesRepository;
        private readonly IRepository<ChangeRequestType> _changeRequestTypesRepository;
        private readonly IFormBLL _formBLL;
        private readonly IReadonlyRepository<MassIncidentType> _massIncidentTypeRepository;

        public FullFormBLL(IMapper mapper,
                           IUnitOfWork saveChangesCommand,
                           IRepository<Form> formRepository,
                           IReadonlyRepository<Form> formFinder, 
                           IUserAccessBLL access,
                           ICurrentUser currentUser,
                           ILogger<FullFormBLL> logger,
                           IValidatePermissions<Form> validatePermissions,
                           IRepository<ServiceItem> serviceItemsRepository,
                           IRepository<ServiceAttendance> serviceAttendancesRepository,
                           IRepository<ProblemType> problemTypesRepository,
                           IRepository<WorkOrderTemplate> workOrderTemplatesRepository,
                           IRepository<ChangeRequestType> changeRequestTypesRepository,
                           IFormBLL formBLL,
                           IReadonlyRepository<MassIncidentType> massIncidentTypeRepository)
        {
            _mapper = mapper;
            _saveChangesCommand = saveChangesCommand;
            _formRepository = formRepository;
            _formFinder = formFinder;
            _access = access;
            _currentUser = currentUser;
            _logger = logger;
            _validatePermissions = validatePermissions;
            _serviceItemsRepository = serviceItemsRepository;
            _problemTypesRepository = problemTypesRepository;
            _workOrderTemplatesRepository = workOrderTemplatesRepository;
            _serviceAttendancesRepository = serviceAttendancesRepository;
            _changeRequestTypesRepository = changeRequestTypesRepository;
            _formBLL = formBLL;
            _massIncidentTypeRepository = massIncidentTypeRepository;
        }
        
        public async Task<FormBuilderFullFormDetails> GetAsync(Guid formID,
            CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails,
                cancellationToken);

            var fullForm = await GetFullFormAsync(formID, cancellationToken);

            _logger.LogInformation($"User with ID = {_currentUser.UserId} got Full form with id = {formID}");
            
            return BuildDetails(fullForm);
        }

        public Task<FormBuilderFullFormDetails> GetAsync(ObjectClass classID, Guid objectID, CancellationToken cancellationToken = default)
        {
            return classID switch
            {
                ObjectClass.ServiceItem => GetFullFormAsync(_serviceItemsRepository, x => x.ID == objectID, cancellationToken),
                ObjectClass.ServiceAttendance => GetFullFormAsync(_serviceAttendancesRepository, x => x.ID == objectID, cancellationToken),
                ObjectClass.ProblemType => GetFullFormAsync(_problemTypesRepository, x => x.ID == objectID, cancellationToken),
                ObjectClass.WorkOrderTemplate => GetFullFormAsync(_workOrderTemplatesRepository, x => x.ID == objectID, cancellationToken),
                ObjectClass.ChangeRequestType => GetFullFormAsync(_changeRequestTypesRepository, x => x.ID == objectID, cancellationToken),
                ObjectClass.MassIncidentType => GetFullFormAsync(_massIncidentTypeRepository, x => x.IMObjID == objectID, cancellationToken),

                _ => throw new ArgumentOutOfRangeException(nameof(classID), classID, $"Unsupported object class '{classID}'."),
            };
        }

        private Task<Form> GetFullFormAsync(Guid formID, CancellationToken cancellationToken = default)
        {
            return _formRepository.DisableTrackingForQuery()
                .WithMany(x => x.FormTabs)
                .ThenWithMany(x => x.Fields.Where(x => x.GroupFieldID == null && x.ColumnFieldID == null))
                .ThenWith(x => x.Options)
                .WithMany(x => x.FormTabs)
                .ThenWithMany(x => x.Fields)
                .ThenWithMany(x => x.Grouped)
                .ThenWith(x => x.Options)
                .WithMany(x => x.FormTabs)
                .ThenWithMany(x => x.Fields)
                .ThenWithMany(x => x.Columns)
                .FirstOrDefaultAsync(x => x.ID == formID, cancellationToken);
        }

        private async Task<FormBuilderFullFormDetails> GetFullFormAsync<T>(
            IReadonlyRepository<T> repository,
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
            where T : class, IFormBuilder
        {
            var fullForm = await repository
                    .With(x => x.Form)
                        .ThenWithMany(x => x.FormTabs)
                        .ThenWithMany(x => x.Fields.Where(x => x.GroupFieldID == null && x.ColumnFieldID == null))
                        .ThenWith(x => x.Options)
                    .With(x => x.Form)
                        .ThenWithMany(x => x.FormTabs)
                        .ThenWithMany(x => x.Fields)
                        .ThenWithMany(x => x.Grouped)
                        .ThenWith(x => x.Options)
                    .With(x => x.Form)
                        .ThenWithMany(x => x.FormTabs)
                        .ThenWithMany(x => x.Fields)
                        .ThenWithMany(x => x.Columns)
                        .DisableTrackingForQuery()
                    .FirstOrDefaultAsync(predicate, cancellationToken);

            return BuildDetails(fullForm?.Form);
        }

        private FormBuilderFullFormDetails BuildDetails(Form fullForm)
        {
            var result = new FormBuilderFullFormDetails
            {
                Form = new FormBuilderFormDetails(),
                Elements = new List<Elements>()
            };

            if (fullForm == null)
            {
                return result;
            }
            
            result.Form = _mapper.Map<FormBuilderFormDetails>(fullForm);

            foreach (var el in fullForm.FormTabs)
            {
                var element = new Elements
                {
                    Tab = _mapper.Map<FormBuilderFormTabDetails>(el),
                    TabElements = _mapper.Map<List<FormBuilderFormTabFieldDetails>>(el.Fields)
                        .OrderBy(x => x.Order).ToList()
                };

                result.Elements.Add(element);
            }

            result.Elements = result.Elements.OrderBy(x => x.Tab.Order).ToList();
            
            return result;
        }

        private Form BuildForm(FormBuilderFullFormData data, Form form)
        {
            var mappedForm = _mapper.Map<Form>(form);

            _mapper.Map(data.Form, mappedForm);

            mappedForm.FormTabs = new List<FormTab>();
            foreach (var el in data.Elements)
            {
                var tab = _mapper.Map<FormTab>(el.Tab);
                tab.Fields = _mapper.Map<List<FormField>>(el.TabElements);
                mappedForm.FormTabs.Add(tab);
            }

            return mappedForm;
        }

        public async Task<Guid> CloneAsync(Guid formID, string name, string identifier, string description,
            CancellationToken cancellationToken = default)
        {
            if (!await _access.UserHasOperationAsync(_currentUser.UserId, OperationID.FormBuilder_AddAnalogy,
                    cancellationToken))
            {
                throw new AccessDeniedException($"User with ID = {_currentUser.UserId} has no access to clone Form");
            }

            var form = await GetFullFormAsync(formID, cancellationToken);

            var clonedForm = form.Clone(name, description, identifier);

            AddFullFormWithRelatedData(clonedForm);
            
            await _saveChangesCommand.SaveAsync(cancellationToken);

            _logger.LogInformation(
                $"User with ID = {_currentUser.UserId} cloned Form with id = {formID} and create new Form with ID = {clonedForm.ID}");
            
            return clonedForm.ID;
        }

        public async Task<Guid> SaveAsync(Guid id, FormBuilderFullFormData data,
            CancellationToken cancellationToken = default)
        {
            Guid currentGuid;
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Insert,
                cancellationToken);

            using (var transaction =
                   TransactionScopeCreator.Create(IsolationLevel.Serializable, TransactionScopeOption.Required))
            {

                var form = await _formRepository.DisableTrackingForQuery()
                    .FirstOrDefaultAsync(x => x.ID == id, cancellationToken);

                await _formBLL.ValidateNameAsync(data.Form.Name, form.MainID, cancellationToken);

                var builtForm = BuildForm(data, form);

                var fullForm = await _formRepository
                    .WithMany(x => x.FormTabs)
                        .ThenWithMany(x => x.Fields.Where(x => x.GroupFieldID == null && x.ColumnFieldID == null))
                        .ThenWith(x => x.Options)
                    .WithMany(x => x.FormTabs)
                        .ThenWithMany(x => x.Fields)
                        .ThenWithMany(x => x.Grouped)
                        .ThenWith(x => x.Options)
                    .WithMany(x => x.FormTabs)
                        .ThenWithMany(x => x.Fields)
                        .ThenWithMany(x => x.Columns)
                        .ThenWithMany(x => x.Options)
                    .FirstOrDefaultAsync(x => x.ID == id, cancellationToken);

                var AreFormsTheSame = fullForm.Equals(builtForm);

                currentGuid = fullForm.ID;
                if (!AreFormsTheSame)
                {
                    var maxMajorVersion = await _formFinder.MaxAsync(x => x.MainID == builtForm.MainID,
                        x => x.MajorVersion,
                        cancellationToken);
                    
                    var maxMinorVersion = await _formFinder.MaxAsync(
                        x => x.MainID == builtForm.MainID && x.MajorVersion == maxMajorVersion, x => x.MinorVersion,
                        cancellationToken);

                    var clonedForm = builtForm.Clone(builtForm.Name, builtForm.Description, builtForm.Identifier,
                        maxMajorVersion, maxMinorVersion + 1, builtForm.LastIndex, form.MainID);
                    clonedForm.UtcChanged = DateTime.UtcNow;

                    AddFullFormWithRelatedData(clonedForm);

                    currentGuid = clonedForm.ID;
                }
                else
                {
                    _mapper.Map(builtForm, fullForm);

                    fullForm.UtcChanged = DateTime.UtcNow;
                }

                await _saveChangesCommand.SaveAsync(cancellationToken, IsolationLevel.Serializable);
                transaction.Complete();
            }

            _logger.LogInformation($"User with ID = {_currentUser.UserId} changed the Form with id = {id}");

            return currentGuid;
        }

        //TODO что то поделать с тем, что EF Core не сохраняет таб при создании новой формы в элементе "группа" и "таблица"
        private void AddFullFormWithRelatedData(Form newForm)
        {
            _formRepository.Insert(newForm);

            foreach (var tab in newForm.FormTabs)
            {
                foreach (var field in tab.Fields)
                {
                    if (field.Grouped.Any())
                    {
                        foreach (var groupedField in field.Grouped)
                        {
                            groupedField.TabID = field.TabID;
                        }
                    }
                }
                
                foreach (var field in tab.Fields)
                {
                    if (field.Columns.Any())
                    {
                        foreach (var groupedField in field.Columns)
                        {
                            groupedField.TabID = field.TabID;
                        }
                    }
                }
            }
        }
        
        public async Task<string> ExportAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (!await _access.UserHasOperationAsync(_currentUser.UserId, OperationID.FormBuilder_Export,
                    cancellationToken))
            {
                _logger.LogWarning($"User with id = {_currentUser.UserId} has no access to export FormBuilder");
                throw new AccessDeniedException(
                    $"User with id = {_currentUser.UserId} has no access to export FormBuilder");
            }

            var exportForm = await GetFullFormAsync(id, cancellationToken);

            var clonedForm = exportForm.Clone(exportForm.Name, exportForm.Description, exportForm.Identifier,
                lastIndex: exportForm.LastIndex);

            var formForExport = BuildDetails(clonedForm);
            return JsonConvert.SerializeObject(formForExport);
        }

        public async Task ImportAsync(string formBuilderJson, CancellationToken cancellationToken = default)
        {
            if (!await _access.UserHasOperationAsync(_currentUser.UserId, OperationID.FormBuilder_Import,
                    cancellationToken))
            {
                _logger.LogWarning($"User with id = {_currentUser.UserId} has no access to import FormBuilder");
                throw new AccessDeniedException(
                    $"User with id = {_currentUser.UserId} has no access to import FormBuilder");
            }

            var form = JsonConvert.DeserializeObject<FormBuilderFullFormData>(formBuilderJson);

            var tempForm = new Form();
            var clonedForm = BuildForm(form, tempForm);

            using (var transaction =
                   TransactionScopeCreator.Create(IsolationLevel.Serializable, TransactionScopeOption.Required))
            {
                clonedForm.Name = await GetUniqueNameAsync(clonedForm.Name, cancellationToken);
                
                AddFullFormWithRelatedData(clonedForm);
            
                await _saveChangesCommand.SaveAsync(cancellationToken, IsolationLevel.Serializable);
                transaction.Complete();
            }
        }

        private async Task<string> GetUniqueNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var formWithThisName =
                await _formRepository.FirstOrDefaultAsync(x => x.Name.Equals(name), cancellationToken);

            if (formWithThisName == null)
            {
                return name;
            }
            
            var numberWithCirclet = _numberWithCirclesRegex.Match(name).ToString();
            
            if (string.IsNullOrEmpty(numberWithCirclet))
            {
                return await GetUniqueNameAsync($"{name} (1)", cancellationToken);
            }

            var number = _onlyNumberRegex.Match(numberWithCirclet);

            if (!int.TryParse(number.ToString(), out var intNumber))
            {
                throw new NotSupportedException($"Cant get name version for Form in import, number = {number}");
            }

            intNumber += 1;

            return await GetUniqueNameAsync(_numberWithCirclesRegex.Replace(name, $"({intNumber})"), cancellationToken);
        }
    }
}
