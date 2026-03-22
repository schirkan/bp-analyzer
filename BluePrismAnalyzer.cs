using System.Text.Json;
using System.Text.RegularExpressions;

namespace BPAnalyzer
{
  public class BluePrismAnalyzer
  {
    public record SddResult(string DisplayName, string InternalName, string Content, string OutputPath);

    public static List<SddResult> GenerateSdds(
        string outputDir = "output",
        string templatePath = "templates/SDD_Process_Template.md")
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

      var classes = JsonDocument.Parse(classesJson).RootElement;
      var dependencies = JsonDocument.Parse(dependenciesJson).RootElement;
      var exceptions = JsonDocument.Parse(exceptionsJson).RootElement;

      var processDict = classes.GetProperty("process");
      var results = new List<SddResult>();

      foreach (var proc in processDict.EnumerateObject())
      {
        string internalName = proc.Name;
        string displayName = proc.Value.GetString() ?? proc.Name;

        // Recursively collect all dependencies (flat, sorted, unique)
        HashSet<string> allDeps = new();
        void CollectDeps(string procName)
        {
          foreach (var dep in dependencies.EnumerateObject())
          {
            if (dep.Name.StartsWith(procName + "."))
            {
              foreach (var d in dep.Value.EnumerateArray())
              {
                var depName = d.GetString() ?? "";
                if (!string.IsNullOrWhiteSpace(depName) && allDeps.Add(depName))
                {
                  var depProcKey = depName.Split('.')[0];
                  if (processDict.TryGetProperty(depProcKey, out _)
                      || (classes.TryGetProperty("object", out var objDict) && objDict.TryGetProperty(depProcKey, out _)))
                  {
                    CollectDeps(depProcKey);
                  }
                }
              }
            }
          }
        }
        CollectDeps(internalName);
        var deps = allDeps.OrderBy(x => x).ToList();

        // Recursively collect all exceptions for a process and its dependencies (stage-aware)
        HashSet<string> visitedForEx = new();
        List<(string Stage, string Type, string Message, string Source)> allEx = new();
        void CollectExceptions(string procOrStage)
        {
          if (!visitedForEx.Add(procOrStage)) return;
          // Find all exception keys that match procOrStage as prefix (e.g. MP_System_Update.Main, Microsoft_Store.Launch)
          foreach (var ex in exceptions.EnumerateObject())
          {
            if (ex.Name.Equals(procOrStage, StringComparison.OrdinalIgnoreCase))
            {
              foreach (var arr in ex.Value.EnumerateArray())
              {
                if (arr.GetArrayLength() == 2)
                  allEx.Add((Stage: ex.Name.Contains('.') ? ex.Name.Split('.')[1] : ex.Name, Type: arr[0].GetString() ?? "", Message: arr[1].GetString() ?? "", Source: procOrStage));
              }
            }
          }
          // Recurse into dependencies for this procOrStage
          foreach (var dep in dependencies.EnumerateObject())
          {
            if (dep.Name.Equals(procOrStage, StringComparison.OrdinalIgnoreCase))
            {
              foreach (var d in dep.Value.EnumerateArray())
              {
                var depName = d.GetString() ?? "";
                if (!string.IsNullOrWhiteSpace(depName))
                  CollectExceptions(depName);
              }
            }
          }
        }
        // Start with all entry points: Main stage and all direct dependencies
        CollectExceptions(internalName + ".Main");
        foreach (var dep in deps)
        {
          CollectExceptions(dep);
        }

        // Render template
        string sdd = template;
        sdd = sdd.Replace("{{ProcessName}}", displayName);

        // Replace Dependencies block (extract block and format per dependency, split Source.Stage)
        sdd = Regex.Replace(
          sdd,
          "{{#Dependencies}}([\\s\\S]*?){{/Dependencies}}",
          match =>
          {
            string block = match.Groups[1].Value;
            if (deps.Count == 0) return "(none)";
            return string.Join("\n", deps.Select(d =>
            {
              var parts = d.Split('.');
              string source = parts.Length > 0 ? parts[0] : d;
              string stage = parts.Length > 1 ? parts[1] : "";
              return block.Replace("{{Source}}", source)
                          .Replace("{{Stage}}", stage).Trim();
            }));
          },
          RegexOptions.Multiline);

        // Replace Exceptions block (extract block and format per exception)
        sdd = Regex.Replace(
          sdd,
          "{{#Exceptions}}([\\s\\S]*?){{/Exceptions}}",
          match =>
          {
            string block = match.Groups[1].Value;
            if (allEx.Count == 0) return "(none)";
            return string.Join("\n", allEx.Select(e =>
            {
              // If Source already contains Stage (e.g. Microsoft_Store.Launch.Launch), avoid double
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

        string outPath = Path.Combine(outputDir, $"SDD {displayName}.md");
        results.Add(new SddResult(displayName, internalName, sdd, outPath));
      }
      return results;
    }
  }
}
