
using System.Text.Json;

/// <summary>
/// Registry zur Aufzeichnung von Abhängigkeiten zwischen Methoden/Klassen während der Codegenerierung.
/// </summary>
public static class DependencyRegistry
{
  // Thread-lokaler Kontext für parallele Codegenerierung
  private static readonly AsyncLocal<string> currentSourceClass = new AsyncLocal<string>();
  private static readonly AsyncLocal<string> currentSourceMethod = new AsyncLocal<string>();

  // Abhängigkeiten: (SourceClass, SourceMethod, TargetClass, TargetMethod)
  private static readonly HashSet<(string SourceClass, string SourceMethod, string TargetClass, string TargetMethod)> dependencies
    = new HashSet<(string, string, string, string)>();

  // Exceptions: (SourceClass, SourceMethod, ExceptionType, ExceptionText)
  private static readonly HashSet<(string SourceClass, string SourceMethod, string ExceptionType, string ExceptionText)> exceptions
    = new HashSet<(string, string, string, string)>();

  /// <summary>
  /// Registriert eine Exception mit Typ und Text.
  /// </summary>
  public static void RegisterException(string exceptionType, string exceptionText)
  {
    if (string.IsNullOrEmpty(currentSourceClass.Value) || string.IsNullOrEmpty(currentSourceMethod.Value))
      throw new InvalidOperationException("SetCurrentClass und SetCurrentMethod muss vor RegisterDependency aufgerufen werden.");

    exceptions.Add((currentSourceClass.Value ?? "", currentSourceMethod.Value ?? "", exceptionType, exceptionText));
  }

  /// <summary>
  /// Schreibt alle registrierten Exceptions als JSON-Datei.
  /// </summary>
  /// <param name="filePath">Pfad zur Ausgabedatei</param>
  public static void WriteExceptionsToJson(string filePath)
  {
    // Dictionary<"Class.Method", List<[ExceptionType, ExceptionText]>>
    var exDict = new Dictionary<string, List<string[]>>();

    foreach (var ex in exceptions)
    {
      var sourceKey = $"{ex.SourceClass}.{ex.SourceMethod}";
      var entry = new[] { ex.ExceptionType, ex.ExceptionText };

      if (!exDict.TryGetValue(sourceKey, out var exList))
      {
        exList = new List<string[]>();
        exDict[sourceKey] = exList;
      }
      exList.Add(entry);
    }

    var options = new JsonSerializerOptions { WriteIndented = true };
    File.WriteAllText(filePath, JsonSerializer.Serialize(exDict, options));
  }

  /// <summary>
  /// Setzt den aktuellen Quell-Kontext (Methode), von dem aus Abhängigkeiten registriert werden.
  /// </summary>
  public static void SetCurrentMethod(string methodName)
  {
    currentSourceMethod.Value = methodName;
  }

  /// <summary>
  /// Setzt den aktuellen Quell-Kontext (Klasse), von dem aus Abhängigkeiten registriert werden.
  /// </summary>
  public static void SetCurrentClass(string className)
  {
    currentSourceClass.Value = className;
    currentSourceMethod.Value = "";
  }

  /// <summary>
  /// Registriert eine Abhängigkeit von der aktuellen Source (Klasse+Methode) zu Target (Klasse+Methode).
  /// </summary>
  public static void RegisterDependency(string targetClass, string targetMethod)
  {
    if (string.IsNullOrEmpty(currentSourceClass.Value) || string.IsNullOrEmpty(currentSourceMethod.Value))
      throw new InvalidOperationException("SetCurrentClass und SetCurrentMethod muss vor RegisterDependency aufgerufen werden.");

    dependencies.Add((currentSourceClass.Value ?? "", currentSourceMethod.Value ?? "", targetClass, targetMethod));
  }

  /// <summary>
  /// Gibt alle registrierten Abhängigkeiten zurück.
  /// </summary>
  public static IEnumerable<(string SourceClass, string SourceMethod, string TargetClass, string TargetMethod)> GetAllDependencies()
    => dependencies;

  /// <summary>
  /// Schreibt alle registrierten Abhängigkeiten als JSON-Datei.
  /// </summary>
  /// <param name="filePath">Pfad zur Ausgabedatei</param>
  public static void WriteDependenciesToJson(string filePath)
  {
    // Dictionary<"Class.Method", List<"DepClass.DepMethod">>
    var depDict = new Dictionary<string, List<string>>();

    foreach (var dep in dependencies)
    {
      var sourceKey = $"{dep.SourceClass}.{dep.SourceMethod}";
      var target = string.IsNullOrEmpty(dep.TargetMethod)
        ? dep.TargetClass
        : $"{dep.TargetClass}.{dep.TargetMethod}";

      if (!depDict.TryGetValue(sourceKey, out var depList))
      {
        depList = new List<string>();
        depDict[sourceKey] = depList;
      }
      if (!depList.Contains(target))
        depList.Add(target);
    }

    var options = new JsonSerializerOptions { WriteIndented = true };
    File.WriteAllText(filePath, JsonSerializer.Serialize(depDict, options));
  }
}