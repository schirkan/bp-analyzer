using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

namespace BPAnalyzer;

/// <summary>
/// Generates VB.Net code from BluePrism XML files.
/// Uses Goto statements to recreate the process flow.
/// Supports static variables for BluePrism data items.
/// </summary>
public class BluePrismCodeGen
{
  private readonly string _outputDirectory;

  public BluePrismCodeGen(string outputDirectory)
  {
    _outputDirectory = outputDirectory;
  }

  /// <summary>
  /// Generates VB.Net code from a BluePrism XML file.
  /// </summary>
  public string GenerateCode(string xmlFilePath)
  {
    var doc = XDocument.Load(xmlFilePath);
    var process = doc.Root;

    if (process?.Name.LocalName != "process")
    {
      throw new ArgumentException("Invalid BluePrism XML file - root element must be 'process'");
    }

    var processName = process.Attribute("name")?.Value ?? "UnnamedProcess";
    var version = process.Attribute("version")?.Value ?? "1.0";

    var code = GenerateClass(processName, version, process);
    return code;
  }

  /// <summary>
  /// Generates VB.Net code for all XML files in the xml directory.
  /// </summary>
  public void GenerateAll(string xmlDirectory)
  {
    if (!Directory.Exists(_outputDirectory))
    {
      Directory.CreateDirectory(_outputDirectory);
    }

    var xmlFiles = Directory.GetFiles(xmlDirectory, "*.xml");

    foreach (var xmlFile in xmlFiles)
    {
      try
      {
        var code = GenerateCode(xmlFile);
        var fileName = Path.GetFileNameWithoutExtension(xmlFile) + ".vb";
        var outputPath = Path.Combine(_outputDirectory, fileName);

        File.WriteAllText(outputPath, code);
        Console.WriteLine($"Generated: {outputPath}");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error processing {xmlFile}: {ex.Message}");
      }
    }
  }

