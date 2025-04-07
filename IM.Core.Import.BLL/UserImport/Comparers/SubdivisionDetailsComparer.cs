using IM.Core.Import.BLL.Interface.Import;
using System.Diagnostics.CodeAnalysis;
using InfraManager.DAL.Import;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace IM.Core.Import.BLL.Comparers
{
    internal class SubdivisionDetailsComparer : IEqualityComparer<ISubdivisionDetails>
    {
        public bool Equals(ISubdivisionDetails? x, ISubdivisionDetails? y)
        {
            return x.ExternalID == y.ExternalID
                   && x.Name == y.Name
                   && x.OrganizationID == y.OrganizationID
                   && x.SubdivisionID == y.SubdivisionID;

        }

        public int GetHashCode([DisallowNull] ISubdivisionDetails obj)
        {
            return obj.ExternalID?.GetHashCode() ?? 0 
                ^ obj.Name?.GetHashCode() ?? 0
                ^ obj.OrganizationID?.GetHashCode() ?? 0
                ^ obj.SubdivisionID?.GetHashCode() ?? 0;
        }
    }
}
