namespace IM.Core.Import.BLL.Import.Array;

internal sealed record OrganizationIDKey(Guid? Key) :NullableKey<Guid>(Key)
{
    public override string ToString()
    {
        return $"Идентификатор организации в БД: {LogHelper.ToOutputFormat(Key?.ToString())}";
    }
}