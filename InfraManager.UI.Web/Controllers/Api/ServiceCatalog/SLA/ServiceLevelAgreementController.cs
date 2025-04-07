using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.OrganizationStructure;
using InfraManager.BLL.ServiceCatalogue;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.DAL.ServiceCatalogue.SLA;
using InfraManager.UI.Web.Models.ServiceCatalogue.SLA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.Api.ServiceCatalog.SLA
{
    [Route("api/sla")]
    [ApiController]
    [Authorize]
    public class ServiceLevelAgreementController : ControllerBase
    {
        private readonly IServiceLevelAgreementBLL _serviceLevelAgreementBLL;
        private readonly IServiceLevelAgreementReference _serviceLevelAgreementReference;

        public ServiceLevelAgreementController(IServiceLevelAgreementBLL serviceLevelAgreementBLL,
                                               IServiceLevelAgreementReference serviceLevelAgreementReference)
        {
            _serviceLevelAgreementBLL = serviceLevelAgreementBLL;
            _serviceLevelAgreementReference = serviceLevelAgreementReference;
        }

        #region SLA CRUD
        
        [HttpPost]
        public async Task<Guid> InsertAsync([FromBody] ServiceLevelAgreementData serviceLevelAgreementDetails,
            CancellationToken cancellationToken = default)
        {
            return await _serviceLevelAgreementBLL.InsertAsync(serviceLevelAgreementDetails, cancellationToken);
        }

        [HttpPut("{slaID}")]
        public async Task UpdateAsync(Guid slaID, [FromBody] ServiceLevelAgreementData serviceLevelAgreementDetails,
            CancellationToken cancellationToken = default)
        {
            await _serviceLevelAgreementBLL.UpdateAsync(slaID, serviceLevelAgreementDetails, cancellationToken);
        }


        [HttpDelete("{slaID}")]
        public async Task DeleteAsync([FromRoute] Guid slaID)
        {
            await _serviceLevelAgreementBLL.DeleteAsync(slaID);
        }

        [HttpGet]
        public async Task<ServiceLevelAgreementDetails[]> ListAsync([FromQuery] SLAFilter filter, CancellationToken cancellationToken = default)
        {
           return await _serviceLevelAgreementBLL.ListAsync(filter, cancellationToken);
        }
        
        [HttpGet("{slaID}")]
        public async Task<ServiceLevelAgreementDetails> GetAsync([FromRoute] Guid slaID)
        {
            return await _serviceLevelAgreementBLL.GetAsync(slaID, HttpContext.RequestAborted);
        }
        #endregion
        
        [HttpGet("{slaID}/concluded")]
        public async Task<SLAConcludedWithItem[]> GetConcludedAsync([FromRoute] Guid slaID,
            CancellationToken cancellationToken = default)
        {
            return await _serviceLevelAgreementBLL.GetConcludedWithAsync(slaID, cancellationToken);
        }

        [HttpPost("{slaID}/clone")]
        public async Task CloneAsync([FromRoute] Guid slaID, [FromBody] ServiceLevelAgreementData data,
            CancellationToken cancellationToken = default)
        {
            await _serviceLevelAgreementBLL.CloneAsync(slaID, data, cancellationToken);
        }

        [HttpGet("{slaID}/organization-item-groups")]
        public async Task<OrganizationItemGroupData[]> GetOrganizationItemGroupsAsync([FromRoute] Guid slaID,
            CancellationToken cancellationToken = default)
        {
            return await _serviceLevelAgreementBLL.GetOrganizationItemGroupsAsync(slaID, cancellationToken);
        }

        [HttpGet("{slaID}/service-references")]
        public async Task<SLAReferenceDetails[]> GetServiceReferencesAsync([FromRoute] Guid slaID,
            CancellationToken cancellationToken = default)
        {
            return await _serviceLevelAgreementReference.GetListAsync(slaID, ObjectClass.Service, cancellationToken);
        }

        #region Infrastructure

        [HttpGet("{slaID}/FreeInfrastructure")]
        public async Task<PortfolioServiceInfrastructureItem[]> FreeInfrastructureAsync([FromRoute] Guid slaID,
            [FromQuery] Guid serviceID, CancellationToken cancellationToken = default)
        {
            return await _serviceLevelAgreementBLL.FreeInfrastructureAsync(serviceID, slaID, cancellationToken);
        }

        [HttpGet("{slaID}/Infrastructure")]
        public async Task<PortfolioServiceInfrastructureItem[]> InfrastructureAsync([FromRoute] Guid slaID,
            [FromQuery] Guid serviceID, [FromQuery] BaseFilter filer, CancellationToken cancellationToken = default)
        {
            return await _serviceLevelAgreementBLL.InfrastructureAsync(slaID, serviceID, filer.StartRecordIndex,
                filer.CountRecords, filer.SearchString, cancellationToken);
        }

        [HttpPost("{slaID}/Infrastructure")]
        public async Task InsertInfrastructureAsync([FromRoute] Guid slaID, [FromBody] SLAPostRequestModel requestModel,
            CancellationToken cancellationToken = default)
        {
            await _serviceLevelAgreementBLL.InsertInfrastructureAsync(slaID, requestModel.ServiceReferenceID,
                cancellationToken);
        }

        [HttpDelete("{slaID}/Infrastructure")]
        public async Task DeleteInfrastructureAsync([FromRoute] Guid slaID, [FromBody] SLAPostRequestModel requestModel,
            CancellationToken cancellationToken = default)
        {
            await _serviceLevelAgreementBLL.DeleteInfrastructureAsync(slaID, requestModel.ServiceReferenceID,
                cancellationToken);
        }
        
        #endregion
    }
}
