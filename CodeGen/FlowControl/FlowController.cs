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
        var visited = new HashSet<XElement>();

        void Visit(XElement? stage)
        {
            if (stage == null) return;
            if (visited.Contains(stage)) return;
            visited.Add(stage);

            var stageType = stage.Attribute("type")?.Value;

            if (stageType == "Anchor")
            {
                var anchorOnsuccess = stage.Element("onsuccess")?.Value;
                if (!string.IsNullOrEmpty(anchorOnsuccess) && stageDict.TryGetValue(anchorOnsuccess, out var anchorNextStage)) Visit(anchorNextStage);
                return;
            }

            if (stageType == "ProcessInfo") return;

            sorted.Add(stage);

            var onsuccess = stage.Element("onsuccess")?.Value;
            if (!string.IsNullOrEmpty(onsuccess) && stageDict.TryGetValue(onsuccess, out var nextStage)) Visit(nextStage);

            // decision
            var ontrue = stage.Element("ontrue")?.Value;
            var onfalse = stage.Element("onfalse")?.Value;

            if (!string.IsNullOrEmpty(onfalse) && stageDict.TryGetValue(onfalse, out var falseStage)) Visit(falseStage);
            if (!string.IsNullOrEmpty(ontrue) && stageDict.TryGetValue(ontrue, out var trueStage)) Visit(trueStage);

            // group for choice, wait and loop
            var groupId = stage.Element("groupid")?.Value;
            if (!string.IsNullOrEmpty(groupId))
            {
                var nextGroupStage = stages.FirstOrDefault(e => e.Element("groupid")?.Value == groupId);
                Visit(nextGroupStage);
            }

            // cases for choice and wait 
            var choices = stage.Element("choices")?.Elements("choice").ToList();
            if (choices != null && choices.Any())
            {
                foreach (var choice in choices)
                {
                    var choiceOntrue = choice.Element("ontrue")?.Value;
                    if (!string.IsNullOrEmpty(choiceOntrue) && stageDict.TryGetValue(choiceOntrue, out var choiceOntrueStage)) Visit(choiceOntrueStage);
                }
            }
        }

        Visit(startStage);

        // visit recover stages for exception handling flows
        var recoverStages = stages.Where(stage => stage.Attribute("type")?.Value == "Recover");
        foreach (var stage in recoverStages)
        {
            Visit(stage);
        }

        // visit all stages to find unconnected elements
        foreach (var stage in stages)
        {
            Visit(stage);
        }

        return sorted;
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
        "Block", "SubSheetInfo", "Data", "Collection", "WaitEnd", "End", "Anchor", "ProcessInfo", "ChoiceEnd"
    };
}
