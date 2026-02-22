using System;
using System.Diagnostics;
using System.IO;

public class BluePrismExporter
{
  private string _automateCPath;

  public BluePrismExporter()
  {
    _automateCPath = "C:\\Program Files\\Blue Prism Limited\\Blue Prism Automate\\AutomateC.exe";
  }

  public string AutomateCPath
  {
    get => _automateCPath;
    set => _automateCPath = value;
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
  /// <returns>ExportResult with success status and details</returns>
  public ExportResult Export(string processName, string outputPath, string username, string password)
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
}
