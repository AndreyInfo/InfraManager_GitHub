namespace IM.Core.Import.BLL.Interface.Import.Models
{
    public record ModelAndDetailsModel<T,V>(V Model, T Details)
    {
        public V Model { get; set; } = Model;
        public T Details { get; set; } = Details;
    }
}
