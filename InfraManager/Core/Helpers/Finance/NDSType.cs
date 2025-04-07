
using System;

namespace InfraManager.Core.Helpers.Finance
{
    [Serializable]
    public enum NDSType : byte
    {
        [FriendlyName("Цена включает в себя НДС"), FriendlyName("VAT included", "en-US")]
        AlreadyIncluded = 0,
        [FriendlyName("НДС не облагается"), FriendlyName("Is not a subject to a tax", "en -US")]
        NotNeeded = 1,
        [FriendlyName("Добавить НДС к цене"), FriendlyName("Add VAT to price", "en-US")]
        AddToPrice = 2
    }
}
