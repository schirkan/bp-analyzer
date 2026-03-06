using System.Xml.Linq;

namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Generates VB.NET code for Recover stages (exception handling - store).
/// </summary>
public class RecoverStageGenerator : StageGeneratorBase
{
    public override string StageType => "Recover";

    public override void Generate(XElement stage, System.Text.StringBuilder sb)
    {
        sb.AppendLine($"        StoreException()");

        GenerateGoTo(sb, stage.Document, stage.Element("onsuccess")?.Value);
    }
}
