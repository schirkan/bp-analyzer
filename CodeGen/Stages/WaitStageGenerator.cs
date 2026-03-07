using System.Xml.Linq;
using BPAnalyzer.CodeGen.FlowControl;
using BPAnalyzer.CodeGen.Utilities;

namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Generates VB.NET code for WaitStart stages (waiting for conditions).
/// </summary>
public class WaitStageGenerator : StageGeneratorBase
{
    public override void Generate(XElement stage, System.Text.StringBuilder sb)
    {
        var timeout = stage.Element("timeout")?.Value ?? "0";
        var groupId = stage.Element("groupid")?.Value;

        var doc = stage.Document;
        XElement? waitEnd = null;
        if (!string.IsNullOrEmpty(groupId))
        {
            waitEnd = doc?.Descendants()
                .FirstOrDefault(e => e.Attribute("type")?.Value == "WaitEnd" && e.Element("groupid")?.Value == groupId);
        }

        var choices = stage.Element("choices")?.Elements("choice").ToList();
        sb.AppendLine($"        ' Wait {timeout} seconds for condition with {choices?.Count} choice(s)");
        sb.AppendLine($"        Select Case True");
        if (choices != null && choices.Any())
        {
            foreach (var choice in choices)
            {
                var choiceName = choice.Element("name")?.Value;
                var ontrue = choice.Element("ontrue")?.Value;

                // Generate expression from choice element and condition
                var choiceExpression = GenerateWaitChoiceExpression(choice);

                sb.AppendLine($"            Case {choiceExpression} ' {choiceName}");
                StageNavigator.GenerateGoTo(sb, stage.Document, ontrue, 16);
            }
        }
        if (waitEnd != null)
        {
            var onsuccess = waitEnd.Element("onsuccess")?.Value;
            if (!string.IsNullOrEmpty(onsuccess))
            {
                sb.AppendLine($"            Case Else");
                StageNavigator.GenerateGoTo(sb, stage.Document, onsuccess, 16);
            }
        }
        sb.AppendLine($"        End Select");
    }

    /// <summary>
    /// Generates the expression for a WaitStart choice.
    /// </summary>
    private string GenerateWaitChoiceExpression(XElement choice)
    {
        var element = choice.Element("element");
        var elementId = element?.Attribute("id")?.Value;
        var condition = choice.Element("condition");
        var conditionId = condition?.Element("id")?.Value;
        var compareType = choice.Attribute("comparetype")?.Value;
        var replyValue = choice.Attribute("reply")?.Value;

        var elementName = AppModelResolver.FindElementNameById(choice.Document?.Root?.Element("appdef"), elementId);

        // If we have element and condition, generate proper expression
        if (!string.IsNullOrEmpty(elementName) && !string.IsNullOrEmpty(conditionId))
        {
            var comparison = compareType?.ToLower() switch
            {
                "equal" => "=",
                "notequal" => "<>",
                "lessthan" => "<",
                "greaterthan" => ">",
                "lessthanorequal" => "<=",
                "greaterthanorequal" => ">=",
                _ => "="
            };

            // Determine the value to compare (reply attribute or default True)
            var compareValue = replyValue?.ToLower() == "true" ? "True" : "False";

            return $"Application.Element(\"{elementName}\", \"{elementId}\").{conditionId} {comparison} {compareValue}";
        }

        // Fallback: return the expression attribute if present
        var expression = choice.Attribute("expression")?.Value;
        return ExpressionParser.FormatExpression(expression);
    }
}
