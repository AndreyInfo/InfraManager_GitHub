namespace IM.Core.Import.BLL.Import.Array;

internal record NamedKey(string? Key, string fieldName) :StringKey(Key)
{
    public override string ToString()
    {
        return $"{fieldName}: {LogHelper.ToOutputFormat(Key)}";
    }
}