using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager.BLL.Import;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.BFF.Import
{
    [Route("api/ActiveDirectory/UIADIMClassConcordance")]
    [ApiController]
    [Authorize]
    public class UIADIMClassConcordancesController : ControllerBase
    {
        private readonly IUIADIMClassConcordancesBLL _bll;

        public UIADIMClassConcordancesController(IUIADIMClassConcordancesBLL bll)
        {
            _bll = bll;
        }


        [HttpGet("{configuration-id}/{ad-class-id}/{im-class-id}")]
        public Task<UIADIMClassConcordancesOutputDetails> GetAsync([FromRoute(Name="configuration-id")] Guid configId,
            [FromRoute(Name="ad-class-id")]Guid classID, [FromRoute(Name="im-class-id")] int imClassID,
            CancellationToken cancellationToken = default)
        {
            var id = new UIADIMClassConcordancesKey()
            {
                ADConfigurationID = configId,
                ADClassID = classID,
                IMClassID = imClassID
            };
            return _bll.DetailsAsync(id, cancellationToken);
        }

        [HttpGet]
        public Task<UIADIMClassConcordancesOutputDetails[]> GetAsync([FromQuery] UIADIMClassConcordancesFilter filter,
            CancellationToken cancellationToken = default)
        {
            return _bll.GetDetailsArrayAsync(filter, cancellationToken);
        }

        [HttpPost]
        public Task<UIADIMClassConcordancesOutputDetails> PostAsync([FromBody] UIADIMClassConcordancesDetails data,
            CancellationToken cancellationToken = default)
        {
            return _bll.AddAsync(data, cancellationToken);
        }

        [HttpPut("{configuration-id}/{ad-class-id}/{im-class-id}")]
        public Task<UIADIMClassConcordancesOutputDetails> PutAsync([FromRoute(Name="configuration-id")] Guid configId,
            [FromRoute(Name="ad-class-id")]Guid classID, [FromRoute(Name="im-class-id")] int imClassID,
            [FromBody] UIADIMClassConcordancesDetails data, CancellationToken cancellationToken = default)
        {
            var id = new UIADIMClassConcordancesKey()
            {
                ADConfigurationID = configId,
                ADClassID = classID,
                IMClassID = imClassID
            };
            return _bll.UpdateAsync(id, data, cancellationToken);
        }

        [HttpDelete("{configuration-id}/{ad-class-id}/{im-class-id}")]
        public Task DeleteAsync([FromRoute(Name="configuration-id")] Guid configId,
            [FromRoute(Name="ad-class-id")]Guid classID, [FromRoute(Name="im-class-id")] int imClassID,
            CancellationToken cancellationToken = default)
        {
            var id = new UIADIMClassConcordancesKey()
            {
                ADConfigurationID = configId,
                ADClassID = classID,
                IMClassID = imClassID
            };
            return _bll.DeleteAsync(id, cancellationToken);
        }
    }
}