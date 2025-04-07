namespace IM.Core.Import.BLL.Import.Array;

internal sealed record SurnameNameKey(string surname, string name) :RequiredComplexKey<StringKey,StringKey>(new StringKey(surname), new StringKey(name))
{
    public static bool IsSet(string surname, string name)
    {
        return StringKey.IsSet(surname) && StringKey.IsSet(name);
    }

    public override string GetKey()
    {
        return base.GetKey().Trim();
    }
    public override string ToString()
    {
        return $"[Фамилия: {LogHelper.ToOutputFormat(First.Key)}, Имя: {LogHelper.ToOutputFormat(Second.Key)}";
    }
}