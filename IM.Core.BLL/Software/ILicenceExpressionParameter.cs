namespace InfraManager.BLL.Software
{
    public interface ILicenceExpressionParameter
    {
        int CoreQuantity { get; }
        int CpuQuantity { get; }
        int Coef();
    }
}
