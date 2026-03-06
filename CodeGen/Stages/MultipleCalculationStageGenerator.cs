using System.Xml.Linq;
using BPAnalyzer.CodeGen.Utilities;

namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Generates VB.NET code for MultipleCalculation stages (multiple assignments).
/// </summary>
public class MultipleCalculationStageGenerator : StageGeneratorBase
{
    public override string StageType => "MultipleCalculation";

    public override void Generate(XElement stage, System.Text.StringBuilder sb)
    {
        var calculations = stage.Element("steps")?.Elements("calculation");
        if (calculations != null)
        {
            foreach (var calc in calculations)
            {
                var expression = calc.Attribute("expression")?.Value;
                var formattedExpression = ExpressionParser.FormatExpression(expression);
                var targetStage = calc.Attribute("stage")?.Value;
                var formattedStageName = NameSanitizer.SanitizeVariableName(targetStage ?? "");
                sb.AppendLine($"        {formattedStageName} = {formattedExpression}");
            }
        }

        GenerateGoTo(sb, stage.Document, stage.Element("onsuccess")?.Value);
    }
}
