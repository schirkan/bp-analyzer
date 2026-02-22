// Blue Prism Prozess-Export Tool
// Exportiert Blue Prism Prozesse mit AutomateC.exe CLI

class Program
{
    static void Main(string[] args)
    {
        ExporterCLI exporter = new ExporterCLI();
        exporter.ExportProcess();
    }
}
