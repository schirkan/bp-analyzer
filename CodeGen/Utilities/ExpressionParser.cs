using System.Text.RegularExpressions;

namespace BPAnalyzer.CodeGen.Utilities;

/// <summary>
/// Parses and converts BluePrism expressions to valid VB.NET code.
/// </summary>
public static class ExpressionParser
{
    /// <summary>
    /// Converts BluePrism expressions to valid VB.NET code:
    /// - Removes square brackets [Variable] -> Variable
    /// - Handles Collection column access: Collection.Column -> Collection.CurrentRow("Column")
    /// - Supports nested collections: Collection.Column.SubColumn -> Collection.CurrentRow("Column").CurrentRow("SubColumn")
    /// - Supports spaces in column names: Collection.Column Name -> Collection.CurrentRow("Column Name")
    /// - Sanitizes variable names (replaces invalid chars with _)
    /// </summary>
    public static string FormatExpression(string? expression)
    {
        if (string.IsNullOrWhiteSpace(expression)) return "";

        var result = expression;

        // Handle Collection column access with multiple levels: [Collection.Column1.Column2.Column3]
        // Becomes: Collection.CurrentRow("Column1").CurrentRow("Column2").CurrentRow("Column3")
        // Note: Column names can have spaces, collection names cannot
        result = Regex.Replace(result ?? "", @"\[([a-zA-Z_][a-zA-Z0-9_]*)(\.[^\]]+)+\]", match =>
        {
            var fullMatch = match.Groups[0].Value;
            // Remove the outer brackets and split by dots
            var content = fullMatch.Trim('[', ']');
            var parts = content.Split('.');

            if (parts.Length == 0) return match.Value;

            var collectionName = NameSanitizer.SanitizeVariableName(parts[0]);
            var vbCode = collectionName;

            // Add CurrentRow for each subsequent part (column name)
            for (int i = 1; i < parts.Length; i++)
            {
                var columnName = parts[i].Trim();
                vbCode += $".CurrentRow(\"{columnName}\")";
            }

            return vbCode;
        });

        // Then, handle simple variables: [Variable] -> Variable
        result = Regex.Replace(result ?? "", @"\[([^\]]+)\]", match =>
        {
            var varName = match.Groups[1].Value;
            // Check if it contains a dot (already handled above)
            if (varName.Contains('.')) return match.Value; // Keep original if not handled
            return NameSanitizer.SanitizeVariableName(varName);
        });

        return result;
    }
}
