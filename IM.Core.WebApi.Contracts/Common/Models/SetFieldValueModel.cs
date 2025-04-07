namespace InfraManager.CrossPlatform.WebApi.Contracts.Common.Models
{
    public class SetFieldValueModel
    {
        public object NewValue { get; set; }
        public object OldValue { get; set; }
        public bool? ReplaceAnyway { get; set; }
    }
}
