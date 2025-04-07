using IM.Core.Import.BLL.Interface.Import.Models.Import;

namespace IM.Core.Import.BLL.Import.Importer.DownloadData;

internal class ValidatorFacade<TData>:IValidator<TData>
{
    private readonly IEnumerable<IValidator<TData>> _validators;

    public ValidatorFacade(params IValidator<TData>[] validators)
    {
        _validators = validators;
    }

    public async Task<bool> ValidateAsync(Guid id, TData model, CancellationToken token)
    {
        foreach (var validator in _validators)
        {
            if (!(await validator.ValidateAsync(id, model, token)))
                return false;
        }

        return true;
    }

    
}