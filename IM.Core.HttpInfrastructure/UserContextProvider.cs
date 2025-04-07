using System.Security.Claims;
using InfraManager.CrossPlatform.WebApi.Contracts.Auth;
using Microsoft.AspNetCore.Http;

namespace IM.Core.HttpInfrastructure
{
    public class UserContextProvider : IUserContextProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserContextProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public UserContext GetUserContext()
        {
            var userIdentity = _httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity 
                ?? throw new HttpException(System.Net.HttpStatusCode.Unauthorized);

            var userId = userIdentity.GetUserId();

            return Guid.TryParse(userId, out var guid)
                ? new UserContext(guid)
                : throw new HttpException(System.Net.HttpStatusCode.Unauthorized);
        }
    }
}
