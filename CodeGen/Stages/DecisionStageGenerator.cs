using System.Xml.Linq;
using BPAnalyzer.CodeGen.Utilities;

namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Generates VB.NET code for Decision stages (if-then-else logic).
/// </summary>
public class DecisionStageGenerator : StageGeneratorBase
{
    public override string StageType => "Decision";

    public override void Generate(XElement stage, System.Text.StringBuilder sb)
    {
        var expression = stage.Element("decision")?.Attribute("expression")?.Value;
        var ontrue = stage.Element("ontrue")?.Value;
        var onfalse = stage.Element("onfalse")?.Value;

        var formattedExpression = ExpressionParser.FormatExpression(expression);
        sb.AppendLine($"        If {formattedExpression} Then");

        if (!string.IsNullOrEmpty(ontrue))
        {
            GenerateGoTo(sb, stage.Document, ontrue, 12);
        }
        if (!string.IsNullOrEmpty(onfalse))
        {
            sb.AppendLine($"        Else");
            GenerateGoTo(sb, stage.Document, onfalse, 12);
        }
        sb.AppendLine($"        End If");
    }
}
