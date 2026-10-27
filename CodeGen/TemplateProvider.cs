namespace BPAnalyzer.CodeGen;

/// <summary>
/// Provides template content for the code generator and the SDD generator.
///
/// Lookup order:
/// 1) file on disk (base directory / current directory) - keeps the existing
///    developer workflow intact and allows overriding templates locally,
/// 2) embedded resource from the assembly - makes the single-file build
///    self-contained, no "templates" folder needed next to the executable.
///
/// The embedded copy is byte-identical to the file in the templates/ folder.
/// </summary>
public static class TemplateProvider
{
    private const string ResourcePrefix = "BPAnalyzer.templates.";

    /// <summary>
    /// Returns the template content for the given file name, or null if it cannot be found.
    /// </summary>
    /// <param name="fileName">File name, e.g. "Template_BP_Base.vb".</param>
    public static string? TryGetTemplateText(string fileName)
    {
        foreach (var candidate in DiskCandidates(fileName))
        {
            if (File.Exists(candidate))
                return File.ReadAllText(candidate);
        }

        return TryReadEmbedded(fileName);
    }

    /// <summary>
    /// Returns the template content for the given path (e.g. "templates/SDD_Process_Template.md").
    /// The path is tried as given first, then by file name via the regular lookup.
    /// </summary>
    /// <param name="relativePath">Path to the template.</param>
    public static string? TryGetTemplateTextFromPath(string relativePath)
    {
        if (!string.IsNullOrWhiteSpace(relativePath) && File.Exists(relativePath))
            return File.ReadAllText(relativePath);

        var fileName = Path.GetFileName(relativePath);
        if (string.IsNullOrEmpty(fileName))
            return null;

        return TryGetTemplateText(fileName);
    }

    private static IEnumerable<string> DiskCandidates(string fileName)
    {
        yield return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "templates", fileName);
        yield return Path.Combine(Directory.GetCurrentDirectory(), "templates", fileName);
        yield return Path.Combine(Directory.GetCurrentDirectory(), fileName);
    }

    private static string? TryReadEmbedded(string fileName)
    {
        var resourceName = ResourcePrefix + fileName;
        using var stream = typeof(TemplateProvider).Assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
            return null;

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
