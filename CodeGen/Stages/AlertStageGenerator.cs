using System.Xml.Linq;
using BPAnalyzer.CodeGen.Utilities;

namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Generates VB.NET code for Alert stages.
/// </summary>
public class AlertStageGenerator : StageGeneratorBase
{
    public override string StageType => "Alert";

    public override void Generate(XElement stage, System.Text.StringBuilder sb)
    {
        var name = stage.Attribute("name")?.Value;
        var alertElement = stage.Element("alert");
        var expression = alertElement?.Attribute("expression")?.Value;

        if (!string.IsNullOrEmpty(expression))
        {
            // Format the expression (remove quotes if present)
            var formattedExpression = ExpressionParser.FormatExpression(expression);

            sb.AppendLine($"        BP_Alert.Notify({formattedExpression})");
        }
        else
        {
            sb.AppendLine($"        ' Alert: {name}");
        }

        GenerateGoTo(sb, stage.Document, stage.Element("onsuccess")?.Value);
    }
}
