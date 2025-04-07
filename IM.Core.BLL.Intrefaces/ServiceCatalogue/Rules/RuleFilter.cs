using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceCatalogue.Rules
{
    public class RuleFilter : BaseFilter
    {
        public Guid? SLAID { get; init; }
        public int? OperationalLevelAgreementID { get; init; }
    }
}
