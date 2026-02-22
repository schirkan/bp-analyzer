using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

public class ProcessExporter
{
  // Standardpfad für Blue Prism AutomateC (kann angepasst werden)
  private string _automateCPath;

  public ProcessExporter()
  {
    _automateCPath = "C:\\Program Files\\Blue Prism Limited\\Blue Prism Automate\\AutomateC.exe";
  }

  public ProcessExporter(string automateCPath)
  {
    _automateCPath = automateCPath;
  }

  /// <summary>
  /// Exportiert einen Blue Prism Prozess in eine Datei
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
      _automateCPath = automatePath;
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
      // Passwort nicht in der Konsole anzeigen
      password = ReadPassword();
    }

    // Export durchführen
    Console.WriteLine();
    Console.WriteLine("Exportiere Prozess...");

    try
    {
      bool success = ExecuteExport(processName, targetPath, username, password);

      if (success)
      {
        Console.WriteLine();
        Console.WriteLine($"Erfolg! Prozess '{processName}' wurde exportiert nach:");
        Console.WriteLine(targetPath);
      }
      else
      {
        Console.WriteLine();
        Console.WriteLine("Fehler beim Exportieren des Prozesses.");
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Fehler: {ex.Message}");
    }

    Console.WriteLine();
    Console.WriteLine("Drücken Sie eine beliebige Taste zum Beenden...");
    Console.ReadKey();
  }

  /// <summary>
  /// Führt den Export-Befehl aus
  /// </summary>
  private bool ExecuteExport(string processName, string outputPath, string? username, string? password)
  {
    // Befehl zusammenbauen
    // AutomateC /export "ProcessName" /user username password
    string arguments;
    string safeArguments;

    if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
    {
      arguments = $"/export \"{processName}\" /user \"{username}\" \"{password}\"";
      safeArguments = $"/export \"{processName}\" /user \"{username}\" \"******\"";
    }
    else
    {
      // Versuchen mit SSO (Single Sign-On)
      arguments = $"/export \"{processName}\" /sso";
      safeArguments = arguments;
    }

    Console.WriteLine($"Ausführen: {_automateCPath} {safeArguments}");
    Console.WriteLine();

    // Temp-Verzeichnis als Working Directory verwenden
    string tempDirectory = Path.GetTempPath();

    // Vor dem Export: Mögliche existierende Dateien löschen
    // Blue Prism exportiert entweder als "BPA Object - <name>.xml" oder "BPA Process - <name>.xml"
    string[] possibleFileNames = new[]
    {
      $"BPA Object - {processName}.xml",
      $"BPA Process - {processName}.xml"
    };

    Console.WriteLine("Lösche existierende Dateien im Temp-Verzeichnis...");
    foreach (string fileName in possibleFileNames)
    {
      string filePath = Path.Combine(tempDirectory, fileName);
      if (File.Exists(filePath))
      {
        try
        {
          File.Delete(filePath);
          Console.WriteLine($"  Gelöscht: {fileName}");
        }
        catch (Exception ex)
        {
          Console.WriteLine($"  Warnung: Konnte {fileName} nicht löschen: {ex.Message}");
        }
      }
    }
    Console.WriteLine();

    ProcessStartInfo psi = new ProcessStartInfo
    {
      FileName = _automateCPath,
      Arguments = arguments,
      WorkingDirectory = tempDirectory,
      UseShellExecute = false,
      RedirectStandardOutput = true,
      RedirectStandardError = true,
      CreateNoWindow = true
    };

    using (Process? process = Process.Start(psi))
    {
      if (process == null)
      {
        Console.WriteLine("Fehler: Prozess konnte nicht gestartet werden.");
        return false;
      }

      // Ausgabe lesen
      string output = process.StandardOutput.ReadToEnd();
      string error = process.StandardError.ReadToEnd();

      process.WaitForExit();

      // Ausgabe anzeigen
      if (!string.IsNullOrWhiteSpace(output))
      {
        Console.WriteLine("Ausgabe:");
        Console.WriteLine(output);
      }

      if (!string.IsNullOrWhiteSpace(error))
      {
        Console.WriteLine("Fehler:");
        Console.WriteLine(error);
      }

      bool success = process.ExitCode == 0;

      if (success)
      {
        Console.WriteLine($"Exit-Code: {process.ExitCode} (Erfolg)");

        // Datei von Temp in das endgültige Zielverzeichnis verschieben
        try
        {
          // Zielverzeichnis sicherstellen
          string? targetDir = Path.GetDirectoryName(outputPath);
          if (!string.IsNullOrEmpty(targetDir) && !Directory.Exists(targetDir))
          {
            Directory.CreateDirectory(targetDir);
          }

          // Finde die exportierte Datei im Temp-Verzeichnis
          string? exportedFilePath = null;
          foreach (string fileName in possibleFileNames)
          {
            string filePath = Path.Combine(tempDirectory, fileName);
            if (File.Exists(filePath))
            {
              exportedFilePath = filePath;
              Console.WriteLine($"Gefundene exportierte Datei: {fileName}");
              break;
            }
          }

          if (exportedFilePath != null)
          {
            // Vorhandene Zieldatei löschen
            if (File.Exists(outputPath))
            {
              File.Delete(outputPath);
            }
            File.Move(exportedFilePath, outputPath);
            Console.WriteLine($"Datei verschoben nach: {outputPath}");
          }
          else
          {
            Console.WriteLine($"Fehler: Keine exportierte Datei im Temp-Verzeichnis gefunden.");
            Console.WriteLine("Mögliche Dateinamen:");
            foreach (string fileName in possibleFileNames)
            {
              Console.WriteLine($"  - {fileName}");
            }
            success = false;
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Fehler beim Verschieben der Datei: {ex.Message}");
          success = false;
        }
      }
      else
      {
        Console.WriteLine($"Exit-Code: {process.ExitCode} (Fehler)");
      }

      return success;
    }
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

  /// <summary>
  /// Führt einen Export mit direkten Parametern aus (für Automatisierung)
  /// </summary>
  public bool Export(string processName, string outputPath, string username, string password)
  {
    if (string.IsNullOrWhiteSpace(processName))
    {
      throw new ArgumentException("Prozessname darf nicht leer sein.", nameof(processName));
    }

    if (string.IsNullOrWhiteSpace(outputPath))
    {
      throw new ArgumentException("Ausgabepfad darf nicht leer sein.", nameof(outputPath));
    }

    return ExecuteExport(processName, outputPath, username, password);
  }
}
