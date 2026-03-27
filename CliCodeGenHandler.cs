namespace BPAnalyzer
{
  public class CliCodeGenHandler
  {
    /// <summary>
    /// Handles code generation from Blue Prism XML files
    /// </summary>
    public static void Run(string[] args)
    {
      string? xmlDirectory = null;
      string? outputDirectory = null;

      // Parse arguments
      for (int i = 1; i < args.Length; i++)
      {
        string arg = args[i];
        if (arg.StartsWith("--xml=", StringComparison.OrdinalIgnoreCase))
          xmlDirectory = arg.Substring("--xml=".Length);
        else if (arg.StartsWith("--output=", StringComparison.OrdinalIgnoreCase))
          outputDirectory = arg.Substring("--output=".Length);
        // Positional arguments
        else if (!arg.StartsWith("-"))
        {
          if (xmlDirectory == null) xmlDirectory = arg;
          else if (outputDirectory == null) outputDirectory = arg;
        }
      }

      // Interactive prompt for missing parameters
      if (string.IsNullOrWhiteSpace(xmlDirectory))
      {
        Console.Write("XML directory (empty for ./xml): ");
        xmlDirectory = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(xmlDirectory))
          xmlDirectory = "xml";
      }
      if (string.IsNullOrWhiteSpace(outputDirectory))
      {
        Console.Write("Output directory (empty for ./code): ");
        outputDirectory = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(outputDirectory))
          outputDirectory = "code";
      }

      // Check if XML directory exists
      if (!Directory.Exists(xmlDirectory))
      {
        Console.WriteLine($"Error: XML directory not found: {xmlDirectory}");
        Environment.ExitCode = 1;
        return;
      }

      // Output info
      Console.WriteLine("=== BluePrism VB.NET Code Generation ===");
      Console.WriteLine($"XML directory: {xmlDirectory}");
      Console.WriteLine($"Output directory: {outputDirectory}");
      Console.WriteLine();

      // Run code generation
      try
      {
        var codeGen = new BluePrismCodeGen(outputDirectory);
        codeGen.GenerateAll(xmlDirectory);
        Console.WriteLine();
        Console.WriteLine("Code generation completed!");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error: {ex.Message}");
        Environment.ExitCode = 1;
      }
    }

    /// <summary>
    /// Prints usage information for the codegen mode
    /// </summary>
    public static void PrintUsage()
    {
      Console.WriteLine("Usage:");
      Console.WriteLine("  BP-Analyzer codegen [--xml=<directory>] [--output=<directory>]");
      Console.WriteLine();
      Console.WriteLine("Options:");
      Console.WriteLine("  --xml=<directory>     Directory with BluePrism XML files (default: xml/)");
      Console.WriteLine("  --output=<directory>  Output directory (default: output/)");
      Console.WriteLine();
      Console.WriteLine("Example:");
      Console.WriteLine("  BP-Analyzer codegen --xml=xml --output=output");
    }
  }
}
