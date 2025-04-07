using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.Asset;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.BLL.ServiceDesk.ServiceUnits
{
    internal class ServiceUnitPerformersBLL : IServiceUnitPerformersBLL, ISelfRegisteredService<IServiceUnitPerformersBLL>
    {
        private readonly IMapper _mapper;
        private readonly IReadonlyRepository<Group> _repositoryGroups;
        private readonly IReadonlyRepository<User> _repositoryUsers;
        private readonly IRepository<OrganizationItemGroup> _repositoryOrganizationItemGroup;
        private readonly IUnitOfWork _saveChangesCommand;
        public ServiceUnitPerformersBLL(IMapper mapper
                              , IReadonlyRepository<Group> repositoryGroups
                              , IReadonlyRepository<User> repositoryUsers
                              , IRepository<OrganizationItemGroup> repositoryOrganizationItemGroup
                              , IUnitOfWork saveChangesCommand)
        {
            _mapper = mapper;
            _repositoryGroups = repositoryGroups;
            _repositoryUsers = repositoryUsers;
            _repositoryOrganizationItemGroup = repositoryOrganizationItemGroup;
            _saveChangesCommand = saveChangesCommand;
        }
        
        public async Task<PerformerDetails[]> GetPerformersByServiceUnitIdAsync(Guid serviceUnitId, CancellationToken cancellationToken)
        {
            var result = await GetPerformerOfGroupAsync(serviceUnitId, cancellationToken);
            result = result.Union(await GetPerformerOfUsersAsync(serviceUnitId, cancellationToken)).ToArray();
            result.ForEach(c => c.ServiceUnitID = serviceUnitId);
            return result;

        }
        private async Task<PerformerDetails[]> GetPerformerOfGroupAsync(Guid serviceUnitID, CancellationToken cancellationToken)
        {
            var organizationItemGroups = await _repositoryOrganizationItemGroup.ToArrayAsync(c => c.ID == serviceUnitID && c.ItemClassID == ObjectClass.Group
                                                                            , cancellationToken);

            var groups = await _repositoryGroups.With(c => c.ResponsibleUser)
                .ToArrayAsync(c => organizationItemGroups.Select(v => v.ItemID).Contains(c.IMObjID)
                                                        , cancellationToken);
            
            return _mapper.Map<PerformerDetails[]>(groups);
        }
        private async Task<PerformerDetails[]> GetPerformerOfUsersAsync(Guid serviceUnitID, CancellationToken cancellationToken)
        {
            var organizationItemGroups = await _repositoryOrganizationItemGroup.ToArrayAsync(c => c.ID == serviceUnitID && c.ItemClassID == ObjectClass.User
                                                                            , cancellationToken);
            var users = await _repositoryUsers.ToArrayAsync(c => organizationItemGroups.Select(v => v.ItemID).Contains(c.IMObjID)
                                                        , cancellationToken);

            var result = _mapper.Map<PerformerDetails[]>(users);

            return result;
        }

     
        public async Task AddPerformersAsync(PerformerServiceUnitDetails model, CancellationToken cancellationToken)
        {
            if (await _repositoryOrganizationItemGroup.AnyAsync(c => c.ID == model.ServiceUnitID && c.ItemID == model.PerformerID, cancellationToken))
                throw new InvalidObjectException($"Уже существует исполнитель с ID: {model.PerformerID}");// TODO локализация

            var saveModel = _mapper.Map<OrganizationItemGroup>(model);
            _repositoryOrganizationItemGroup.Insert(saveModel);
            await _saveChangesCommand.SaveAsync(cancellationToken);
        }


        public async Task DeletePerformersAsync(PerformerServiceUnitDetails[] models, CancellationToken cancellationToken)
        {
            var deleteModels = _mapper.Map<OrganizationItemGroup[]>(models);
            
            foreach (var item in deleteModels)
            {
                _repositoryOrganizationItemGroup.Delete(item);
            }
             
            await _saveChangesCommand.SaveAsync(cancellationToken);
        }
    }
}
