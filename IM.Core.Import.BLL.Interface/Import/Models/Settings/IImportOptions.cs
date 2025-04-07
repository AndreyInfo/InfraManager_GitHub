using InfraManager;

namespace IM.Core.Import.BLL.Interface.Import.Models.Settings;

public interface IImportOptions
{
    string EncodingName { get; }
    uint MaxPacketSize { get; set; }
    string ManufacturerExternalIDFieldName { get; }
    IReadOnlyDictionary<string, string> PropertyNames { get; }
    IReadOnlyDictionary<string, string> Scripts { get; set; }
    ObjectClass ModelClass { get; set; }
}