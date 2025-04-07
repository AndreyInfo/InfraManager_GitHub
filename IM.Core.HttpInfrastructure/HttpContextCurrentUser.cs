using InfraManager.BLL;
using Microsoft.AspNetCore.Http;

namespace IM.Core.HttpInfrastructure
{
    public class HttpContextCurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextCurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId => _httpContextAccessor.HttpContext.GetUserId();
    }
}
