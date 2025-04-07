
namespace IM.Core.Import.BLL.Interface.Configurations.View
{
    public class ConfigurationCSVDetails
    {
        public Guid ID { get; init; }
        public string Name { get; init; }
        public string Note { get; init; }
        public char Delimiter { get; init; }
        public string Configuration { get; set; }
    }
}
