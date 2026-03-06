using System.Xml.Linq;

namespace BPAnalyzer.CodeGen.Utilities;

/// <summary>
/// Resolves BluePrism process information from XML files.
/// </summary>
public static class ProcessResolver
{
    /// <summary>
    /// Finds the process name by searching for the processid in all XML files.
    /// </summary>
    public static string? FindProcessNameById(string? processId)
    {
        if (string.IsNullOrEmpty(processId)) return null;

        var xmlDirectory = Path.Combine(Directory.GetCurrentDirectory(), "xml");
        if (!Directory.Exists(xmlDirectory)) return null;

        var xmlFiles = Directory.GetFiles(xmlDirectory, "*.xml");
        foreach (var xmlFile in xmlFiles)
        {
            try
            {
                var doc = XDocument.Load(xmlFile);
                var process = doc.Root;
                if (process?.Name.LocalName != "process") continue;

                var preferredId = process.Attribute("preferredid")?.Value;
                if (preferredId == processId)
                {
                    return process.Attribute("name")?.Value;
                }
            }
            catch
            {
                // Skip invalid XML files
            }
        }

        return null;
    }
}
