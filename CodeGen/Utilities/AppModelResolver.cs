using System.Xml.Linq;

namespace BPAnalyzer.CodeGen.Utilities;

/// <summary>
/// Provides methods to resolve element information from the BluePrism application model (appdef).
/// </summary>
public static class AppModelResolver
{
    /// <summary>
    /// Finds the element name by ID using LINQ (descendants).
    /// </summary>
    /// <param name="appdef">The appdef root element containing element definitions.</param>
    /// <param name="elementId">The element ID to search for.</param>
    /// <returns>The element name if found, otherwise null.</returns>
    public static string? FindElementNameById(XElement? appdef, string? elementId)
    {
        if (appdef == null || string.IsNullOrEmpty(elementId)) return null;

        return appdef.Descendants("element")
            .Where(e => e.Element("id")?.Value.Equals(elementId, StringComparison.OrdinalIgnoreCase) == true)
            .Select(e => e.Attribute("name")?.Value)
            .FirstOrDefault();
    }
}
