using System.Xml.Linq;
using BPAnalyzer.CodeGen.FlowControl;
using BPAnalyzer.CodeGen.Utilities;

namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Generates VB.NET code for SubSheet stages (calling other subsheets/actions within the same object).
/// Similar to Action stage but calls internal subsheets.
/// </summary>
public class SubSheetStageGenerator : StageGeneratorBase
{
    public override void Generate(XElement stage, System.Text.StringBuilder sb)
    {
        var name = stage.Attribute("name")?.Value;
        var processId = stage.Element("processid")?.Value;

        // Get the subsheet name from the XML by looking up the subsheet definition
        var subsheetName = SubsheetResolver.FindSubsheetName(stage.Document, processId);

        if (!string.IsNullOrEmpty(subsheetName))
        {
            var methodName = NameSanitizer.SanitizeMethodName(subsheetName);

            var inputs = stage.Element("inputs")?.Elements("input").ToList() ?? new List<XElement>();
            var outputs = stage.Element("outputs")?.Elements("output").ToList() ?? new List<XElement>();

            var paramString = GenerateParameterCode(inputs, outputs);

            // SubSheet calls are internal method calls within the same class
            sb.AppendLine($"        {methodName}({paramString})");
        }

        StageNavigator.GenerateGoTo(sb, stage.Document, stage.Element("onsuccess")?.Value);
    }
}
