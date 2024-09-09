using System.Text.RegularExpressions;

namespace HomeStream.Application.Common;

public static class StringExtensions
{
    public static string ToHumanTitleCase(this string str)
    {
        string result = Regex.Replace(str, "([a-z])([A-Z])", "$1 $2");
        return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result.ToLower());
    }

    public static string ThrowExceptionIfNullOrEmptyWithError(this string? str, string message)
    {
        if (string.IsNullOrEmpty(str))
        {
            throw new ArgumentException(message);
        }
        return str;
    }
}
