namespace IM.Core.Import.BLL.Import.Array;

internal sealed record FIOKey(string? Key) :StringKey(Key)
{
    public override string ToString()
    {
        return $"ФИО: {LogHelper.ToOutputFormat(Key)}";
    }
}