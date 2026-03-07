using System.Xml.Linq;
using BPAnalyzer.CodeGen.Stages;
using BPAnalyzer.CodeGen.Utilities;
using BPAnalyzer.CodeGen.FlowControl;

namespace BPAnalyzer.CodeGen;

/// <summary>
/// Generates VB.NET methods from BluePrism subsheets/stages.
/// </summary>
public static class MethodGenerator
{
    public static void GenerateCodeStageAsMethods(XElement process, System.Text.StringBuilder sb)
    {
        var codeStages = process.Descendants().Where(e => e.Name.LocalName == "stage" && e.Attribute("type")?.Value == "Code").ToList();

        foreach (var codeStage in codeStages)
        {
            var name = codeStage.Attribute("name")?.Value ?? "";
            var methodName = NameSanitizer.SanitizeMethodName("CodeStage_" + name);

            GenerateMethod(sb, methodName, "Private", name, [codeStage]);
        }
    }

    /// <summary>
    /// Generates all subsheets as methods (Main, Constructor, Destructor, and custom methods).
    /// </summary>
    public static void GenerateSubsheetsAsMethods(XElement process, System.Text.StringBuilder sb, bool isObject = false)
    {
        var subsheets = process.Descendants("subsheet").ToList();
        var allStages = process.Descendants().Where(e => e.Name.LocalName == "stage").ToList();
        var stagesWithoutSubsheet = allStages.Where(e => string.IsNullOrEmpty(e.Element("subsheetid")?.Value)).ToList();

        if (stagesWithoutSubsheet.Any())
        {
            var narrative = process.Attribute("narrative")?.Value;

            if (isObject)
            {
                // For objects: generate constructor for stages without subsheetid (initialization)
                GenerateMethod(sb, "New", "Public", narrative ?? "Constructor - object initialization", stagesWithoutSubsheet);
            }
            else
            {
                // For processes: generate Main method for stages without subsheetid
                GenerateMethod(sb, "Main", "Public", narrative ?? "Main process method", stagesWithoutSubsheet);
            }
        }

        // Generate each subsheet as a method
        foreach (var subsheet in subsheets)
        {
            try
            {
                var subsheetId = subsheet.Attribute("subsheetid")?.Value;
                var subsheetName = subsheet.Element("name")?.Value ?? "Unnamed";
                var subsheetType = subsheet.Attribute("type")?.Value ?? "Normal";
                var subsheetStages = allStages.Where(e => e.Element("subsheetid")?.Value == subsheetId).ToList();
                var published = subsheet.Attribute("published")?.Value?.ToLower() == "true";
                var subSheetInfoStage = subsheetStages.FirstOrDefault(s => s.Attribute("type")?.Value == "SubSheetInfo");
                var methodNarrative = subSheetInfoStage?.Element("narrative")?.Value;
                var isDestructor = subsheetType == "CleanUp";
                var methodVisibility = isDestructor ? "Protected Overrides" : published ? "Public" : "Private";
                var methodName = isDestructor ? "Finalize" : NameSanitizer.SanitizeMethodName(subsheetName);
                GenerateMethod(sb, methodName, methodVisibility, methodNarrative, subsheetStages);
            }
            catch (Exception ex)
            {
                sb.AppendLine($"    ' Error generating subsheet: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Generates a single method from stages.
    /// </summary>
    public static void GenerateMethod(
        System.Text.StringBuilder sb,
        string methodName,
        string methodVisibility,
        string? methodNarrative,
        List<XElement> stages)
    {
        var startStage = stages.FirstOrDefault(s => s.Attribute("type")?.Value == "Start");
        var endStage = stages.FirstOrDefault(s => s.Attribute("type")?.Value == "End");
        XElement? codeStage = null;

        if (startStage == null || endStage == null)
        {
            codeStage = stages.FirstOrDefault(s => s.Attribute("type")?.Value == "Code");
            if (codeStage == null) return;
            startStage = codeStage;
            endStage = codeStage;
        }

        // Collect input/output parameter names (case-insensitive for duplicate detection)
        var inputParamNames = startStage.Element("inputs")?.Elements("input")
            .Select(i => i.Attribute("name")?.Value?.Trim())
            .Where(n => !string.IsNullOrEmpty(n))
            .Select(n => n!.ToLower())
            .ToHashSet(StringComparer.OrdinalIgnoreCase) ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var outputParamNames = endStage.Element("outputs")?.Elements("output")
            .Select(o => o.Attribute("name")?.Value?.Trim())
            .Where(n => !string.IsNullOrEmpty(n))
            .Select(n => n!.ToLower())
            .ToHashSet(StringComparer.OrdinalIgnoreCase) ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var paramNames = inputParamNames.Union(outputParamNames).ToHashSet();

        var inputs = startStage.Element("inputs")?.Elements("input").ToList() ?? new List<XElement>();
        var outputs = endStage.Element("outputs")?.Elements("output").ToList() ?? new List<XElement>();

        // Method documentation
        sb.AppendLine($"    ''' <summary>");
        var description = GetMethodDescription(methodName, methodNarrative);
        foreach (var line in description.Split('\n'))
        {
            sb.AppendLine($"    ''' {line}");
        }
        sb.AppendLine($"    ''' </summary>");

        // Parameter descriptions        
        foreach (var input in inputs)
        {
            var inputName = input.Attribute("name")?.Value ?? "";
            var inputNarrative = input.Attribute("narrative")?.Value;
            if (!string.IsNullOrEmpty(inputNarrative))
            {
                // Remove special characters that break VB.NET doc comments
                var cleanNarrative = new string(inputNarrative.Where(c => !"<>&'\"".Contains(c)).ToArray());
                sb.AppendLine($"    ''' <param name=\"{NameSanitizer.SanitizeVariableName(inputName)}\">{cleanNarrative}</param>");
            }
        }

        foreach (var output in outputs)
        {
            var outputName = output.Attribute("name")?.Value ?? "";
            var outputNarrative = output.Attribute("narrative")?.Value;
            if (!string.IsNullOrEmpty(outputNarrative))
            {
                // Remove special characters that break VB.NET doc comments
                var cleanNarrative = new string(outputNarrative.Where(c => !"<>&'\"".Contains(c)).ToArray());
                sb.AppendLine($"    ''' <param name=\"{NameSanitizer.SanitizeVariableName(outputName)}\">{cleanNarrative}</param>");
            }
        }

        // Method signature
        var methodType = $"{methodVisibility} Sub";
        sb.Append($"    {methodType} {methodName}(");
        var paramList = new List<string>();

        foreach (var input in inputs)
        {
            var inputName = input.Attribute("name")?.Value ?? "";
            var inputType = input.Attribute("type")?.Value ?? "text";
            var vbType = TypeMapper.MapDataType(inputType);
            var sanitizedName = NameSanitizer.SanitizeVariableName(inputName);
            var defaultValue = TypeMapper.GetOptionalDefaultValue(inputType);
            var nullableMarker = TypeMapper.IsValueType(inputType) ? "?" : "";

            // Skip if this input name exists in outputs (case-insensitive)
            if (outputParamNames.Contains(inputName.ToLower())) continue;
            paramList.Add($"Optional ByVal {sanitizedName} As {vbType}{nullableMarker} = {defaultValue}");
        }

        foreach (var output in outputs)
        {
            var outputName = output.Attribute("name")?.Value ?? "";
            var outputType = output.Attribute("type")?.Value ?? "text";
            var vbType = TypeMapper.MapDataType(outputType);
            var sanitizedName = NameSanitizer.SanitizeVariableName(outputName);
            var defaultValue = TypeMapper.GetOptionalDefaultValue(outputType);
            var nullableMarker = TypeMapper.IsValueType(outputType) ? "?" : "";

            paramList.Add($"Optional ByRef {sanitizedName} As {vbType}{nullableMarker} = {defaultValue}");
        }
        sb.AppendLine(string.Join(", ", paramList) + ")");
        sb.AppendLine();

        if (codeStage != null)
        {
            GenerateMethodBodyFromCodeStage(sb, codeStage);
        }
        else
        {
            GenerateMethodBodyFromStages(sb, stages, endStage, paramNames);
        }

        sb.AppendLine("    End Sub");
        sb.AppendLine();
    }

    /// <summary>
    /// Generate method body from code stage
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="codeStage"></param>
    public static void GenerateMethodBodyFromCodeStage(System.Text.StringBuilder sb, XElement codeStage)
    {
        var code = codeStage.Element("code")?.Value;

        if (string.IsNullOrWhiteSpace(code)) return;
        foreach (var line in code.Split('\n'))
        {
            sb.AppendLine($"        {line}");
        }
        sb.AppendLine();
    }

    /// <summary>
    /// Generate method body from stages
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="stages"></param>
    /// <param name="endStage"></param>
    /// <param name="paramNames"></param>
    private static void GenerateMethodBodyFromStages(System.Text.StringBuilder sb, List<XElement> stages, XElement endStage, HashSet<string> paramNames)
    {
        // Local variables and initialization
        DataItemGenerator.GenerateLocalDataItems(sb, stages, paramNames);
        DataItemGenerator.GenerateInitialValueInitialization(sb, stages);

        var sortedStages = FlowController.SortStagesByExecutionOrder(stages);

        foreach (var stage in sortedStages)
        {
            var stageType = stage.Attribute("type")?.Value!;
            var stageName = stage.Attribute("name")?.Value!;
            var stageId = stage.Attribute("stageid")?.Value!;

            if (string.IsNullOrEmpty(stageName)) continue;
            if (FlowController.SkipStages.Contains(stageType)) continue;

            // Start stage
            if (stageType == "Start")
            {
                GenerateStartStageForMethod(stage, sb);
                continue;
            }

            // Generate label
            var labelName = StageNavigator.GetLabel(stageType, stageId);
            sb.AppendLine($"        {labelName}:"); // will be switched in post processing
            sb.AppendLine($"        ' {stageName}");

            // Add error handling
            var recoverStage = stages.FirstOrDefault(s => s.Attribute("type")?.Value == "Recover");
            if (stageType != "Recover" && stageType != "Resume" && stageType != "Note" && recoverStage != null)
            {
                var recoverStageId = recoverStage.Attribute("stageid")?.Value;
                if (!string.IsNullOrEmpty(recoverStageId))
                {
                    var recoverLabel = StageNavigator.GetLabel("Recover", recoverStageId);
                    sb.AppendLine($"        On Error GoTo {recoverLabel}");
                }
            }

            // Generate stage code
            var generator = StageGeneratorFactory.GetGenerator(stageType);
            if (generator != null)
            {
                generator.Generate(stage, sb);
            }
            else
            {
                sb.AppendLine($"        ' TODO: Implement stage type '{stageType}'");
            }

            sb.AppendLine();
        }

        // Generate End label
        if (endStage != null)
        {
            var endStageId = endStage.Attribute("stageid")?.Value ?? "";
            var endLabelName = StageNavigator.GetLabel("End", endStageId);
            sb.AppendLine($"        {endLabelName}:");

            var endOutputs = endStage.Element("outputs")?.Elements("output");
            if (endOutputs != null && endOutputs.Any())
            {
                foreach (var output in endOutputs)
                {
                    var outputName = output.Attribute("name")?.Value!;
                    var stageAttr = output.Attribute("stage")?.Value;
                    if (!string.IsNullOrEmpty(stageAttr) && stageAttr != outputName)
                    {
                        sb.AppendLine($"        {NameSanitizer.SanitizeVariableName(outputName)} = {NameSanitizer.SanitizeVariableName(stageAttr)}");
                    }
                }
            }
            sb.AppendLine();
        }
    }

    /// <summary>
    /// Generates code for Start stage (input parameter handling).
    /// </summary>
    private static void GenerateStartStageForMethod(XElement stage, System.Text.StringBuilder sb)
    {
        var inputs = stage.Element("inputs")?.Elements("input").ToList();

        if (inputs != null && inputs.Any())
        {
            var hasInputAssignments = false;
            foreach (var input in inputs)
            {
                var inputName = input.Attribute("name")?.Value;
                var stageName = input.Attribute("stage")?.Value;
                var inputType = input.Attribute("type")?.Value ?? "text";
                var isValueType = TypeMapper.IsValueType(inputType);

                if (!string.IsNullOrEmpty(inputName) && !string.IsNullOrEmpty(stageName) && stageName != inputName)
                {
                    if (!hasInputAssignments)
                    {
                        sb.AppendLine("        ' Initialize local variables with input values");
                        hasInputAssignments = true;
                    }

                    // Only use HasValue/Value pattern for value types (nullable types)
                    // For reference types (String, DataTable), just assign directly
                    if (isValueType)
                    {
                        sb.AppendLine($"        If {NameSanitizer.SanitizeVariableName(inputName)}.HasValue Then");
                        sb.AppendLine($"            {NameSanitizer.SanitizeVariableName(stageName)} = {NameSanitizer.SanitizeVariableName(inputName)}.Value");
                        sb.AppendLine($"        End If");
                    }
                    else
                    {
                        // For reference types, check for Nothing
                        sb.AppendLine($"        If {NameSanitizer.SanitizeVariableName(inputName)} IsNot Nothing Then");
                        sb.AppendLine($"            {NameSanitizer.SanitizeVariableName(stageName)} = {NameSanitizer.SanitizeVariableName(inputName)}");
                        sb.AppendLine($"        End If");
                    }
                }
            }
            if (hasInputAssignments) sb.AppendLine();
        }

        // Generate GoTo
        StageNavigator.GenerateGoTo(sb, stage.Document, stage.Element("onsuccess")?.Value);
    }

    /// <summary>
    /// Gets a uniform method description based on method type.
    /// </summary>
    private static string GetMethodDescription(string methodName, string? methodNarrative)
    {
        if (!string.IsNullOrEmpty(methodNarrative) && methodNarrative.Trim().Length > 0)
        {
            return methodNarrative.Trim();
        }

        return methodName.ToLower() switch
        {
            "main" => "Main process entry point",
            "new" => "Constructor (Initialize) - called when object is created",
            "finalize" => "Destructor (CleanUp) - called when object is disposed",
            _ => $"BluePrism page: {methodName}"
        };
    }
}
