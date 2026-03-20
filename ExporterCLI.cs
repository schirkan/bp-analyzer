public class ExporterCLI
{
  private BluePrismExporter _exporter;

  public ExporterCLI()
  {
    _exporter = new BluePrismExporter();
  }

  /// <summary>
  /// Exportiert einen Blue Prism Prozess in eine Datei (Console UI)
  /// </summary>
  public void ExportProcess()
  {
    Console.WriteLine("=== Blue Prism Prozess-Export ===");
    Console.WriteLine();

    // Blue Prism Pfad abfragen (optional)
    Console.Write("Pfad zu AutomateC.exe (leer für Standard): ");
    string? automatePath = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(automatePath))
    {
      _exporter.AutomateCPath = automatePath;
    }

    // Prozessname abfragen
    Console.Write("Prozessname: ");
    string? processName = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(processName))
    {
      Console.WriteLine("Fehler: Bitte geben Sie einen gültigen Prozessnamen ein.");
      return;
    }

    // Ausgabepfad abfragen
    Console.Write("Ausgabepfad (leer für aktuelles Verzeichnis): ");
    string? outputPath = Console.ReadLine();

    string targetPath;
    if (string.IsNullOrWhiteSpace(outputPath))
    {
      targetPath = Path.Combine(Directory.GetCurrentDirectory(), "xml", processName + ".xml");
    }
    else
    {
      // Prüfen ob ein Verzeichnis oder Dateipfad angegeben wurde
      if (Directory.Exists(outputPath))
      {
        targetPath = Path.Combine(outputPath, processName + ".xml");
      }
      else if (File.Exists(outputPath))
      {
        targetPath = outputPath;
      }
      else
      {
        // Prüfen, ob das übergeordnete Verzeichnis existiert
        string? parentDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(parentDir) && Directory.Exists(parentDir))
        {
          targetPath = outputPath;
        }
        else
        {
          targetPath = Path.Combine(Directory.GetCurrentDirectory(), "xml", processName + ".xml");
          Console.WriteLine($"Warnung: Angegebener Pfad nicht gefunden. Standardpfad wird verwendet: {targetPath}");
        }
      }
    }

    // Overwrite-Option abfragen
    Console.Write("Vorhandene Dateien überschreiben? (yes/no, leer für no): ");
    string? overwriteInput = Console.ReadLine();
    bool overwrite = !string.IsNullOrWhiteSpace(overwriteInput) && overwriteInput.Trim().Equals("yes", StringComparison.OrdinalIgnoreCase);

    // Authentifizierung abfragen
    Console.WriteLine();
    Console.WriteLine("Authentifizierung:");
    Console.Write("Benutzername: ");
    string? username = Console.ReadLine();

    string? password = null;
    if (!string.IsNullOrWhiteSpace(username))
    {
      Console.Write("Passwort: ");
      password = ReadPassword();
    }

    // Export durchführen (mit rekursiven Abhängigkeiten)
    Console.WriteLine();
    Console.WriteLine("Exportiere Prozess und Abhängigkeiten...");

    // Für einzelne Datei-Export verwenden wir das Ausgabeverzeichnis
    string outputDirectory = Path.GetDirectoryName(targetPath) ?? Directory.GetCurrentDirectory();
    var results = _exporter.ExportWithDependencies(processName, outputDirectory, username ?? "", password ?? "", overwrite);

    // Zusammenfassung ausgeben
    int successCount = results.Count(r => r.Success);
    int failCount = results.Count(r => !r.Success);

    Console.WriteLine();
    Console.WriteLine("=== Export-Ergebnis ===");
    Console.WriteLine($"Erfolgreich: {successCount}");
    Console.WriteLine($"Fehlgeschlagen: {failCount}");

    if (successCount > 0)
    {
      Console.WriteLine();
      Console.WriteLine("Exportierte Dateien:");
      foreach (var result in results.Where(r => r.Success))
      {
        Console.WriteLine($"  - {result.OutputPath}");
      }
    }

    if (failCount > 0)
    {
      Console.WriteLine();
      Console.WriteLine("Fehlgeschlagene Exporte:");
      foreach (var result in results.Where(r => !r.Success))
      {
        Console.WriteLine($"  - {result.OutputPath}: {result.ErrorMessage}");
      }
    }

    Console.WriteLine();
    Console.WriteLine("Drücken Sie eine beliebige Taste zum Beenden...");
    Console.ReadKey();
  }

  /// <summary>
  /// Führt einen Export mit direkten Parametern aus (für Automatisierung)
  /// </summary>
  public bool Export(string processName, string outputPath, string username, string password, bool overwrite = false)
  {
    string outputDirectory = Path.GetDirectoryName(outputPath) ?? Directory.GetCurrentDirectory();
    var results = _exporter.ExportWithDependencies(processName, outputDirectory, username, password, overwrite);
    return results.All(r => r.Success);
  }

  /// <summary>
  /// Führt einen rekursiven Export mit allen Abhängigkeiten durch
  /// </summary>
  public List<BluePrismExporter.ExportResult> ExportWithDependencies(string processName, string outputDirectory, string username, string password, bool overwrite = false)
  {
    return _exporter.ExportWithDependencies(processName, outputDirectory, username, password, overwrite);
  }

  /// <summary>
  /// Liest ein Passwort ohne es in der Konsole anzuzeigen
  /// </summary>
  private string ReadPassword()
  {
    string password = "";
    ConsoleKeyInfo key;

    do
    {
      key = Console.ReadKey(true);

      // Backspace nicht löschen
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
}
