using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.SoftwareType
{
    public class SoftwareTypeBLL : ISoftwareTypeBLL, ISelfRegisteredService<ISoftwareTypeBLL>
    {

        private readonly IRepository<DAL.Software.SoftwareType> _repository;
        private readonly IFinder<DAL.Software.SoftwareType> _finder;
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IValidatePermissions<DAL.Software.SoftwareType> _validatePermissions;
        private readonly ILogger<SoftwareTypeBLL> _logger;
        private readonly ICurrentUser _currentUser;

        public SoftwareTypeBLL(
            IFinder<DAL.Software.SoftwareType> finder,
            IRepository<DAL.Software.SoftwareType> repository,
            IUnitOfWork saveChangesCommand,
            IValidatePermissions<DAL.Software.SoftwareType> validatePermissions,
            ILogger<SoftwareTypeBLL> logger,
            ICurrentUser currentUser)
        {
            _repository = repository;
            _finder = finder;
            _saveChangesCommand = saveChangesCommand;
            _validatePermissions = validatePermissions;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<DAL.Software.SoftwareType> GetSoftwareType(Guid softwareTypeID, CancellationToken cancellationToken = default) //TODO CHANGE ENtity TO DTO
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails,cancellationToken);

            var result = await _finder.FindAsync(new object[] { softwareTypeID }, cancellationToken)
                ?? throw new ObjectNotFoundException<Guid>(softwareTypeID, ObjectClass.SoftwareType);

            return result;
        }

        //TODO разделить на два метода(обновление и добавление
        public async Task<bool> SaveAsync(DAL.Software.SoftwareType model, CancellationToken cancellationToken)
        {
            if (model.ID == Guid.Empty)
            {
                await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Insert, cancellationToken);

                model.ID = Guid.NewGuid();
                model.Note = model.Note ?? string.Empty;

                _repository.Insert(model);
            }
            else
            {
                await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Update, cancellationToken);

                var updated = await _finder.FindAsync(new object[] { model.ID }, cancellationToken)
                    ?? throw new ObjectNotFoundException<Guid>(model.ID, ObjectClass.SoftwareType);

                updated.Name = model.Name ?? updated.Name;
                updated.Note = model.Note ?? updated.Note;
                updated.ParentId = model.ParentId ?? updated.ParentId;
            }
            await _saveChangesCommand.SaveAsync(cancellationToken);

            return true;
        }

        public async Task RemoveAsync(DAL.Software.SoftwareType model, CancellationToken cancellationToken)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Delete, cancellationToken);

            var deleted = await _finder.FindAsync(model.ID, cancellationToken);
            _repository.Delete(deleted);

            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

    }
}
