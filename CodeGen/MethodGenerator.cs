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
    /// <summary>
    /// Generates all subsheets as methods (Main, Constructor, Destructor, and custom methods).
    /// </summary>
    public static void GenerateSubsheetsAsMethods(XElement process, System.Text.StringBuilder sb, bool isObject = false)
    {
        var subsheets = process.Descendants("subsheet").ToList();
        var allStages = process.Descendants()
            .Where(e => e.Name.LocalName == "stage")
            .ToList();

        // For objects: generate constructor for stages without subsheetid (initialization)
        if (isObject)
        {
            var globalStages = allStages
                .Where(e => string.IsNullOrEmpty(e.Element("subsheetid")?.Value))
                .ToList();

            if (globalStages.Any())
            {
                GenerateMethod(sb, "New", "Public", "Constructor - initialization code from stages without subsheet", globalStages, true);
                sb.AppendLine();
            }
        }
        else
        {
            // For processes: generate Main method for stages without subsheetid
            var mainStages = allStages
                .Where(e => string.IsNullOrEmpty(e.Element("subsheetid")?.Value))
                .ToList();

            if (mainStages.Any())
            {
                GenerateMethod(sb, "Main", "Public", "Main process method (stages without subsheet)", mainStages, false);
                sb.AppendLine();
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

                var subsheetStages = allStages
                    .Where(e => e.Element("subsheetid")?.Value == subsheetId)
                    .ToList();

                var published = subsheet.Attribute("published")?.Value?.ToLower() == "true";
                var subSheetInfoStage = subsheetStages.FirstOrDefault(s => s.Attribute("type")?.Value == "SubSheetInfo");
                var methodNarrative = subSheetInfoStage?.Element("narrative")?.Value;

                string methodVisibility;
                var isDestructor = subsheetType == "CleanUp";

                if (isDestructor)
                {
                    methodVisibility = "Protected Overrides";
                }
                else if (isObject)
                {
                    methodVisibility = published ? "Public" : "Private";
                }
                else
                {
                    methodVisibility = "Private";
                }

                var methodName = isDestructor ? "Finalize" : NameSanitizer.SanitizeMethodName(subsheetName);
                GenerateMethod(sb, methodName, methodVisibility, methodNarrative, subsheetStages, isDestructor);
                sb.AppendLine();
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
        List<XElement> stages,
        bool isConstructor)
    {
        var startStage = stages.FirstOrDefault(s => s.Attribute("type")?.Value == "Start");
        var endStage = stages.FirstOrDefault(s => s.Attribute("type")?.Value == "End");

        if (startStage == null || endStage == null) return;

        // Collect input/output parameter names
        var inputParamNames = startStage.Element("inputs")?.Elements("input")
            .Select(i => i.Attribute("name")?.Value?.ToLower())
            .Where(n => !string.IsNullOrEmpty(n))
            .ToHashSet() as HashSet<string> ?? [];

        var outputParamNames = endStage.Element("outputs")?.Elements("output")
            .Select(o => o.Attribute("name")?.Value?.ToLower())
            .Where(n => !string.IsNullOrEmpty(n))
            .ToHashSet() as HashSet<string> ?? [];

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
        if (inputs.Any() || outputs.Any())
        {
            foreach (var input in inputs)
            {
                var inputName = input.Attribute("name")?.Value ?? "";
                var inputNarrative = input.Attribute("narrative")?.Value;
                if (!string.IsNullOrEmpty(inputNarrative))
                {
                    sb.AppendLine($"    ''' <param name=\"{NameSanitizer.SanitizeVariableName(inputName)}\">{inputNarrative}</param>");
                }
            }
            foreach (var output in outputs)
            {
                var outputName = output.Attribute("name")?.Value ?? "";
                var outputNarrative = output.Attribute("narrative")?.Value;
                if (!string.IsNullOrEmpty(outputNarrative))
                {
                    sb.AppendLine($"    ''' <param name=\"{NameSanitizer.SanitizeVariableName(outputName)}\">{outputNarrative}</param>");
                }
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

            if (outputParamNames.Contains(inputName)) continue;
            paramList.Add($"Optional ByVal {sanitizedName} As {vbType} = {defaultValue}");
        }

        foreach (var output in outputs)
        {
            var outputName = output.Attribute("name")?.Value ?? "";
            var outputType = output.Attribute("type")?.Value ?? "text";
            var vbType = TypeMapper.MapDataType(outputType);
            var sanitizedName = NameSanitizer.SanitizeVariableName(outputName);
            var defaultValue = TypeMapper.GetOptionalDefaultValue(outputType);
            paramList.Add($"Optional ByRef {sanitizedName} As {vbType} = {defaultValue}");
        }

        sb.AppendLine(string.Join(", ", paramList) + ")");
        sb.AppendLine();

        // Local variables and initialization
        DataItemGenerator.GenerateLocalDataItems(sb, stages, paramNames);
        DataItemGenerator.GenerateInitialValueInitialization(sb, stages);

        // Generate method body
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
            sb.AppendLine($"        {labelName}: ' {stageName}");

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

        sb.AppendLine("    End Sub");
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

                if (!string.IsNullOrEmpty(inputName) && !string.IsNullOrEmpty(stageName) && stageName != inputName)
                {
                    if (!hasInputAssignments)
                    {
                        sb.AppendLine("        ' Initialize local variables with input values");
                        hasInputAssignments = true;
                    }
                    sb.AppendLine($"        {NameSanitizer.SanitizeVariableName(stageName)} = {NameSanitizer.SanitizeVariableName(inputName)}");
                }
            }
            if (hasInputAssignments) sb.AppendLine();
        }

        // Generate GoTo
        var targetStageId = stage.Element("onsuccess")?.Value;
        if (!string.IsNullOrEmpty(targetStageId) && stage.Document != null)
        {
            var targetStageLabel = StageNavigator.ResolveStageLabel(targetStageId, stage.Document);
            sb.AppendLine($"        GoTo {targetStageLabel}");
        }
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
