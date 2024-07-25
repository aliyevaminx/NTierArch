using System.Text.RegularExpressions;

namespace Core.Extensions;

public static class StringExtensions
{
    public static bool IsValidChoice(this string choice)
    {
        if (choice == "y" || choice == "n")
            return true;
        return false;
    }

    public static bool IsValidEmail(this string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
}
