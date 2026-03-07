using System.Xml.Linq;
using BPAnalyzer.CodeGen.FlowControl;

namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Generates VB.NET code for Note stages (comments/annotations).
/// </summary>
public class NoteStageGenerator : StageGeneratorBase
{
    public override void Generate(XElement stage, System.Text.StringBuilder sb)
    {
        var narrative = stage.Element("narrative")?.Value;

        if (!string.IsNullOrEmpty(narrative))
        {
            // Split the narrative into lines and add comment prefix to each line
            var lines = narrative.Split('\n');
            foreach (var line in lines)
            {
                var trimmed = line.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                {
                    sb.AppendLine($"        ' {trimmed}");
                }
            }
        }

        StageNavigator.GenerateGoTo(sb, stage.Document, stage.Element("onsuccess")?.Value);
    }
}
