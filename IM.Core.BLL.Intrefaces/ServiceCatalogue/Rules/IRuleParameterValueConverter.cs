namespace InfraManager.BLL.ServiceCatalogue.Rules;

public interface IRuleParameterConverter
{
    /// <summary>
    /// ��������� RuleParameterValue � Byte[]
    /// </summary>
    /// <param name="parameterValues">������ ������ ��� �������� � �����</param>
    byte[] ParametersToBytes(RuleParameterValue[] parameterValues);


    /// <summary>
    /// ��������� Byte[] � RuleParameterValue
    /// </summary>
    /// <param name="ruleParameterData">������ ������ ��� �������� � �������� ������ RuleParameterValue[]</param>
    RuleParameterValue[] ParametersFromBytes(byte[] ruleParameterData);
}