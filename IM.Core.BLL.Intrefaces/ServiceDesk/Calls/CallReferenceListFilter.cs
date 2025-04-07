using System;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    public class CallReferenceListFilter : BaseFilter
    {
        public Guid? CallID { get; init; }
        public Guid? ObjectID { get; set; }

        public override string ToString() =>
            $"{(CallID.HasValue ? $"CallID = {CallID} " : string.Empty)}{(ObjectID.HasValue ? $"ObjectID = {ObjectID}" : string.Empty)}".Trim();
    }
}
