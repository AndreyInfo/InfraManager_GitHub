using InfraManager.DAL.OrganizationStructure;
using System.Diagnostics.CodeAnalysis;

namespace IM.Core.Import.BLL.Comparers
{
    internal class SubdivisionComparer : IEqualityComparer<Subdivision>
    {
        public bool Equals(Subdivision? x, Subdivision? y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;
            return x.ExternalID == y?.ExternalID
                 && x.Name == y.Name
                 && x.OrganizationID == y.OrganizationID
                 && x.SubdivisionID == y.SubdivisionID;
        }

        public int GetHashCode([DisallowNull] Subdivision obj)
        {
            return (obj.ExternalID?.GetHashCode() ?? 0)
                   ^ (obj.Name?.GetHashCode() ?? 0)
                   ^ obj.OrganizationID.GetHashCode()
                   ^ (obj.SubdivisionID?.GetHashCode() ?? 0);
        }
    }
}
