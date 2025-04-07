using System;

namespace InfraManager.DAL.Import
{
    public class ImportMainTabModel
    {
        public string Name { get; init; }
        public string Note { get; init; }
        public byte ProviderType { get; init; }
        public Guid? SelectedConfiguration { get; set; }
        public string PolledObject { get; set; }
    }
}
