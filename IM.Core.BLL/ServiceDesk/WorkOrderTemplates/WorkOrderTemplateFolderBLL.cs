using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Inframanager.BLL;
using InfraManager.DAL.AccessManagement;
using Microsoft.Extensions.Logging;
using Inframanager.BLL.AccessManagement;
using Inframanager;

namespace InfraManager.BLL.ServiceDesk.WorkOrderTemplates;

internal class WorkOrderTemplateFolderBLL : IWorkOrderTemplateFolderBLL, ISelfRegisteredService<IWorkOrderTemplateFolderBLL>
{
    private readonly IRepository<WorkOrderTemplateFolder> _workOrderTemplateFolderRepository;
    private readonly IRepository<WorkOrderTemplate> _workOrderTemplateRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidatePermissions<WorkOrderTemplateFolder> _validatePermissions;
    private readonly ILogger<WorkOrderTemplateFolderBLL> _logger;
    private readonly ICurrentUser _currentUser;
    public WorkOrderTemplateFolderBLL(IRepository<WorkOrderTemplateFolder> workOrderTemplateFolderRepository
                                , IRepository<WorkOrderTemplate> workOrderTemplateRepository
                                , IUnitOfWork unitOfWork
                                , IValidatePermissions<WorkOrderTemplateFolder> validatePermissions
                                , ILogger<WorkOrderTemplateFolderBLL> logger
                                , ICurrentUser currentUser)
    {
        _workOrderTemplateFolderRepository = workOrderTemplateFolderRepository;
        _workOrderTemplateRepository = workOrderTemplateRepository;
        _unitOfWork = unitOfWork;
        _validatePermissions = validatePermissions;
        _logger = logger;
        _currentUser = currentUser;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Delete, cancellationToken);

        var root = await _workOrderTemplateFolderRepository.WithMany(c=> c.Templates)
                                                           .WithMany(c=> c.SubFolder)
                                                           .FirstOrDefaultAsync(c => c.ID == id, cancellationToken)
                                                           ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.WorkOrderTemplateFolder);

        var parents = new List<WorkOrderTemplateFolder>
        {
            root
        };
        while (parents.Any())
        {
            root = parents.First();
            // Удаляю вложенные шаблоны и папки
            _workOrderTemplateFolderRepository.Delete(root);
            root.Templates.ForEach(c => _workOrderTemplateRepository.Delete(c));

            parents.AddRange(root.SubFolder);
            parents.Remove(root);
        }

        await _unitOfWork.SaveAsync(cancellationToken);
    }
}
