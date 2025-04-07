using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Localization;
using InfraManager.DAL;

namespace InfraManager.BLL;

public abstract class BooleanToLocalizedTextLookupQuery : ILookupQuery
{
    protected abstract string  TrueValueResourceName { get; }

    protected abstract string  FalseValueResourceName { get; }


    private readonly ILocalizeText _localizer;


    protected BooleanToLocalizedTextLookupQuery(ILocalizeText localizer)
    {
        _localizer = localizer;
    }

    public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var trueValueText = await _localizer.LocalizeAsync(TrueValueResourceName, cancellationToken);
        var falseValueText = await _localizer.LocalizeAsync(FalseValueResourceName, cancellationToken);

        return new[]
        {
            new ValueData { ID = true.ToString(), Info = trueValueText, },
            new ValueData { ID = false.ToString(), Info = falseValueText, },
        };
    }
}