  private string GenerateClass(string processName, string version, XElement process)
  {
    var sb = new System.Text.StringBuilder();

    // Check if this is an object or process
    var processType = process.Attribute("type")?.Value;
    var isObject = processType == "object";

    // Get ProcessInfo for class documentation
    var processInfo = process.Descendants()
        .FirstOrDefault(e => e.Attribute("type")?.Value == "ProcessInfo");

    // Class header with ProcessInfo as class comments
    sb.AppendLine($"' Generated from BluePrism {(isObject ? "object" : "process")}: {processName}");
    sb.AppendLine($"' Version: {version}");
    sb.AppendLine($"' Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

    if (processInfo != null)
    {
      var narrative = process.Attribute("narrative")?.Value;
      if (!string.IsNullOrEmpty(narrative))
      {
        sb.AppendLine($"' ");
        sb.AppendLine($"' {narrative}");
      }

      var references = processInfo.Element("references")?.Elements("reference");
      if (references != null && references.Any())
      {
        sb.AppendLine($"' ");
        sb.AppendLine($"' References:");
        foreach (var reference in references)
        {
          sb.AppendLine($"'   - {reference.Value}");
        }
      }

      var imports = processInfo.Element("imports")?.Elements("import");
      if (imports != null && imports.Any())
      {
        sb.AppendLine($"' ");
        sb.AppendLine($"' Imports:");
        foreach (var import in imports)
        {
          sb.AppendLine($"'   - {import.Value}");
        }
      }
    }

    sb.AppendLine();
    sb.AppendLine("Imports System");
    sb.AppendLine("Imports System.Collections.Generic");
    sb.AppendLine("Imports System.Linq");
    sb.AppendLine("Imports System.Text");
    sb.AppendLine("Imports System.Data");
    sb.AppendLine();
    sb.AppendLine($"''' <summary>");
    sb.AppendLine($"''' BluePrism {(isObject ? "object" : "process")}: {processName}");
    sb.AppendLine($"''' </summary>");
    sb.AppendLine($"Public Class {SanitizeClassName(processName)}");
    sb.AppendLine();

    // Generate data items - separate global (class level) and local (method level)
    sb.AppendLine("    #Region \"Global Data Items\"");
    sb.AppendLine();
    GenerateGlobalDataItems(process, sb);
    sb.AppendLine();
    sb.AppendLine("    #End Region");
    sb.AppendLine();

    // Generate subsheets as functions/methods
    sb.AppendLine("    #Region \"Methods\"");
    sb.AppendLine();
    GenerateSubsheetsAsMethods(process, sb, isObject);
    sb.AppendLine();
    sb.AppendLine("    #End Region");
    sb.AppendLine();

    // Class footer
    sb.AppendLine("End Class");

    return sb.ToString();
  }

  private void GenerateGlobalDataItems(XElement process, System.Text.StringBuilder sb)
  {
    var allStages = process.Descendants()
        .Where(e => e.Name.LocalName == "stage")
        .ToList();

    // Global data items: Data stages with initialvalue or marked as global (not in a subsheet)
    var globalDataStages = allStages
      .Where(e =>
      {
        var stageType = e.Attribute("type")?.Value;
        if (stageType != "Data") return false;

        // Has no subsheetid AND has initialvalue means global
        var hasNoSubsheet = string.IsNullOrEmpty(e.Element("subsheetid")?.Value);
        var hasInitialValue = !string.IsNullOrEmpty(e.Element("initialvalue")?.Value);

        return hasNoSubsheet && hasInitialValue;
      })
      .ToList();

    if (globalDataStages.Count == 0)
    {
      sb.AppendLine("    ' No global data items");
      return;
    }

    foreach (var stage in globalDataStages)
    {
      var name = stage.Attribute("name")?.Value;
      var datatype = stage.Element("datatype")?.Value ?? "text";
      var initialValue = stage.Element("initialvalue")?.Value ?? "";
      var isPrivate = stage.Element("private") != null;

      if (string.IsNullOrEmpty(name)) continue;

      var vbType = MapDataType(datatype, stage);
      var visibility = isPrivate ? "Private" : "Public";

      sb.AppendLine($"    ''' <summary>");
      sb.AppendLine($"    ''' Global data item: {name} ({datatype})");
      sb.AppendLine($"    ''' </summary>");
      sb.AppendLine($"    {visibility} {SanitizeVariableName(name)} As {vbType}");
    }
  }

  private void GenerateSubsheetsAsMethods(XElement process, System.Text.StringBuilder sb, bool isObject = false)
  {
    // Get all subsheets
    var subsheets = process.Descendants("subsheet").ToList();

    // Get all stages
    var allStages = process.Descendants()
        .Where(e => e.Name.LocalName == "stage")
        .ToList();

    // If no subsheets, generate the main process flow as a default method
    if (subsheets.Count == 0)
    {
      GenerateMainMethod(process, sb);
      return;
    }

    // For objects: generate constructor for stages without subsheetid (initialization)
    if (isObject)
    {
      var globalStages = allStages
        .Where(e => string.IsNullOrEmpty(e.Element("subsheetid")?.Value))
        .ToList();

      if (globalStages.Any())
      {
        GenerateConstructor(process, sb, globalStages);
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
        GenerateMainMethod(process, sb, mainStages);
        sb.AppendLine();
      }
    }

    // Generate each subsheet as a method
    foreach (var subsheet in subsheets)
    {
      var subsheetId = subsheet.Attribute("subsheetid")?.Value;
      // Name is a child element, not an attribute
      var subsheetName = subsheet.Element("name")?.Value ?? "Unnamed";
      var subsheetType = subsheet.Attribute("type")?.Value ?? "Normal";

      // Get all stages for this subsheet - subsheetid is a child element, not an attribute
      var subsheetStages = allStages
        .Where(e =>
        {
          var stageSubsheetId = e.Element("subsheetid")?.Value;
          return stageSubsheetId == subsheetId;
        })
        .ToList();

      if (subsheetStages.Count == 0) continue;

      // Find Start and End stages for this subsheet
      var startStage = subsheetStages.FirstOrDefault(s => s.Attribute("type")?.Value == "Start");
      var endStage = subsheetStages.FirstOrDefault(s => s.Attribute("type")?.Value == "End");

      // Determine method type (Sub or Function) and handle CleanUp as destructor
      var isDestructor = subsheetType == "CleanUp";
      var methodType = isDestructor ? "Protected Overrides Sub" : "Public Sub";
      var methodName = isDestructor ? "Finalize" : SanitizeMethodName(subsheetName);

      // Collect input parameters from Start stage
      var inputs = startStage?.Element("inputs")?.Elements("input").ToList() ?? new List<XElement>();

      // Collect output parameters from End stage (linked by subsheetid)
      var outputs = endStage?.Element("outputs")?.Elements("output").ToList() ?? new List<XElement>();

      // Method documentation
      sb.AppendLine($"    ''' <summary>");
      if (isDestructor)
      {
        sb.AppendLine($"    ''' Destructor (CleanUp) - called when object is disposed");
      }
      else
      {
        sb.AppendLine($"    ''' BluePrism subsheet: {subsheetName} (Type: {subsheetType})");
      }
      sb.AppendLine($"    ''' </summary>");

      // Method signature
      sb.Append($"    {methodType} {methodName}(");
      var paramList = new List<string>();

      // Add input parameters
      foreach (var input in inputs)
      {
        var inputName = input.Attribute("name")?.Value ?? "";
        var inputType = input.Attribute("type")?.Value ?? "text";
        var vbType = MapDataType(inputType);
        paramList.Add($"ByVal {SanitizeVariableName(inputName)} As {vbType}");
      }

      // Add output parameters as Out (for End stage)
      foreach (var output in outputs)
      {
        var outputName = output.Attribute("name")?.Value ?? "";
        var outputType = output.Attribute("type")?.Value ?? "text";
        var vbType = MapDataType(outputType);
        paramList.Add($"<Out> ByRef {SanitizeVariableName(outputName)} As {vbType}");
      }

      sb.AppendLine(string.Join(", ", paramList) + ")");

      // Generate local data items for this subsheet (Data and Collection stages)
      var localDataStages = subsheetStages
        .Where(e =>
        {
          var stageType = e.Attribute("type")?.Value;
          return stageType == "Data" || stageType == "Collection";
        })
        .ToList();

      if (localDataStages.Any())
      {
        sb.AppendLine("        ' Local variables");
        foreach (var dataStage in localDataStages)
        {
          var dataName = dataStage.Attribute("name")?.Value!;
          var dataType = dataStage.Element("datatype")?.Value ?? "text";
          var initialValue = dataStage.Element("initialvalue")?.Value;
          var vbType = MapDataType(dataType, dataStage);

          if (!string.IsNullOrEmpty(initialValue))
          {
            var formattedValue = FormatInitialValue(dataType, initialValue, dataStage);
            sb.AppendLine($"        Dim {SanitizeVariableName(dataName)} As {vbType} = {formattedValue}");
          }
          else
          {
            sb.AppendLine($"        Dim {SanitizeVariableName(dataName)} As {vbType}");
          }
        }
        sb.AppendLine();
      }

      // Generate method body with sorted stages
      sb.AppendLine();

      // Sort stages by execution order
      var sortedStages = SortStagesByExecutionOrder(subsheetStages);

      // Generate code for each stage (skip Data, WaitEnd, and End stages - End is handled separately)
      foreach (var stage in sortedStages)
      {
        var stageType = stage.Attribute("type")?.Value!;
        var stageName = stage.Attribute("name")?.Value!;
        var stageId = stage.Attribute("stageid")?.Value!;

        if (string.IsNullOrEmpty(stageName)) continue;

        // Stages that are handled elsewhere or don't need a label
        var skipStages = new HashSet<string> { "Data", "Collection", "WaitEnd", "End", "Anchor", "ProcessInfo" };
        if (skipStages.Contains(stageType))
        {
          continue;
        }

        // SubSheetInfo: only generates a comment, no separate label
        if (stageType == "SubSheetInfo")
        {
          sb.AppendLine($"        ' SubSheet: {stage.Element("name")?.Value}");
          continue;
        }

        // Start stage: generates only input param comments, no separate label
        if (stageType == "Start")
        {
          sb.AppendLine($"        ' Stage: {stageName} ({stageType})");
          GenerateStartStageForMethod(stage, sb);
          continue;
        }

        // Generate label for this stage
        var labelName = GetUniqueLabelName(stageName, stageId);
        sb.AppendLine($"        ' Stage: {stageName} ({stageType})");
        sb.AppendLine($"{labelName}:");

        switch (stageType)
        {
          case "Action":
            GenerateActionStage(stage, sb);
            break;
          case "Process":
            GenerateProcessStage(stage, sb);
            break;
          case "Code":
            GenerateCodeStage(stage, sb);
            break;
          case "Decision":
            GenerateDecisionStage(stage, sb);
            break;
          case "Calculation":
            GenerateCalculationStage(stage, sb);
            break;
          case "MultipleCalculation":
            GenerateMultipleCalculationStage(stage, sb);
            break;
          case "Navigate":
            GenerateNavigateStage(stage, sb);
            break;
          case "Note":
            GenerateNoteStage(stage, sb);
            break;
          case "Exception":
            GenerateExceptionStage(stage, sb);
            break;
          case "Recover":
            GenerateRecoverStage(stage, sb);
            break;
          case "Resume":
            GenerateResumeStage(stage, sb);
            break;
          case "Anchor":
            GenerateAnchorStage(stage, sb);
            break;
          case "WaitStart":
            GenerateWaitStage(stage, sb);
            break;
          case "Block":
            GenerateBlockStage(stage, sb);
            break;
          case "ProcessInfo":
            // Skip - already handled as class documentation
            break;
          default:
            sb.AppendLine($"            ' TODO: Implement stage type '{stageType}'");
            break;
        }

        sb.AppendLine();
      }

      // Generate End label at the end of the method (single End stage per method)
      if (endStage != null)
      {
        var endStageName = endStage.Attribute("name")?.Value ?? "End";
        var endStageId = endStage.Attribute("stageid")?.Value ?? "";
        var endLabelName = GetUniqueLabelName(endStageName, endStageId);

        sb.AppendLine($"        ' Stage: {endStageName} (End)");
        sb.AppendLine($"{endLabelName}:");

        // Generate output parameters if any
        var endOutputs = endStage.Element("outputs")?.Elements("output");
        if (endOutputs != null && endOutputs.Any())
        {
          sb.AppendLine($"            ' Set output parameters");
          foreach (var output in endOutputs)
          {
            var outputName = output.Attribute("name")?.Value!;
            var stageAttr = output.Attribute("stage")?.Value;
            if (!string.IsNullOrEmpty(stageAttr))
            {
              sb.AppendLine($"            {SanitizeVariableName(outputName)} = {SanitizeVariableName(stageAttr)}");
            }
          }
        }
        sb.AppendLine();
      }

      sb.AppendLine("    End Sub");
      sb.AppendLine();
    }
  }

  private void GenerateConstructor(XElement process, System.Text.StringBuilder sb, List<XElement> stages)
  {
    var processName = process.Attribute("name")?.Value ?? "Object";

    // Find End stage
    var endStage = stages.FirstOrDefault(s => s.Attribute("type")?.Value == "End");

    sb.AppendLine($"    ''' <summary>");
    sb.AppendLine($"    ''' Constructor - initialization code from stages without subsheet");
    sb.AppendLine($"    ''' </summary>");
    sb.AppendLine($"    Public Sub New()");
    sb.AppendLine("        ' Constructor body generated from BluePrism global stages");
    sb.AppendLine();

    // Sort and generate stages
    var sortedStages = SortStagesByExecutionOrder(stages);

    // Stages to skip
    var skipStages = new HashSet<string> { "Data", "WaitEnd", "End", "Anchor", "ProcessInfo" };

    foreach (var stage in sortedStages)
    {
      var stageType = stage.Attribute("type")?.Value!;
      var stageName = stage.Attribute("name")?.Value!;
      var stageId = stage.Attribute("stageid")?.Value!;

      if (string.IsNullOrEmpty(stageName)) continue;

      // Skip stages handled elsewhere
      if (skipStages.Contains(stageType))
      {
        continue;
      }

      // SubSheetInfo: only generates a comment
      if (stageType == "SubSheetInfo")
      {
        sb.AppendLine($"        ' SubSheet: {stageName}");
        continue;
      }

      // Start stage: generates only input param comments and goto, no separate label
      if (stageType == "Start")
      {
        sb.AppendLine($"        ' Stage: {stageName} ({stageType})");
        GenerateStartStageForMethod(stage, sb);
        continue;
      }

      // Generate label for this stage
      var labelName = GetUniqueLabelName(stageName, stageId);
      sb.AppendLine($"        ' Stage: {stageName} ({stageType})");
      sb.AppendLine($"{labelName}:");

      switch (stageType)
      {
        case "Action":
          GenerateActionStage(stage, sb);
          break;
        case "Code":
          GenerateCodeStage(stage, sb);
          break;
        case "Calculation":
          GenerateCalculationStage(stage, sb);
          break;
        case "Note":
          GenerateNoteStage(stage, sb);
          break;
        case "Block":
          GenerateBlockStage(stage, sb);
          break;
        default:
          sb.AppendLine($"            ' TODO: Implement stage type '{stageType}'");
          break;
      }

      sb.AppendLine();
    }

    // Generate End label at the end of constructor
    if (endStage != null)
    {
      var endStageName = endStage.Attribute("name")?.Value ?? "End";
      var endStageId = endStage.Attribute("stageid")?.Value ?? "";
      var endLabelName = GetUniqueLabelName(endStageName, endStageId);

      sb.AppendLine($"        ' Stage: {endStageName} (End)");
      sb.AppendLine($"{endLabelName}:");

      // Generate output parameters if any
      var endOutputs = endStage.Element("outputs")?.Elements("output");
      if (endOutputs != null && endOutputs.Any())
      {
        sb.AppendLine($"            ' Set output parameters");
        foreach (var output in endOutputs)
        {
          var outputName = output.Attribute("name")?.Value!;
          var stageAttr = output.Attribute("stage")?.Value;
          if (!string.IsNullOrEmpty(stageAttr))
          {
            sb.AppendLine($"            {SanitizeVariableName(outputName)} = {SanitizeVariableName(stageAttr)}");
          }
        }
      }
      sb.AppendLine();
    }

    sb.AppendLine("    End Sub");
  }

  private void GenerateMainMethod(XElement process, System.Text.StringBuilder sb)
  {
    // Get stages without subsheetid
    var stages = process.Descendants()
        .Where(e => e.Name.LocalName == "stage" && string.IsNullOrEmpty(e.Element("subsheetid")?.Value))
        .ToList();

    GenerateMainMethod(process, sb, stages);
  }

  private void GenerateMainMethod(XElement process, System.Text.StringBuilder sb, List<XElement> stages)
  {
    var processName = process.Attribute("name")?.Value ?? "Run";
    var methodName = processName == "Run" ? "Run" : "Execute";

    // Find End stage for main method
    var endStage = stages.FirstOrDefault(s => s.Attribute("type")?.Value == "End");

    sb.AppendLine($"    ''' <summary>");
    sb.AppendLine($"    ''' Main process method (stages without subsheet)");
    sb.AppendLine($"    ''' </summary>");
    sb.AppendLine($"    Public Sub {methodName}()");
    sb.AppendLine();

    if (stages.Count == 0)
    {
      sb.AppendLine("        ' No stages found");
      sb.AppendLine("    End Sub");
      return;
    }

    // Generate local data items (Data stages without subsheetid and without initialvalue)
    var localDataStages = stages
      .Where(e => e.Attribute("type")?.Value == "Data")
      .ToList();

    if (localDataStages.Any())
    {
      sb.AppendLine("        ' Local variables");
      foreach (var dataStage in localDataStages)
      {
        var dataName = dataStage.Attribute("name")?.Value!;
        var dataType = dataStage.Element("datatype")?.Value ?? "text";
        var vbType = MapDataType(dataType, dataStage);
        sb.AppendLine($"        Dim {SanitizeVariableName(dataName)} As {vbType}");
      }
      sb.AppendLine();
    }

    // Sort and generate stages
    var sortedStages = SortStagesByExecutionOrder(stages);

    // Stages that are handled elsewhere or don't need a label
    var skipStages = new HashSet<string> { "Data", "WaitEnd", "End", "Anchor", "ProcessInfo" };

    foreach (var stage in sortedStages)
    {
      var stageType = stage.Attribute("type")?.Value!;
      var stageName = stage.Attribute("name")?.Value!;
      var stageId = stage.Attribute("stageid")?.Value!;

      if (string.IsNullOrEmpty(stageName)) continue;

      // Skip Data and End stages - End is handled at the end
      if (skipStages.Contains(stageType))
      {
        continue;
      }

      // Start stage: generates only input param comments and goto, no separate label
      if (stageType == "Start")
      {
        sb.AppendLine($"        ' Stage: {stageName} ({stageType})");
        GenerateStartStageForMethod(stage, sb);
        continue;
      }

      var labelName = GetUniqueLabelName(stageName, stageId);
      sb.AppendLine($"        ' Stage: {stageName} ({stageType})");
      sb.AppendLine($"{labelName}:");

      switch (stageType)
      {
        case "Start":
          GenerateStartStageForMethod(stage, sb);
          break;
        case "End":
          GenerateEndStageForMethod(stage, sb);
          break;
        case "Action":
          GenerateActionStage(stage, sb);
          break;
        case "Process":
          GenerateProcessStage(stage, sb);
          break;
        case "ProcessInfo":
          // Skip
          break;
        case "Code":
          GenerateCodeStage(stage, sb);
          break;
        case "Decision":
          GenerateDecisionStage(stage, sb);
          break;
        case "Calculation":
          GenerateCalculationStage(stage, sb);
          break;
        case "MultipleCalculation":
          GenerateMultipleCalculationStage(stage, sb);
          break;
        case "Navigate":
          GenerateNavigateStage(stage, sb);
          break;
        case "Note":
          GenerateNoteStage(stage, sb);
          break;
        case "Exception":
          GenerateExceptionStage(stage, sb);
          break;
        case "Block":
          GenerateBlockStage(stage, sb);
          break;
        default:
          sb.AppendLine($"            ' TODO: Implement stage type '{stageType}'");
          break;
      }

      sb.AppendLine();
    }

    // Generate End label at the end of the method
    if (endStage != null)
    {
      var endStageName = endStage.Attribute("name")?.Value ?? "End";
      var endStageId = endStage.Attribute("stageid")?.Value ?? "";
      var endLabelName = GetUniqueLabelName(endStageName, endStageId);

      sb.AppendLine($"        ' Stage: {endStageName} (End)");
      sb.AppendLine($"{endLabelName}:");

      // Generate output parameters if any
      var endOutputs = endStage.Element("outputs")?.Elements("output");
      if (endOutputs != null && endOutputs.Any())
      {
        sb.AppendLine($"            ' Set output parameters");
        foreach (var output in endOutputs)
        {
          var outputName = output.Attribute("name")?.Value!;
          var stageAttr = output.Attribute("stage")?.Value;
          if (!string.IsNullOrEmpty(stageAttr))
          {
            sb.AppendLine($"            {SanitizeVariableName(outputName)} = {SanitizeVariableName(stageAttr)}");
          }
        }
      }
      sb.AppendLine();
    }

    sb.AppendLine("    End Sub");
  }

  private List<XElement> SortStagesByExecutionOrder(List<XElement> stages)
  {
    // Build a dictionary for quick lookup
    var stageDict = stages.ToDictionary(s => s.Attribute("stageid")?.Value ?? "", s => s);

    // Find the Start stage
    var startStage = stages.FirstOrDefault(s => s.Attribute("type")?.Value == "Start");

    if (startStage == null)
    {
      // No start stage found, return original order
      return stages;
    }

    // Topological sort following onsuccess links
    var sorted = new List<XElement>();
    var visited = new HashSet<string>();

    void Visit(XElement stage)
    {
      if (stage == null) return;

      var stageId = stage.Attribute("stageid")?.Value;
      if (string.IsNullOrEmpty(stageId)) return;

      if (visited.Contains(stageId)) return;
      visited.Add(stageId);

      var stageType = stage.Attribute("type")?.Value;

      // Skip Anchor stages in output, but follow their onsuccess chain
      if (stageType == "Anchor")
      {
        var anchorOnsuccess = stage.Element("onsuccess")?.Value;
        if (!string.IsNullOrEmpty(anchorOnsuccess) && stageDict.TryGetValue(anchorOnsuccess, out var anchorNextStage))
        {
          Visit(anchorNextStage);
        }
        return;
      }

      // Skip ProcessInfo stages - already used as class documentation
      if (stageType == "ProcessInfo")
      {
        return;
      }

      sorted.Add(stage);

      // Follow onsuccess
      var onsuccess = stage.Element("onsuccess")?.Value;
      if (!string.IsNullOrEmpty(onsuccess) && stageDict.TryGetValue(onsuccess, out var nextStage))
      {
        Visit(nextStage);
      }

      // Also follow ontrue and onfalse for Decision stages
      var ontrue = stage.Element("ontrue")?.Value;
      var onfalse = stage.Element("onfalse")?.Value;

      if (!string.IsNullOrEmpty(ontrue) && stageDict.TryGetValue(ontrue, out var trueStage))
      {
        Visit(trueStage);
      }

      if (!string.IsNullOrEmpty(onfalse) && stageDict.TryGetValue(onfalse, out var falseStage))
      {
        Visit(falseStage);
      }
    }

    Visit(startStage);

    // Add any remaining stages that weren't connected
    foreach (var stage in stages)
    {
      var stageId = stage.Attribute("stageid")?.Value!;
      if (!visited.Contains(stageId))
      {
        sorted.Add(stage);
      }
    }

    return sorted;
  }

  private void GenerateStartStageForMethod(XElement stage, System.Text.StringBuilder sb)
  {
    var inputs = stage.Element("inputs")?.Elements("input");

    if (inputs != null && inputs.Any())
    {
      sb.AppendLine($"            ' Initialize input parameters");
      foreach (var input in inputs)
      {
        var inputName = input.Attribute("name")?.Value;
        var expr = input.Attribute("expr")?.Value;
        if (!string.IsNullOrEmpty(expr))
        {
          sb.AppendLine($"            ' {inputName} = {expr}");
        }
      }
    }

    var onsuccess = stage.Element("onsuccess")?.Value;
    if (!string.IsNullOrEmpty(onsuccess))
    {
      var targetStage = FindStageName(onsuccess, stage.Document!);
      sb.AppendLine($"            GoTo {targetStage}");
    }
    else
    {
      sb.AppendLine($"            ' No path defined");
    }
  }

  private void GenerateEndStageForMethod(XElement stage, System.Text.StringBuilder sb)
  {
    var outputs = stage.Element("outputs")?.Elements("output");

    if (outputs != null && outputs.Any())
    {
      sb.AppendLine($"            ' Set output parameters");
      foreach (var output in outputs)
      {
        var outputName = output.Attribute("name")?.Value!;
        var stageAttr = output.Attribute("stage")?.Value;
        if (!string.IsNullOrEmpty(stageAttr))
        {
          sb.AppendLine($"            {SanitizeVariableName(outputName)} = {SanitizeVariableName(stageAttr)}");
        }
      }
    }

    sb.AppendLine($"            Exit Sub");
  }

  private void GenerateActionStage(XElement stage, System.Text.StringBuilder sb)
  {
    var name = stage.Attribute("name")?.Value;
    var resource = stage.Element("resource");
    var objectName = resource?.Attribute("object")?.Value;
    var action = resource?.Attribute("action")?.Value;
    var onsuccess = stage.Element("onsuccess")?.Value;

    sb.AppendLine($"            ' Action: {name}");

    if (!string.IsNullOrEmpty(objectName) && !string.IsNullOrEmpty(action))
    {
      sb.AppendLine($"            ' Calling: {objectName}.{action}()");
      sb.AppendLine($"            ' TODO: Implement action call");
    }

    if (!string.IsNullOrEmpty(onsuccess))
    {
      var targetStage = FindStageName(onsuccess, stage.Document!);
      sb.AppendLine($"            GoTo {targetStage}");
    }
  }

  private void GenerateProcessStage(XElement stage, System.Text.StringBuilder sb)
  {
    var name = stage.Attribute("name")?.Value;
    var onsuccess = stage.Element("onsuccess")?.Value;

    sb.AppendLine($"            ' Call Process: {name}");

    var inputs = stage.Element("inputs")?.Elements("input");
    if (inputs != null)
    {
      foreach (var input in inputs)
      {
        var inputName = input.Attribute("name")?.Value;
        var expr = input.Attribute("expr")?.Value;
        sb.AppendLine($"            ' Input: {inputName} = {expr}");
      }
    }

    var outputs = stage.Element("outputs")?.Elements("output");
    if (outputs != null)
    {
      foreach (var output in outputs)
      {
        var outputName = output.Attribute("name")?.Value;
        sb.AppendLine($"            ' Output: {outputName}");
      }
    }

    sb.AppendLine($"            ' TODO: Implement process call");

    if (!string.IsNullOrEmpty(onsuccess))
    {
      var targetStage = FindStageName(onsuccess, stage.Document!);
      sb.AppendLine($"            GoTo {targetStage}");
    }
  }

  private void GenerateCodeStage(XElement stage, System.Text.StringBuilder sb)
  {
    var name = stage.Attribute("name")?.Value;
    var code = stage.Element("code")?.Value;
    var onsuccess = stage.Element("onsuccess")?.Value;

    sb.AppendLine($"            ' Code Stage: {name}");

    if (!string.IsNullOrWhiteSpace(code))
    {
      sb.AppendLine($"            ' Original code:");
      foreach (var line in code.Split('\n'))
      {
        var trimmed = line.Trim();
        if (!string.IsNullOrEmpty(trimmed))
        {
          sb.AppendLine($"            ' {trimmed}");
        }
      }
    }

    sb.AppendLine($"            ' TODO: Convert to VB.Net");

    if (!string.IsNullOrEmpty(onsuccess))
    {
      var targetStage = FindStageName(onsuccess, stage.Document!);
      sb.AppendLine($"            GoTo {targetStage}");
    }
  }

  private void GenerateDecisionStage(XElement stage, System.Text.StringBuilder sb)
  {
    var expression = stage.Element("decision")?.Attribute("expression")?.Value;
    var ontrue = stage.Element("ontrue")?.Value;
    var onfalse = stage.Element("onfalse")?.Value;

    sb.AppendLine($"            ' Decision: If {expression} Then");

    if (!string.IsNullOrEmpty(ontrue) && !string.IsNullOrEmpty(onfalse))
    {
      // Resolve anchor chain for both branches
      var trueTarget = ResolveAnchorChain(ontrue, stage.Document!);
      var falseTarget = ResolveAnchorChain(onfalse, stage.Document!);

      sb.AppendLine($"                GoTo {trueTarget}");
      sb.AppendLine($"            Else");
      sb.AppendLine($"                GoTo {falseTarget}");
      sb.AppendLine($"            End If");
    }
  }

  private void GenerateCalculationStage(XElement stage, System.Text.StringBuilder sb)
  {
    var calculation = stage.Element("calculation")?.Attribute("expression")?.Value;
    var stageName = stage.Element("calculation")?.Attribute("stage")?.Value;
    var onsuccess = stage.Element("onsuccess")?.Value;

    sb.AppendLine($"            ' Calculation: {stageName} = {calculation}");

    if (!string.IsNullOrEmpty(onsuccess))
    {
      var targetStage = FindStageName(onsuccess, stage.Document!);
      sb.AppendLine($"            GoTo {targetStage}");
    }
  }

  private void GenerateMultipleCalculationStage(XElement stage, System.Text.StringBuilder sb)
  {
    var calculations = stage.Element("steps")?.Elements("calculation");
    if (calculations != null)
    {
      foreach (var calc in calculations)
      {
        var expression = calc.Attribute("expression")?.Value;
        var targetStage = calc.Attribute("stage")?.Value;
        sb.AppendLine($"            ' {targetStage} = {expression}");
      }
    }

    var onsuccess = stage.Element("onsuccess")?.Value;
    if (!string.IsNullOrEmpty(onsuccess))
    {
      var targetStage = FindStageName(onsuccess, stage.Document!);
      sb.AppendLine($"            GoTo {targetStage}");
    }
  }

  private void GenerateNavigateStage(XElement stage, System.Text.StringBuilder sb)
  {
    var onsuccess = stage.Element("onsuccess")?.Value;

    sb.AppendLine($"            ' Navigate: UI automation");
    sb.AppendLine($"            ' TODO: Implement");

    if (!string.IsNullOrEmpty(onsuccess))
    {
      var targetStage = FindStageName(onsuccess, stage.Document!);
      sb.AppendLine($"            GoTo {targetStage}");
    }
  }

  private void GenerateNoteStage(XElement stage, System.Text.StringBuilder sb)
  {
    var narrative = stage.Element("narrative")?.Value;
    var onsuccess = stage.Element("onsuccess")?.Value;

    if (!string.IsNullOrEmpty(narrative))
    {
      sb.AppendLine($"            ' Note: {narrative}");
    }

    if (!string.IsNullOrEmpty(onsuccess))
    {
      var targetStage = FindStageName(onsuccess, stage.Document!);
      sb.AppendLine($"            GoTo {targetStage}");
    }
  }

  private void GenerateExceptionStage(XElement stage, System.Text.StringBuilder sb)
  {
    var detail = stage.Element("exception")?.Attribute("detail")?.Value;
    var exceptionType = stage.Element("exception")?.Attribute("type")?.Value;

    sb.AppendLine($"            ' Throw Exception: {exceptionType}");
    sb.AppendLine($"            ' Detail: {detail}");
    sb.AppendLine($"            Throw New Exception({detail})");
  }

  private void GenerateRecoverStage(XElement stage, System.Text.StringBuilder sb)
  {
    var onsuccess = stage.Element("onsuccess")?.Value;

    sb.AppendLine($"            ' Recover from error");

    if (!string.IsNullOrEmpty(onsuccess))
    {
      var targetStage = FindStageName(onsuccess, stage.Document!);
      sb.AppendLine($"            GoTo {targetStage}");
    }
  }

  private void GenerateResumeStage(XElement stage, System.Text.StringBuilder sb)
  {
    var onsuccess = stage.Element("onsuccess")?.Value;

    sb.AppendLine($"            ' Resume");

    if (!string.IsNullOrEmpty(onsuccess))
    {
      var targetStage = FindStageName(onsuccess, stage.Document!);
      sb.AppendLine($"            GoTo {targetStage}");
    }
  }

  private void GenerateAnchorStage(XElement stage, System.Text.StringBuilder sb)
  {
    var onsuccess = stage.Element("onsuccess")?.Value;

    sb.AppendLine($"            ' Anchor point");

    if (!string.IsNullOrEmpty(onsuccess))
    {
      var targetStage = FindStageName(onsuccess, stage.Document!);
      sb.AppendLine($"            GoTo {targetStage}");
    }
  }

  private void GenerateWaitStage(XElement stage, System.Text.StringBuilder sb)
  {
    var name = stage.Attribute("name")?.Value;
    var stageType = stage.Attribute("type")?.Value;
    var timeout = stage.Attribute("timeout")?.Value;
    var stageId = stage.Attribute("stageid")?.Value;
    var groupId = stage.Element("groupid")?.Value;

    sb.AppendLine($"            ' Wait: {name} (Type: {stageType})");

    // Find the corresponding WaitEnd for WaitStart (they share the same groupid)
    var doc = stage.Document;
    XElement? waitEnd = null;
    if (!string.IsNullOrEmpty(groupId) && stageType == "WaitStart")
    {
      waitEnd = doc?.Descendants()
        .FirstOrDefault(e => e.Attribute("type")?.Value == "WaitEnd" && e.Element("groupid")?.Value == groupId);
    }

    var choices = stage.Element("choices")?.Elements("choice").ToList();
    var hasTimeout = !string.IsNullOrEmpty(timeout);

    if (choices != null && choices.Any())
    {
      // Generate Select Case for choices
      sb.AppendLine($"            ' Wait for condition with {choices.Count} choice(s)");
      if (hasTimeout)
      {
        sb.AppendLine($"            Dim timeout_{stageId?.Substring(0, 8)} As Integer = {timeout}");
        sb.AppendLine($"            Dim elapsed_{stageId?.Substring(0, 8)} As Integer = 0");
      }

      sb.AppendLine($"            ' Select Case for wait conditions:");
      foreach (var choice in choices)
      {
        var choiceName = choice.Attribute("name")?.Value;
        var expression = choice.Attribute("expression")?.Value;
        var ontrue = choice.Element("ontrue")?.Value;

        sb.AppendLine($"            ' Case: {choiceName} = {expression}");
        if (!string.IsNullOrEmpty(ontrue))
        {
          var targetStage = ResolveAnchorChain(ontrue, stage.Document!);
          sb.AppendLine($"            '     GoTo {targetStage}");
        }
      }

      // Add WaitEnd as default case - point to WaitEnd's onsuccess
      if (waitEnd != null)
      {
        var waitEndName = waitEnd.Attribute("name")?.Value;
        var waitEndOnSuccess = waitEnd.Element("onsuccess")?.Value;

        if (!string.IsNullOrEmpty(waitEndOnSuccess))
        {
          var waitEndTarget = FindStageName(waitEndOnSuccess, stage.Document!);
          sb.AppendLine($"            ' Case Else (Default): WaitEnd [{waitEndName}], go to next stage");
          sb.AppendLine($"            '     GoTo {waitEndTarget}");
        }
        else
        {
          sb.AppendLine($"            ' Case Else (Default): WaitEnd [{waitEndName}] reached");
        }
      }
      else
      {
        sb.AppendLine($"            ' Case Else (Default): Continue waiting");
      }

      if (hasTimeout)
      {
        var onTimeout = stage.Attribute("type")?.Value == "WaitStart"
            ? "TimeoutException"
            : "Continue";
        sb.AppendLine($"            ' If timeout: Throw or continue");
      }
    }
    else if (hasTimeout)
    {
      sb.AppendLine($"            ' Wait with timeout: {timeout} seconds");

      // If there's a WaitEnd, add it as the default - point to WaitEnd's onsuccess
      if (waitEnd != null)
      {
        var waitEndOnSuccess = waitEnd.Element("onsuccess")?.Value;
        if (!string.IsNullOrEmpty(waitEndOnSuccess))
        {
          var waitEndTarget = FindStageName(waitEndOnSuccess, stage.Document!);
          sb.AppendLine($"            ' Case Else (Default): GoTo {waitEndTarget}");
        }
      }
    }
    else
    {
      sb.AppendLine($"            ' Wait indefinitely");

      // If there's a WaitEnd, add it as the default - point to WaitEnd's onsuccess
      if (waitEnd != null)
      {
        var waitEndOnSuccess = waitEnd.Element("onsuccess")?.Value;
        if (!string.IsNullOrEmpty(waitEndOnSuccess))
        {
          var waitEndTarget = FindStageName(waitEndOnSuccess, stage.Document!);
          sb.AppendLine($"            ' Case Else (Default): GoTo {waitEndTarget}");
        }
      }
    }
  }

  private void GenerateBlockStage(XElement stage, System.Text.StringBuilder sb)
  {
    var name = stage.Attribute("name")?.Value;

    sb.AppendLine($"            ' Block: {name}");

    var onsuccess = stage.Element("onsuccess")?.Value;
    if (!string.IsNullOrEmpty(onsuccess))
    {
      var targetStage = FindStageName(onsuccess, stage.Document!);
      sb.AppendLine($"            GoTo {targetStage}");
    }
  }

  private string FindStageName(string stageId, XDocument doc)
  {
    if (string.IsNullOrEmpty(stageId)) return "Unknown_Label";

    var stage = doc.Descendants()
        .FirstOrDefault(e => e.Name.LocalName == "stage" && e.Attribute("stageid")?.Value == stageId);

    var stageName = stage?.Attribute("name")?.Value ?? $"Stage_{stageId}";
    var stageIdAttr = stage?.Attribute("stageid")?.Value ?? stageId;

    return GetUniqueLabelName(stageName, stageIdAttr);
  }

  /// <summary>
  /// Resolves anchor chain: follows onsuccess through Anchor stages to find the final target
  /// </summary>
  private string ResolveAnchorChain(string stageId, XDocument doc)
  {
    var visited = new HashSet<string>();
    var currentId = stageId;

    while (!string.IsNullOrEmpty(currentId) && !visited.Contains(currentId))
    {
      visited.Add(currentId);

      var stage = doc.Descendants()
          .FirstOrDefault(e => e.Name.LocalName == "stage" && e.Attribute("stageid")?.Value == currentId);

      if (stage == null) break;

      var stageType = stage.Attribute("type")?.Value;

      // If not an Anchor, return this stage
      if (stageType != "Anchor")
      {
        return FindStageName(currentId, doc);
      }

      // If Anchor, follow onsuccess
      currentId = stage.Element("onsuccess")?.Value;
    }

    return FindStageName(stageId, doc);
  }

  private string GetUniqueLabelName(string stageName, string stageId)
  {
    var sanitizedName = System.Text.RegularExpressions.Regex.Replace(stageName ?? "", @"[^a-zA-Z0-9_]", "_");
    if (!char.IsLetter(sanitizedName[0]))
    {
      sanitizedName = "_" + sanitizedName;
    }
    var sanitizedId = System.Text.RegularExpressions.Regex.Replace(stageId ?? "", @"[^a-zA-Z0-9_]", "_");
    return $"{sanitizedName}_{sanitizedId}_Label";
  }

  private string SanitizeMethodName(string name)
  {
    var sanitized = System.Text.RegularExpressions.Regex.Replace(name ?? "", @"[^a-zA-Z0-9_]", "_");
    if (!char.IsLetter(sanitized[0]))
    {
      sanitized = "_" + sanitized;
    }
    return sanitized;
  }

  private string MapDataType(string bluePrismType, XElement? stage = null)
  {
    // Check for collection with typename
    if (bluePrismType?.ToLower() == "collection" && stage != null)
    {
      var collectionType = stage.Element("typename")?.Value;
      if (!string.IsNullOrEmpty(collectionType))
      {
        return $"DataTable(Of {collectionType})";
      }
      return "DataTable";
    }

    return bluePrismType?.ToLower() switch
    {
      "text" or "password" => "String",
      "number" => "Decimal",
      "flag" or "boolean" => "Boolean",
      "date" => "DateTime",
      "datetime" => "DateTime",
      "time" => "TimeSpan",
      "timespan" => "TimeSpan",
      "collection" => "DataTable",
      "binary" => "Byte()",
      "object" => "Object",
      _ => "String"
    };
  }

  private string FormatInitialValue(string dataType, string value, XElement? stage = null)
  {
    if (string.IsNullOrEmpty(value))
    {
      return dataType?.ToLower() switch
      {
        "text" or "password" => "\"\"",
        "number" => "0",
        "flag" or "boolean" => "False",
        "date" or "datetime" => "DateTime.MinValue",
        "timespan" or "time" => "TimeSpan.Zero",
        "collection" => "New DataTable()",
        _ => "Nothing"
      };
    }

    return dataType?.ToLower() switch
    {
      "text" or "password" => $"\"{value.Replace("\"", "\"\"")}\"",
      "number" => value,
      "flag" or "boolean" => value.ToLower() == "true" ? "True" : "False",
      "date" => $"DateTime.Parse(\"{value}\")",
      "datetime" => $"DateTime.Parse(\"{value}\")",
      "time" => $"TimeSpan.Parse(\"{value}\")",
      "timespan" => $"TimeSpan.Parse(\"{value}\")",
      "collection" => FormatCollectionInitialValue(value, stage),
      _ => $"\"{value.Replace("\"", "\"\"")}\""
    };
  }

  private string FormatCollectionInitialValue(string value, XElement? stage)
  {
    // Check for initialvalue with row data
    var initialValueElement = stage?.Element("initialvalue");
    if (initialValueElement == null)
    {
      return "New DataTable()";
    }

    // Get collection fields definition
    var collectionInfo = stage?.Element("collectioninfo");
    var fields = collectionInfo?.Elements("field").ToList();

    if (fields == null || !fields.Any())
    {
      return "New DataTable()";
    }

    var sb = new System.Text.StringBuilder();
    sb.AppendLine("New DataTable(");

    // Add columns
    foreach (var field in fields)
    {
      var fieldName = field.Attribute("name")?.Value;
      var fieldType = field.Attribute("type")?.Value ?? "text";
      var vbType = MapDataType(fieldType);
      sb.AppendLine($"    ' Column: {fieldName} ({vbType})");
    }

    // Check for row data
    var rows = initialValueElement.Elements("row").ToList();
    if (rows.Any())
    {
      sb.AppendLine("    ' Initial rows:");
      foreach (var row in rows)
      {
        var rowValues = new List<string>();
        foreach (var field in row.Elements("field"))
        {
          var fieldValue = field.Attribute("value")?.Value ?? "";
          var fieldType = field.Attribute("type")?.Value ?? "text";
          rowValues.Add(FormatInitialValue(fieldType, fieldValue));
        }
        sb.AppendLine($"    '   [{string.Join(", ", rowValues)}]");
      }
    }

    sb.Append(")");
    return sb.ToString();
  }

  private string SanitizeClassName(string name)
  {
    var sanitized = System.Text.RegularExpressions.Regex.Replace(name ?? "", @"[^a-zA-Z0-9_]", "_");
    if (!char.IsLetter(sanitized[0]))
    {
      sanitized = "_" + sanitized;
    }
    return sanitized;
  }

  private string SanitizeVariableName(string name)
  {
    var sanitized = System.Text.RegularExpressions.Regex.Replace(name ?? "", @"[^a-zA-Z0-9_]", "_");
    if (!char.IsLetter(sanitized[0]))
    {
      sanitized = "_" + sanitized;
    }
    return sanitized;
  }
}
