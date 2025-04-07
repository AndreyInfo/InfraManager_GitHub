using AutoMapper;
using InfraManager.DAL.OrganizationStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.DAL;

namespace InfraManager.BLL.OrganizationStructure;

internal class OrganizationStructureBLL : IOrganizationStructureBLL,
    ISelfRegisteredService<IOrganizationStructureBLL>
{
    private readonly IMapper _mapper;
    private readonly Owners.IOwnerBLL _ownerBll;
    private readonly IOrganizationBLL _organizationBll;
    private readonly ISubdivisionBLL _subdivisionBll;
    private readonly IReadonlyRepository<User> _usersRepository;
    private readonly IReadonlyRepository<Subdivision> _subdivisionsRepository;

    public OrganizationStructureBLL(
        IMapper mapper,
        Owners.IOwnerBLL ownerBll, 
        IOrganizationBLL organizationBll, 
        ISubdivisionBLL subdivisionBll,
        IReadonlyRepository<User> usersRepository,
        IReadonlyRepository<Subdivision> subdivisionsRepository)
    {
        _mapper = mapper;
        _ownerBll = ownerBll;
        _organizationBll = organizationBll;
        _subdivisionBll = subdivisionBll;
        _usersRepository = usersRepository;
        _subdivisionsRepository = subdivisionsRepository;
    }

    public async Task<OrganizationStructureNodeModelDetails[]> GetNodesAsync(
        OrganizationStructureNodeRequestModelDetails nodeRequest,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteGetNodesAsync(nodeRequest, cancellationToken);
    }
    
    
    /// TODO: заменить switch на IServiceMapper
    public async Task<OrganizationStructureNodeModelDetails[]> GetPathToNodeAsync(OrganizationStructureNodeRequestModelDetails nodeRequest, CancellationToken cancellationToken)
    {
        return nodeRequest.ClassID switch
        {
            ObjectClass.Unknown => new[] {await GetOwnerAsync(cancellationToken)},
            ObjectClass.Owner => new[] {await GetOwnerAsync(cancellationToken)},
            ObjectClass.Organizaton => new[] {await GetOrganizationPathAsync(nodeRequest.ObjectID.Value, cancellationToken)},
            ObjectClass.Division => await GetSubdivisionPathAsync(nodeRequest.ObjectID.Value, cancellationToken),
            _ => throw new Exception(
                $"Объект с classID = {nodeRequest.ClassID} не используется в дереве местоположений")
        };
    }

    private async Task<OrganizationStructureNodeModelDetails> GetOrganizationPathAsync(Guid organizationID,CancellationToken cancellationToken)
    {
        var organization = await _organizationBll.GetAsync(organizationID, cancellationToken);

        var node = _mapper.Map<OrganizationStructureNodeModelDetails>(organization);
        return node;
    }

    private async Task<OrganizationStructureNodeModelDetails[]> GetSubdivisionPathAsync(Guid subdivisionID, CancellationToken cancellationToken)
    {
        var subdivisions = await _subdivisionBll.GetPathToSubdivisionAsync(subdivisionID, cancellationToken);
        
        var organization = await _organizationBll.GetAsync(
            subdivisions.FirstOrDefault(x=>x.ID == subdivisionID)!.OrganizationID, cancellationToken);

        var nodes = _mapper.Map<OrganizationStructureNodeModelDetails[]>(subdivisions).ToList();
        
        var organizationNode = _mapper.Map<OrganizationStructureNodeModelDetails>(organization);
        
        nodes.Add(organizationNode);

        nodes.Reverse();
        
        return nodes.ToArray();
    }

    
    //TODO: Заменить на IMapperService
    private async Task<OrganizationStructureNodeModelDetails[]> ExecuteGetNodesAsync(OrganizationStructureNodeRequestModelDetails nodeRequest, CancellationToken cancellationToken) => 
        nodeRequest.ClassID switch
        {
            ObjectClass.Unknown => new [] {await GetOwnerAsync(cancellationToken)},
            ObjectClass.Owner => await GetOrganizationsAsync(cancellationToken),
            ObjectClass.Organizaton => await GetSubdivisionByOrganizationIDAsync(nodeRequest.ObjectID.Value,
                cancellationToken),
            ObjectClass.Division => await GetSubdivisionsAsync(nodeRequest, cancellationToken),
            _ => throw new Exception(
                $"Объект с classID = {nodeRequest.ClassID} не используется в дереве местоположений")
        };

    private async Task<OrganizationStructureNodeModelDetails> GetOwnerAsync(CancellationToken cancellationToken)
    {
        var owner = await _ownerBll.GetFirstAsync(cancellationToken);

        return _mapper.Map<OrganizationStructureNodeModelDetails>(owner);
    }

    private async Task<OrganizationStructureNodeModelDetails[]> GetOrganizationsAsync(CancellationToken cancellationToken)
    {
        var organizations = await _organizationBll.GetAllAsync(cancellationToken);

        return _mapper.Map<OrganizationStructureNodeModelDetails[]>(organizations);
    }

    private async Task<OrganizationStructureNodeModelDetails[]> GetSubdivisionByOrganizationIDAsync(Guid organizationID, CancellationToken cancellationToken) //TODO перенести в фильтр SubdivisionList
    {
        var subdivisions = await _subdivisionBll.GetDetailsPageAsync(new SubdivisionListFilter
            {
                OrganizationID = organizationID,
                OnlyRoots = true,
                SortBy = nameof(Subdivision.Name),
                Ascending = true
            },
            new ClientPageFilter<Subdivision>()
            {
                OrderByProperty = nameof(Subdivision.Name)
            },
            cancellationToken);
        return _mapper.Map<OrganizationStructureNodeModelDetails[]>(subdivisions);
    }

    private async Task<OrganizationStructureNodeModelDetails[]> GetSubdivisionsAsync(
        OrganizationStructureNodeRequestModelDetails nodeRequest, CancellationToken cancellationToken)
    {
        var foundSubdivision =
            await _subdivisionsRepository.WithMany(x => x.ChildSubdivisions).FirstOrDefaultAsync(
                x => x.ID == nodeRequest.ObjectID.Value, cancellationToken) ??
            throw new ObjectNotFoundException($"Subdivision not found with id = {nodeRequest.ObjectID}");

        var result = new List<OrganizationStructureNodeModelDetails>();

        if (foundSubdivision.ChildSubdivisions.Any())
        {
            var childSubdivisions =
                _mapper.Map<OrganizationStructureNodeModelDetails[]>(foundSubdivision.ChildSubdivisions);

            result.AddRange(childSubdivisions);
        }

        if (nodeRequest.IncludeUsers)
        {
            var childUsers = _mapper.Map<OrganizationStructureNodeModelDetails[]>(
                await _usersRepository.ToArrayAsync(x => x.SubdivisionID == nodeRequest.ObjectID.Value,
                    cancellationToken));

            result.AddRange(childUsers);
        }

        return result.ToArray();
    }
}
