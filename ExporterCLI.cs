using System;
using System.IO;

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
      targetPath = Path.Combine(Directory.GetCurrentDirectory(), processName + ".xml");
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
          targetPath = Path.Combine(Directory.GetCurrentDirectory(), processName + ".xml");
          Console.WriteLine($"Warnung: Angegebener Pfad nicht gefunden. Standardpfad wird verwendet: {targetPath}");
        }
      }
    }

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

    // Export durchführen
    Console.WriteLine();
    Console.WriteLine("Exportiere Prozess...");

    var result = _exporter.Export(processName, targetPath, username ?? "", password ?? "");

    // Ausgabe des Ergebnisses
    if (!string.IsNullOrWhiteSpace(result.StandardOutput))
    {
      Console.WriteLine("Ausgabe:");
      Console.WriteLine(result.StandardOutput);
    }

    if (!string.IsNullOrWhiteSpace(result.StandardError))
    {
      Console.WriteLine("Fehler:");
      Console.WriteLine(result.StandardError);
    }

    if (result.Success)
    {
      Console.WriteLine();
      Console.WriteLine($"Erfolg! Prozess '{processName}' wurde exportiert nach:");
      Console.WriteLine(targetPath);
    }
    else
    {
      Console.WriteLine();
      Console.WriteLine($"Fehler beim Exportieren. Exit-Code: {result.ExitCode}");
      if (!string.IsNullOrEmpty(result.ErrorMessage))
      {
        Console.WriteLine(result.ErrorMessage);
      }
    }

    Console.WriteLine();
    Console.WriteLine("Drücken Sie eine beliebige Taste zum Beenden...");
    Console.ReadKey();
  }

  /// <summary>
  /// Führt einen Export mit direkten Parametern aus (für Automatisierung)
  /// </summary>
  public bool Export(string processName, string outputPath, string username, string password)
  {
    var result = _exporter.Export(processName, outputPath, username, password);
    return result.Success;
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
