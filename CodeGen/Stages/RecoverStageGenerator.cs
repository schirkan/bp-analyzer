using System.Xml.Linq;
using BPAnalyzer.CodeGen.FlowControl;

namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Generates VB.NET code for Recover stages (exception handling - store).
/// </summary>
public class RecoverStageGenerator : StageGeneratorBase
{
    public override void Generate(XElement stage, System.Text.StringBuilder sb)
    {
        sb.AppendLine($"        StoreException()");

        StageNavigator.GenerateGoTo(sb, stage.Document, stage.Element("onsuccess")?.Value);
    }
}
