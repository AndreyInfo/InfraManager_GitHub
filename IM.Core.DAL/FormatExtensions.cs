namespace InfraManager.DAL
{
    public static class FormatExtensions
    {
        public static string ToStartsWithPattern(this string searchText)
        {
            return string.Concat(searchText.Replace("%", "[%]"), "%");
        }

        public static string ToContainsPattern(this string searchText)
        {
            return string.Concat("%", searchText.Replace("%", "[%]"), "%");
        }

        public static string ToEndsWithPattern(this string searchText)
        {
            return string.Concat("%", searchText.Replace("%", "[%]"));
        }
    }
}
