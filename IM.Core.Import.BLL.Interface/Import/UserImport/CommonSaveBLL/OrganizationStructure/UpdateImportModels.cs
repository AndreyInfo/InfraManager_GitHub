namespace IM.Core.Import.BLL.Interface.Import.Models
{
    public record UpdateImportModels<T,V>(Dictionary<T, V> ModelsForUpdate, IEnumerable<T> FoundModels)
    {
        public Dictionary<T,V> ModelsForUpdate { get; set; } = ModelsForUpdate;
        public IEnumerable<T> FoundModels { get; set; } = FoundModels;
    }
}
