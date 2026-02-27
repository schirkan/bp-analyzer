// Blue Prism Prozess-Export Tool
// Exportiert Blue Prism Prozesse mit AutomateC.exe CLI
// Unterstützt rekursiven Export von abhängigen Objekten

using System;
using System.Linq;
using BPAnalyzer;

class Program
{
    static void Main(string[] args)
    {
        ExporterCLI exporter = new ExporterCLI();

        // Wenn Argumente übergeben wurden, verwende den CLI-Modus
        if (args.Length > 0)
        {
            // Prüfen auf Code-Generierungs-Modus
            if (args[0].Equals("--codegen", StringComparison.OrdinalIgnoreCase))
            {
                HandleCodeGen(args);
                return;
            }

            HandleCommandLineArgs(args, exporter);
        }
        else
        {
            // Interaktiver Modus
            exporter.ExportProcess();
        }
    }

    /// <summary>
    /// Behandelt die Code-Generierung aus BluePrism XML-Dateien
    /// </summary>
    static void HandleCodeGen(string[] args)
    {
        string? xmlDirectory = null;
        string? outputDirectory = null;

        for (int i = 1; i < args.Length; i++)
        {
            string arg = args[i];

            if (arg.StartsWith("--xml=", StringComparison.OrdinalIgnoreCase))
            {
                xmlDirectory = arg.Substring("--xml=".Length);
            }
            else if (arg.StartsWith("--output=", StringComparison.OrdinalIgnoreCase))
            {
                outputDirectory = arg.Substring("--output=".Length);
            }
            else if (!arg.StartsWith("-"))
            {
                if (xmlDirectory == null) xmlDirectory = arg;
                else if (outputDirectory == null) outputDirectory = arg;
            }
        }

        if (string.IsNullOrWhiteSpace(xmlDirectory))
        {
            xmlDirectory = "xml";
        }

        if (string.IsNullOrWhiteSpace(outputDirectory))
        {
            outputDirectory = "output";
        }

        if (!System.IO.Directory.Exists(xmlDirectory))
        {
            Console.WriteLine($"Fehler: XML-Verzeichnis nicht gefunden: {xmlDirectory}");
            return;
        }

        Console.WriteLine("=== BluePrism VB.NET Code-Generierung ===");
        Console.WriteLine($"XML-Verzeichnis: {xmlDirectory}");
        Console.WriteLine($"Ausgabeverzeichnis: {outputDirectory}");
        Console.WriteLine();

        try
        {
            var codeGen = new BluePrismCodeGen(outputDirectory);
            codeGen.GenerateAll(xmlDirectory);

            Console.WriteLine();
            Console.WriteLine("Code-Generierung abgeschlossen!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler: {ex.Message}");
            Environment.ExitCode = 1;
        }
    }

    /// <parameter name="summary">
    /// Behandelt Kommandozeilenargumente für den automatisierten Export
    /// </parameter>
    /// <param name="args">Kommandozeilenargumente</param>
    /// <param name="exporter">Die ExporterCLI Instanz</param>
    static void HandleCommandLineArgs(string[] args, ExporterCLI exporter)
    {
        // Argumente parsen
        // Erwartetes Format: [processname] [outputpath] [username] [password] [overwrite]
        // Oder benannte Argumente: --process=xxx --output=xxx --user=xxx --password=xxx --overwrite=yes

        string? processName = null;
        string? outputPath = null;
        string? username = null;
        string? password = null;
        bool overwrite = false;

        // Versuche zuerst benannte Argumente zu parsen
        for (int i = 0; i < args.Length; i++)
        {
            string arg = args[i];

            if (arg.StartsWith("--process=", StringComparison.OrdinalIgnoreCase))
            {
                processName = arg.Substring("--process=".Length);
            }
            else if (arg.StartsWith("--output=", StringComparison.OrdinalIgnoreCase))
            {
                outputPath = arg.Substring("--output=".Length);
            }
            else if (arg.StartsWith("--user=", StringComparison.OrdinalIgnoreCase))
            {
                username = arg.Substring("--user=".Length);
            }
            else if (arg.StartsWith("--password=", StringComparison.OrdinalIgnoreCase))
            {
                password = arg.Substring("--password=".Length);
            }
            else if (arg.StartsWith("--overwrite=", StringComparison.OrdinalIgnoreCase))
            {
                string value = arg.Substring("--overwrite=".Length);
                overwrite = value.Equals("yes", StringComparison.OrdinalIgnoreCase);
            }
            else if (!arg.StartsWith("-"))
            {
                // Positionale Argumente
                if (processName == null) processName = arg;
                else if (outputPath == null) outputPath = arg;
                else if (username == null) username = arg;
                else if (password == null) password = arg;
            }
        }

        // Überprüfen der erforderlichen Argumente
        if (string.IsNullOrWhiteSpace(processName))
        {
            Console.WriteLine("Fehler: Prozessname ist erforderlich.");
            Console.WriteLine();
            PrintUsage();
            return;
        }

        if (string.IsNullOrWhiteSpace(outputPath))
        {
            outputPath = System.IO.Directory.GetCurrentDirectory();
        }

        // Ausführen des Exports
        Console.WriteLine("=== Blue Prism Prozess-Export (CLI-Modus) ===");
        Console.WriteLine($"Prozess: {processName}");
        Console.WriteLine($"Ausgabeverzeichnis: {outputPath}");
        Console.WriteLine($"Überschreiben: {(overwrite ? "ja" : "nein")}");
        Console.WriteLine();

        try
        {
            var results = exporter.ExportWithDependencies(
                processName,
                outputPath,
                username ?? "",
                password ?? "",
                overwrite);

            // Ergebnis ausgeben
            int successCount = results.Count(r => r.Success);
            int failCount = results.Count(r => !r.Success);

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

            // Exit-Code basierend auf dem Ergebnis setzen
            Environment.ExitCode = failCount > 0 ? 1 : 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler: {ex.Message}");
            Environment.ExitCode = 1;
        }
    }

    /// <parameter name="summary">
    /// Zeigt die usage-Informationen an
    /// </parameter>
    static void PrintUsage()
    {
        Console.WriteLine("Verwendung:");
        Console.WriteLine("  BP-Analyzer.exe [Optionen]");
        Console.WriteLine();
        Console.WriteLine("Optionen:");
        Console.WriteLine("  --process=<name>     Name des zu exportierenden Prozesses (erforderlich)");
        Console.WriteLine("  --output=<pfad>      Ausgabeverzeichnis (Standard: aktuelles Verzeichnis)");
        Console.WriteLine("  --user=<benutzername> Blue Prism Benutzername");
        Console.WriteLine("  --password=<passwort> Blue Prism Passwort");
        Console.WriteLine("  --overwrite=<yes|no> Vorhandene Dateien überschreiben (Standard: no)");
        Console.WriteLine();
        Console.WriteLine("Beispiele:");
        Console.WriteLine("  BP-Analyzer.exe --process \"MeinProzess\" --output \"C:\\Exporte\"");
        Console.WriteLine("  BP-Analyzer.exe --process \"MeinProzess\" --output \"C:\\Exporte\" --overwrite yes");
        Console.WriteLine("  BP-Analyzer.exe --process \"MeinProzess\" --user admin --password geheim");
    }
}
