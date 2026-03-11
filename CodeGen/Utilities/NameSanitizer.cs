using System.Text.RegularExpressions;

namespace BPAnalyzer.CodeGen.Utilities;

/// <summary>
/// Provides methods to sanitize names for VB.NET code generation.
/// Ensures valid VB.NET identifiers by removing/replacing invalid characters.
/// </summary>
public static class NameSanitizer
{
    /// <summary>
    /// Sanitizes a class name to be a valid VB.NET identifier.
    /// </summary>
    public static string SanitizeClassName(string name)
    {
        if (string.IsNullOrEmpty(name)) return "";
        var sanitized = SanitizeId(name);
        if (!char.IsLetter(sanitized[0])) sanitized = "_" + sanitized;
        return Regex.Replace(sanitized, "_+", "_");
    }

    /// <summary>
    /// Sanitizes a variable name to be a valid VB.NET identifier.
    /// </summary>
    public static string SanitizeVariableName(string? name)
    {
        if (string.IsNullOrEmpty(name)) return "";
        if (name.Contains("."))
        {
            return ExpressionParser.FormatExpression("[" + name + "]");
        }
        var sanitized = SanitizeId(name);
        if (!char.IsLetter(sanitized[0])) sanitized = "_" + sanitized;
        return Regex.Replace(sanitized, "_+", "_");
    }

    /// <summary>
    /// Sanitizes a method name to be a valid VB.NET identifier.
    /// </summary>
    public static string SanitizeMethodName(string name)
    {
        if (string.IsNullOrEmpty(name)) return "";
        var sanitized = SanitizeId(name);
        if (!char.IsLetter(sanitized[0])) sanitized = "_" + sanitized;
        return Regex.Replace(sanitized, "_+", "_");
    }

    /// <summary>
    /// Sanitizes an ID (stage ID, etc.) to be a valid VB.NET identifier part.
    /// </summary>
    public static string SanitizeId(string id)
    {
        return Regex.Replace(id ?? "", @"[^a-zA-Z0-9_]", "_");
    }
}
