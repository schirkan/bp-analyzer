using System.Xml.Linq;
using BPAnalyzer.CodeGen.FlowControl;
using BPAnalyzer.CodeGen.Utilities;

namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Generates VB.NET code for Start stage (input parameter handling).
/// </summary>
public class StartStageGenerator : StageGeneratorBase
{
    public override void Generate(XElement stage, System.Text.StringBuilder sb)
    {
        var inputs = stage.Element("inputs")?.Elements("input").ToList();

        if (inputs != null && inputs.Any())
        {
            var hasInputAssignments = false;
            foreach (var input in inputs)
            {
                var inputName = input.Attribute("name")?.Value;
                var stageName = input.Attribute("stage")?.Value;
                var inputType = input.Attribute("type")?.Value ?? "text";
                var isValueType = TypeMapper.IsValueType(inputType);

                if (!string.IsNullOrEmpty(inputName) && !string.IsNullOrEmpty(stageName) && stageName != inputName)
                {
                    if (!hasInputAssignments)
                    {
                        sb.AppendLine("        ' Initialize local variables with input values");
                        hasInputAssignments = true;
                    }

                    // Only use HasValue/Value pattern for value types (nullable types)
                    // For reference types (String, DataTable), just assign directly
                    if (isValueType)
                    {
                        sb.AppendLine($"        If {NameSanitizer.SanitizeVariableName(inputName)}.HasValue Then");
                        sb.AppendLine($"            {NameSanitizer.SanitizeVariableName(stageName)} = {NameSanitizer.SanitizeVariableName(inputName)}.Value");
                        sb.AppendLine($"        End If");
                    }
                    else
                    {
                        // For reference types, check for Nothing
                        sb.AppendLine($"        If {NameSanitizer.SanitizeVariableName(inputName)} IsNot Nothing Then");
                        sb.AppendLine($"            {NameSanitizer.SanitizeVariableName(stageName)} = {NameSanitizer.SanitizeVariableName(inputName)}");
                        sb.AppendLine($"        End If");
                    }
                }
            }
            if (hasInputAssignments) sb.AppendLine();
        }

        // Generate GoTo
        StageNavigator.GenerateGoTo(sb, stage.Document, stage.Element("onsuccess")?.Value);
    }
}
