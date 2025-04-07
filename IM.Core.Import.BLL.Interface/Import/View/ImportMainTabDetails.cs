namespace IM.Core.Import.BLL.Interface.Import.View
{
    public class ImportMainTabDetails
    {
        public Guid ID { get; set; }
        public string Name { get; init; }
        public string Note { get; init; }
        public byte ProviderType { get; init; }
        public Guid? SelectedConfiguration { get; set; }
        public string PolledObject { get; set; }
    }
}