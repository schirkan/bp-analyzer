using System.Xml.Linq;
using BPAnalyzer.CodeGen.Utilities;

namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Generates VB.NET code for Navigate stages (UI automation).
/// </summary>
public class NavigateStageGenerator : StageGeneratorBase
{
    public override string StageType => "Navigate";

    public override void Generate(XElement stage, System.Text.StringBuilder sb)
    {
        var document = stage.Document;
        var steps = stage.Elements("step").ToList();

        if (steps.Count == 0)
        {
            sb.AppendLine($"        ' Navigate: No steps defined");
            GenerateGoTo(sb, document, stage.Element("onsuccess")?.Value);
            return;
        }

        foreach (var step in steps)
        {
            var elementId = step.Element("element")?.Attribute("id")?.Value;
            var actionId = step.Element("action")?.Element("id")?.Value;
            var actionArgs = step.Element("action")?.Element("arguments")?.Elements("argument").ToList() ?? new List<XElement>();

            var elementName = AppModelResolver.FindElementNameById(document?.Root?.Element("appdef"), elementId);

            if (string.IsNullOrEmpty(elementName) || string.IsNullOrEmpty(actionId)) continue;

            var sanitizedActionName = NameSanitizer.SanitizeMethodName(actionId);

            // Generate parameter code for action arguments
            var paramList = new List<string>();
            foreach (var arg in actionArgs)
            {
                var argId = arg.Element("id")?.Value;
                var argValue = arg.Element("value")?.Value;
                if (!string.IsNullOrEmpty(argId) && !string.IsNullOrEmpty(argValue))
                {
                    paramList.Add($"{argId}:=\"{argValue}\"");
                }
            }

            var paramString = string.Join(", ", paramList);

            sb.AppendLine($"        Application.Element(\"{elementName}\", \"{elementId}\").{sanitizedActionName}({paramString})");
        }

        GenerateGoTo(sb, document, stage.Element("onsuccess")?.Value);
    }
}
