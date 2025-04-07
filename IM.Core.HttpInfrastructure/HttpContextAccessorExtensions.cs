using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace IM.Core.HttpInfrastructure;

public static class HttpContextAccessorExtensions
{
    public static Guid GetUserId(this HttpContext httpContext)
    {
        return new Guid(((ClaimsIdentity)httpContext.User.Identity).GetUserId());
    }
}