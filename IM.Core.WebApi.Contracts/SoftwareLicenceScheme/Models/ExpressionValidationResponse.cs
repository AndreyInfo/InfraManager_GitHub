namespace InfraManager.CrossPlatform.WebApi.Contracts.SoftwareLicenceScheme.Models
{
    public class ExpressionValidationResponse
    {
        public bool IsSuccess { get; set; }
        public string MessageKey { get; set; }
        public object[] MessageArguments { get; set; }
    }
}
