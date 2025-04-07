namespace IM.Core.Import.BLL.Import;

internal class LogHelper
{
    private const string nullString = "(null)";
    private const string EmptyString = "(пусто)";

    public static string ToOutputFormat<T>(IEnumerable<T> source)
    {
        if (source == null)
            return nullString;
        var enumerable = source.Select(x => ToOutputFormat(x?.ToString()));
        return $"[{string.Join(",", enumerable)}]";
    }
    public static string ToOutputFormat(string? source)
    {
        var result = source ?? nullString;
        result = string.IsNullOrWhiteSpace(result)
            ? EmptyString
            : result;
        return result;
    }
}