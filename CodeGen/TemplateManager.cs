namespace BPAnalyzer.CodeGen;

/// <summary>
/// Manages template files for code generation.
/// </summary>
public class TemplateManager
{
    private readonly string _outputDirectory;

    public TemplateManager(string outputDirectory)
    {
        _outputDirectory = outputDirectory;
    }

    /// <summary>
    /// Copies the template file to the output directory if it doesn't exist.
    /// </summary>
    public void CopyTemplateFile()
    {
        var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "templates", "Template_BP_Base.vb");
        var outputPath = Path.Combine(_outputDirectory, "BP_Base.vb");

        // Try different relative paths
        if (!File.Exists(templatePath))
        {
            templatePath = Path.Combine(Directory.GetCurrentDirectory(), "templates", "Template_BP_Base.vb");
        }
        if (!File.Exists(templatePath))
        {
            templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Template_BP_Base.vb");
        }
        if (!File.Exists(templatePath))
        {
            templatePath = "Template_BP_Base.vb";
        }

        if (File.Exists(templatePath))
        {
            File.Copy(templatePath, outputPath, true);
            Console.WriteLine($"Generated: {outputPath}");
        }
    }

    /// <summary>
    /// Copies the project file to the output directory with all generated files included.
    /// </summary>
    public void CopyProjectFile(HashSet<string>? references = null)
    {
        var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "templates", "Template_BP_Project.vbproj");
        var outputPath = Path.Combine(_outputDirectory, "BluePrism_Generated.vbproj");

        // Try different relative paths
        if (!File.Exists(templatePath))
        {
            templatePath = Path.Combine(Directory.GetCurrentDirectory(), "templates", "Template_BP_Project.vbproj");
        }
        if (!File.Exists(templatePath))
        {
            templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Template_BP_Project.vbproj");
        }
        if (!File.Exists(templatePath))
        {
            templatePath = "Template_BP_Project.vbproj";
        }

        if (File.Exists(templatePath))
        {
            var templateContent = File.ReadAllText(templatePath);

            // Get all .vb files in output directory
            var vbFiles = Directory.GetFiles(_outputDirectory, "*.vb")
                .Select(f => Path.GetFileName(f))
                .OrderBy(f => f)
                .ToList();

            // Generate Compile Include lines
            var fileIncludes = new List<string>();
            foreach (var vbFile in vbFiles)
            {
                fileIncludes.Add($"    <Compile Include=\"{vbFile}\" />");
            }

            // Replace the placeholder with actual file includes
            var updatedContent = templateContent.Replace("    <!-- FILES -->", string.Join(Environment.NewLine, fileIncludes));

            // Add references from ProcessInfo if any
            if (references != null && references.Any())
            {
                var defaultRefs = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "System", "System.Data", "System.Xml", "System.Drawing", "System.Windows.Forms"
                };

                var refLines = new List<string>();
                foreach (var reference in references)
                {
                    var refName = Path.GetFileNameWithoutExtension(reference);
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
