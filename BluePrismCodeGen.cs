using System.Xml.Linq;
using BPAnalyzer.CodeGen;

namespace BPAnalyzer;

/// <summary>
/// Generates VB.Net code from BluePrism XML files.
/// Uses Goto statements to recreate the process flow.
/// Supports static variables for BluePrism data items.
/// </summary>
public class BluePrismCodeGen
{
    private readonly string _outputDirectory;
    private HashSet<string> _currentReferences = [];
    private TemplateManager? _templateManager;

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

        var code = ClassGenerator.GenerateClass(process, _currentReferences);
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

        // Initialize template manager
        _templateManager = new TemplateManager(_outputDirectory);

        // Copy template files first
        _templateManager.CopyTemplateFile();

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
                Console.WriteLine(ex.StackTrace);
            }
        }

        // Copy project file last (after all .vb files are generated)
        _templateManager.CopyProjectFile(_currentReferences);
    }
}
