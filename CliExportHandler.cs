namespace BPAnalyzer
{
  public class CliExportHandler
  {
    /// <summary>
    /// Handles the export of a Blue Prism process to a file (Console UI)
    /// </summary>

    public static void Run(string[] args)
    {
      string? processName = null;
      string? outputPath = null;
      string? username = null;
      string? password = null;
      bool? overwrite = null;
      var interactive = args.Length == 0;

      // Parse arguments
      for (int i = 1; i < args.Length; i++)
      {
        string arg = args[i];
        // Named arguments
        if (arg.StartsWith("--process=", StringComparison.OrdinalIgnoreCase))
          processName = arg.Substring("--process=".Length);
        else if (arg.StartsWith("--output=", StringComparison.OrdinalIgnoreCase))
          outputPath = arg.Substring("--output=".Length);
        else if (arg.StartsWith("--user=", StringComparison.OrdinalIgnoreCase))
          username = arg.Substring("--user=".Length);
        else if (arg.StartsWith("--password=", StringComparison.OrdinalIgnoreCase))
          password = arg.Substring("--password=".Length);
        else if (arg.StartsWith("--overwrite=", StringComparison.OrdinalIgnoreCase))
          overwrite = arg.Substring("--overwrite=".Length).Equals("yes", StringComparison.OrdinalIgnoreCase);
        // Positional arguments
        else if (!arg.StartsWith("-"))
        {
          if (processName == null) processName = arg;
          else if (outputPath == null) outputPath = arg;
          else if (username == null) username = arg;
          else if (password == null) password = arg;
        }
      }

      // Interactive prompt for missing parameters
      if (interactive && string.IsNullOrWhiteSpace(processName))
      {
        Console.Write("Process name: ");
        processName = Console.ReadLine();
      }
      if (string.IsNullOrWhiteSpace(processName))
      {
        Console.WriteLine("Error: Process name is required.");
        Console.WriteLine();
        PrintUsage();
        Environment.ExitCode = 1;
        return;
      }

      if (interactive && string.IsNullOrWhiteSpace(outputPath))
      {
        Console.Write("Output directory (empty for ./xml): ");
        outputPath = Console.ReadLine();
      }
      if (string.IsNullOrWhiteSpace(outputPath))
      {
        outputPath = Path.Combine(Directory.GetCurrentDirectory(), "xml");
      }

      if (interactive && overwrite == null)
      {
        Console.Write("Overwrite existing files? (yes/no, empty for yes): ");
        var ow = Console.ReadLine();
        overwrite = string.IsNullOrWhiteSpace(ow) || ow.Trim().Equals("yes", StringComparison.OrdinalIgnoreCase);
      }
      if (overwrite == null)
      {
        overwrite = true;
      }

      if (interactive && string.IsNullOrWhiteSpace(username))
      {
        Console.Write("Username (empty for SSO): ");
        username = Console.ReadLine();
      }
      if (interactive && string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(username))
      {
        Console.Write("Password: ");
        password = ReadPassword();
      }

      Console.WriteLine();
      Console.WriteLine("=== Blue Prism Process Export (CLI mode) ===");
      Console.WriteLine($"Process: {processName}");
      Console.WriteLine($"Output directory: {outputPath}");
      Console.WriteLine($"Overwrite: {(overwrite == true ? "yes" : "no")}");
      Console.WriteLine();

      // Execute export
      try
      {
        var exporter = new BluePrismExporter();
        // Optional: set AutomateCPath if needed
        // exporter.AutomateCPath = ...;

        // Ensure output directory exists
        if (!Directory.Exists(outputPath))
          Directory.CreateDirectory(outputPath);

        var results = exporter.ExportWithDependencies(
            processName,
            outputPath,
            username ?? "",
            password ?? "",
            overwrite == true);

        // Output result
        int successCount = results.Count(r => r.Success);
        int failCount = results.Count(r => !r.Success);

        Console.WriteLine("=== Export Result ===");
        Console.WriteLine($"Succeeded: {successCount}");
        Console.WriteLine($"Failed: {failCount}");

        if (successCount > 0)
        {
          Console.WriteLine();
          Console.WriteLine("Exported files:");
          foreach (var result in results.Where(r => r.Success))
            Console.WriteLine($"  - {result.OutputPath}");
        }

        if (failCount > 0)
        {
          Console.WriteLine();
          Console.WriteLine("Failed exports:");
          foreach (var result in results.Where(r => !r.Success))
            Console.WriteLine($"  - {result.OutputPath}: {result.ErrorMessage}");
        }

        Environment.ExitCode = failCount > 0 ? 1 : 0;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error: {ex.Message}");
        Environment.ExitCode = 1;
      }
    }

    // Reads a password from the console without echoing
    private static string ReadPassword()
    {
      string password = "";
      ConsoleKeyInfo key;
      do
      {
        key = Console.ReadKey(true);
        if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
        {
          password += key.KeyChar;
          Console.Write("*");
        }
        else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
        {
          password = password.Substring(0, password.Length - 1);
          Console.Write("\b \b");
        }
      }
      while (key.Key != ConsoleKey.Enter);
      Console.WriteLine();
      return password;
    }

    /// <summary>
    /// Prints usage information for the export mode
    /// </summary>
    public static void PrintUsage()
    {
      Console.WriteLine("Usage:");
      Console.WriteLine("  BP-Analyzer export --process=<name> [--output=<path>] [--user=<username>] [--password=<password>] [--overwrite=<yes|no>]");
      Console.WriteLine();
      Console.WriteLine("Options:");
      Console.WriteLine("  --process=<name>     Name of the process to export (required)");
      Console.WriteLine("  --output=<path>      Output directory (default: xml/)");
      Console.WriteLine("  --user=<username>    Blue Prism username");
      Console.WriteLine("  --password=<password> Blue Prism password");
      Console.WriteLine("  --overwrite=<yes|no> Overwrite existing files (default: yes)");
      Console.WriteLine();
      Console.WriteLine("Example:");
      Console.WriteLine("  BP-Analyzer export --process \"MyProcess\" --output \"C:\\Exports\"");
    }
  }
}
