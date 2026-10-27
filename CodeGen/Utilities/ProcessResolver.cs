using System.Xml.Linq;

namespace BPAnalyzer.CodeGen.Utilities;

/// <summary>
/// Resolves BluePrism process information from XML files.
/// </summary>
public static class ProcessResolver
{
    // Directory that is searched for the process definition. Set by the code generator
    // so that the lookup uses the XML directory passed via --xml= and not the
    // current working directory.
    private static string? _xmlSearchDirectory;

    /// <summary>
    /// Sets the directory that is searched for process definitions.
    /// </summary>
    public static void SetXmlSearchDirectory(string? xmlDirectory)
    {
        _xmlSearchDirectory = string.IsNullOrWhiteSpace(xmlDirectory) ? null : xmlDirectory;
    }

    /// <summary>
    /// Finds the process name by searching for the processid in all XML files.
    /// </summary>
    public static string? FindProcessNameById(string? processId)
    {
        if (string.IsNullOrEmpty(processId)) return null;

        foreach (var xmlDirectory in SearchDirectories())
        {
            var processName = SearchInDirectory(xmlDirectory, processId);
            if (processName != null) return processName;
        }

        return null;
    }

    /// <summary>
    /// Directories that are searched, in order: the configured XML directory,
    /// then the default "xml" folder in the current working directory.
    /// </summary>
    private static IEnumerable<string> SearchDirectories()
    {
        if (!string.IsNullOrWhiteSpace(_xmlSearchDirectory))
            yield return _xmlSearchDirectory;

        yield return Path.Combine(Directory.GetCurrentDirectory(), "xml");
    }

    private static string? SearchInDirectory(string xmlDirectory, string processId)
    {
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
