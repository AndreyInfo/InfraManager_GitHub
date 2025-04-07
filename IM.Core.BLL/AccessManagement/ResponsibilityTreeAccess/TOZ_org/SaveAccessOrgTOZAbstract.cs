using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.OrganizationStructure;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess.TOZ_org;
internal abstract class SaveAccessOrgTOZAbstract<TEntity> : SaveAccessAbstract<TEntity> where TEntity : class
{
    protected readonly IReadonlyRepository<Subdivision> _subdivisionRepository;

    protected override AccessTypes AccessType => AccessTypes.TOZ_org;
    protected override IReadOnlyCollection<ObjectClass> ChildClasses => new ObjectClass[]
    {
        ObjectClass.Division
    };

    public SaveAccessOrgTOZAbstract(IRepository<ObjectAccess> objectAccesses
     , IMapper mapper
     , IUnitOfWork unitOfWork
     , IReadonlyRepository<Subdivision> subdivisionRepository)
     : base(objectAccesses
         , mapper
         , unitOfWork)
    {
        _subdivisionRepository = subdivisionRepository;
    }



    protected async Task InsertDivisionAsync(Expression<Func<Subdivision, bool>> expression, Guid ownerID, CancellationToken cancellationToken)
    {
        var divisions = await _subdivisionRepository.ToArrayAsync(expression, cancellationToken);
        foreach (var division in divisions)
        {
            await InserSubDivision(division.ID, ownerID, cancellationToken);
            await InsertItemAsync(ownerID, division.ID, ObjectClass.Division, cancellationToken: cancellationToken);
        }
    }

    private async Task InserSubDivision(Guid divisionID, Guid ownerID, CancellationToken cancellationToken)
    {
        var subdivisions = await GetAllSubdivisionAsync(divisionID, cancellationToken);
        foreach (var item in subdivisions)
            await InsertItemAsync(ownerID, item.ID, ObjectClass.Division, cancellationToken: cancellationToken);
    }

    protected async Task<Subdivision[]> GetAllSubdivisionAsync(Guid id, CancellationToken cancellationToken)
    {
        var root = await _subdivisionRepository.WithMany(c => c.ChildSubdivisions)
                   .FirstOrDefaultAsync(x => x.ID == id, cancellationToken)
                   ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.Division);

        var result = new Queue<Subdivision>();
        var nodes = new Queue<Subdivision>();
        do
        {
            result.Enqueue(root);

            foreach (var item in root.ChildSubdivisions)
                nodes.Enqueue(item);

        } while (nodes.TryDequeue(out root));

        return result.ToArray();
    }
}
