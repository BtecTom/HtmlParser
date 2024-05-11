using System.Text;

namespace ForbesHtmlParser;

internal static class StringExtension
{
    /// <summary>
    /// Cleans a given string to remove extra spaces, command characters, "&amp;"
    /// </summary>
    /// <param name="strToClean">the string being cleaned</param>
    /// <returns>A cleaned string</returns>
    internal static string Clean(this string strToClean)
    {
        strToClean = RemoveExtraSpaces(strToClean);
        strToClean = RemoveEscapeCharacters(strToClean);
        strToClean = ReplaceAmp(strToClean);
        return strToClean.Trim();
    }

    private static string RemoveExtraSpaces(string strToClean)
    {
        var words = strToClean.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        return string.Join(" ", words);
    }

    private static string RemoveEscapeCharacters(string strToClean)
    {
        var result = new StringBuilder();

        foreach (var character in strToClean.Where(character => !char.IsControl(character)))
        {
            result.Append(character);
        }

        return result.ToString();
    }

    private static string ReplaceAmp(string strToClean)
    {
        return strToClean.Replace("&amp;", "and");
    }
}