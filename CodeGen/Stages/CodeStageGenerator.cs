using System.Xml.Linq;
using BPAnalyzer.CodeGen.Utilities;

namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Generates VB.NET code for Code stages (embedded code).
/// </summary>
public class CodeStageGenerator : StageGeneratorBase
{
    public override string StageType => "Code";

    public override void Generate(XElement stage, System.Text.StringBuilder sb)
    {
        var name = stage.Attribute("name")?.Value!;
        var code = stage.Element("code")?.Value;

        if (!string.IsNullOrWhiteSpace(code))
        {
            sb.AppendLine($"        ' Original code:");
            foreach (var line in code.Split('\n'))
            {
                var trimmed = line.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                {
                    sb.AppendLine($"        ' {trimmed}");
                }
            }
        }

        sb.AppendLine($"        ' TODO: Convert to VB.Net");


        var sanitizedName = NameSanitizer.SanitizeMethodName("CodeStage_" + name);
        var inputs = stage.Element("inputs")?.Elements("input").ToList() ?? new List<XElement>();
        var outputs = stage.Element("outputs")?.Elements("output").ToList() ?? new List<XElement>();

        var paramString = GenerateParameterCode(inputs, outputs);

        sb.AppendLine($"        {sanitizedName}({paramString})");

        GenerateGoTo(sb, stage.Document, stage.Element("onsuccess")?.Value);
    }
}
