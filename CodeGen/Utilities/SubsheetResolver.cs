using System.Xml.Linq;

namespace BPAnalyzer.CodeGen.Utilities;

/// <summary>
/// Resolves BluePrism subsheet information from XML documents.
/// </summary>
public static class SubsheetResolver
{
    /// <summary>
    /// Finds the subsheet name from the document by looking up the subsheet ID.
    /// </summary>
    public static string? FindSubsheetName(XDocument? doc, string? subsheetId)
    {
        if (doc == null || string.IsNullOrEmpty(subsheetId))
            return null;

        // Look for subsheet element with matching subsheetid
        var subsheet = doc.Descendants()
            .FirstOrDefault(e => e.Name.LocalName == "subsheet" 
                && e.Attribute("subsheetid")?.Value == subsheetId);

        return subsheet?.Element("name")?.Value;
    }
}
