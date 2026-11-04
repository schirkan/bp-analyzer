using System.Text.Json;
using BPAnalyzer.CodeGen;

namespace BPAnalyzer;

/// <summary>
/// Optional configuration for the analyzer, read from "bp-analyzer.config.json"
/// next to the executable.
///
/// The file is optional - without it the built-in defaults apply. The export mode
/// needs a path to Blue Prism's AutomateC.exe; every other mode works without any
/// configuration at all.
/// </summary>
public class AnalyzerConfig
{
    /// <summary>
    /// Path to Blue Prism's AutomateC.exe. Required for the export mode.
    /// The default is the standard Blue Prism installation path.
    /// </summary>
    public string? AutomateCPath { get; set; }

    /// <summary>
    /// Default path of Blue Prism's AutomateC.exe.
    /// </summary>
    public const string DefaultAutomateCPath =
        @"C:\Program Files\Blue Prism Limited\Blue Prism Automate\AutomateC.exe";

    /// <summary>
    /// Name of the configuration file.
    /// </summary>
    public const string FileName = "bp-analyzer.config.json";

    /// <summary>
    /// Loads the configuration file if it exists next to the executable.
    /// Returns null when there is no file or it cannot be read.
    /// </summary>
    /// <param name="directory">Directory to look in (default: the executable's directory).</param>
    public static AnalyzerConfig? Load(string? directory = null)
    {
        foreach (var path in CandidatePaths(directory))
        {
            if (!File.Exists(path))
                continue;

            try
            {
                var json = File.ReadAllText(path);
                var config = JsonSerializer.Deserialize(json, CodeGenJsonContext.Default.AnalyzerConfig);
                if (config != null)
                    return config;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Warning: {path} could not be read ({ex.Message}). Using defaults.");
                return null;
            }
        }

        return null;
    }

    /// <summary>
    /// Locations that are searched for the configuration file, in order:
    /// the executable's directory, then the current working directory.
    /// </summary>
    private static IEnumerable<string> CandidatePaths(string? directory)
    {
        var dir = string.IsNullOrWhiteSpace(directory) ? AppContext.BaseDirectory : directory;
        yield return Path.Combine(dir, FileName);

        var cwd = Directory.GetCurrentDirectory();
        if (!string.Equals(cwd, dir, StringComparison.OrdinalIgnoreCase))
            yield return Path.Combine(cwd, FileName);
    }
}
