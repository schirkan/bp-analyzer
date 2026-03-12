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

            var dataType = stage.Element("datatype")?.Value ?? "text";
            var vbType = TypeMapper.MapDataType(dataType, stage);
            var nullableMarker = TypeMapper.IsValueType(dataType) ? "?" : "";

            sb.AppendLine($"    Protected {NameSanitizer.SanitizeVariableName(name)} As {vbType}{nullableMarker}");
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
        // Generate initialization for regular data stages
        var dataStages = stages.Where(e => e.Attribute("type")?.Value == "Data").ToList();
        if (dataStages.Any())
        {
            var commentWritten = false;

            foreach (var dataStage in dataStages)
            {
                var dataName = dataStage.Attribute("name")?.Value!;
                var dataType = dataStage.Element("datatype")?.Value ?? "text";
                var initialValue = dataStage.Element("initialvalue")?.Value;
                var alwaysinit = dataStage.Element("alwaysinit") != null;

                // TODO: value of input parameters will be overwritten
                if (!string.IsNullOrEmpty(initialValue))
                {
                    if (!commentWritten)
                    {
                        sb.AppendLine("        ' Initialize variables");
                        commentWritten = true;
                    }

                    var formattedValue = TypeMapper.FormatInitialValue(dataType, initialValue, dataStage);
                    if (!alwaysinit) sb.AppendLine($"        If {NameSanitizer.SanitizeVariableName(dataName)} Is Nothing Then");
                    if (!alwaysinit) sb.Append("    ");
                    sb.AppendLine($"        {NameSanitizer.SanitizeVariableName(dataName)} = {formattedValue}");
                    if (!alwaysinit) sb.AppendLine($"        End If");
                }
            }
        }

        // Generate initialization for collection stages
        var collectionStages = stages.Where(e => e.Attribute("type")?.Value == "Collection").ToList();
        if (collectionStages.Any())
        {
            sb.AppendLine();
            sb.AppendLine("        ' Initialize collections");
            foreach (var collectionStage in collectionStages)
            {
                var collectionName = collectionStage.Attribute("name")?.Value!;
                var collectionInit = GenerateCollectionInitialization(collectionStage);

                if (!string.IsNullOrEmpty(collectionInit))
                {
                    sb.AppendLine($"        If {NameSanitizer.SanitizeVariableName(collectionName)} Is Nothing Then");
                    // Split by newlines and indent each line
                    foreach (var line in collectionInit.Split('\n'))
                    {
                        sb.AppendLine($"            {line.TrimEnd()}");
                    }
                    sb.AppendLine($"        End If");
                }
            }
        }

        sb.AppendLine();
    }

    /// <summary>
    /// Generates VB.NET code for initializing a Collection (DataTable) with columns and initial rows.
    /// </summary>
    public static string GenerateCollectionInitialization(XElement collectionStage)
    {
        var sb = new System.Text.StringBuilder();
        var collectionName = collectionStage.Attribute("name")?.Value;

        if (string.IsNullOrEmpty(collectionName)) return string.Empty;

        var sanitizedName = NameSanitizer.SanitizeVariableName(collectionName);
        var collectionInfo = collectionStage.Element("collectioninfo");
        var initialValue = collectionStage.Element("initialvalue");

        // Check if collection has columns defined
        var fields = collectionInfo?.Elements("field").ToList();

        if (fields == null || !fields.Any())
        {
            // No columns defined, just initialize empty DataTable
            return $"{sanitizedName} = New DataTable()";
        }

        // Start DataTable initialization
        sb.AppendLine($"{sanitizedName} = New DataTable()");

        // Add columns
        foreach (var field in fields)
        {
            var fieldName = field.Attribute("name")?.Value;
            var fieldType = field.Attribute("type")?.Value ?? "text";
            var vbType = TypeMapper.MapDataType(fieldType);

            // Map BluePrism types to VB.NET GetType
            var getTypeArg = fieldType.ToLower() switch
            {
                "text" or "password" => "String",
                "number" => "Decimal",
                "flag" or "boolean" => "Boolean",
                "date" or "datetime" => "DateTime",
                "time" or "timespan" => "TimeSpan",
                "binary" => "Byte()",
                _ => "String"
            };

            sb.AppendLine($"{sanitizedName}.Columns.Add(\"{fieldName}\", GetType({getTypeArg}))");
        }

        // Add initial rows
        if (initialValue != null)
        {
            var rows = initialValue.Elements("row").ToList();
            foreach (var row in rows)
            {
                var fieldsInRow = row.Elements("field").ToList();
                if (fieldsInRow.Any())
                {
                    var values = new List<string>();
                    foreach (var fieldDef in fields)
                    {
                        var fieldName = fieldDef.Attribute("name")?.Value;
                        var fieldType = fieldDef.Attribute("type")?.Value ?? "text";

                        var fieldInRow = fieldsInRow.FirstOrDefault(f => f.Attribute("name")?.Value == fieldName);
                        var fieldValue = fieldInRow?.Attribute("value")?.Value;

                        if (fieldValue != null)
                        {
                            values.Add(TypeMapper.FormatInitialValue(fieldType, fieldValue));
                        }
                        else
                        {
                            values.Add(TypeMapper.FormatInitialValue(fieldType, ""));
                        }
                    }

                    if (values.Any())
                    {
                        sb.AppendLine($"{sanitizedName}.Rows.Add({string.Join(", ", values)})");
                    }
                }
            }
        }

        return sb.ToString().TrimEnd();
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
            var collectionInit = GenerateCollectionInitialization(collectionStage);
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
