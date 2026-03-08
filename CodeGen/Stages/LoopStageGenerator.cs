using System.Xml.Linq;
using BPAnalyzer.CodeGen.FlowControl;
using BPAnalyzer.CodeGen.Utilities;

namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Generates VB.NET code for LoopStart stages.
/// </summary>
public class LoopStartStageGenerator : StageGeneratorBase
{
    public override void Generate(XElement stage, System.Text.StringBuilder sb)
    {
        // Get the collection name
        var collectionName = NameSanitizer.SanitizeVariableName(stage.Element("loopdata")?.Value);

        // Generate For Each loop
        sb.AppendLine($"        {collectionName}.SelectFirstRow()");

        // Generate GoTo to next stage
        StageNavigator.GenerateGoTo(sb, stage.Document, stage.Element("onsuccess")?.Value);
    }
}

/// <summary>
/// Generates VB.NET code for LoopEnd stages.
/// </summary>
public class LoopEndStageGenerator : StageGeneratorBase
{
    public override void Generate(XElement stage, System.Text.StringBuilder sb)
    {
        var stageName = stage.Attribute("name")?.Value ?? "";
        var groupId = stage.Element("groupid")?.Value;
        var loopStart = stage.Document?.Descendants()
                .FirstOrDefault(e => e.Attribute("type")?.Value == "LoopStart" && e.Element("groupid")?.Value == groupId);

        if (loopStart == null) return;

        var collectionName = NameSanitizer.SanitizeVariableName(loopStart.Element("loopdata")?.Value);

        // Close the loop
        sb.AppendLine($"        If {collectionName}.SelectNextRow() Then");
        StageNavigator.GenerateGoTo(sb, stage.Document, loopStart.Element("onsuccess")?.Value, 12);
        sb.AppendLine($"        End If");
        StageNavigator.GenerateGoTo(sb, stage.Document, stage.Element("onsuccess")?.Value);
    }
}
