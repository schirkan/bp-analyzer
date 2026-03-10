using System.Xml.Linq;
using BPAnalyzer.CodeGen.FlowControl;
using BPAnalyzer.CodeGen.Utilities;

namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Generates VB.NET code for ChoiceStart and ChoiceEnd stages.
/// ChoiceStart generates a Select Case statement for multiple conditions.
/// ChoiceEnd represents the Case Else branch.
/// </summary>
public class ChoiceStageGenerator : StageGeneratorBase
{
    public override void Generate(XElement stage, System.Text.StringBuilder sb)
    {
        var stageType = stage.Attribute("type")?.Value;

        // ChoiceStart - generate Select Case
        var groupId = stage.Element("groupid")?.Value;
        var name = stage.Attribute("name")?.Value;

        var doc = stage.Document;
        XElement? choiceEnd = null;
        if (!string.IsNullOrEmpty(groupId))
        {
            choiceEnd = doc?.Descendants()
                .FirstOrDefault(e => e.Attribute("type")?.Value == "ChoiceEnd" && e.Element("groupid")?.Value == groupId);
        }

        var choices = stage.Element("choices")?.Elements("choice").ToList();

        // Generate Select Case statement
        sb.AppendLine($"        Select Case True");

        if (choices != null && choices.Any())
        {
            foreach (var choice in choices)
            {
                var choiceName = choice.Element("name")?.Value;
                var ontrue = choice.Element("ontrue")?.Value;
                var expression = choice.Attribute("expression")?.Value;

                // Generate expression from choice element and condition
                var choiceExpression = (!string.IsNullOrEmpty(expression)) ? ExpressionParser.FormatExpression(expression) : "True";

                sb.AppendLine($"            Case {choiceExpression} ' {choiceName}");
                StageNavigator.GenerateGoTo(sb, stage.Document, ontrue, 16);
            }
        }

        sb.AppendLine($"        End Select");

        // Handle ChoiceEnd
        if (choiceEnd != null)
        {
            StageNavigator.GenerateGoTo(sb, stage.Document, choiceEnd.Element("onsuccess")?.Value);
        }
    }
}
