using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.OrganizationStructure;
using System.Threading;
using InfraManager.BLL.ProductCatalogue;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.DAL.ServiceCatalogue.SLA;
using InfraManager.BLL.Localization;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices
{
    internal class PortfolioServiceBLL : IPortfolioServiceBLL, ISelfRegisteredService<IPortfolioServiceBLL>
    {
        private readonly IReadonlyRepository<Service> _serviceListQuery;
        private readonly IReadonlyRepository<ServiceCategory> _serviceCategoryListQuery;
        private readonly IReadonlyRepository<ServiceItem> _serviceItemListQuery;
        private readonly IReadonlyRepository<ServiceAttendance> _serviceAttendanceListQuery;
        private readonly IRepository<ServiceReference> _serviceReferenceRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _saveChangesCommand;

        private readonly IReadonlyRepository<User> _usersQuery;
        private readonly IReadonlyRepository<Group> _groupQueueQuery;
        private readonly IReadonlyRepository<Organization> _organizationQuery;
        private readonly IReadonlyRepository<Subdivision> _subdivisionQuery;
        private readonly IReadOnlyList<ObjectClass> _customerClassId;
        private readonly IPortfolioServiceGetInfrastructureQuery _infrastructureGetQuery;
        private readonly IServiceMapper<ObjectClass, IPortfolioServiceTreeQuery> _queryMapper;
        private readonly ILocalizeText _localizeText;


        public PortfolioServiceBLL(IReadonlyRepository<ServiceCategory> serviceCategoryListQuery,
                                   IReadonlyRepository<Service> serviceListQuery,
                                   IReadonlyRepository<ServiceItem> serviceItemListQuery,
                                   IReadonlyRepository<ServiceAttendance> serviceAttendanceListQuery,
                                   IReadonlyRepository<User> usersQuery,
                                   IReadonlyRepository<Group> groupQueueQuery,
                                   IReadonlyRepository<Organization> organizationQuery,
                                   IReadonlyRepository<Subdivision> subdivisionQuery,
                                   IMapper mapper,
                                   IRepository<ServiceReference> serviceServiceRepository,
                                   IUnitOfWork saveChangesCommand,
                                   IPortfolioServiceGetInfrastructureQuery infrastructureGetQuery,
                                   IServiceMapper<ObjectClass, IPortfolioServiceTreeQuery> queryMapper,
                                   ILocalizeText localizeText)
        {
            _mapper = mapper;
            _serviceListQuery = serviceListQuery;
            _serviceCategoryListQuery = serviceCategoryListQuery;
            _serviceItemListQuery = serviceItemListQuery;
            _serviceAttendanceListQuery = serviceAttendanceListQuery;
            _usersQuery = usersQuery;
            _groupQueueQuery = groupQueueQuery;
            _organizationQuery = organizationQuery;
            _subdivisionQuery = subdivisionQuery;
            _customerClassId = new ObjectClass[4]
            {
                ObjectClass.Service,
                ObjectClass.ServiceItem,
                ObjectClass.ServiceAttendance,
                ObjectClass.ServiceCategory,
            };
            _serviceReferenceRepository = serviceServiceRepository;
            _saveChangesCommand = saveChangesCommand;
            _infrastructureGetQuery = infrastructureGetQuery;
            _queryMapper = queryMapper;
            _localizeText = localizeText;
        }

        public async Task<PortfolioServicesItem[]> GetTreeAsync(PortfolioServiceFilter filter, CancellationToken cancellationToken = default)
        {
            if (!filter.ClassID.HasValue)
            {
                return new PortfolioServicesItem[]
                {
                    new()
                    {
                        ClassId = ObjectClass.ServiceCatalogue,
                        ParentId = null,
                        Name = await _localizeText.LocalizeAsync(nameof(Resources.PortfolioServiceRootNode), cancellationToken),
                    }
                };
            }

            var query = _queryMapper.Map(filter.ClassID.Value);

            return await query.ExecuteAsync(filter.SLAID, filter.ParentID.GetValueOrDefault(), cancellationToken);
        }



        public PortfolioServicesItem[] GetPath(ObjectClass classID, Guid id)
        {
            if (!_customerClassId.Contains(classID))
            {
                throw new InvalidObjectException("Invalid Class ID");
            }

            var result = new List<PortfolioServicesItem>();
            bool alreadyRead = false;
            if (classID == ObjectClass.ServiceItem)
            {
                var isExists = _serviceItemListQuery.Query().Any(c => c.ID == id);
                if (!isExists)
                    throw new ObjectNotFoundException<Guid>(id, ObjectClass.ServiceItem);

                var item = _serviceItemListQuery.Query().FirstOrDefault(c => c.ID == id);

                if (item != null)
                {
                    id = item.ServiceID ?? Guid.Empty;
                    alreadyRead = true;
                    result.Add(_mapper.Map<PortfolioServicesItem>(item));
                }
            }
            else if (classID == ObjectClass.ServiceAttendance)
            {
                var isExists = _serviceAttendanceListQuery.Query().Any(c => c.ID == id);
                if (!isExists)
                    throw new ObjectNotFoundException<Guid>(id, ObjectClass.ServiceAttendance);

                var attendance = _serviceAttendanceListQuery.Query().FirstOrDefault(c => c.ID == id);

                if (attendance != null)
                {
                    id = attendance.ServiceID.Value;
                    alreadyRead = true;
                    result.Add(_mapper.Map<PortfolioServicesItem>(attendance));
                }
            }

            if (classID == ObjectClass.Service || alreadyRead)
            {
                var isExists = _serviceListQuery.Query().Any(c => c.ID == id);
                if (!isExists)
                    throw new ObjectNotFoundException<Guid>(id, ObjectClass.Service);

                var service = _serviceListQuery.Query().FirstOrDefault(c => c.ID == id);

                if (service != null)
                {
                    id = service.CategoryID ?? Guid.Empty;
                    alreadyRead = true;
                    result.Add(_mapper.Map<PortfolioServicesItem>(service));
                }
            }

            if (classID == ObjectClass.ServiceCategory || alreadyRead)
            {
                var isExists = _serviceCategoryListQuery.Query().Any(c => c.ID == id);
                if (!isExists)
                    throw new ObjectNotFoundException<Guid>(id, ObjectClass.ServiceCategory);

                var category = _serviceCategoryListQuery.Query().FirstOrDefault(c => c.ID == id);
                result.Add(_mapper.Map<PortfolioServicesItem>(category));
                alreadyRead = true;
            }

            result.Add(new PortfolioServicesItem()
            {
                ClassId = ObjectClass.ServiceCatalogue,
                ParentId = null,
                Name = _localizeText.Localize(nameof(Resources.PortfolioServiceRootNode)),
            });

            return result.ToArray();
        }

        #region Customer
        public ServiceCustomerDetails GetCustomer(ObjectClass classId, Guid id)
        {
            ServiceCustomerDetails result = null;
            if (classId == ObjectClass.User)
            {
                var user = _usersQuery.Query().FirstOrDefault(c => c.IMObjID == id)
                    ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.User);

                result = _mapper.Map<ServiceCustomerDetails>(user);
            }
            else if (classId == ObjectClass.Organizaton)
            {
                var organization = _organizationQuery.Query().FirstOrDefault(c => c.ID == id)
                    ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.Organizaton);

                result = _mapper.Map<ServiceCustomerDetails>(organization);
            }
            else if (classId == ObjectClass.Group)
            {
                var queue = _groupQueueQuery.Query().FirstOrDefault(c => c.IMObjID == id)
                    ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.Group);

                result = _mapper.Map<ServiceCustomerDetails>(queue);
            }
            else if (classId == ObjectClass.Division)
            {

                var subdivision = _subdivisionQuery.Query().FirstOrDefault(c => c.ID == id)
                    ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.Division);

                result = _mapper.Map<ServiceCustomerDetails>(subdivision);
            }

            return result;
        }


        public ServiceCustomerDetails[] GetNotCustomer(ObjectClass[] classIDs, Guid[] ids, string search)
        {
            if (ids is null)
                ids = Array.Empty<Guid>();
            var result = new List<ServiceCustomerDetails>();
            if (classIDs.Contains(ObjectClass.User))
            {
                _usersQuery.With(c => c.Position);
                var query = _usersQuery.Query();
                query = query.Where(c => !ids.Contains(c.IMObjID));
                if (!string.IsNullOrEmpty(search))
                    query = query.Where(c => c.Name.Contains(search));

                var users = query.ToArray();
                result.AddRange(_mapper.Map<List<ServiceCustomerDetails>>(users));
            }

            if (classIDs.Contains(ObjectClass.Organizaton))
            {
                var query = _organizationQuery.Query().Where(c => !ids.Contains(c.ID));
                if (!string.IsNullOrEmpty(search))
                    query = query.Where(c => c.Name.Contains(search));

                var organizations = query.ToArray();
                result.AddRange(_mapper.Map<List<ServiceCustomerDetails>>(organizations));
            }


            if (classIDs.Contains(ObjectClass.Group))
            {
                var query = _groupQueueQuery.Query().Where(c => !ids.Contains(c.IMObjID));
                if (!string.IsNullOrEmpty(search))
                    query = query.Where(c => c.Name.Contains(search));

                var queues = query.ToArray();
                result.AddRange(_mapper.Map<List<ServiceCustomerDetails>>(queues));
            }

            if (classIDs.Contains(ObjectClass.Division))
            {
                var query = _subdivisionQuery.Query().Where(c => !ids.Contains(c.ID));
                if (!string.IsNullOrEmpty(search))
                    query = query.Where(c => c.Name.Contains(search));
                var subdivisions = query.ToArray();
                result.AddRange(_mapper.Map<List<ServiceCustomerDetails>>(subdivisions));
            }

            return result.ToArray();
        }
        #endregion


        public async Task<Guid> AddInfrastructureAsync(ServiceReferenceModel model, CancellationToken cancellationToken = default)
        {
            var existingEntity = await _serviceReferenceRepository.FirstOrDefaultAsync(x => x.ServiceID == model.ServiceID && x.ObjectID == model.ObjectID, cancellationToken);

            if (existingEntity != null)
            {
                throw new InvalidObjectException("Инфраструктура уже существует");
            }

            var newEntity = _mapper.Map<ServiceReference>(model);

            _serviceReferenceRepository.Insert(newEntity);
            await _saveChangesCommand.SaveAsync(cancellationToken);

            return newEntity.ID;
        }

        public async Task<PortfolioServiceInfrastructureItem[]> GetInfrastructureAsync(Guid serviceID, CancellationToken cancellationToken = default)
        {
            return await _infrastructureGetQuery.ExecuteAsync(serviceID, cancellationToken);
        }
    }
}
