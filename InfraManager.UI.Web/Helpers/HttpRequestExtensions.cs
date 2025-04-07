using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;

namespace InfraManager.UI.Web.Helpers
{
    public static class HttpRequestExtensions
    {
        public static IActionResult CreateErrorResponse(this HttpRequest request, HttpStatusCode httpStatusCode, string text)
        {
            return new ContentResult()
            {
                Content = text,
                StatusCode = (int)httpStatusCode
            };
        }

        public static IActionResult CreateResponse(this HttpRequest request, HttpStatusCode httpStatusCode, string text)
        {
            return new ContentResult()
            {
                Content = text,
                StatusCode = (int)httpStatusCode
            };
        }

        public static IActionResult CreateResponse(this HttpRequest request, HttpStatusCode httpStatusCode, object content)
        {
            return new JsonResult(content)
            {
                StatusCode = (int)httpStatusCode
            };
        }
    }
}
