using System;
using System.Collections.Generic;
using System.Linq;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.DeleteStrategies;

internal class ProblemTypeDeleteStrategy : IDeleteStrategy<ProblemType>, ISelfRegisteredService<IDeleteStrategy<ProblemType>>
{
    private readonly DbSet<ProblemType> _problemTypes;
    private readonly List<ProblemType> _problemTypesIdForRemoving = new();

    public ProblemTypeDeleteStrategy(DbSet<ProblemType> problemTypes)
    {
        _problemTypes = problemTypes;
    }

    public void Delete(ProblemType entity)
    {
        SearchDependencies(entity);
        _problemTypesIdForRemoving.ForEach(x => x.MarkForDelete());
    }

    private void SearchDependencies(Lookup entity)
    {
        if (_problemTypesIdForRemoving.Any(x => x.ID == entity.ID))
        {
            throw new ArgumentException("Цикличная зависимость в дереве при удалении типа проблемы");
        }

        var problemType = _problemTypes.FirstOrDefault(x => x.ID == entity.ID);

        _problemTypesIdForRemoving.Add(problemType);

        var childProblemTypes = _problemTypes.Where(x => x.ParentProblemTypeID == problemType.ID).ToList();
        
        foreach (var childProblemType in childProblemTypes)
        {
            SearchDependencies(childProblemType);
        }
    }
}