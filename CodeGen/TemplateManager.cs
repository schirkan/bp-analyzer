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
        var outputPath = Path.Combine(_outputDirectory, "BP_Base.vb");

        // TemplateProvider checks disk locations first, then the embedded resource
        // (keeps the single-file build self-contained).
        var templateContent = TemplateProvider.TryGetTemplateText("Template_BP_Base.vb");

        if (templateContent != null)
        {
            File.WriteAllText(outputPath, templateContent);
            Console.WriteLine($"Generated: {outputPath}");
        }
        else
        {
            Console.WriteLine($"Warning: template 'Template_BP_Base.vb' not found - {outputPath} was not created.");
        }
    }

    /// <summary>
    /// Copies the project file to the output directory with all generated files included.
    /// </summary>
    public void CopyProjectFile(HashSet<string>? references = null)
    {
        var outputPath = Path.Combine(_outputDirectory, "BluePrism_Generated.vbproj");

        // TemplateProvider checks disk locations first, then the embedded resource
        // (keeps the single-file build self-contained).
        var templateContent = TemplateProvider.TryGetTemplateText("Template_BP_Project.vbproj");

        if (templateContent != null)
        {

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
