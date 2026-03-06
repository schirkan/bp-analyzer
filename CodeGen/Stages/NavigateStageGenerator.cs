using System.Xml.Linq;

namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Generates VB.NET code for Navigate stages (UI automation).
/// </summary>
public class NavigateStageGenerator : StageGeneratorBase
{
    public override string StageType => "Navigate";

    public override void Generate(XElement stage, System.Text.StringBuilder sb)
    {
        sb.AppendLine($"        ' Navigate: UI automation");
        sb.AppendLine($"        ' TODO: Implement");

        GenerateGoTo(sb, stage.Document, stage.Element("onsuccess")?.Value);
    }
}
