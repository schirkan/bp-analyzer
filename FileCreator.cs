using System;
using System.IO;

public class FileCreator
{
  public void CreateFile()
  {
    // Aktuelles Arbeitsverzeichnis ermitteln
    string currentDirectory = Directory.GetCurrentDirectory();

    Console.WriteLine("=== Datei-Ersteller ===");
    Console.WriteLine($"Aktuelles Verzeichnis: {currentDirectory}");
    Console.WriteLine();

    // Benutzer nach dem Dateinamen fragen
    Console.Write("Bitte geben Sie einen Dateinamen ein: ");
    string? fileName = Console.ReadLine();

    // Überprüfen, ob ein Dateiname eingegeben wurde
    if (string.IsNullOrWhiteSpace(fileName))
    {
      Console.WriteLine("Fehler: Bitte geben Sie einen gültigen Dateinamen ein.");
      return;
    }

    // Vollständigen Pfad erstellen
    string filePath = Path.Combine(currentDirectory, fileName);

    try
    {
      // Leere Datei erstellen
      File.Create(filePath).Close();

      Console.WriteLine($"Erfolg! Die Datei '{fileName}' wurde erstellt.");
      Console.WriteLine($"Pfad: {filePath}");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Fehler beim Erstellen der Datei: {ex.Message}");
    }

    Console.WriteLine("\nDrücken Sie eine beliebige Taste zum Beenden...");
    Console.ReadKey();
  }
}
