using System.Xml.Linq;

namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Generates VB.NET code for Exception stages (throwing exceptions).
/// </summary>
public class ExceptionStageGenerator : StageGeneratorBase
{
    public override void Generate(XElement stage, System.Text.StringBuilder sb)
    {
        var exceptionElement = stage.Element("exception");
        var detail = exceptionElement?.Attribute("detail")?.Value;
        var exceptionType = exceptionElement?.Attribute("type")?.Value;
        var useCurrent = exceptionElement?.Attribute("usecurrent")?.Value?.ToLower() == "yes";

        // Check if usecurrent="yes" - then rethrow the stored exception
        if (useCurrent)
        {
            sb.AppendLine($"        Throw GetLastException()");
        }
        else
        {
            sb.AppendLine($"        Throw New BP_Exception(\"{exceptionType}\", {detail})");
        }
    }
}
