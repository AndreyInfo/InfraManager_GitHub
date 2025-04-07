namespace IM.Core.Import.BLL.Import.Array;


internal sealed record ParentSubdivisionIDKey(Guid? Key) :NullableKey<Guid>(Key)
{
    public override string ToString()
    {
        return $"Идентификатор родительского подразделения в БД {LogHelper.ToOutputFormat(Key?.ToString())}";
    }
}