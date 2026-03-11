using System.Xml.Linq;
using BPAnalyzer.CodeGen.Utilities;

namespace BPAnalyzer.CodeGen;

/// <summary>
/// Generates VB.NET code for data items (global and local).
/// </summary>
public static class DataItemGenerator
{
    /// <summary>
    /// Generates global (class-level) data items.
    /// </summary>
    public static void GenerateGlobalDataItems(XElement process, System.Text.StringBuilder sb)
    {
        var allStages = process.Descendants()
            .Where(e => e.Name.LocalName == "stage")
            .ToList();

        // Global data items: Data and Collection stages that are not private
        var globalDataStages = allStages
            .Where(e =>
            {
                var stageType = e.Attribute("type")?.Value;
                if (stageType != "Data" && stageType != "Collection") return false;
                return e.Element("private") == null;
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
            if (string.IsNullOrEmpty(name)) continue;

            var datatype = stage.Element("datatype")?.Value ?? "text";
            var vbType = TypeMapper.MapDataType(datatype, stage);

            sb.AppendLine($"    ' {name} ({datatype})");
            sb.AppendLine($"    Protected {NameSanitizer.SanitizeVariableName(name)} As {vbType}");
        }
    }

    /// <summary>
    /// Generates local (method-level) data items with optional initialization.
    /// </summary>
    public static void GenerateLocalDataItems(
        System.Text.StringBuilder sb,
        List<XElement> stages,
        HashSet<string> paramNames)
    {
        var allDataStages = stages
            .Where(e =>
            {
                var stageType = e.Attribute("type")?.Value;
                return stageType == "Data" || stageType == "Collection";
            })
            .ToList();

        // Local data items: private data stages that are not input/output parameters
        var localDataStages = allDataStages
            .Where(e =>
            {
                // Skip if not private
                if (e.Element("private") == null) return false;

                // Skip if this is an input or output parameter
                var stageName = e.Attribute("name")?.Value?.ToLower();
                if (string.IsNullOrEmpty(stageName)) return false;
                if (paramNames.Contains(stageName)) return false;

                return true;
            })
            .ToList();

        if (!localDataStages.Any()) return;

        sb.AppendLine("        ' Local variables");

        foreach (var dataStage in localDataStages)
        {
            var dataName = dataStage.Attribute("name")?.Value!;
            var dataType = dataStage.Element("datatype")?.Value ?? "text";
            var vbType = TypeMapper.MapDataType(dataType, dataStage);
            var hasAlwaysInit = dataStage.Element("alwaysinit") != null;
            var keyword = hasAlwaysInit ? "Dim" : "Static";
            // Make local variables nullable for value types
            var nullableMarker = TypeMapper.IsValueType(dataType) ? "?" : "";

            sb.AppendLine($"        {keyword} {NameSanitizer.SanitizeVariableName(dataName)} As {vbType}{nullableMarker}");
        }
        sb.AppendLine();
    }

    /// <summary>
    /// Generates initialization code for variables with initial values.
    /// </summary>
    public static void GenerateInitialValueInitialization(
        System.Text.StringBuilder sb,
        List<XElement> stages)
    {
        var allDataStages = stages
            .Where(e =>
            {
                var stageType = e.Attribute("type")?.Value;
                return stageType == "Data" || stageType == "Collection";
            })
            .ToList();

        var stagesWithInitialValue = allDataStages.Where(e => e.Element("initialvalue") != null).ToList();

        // Separate regular data stages from collection stages
        var regularDataStages = stagesWithInitialValue.Where(e => e.Attribute("type")?.Value == "Data").ToList();
        var collectionStages = stagesWithInitialValue.Where(e => e.Attribute("type")?.Value == "Collection").ToList();

        // Generate initialization for regular data stages
        if (regularDataStages.Any())
        {
            var commentWritten = false;
            foreach (var dataStage in regularDataStages)
            {
                var dataName = dataStage.Attribute("name")?.Value!;
                var dataType = dataStage.Element("datatype")?.Value ?? "text";
                var initialValue = dataStage.Element("initialvalue")?.Value;
                var alwaysinit = dataStage.Element("alwaysinit");

                if (!string.IsNullOrEmpty(initialValue))
                {
                    if (!commentWritten)
                    {
                        sb.AppendLine("        ' Initialize variables with initialvalue");
                        commentWritten = true;
                    }

                    var formattedValue = TypeMapper.FormatInitialValue(dataType, initialValue, dataStage);
                    if (alwaysinit == null)
                    {
                        sb.AppendLine($"        If {NameSanitizer.SanitizeVariableName(dataName)} Is Nothing Then");
                        sb.AppendLine($"            {NameSanitizer.SanitizeVariableName(dataName)} = {formattedValue}");
                        sb.AppendLine($"        End If");
                    }
                    else
                    {
                        sb.AppendLine($"        {NameSanitizer.SanitizeVariableName(dataName)} = {formattedValue}");
                    }
                }
            }
        }

        // Generate initialization for collection stages
        if (collectionStages.Any())
        {
            sb.AppendLine();
            sb.AppendLine("        ' Initialize collections");
            foreach (var collectionStage in collectionStages)
            {
                var collectionInit = TypeMapper.GenerateCollectionInitialization(collectionStage);
                if (!string.IsNullOrEmpty(collectionInit))
                {
                    // Split by newlines and indent each line
                    foreach (var line in collectionInit.Split('\n'))
                    {
                        sb.AppendLine($"        {line.TrimEnd()}");
                    }
                }
            }
        }

        sb.AppendLine();
    }

    /// <summary>
    /// Generates initialization code for global collections (class-level).
    /// </summary>
    public static void GenerateGlobalCollectionInitialization(XElement process, System.Text.StringBuilder sb)
    {
        var allStages = process.Descendants()
            .Where(e => e.Name.LocalName == "stage")
            .ToList();

        // Global collections: Collection stages that are not in a subsheet and not private
        var globalCollectionStages = allStages
            .Where(e =>
            {
                var stageType = e.Attribute("type")?.Value;
                if (stageType != "Collection") return false;

                // Has no subsheetid means global
                var hasNoSubsheet = string.IsNullOrEmpty(e.Element("subsheetid")?.Value);

                // Is not private
                var isPrivate = e.Element("private") != null;

                return hasNoSubsheet && !isPrivate;
            })
            .Where(e => e.Element("collectioninfo") != null || e.Element("initialvalue") != null)
            .ToList();

        if (globalCollectionStages.Count == 0)
        {
            return;
        }

        sb.AppendLine("    ' Initialize global collections");
        foreach (var collectionStage in globalCollectionStages)
        {
            var collectionInit = TypeMapper.GenerateCollectionInitialization(collectionStage);
            if (!string.IsNullOrEmpty(collectionInit))
            {
                // Indent for class level
                foreach (var line in collectionInit.Split('\n'))
                {
                    sb.AppendLine($"    {line.TrimEnd()}");
                }
            }
        }
        sb.AppendLine();
    }
}
