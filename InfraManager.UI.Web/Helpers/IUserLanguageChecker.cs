using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Helpers
{
    public interface IUserLanguageChecker
    {
        Task CheckAsync(HttpContext httpContext);
    }
}
