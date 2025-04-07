namespace IM.Core.Import.BLL.Interface.Import.Models.Import;

public interface IValidator<TData>
{
    Task<bool> ValidateAsync(Guid id, TData model, CancellationToken token);
}

public interface IValidator<TData, TEntityType> : IValidator<TData>
{
}