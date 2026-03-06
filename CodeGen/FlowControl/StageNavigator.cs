using System.Xml.Linq;
using BPAnalyzer.CodeGen.Utilities;

namespace BPAnalyzer.CodeGen.FlowControl;

/// <summary>
/// Handles stage navigation, label resolution, and anchor chain resolution.
/// </summary>
public static class StageNavigator
{
    /// <summary>
    /// Resolves the stage label for a given stage ID, following anchor chains.
    /// </summary>
    public static string ResolveStageLabel(string stageId, XDocument doc)
    {
        if (string.IsNullOrEmpty(stageId)) return GetLabel("Unknown", stageId);

        var stage = doc.Descendants()
            .FirstOrDefault(e => e.Name.LocalName == "stage" && e.Attribute("stageid")?.Value == stageId);

        if (stage == null) return GetLabel("Unknown", stageId);

        var stageType = stage.Attribute("type")?.Value ?? "Unknown";

        if (stageType == "End")
        {
            var subsheetId = stage.Element("subsheetid")?.Value;
            var endStage = doc.Descendants()
                .FirstOrDefault(e => e.Name.LocalName == "stage"
                    && e.Attribute("type")?.Value == "End"
                    && e.Element("subsheetid")?.Value == subsheetId);

            return GetLabel("End", endStage?.Attribute("stageid")?.Value ?? stageId);
        }

        if (stageType == "Anchor")
        {
            return ResolveAnchorChain(stageId, doc);
        }

        return GetLabel(stageType, stageId);
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
                return GetLabel("Unknown", stageId);
            }

            var stageType = stage.Attribute("type")?.Value;
            if (stageType != "Anchor")
            {
                return ResolveStageLabel(currentId, doc);
            }

            currentId = stage.Element("onsuccess")?.Value;
        }

        return GetLabel("Unknown", stageId);
    }

    /// <summary>
    /// Gets a formatted label for a stage.
    /// </summary>
    public static string GetLabel(string stageType, string stageId)
    {
        var sanitizedId = NameSanitizer.SanitizeId(stageId);
        return $"{stageType}_{sanitizedId}_Label";
    }
}
