using System.Xml.Linq;
using BPAnalyzer.CodeGen.FlowControl;

namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Generates VB.NET code for Resume stages (exception handling - resume).
/// </summary>
public class ResumeStageGenerator : StageGeneratorBase
{
    public override void Generate(XElement stage, System.Text.StringBuilder sb)
    {
        var onsuccess = stage.Element("onsuccess")?.Value;

        // Clear the stored exception before resuming
        sb.AppendLine($"        ClearException()");

        if (!string.IsNullOrEmpty(onsuccess))
        {
            var targetStage = StageNavigator.ResolveStageLabel(onsuccess, stage.Document!);
            sb.AppendLine($"        Resume {targetStage}");
        }
    }
}
