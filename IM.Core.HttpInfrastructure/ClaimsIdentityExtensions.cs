using System.Security.Claims;

namespace IM.Core.HttpInfrastructure
{
    public static class ClaimsIdentityExtensions
    {
        private static string IdClaim = "id";

        public static Claim CreateIdClaim(string userId)
        {
            return new Claim(IdClaim, userId);
        }

        public static string GetUserId(this ClaimsIdentity identity)
        {
            return identity.Claims.SingleOrDefault(c => c.Type == IdClaim)?.Value;
        }
    }
}
