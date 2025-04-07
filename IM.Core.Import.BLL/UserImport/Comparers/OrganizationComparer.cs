using InfraManager.DAL.OrganizationStructure;
using System.Diagnostics.CodeAnalysis;

namespace IM.Core.Import.BLL.Comparers
{
    internal class OrganizationComparer : IEqualityComparer<Organization>
    {
        public bool Equals(Organization? x, Organization? y)
        {
            return x.ExternalId == y.ExternalId
                             && x.Name == y.Name;
        }

        public int GetHashCode([DisallowNull] Organization obj)
        {
            return obj.ExternalId.GetHashCode() ^ obj.Name.GetHashCode();
        }
    }
}
