using System.Text.RegularExpressions;

namespace HRMS.Domain.ValueObjects;

public sealed record Email
{
    public string Value { get; }

    private Email(string value) => Value = value;

    public static Email Create(string raw)
    {
        var normalized = raw?.Trim().ToLowerInvariant()
            ?? throw new ArgumentNullException(nameof(raw));

        if (!Regex.IsMatch(normalized, @"^[^@\s]+@[^@\s]+\.[^@\s]+$") || normalized.Length > 256)
            throw new ArgumentException($"'{raw}' is not a valid email address.");

        return new Email(normalized);
    }

    public override string ToString() => Value;
}
