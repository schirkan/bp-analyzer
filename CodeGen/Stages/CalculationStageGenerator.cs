using System.Xml.Linq;
using BPAnalyzer.CodeGen.Utilities;

namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Generates VB.NET code for Calculation stages (assignments).
/// </summary>
public class CalculationStageGenerator : StageGeneratorBase
{
    public override string StageType => "Calculation";

    public override void Generate(XElement stage, System.Text.StringBuilder sb)
    {
        var calculation = stage.Element("calculation")?.Attribute("expression")?.Value;
        var stageName = stage.Element("calculation")?.Attribute("stage")?.Value;

        var formattedExpression = ExpressionParser.FormatExpression(calculation);
        var formattedStageName = NameSanitizer.SanitizeVariableName(stageName ?? "");
        sb.AppendLine($"        {formattedStageName} = {formattedExpression}");

        GenerateGoTo(sb, stage.Document, stage.Element("onsuccess")?.Value);
    }
}
