using System.Xml.Linq;

namespace BPAnalyzer.CodeGen.Utilities;

/// <summary>
/// Maps BluePrism data types to VB.NET types.
/// </summary>
public static class TypeMapper
{
    /// <summary>
    /// Maps a BluePrism data type to the corresponding VB.NET type.
    /// </summary>
    public static string MapDataType(string bluePrismType, XElement? stage = null)
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

    /// <summary>
    /// Formats an initial value for a given BluePrism data type.
    /// </summary>
    public static string FormatInitialValue(string dataType, string value, XElement? stage = null)
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
            var vbType = MapDataType(fieldType);
            
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
                            values.Add(FormatInitialValue(fieldType, fieldValue));
                        }
                        else
                        {
                            values.Add(FormatInitialValue(fieldType, ""));
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
    /// Returns the default value for an optional parameter based on the BluePrism data type.
    /// Used to make all parameters optional with sensible defaults.
    /// </summary>
    public static string GetOptionalDefaultValue(string bluePrismType)
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

    /// <summary>
    /// Determines if a BluePrism data type maps to a VB.NET value type (which can be nullable).
    /// Reference types like String and DataTable cannot use nullable (?).
    /// </summary>
    public static bool IsValueType(string bluePrismType)
    {
        // return false; // TODO
        return bluePrismType?.ToLower() switch
        {
            "text" or "password" => false,  // String - reference type
            "number" => true,                // Decimal - value type
            "flag" or "boolean" => true,      // Boolean - value type
            "date" or "datetime" => true,     // DateTime - value type
            "time" or "timespan" => true,     // TimeSpan - value type
            "collection" => false,            // DataTable - reference type
            "binary" => false,                // Byte() - reference type (array)
            "object" => false,                // Object - reference type
            _ => false                        // Default to reference type
        };
    }
}
