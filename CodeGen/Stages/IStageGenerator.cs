using System.Xml.Linq;

namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Interface for all stage generators.
/// Each stage type in BluePrism should have its own implementation.
/// </summary>
public interface IStageGenerator
{
    /// <summary>
    /// Generates VB.NET code for the given stage.
    /// </summary>
    /// <param name="stage">The XML element representing the stage.</param>
    /// <param name="sb">The StringBuilder to append code to.</param>
    void Generate(XElement stage, System.Text.StringBuilder sb);
}
