using System.Xml.Linq;

namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Generates VB.NET code for Navigate stages (UI automation).
/// </summary>
public class NavigateStageGenerator : StageGeneratorBase
{
    public override string StageType => "Navigate";

    public override void Generate(XElement stage, System.Text.StringBuilder sb)
    {
        sb.AppendLine($"        ' Navigate: UI automation");
        sb.AppendLine($"        ' TODO: Implement");

        GenerateGoTo(sb, stage.Document, stage.Element("onsuccess")?.Value);
    }


    /// <summary>
    /// Finds the element name by ID using LINQ (descendants).
    /// </summary>
    private static string? FindElementNameById(XElement parent, string elementId)
    {
        return parent.Descendants("element")
            .Where(e => e.Element("id")?.Value.Equals(elementId, StringComparison.OrdinalIgnoreCase) == true)
            .Select(e => e.Attribute("name")?.Value)
            .FirstOrDefault();
    }
}
