using System.Xml.Linq;

namespace BPAnalyzer.CodeGen.FlowControl;

/// <summary>
/// Handles flow control logic: stage sorting, navigation, and label generation.
/// </summary>
public static class FlowController
{
    /// <summary>
    /// Sorts stages by execution order (depth-first traversal).
    /// </summary>
    public static List<XElement> SortStagesByExecutionOrder(List<XElement> stages)
    {
        var stageDict = stages.ToDictionary(s => s.Attribute("stageid")?.Value ?? "", s => s);
        var startStage = stages.FirstOrDefault(s => s.Attribute("type")?.Value == "Start");

        if (startStage == null) return stages;

        var sorted = new List<XElement>();
        var visited = new HashSet<string>();

        void Visit(XElement stage)
        {
            if (stage == null) return;
            var stageId = stage.Attribute("stageid")?.Value;
            if (string.IsNullOrEmpty(stageId)) return;
            if (visited.Contains(stageId)) return;
            visited.Add(stageId);

            var stageType = stage.Attribute("type")?.Value;

            if (stageType == "Anchor")
            {
                var anchorOnsuccess = stage.Element("onsuccess")?.Value;
                if (!string.IsNullOrEmpty(anchorOnsuccess) && stageDict.TryGetValue(anchorOnsuccess, out var anchorNextStage))
                {
                    Visit(anchorNextStage);
                }
                return;
            }

            if (stageType == "ProcessInfo") return;

            sorted.Add(stage);

            var onsuccess = stage.Element("onsuccess")?.Value;
            if (!string.IsNullOrEmpty(onsuccess) && stageDict.TryGetValue(onsuccess, out var nextStage))
            {
                Visit(nextStage);
            }

            var ontrue = stage.Element("ontrue")?.Value;
            var onfalse = stage.Element("onfalse")?.Value;

            if (!string.IsNullOrEmpty(ontrue) && stageDict.TryGetValue(ontrue, out var trueStage)) Visit(trueStage);
            if (!string.IsNullOrEmpty(onfalse) && stageDict.TryGetValue(onfalse, out var falseStage)) Visit(falseStage);
        }

        Visit(startStage);

        foreach (var stage in stages)
        {
            var stageId = stage.Attribute("stageid")?.Value!;
            if (!visited.Contains(stageId)) sorted.Add(stage);
        }

        return sorted;
    }

    /// <summary>
    /// Checks if any stage in the method has a Recover stage (at page level).
    /// </summary>
    public static bool HasRecoverStage(List<XElement> stages)
    {
        return stages.Any(s => s.Attribute("type")?.Value == "Recover");
    }

    /// <summary>
    /// Checks if a stage is within a block by comparing coordinates.
    /// </summary>
    public static bool IsStageInBlock(XElement stage, XElement block)
    {
        var stageDisplay = stage.Element("display");
        var blockDisplay = block.Element("display");

        if (stageDisplay == null || blockDisplay == null) return false;

        var stageX = ParseCoordinate(stageDisplay.Attribute("x")?.Value);
        var stageY = ParseCoordinate(stageDisplay.Attribute("y")?.Value);

        var blockX = ParseCoordinate(blockDisplay.Attribute("x")?.Value);
        var blockY = ParseCoordinate(blockDisplay.Attribute("y")?.Value);
        var blockW = ParseCoordinate(blockDisplay.Attribute("w")?.Value);
        var blockH = ParseCoordinate(blockDisplay.Attribute("h")?.Value);

        return stageX >= blockX && stageX <= blockX + blockW &&
               stageY >= blockY && stageY <= blockY + blockH;
    }

    private static double ParseCoordinate(string? value)
    {
        if (string.IsNullOrEmpty(value)) return 0;
        return double.TryParse(value, out var result) ? result : 0;
    }

    /// <summary>
    /// Gets the set of stage types that should be skipped during generation.
    /// </summary>
    public static readonly HashSet<string> SkipStages = new()
    {
        "Block", "SubSheetInfo", "Data", "Collection", "WaitEnd", "End", "Anchor", "ProcessInfo"
    };
}
