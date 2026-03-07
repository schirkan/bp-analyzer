using System.Xml.Linq;
using BPAnalyzer.CodeGen.FlowControl;
using BPAnalyzer.CodeGen.Utilities;

namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Generates VB.NET code for Process stages (calling other BluePrism processes).
/// </summary>
public class ProcessStageGenerator : StageGeneratorBase
{
    public override void Generate(XElement stage, System.Text.StringBuilder sb)
    {
        var name = stage.Attribute("name")?.Value;
        var processId = stage.Element("processid")?.Value;

        // Find process name from processid by searching in all XML files
        var processName = ProcessResolver.FindProcessNameById(processId);

        if (!string.IsNullOrEmpty(processName))
        {
            var className = NameSanitizer.SanitizeClassName(processName);

            var inputs = stage.Element("inputs")?.Elements("input").ToList() ?? new List<XElement>();
            var outputs = stage.Element("outputs")?.Elements("output").ToList() ?? new List<XElement>();

            var paramString = GenerateParameterCode(inputs, outputs);

            if (!string.IsNullOrEmpty(paramString))
                sb.AppendLine($"        {className}.Instance.Main({paramString})");
            else
                sb.AppendLine($"        {className}.Instance.Main()");
        }
        else
        {
            sb.AppendLine($"        ' Call Process: {name}");
            sb.AppendLine($"        ' TODO: Implement process call (processid: {processId})");
        }

        StageNavigator.GenerateGoTo(sb, stage.Document, stage.Element("onsuccess")?.Value);
    }
}
