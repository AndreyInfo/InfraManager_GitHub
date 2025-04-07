using System;
using System.IO;

namespace InfraManager
{
    public static class StringExtensions
    {
        public static string RemoveHtmlTags(this string htmlString)
        {
            if (htmlString == null)
            {
                return null;
            }
            
            //TODO: простыню ниже привести в человеческий вид
            var result = htmlString.Replace("\t", string.Empty);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&nbsp;", " ",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            // Remove repeating spaces because browsers ignore them
            result = System.Text.RegularExpressions.Regex.Replace(result, @"( )+", " ");

            //remove comments and xml from outlook
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<!(-)+[\s\S]*?(-)+>", String.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            // Remove the header (prepare first by clearing attributes)
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*head([^>])*>", "<head>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"(<( )*(/)( )*head( )*>)", "</head>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"(<head>)[\s\S]*(</head>)", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // remove all scripts (prepare first by clearing attributes)
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*script([^>])*>", "<script>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"(<( )*(/)( )*script( )*>)", "</script>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"(<script>)[\s\S]*(</script>)", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // remove all styles (prepare first by clearing attributes)
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*style([^>])*>", "<style>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"(<( )*(/)( )*style( )*>)", "</style>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"(<style>)[\s\S]*(</style>)", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // insert tabs in spaces of <td> tags
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*td([^>])*>", "\t",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // insert line breaks in places of <BR> and <LI> tags
            /*result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*br[\s\/]*>", "\r\n",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);*/
            result = System.Text.RegularExpressions.Regex.Replace(result,
                    @"<span>\r\n<span>", "<span><span>",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                    @"<br/>\r\n", Environment.NewLine,
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                    @"<br/><body>\r\n", "<body>\r\n",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                    @"<br/><br/>", Environment.NewLine,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                    @"<br>\r\n", Environment.NewLine,
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                    @"<br><body>\r\n", "<body>\r\n",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                    @"<br><br>", Environment.NewLine,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*li( )*>", "\r\n",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // insert line paragraphs (double line breaks) in place
            // if <P>, <DIV> and <TR> tags
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*tr([^>])*>", "\r\n",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*p([^>])*>", "\r\n",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*div([^>])*>", "\r\n",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // Remove remaining tags like <a>, links, images,
            // comments etc - anything that's enclosed inside < >
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"</([^>])*>", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            //
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<\s*img[^>]*>", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            //
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<([^<> @])+([ ])+([^>])*>", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<([^<> @])+>", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // replace special characters:
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @" ", " ",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&nbsp;", " ",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&bull;", " * ",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&lsaquo;", "<",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&rsaquo;", ">",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&trade;", "(tm)",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&frasl;", "/",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&lt;", "<",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&gt;", ">",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&copy;", "(c)",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&reg;", "(r)",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&#43;", "+",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&quot;", "\"",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&amp;", "&",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            // Remove all others. More can be added, see
            // http://hotwired.lycos.com/webmonkey/reference/special_characters/
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&(.{2,6});", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // Remove extra line breaks and tabs:
            // replace over 2 breaks with 2 and over 4 tabs with 4.
            // Prepare first to remove any whitespaces in between
            // the escaped characters and remove redundant tabs in between line breaks
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\n)( )+(\n)", "\n\n",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\r)( )+(\r)", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\t)( )+(\t)", "\t\t",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\t)( )+(\r)", "\t\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\r)( )+(\t)", "\r\t",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "[\\n]+", "\n",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "[\\r]+", "\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            // Remove redundant tabs
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\r)(\t)+(\r)", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            // Remove multiple tabs following a line break with just one tab
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\r)(\t)+", "\r\t",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            return result.Trim();
        }

        public static string RemoveInvalidPathChars(this string str)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            foreach (var ch in invalidChars)
                str = str.Replace(ch, '@');
            return str;
        }

        public static string Slice(this string @string, int maxLength)
        {
            return @string != null ? (@string.Length > maxLength ? @string[..maxLength] : @string) : string.Empty;
        }
    }
}
