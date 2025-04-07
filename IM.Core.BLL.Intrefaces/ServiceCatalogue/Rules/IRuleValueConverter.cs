using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceCatalogue.Rules
{
    /// <summary>
    /// Конвертер предназначенный для трансформации <see cref="RuleValue"/> в массив байтов и восстановки обратно в модель данных.
    /// </summary>
    public interface IRuleValueConverter
    {
        ValueTask<byte[]> ConvertToBytesAsync(RuleValue ruleValue, CancellationToken cancellationToken = default);

        ValueTask<RuleValue> ConvertFromBytesAsync(byte[] ruleValueData, CancellationToken cancellationToken = default);
    }
}
