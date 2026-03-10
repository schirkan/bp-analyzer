using System.Xml.Linq;
using BPAnalyzer.CodeGen.FlowControl;
using BPAnalyzer.CodeGen.Utilities;

namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Generates VB.NET code for Write stages (writing values to UI elements).
/// </summary>
public class WriteStageGenerator : StageGeneratorBase
{
    public override void Generate(XElement stage, System.Text.StringBuilder sb)
    {
        var document = stage.Document;
        var steps = stage.Elements("step").ToList();

        if (steps.Count == 0)
        {
            sb.AppendLine($"        ' Write: No steps defined");
            StageNavigator.GenerateGoTo(sb, document, stage.Element("onsuccess")?.Value);
            return;
        }

        foreach (var step in steps)
        {
            var elementId = step.Element("element")?.Attribute("id")?.Value;
            var expr = step.Attribute("expr")?.Value;

            var elementName = AppModelResolver.FindElementNameById(document?.Root?.Element("appdef"), elementId);

            if (string.IsNullOrEmpty(elementName) || string.IsNullOrEmpty(expr)) continue;

            var formattedExpr = ExpressionParser.FormatExpression(expr);

            sb.AppendLine($"        Application.Element(\"{elementName}\").Write({formattedExpr})");
            // sb.AppendLine($"        Application.Element(\"{elementName}\", \"{elementId}\").Write({formattedExpr})");
        }

        StageNavigator.GenerateGoTo(sb, document, stage.Element("onsuccess")?.Value);
    }
}
