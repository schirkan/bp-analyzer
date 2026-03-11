using System.Xml.Linq;
using BPAnalyzer.CodeGen.Utilities;

namespace BPAnalyzer.CodeGen.FlowControl;

/// <summary>
/// Handles stage navigation, label resolution, and anchor chain resolution.
/// </summary>
public static class StageNavigator
{
    /// <summary>
    /// Generates the GoTo statement for the next stage.
    /// </summary>
    public static void GenerateGoTo(System.Text.StringBuilder sb, XDocument? document, string? targetStageId, int indentation = 8)
    {
        if (string.IsNullOrEmpty(targetStageId) || document == null)
        {
            return;
        }

        var targetStageLabel = ResolveStageLabel(targetStageId, document);
        var spaces = "".PadLeft(indentation);
        sb.AppendLine($"{spaces}GoTo {targetStageLabel}");
    }

    /// <summary>
    /// Resolves the stage label for a given stage ID, following anchor chains.
    /// </summary>
    public static string ResolveStageLabel(string stageId, XDocument doc)
    {
        if (string.IsNullOrEmpty(stageId)) return "Unknown_" + stageId;

        var stage = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "stage" && e.Attribute("stageid")?.Value == stageId);
        if (stage == null) return "Unknown_" + stageId;

        var stageType = stage.Attribute("type")?.Value ?? "Unknown";

        if (stageType == "End")
        {
            var subsheetId = stage.Element("subsheetid")?.Value;
            var endStage = doc.Descendants().FirstOrDefault(e =>
                e.Name.LocalName == "stage" &&
                e.Attribute("type")?.Value == "End" &&
                e.Element("subsheetid")?.Value == subsheetId);

            return GetLabel(endStage?.Attribute("stageid")?.Value ?? stageId, doc);
        }
        else if (stageType == "Anchor")
        {
            return ResolveAnchorChain(stageId, doc);
        }

        return GetLabel(stageId, doc);
    }

    /// <summary>
    /// Resolves an anchor chain to find the final target stage.
    /// </summary>
    public static string ResolveAnchorChain(string stageId, XDocument doc)
    {
        var visited = new HashSet<string>();
        var currentId = stageId;

        while (!string.IsNullOrEmpty(currentId) && !visited.Contains(currentId))
        {
            visited.Add(currentId);

            var stage = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "stage" && e.Attribute("stageid")?.Value == currentId);
            if (stage == null) break;

            var stageType = stage.Attribute("type")?.Value;
            if (stageType != "Anchor")
            {
                return ResolveStageLabel(currentId, doc);
            }

            currentId = stage.Element("onsuccess")?.Value;
        }

        return "Unknown_" + stageId;
    }

    private static Dictionary<string, string> _labelMapping = [];
    private static List<Tuple<string, string>> _labelAndPage = [];

    /// <summary>
    /// Gets a formatted label for a stage.
    /// </summary>
    public static string GetLabel(string stageId, XDocument? doc)
    {
        if (_labelMapping.TryGetValue(stageId, out string? label))
        {
            return label;
        }

        var stage = doc?.Descendants().FirstOrDefault(e => e.Name.LocalName == "stage" && e.Attribute("stageid")?.Value == stageId);
        var stageName = stage?.Attribute("name")?.Value!;
        var stageType = stage?.Attribute("type")?.Value;
        var subsheetId = stage?.Element("subsheetid")?.Value ?? "";

        var subsheetName = SubsheetResolver.FindSubsheetName(doc, subsheetId);
        if (string.IsNullOrWhiteSpace(subsheetName)) subsheetName = "Main";
        var sanitizedSubsheetName = NameSanitizer.SanitizeMethodName(subsheetName);

        var newLabel = "";
        if (stageType == "Start" || stageType == "End")
        {
            newLabel = stageType + "_" + sanitizedSubsheetName;
        }
        else
        {
            var key = sanitizedSubsheetName + "_" + NameSanitizer.SanitizeMethodName(stageName);
            newLabel = key;
            var counter = 1;
            while (_labelAndPage.Any(x => x.Item1 == subsheetId && x.Item2 == newLabel))
            {
                newLabel = key + "_" + (++counter);
            }
        }

        _labelAndPage.Add(new Tuple<string, string>(subsheetId, newLabel));
        _labelMapping.Add(stageId, newLabel);
        return newLabel;
    }
}
