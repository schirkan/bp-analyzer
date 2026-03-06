using System.Xml.Linq;

namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Generates VB.NET code for Resume stages (exception handling - resume).
/// </summary>
public class ResumeStageGenerator : StageGeneratorBase
{
    public override string StageType => "Resume";

    public override void Generate(XElement stage, System.Text.StringBuilder sb)
    {
        var onsuccess = stage.Element("onsuccess")?.Value;

        // Clear the stored exception before resuming
        sb.AppendLine($"        ClearException()");

        if (!string.IsNullOrEmpty(onsuccess))
        {
            var targetStage = FindStageLabel(onsuccess, stage.Document!);
            sb.AppendLine($"        Resume {targetStage}");
        }
    }
}
