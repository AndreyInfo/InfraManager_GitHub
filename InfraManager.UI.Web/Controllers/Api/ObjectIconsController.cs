using InfraManager.BLL.ObjectIcons;
using InfraManager.UI.Web.ResourceMapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using IOFile = System.IO.File;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace InfraManager.UI.Web.Controllers.Api
{
    [Route("api")]
    [ApiController]
    public class ObjectIconsController : ControllerBase
    {
        private const string DefaultIconExtension = "png";
        private const string DefaultIconMediaType = $"image/{DefaultIconExtension}";

        private readonly IObjectIconBLL _service;
        private readonly IWebHostEnvironment _environment;

        public ObjectIconsController(IObjectIconBLL service, IWebHostEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }

        [HttpGet("{resource}/{id}/icon")]
        public async Task<ActionResult<ObjectIconDetails>> GetAsync(WebApiResource resource, Guid id, CancellationToken cancellationToken = default)
        {
            if (!resource.TryGetObjectClass(out var classID))
            {
                return NotFound();
            }
            return await _service.GetAsync(new InframanagerObject(id, classID), cancellationToken);
        }

        [Authorize]
        [HttpPost("{resource}/{id}/icon")]
        public async Task<IActionResult> PostAsync(
            WebApiResource resource,
            Guid id,
            [FromForm] ObjectIconData data,
            IFormFile file,
            CancellationToken cancellationToken = default)
        {
            if (file != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream, cancellationToken);
                    return await PostAsync(
                        resource,
                        id,
                        new ObjectIconData
                        {
                            Content = memoryStream.ToArray()
                        },
                        cancellationToken);
                }
            }

            return await PostAsync(resource, id, data, cancellationToken);
        }

        private async Task<IActionResult> PostAsync(
            WebApiResource resource,
            Guid id,
            [FromBody] ObjectIconData data,
            CancellationToken cancellationToken = default)
        {
            if (!resource.TryGetObjectClass(out var classID))
            {
                return NotFound();
            }

            await _service.SetAsync(new InframanagerObject(id, classID), data, cancellationToken);
            return Ok();
        }
    }
}
