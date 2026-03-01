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
  private HashSet<string> _currentReferences = new HashSet<string>();

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

    // Copy template files first
    CopyTemplateFile();

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

    // Copy project file last (after all .vb files are generated)
    CopyProjectFile();
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

    // Collect references and imports from ProcessInfo
    var allReferences = new HashSet<string>();
    var allImports = new HashSet<string>();

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
          var refValue = reference.Value;
          sb.AppendLine($"'   - {refValue}");
          allReferences.Add(refValue);
        }
      }

      var imports = processInfo.Element("imports")?.Elements("import");
      if (imports != null && imports.Any())
      {
        sb.AppendLine($"' ");
        sb.AppendLine($"' Imports:");
        foreach (var import in imports)
        {
          var importValue = import.Value;
          sb.AppendLine($"'   - {importValue}");
          allImports.Add(importValue);
        }
      }
    }

    // Store references for project file
    if (allReferences.Any())
    {
      _currentReferences = allReferences;
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
        // Skip duplicates and already imported namespaces
        if (import != "System" && import != "System.Collections.Generic" &&
            import != "System.Linq" && import != "System.Text" && import != "System.Data")
        {
          sb.AppendLine($"Imports {import}");
        }
      }
    }
    sb.AppendLine();

    sb.AppendLine($"''' <summary>");
    sb.AppendLine($"''' BluePrism {(isObject ? "object" : "process")}: {processName}");
    sb.AppendLine($"''' </summary>");
    sb.AppendLine($"Public Class {SanitizeClassName(processName)}");
    sb.AppendLine($"    Inherits BP_Base");
    sb.AppendLine();

    // Generate Singleton Instance property with Lazy initialization
    var className = SanitizeClassName(processName);
    sb.AppendLine("    #Region \"Singleton Instance\"");
    sb.AppendLine();
    sb.AppendLine($"    ''' <summary>");
    sb.AppendLine($"    ''' Shared singleton instance");
    sb.AppendLine($"    ''' </summary>");
    sb.AppendLine($"    Private Shared ReadOnly _lazyInstance As New Lazy(Of {className})(Function() New {className}())");
    sb.AppendLine();
    sb.AppendLine($"    Public Shared ReadOnly Property Instance As {className}");
    sb.AppendLine($"        Get");
    sb.AppendLine($"            Return _lazyInstance.Value");
    sb.AppendLine($"        End Get");
    sb.AppendLine($"    End Property");
    sb.AppendLine();
    sb.AppendLine("    #End Region");
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

  private void GenerateObjectReferences(XElement process, System.Text.StringBuilder sb)
  {
    // Find all Action stages and collect unique object names
    var actionStages = process.Descendants()
        .Where(e => e.Name.LocalName == "stage" && e.Attribute("type")?.Value == "Action")
        .ToList();

    var objectNames = new HashSet<string>();
    foreach (var stage in actionStages)
    {
      var resource = stage.Element("resource");
      var objectName = resource?.Attribute("object")?.Value;
      if (!string.IsNullOrEmpty(objectName))
      {
        objectNames.Add(objectName);
      }
    }

    if (objectNames.Count == 0)
    {
      sb.AppendLine("    ' No object references");
      return;
    }

    foreach (var objectName in objectNames)
    {
      var sanitizedVarName = "_" + SanitizeVariableName(objectName);
      var className = SanitizeClassName(objectName);

      sb.AppendLine($"    ''' <summary>");
      sb.AppendLine($"    ''' Object reference: {objectName}");
      sb.AppendLine($"    ''' </summary>");
      sb.AppendLine($"    Private {sanitizedVarName} As {className} = New {className}()");
    }
  }

  private void GenerateGlobalDataItems(XElement process, System.Text.StringBuilder sb)
  {
    var allStages = process.Descendants()
        .Where(e => e.Name.LocalName == "stage")
        .ToList();

    // Global data items: Data and Collection stages that are not in a subsheet and not private
    var globalDataStages = allStages
      .Where(e =>
      {
        var stageType = e.Attribute("type")?.Value;
        if (stageType != "Data" && stageType != "Collection") return false;

        // Has no subsheetid means global
        var hasNoSubsheet = string.IsNullOrEmpty(e.Element("subsheetid")?.Value);

        // Is not private
        var isPrivate = e.Element("private") != null;

        return hasNoSubsheet && !isPrivate;
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

    // For objects: generate constructor for stages without subsheetid (initialization)
    if (isObject)
    {
      var globalStages = allStages
        .Where(e => string.IsNullOrEmpty(e.Element("subsheetid")?.Value))
        .ToList();

      if (globalStages.Any())
      {
        GenerateMethod(process, sb, "New", "Public", "Constructor - initialization code from stages without subsheet", globalStages, isObject, true);
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
        GenerateMethod(process, sb, "Main", "Public", "Main process method (stages without subsheet)", mainStages, isObject, false);
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

        // Get all stages for this subsheet
        var subsheetStages = allStages
          .Where(e =>
          {
            var stageSubsheetId = e.Element("subsheetid")?.Value;
            return stageSubsheetId == subsheetId;
          })
          .ToList();

        // Get published attribute - determines if method is public or private
        var published = subsheet.Attribute("published")?.Value?.ToLower() == "true";

        // Find SubSheetInfo stage for this subsheet to get narrative
        var subSheetInfoStage = subsheetStages?.FirstOrDefault(s => s.Attribute("type")?.Value == "SubSheetInfo");
        var methodNarrative = subSheetInfoStage?.Element("narrative")?.Value;

        // Determine visibility
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

        var methodName = isDestructor ? "Finalize" : SanitizeMethodName(subsheetName);

        GenerateMethod(process, sb, methodName, methodVisibility, methodNarrative, subsheetStages, isObject, isDestructor);
        sb.AppendLine();
      }
      catch (Exception ex)
      {
        sb.AppendLine($"    ' Error generating subsheet: {ex.Message}");
      }
    }
  }

  /// <summary>
  /// Generates a uniform method description based on method type
  /// </summary>
  private string GetMethodDescription(string methodName, string? methodNarrative)
  {
    // If there's a narrative from SubSheetInfo, use it
    if (!string.IsNullOrEmpty(methodNarrative) && methodNarrative.Trim().Length > 0)
    {
      return methodNarrative.Trim();
    }

    // Generate description based on method type
    return methodName.ToLower() switch
    {
      "main" => "Main process entry point",
      "new" => "Constructor (Initialize) - called when object is created",
      "finalize" => "Destructor (CleanUp) - called when object is disposed",
      _ => $"BluePrism method: {methodName}"
    };
  }

  /// <summary>
  /// Unified method generation - generates a method from stages
  /// </summary>
  private void GenerateMethod(XElement process, System.Text.StringBuilder sb, string methodName, string methodVisibility, string? methodNarrative, List<XElement> stages, bool isObject, bool isConstructor, string? processNarrative = null)
  {
    // Guard against null or empty stages
    if (stages == null || stages.Count == 0)
    {
      return;
    }

    // Find Start and End stages - with null checks
    var startStage = stages?.FirstOrDefault(s => s.Attribute("type")?.Value == "Start");
    var endStage = stages?.FirstOrDefault(s => s.Attribute("type")?.Value == "End");

    // Collect input parameter names from Start stage
    var inputParamNames = startStage?.Element("inputs")?.Elements("input")
        .Select(i => i.Attribute("name")?.Value?.ToLower())
        .Where(n => !string.IsNullOrEmpty(n))
        .ToHashSet();

    // Collect output parameter names from End stage
    var outputParamNames = endStage?.Element("outputs")?.Elements("output")
        .Select(o => o.Attribute("name")?.Value?.ToLower())
        .Where(n => !string.IsNullOrEmpty(n))
        .ToHashSet();

    // Combine input and output parameter names
    var paramNames = (inputParamNames ?? []).Union(outputParamNames ?? []).ToHashSet();

    // Collect input parameters from Start stage
    var inputs = startStage?.Element("inputs")?.Elements("input").ToList() ?? new List<XElement>();

    // Collect output parameters from End stage
    var outputs = endStage?.Element("outputs")?.Elements("output").ToList() ?? new List<XElement>();

    // Method documentation with uniform comments - handle multiline descriptions
    sb.AppendLine($"    ''' <summary>");
    var description = GetMethodDescription(methodName, methodNarrative);
    var descriptionLines = description.Split('\n');
    foreach (var line in descriptionLines)
    {
      sb.AppendLine($"    ''' {line}");
    }
    sb.AppendLine($"    ''' </summary>");

    // Add parameter descriptions if any
    if (inputs.Any() || outputs.Any())
    {
      foreach (var input in inputs)
      {
        var inputName = input.Attribute("name")?.Value ?? "";
        var inputNarrative = input.Attribute("narrative")?.Value;
        if (!string.IsNullOrEmpty(inputNarrative))
        {
          sb.AppendLine($"    ''' <param name=\"{SanitizeVariableName(inputName)}\">{inputNarrative}</param>");
        }
      }
      foreach (var output in outputs)
      {
        var outputName = output.Attribute("name")?.Value ?? "";
        var outputNarrative = output.Attribute("narrative")?.Value;
        if (!string.IsNullOrEmpty(outputNarrative))
        {
          sb.AppendLine($"    ''' <param name=\"{SanitizeVariableName(outputName)}\">{outputNarrative}</param>");
        }
      }
    }

    // Method signature
    var methodType = $"{methodVisibility} Sub";
    sb.Append($"    {methodType} {methodName}(");
    var paramList = new List<string>();

    // Add input parameters (only for non-constructor)
    if (!isConstructor)
    {
      foreach (var input in inputs)
      {
        var inputName = input.Attribute("name")?.Value ?? "";
        var inputType = input.Attribute("type")?.Value ?? "text";
        var vbType = MapDataType(inputType);
        var defaultValue = GetOptionalDefaultValue(inputType);
        paramList.Add($"Optional ByVal {SanitizeVariableName(inputName)} As {vbType} = {defaultValue}");
      }

      // Add output parameters (skip if already added as input - they become ByRef)
      foreach (var output in outputs)
      {
        var outputName = output.Attribute("name")?.Value ?? "";
        var outputType = output.Attribute("type")?.Value ?? "text";
        var vbType = MapDataType(outputType);
        var sanitizedName = SanitizeVariableName(outputName);
        var defaultValue = GetOptionalDefaultValue(outputType);

        // Skip if already added as input (they become ByRef, but we only need to declare once)
        if (inputParamNames.Contains(outputName?.ToLower()))
        {
          continue; // Already added as ByVal input, now acts as ByRef
        }

        paramList.Add($"Optional ByRef {sanitizedName} As {vbType} = {defaultValue}");
      }
    }

    sb.AppendLine(string.Join(", ", paramList) + ")");
    sb.AppendLine();

    // Generate local data items
    // For constructor: exclude private, initial value, and alwaysinit
    // For other methods: include Data/Collection stages that are NOT input/output parameters
    var localDataStages = (stages ?? new List<XElement>())
      .Where(e =>
      {
        var stageType = e.Attribute("type")?.Value;
        if (stageType != "Data" && stageType != "Collection") return false;

        if (isConstructor)
        {
          // Skip if private
          if (e.Element("private") != null) return false;
          // Skip if has initial value (these become global)
          if (!string.IsNullOrEmpty(e.Element("initialvalue")?.Value)) return false;
          // Skip Collection with alwaysinit (these become global)
          if (stageType == "Collection" && e.Element("alwaysinit") != null) return false;
          return true;
        }
        else
        {
          // For non-constructor methods, include data/collection stages
          // EXCEPT those that are input/output parameters
          var stageName = e.Attribute("name")?.Value?.ToLower();
          if (string.IsNullOrEmpty(stageName)) return false;

          // Skip if this is an input or output parameter
          if (paramNames.Contains(stageName)) return false;

          return true;
        }
      })
      .ToList();

    // Separate variable definitions from initializations
    // Variables with alwaysinit should be initialized conditionally
    if (localDataStages.Any())
    {
      sb.AppendLine("        ' Local variables");

      // First: define variables without initialization
      foreach (var dataStage in localDataStages)
      {
        var dataName = dataStage.Attribute("name")?.Value!;
        var dataType = dataStage.Element("datatype")?.Value ?? "text";
        var vbType = MapDataType(dataType, dataStage);

        sb.AppendLine($"        Dim {SanitizeVariableName(dataName)} As {vbType}");
      }
      sb.AppendLine();
    }

    // Generate initialization section for local variables with alwaysinit
    var alwaysInitLocalStages = localDataStages.Where(e => e.Element("alwaysinit") != null).ToList();
    if (alwaysInitLocalStages.Any())
    {
      sb.AppendLine("        ' Initialize local variables with alwaysinit");
      foreach (var dataStage in alwaysInitLocalStages)
      {
        var dataName = dataStage.Attribute("name")?.Value!;
        var dataType = dataStage.Element("datatype")?.Value ?? "text";
        var initialValue = dataStage.Element("initialvalue")?.Value;

        if (!string.IsNullOrEmpty(initialValue))
        {
          var formattedValue = FormatInitialValue(dataType, initialValue, dataStage);
          sb.AppendLine($"        If {SanitizeVariableName(dataName)} Is Nothing Then");
          sb.AppendLine($"            {SanitizeVariableName(dataName)} = {formattedValue}");
          sb.AppendLine($"        End If");
        }
      }
      sb.AppendLine();
    }

    // Generate method body

    // Sort stages by execution order
    var sortedStages = SortStagesByExecutionOrder(stages);

    // Stages to skip (Block stages are also skipped - they are handled by coordinates check)
    var skipStages = new HashSet<string> { "Block", "SubSheetInfo", "Data", "Collection", "WaitEnd", "End", "Anchor", "ProcessInfo" };

    foreach (var stage in sortedStages)
    {
      var stageType = stage.Attribute("type")?.Value!;
      var stageName = stage.Attribute("name")?.Value!;
      var stageId = stage.Attribute("stageid")?.Value!;

      if (string.IsNullOrEmpty(stageName)) continue;

      if (skipStages.Contains(stageType)) continue;

      // Start stage: generates only input param comments and goto
      if (stageType == "Start")
      {
        GenerateStartStageForMethod(stage, sb);
        continue;
      }

      // Generate label for this stage
      var labelName = GetStageLabel(stageType, stageId);
      sb.AppendLine($"        {labelName}: ' {stageName}");

      // Add On Error GoTo for exception handling if in a block or page with Recover stage
      // Skip for Recover, Resume, Note and Exception stages
      if (stageType != "Recover" && stageType != "Resume" && stageType != "Note" && HasRecoverStage(stages))
      {
        var recoverStage = stages.FirstOrDefault(s => s.Attribute("type")?.Value == "Recover");
        if (recoverStage != null)
        {
          var recoverStageId = recoverStage.Attribute("stageid")?.Value;
          if (!string.IsNullOrEmpty(recoverStageId))
          {
            var recoverLabel = GetStageLabel("Recover", recoverStageId);
            sb.AppendLine($"        On Error GoTo {recoverLabel}");
          }
        }
      }

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
        case "WaitStart":
          GenerateWaitStage(stage, sb);
          break;
        default:
          sb.AppendLine($"        ' TODO: Implement stage type '{stageType}'");
          break;
      }

      sb.AppendLine();
    }

    // Generate End label at the end
    if (endStage != null)
    {
      var endStageId = endStage.Attribute("stageid")?.Value ?? "";
      // Use central method for consistent label format
      var endLabelName = GetStageLabel("End", endStageId);
      sb.AppendLine($"        {endLabelName}:");

      // Generate output parameters if any (skip if outputName = stageAttr)
      var endOutputs = endStage.Element("outputs")?.Elements("output");
      if (endOutputs != null && endOutputs.Any())
      {
        foreach (var output in endOutputs)
        {
          var outputName = output.Attribute("name")?.Value!;
          var stageAttr = output.Attribute("stage")?.Value;
          if (!string.IsNullOrEmpty(stageAttr) && stageAttr != outputName)
          {
            sb.AppendLine($"        {SanitizeVariableName(outputName)} = {SanitizeVariableName(stageAttr)}");
          }
        }
      }
      sb.AppendLine();
    }

    sb.AppendLine("    End Sub");
  }

  private List<XElement> SortStagesByExecutionOrder(List<XElement> stages)
  {
    var stageDict = stages.ToDictionary(s => s.Attribute("stageid")?.Value ?? "", s => s);
    var startStage = stages.FirstOrDefault(s => s.Attribute("type")?.Value == "Start");

    if (startStage == null) return stages;

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

      if (stageType == "Anchor")
      {
        var anchorOnsuccess = stage.Element("onsuccess")?.Value;
        if (!string.IsNullOrEmpty(anchorOnsuccess) && stageDict.TryGetValue(anchorOnsuccess, out var anchorNextStage))
        {
          Visit(anchorNextStage);
        }
        return;
      }

      if (stageType == "ProcessInfo") return;

      sorted.Add(stage);

      var onsuccess = stage.Element("onsuccess")?.Value;
      if (!string.IsNullOrEmpty(onsuccess) && stageDict.TryGetValue(onsuccess, out var nextStage))
      {
        Visit(nextStage);
      }

      var ontrue = stage.Element("ontrue")?.Value;
      var onfalse = stage.Element("onfalse")?.Value;

      if (!string.IsNullOrEmpty(ontrue) && stageDict.TryGetValue(ontrue, out var trueStage)) Visit(trueStage);
      if (!string.IsNullOrEmpty(onfalse) && stageDict.TryGetValue(onfalse, out var falseStage)) Visit(falseStage);
    }

    Visit(startStage);

    foreach (var stage in stages)
    {
      var stageId = stage.Attribute("stageid")?.Value!;
      if (!visited.Contains(stageId)) sorted.Add(stage);
    }

    return sorted;
  }

  private void GenerateStartStageForMethod(XElement stage, System.Text.StringBuilder sb)
  {
    var inputs = stage.Element("inputs")?.Elements("input");
    var hasInputsWithAlwaysInit = false;

    // First pass: check if any input has alwaysinit
    if (inputs != null && inputs.Any())
    {
      foreach (var input in inputs)
      {
        // Check if the corresponding data stage has alwaysinit
        var inputName = input.Attribute("name")?.Value;
        if (!string.IsNullOrEmpty(inputName))
        {
          // Find the corresponding Data stage in the same subsheet
          var subsheetId = stage.Element("subsheetid")?.Value;
          var dataStage = stage.Document?.Descendants()
            .FirstOrDefault(e =>
              e.Attribute("type")?.Value == "Data" &&
              e.Attribute("name")?.Value == inputName &&
              e.Element("subsheetid")?.Value == subsheetId);

          if (dataStage?.Element("alwaysinit") != null)
          {
            hasInputsWithAlwaysInit = true;
            break;
          }
        }
      }
    }

    // Generate initialization for inputs with alwaysinit
    if (hasInputsWithAlwaysInit && inputs != null)
    {
      sb.AppendLine("        ' Initialize input parameters with alwaysinit");
      foreach (var input in inputs)
      {
        var inputName = input.Attribute("name")?.Value;
        var inputType = input.Attribute("type")?.Value ?? "text";

        if (!string.IsNullOrEmpty(inputName))
        {
          // Find the corresponding Data stage
          var subsheetId = stage.Element("subsheetid")?.Value;
          var dataStage = stage.Document?.Descendants()
            .FirstOrDefault(e =>
              e.Attribute("type")?.Value == "Data" &&
              e.Attribute("name")?.Value == inputName &&
              e.Element("subsheetid")?.Value == subsheetId);

          if (dataStage?.Element("alwaysinit") != null)
          {
            var initialValue = dataStage.Element("initialvalue")?.Value;
            if (!string.IsNullOrEmpty(initialValue))
            {
              var formattedValue = FormatInitialValue(inputType, initialValue, dataStage);
              sb.AppendLine($"        If {SanitizeVariableName(inputName)} Is Nothing Then");
              sb.AppendLine($"            {SanitizeVariableName(inputName)} = {formattedValue}");
              sb.AppendLine($"        End If");
            }
          }
        }
      }
      sb.AppendLine();
    }

    if (inputs != null && inputs.Any())
    {
      foreach (var input in inputs)
      {
        var inputName = input.Attribute("name")?.Value;
        var expr = input.Attribute("expr")?.Value;
        if (!string.IsNullOrEmpty(expr))
        {
          sb.AppendLine($"        ' {inputName} = {expr}");
        }
      }
    }

    var onsuccess = stage.Element("onsuccess")?.Value;
    if (!string.IsNullOrEmpty(onsuccess))
    {
      var targetStage = FindStageName(onsuccess, stage.Document!);
      sb.AppendLine($"        GoTo {targetStage}");
    }
    else
    {
      sb.AppendLine($"        ' No path defined");
    }
  }

  private void GenerateActionStage(XElement stage, System.Text.StringBuilder sb)
  {
    var name = stage.Attribute("name")?.Value;
    var resource = stage.Element("resource");
    var objectName = resource?.Attribute("object")?.Value;
    var action = resource?.Attribute("action")?.Value;
    var onsuccess = stage.Element("onsuccess")?.Value;

    if (!string.IsNullOrEmpty(objectName) && !string.IsNullOrEmpty(action))
    {
      var className = SanitizeClassName(objectName);
      var sanitizedActionName = SanitizeMethodName(action);

      var inputs = stage.Element("inputs")?.Elements("input").ToList();
      var inputParams = new List<string>();
      if (inputs != null)
      {
        foreach (var input in inputs)
        {
          var inputName = input.Attribute("name")?.Value;
          var inputExpr = input.Attribute("expr")?.Value;
          var sanitizedInputName = SanitizeVariableName(inputName ?? "");

          if (!string.IsNullOrEmpty(inputExpr))
          {
            var formattedExpr = FormatExpression(inputExpr);
            inputParams.Add($"{sanitizedInputName}:={formattedExpr}");
          }
          else if (!string.IsNullOrEmpty(inputName))
          {
            // Sanitize variable names in the value as well
            var sanitizedValue = SanitizeVariableName(inputName);
            inputParams.Add($"{sanitizedInputName}:={sanitizedValue}");
          }
        }
      }

      var outputs = stage.Element("outputs")?.Elements("output").ToList();
      var outputParams = new List<string>();
      if (outputs != null)
      {
        foreach (var output in outputs)
        {
          var outputName = output.Attribute("name")?.Value;
          var outputStage = output.Attribute("stage")?.Value;
          var sanitizedOutputName = SanitizeVariableName(outputName ?? "");

          if (!string.IsNullOrEmpty(outputStage))
          {
            // Sanitize variable names in the value as well
            var sanitizedStage = SanitizeVariableName(outputStage);
            outputParams.Add($"{sanitizedOutputName}:={sanitizedStage}");
          }
          else if (!string.IsNullOrEmpty(outputName))
          {
            // Sanitize variable names in the value as well
            var sanitizedValue = SanitizeVariableName(outputName);
            outputParams.Add($"{sanitizedOutputName}:={sanitizedValue}");
          }
        }
      }

      var allParams = inputParams.Concat(outputParams).ToList();
      var paramString = string.Join(", ", allParams);

      if (!string.IsNullOrEmpty(paramString))
        sb.AppendLine($"        {className}.Instance.{sanitizedActionName}({paramString})");
      else
        sb.AppendLine($"        {className}.Instance.{sanitizedActionName}()");
    }

    if (!string.IsNullOrEmpty(onsuccess))
    {
      var targetStage = FindStageName(onsuccess, stage.Document!);
      sb.AppendLine($"        GoTo {targetStage}");
    }
  }

  private void GenerateProcessStage(XElement stage, System.Text.StringBuilder sb)
  {
    var name = stage.Attribute("name")?.Value;
    var onsuccess = stage.Element("onsuccess")?.Value;
    var processId = stage.Element("processid")?.Value;

    // Find process name from processid by searching in all XML files
    var processName = FindProcessNameById(processId);

    if (!string.IsNullOrEmpty(processName))
    {
      var className = SanitizeClassName(processName);

      // Process inputs
      var inputs = stage.Element("inputs")?.Elements("input").ToList();
      var inputParams = new List<string>();
      if (inputs != null)
      {
        foreach (var input in inputs)
        {
          var inputName = input.Attribute("name")?.Value;
          var inputExpr = input.Attribute("expr")?.Value;
          var sanitizedInputName = SanitizeVariableName(inputName ?? "");

          if (!string.IsNullOrEmpty(inputExpr))
          {
            var formattedExpr = FormatExpression(inputExpr);
            inputParams.Add($"{sanitizedInputName}:={formattedExpr}");
          }
          else if (!string.IsNullOrEmpty(inputName))
          {
            var sanitizedValue = SanitizeVariableName(inputName);
            inputParams.Add($"{sanitizedInputName}:={sanitizedValue}");
          }
        }
      }

      // Process outputs
      var outputs = stage.Element("outputs")?.Elements("output").ToList();
      var outputParams = new List<string>();
      if (outputs != null)
      {
        foreach (var output in outputs)
        {
          var outputName = output.Attribute("name")?.Value;
          var outputStage = output.Attribute("stage")?.Value;
          var sanitizedOutputName = SanitizeVariableName(outputName ?? "");

          if (!string.IsNullOrEmpty(outputStage))
          {
            var sanitizedStage = SanitizeVariableName(outputStage);
            outputParams.Add($"{sanitizedOutputName}:={sanitizedStage}");
          }
          else if (!string.IsNullOrEmpty(outputName))
          {
            var sanitizedValue = SanitizeVariableName(outputName);
            outputParams.Add($"{sanitizedOutputName}:={sanitizedValue}");
          }
        }
      }

      var allParams = inputParams.Concat(outputParams).ToList();
      var paramString = string.Join(", ", allParams);

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

    if (!string.IsNullOrEmpty(onsuccess))
    {
      var targetStage = FindStageName(onsuccess, stage.Document!);
      sb.AppendLine($"        GoTo {targetStage}");
    }
  }

  /// <summary>
  /// Finds the process name by searching for the processid in all XML files
  /// </summary>
  private string? FindProcessNameById(string? processId)
  {
    if (string.IsNullOrEmpty(processId)) return null;

    var xmlDirectory = Path.Combine(Directory.GetCurrentDirectory(), "xml");
    if (!Directory.Exists(xmlDirectory)) return null;

    var xmlFiles = Directory.GetFiles(xmlDirectory, "*.xml");
    foreach (var xmlFile in xmlFiles)
    {
      try
      {
        var doc = XDocument.Load(xmlFile);
        var process = doc.Root;
        if (process?.Name.LocalName != "process") continue;

        var preferredId = process.Attribute("preferredid")?.Value;
        if (preferredId == processId)
        {
          return process.Attribute("name")?.Value;
        }
      }
      catch
      {
        // Skip invalid XML files
      }
    }

    return null;
  }

  private void GenerateCodeStage(XElement stage, System.Text.StringBuilder sb)
  {
    var name = stage.Attribute("name")?.Value;
    var code = stage.Element("code")?.Value;
    var onsuccess = stage.Element("onsuccess")?.Value;

    sb.AppendLine($"        ' Code Stage: {name}");

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

    if (!string.IsNullOrEmpty(onsuccess))
    {
      var targetStage = FindStageName(onsuccess, stage.Document!);
      sb.AppendLine($"        GoTo {targetStage}");
    }
  }

  private void GenerateDecisionStage(XElement stage, System.Text.StringBuilder sb)
  {
    var expression = stage.Element("decision")?.Attribute("expression")?.Value;
    var ontrue = stage.Element("ontrue")?.Value;
    var onfalse = stage.Element("onfalse")?.Value;

    var formattedExpression = FormatExpression(expression);
    sb.AppendLine($"        If {formattedExpression} Then");

    if (!string.IsNullOrEmpty(ontrue) && !string.IsNullOrEmpty(onfalse))
    {
      var trueTarget = ResolveAnchorChain(ontrue, stage.Document!);
      var falseTarget = ResolveAnchorChain(onfalse, stage.Document!);

      sb.AppendLine($"            GoTo {trueTarget}");
      sb.AppendLine($"        Else");
      sb.AppendLine($"            GoTo {falseTarget}");
      sb.AppendLine($"        End If");
    }
  }

  private void GenerateCalculationStage(XElement stage, System.Text.StringBuilder sb)
  {
    var calculation = stage.Element("calculation")?.Attribute("expression")?.Value;
    var stageName = stage.Element("calculation")?.Attribute("stage")?.Value;
    var onsuccess = stage.Element("onsuccess")?.Value;

    var formattedCalculation = FormatExpression(calculation);
    var formattedStageName = SanitizeVariableName(stageName ?? "");
    sb.AppendLine($"        {formattedStageName} = {formattedCalculation}");

    if (!string.IsNullOrEmpty(onsuccess))
    {
      var targetStage = FindStageName(onsuccess, stage.Document!);
      sb.AppendLine($"        GoTo {targetStage}");
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
        sb.AppendLine($"        ' {targetStage} = {expression}");
      }
    }

    var onsuccess = stage.Element("onsuccess")?.Value;
    if (!string.IsNullOrEmpty(onsuccess))
    {
      var targetStage = FindStageName(onsuccess, stage.Document!);
      sb.AppendLine($"        GoTo {targetStage}");
    }
  }

  private void GenerateNavigateStage(XElement stage, System.Text.StringBuilder sb)
  {
    var onsuccess = stage.Element("onsuccess")?.Value;
    sb.AppendLine($"        ' Navigate: UI automation");
    sb.AppendLine($"        ' TODO: Implement");

    if (!string.IsNullOrEmpty(onsuccess))
    {
      var targetStage = FindStageName(onsuccess, stage.Document!);
      sb.AppendLine($"        GoTo {targetStage}");
    }
  }

  private void GenerateNoteStage(XElement stage, System.Text.StringBuilder sb)
  {
    var narrative = stage.Element("narrative")?.Value;
    var onsuccess = stage.Element("onsuccess")?.Value;

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

    if (!string.IsNullOrEmpty(onsuccess))
    {
      var targetStage = FindStageName(onsuccess, stage.Document!);
      sb.AppendLine($"        GoTo {targetStage}");
    }
  }

  private void GenerateExceptionStage(XElement stage, System.Text.StringBuilder sb)
  {
    var exceptionElement = stage.Element("exception");
    var detail = exceptionElement?.Attribute("detail")?.Value;
    var exceptionType = exceptionElement?.Attribute("type")?.Value;
    var useCurrent = exceptionElement?.Attribute("usecurrent")?.Value?.ToLower() == "yes";

    // Check if usecurrent="yes" - then rethrow the stored exception
    if (useCurrent)
    {
      sb.AppendLine($"        RethrowException()");
    }
    else
    {
      // Raise the exception (StoreException will be called in Recover stage)
      sb.AppendLine($"        RaiseException(\"{exceptionType}\", {detail})");
    }
  }

  private void GenerateRecoverStage(XElement stage, System.Text.StringBuilder sb)
  {
    var onsuccess = stage.Element("onsuccess")?.Value;

    // Store the current exception using Err.GetException()
    sb.AppendLine($"        StoreException()");

    if (!string.IsNullOrEmpty(onsuccess))
    {
      var targetStage = FindStageName(onsuccess, stage.Document!);
      sb.AppendLine($"        GoTo {targetStage}");
    }
  }

  private void GenerateResumeStage(XElement stage, System.Text.StringBuilder sb)
  {
    var onsuccess = stage.Element("onsuccess")?.Value;

    // Clear the stored exception before resuming
    sb.AppendLine($"        ClearException()");

    if (!string.IsNullOrEmpty(onsuccess))
    {
      var targetStage = FindStageName(onsuccess, stage.Document!);
      sb.AppendLine($"        Resume {targetStage}");
    }
  }

  private void GenerateAnchorStage(XElement stage, System.Text.StringBuilder sb)
  {
    var onsuccess = stage.Element("onsuccess")?.Value;
    sb.AppendLine($"        ' Anchor point");

    if (!string.IsNullOrEmpty(onsuccess))
    {
      var targetStage = FindStageName(onsuccess, stage.Document!);
      sb.AppendLine($"        GoTo {targetStage}");
    }
  }

  private void GenerateWaitStage(XElement stage, System.Text.StringBuilder sb)
  {
    var name = stage.Attribute("name")?.Value;
    var stageType = stage.Attribute("type")?.Value;
    var timeout = stage.Element("timeout")?.Value ?? "0";
    var stageId = stage.Attribute("stageid")?.Value;
    var groupId = stage.Element("groupid")?.Value;

    sb.AppendLine($"        ' Wait: {name} (Type: {stageType})");

    var doc = stage.Document;
    XElement? waitEnd = null;
    if (!string.IsNullOrEmpty(groupId) && stageType == "WaitStart")
    {
      waitEnd = doc?.Descendants()
        .FirstOrDefault(e => e.Attribute("type")?.Value == "WaitEnd" && e.Element("groupid")?.Value == groupId);
    }

    var choices = stage.Element("choices")?.Elements("choice").ToList();
    if (choices != null && choices.Any())
    {
      sb.AppendLine($"        ' Wait {timeout} seconds for condition with {choices.Count} choice(s)");
      sb.AppendLine($"        Select Case True");
      foreach (var choice in choices)
      {
        var choiceName = choice.Element("name")?.Value;
        var ontrue = choice.Element("ontrue")?.Value;

        // Generate expression from choice element and condition
        var choiceExpression = GenerateWaitChoiceExpression(choice);

        sb.AppendLine($"            Case {choiceExpression} ' {choiceName}");
        if (!string.IsNullOrEmpty(ontrue))
        {
          var targetStage = ResolveAnchorChain(ontrue, stage.Document!);
          sb.AppendLine($"                GoTo {targetStage}");
        }
      }
      if (waitEnd != null)
      {
        var waitEndOnSuccess = waitEnd.Element("onsuccess")?.Value;
        if (!string.IsNullOrEmpty(waitEndOnSuccess))
        {
          var elseTarget = ResolveAnchorChain(waitEndOnSuccess, stage.Document!);
          sb.AppendLine($"            Case Else");
          sb.AppendLine($"                GoTo {elseTarget}");
        }
      }
      sb.AppendLine($"        End Select");
    }
  }

  /// <summary>
  /// Generates the expression for a WaitStart choice.
  /// Format: Application.Element("elementId").conditionId = True
  /// </summary>
  private string GenerateWaitChoiceExpression(XElement choice)
  {
    var element = choice.Element("element");
    var elementId = element?.Attribute("id")?.Value;
    var condition = choice.Element("condition");
    var conditionId = condition?.Element("id")?.Value;
    var compareType = choice.Attribute("comparetype")?.Value;
    var replyValue = choice.Attribute("reply")?.Value;

    // If we have element and condition, generate proper expression
    if (!string.IsNullOrEmpty(elementId) && !string.IsNullOrEmpty(conditionId))
    {
      var comparison = compareType?.ToLower() switch
      {
        "equal" => "=",
        "notequal" => "<>",
        "lessthan" => "<",
        "greaterthan" => ">",
        "lessthanorequal" => "<=",
        "greaterthanorequal" => ">=",
        _ => "="
      };

      // Determine the value to compare (reply attribute or default True)
      var compareValue = replyValue?.ToLower() == "true" ? "True" : "False";

      return $"Application.Element(\"{elementId}\").{conditionId} {comparison} {compareValue}";
    }

    // Fallback: return the expression attribute if present
    var expression = choice.Attribute("expression")?.Value;
    return FormatExpression(expression);
  }

  /// <summary>
  /// Checks if a stage is within a block by comparing coordinates
  /// Block: <display x="-30" y="-75" w="180" h="60" />
  /// Stage: <display x="105" y="-45" />
  /// Returns true if stage is inside the block's rectangle
  /// </summary>
  private bool IsStageInBlock(XElement stage, XElement block)
  {
    var stageDisplay = stage.Element("display");
    var blockDisplay = block.Element("display");

    if (stageDisplay == null || blockDisplay == null) return false;

    var stageX = ParseCoordinate(stageDisplay.Attribute("x")?.Value);
    var stageY = ParseCoordinate(stageDisplay.Attribute("y")?.Value);

    var blockX = ParseCoordinate(blockDisplay.Attribute("x")?.Value);
    var blockY = ParseCoordinate(blockDisplay.Attribute("y")?.Value);
    var blockW = ParseCoordinate(blockDisplay.Attribute("w")?.Value);
    var blockH = ParseCoordinate(blockDisplay.Attribute("h")?.Value);

    // Check if stage coordinates are within block rectangle
    return stageX >= blockX && stageX <= blockX + blockW &&
           stageY >= blockY && stageY <= blockY + blockH;
  }

  private double ParseCoordinate(string? value)
  {
    if (string.IsNullOrEmpty(value)) return 0;
    return double.TryParse(value, out var result) ? result : 0;
  }

  /// <summary>
  /// Finds all blocks that have a Recover stage within them for a given set of stages
  /// </summary>
  private Dictionary<string, XElement> FindBlocksWithRecover(List<XElement> stages)
  {
    var result = new Dictionary<string, XElement>();

    // Find all blocks
    var blocks = stages.Where(s => s.Attribute("type")?.Value == "Block").ToList();

    // Find all recover stages
    var recoverStages = stages.Where(s => s.Attribute("type")?.Value == "Recover").ToList();

    foreach (var block in blocks)
    {
      var blockId = block.Attribute("stageid")?.Value;
      if (string.IsNullOrEmpty(blockId)) continue;

      // Check if there's a recover stage within this block
      foreach (var recover in recoverStages)
      {
        if (IsStageInBlock(recover, block))
        {
          result[blockId] = block;
          break;
        }
      }
    }

    return result;
  }

  /// <summary>
  /// Checks if any stage in the method has a Recover stage (at page level)
  /// </summary>
  private bool HasRecoverStage(List<XElement> stages)
  {
    return stages.Any(s => s.Attribute("type")?.Value == "Recover");
  }

  private string FindStageName(string stageId, XDocument doc)
  {
    if (string.IsNullOrEmpty(stageId)) return "Unknown_Label";

    var stage = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "stage" && e.Attribute("stageid")?.Value == stageId);

    // If stage not found, return unknown label
    if (stage == null)
    {
      return "Unknown_Label";
    }

    var stageType = stage.Attribute("type")?.Value ?? "Unknown";

    // For End stages, use GetEndLabel
    if (stageType == "End")
    {
      var stageSubsheetId = stage.Element("subsheetid")?.Value;

      // Get the first End stage in the same subsheet
      var endStage = doc.Descendants()
        .FirstOrDefault(e => e.Name.LocalName == "stage"
          && e.Attribute("type")?.Value == "End"
          && e.Element("subsheetid")?.Value == stageSubsheetId);

      // If no matching end stage found, use the original stageId
      if (endStage == null)
      {
        return GetStageLabel("End", stageId);
      }

      return GetStageLabel("End", endStage.Attribute("stageid")?.Value ?? stageId);
    }

    // Use consistent label format: Type_StageId_Label
    return GetStageLabel(stageType, stageId);
  }

  /// <summary>
  /// Central method to generate a consistent label name from stage type and stage ID
  /// Format: Type_StageId_Label
  /// </summary>
  private string GetStageLabel(string stageType, string stageId)
  {
    var sanitizedId = SanitizeId(stageId);
    return $"{stageType}_{sanitizedId}_Label";
  }

  private string SanitizeId(string id)
  {
    return System.Text.RegularExpressions.Regex.Replace(id ?? "", @"[^a-zA-Z0-9_]", "_");
  }

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
      if (stageType != "Anchor")
      {
        return FindStageName(currentId, doc);
      }

      currentId = stage.Element("onsuccess")?.Value;
    }

    return FindStageName(stageId, doc);
  }

  private string SanitizeMethodName(string name)
  {
    var sanitized = System.Text.RegularExpressions.Regex.Replace(name ?? "", @"[^a-zA-Z0-9_]", "_");
    if (!char.IsLetter(sanitized[0])) sanitized = "_" + sanitized;
    return sanitized;
  }

  private string MapDataType(string bluePrismType, XElement? stage = null)
  {
    if (bluePrismType?.ToLower() == "collection" && stage != null)
    {
      var collectionType = stage.Element("typename")?.Value;
      if (!string.IsNullOrEmpty(collectionType))
        return $"DataTable(Of {collectionType})";
      return "DataTable";
    }

    return bluePrismType?.ToLower() switch
    {
      "text" or "password" => "String",
      "number" => "Decimal",
      "flag" or "boolean" => "Boolean",
      "date" or "datetime" => "DateTime",
      "time" or "timespan" => "TimeSpan",
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
      "date" or "datetime" => $"DateTime.Parse(\"{value}\")",
      "time" or "timespan" => $"TimeSpan.Parse(\"{value}\")",
      "collection" => "New DataTable()",
      _ => $"\"{value.Replace("\"", "\"\"")}\""
    };
  }

  /// <summary>
  /// Returns the default value for an optional parameter based on the BluePrism data type.
  /// Used to make all parameters optional with sensible defaults.
  /// </summary>
  private string GetOptionalDefaultValue(string bluePrismType)
  {
    return bluePrismType?.ToLower() switch
    {
      "text" or "password" => "Nothing",
      "number" => "Nothing",
      "flag" or "boolean" => "Nothing",
      "date" or "datetime" => "Nothing",
      "time" or "timespan" => "Nothing",
      "collection" => "Nothing",
      "binary" => "Nothing",
      "object" => "Nothing",
      _ => "Nothing"
    };
  }

  private string SanitizeClassName(string name)
  {
    var sanitized = System.Text.RegularExpressions.Regex.Replace(name ?? "", @"[^a-zA-Z0-9_]", "_");
    if (!char.IsLetter(sanitized[0])) sanitized = "_" + sanitized;
    return sanitized;
  }

  private string SanitizeVariableName(string name)
  {
    var sanitized = System.Text.RegularExpressions.Regex.Replace(name ?? "", @"[^a-zA-Z0-9_]", "_");
    if (!char.IsLetter(sanitized[0])) sanitized = "_" + sanitized;
    return sanitized;
  }

  /// <summary>
  /// Converts BluePrism expressions to valid VB.NET code
  /// - Removes square brackets [Variable] -> Variable
  /// - Handles Collection column access: Collection.Column -> Collection.CurrentRow("Column")
  /// - Supports nested collections: Collection.Column.SubColumn -> Collection.CurrentRow("Column").CurrentRow("SubColumn")
  /// - Supports spaces in column names: Collection.Column Name -> Collection.CurrentRow("Column Name")
  /// - Sanitizes variable names (replaces invalid chars with _)
  /// </summary>
  private string FormatExpression(string? expression)
  {
    if (string.IsNullOrWhiteSpace(expression)) return "";

    var result = expression;

    // Handle Collection column access with multiple levels: [Collection.Column1.Column2.Column3]
    // Becomes: Collection.CurrentRow("Column1").CurrentRow("Column2").CurrentRow("Column3")
    // Note: Column names can have spaces, collection names cannot
    result = System.Text.RegularExpressions.Regex.Replace(result ?? "", @"\[([a-zA-Z_][a-zA-Z0-9_]*)(\.[^\]]+)+\]", match =>
    {
      var fullMatch = match.Groups[0].Value;
      // Remove the outer brackets and split by dots
      var content = fullMatch.Trim('[', ']');
      var parts = content.Split('.');

      if (parts.Length == 0) return match.Value;

      var collectionName = SanitizeVariableName(parts[0]);
      var vbCode = collectionName;

      // Add CurrentRow for each subsequent part (column name)
      for (int i = 1; i < parts.Length; i++)
      {
        var columnName = parts[i].Trim();
        vbCode += $".CurrentRow(\"{columnName}\")";
      }

      return vbCode;
    });

    // Then, handle simple variables: [Variable] -> Variable
    result = System.Text.RegularExpressions.Regex.Replace(result ?? "", @"\[([^\]]+)\]", match =>
    {
      var varName = match.Groups[1].Value;
      // Check if it contains a dot (already handled above)
      if (varName.Contains('.')) return match.Value; // Keep original if not handled
      return SanitizeVariableName(varName);
    });

    // Replace BluePrism exception functions with our base class methods
    result = result.Replace("ExceptionDetail()", "ExceptionText()");

    return result;
  }

  /// <summary>
  /// Copies the template file to the output directory if it doesn't exist
  /// </summary>
  private void CopyTemplateFile()
  {
    var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "templates", "Template_BP_Base.vb");
    var outputPath = Path.Combine(_outputDirectory, "_BP_Base.vb");

    // Try different relative paths
    if (!File.Exists(templatePath))
    {
      templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Template_BP_Base.vb");
    }

    if (!File.Exists(templatePath))
    {
      // Try from project root
      templatePath = "Template_BP_Base.vb";
    }

    if (File.Exists(templatePath) && !File.Exists(outputPath))
    {
      File.Copy(templatePath, outputPath, false);
      Console.WriteLine($"Generated: {outputPath}");
    }
  }

  /// <summary>
  /// Copies the project file to the output directory with all generated files included
  /// </summary>
  private void CopyProjectFile()
  {
    var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "templates", "Template_BP_Project.vbproj");
    var outputPath = Path.Combine(_outputDirectory, "BluePrism_Generated.vbproj");

    // Try different relative paths
    if (!File.Exists(templatePath))
    {
      templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Template_BP_Project.vbproj");
    }

    if (!File.Exists(templatePath))
    {
      // Try from project root
      templatePath = "Template_BP_Project.vbproj";
    }

    if (File.Exists(templatePath))
    {
      var templateContent = File.ReadAllText(templatePath);

      // Get all .vb files in output directory
      var vbFiles = Directory.GetFiles(_outputDirectory, "*.vb")
        .Select(f => Path.GetFileName(f))
        .Where(f => f != "BluePrism_Generated.vbproj")
        .OrderBy(f => f == "_BP_Base.vb" ? 0 : 1)  // _BP_Base.vb first
        .ThenBy(f => f)
        .ToList();

      // Generate Compile Include lines
      var fileIncludes = new List<string>();
      foreach (var vbFile in vbFiles)
      {
        fileIncludes.Add($"    <Compile Include=\"{vbFile}\" />");
      }

      // Replace the placeholder with actual file includes
      var updatedContent = templateContent.Replace("    <!-- FILES -->", string.Join(Environment.NewLine, fileIncludes));

      // Add references from ProcessInfo if any (only those not already in template)
      if (_currentReferences != null && _currentReferences.Any())
      {
        var defaultRefs = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
          "System", "System.Data", "System.Xml", "System.Drawing", "System.Windows.Forms"
        };

        var refLines = new List<string>();
        foreach (var reference in _currentReferences)
        {
          var refName = Path.GetFileNameWithoutExtension(reference);
          // Skip if it's a default reference already in template
          if (defaultRefs.Contains(refName)) continue;

          refLines.Add($"    <Reference Include=\"{refName}\">");
          refLines.Add($"      <HintPath>{reference}</HintPath>");
          refLines.Add($"    </Reference>");
        }

        if (refLines.Any())
        {
          updatedContent = updatedContent.Replace("    <!-- REFERENCES -->", string.Join(Environment.NewLine, refLines));
        }
        else
        {
          updatedContent = updatedContent.Replace("    <!-- REFERENCES -->", "");
        }
      }

      File.WriteAllText(outputPath, updatedContent);
      Console.WriteLine($"Generated: {outputPath}");
    }
  }
}
