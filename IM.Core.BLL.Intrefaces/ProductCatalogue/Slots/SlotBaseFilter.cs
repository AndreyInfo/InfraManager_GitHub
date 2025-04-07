using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;

namespace InfraManager.BLL.ProductCatalogue.Slots;
public class SlotBaseFilter : BaseFilter
{
    public Guid ObjectID { get; init; }
}