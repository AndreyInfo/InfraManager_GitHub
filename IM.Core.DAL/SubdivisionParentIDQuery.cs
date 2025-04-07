using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL;

public class SubdivisionParentIDQuery : ISubdivisionParentIDQuery,ISelfRegisteredService<ISubdivisionParentIDQuery>
{
    private readonly DbSet<Subdivision> _subdivisions;

    public SubdivisionParentIDQuery(DbSet<Subdivision> subdivisions)
    {
        _subdivisions = subdivisions;
    }
    public async Task<Guid?> ExecuteAsync(Guid? organizationID,
        IEnumerable<string> subdivisionParent,
        Guid? defaultOrganizationID, Guid? parentSubdivisionID,
        CancellationToken token = default)
    {
        Guid? currentID = null;
        Guid? currentOrganizationID;
        if (organizationID == null)
        {
            if (defaultOrganizationID == null)
                return null;
            currentOrganizationID = defaultOrganizationID;
            currentID = parentSubdivisionID;
        }
        else
        {
            currentOrganizationID = organizationID;
        }
        
        if (defaultOrganizationID == null)
        {
            foreach (var subdivisionName in subdivisionParent)
            {
                var localParentID = currentID;
                var subdivisionID = await _subdivisions.AsQueryable()
                    .Where(x => (x.OrganizationID != Guid.Empty && x.OrganizationID == currentOrganizationID)
                                && x.SubdivisionID == localParentID &&
                                x.Name == subdivisionName).Select(x => (Guid?) x.ID).SingleOrDefaultAsync(token);
                if (subdivisionID == null)
                    return null;
                currentID = subdivisionID;
            }
            return currentID;
        }

        bool wasNotFound = false;
        foreach (var subdivisionName in subdivisionParent)
        {
            wasNotFound = false;
            var localCurrentID = currentID;
            var localOrganizationID = currentOrganizationID;
            var subdivisionID = await _subdivisions.AsQueryable()
                .Where(x => (x.OrganizationID != Guid.Empty && x.OrganizationID == localOrganizationID)
                            && x.SubdivisionID == localCurrentID &&
                            x.Name == subdivisionName).Select(x => (Guid?) x.ID).SingleOrDefaultAsync(token);
            if (subdivisionID == null)
            {
                wasNotFound = true;
                currentOrganizationID = defaultOrganizationID.Value;
                currentID = defaultOrganizationID;
            }
            else
                currentID = subdivisionID;
        }
        if (wasNotFound)
            return null;
        return currentID;
    }
}