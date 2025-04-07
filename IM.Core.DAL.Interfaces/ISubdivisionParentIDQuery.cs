using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL;

public interface ISubdivisionParentIDQuery
{
    Task<Guid?> ExecuteAsync(Guid? organizationID,
        IEnumerable<string> subdivisionParent,
        Guid? defaultOrganizationID, Guid? parentSubdivisionID = null,
        CancellationToken token = default);
}