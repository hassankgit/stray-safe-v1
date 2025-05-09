using System.Text.RegularExpressions;

namespace StraySafe.Data.Utilities;

public static class EnumExtensions
{
    private static readonly Dictionary<string, string> NumberWords = new(StringComparer.OrdinalIgnoreCase)
    {
        { "zero", "0" },
        { "one", "1" },
        { "two", "2" },
        { "three", "3" },
        { "four", "4" },
        { "five", "5" },
        { "six", "6" },
        { "seven", "7" },
        { "eight", "8" },
        { "nine", "9" },
        { "ten", "10" },
        { "eleven", "11" },
        { "twelve", "12" },
        {"plus", "+" }
    };

    public static string ToLabel(this Enum value)
    {
        string? baseLabel = value.ToString().ToLowerInvariant().Replace("_", " ");
        return ReplaceUnwantedCharacters(baseLabel);
    }

    private static string ReplaceUnwantedCharacters(string input)
    {
        foreach (KeyValuePair<string, string> kvp in NumberWords)
        {
            input = Regex.Replace(input, $@"\b{kvp.Key}\b", kvp.Value, RegexOptions.IgnoreCase);
        }
        return input;
    }
}
