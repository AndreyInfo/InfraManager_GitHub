namespace IM.Core.Import.BLL.Import.Array;

internal sealed record OrganizationNameKey(string? Key) :StringKey(Key)
{
    
    public override string ToString()
    {
        return $"Имя организации: {LogHelper.ToOutputFormat(Key)}";
    }
}