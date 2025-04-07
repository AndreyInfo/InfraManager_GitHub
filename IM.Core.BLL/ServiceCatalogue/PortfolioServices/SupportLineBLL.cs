using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceCatalogue.SLA;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices
{
    internal class SupportLineBLL : ISupportLineBLL, ISelfRegisteredService<ISupportLineBLL>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<SupportLineResponsible> _repository;
        private readonly ISupportLineResponsibleQuery _supportLineResponsibleQuery;
        private readonly ISupportBLL<User> _finderUser;
        private readonly ISupportBLL<Group> _finderGroup;
        private readonly ISupportBLL<ServiceUnit> _serviceUnit;
        private readonly ISupportBLL<Subdivision> _finderSubdivision;
        private readonly ISupportBLL<Organization> _finderOrganization;
        private readonly IUnitOfWork _unitOfWork;
        public SupportLineBLL(IMapper mapper
                              , IRepository<SupportLineResponsible> repository
                              , ISupportLineResponsibleQuery supportLineResponsibleQuery
                              , ISupportBLL<User> finderUser
                              , ISupportBLL<Group> finderGroup
                              , ISupportBLL<ServiceUnit> serviceUnit
                              , IUnitOfWork unitOfWork
                              , ISupportBLL<Subdivision> finderSubdivision
                              , ISupportBLL<Organization> finderOrganization)
        {
            _mapper = mapper;
            _repository = repository;
            _supportLineResponsibleQuery = supportLineResponsibleQuery;
            _finderUser = finderUser;
            _finderGroup = finderGroup;
            _serviceUnit = serviceUnit;
            _unitOfWork = unitOfWork;
            _finderSubdivision = finderSubdivision;
            _finderOrganization = finderOrganization;
        }

        public async Task CopySupportLineFromServiceAsync(Guid objectID, Guid serviceID, ObjectClass classID, CancellationToken cancellationToken)
        {
            await DeleteAllAsync(objectID, ObjectClass.ServiceItem, cancellationToken);

            var supportLineService = await GetResponsibleObjectLineAsync(serviceID, ObjectClass.Service, cancellationToken);
            var supportLineServiceItem = (SupportLineResponsibleDetails[])supportLineService.Clone();
            supportLineServiceItem.ForEach(c =>
            {
                c.ObjectID = objectID;
                c.ObjectClassID = classID;
            });
            await SaveAsync(supportLineService, objectID, classID, cancellationToken);
        }

        private async Task DeleteAllAsync(Guid objecId, ObjectClass objectClassId, CancellationToken cancellationToken)
        {
            var existsModels = await _repository.ToArrayAsync(c => c.ObjectClassID == objectClassId && c.ObjectID == objecId, cancellationToken);
            existsModels.ForEach(c => _repository.Delete(c));
        }
        
        public async Task<SupportLineResponsibleDetails[]> GetResponsibleObjectLineAsync(Guid objectID, ObjectClass classID, CancellationToken cancellationToken )
        {
            var result = new List<SupportLineResponsibleDetails>();
            var supportObjectLines =
                await _supportLineResponsibleQuery.ExecuteAsync(objectID, classID, cancellationToken);

            foreach (var support in supportObjectLines)
            {
                var responsibleObject = await DefiningSupportEntityAsync(support, cancellationToken);
                if (responsibleObject is null)
                    continue;

                responsibleObject.Line = support.LineNumber;
                responsibleObject.ObjectID = objectID;
                responsibleObject.ObjectClassID = classID;

                result.Add(responsibleObject);
            }

            return result.ToArray();
        }

        private async Task<SupportLineResponsibleDetails> DefiningSupportEntityAsync(SupportLineResponsible model, CancellationToken cancellationToken)
        => model.OrganizationItemClassID switch
            {
                ObjectClass.User => await _finderUser.GetSupportByIdAsync(c => c.IMObjID == model.OrganizationItemID,
                    cancellationToken),
                ObjectClass.Group => await _finderGroup.GetSupportByIdAsync(c => c.IMObjID == model.OrganizationItemID,
                    cancellationToken),
                ObjectClass.Division => await _finderSubdivision.GetSupportByIdAsync(
                    c => c.ID == model.OrganizationItemID, cancellationToken),
                ObjectClass.ServiceUnit => await _serviceUnit.GetSupportByIdAsync(c => c.ID == model.OrganizationItemID,
                    cancellationToken),
                ObjectClass.Organizaton => await _finderOrganization.GetSupportByIdAsync(
                    c => c.ID == model.OrganizationItemID, cancellationToken),
                _ => throw new Exception(
                    $"не обрабатываются бизнес логикой сущности с classId{model.OrganizationItemClassID}")
            };

        public async Task SaveAsync(IEnumerable<SupportLineResponsibleDetails> models, Guid objecID, ObjectClass objectClassID, CancellationToken cancellationToken)
        {
            if (models is null)
                return;

            var saveModels = _mapper.Map<SupportLineResponsible[]>(models);
            var existsModels = await _repository.ToArrayAsync(c => c.ObjectClassID == objectClassID && c.ObjectID == objecID, cancellationToken);
            
            var notChange = saveModels.Intersect(existsModels).ToArray();

            var removingModels = existsModels.Except(notChange).ToArray();
            removingModels.ForEach(c => _repository.Delete(c));

            var addingModels = saveModels.Except(notChange).ToArray();
            addingModels.ForEach(c => _repository.Insert(c));
            await _unitOfWork.SaveAsync(cancellationToken);
        }

    }

    
}
