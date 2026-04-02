namespace BPAnalyzer
{
  public class CliAnalyzeHandler
  {
    /// <summary>
    /// Handles the analyze mode
    /// </summary>
    public static void Run(string[] args)
    {
      string? codeDir = null;
      string? outputDir = null;
      string templatePath = "templates/SDD_Process_Template.md";
      List<string> excludeExceptionSources = new() { "ERGO_", "Utility_" };

      // Argument Parsing
      for (int i = 1; i < args.Length; i++)
      {
        string arg = args[i];
        if (arg.StartsWith("--code=", StringComparison.OrdinalIgnoreCase))
          codeDir = arg.Substring("--code=".Length);
        else if (arg.StartsWith("--output=", StringComparison.OrdinalIgnoreCase))
          outputDir = arg.Substring("--output=".Length);
        else if (arg.StartsWith("--exclude-exception-sources=", StringComparison.OrdinalIgnoreCase))
        {
          var val = arg.Substring("--exclude-exception-sources=".Length);
          excludeExceptionSources = val.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        }
        else if (!arg.StartsWith("-"))
        {
          if (codeDir == null) codeDir = arg;
          else if (outputDir == null) outputDir = arg;
        }
      }

      // Interactive prompt if codeDir is missing
      if (string.IsNullOrWhiteSpace(codeDir))
      {
        Console.Write("Code directory with JSON files (empty for ./code): ");
        codeDir = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(codeDir))
          codeDir = "code";
      }

      // Interactive prompt if outputDir is missing
      if (string.IsNullOrWhiteSpace(outputDir))
      {
        Console.Write("Output directory for SDDs (empty for ./sdd): ");
        outputDir = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(outputDir))
          outputDir = "sdd";
      }

      // Ensure output directory exists (after outputDir is set)
      if (!Directory.Exists(outputDir))
      {
        Directory.CreateDirectory(outputDir);
      }

      // JSON files are read from codeDir
      string classesPath = Path.Combine(codeDir, "classes.json");
      string dependenciesPath = Path.Combine(codeDir, "dependencies.json");
      string exceptionsPath = Path.Combine(codeDir, "exceptions.json");

      // Load JSON files
      if (!File.Exists(classesPath) || !File.Exists(dependenciesPath) || !File.Exists(exceptionsPath))
      {
        Console.WriteLine("Required JSON files (classes.json, dependencies.json, exceptions.json) not found in code directory.");
        Environment.ExitCode = 1;
        return;
      }

      try
      {
        var results = BluePrismAnalyzer.GenerateSdds(codeDir, outputDir, templatePath, excludeExceptionSources);
        foreach (var sdd in results)
        {
          File.WriteAllText(sdd.OutputPath, sdd.Content);
          Console.WriteLine($"Generated: {sdd.OutputPath}");
        }
        Console.WriteLine("Analyze completed. SDD files generated for all processes in code directory.");
        Environment.ExitCode = 0;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Analyze failed: {ex.Message}");
        Environment.ExitCode = 1;
      }
    }

    /// <summary>
    /// Prints usage information for the analyze mode
    /// </summary>
    public static void PrintUsage()
    {
      Console.WriteLine("Usage:");
      Console.WriteLine("  BP-Analyzer analyze [--code=<json-dir>] [--output=<sdd-dir>] [--exclude-exception-sources=<prefix1,prefix2,...>]");
      Console.WriteLine();
      Console.WriteLine("--code:    Directory with classes.json, dependencies.json, exceptions.json (default: ./code)");
      Console.WriteLine("--output:  Target directory for SDD files (default: ./sdd)");
      Console.WriteLine("--exclude-exception-sources:  Kommagetrennte Liste von Prefixen, deren Exceptions ausgeschlossen werden (default: ERGO_,Utility_)");
    }
  }
}
