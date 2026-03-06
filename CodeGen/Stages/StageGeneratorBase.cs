using System.Xml.Linq;
using BPAnalyzer.CodeGen.Utilities;

namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Base class for all stage generators providing common functionality.
/// </summary>
public abstract class StageGeneratorBase : IStageGenerator
{
  /// <inheritdoc/>
  public abstract string StageType { get; }

  /// <summary>
  /// Generates the GoTo statement for the next stage.
  /// </summary>
  protected void GenerateGoTo(System.Text.StringBuilder sb, XDocument? document, string? targetStageId, int indentation = 8)
  {
    if (string.IsNullOrEmpty(targetStageId) || document == null)
    {
      return;
    }

    var targetStageLabel = FindStageLabel(targetStageId, document);
    var spaces = "".PadLeft(indentation);
    sb.AppendLine($"{spaces}GoTo {targetStageLabel}");
  }

  /// <summary>
  /// Generates parameter code for Action and Process stages.
  /// This eliminates duplicate code between these two stage types.
  /// </summary>
  protected string GenerateParameterCode(
      IEnumerable<XElement> inputs,
      IEnumerable<XElement> outputs)
  {
    var inputParams = new List<string>();

    foreach (var input in inputs)
    {
      var inputName = input.Attribute("name")?.Value;
      var inputExpr = input.Attribute("expr")?.Value;

      if (string.IsNullOrEmpty(inputName) || string.IsNullOrEmpty(inputExpr)) continue;

      var sanitizedInputName = NameSanitizer.SanitizeVariableName(inputName ?? "");
      var formattedExpr = ExpressionParser.FormatExpression(inputExpr);

      inputParams.Add($"{sanitizedInputName}:={formattedExpr}");
    }

    var outputParams = new List<string>();
    foreach (var output in outputs)
    {
      var outputName = output.Attribute("name")?.Value;
      var outputStage = output.Attribute("stage")?.Value;

      if (string.IsNullOrEmpty(outputName) || string.IsNullOrEmpty(outputStage)) continue;

      var sanitizedOutputName = NameSanitizer.SanitizeVariableName(outputName ?? "");
      var sanitizedStage = NameSanitizer.SanitizeVariableName(outputStage ?? "");

      outputParams.Add($"{sanitizedOutputName}:={sanitizedStage}");
    }

    var allParams = inputParams.Concat(outputParams).ToList();
    return string.Join(", ", allParams);
  }

  /// <summary>
  /// Finds the stage label for a given stage ID.
  /// </summary>
  protected string FindStageLabel(string stageId, XDocument doc)
  {
    if (string.IsNullOrEmpty(stageId)) return GetStageLabel("Unknown", stageId);

    var stage = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "stage" && e.Attribute("stageid")?.Value == stageId);

    if (stage == null) return GetStageLabel("Unknown", stageId);

    var stageType = stage.Attribute("type")?.Value ?? "Unknown";

    if (stageType == "End")
    {
      var stageSubsheetId = stage.Element("subsheetid")?.Value;
      var endStage = doc.Descendants()
          .FirstOrDefault(e => e.Name.LocalName == "stage"
              && e.Attribute("type")?.Value == "End"
              && e.Element("subsheetid")?.Value == stageSubsheetId);

      return GetStageLabel("End", endStage?.Attribute("stageid")?.Value ?? stageId);
    }

    if (stageType == "Anchor")
    {
      return ResolveAnchorChain(stageId, doc);
    }

    return GetStageLabel(stageType, stageId);
  }

  private string ResolveAnchorChain(string stageId, XDocument doc)
  {
    var visited = new HashSet<string>();
    var currentId = stageId;

    while (!string.IsNullOrEmpty(currentId) && !visited.Contains(currentId))
    {
      visited.Add(currentId);
      var stage = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "stage" && e.Attribute("stageid")?.Value == currentId);

      if (stage == null)
      {
        return GetStageLabel("Unknown", stageId);
      }

      var stageType = stage.Attribute("type")?.Value;
      if (stageType != "Anchor")
      {
        return FindStageLabel(currentId, doc);
      }

      currentId = stage.Element("onsuccess")?.Value;
    }

    return GetStageLabel("Unknown", stageId);
  }

  /// <summary>
  /// Gets the stage label for a given stage type and ID.
  /// </summary>
  protected string GetStageLabel(string stageType, string stageId)
  {
    var sanitizedId = NameSanitizer.SanitizeId(stageId);
    return $"{stageType}_{sanitizedId}_Label";
  }

  /// <inheritdoc/>
  public abstract void Generate(XElement stage, System.Text.StringBuilder sb);
}
