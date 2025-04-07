using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager.BLL.Import;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.BFF.Import
{
    [Route("api/ActiveDirectory/UIADIMFieldConcordance")]
    [ApiController]
    [Authorize]
    public class UIADIMFieldConcordancesController : ControllerBase
    {
        private readonly IUIADIMFieldConcordancesBLL _bll;

        public UIADIMFieldConcordancesController(IUIADIMFieldConcordancesBLL bll)
        {
            _bll = bll;
        }


        [HttpGet("{configuration-id}/{ad-class-id}/{im-field-id}")]
        public Task<UIADIMFieldConcordancesOutputDetails> GetAsync([FromRoute(Name="configuration-id")] Guid configId,
            [FromRoute(Name="ad-class-id")]Guid classID, [FromRoute(Name="im-field-id")] int imFieldID,
            CancellationToken cancellationToken = default)
        {
            var id = new UIADIMFieldConcordancesKey()
            {
                ADConfigurationID = configId,
                ADClassID = classID,
                IMFieldID = imFieldID
            };
            return _bll.DetailsAsync(id, cancellationToken);
        }

        [HttpGet]
        public Task<UIADIMFieldConcordancesOutputDetails[]> GetAsync([FromQuery] UIADIMFieldConcordancesFilter filter,
            CancellationToken cancellationToken = default)
        {
            return _bll.GetDetailsArrayAsync(filter, cancellationToken);
        }

        [HttpPost]
        public Task<UIADIMFieldConcordancesOutputDetails> PostAsync([FromBody] UIADIMFieldConcordancesDetails data,
            CancellationToken cancellationToken = default)
        {
            return _bll.AddAsync(data, cancellationToken);
        }

        [HttpPut("{configuration-id}/{ad-class-id}/{im-field-id}")]
        public Task<UIADIMFieldConcordancesOutputDetails> PutAsync([FromRoute(Name="configuration-id")] Guid configId,
            [FromRoute(Name="ad-class-id")]Guid classID, [FromRoute(Name="im-field-id")] int imFieldID,
            [FromBody] UIADIMFieldConcordancesDetails data, CancellationToken cancellationToken = default)
        {
            var id = new UIADIMFieldConcordancesKey()
            {
                ADConfigurationID = configId,
                ADClassID = classID,
                IMFieldID = imFieldID
            };
            return _bll.UpdateAsync(id, data, cancellationToken);
        }

        [HttpDelete("{configuration-id}/{ad-class-id}/{im-field-id}")]
        public Task DeleteAsync([FromRoute(Name="configuration-id")] Guid configId,
            [FromRoute(Name="ad-class-id")]Guid classID, [FromRoute(Name="im-field-id")] int imFieldID,
            CancellationToken cancellationToken = default)
        {
            var id = new UIADIMFieldConcordancesKey()
            {
                ADConfigurationID = configId,
                ADClassID = classID,
                IMFieldID = imFieldID
            };
            return _bll.DeleteAsync(id, cancellationToken);
        }
    }
}