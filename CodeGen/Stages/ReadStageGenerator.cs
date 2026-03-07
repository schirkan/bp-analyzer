using System.Xml.Linq;
using BPAnalyzer.CodeGen.FlowControl;
using BPAnalyzer.CodeGen.Utilities;

namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Generates VB.NET code for Read stages (reading values from UI elements).
/// </summary>
public class ReadStageGenerator : StageGeneratorBase
{
    public override void Generate(XElement stage, System.Text.StringBuilder sb)
    {
        var document = stage.Document;
        var steps = stage.Elements("step").ToList();

        if (steps.Count == 0)
        {
            sb.AppendLine($"        ' Read: No steps defined");
            StageNavigator.GenerateGoTo(sb, document, stage.Element("onsuccess")?.Value);
            return;
        }

        foreach (var step in steps)
        {
            var elementId = step.Element("element")?.Attribute("id")?.Value;
            var actionId = step.Element("action")?.Element("id")?.Value;
            var stageName = step.Attribute("stage")?.Value;

            var elementName = AppModelResolver.FindElementNameById(document?.Root?.Element("appdef"), elementId);

            if (string.IsNullOrEmpty(elementName) || string.IsNullOrEmpty(actionId) || string.IsNullOrEmpty(stageName)) continue;

            var sanitizedStageName = NameSanitizer.SanitizeVariableName(stageName);

            sb.AppendLine($"        {sanitizedStageName} = Application.Element(\"{elementName}\", \"{elementId}\").{actionId}()");
        }

        StageNavigator.GenerateGoTo(sb, document, stage.Element("onsuccess")?.Value);
    }
}
