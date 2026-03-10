using System.Xml.Linq;
using BPAnalyzer.CodeGen.Utilities;
using BPAnalyzer.CodeGen.FlowControl;

namespace BPAnalyzer.CodeGen;

/// <summary>
/// Generates VB.NET class structure (header, imports, singleton, data items, methods).
/// </summary>
public static class ClassGenerator
{
    /// <summary>
    /// Generates the complete class code for a BluePrism process/object.
    /// </summary>
    public static string GenerateClass(
        string processName,
        string version,
        XElement process,
        HashSet<string>? currentReferences)
    {
        var sb = new System.Text.StringBuilder();

        var processType = process.Attribute("type")?.Value;
        var isObject = processType == "object";

        // Get ProcessInfo for class documentation
        var processInfo = process.Descendants().First(e => e.Attribute("type")?.Value == "ProcessInfo");

        // Class header with ProcessInfo as class comments
        sb.AppendLine($"' Generated from BluePrism {(isObject ? "object" : "process")}: {processName}");
        sb.AppendLine($"' Version: {version}");
        sb.AppendLine($"' Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

        // Collect references and imports from ProcessInfo
        var allReferences = new HashSet<string>();
        var allImports = new HashSet<string>();

        var references = processInfo.Element("references")?.Elements("reference");
        if (references != null && references.Any())
        {
            foreach (var reference in references)
            {
                var refValue = reference.Value;
                allReferences.Add(refValue);
            }
        }

        var imports = processInfo.Element("imports")?.Elements("import");
        if (imports != null && imports.Any())
        {
            foreach (var import in imports)
            {
                var importValue = import.Value;
                allImports.Add(importValue);
            }
        }

        // Store references for project file
        if (allReferences.Any() && currentReferences != null)
        {
            currentReferences = allReferences;
        }

        // Add default imports
        sb.AppendLine();
        sb.AppendLine("Imports System");
        sb.AppendLine("Imports System.Collections.Generic");
        sb.AppendLine("Imports System.Linq");
        sb.AppendLine("Imports System.Text");
        sb.AppendLine("Imports System.Data");

        // Add imports from ProcessInfo
        if (allImports.Any())
        {
            foreach (var import in allImports)
            {
                if (import != "System" &&
                    import != "System.Collections.Generic" &&
                    import != "System.Linq" &&
                    import != "System.Text" &&
                    import != "System.Data")
                {
                    sb.AppendLine($"Imports {import}");
                }
            }
        }
        sb.AppendLine();

        sb.AppendLine($"''' <summary>");
        sb.AppendLine($"''' BluePrism {(isObject ? "object" : "process")}: {processName}");
        sb.AppendLine($"''' </summary>");
        sb.AppendLine($"Public Class {NameSanitizer.SanitizeClassName(processName)}");
        sb.AppendLine($"    Inherits BP_Base");
        sb.AppendLine();

        // Generate Singleton Instance property
        var className = NameSanitizer.SanitizeClassName(processName);
        GenerateSingletonInstance(sb, className);
        sb.AppendLine();

        // Generate global data items
        sb.AppendLine("    #Region \"Global Data Items\"");
        sb.AppendLine();
        DataItemGenerator.GenerateGlobalDataItems(process, sb);
        sb.AppendLine();
        sb.AppendLine("    #End Region");
        sb.AppendLine();

        // Generate subsheets as methods
        sb.AppendLine("    #Region \"Methods\"");
        sb.AppendLine();
        MethodGenerator.GenerateSubsheetsAsMethods(process, sb, isObject);
        sb.AppendLine();
        sb.AppendLine("    #End Region");
        sb.AppendLine();

        if (isObject)
        {
            // App Model
            sb.AppendLine("    #Region \"App Model\"");
            sb.AppendLine();
            sb.AppendLine("    Protected Application As Object"); // TODO
            sb.AppendLine();
            sb.AppendLine("    #End Region");
            sb.AppendLine();

            var globalCode = processInfo?.Element("code")?.Value ?? "";
            if (!string.IsNullOrWhiteSpace(globalCode))
            {
                // Generate global code
                sb.AppendLine("    #Region \"Global Code\"");
                sb.AppendLine();
                MethodGenerator.GenerateMethodBodyFromCodeStage(sb, processInfo!);
                sb.AppendLine();
                sb.AppendLine("    #End Region");
                sb.AppendLine();
            }

            var hasCodeStages = process.Descendants("stage").Any(x => x.Attribute("type")?.Value == "Code");
            if (hasCodeStages)
            {
                // Generate code stages as methods
                sb.AppendLine("    #Region \"Code Stages\"");
                sb.AppendLine();
                MethodGenerator.GenerateCodeStageAsMethods(process, sb);
                sb.AppendLine();
                sb.AppendLine("    #End Region");
                sb.AppendLine();
            }
        }

        // Class footer
        sb.AppendLine("End Class");

        return PostProcessCode(process, sb.ToString());
    }

    /// <summary>
    /// Generates the singleton instance property.
    /// </summary>
    private static void GenerateSingletonInstance(System.Text.StringBuilder sb, string className)
    {
        sb.AppendLine("    #Region \"Singleton Instance\"");
        sb.AppendLine();
        sb.AppendLine($"    Private Shared ReadOnly _lazyInstance As New Lazy(Of {className})(Function() New {className}())");
        sb.AppendLine();
        sb.AppendLine($"    Public Shared ReadOnly Property Instance As {className}");
        sb.AppendLine($"        Get");
        sb.AppendLine($"            Return _lazyInstance.Value");
        sb.AppendLine($"        End Get");
        sb.AppendLine($"    End Property");
        sb.AppendLine();
        sb.AppendLine("    #End Region");
    }

    /// <summary>
    /// Post-processes the generated code to remove unnecessary goto statements and labels.
    /// </summary>
    private static string PostProcessCode(XElement process, string code)
    {
        var allStages = process.Descendants().Where(e => e.Name.LocalName == "stage").ToList();

        foreach (var stage in allStages)
        {
            var stageId = stage.Attribute("stageid")?.Value;
            var stageType = stage.Attribute("type")?.Value;
            if (stageId == null || stageType == null) continue;

            var stageLabel = StageNavigator.GetLabel(stageType, stageId, stage.Document);

            // Remove Goto directly before label
            code = System.Text.RegularExpressions.Regex.Replace(code, $"GoTo {stageLabel}\\s*?( *{stageLabel}:)", "\r\n$1");

            // Remove labels without references
            var firstIndex = code.IndexOf(" " + stageLabel);
            var lastIndex = code.LastIndexOf(" " + stageLabel);
            if (firstIndex == lastIndex)
            {
                code = System.Text.RegularExpressions.Regex.Replace(code, $" * {stageLabel}:", "");
            }
        }

        // Remove multiple newlines
        code = System.Text.RegularExpressions.Regex.Replace(code, "\\s*\r\n(\r\n)+", "\r\n\r\n");

        // switch label and comment
        code = System.Text.RegularExpressions.Regex.Replace(code, "(\\w*_Label:)\\s+(' .*)", "$2\n        $1");

        return code;
    }
}
