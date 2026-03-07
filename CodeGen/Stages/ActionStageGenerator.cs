using System.Xml.Linq;
using BPAnalyzer.CodeGen.FlowControl;
using BPAnalyzer.CodeGen.Utilities;

namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Generates VB.NET code for Action stages (calling other BluePrism objects).
/// </summary>
public class ActionStageGenerator : StageGeneratorBase
{
    public override void Generate(XElement stage, System.Text.StringBuilder sb)
    {
        var resource = stage.Element("resource");
        var objectName = resource?.Attribute("object")?.Value;
        var action = resource?.Attribute("action")?.Value;

        if (!string.IsNullOrEmpty(objectName) && !string.IsNullOrEmpty(action))
        {
            var className = NameSanitizer.SanitizeClassName(objectName);
            var sanitizedActionName = NameSanitizer.SanitizeMethodName(action);

            var inputs = stage.Element("inputs")?.Elements("input").ToList() ?? new List<XElement>();
            var outputs = stage.Element("outputs")?.Elements("output").ToList() ?? new List<XElement>();

            var paramString = GenerateParameterCode(inputs, outputs);

            sb.AppendLine($"        {className}.Instance.{sanitizedActionName}({paramString})");
        }

        StageNavigator.GenerateGoTo(sb, stage.Document, stage.Element("onsuccess")?.Value);
    }
}
