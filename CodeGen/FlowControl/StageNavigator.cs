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
        if (string.IsNullOrEmpty(stageId)) return GetLabel("Unknown", stageId, doc);

        var stage = doc.Descendants()
            .FirstOrDefault(e => e.Name.LocalName == "stage" && e.Attribute("stageid")?.Value == stageId);

        if (stage == null) return GetLabel("Unknown", stageId, doc);

        var stageType = stage.Attribute("type")?.Value ?? "Unknown";

        if (stageType == "End")
        {
            var subsheetId = stage.Element("subsheetid")?.Value;
            var endStage = doc.Descendants()
                .FirstOrDefault(e => e.Name.LocalName == "stage"
                    && e.Attribute("type")?.Value == "End"
                    && e.Element("subsheetid")?.Value == subsheetId);

            return GetLabel("End", endStage?.Attribute("stageid")?.Value ?? stageId, doc);
        }

        if (stageType == "Anchor")
        {
            return ResolveAnchorChain(stageId, doc);
        }

        return GetLabel(stageType, stageId, doc);
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

            var stage = doc.Descendants()
                .FirstOrDefault(e => e.Name.LocalName == "stage" && e.Attribute("stageid")?.Value == currentId);

            if (stage == null)
            {
                return GetLabel("Unknown", stageId, doc);
            }

            var stageType = stage.Attribute("type")?.Value;
            if (stageType != "Anchor")
            {
                return ResolveStageLabel(currentId, doc);
            }

            currentId = stage.Element("onsuccess")?.Value;
        }

        return GetLabel("Unknown", stageId, doc);
    }

    private static Dictionary<string, string> _labelMapping = new Dictionary<string, string>();
    private static Dictionary<string, int> _labelCounter = new Dictionary<string, int>();

    /// <summary>
    /// Gets a formatted label for a stage.
    /// </summary>
    public static string GetLabel(string stageType, string stageId, XDocument? doc)
    {
        if (_labelMapping.TryGetValue(stageId, out string? label))
        {
            return label;
        }

        var processId = doc?.Root?.Attribute("preferredid")?.Value;
        var uniqueId = "_";
        var key = processId + stageType;
        if (_labelCounter.TryGetValue(key, out int counter))
        {
            _labelCounter[key]++;
            uniqueId = "_" + _labelCounter[key] + "_";
        }
        else
        {
            _labelCounter[key] = 1;
        }

        if (stageType == "Start" || stageType == "End")
        {
            var stage = doc?.Root?.Elements("stage").FirstOrDefault(x => x.Attribute("stageid")?.Value == stageId);
            var subsheetId = stage?.Element("subsheetid")?.Value;
            var subsheetName = SubsheetResolver.FindSubsheetName(doc, subsheetId);
            uniqueId = "_" + NameSanitizer.SanitizeMethodName(subsheetName) + "_";
        }

        // var sanitizedId = NameSanitizer.SanitizeId(stageId);
        var newLabel = $"{stageType}{uniqueId}Label";
        _labelMapping.Add(stageId, newLabel);
        return newLabel;

        // var sanitizedId = NameSanitizer.SanitizeId(stageId);
        // return $"{stageType}_{sanitizedId}_Label";
    }
}
