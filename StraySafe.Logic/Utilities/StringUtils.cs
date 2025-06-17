namespace StraySafe.Logic.Utilities;

public static class StringUtils
{
    public static string? NullIfWhiteSpace(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }
        return value;
    }
}