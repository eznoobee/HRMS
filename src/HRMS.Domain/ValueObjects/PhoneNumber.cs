using System.Text.RegularExpressions;

namespace HRMS.Domain.ValueObjects;

public sealed record PhoneNumber
{
    public string Value { get; }

    private PhoneNumber(string value) => Value = value;

    /// <summary>
    /// Accepts Iraqi local format (07XXXXXXXXX) or international (+964XXXXXXXXX / 00964XXXXXXXXX).
    /// Always stores as E.164 (+964XXXXXXXXX).
    /// </summary>
    public static PhoneNumber Create(string raw)
    {
        var cleaned = Regex.Replace(raw.Trim(), @"[\s\-\(\)]", "");

        string normalized;
        if (cleaned.StartsWith("00964"))
            normalized = "+" + cleaned[2..];
        else if (cleaned.StartsWith("+964"))
            normalized = cleaned;
        else if (cleaned.StartsWith("964"))
            normalized = "+" + cleaned;
        else if (cleaned.StartsWith("0"))
            normalized = "+964" + cleaned[1..];
        else
            normalized = cleaned;

        if (!Regex.IsMatch(normalized, @"^\+\d{10,14}$"))
            throw new ArgumentException(
                $"'{raw}' is not a valid phone number. Use format 07XXXXXXXXX or +964XXXXXXXXX.");

        return new PhoneNumber(normalized);
    }

    public static PhoneNumber? TryCreate(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return null;
        try { return Create(raw); }
        catch { return null; }
    }

    public static bool IsValid(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return false;
        try { Create(raw); return true; }
        catch { return false; }
    }

    public override string ToString() => Value;
}
