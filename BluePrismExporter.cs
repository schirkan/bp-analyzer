using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;

public class BluePrismExporter
{
  private string _automateCPath;
  private HashSet<string> _exportedObjects;

  public BluePrismExporter()
  {
    _automateCPath = "C:\\Program Files\\Blue Prism Limited\\Blue Prism Automate\\AutomateC.exe";
    _exportedObjects = new HashSet<string>();
  }

  public string AutomateCPath
  {
    get => _automateCPath;
    set => _automateCPath = value;
  }

  /// <summary>
  /// Setzt den Zustand der exportierten Objekte zurück (für neuen Exportlauf)
  /// </summary>
  public void ResetExportedObjects()
  {
    _exportedObjects.Clear();
  }

  /// <summary>
  /// Export result containing success status and details
  /// </summary>
  public class ExportResult
  {
    public bool Success { get; set; }
    public string OutputPath { get; set; } = "";
    public string StandardOutput { get; set; } = "";
    public string StandardError { get; set; } = "";
    public int ExitCode { get; set; }
    public string? ErrorMessage { get; set; }
  }

  /// <summary>
  /// Executes the Blue Prism export command
  /// </summary>
  /// <param name="processName">Name of the process to export</param>
  /// <param name="outputPath">Target file path</param>
  /// <param name="username">Blue Prism username</param>
  /// <param name="password">Blue Prism password</param>
  /// <param name="overwrite">If true, existing files will be overwritten</param>
  /// <returns>ExportResult with success status and details</returns>
  public ExportResult Export(string processName, string outputPath, string username, string password, bool overwrite = false)
  {
    var result = new ExportResult { OutputPath = outputPath };

    if (string.IsNullOrWhiteSpace(processName))
    {
      result.Success = false;
      result.ErrorMessage = "Process name cannot be empty";
      return result;
    }

    if (string.IsNullOrWhiteSpace(outputPath))
    {
      result.Success = false;
      result.ErrorMessage = "Output path cannot be empty";
      return result;
    }

    // Check if file already exists and overwrite is not enabled
    if (!overwrite && File.Exists(outputPath))
    {
      result.Success = true;
      result.ErrorMessage = "File already exists, skipping export";
      return result;
    }

    // Build command arguments
    string arguments;
    if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
    {
      arguments = $"/export \"{processName}\" /user \"{username}\" \"{password}\"";
    }
    else
    {
      arguments = $"/export \"{processName}\" /sso";
    }

    // Use temp directory as working directory
    string tempDirectory = Path.GetTempPath();

    // Possible export file names from Blue Prism
    string[] possibleFileNames = new[]
    {
      $"BPA Object - {processName}.xml",
      $"BPA Process - {processName}.xml"
    };

    // Delete existing files before export
    foreach (string fileName in possibleFileNames)
    {
      string filePath = Path.Combine(tempDirectory, fileName);
      if (File.Exists(filePath))
      {
        try
        {
          File.Delete(filePath);
        }
        catch
        {
          // Ignore deletion errors
        }
      }
    }

    // Execute the export command
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
        result.Success = false;
        result.ErrorMessage = "Failed to start process";
        return result;
      }

      result.StandardOutput = process.StandardOutput.ReadToEnd();
      result.StandardError = process.StandardError.ReadToEnd();
      process.WaitForExit();

      result.ExitCode = process.ExitCode;

      if (process.ExitCode != 0)
      {
        result.Success = false;
        return result;
      }

      // Find and move the exported file
      try
      {
        // Ensure target directory exists
        string? targetDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(targetDir) && !Directory.Exists(targetDir))
        {
          Directory.CreateDirectory(targetDir);
        }

        // Find exported file
        string? exportedFilePath = null;
        foreach (string fileName in possibleFileNames)
        {
          string filePath = Path.Combine(tempDirectory, fileName);
          if (File.Exists(filePath))
          {
            exportedFilePath = filePath;
            break;
          }
        }

