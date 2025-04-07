using IM.Core.Import.BLL.Interface.Import;
using System.Diagnostics.CodeAnalysis;

namespace IM.Core.Import.BLL.Comparers
{
    internal class OrganizationDetailsComparer : IEqualityComparer<OrganizationDetails>
    {
        public bool Equals(OrganizationDetails? x, OrganizationDetails? y)
        {
            return x.ExternalId == y.ExternalId
                 && x.Name == y.Name; 

        }

        public int GetHashCode([DisallowNull] OrganizationDetails obj)
        {
            return obj.ExternalId?.GetHashCode() ?? 0 ^ obj.Name?.GetHashCode() ?? 0;
        }
    }
}
