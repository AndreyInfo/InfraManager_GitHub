using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraManager.WebApi.Contracts.Common
{
    public class WorkflowSchemeFilter : BaseFilter
    {
        public int? ClassID { get; set; }
        public byte? Status { get; set; }
    }
}
