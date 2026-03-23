using System.Text.Json;
using System.Text.RegularExpressions;

namespace BPAnalyzer
{
  public class BluePrismAnalyzer
  {
    public record SddResult(string DisplayName, string InternalName, string Content, string OutputPath);

    /// <summary>
    /// Generates Software Design Documents (SDDs) for all processes based on JSON data and a template.
    /// </summary>
    /// <param name="outputDir">The directory containing the JSON files and where SDDs will be output.</param>
    /// <param name="templatePath">The path to the SDD template file.</param>
    /// <returns>A list of SddResult objects containing the generated SDDs.</returns>
    public static List<SddResult> GenerateSdds(
        string outputDir = "output",
        string templatePath = "templates/SDD_Process_Template.md")
    {
      // Load and parse JSON data and template
      var (classes, dependencies, exceptions, template) = LoadData(outputDir, templatePath);

      var processDict = classes.GetProperty("process");
      // var objectDict = classes.GetProperty("object");
      var exDict = exceptions.EnumerateObject().ToDictionary(x => x.Name, x => x.Value.EnumerateArray().ToList());

      var results = new List<SddResult>();
      foreach (var proc in processDict.EnumerateObject())
      {
        string internalName = proc.Name;
        string displayName = proc.Value.GetString() ?? proc.Name;
        var processMethods = dependencies.EnumerateObject().Where(x => x.Name.StartsWith(internalName + ".")).Select(x => x.Name);

        // Collect all dependencies recursively
        HashSet<string> allDeps = [];
        foreach (var method in processMethods)
        {
          CollectDependencies(method, dependencies, allDeps);
        }
        var deps = allDeps.OrderBy(x => x).ToList();

        // Collect all exceptions recursively for the process and its dependencies
        HashSet<(string ClassName, string Method, string Type, string Message)> allEx = [];
        // CollectExceptions(internalName + ".Main", exDict, allEx);
        foreach (var method in processMethods)
        {
          CollectExceptions(method, exDict, allEx);
        }
        foreach (var dep in deps)
        {
          CollectExceptions(dep, exDict, allEx);
        }

        // Render the SDD using the template
        string sdd = RenderTemplate(template, displayName, deps, allEx);

        string outPath = Path.Combine(outputDir, $"SDD {displayName}.md");
        results.Add(new SddResult(displayName, internalName, sdd, outPath));
      }
      return results;
    }

    /// <summary>
    /// Loads and parses the required JSON files and template.
    /// </summary>
    private static (JsonElement classes, JsonElement dependencies, JsonElement exceptions, string template) LoadData(string outputDir, string templatePath)
    {
      string classesPath = Path.Combine(outputDir, "classes.json");
      string dependenciesPath = Path.Combine(outputDir, "dependencies.json");
      string exceptionsPath = Path.Combine(outputDir, "exceptions.json");

      if (!File.Exists(classesPath) || !File.Exists(dependenciesPath) || !File.Exists(exceptionsPath))
        throw new FileNotFoundException("Required JSON files not found in output directory.");

      var classesJson = File.ReadAllText(classesPath);
      var dependenciesJson = File.ReadAllText(dependenciesPath);
      var exceptionsJson = File.ReadAllText(exceptionsPath);
      var template = File.ReadAllText(templatePath);

      return (JsonDocument.Parse(classesJson).RootElement, JsonDocument.Parse(dependenciesJson).RootElement, JsonDocument.Parse(exceptionsJson).RootElement, template);
    }

    /// <summary>
    /// Recursively collects all dependencies for a given process.
    /// </summary>
    private static void CollectDependencies(string procName, JsonElement dependencies, HashSet<string> allDeps)
    {
      var dep = dependencies.EnumerateObject().FirstOrDefault(x => x.Name == procName);
      if (dep.Equals(default(JsonProperty))) return;
      foreach (var d in dep.Value.EnumerateArray())
      {
        var depName = d.GetString() ?? "";
        if (!string.IsNullOrWhiteSpace(depName) && allDeps.Add(depName))
        {
          CollectDependencies(depName, dependencies, allDeps);
        }
      }
    }

    /// <summary>
    /// Recursively collects all exceptions for a process or stage and its dependencies.
    /// </summary>
    private static void CollectExceptions(string method, Dictionary<string, List<JsonElement>> exDict, HashSet<(string Source, string Stage, string Type, string Message)> result)
    {
      List<JsonElement> exceptions;
      if (!exDict.TryGetValue(method, out exceptions)) return;

      var separatorPosition = method.LastIndexOf('.');
      var className = method.Substring(0, separatorPosition);
      var methodName = method.Substring(separatorPosition + 1);

      foreach (var arr in exceptions)
      {
        if (arr.GetArrayLength() == 2)
        {
          result.Add((
            Source: className,
            Stage: methodName,
            Type: arr[0].GetString() ?? "",
            Message: arr[1].GetString() ?? ""
          ));
        }
      }
    }

    /// <summary>
    /// Renders the SDD template with the provided data.
    /// </summary>
    private static string RenderTemplate(string template, string displayName, List<string> deps, HashSet<(string Source, string Stage, string Type, string Message)> allEx)
    {
      string sdd = template.Replace("{{ProcessName}}", displayName);

      var groupedDeps = deps.Select(d =>
        {
          var separatorPosition = d.LastIndexOf('.');
          var className = d.Substring(0, separatorPosition);
          var methodName = d.Substring(separatorPosition + 1);
          return (className, methodName);
        })
        .GroupBy(x => x.className)
        .ToDictionary(x => x.Key, x => string.Join(", ", x.Select(d => d.methodName)))
        .OrderBy(x => x.Key);

      // Replace Dependencies block
      sdd = Regex.Replace(
        sdd,
        "{{#Dependencies}}([\\s\\S]*?){{/Dependencies}}",
        match =>
        {
          string block = match.Groups[1].Value;
          if (deps.Count == 0) return "(none)";

          return string.Join("\n", groupedDeps.Select(d =>
          {
            string source = d.Key;
            string stage = d.Value;
            return block.Replace("{{Source}}", source)
                        .Replace("{{Stage}}", stage).Trim();
          }));
        },
        RegexOptions.Multiline);

      // Replace Exceptions block
      sdd = Regex.Replace(
        sdd,
        "{{#Exceptions}}([\\s\\S]*?){{/Exceptions}}",
        match =>
        {
          string block = match.Groups[1].Value;
          if (allEx.Count == 0) return "(none)";
          return string.Join("\n", allEx.Select(e =>
          {
            string src = e.Source;
            string stage = e.Stage;
            if (src.EndsWith($".{stage}") || src == stage)
              src = src.Substring(0, src.Length - stage.Length - 1);
            return block.Replace("{{Stage}}", stage)
                        .Replace("{{Type}}", e.Type)
                        .Replace("{{Message}}", e.Message)
                        .Replace("{{Source}}", src).Trim();
          }));
        },
        RegexOptions.Multiline);

      return sdd;
    }
  }
}
