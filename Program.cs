// Blue Prism Process Export Tool
// Exports Blue Prism processes using the AutomateC.exe CLI
// Supports recursive export of dependent objects

using BPAnalyzer;


namespace BPAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                RunInteractive();
                return;
            }

            string mode = args[0].ToLowerInvariant();
            switch (mode)
            {
                case "export":
                    CliExportHandler.Run(args);
                    break;
                case "codegen":
                    CliCodeGenHandler.Run(args);
                    break;
                case "analyze":
                    CliAnalyzeHandler.Run(args);
                    break;
                case "help":
                case "--help":
                case "-h":
                    PrintUsage();
                    break;
                default:
                    Console.WriteLine($"Unbekannter Modus: {mode}\n");
                    PrintUsage();
                    Environment.ExitCode = 1;
                    break;
            }
        }

        static void RunInteractive()
        {
            Console.WriteLine("Welcome to BP-Analyzer (interactive mode)");
            Console.WriteLine("Please select a mode:");
            Console.WriteLine("  1) Export");
            Console.WriteLine("  2) Codegen");
            Console.WriteLine("  3) Analyze");
            Console.Write("Your choice (1-3): ");
            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    CliExportHandler.Run(new string[] { "export" });
                    break;
                case "2":
                    CliCodeGenHandler.Run(new string[] { "codegen" });
                    break;
                case "3":
                    CliAnalyzeHandler.Run(new string[] { "analyze" });
                    break;
                default:
                    Console.WriteLine("Invalid selection.");
                    break;
            }
        }

        static void PrintUsage()
        {
            Console.WriteLine("BP-Analyzer CLI");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  BP-Analyzer <mode> [options]");
            Console.WriteLine();
            Console.WriteLine("Modes:");
            Console.WriteLine("  export    Export Blue Prism processes as XML");
            Console.WriteLine("  codegen   Generate VB.NET code from Blue Prism XML");
            Console.WriteLine("  analyze   Analyze Blue Prism processes and create SDD document");
            Console.WriteLine();
            Console.WriteLine("For details on the options of a mode:");
            Console.WriteLine("  BP-Analyzer <mode> --help");
        }
    }
}
