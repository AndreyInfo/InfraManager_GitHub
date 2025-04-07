using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;
using System.Collections.Generic;

namespace InfraManager.CrossPlatform.WebApi.Contracts.SoftwareLicenceScheme.Models
{
    public class SoftwareLicenceSchemeDeleteResponse
    {
        public List<BaseResult<Guid, SoftwareLicenceSchemeRules>> Results { get; set; }
    }
}