        if (exportedFilePath != null)
        {
          // Delete existing target file
          if (File.Exists(outputPath))
          {
            File.Delete(outputPath);
          }
          File.Move(exportedFilePath, outputPath);
          result.Success = true;
        }
        else
        {
          result.Success = false;
          result.ErrorMessage = "Exported file not found in temp directory";
        }
      }
      catch (Exception ex)
      {
        result.Success = false;
        result.ErrorMessage = $"Failed to move exported file: {ex.Message}";
      }
    }

    return result;
  }

  /// <summary>
  /// Exportiert einen Prozess und alle abhängigen Objekte rekursiv
  /// </summary>
  /// <param name="processName">Name des zu exportierenden Prozesses/Objekts</param>
  /// <param name="outputDirectory">Zielverzeichnis für die exportierten Dateien</param>
  /// <param name="username">Blue Prism Benutzername</param>
  /// <param name="password">Blue Prism Passwort</param>
  /// <param name="overwrite">Wenn true, werden existierende Dateien überschrieben</param>
  /// <returns>Liste der ExportErgebnisse</returns>
  public List<ExportResult> ExportWithDependencies(string processName, string outputDirectory, string username, string password, bool overwrite = false)
  {
    var results = new List<ExportResult>();

    // Reset the exported objects set for a new export run
    _exportedObjects.Clear();

    // Ensure output directory exists
    if (!Directory.Exists(outputDirectory))
    {
      Directory.CreateDirectory(outputDirectory);
    }

    // Export the main process and its dependencies recursively
    ExportRecursive(processName, outputDirectory, username, password, overwrite, results);

    return results;
  }

  /// <summary>
  /// Prüft, ob ein Objekt ein internes BluePrism-Systemobjekt ist
  /// </summary>
  private bool IsInternalObject(string objectName)
  {
    return objectName.StartsWith("Blueprism.AutomateProcessCore.", StringComparison.OrdinalIgnoreCase);
  }

  /// <summary>
  /// Rekursive Hilfsmethode zum Exportieren eines Prozesses und seiner Abhängigkeiten
  /// </summary>
  private void ExportRecursive(string objectName, string outputDirectory, string username, string password, bool overwrite, List<ExportResult> results)
  {
    // Check if this object has already been exported
    if (_exportedObjects.Contains(objectName))
    {
      Console.WriteLine($"  -> '{objectName}' wurde bereits exportiert, überspringen...");
      return;
    }

    // Skip internal BluePrism objects
    if (IsInternalObject(objectName))
    {
      Console.WriteLine($"  -> '{objectName}' ist ein internes Objekt, überspringen...");
      _exportedObjects.Add(objectName);
      return;
    }

    // Determine output path
    string outputPath = Path.Combine(outputDirectory, objectName + ".xml");

    // Export the current object
    Console.WriteLine($"Exportiere: {objectName}");
    var result = Export(objectName, outputPath, username, password, overwrite);
    results.Add(result);

    if (result.Success)
    {
      // Mark as exported
      _exportedObjects.Add(objectName);

      // Find and export dependencies from the exported XML file
      var dependencies = GetDependenciesFromXml(outputPath);
      foreach (var dependency in dependencies)
      {
        ExportRecursive(dependency, outputDirectory, username, password, overwrite, results);
      }
    }
    else
    {
      Console.WriteLine($"  FEHLER beim Exportieren von '{objectName}': {result.ErrorMessage}");
    }
  }

  /// <summary>
  /// Liest eine exportierte XML-Datei und extrahiert alle referenzierten Objekte (resource object="xxx")
  /// </summary>
  /// <param name="xmlFilePath">Pfad zur XML-Datei</param>
  /// <returns>Liste der Objektnamen, von denen dieses Objekt abhängt</returns>
  public List<string> GetDependenciesFromXml(string xmlFilePath)
  {
    var dependencies = new List<string>();

    try
    {
      if (!File.Exists(xmlFilePath))
      {
        return dependencies;
      }

      // Read the XML file content
      string xmlContent = File.ReadAllText(xmlFilePath);

      // Use regex to find all "resource object="xxx"" patterns
      // This pattern matches: resource object="ObjectName" or resource object='ObjectName'
      var regex = new Regex(@"resource\s+object\s*=\s*[""']([^""']+)[""']", RegexOptions.IgnoreCase);
      var matches = regex.Matches(xmlContent);

      foreach (Match match in matches)
      {
        string objectName = match.Groups[1].Value;
        if (!string.IsNullOrWhiteSpace(objectName) && !dependencies.Contains(objectName))
        {
          dependencies.Add(objectName);
        }
      }

      // Also try XML parsing for more reliable extraction
      try
      {
        XDocument doc = XDocument.Load(xmlFilePath);

        // Find all elements with "object" attribute that are part of resource references
        var resourceElements = doc.Descendants()
          .Where(e => e.Attribute("object") != null &&
                     e.Name.LocalName.Equals("resource", StringComparison.OrdinalIgnoreCase));

        foreach (var element in resourceElements)
        {
          string? objectName = element.Attribute("object")?.Value;
          if (!string.IsNullOrWhiteSpace(objectName) && !dependencies.Contains(objectName))
          {
            dependencies.Add(objectName);
          }
        }
      }
      catch
      {
        // If XML parsing fails, we still have the regex results
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Fehler beim Parsen der XML-Datei {xmlFilePath}: {ex.Message}");
    }

    return dependencies;
  }
}
