using System.Xml.Linq;
using BPAnalyzer.CodeGen.Utilities;

namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Base class for all stage generators providing common functionality.
/// </summary>
public abstract class StageGeneratorBase : IStageGenerator
{
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

    if (allParams.Count == 1) return allParams[0];
    return (LinePrefix + string.Join(", " + LinePrefix, allParams)).TrimEnd();
  }

  private const string LinePrefix = "\r\n            ";

  /// <inheritdoc/>
  public abstract void Generate(XElement stage, System.Text.StringBuilder sb);
}
