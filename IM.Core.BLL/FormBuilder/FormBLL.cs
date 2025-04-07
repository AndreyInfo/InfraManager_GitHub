using AutoMapper;
using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.FormBuilder;
using InfraManager.DAL.FormBuilder.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Inframanager;
using Inframanager.BLL;
using InfraManager.BLL.AccessManagement;
using InfraManager.BLL.FormBuilder.Contracts;
using InfraManager.BLL.FormBuilder.Forms;
using InfraManager.BLL.Localization;
using InfraManager.ResourcesArea;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.FormBuilder
{
    public class FormBLL : IFormBLL, ISelfRegisteredService<IFormBLL>
    {
        private readonly IFinder<Form> _formFinder;
        private readonly IRepository<Form> _repository;
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IMapper _mapper;
        private readonly IClassIM _classIM;
        private readonly IGuidePaggingFacade<Form, FormBuilderForTable> _paggingFacade;
        private readonly IInsertEntityBLL<Form, FormBuilderFormData> _insertBLL;
        private readonly IModifyEntityBLL<Guid, Form, FormBuilderFormData, FormBuilderFormDetails> _modifyEntityBLL;
        private readonly ILogger<FormBLL> _logger;
        private readonly IValidatePermissions<Form> _validatePermissions;
        private readonly ICurrentUser _currentUser;
        private readonly IUserAccessBLL _access;
        private readonly IPagingQueryCreator _pagingQueryCreator;
        private readonly ILocalizeText _localize;

        public FormBLL(IFinder<Form> formFinder,
                       IRepository<Form> repository,
                       IUnitOfWork saveChangesCommand,
                       IMapper mapper,
                       IClassIM classIM,
                       IGuidePaggingFacade<Form, FormBuilderForTable> paggingFacade,
                       IInsertEntityBLL<Form, FormBuilderFormData> insertBLL,
                       IModifyEntityBLL<Guid, Form, FormBuilderFormData, FormBuilderFormDetails> modifyEntityBLL,
                       ILogger<FormBLL> logger,
                       IValidatePermissions<Form> validatePermissions,
                       ICurrentUser currentUser,
                       IUserAccessBLL access,
                       IPagingQueryCreator pagingQueryCreator,
                       ILocalizeText localize)
        {
            _formFinder = formFinder;
            _repository = repository;
            _saveChangesCommand = saveChangesCommand;
            _mapper = mapper;
            _classIM = classIM;
            _paggingFacade = paggingFacade;
            _insertBLL = insertBLL;
            _modifyEntityBLL = modifyEntityBLL;
            _logger = logger;
            _validatePermissions = validatePermissions;
            _currentUser = currentUser;
            _access = access;
            _pagingQueryCreator = pagingQueryCreator;
            _localize = localize;
        }


        public async Task<FormBuilderFormDetails[]> ListAsync(FormBuilderFilter filter,
            CancellationToken cancellationToken = default)
        {
            var query = _repository.Query();

            if (filter.ClassID.HasValue)
            {
                query = query.Where(i => i.ClassID == filter.ClassID);
            }

            if (filter.Statuses?.Length > 0)
            {
                query = query.Where(i => filter.Statuses.Contains(i.Status));
            }

            if (filter.FormID.HasValue)
            {
                query = query.Where(i => i.ID == filter.FormID);
            }

            if (filter.MainID.HasValue)
            {
                query = query.Where(i => i.MainID == filter.MainID);

                if (string.IsNullOrEmpty(filter.ViewName))
                {
                    var pagging = _pagingQueryCreator.Create(query.OrderByDescending(x => x.MajorVersion)
                        .ThenByDescending(x => x.MinorVersion));

                    var result =
                        await pagging.PageAsync(filter.StartRecordIndex, filter.CountRecords, cancellationToken);
                    return _mapper.Map<FormBuilderFormDetails[]>(result);
                }
            }

            if (filter.OnlyHighLevel)
            {
                var result = (await _repository.ToArrayAsync(cancellationToken)).GroupBy(x => x.MainID).Select(x =>
                    x.OrderByDescending(z => z.MajorVersion).ThenByDescending(z => z.MinorVersion).First()).ToArray();

                return _mapper.Map<FormBuilderFormDetails[]>(result);
            }

            var forms = await _paggingFacade.GetPaggingAsync(filter,
                query,
                x => x.Name.ToLower().Contains(filter.SearchString.ToLower()),
                cancellationToken);

            var mappedForms = _mapper.Map<FormBuilderFormDetails[]>(forms);

            foreach(var el in mappedForms)
            {
                el.Class = await _classIM.GetClassNameAsync(el.ClassID, cancellationToken);
            }

            return mappedForms;
        }

        public async Task<FormBuilderFormDetails> GetAsync(Guid formID, CancellationToken cancellationToken = default)
        {
            var form = await _repository.FirstOrDefaultAsync(x => x.ID == formID, cancellationToken)
                ?? throw new ObjectNotFoundException($"Form not found with id = {formID}");

            return _mapper.Map<FormBuilderFormDetails>(form);
        }

        public async Task<FormBuilderFormDetails> AddAsync(FormBuilderFormData data,
            CancellationToken cancellationToken)
        {
            Form entity;
            using (var transaction =
                   TransactionScopeCreator.Create(IsolationLevel.Serializable, TransactionScopeOption.Required))
            {
                await ValidateNameAsync(data.Name, cancellationToken: cancellationToken);

                entity = await _insertBLL.CreateAsync(data, cancellationToken);
                await _saveChangesCommand.SaveAsync(cancellationToken, IsolationLevel.Serializable);

                transaction.Complete();
            }

            return _mapper.Map<FormBuilderFormDetails>(entity);
        }

        public async Task<FormBuilderFormDetails> UpdateAsync(Guid id, FormBuilderFormData data,
            CancellationToken cancellationToken)
        {
            Form entity;
            using (var transaction =
                   TransactionScopeCreator.Create(IsolationLevel.Serializable, TransactionScopeOption.Required))
            {
                entity = await _modifyEntityBLL.ModifyAsync(id, data, cancellationToken);
                await ValidateNameAsync(data.Name, entity.MainID, cancellationToken);

                entity.UtcChanged = DateTime.UtcNow;

                await _saveChangesCommand.SaveAsync(cancellationToken, IsolationLevel.Serializable);
                transaction.Complete();
            }

            return _mapper.Map<FormBuilderFormDetails>(entity);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            if (!await _validatePermissions.UserHasPermissionAsync(_currentUser.UserId, ObjectAction.Delete,
                    cancellationToken))
            {
                throw new AccessDeniedException(
                    $"User with id = {_currentUser.UserId} cant delete Form Builder Form");
            }

            using (var transaction =
                   TransactionScopeCreator.Create(IsolationLevel.RepeatableRead, TransactionScopeOption.Required))
            {

                var entity = await _repository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken)
                             ?? throw new ObjectNotFoundException<Guid>(id, "Form Builder Form was not found");

                if (entity.Status == FormBuilderFormStatus.Published ||
                    entity.Status == FormBuilderFormStatus.Blocked ||
                    entity.Status == FormBuilderFormStatus.Overriden)
                {
                    _logger.LogWarning(
                        "User with id = {UserID} tried delete already published or blocked Form with id = {ID}",
                        _currentUser.UserId, id);

                    throw new InvalidObjectException(await _localize.LocalizeAsync(
                        nameof(Resources.FormBuilder_CantDeleteForm), cancellationToken));
                }

                _repository.Delete(entity);
                await _saveChangesCommand.SaveAsync(cancellationToken, IsolationLevel.RepeatableRead);

                _logger.LogTrace(
                    "User with id = {UserID} successfully deleted Form Builder Form with id = {ID}",
                    _currentUser.UserId, id);

                transaction.Complete();
            }
        }


        public async Task PublishAsync(Guid formID, CancellationToken cancellationToken = default)
        {
            if (!await _access.UserHasOperationAsync(_currentUser.UserId, OperationID.FormBuilder_Publish,
                    cancellationToken))
            {
                throw new AccessDeniedException(
                    $"User with id = {_currentUser.UserId} cant publish Form with id = {formID}");
            }

            using (var transaction =
                   TransactionScopeCreator.Create(IsolationLevel.RepeatableRead, TransactionScopeOption.Required))
            {
                var form = await _formFinder.FindAsync(formID, cancellationToken);

                if (form.Status == FormBuilderFormStatus.Published)
                {
                    throw new InvalidObjectException(await _localize.LocalizeAsync(
                        nameof(Resources.FormBuilder_FormAlreadyPublished), cancellationToken));
                }

                var majorVersion = form.MajorVersion;

                var publishedForm = await _repository.FirstOrDefaultAsync(
                    x => x.Status == FormBuilderFormStatus.Published && x.MainID == form.MainID, cancellationToken);

                if (publishedForm != null)
                {
                    publishedForm.Status = FormBuilderFormStatus.Overriden;
                    majorVersion = publishedForm.MajorVersion;
                }

                form.Status = FormBuilderFormStatus.Published;
                form.MajorVersion = majorVersion + 1;
                form.MinorVersion = 0;

                await _saveChangesCommand.SaveAsync(cancellationToken, IsolationLevel.RepeatableRead);
                transaction.Complete();
            }
        }

        public FormBuilderClassType[] GetAvailableTypes()
        {
            ObjectClass[] objectClasses = {
                ObjectClass.Problem,
                ObjectClass.ChangeRequest,
                ObjectClass.LifeCycle,
                ObjectClass.SLA,
                ObjectClass.ServiceAttendance,
                ObjectClass.ServiceItem,
                ObjectClass.WorkOrderTemplate,
                ObjectClass.MassIncident,
                ObjectClass.ProductCatalogType
            };

            var fbTypes = new List<FormBuilderClassType>();

            foreach (var el in objectClasses)
            {
                fbTypes.Add(new FormBuilderClassType { Name = _classIM.GetClassName(el), ID = (int)el });
            }

            return fbTypes.ToArray();
        }

        public async Task ValidateNameAsync(string name, Guid? mainID = null,
            CancellationToken cancellationToken = default)
        {
            var query = _repository.Query().Where(x => x.Name.ToLower().Equals(name.ToLower()));

            if (mainID.HasValue)
            {
                query = query.Where(x => x.MainID != mainID);
            }

            var existingForm = await query.ExecuteAsync(cancellationToken);

            if (existingForm.Length != 0)
            {
                throw new InvalidObjectException(string.Format(await _localize.LocalizeAsync(
                    nameof(Resources.FormBuilder_NameExists), cancellationToken), name));
            }
        }
    }
}
