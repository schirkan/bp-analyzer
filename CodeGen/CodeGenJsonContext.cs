using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace BPAnalyzer.CodeGen;

/// <summary>
/// Source-generated JSON metadata for the three registry files
/// (classes.json, dependencies.json, exceptions.json).
///
/// Required for trimmed single-file builds: with PublishTrimmed=true the runtime
/// disables reflection-based serialization, so JsonSerializer.Serialize&lt;T&gt;(T, options)
/// would fail with "Reflection-based serialization has been disabled for this application".
/// The source generator emits the type metadata at compile time, which is trim-safe.
/// </summary>
// PropertyNameCaseInsensitive: the config file uses camelCase (automateCPath),
// the C# property is PascalCase (AutomateCPath). Without this the value would be
// silently ignored.
[JsonSourceGenerationOptions(WriteIndented = true, PropertyNameCaseInsensitive = true)]
[JsonSerializable(typeof(Dictionary<string, List<string[]>>))]
[JsonSerializable(typeof(Dictionary<string, List<string>>))]
[JsonSerializable(typeof(Dictionary<string, Dictionary<string, string>>))]
[JsonSerializable(typeof(BPAnalyzer.AnalyzerConfig))]
public partial class CodeGenJsonContext : JsonSerializerContext
{
}
