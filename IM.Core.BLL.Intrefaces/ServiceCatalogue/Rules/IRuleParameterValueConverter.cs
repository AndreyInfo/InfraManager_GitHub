namespace InfraManager.BLL.ServiceCatalogue.Rules;

public interface IRuleParameterConverter
{
    /// <summary>
    /// Переводит RuleParameterValue в Byte[]
    /// </summary>
    /// <param name="parameterValues">Данные правил для перевода в байты</param>
    byte[] ParametersToBytes(RuleParameterValue[] parameterValues);


    /// <summary>
    /// Переводит Byte[] в RuleParameterValue
    /// </summary>
    /// <param name="ruleParameterData">Массив байтов для перевода в понятную модель RuleParameterValue[]</param>
    RuleParameterValue[] ParametersFromBytes(byte[] ruleParameterData);
}