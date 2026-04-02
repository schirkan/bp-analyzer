# BP-Analyzer

BluePrism XML to VB.NET Code Generator

This tool converts BluePrism XML export files into VB.NET classes with GoTo-based process flow.

## Table of Contents

- [BP-Analyzer](#bp-analyzer)
  - [Table of Contents](#table-of-contents)
  - [Installation](#installation)
  - [CLI Usage](#cli-usage)
    - [Export Mode](#export-mode)
    - [Codegen Mode](#codegen-mode)
    - [Analyze Mode](#analyze-mode)
    - [Notes](#notes)
  - [Compiling Output](#compiling-output)
    - [Using dotnet MSBuild](#using-dotnet-msbuild)
    - [Using MSBuild (Visual Studio)](#using-msbuild-visual-studio)
    - [Compiled Output](#compiled-output)
  - [Generated Code](#generated-code)
    - [Singleton Pattern](#singleton-pattern)
    - [Exception Handling](#exception-handling)
    - [Process Flow](#process-flow)
  - [File Structure](#file-structure)
  - [Supported BluePrism Stages](#supported-blueprism-stages)
  - [License](#license)

## Installation

```bash
# Build project
dotnet build BP-Analyzer.csproj

# Or run directly
dotnet run --project BP-Analyzer.csproj

# run codegen and build output
dotnet run --project BP-Analyzer.csproj -- codegen && dotnet build output/BluePrism_Generated.vbproj 2>&1
```


## CLI Usage

The CLI supports three main modes:

- **export**   – Exports Blue Prism processes as XML
- **codegen**  – Generates VB.NET code from Blue Prism XML
- **analyze**  – Analyzes Blue Prism processes

If started without parameters, an interactive menu is shown. All required parameters for the selected mode will be prompted interactively.

---

### Export Mode
Exports a Blue Prism process as an XML file.

```
dotnet run --project BP-Analyzer.csproj -- export --process=<name> [--output=<path>] [--user=<username>] [--password=<password>] [--overwrite=<yes|no>]
```

- `--process=<name>`        Name of the process to export (**required**)
- `--output=<path>`         Output directory (default: xml/)
- `--user=<username>`       Blue Prism username
- `--password=<password>`   Blue Prism password
- `--overwrite=<yes|no>`    Overwrite existing files (default: yes)

**Example:**
```
dotnet run --project BP-Analyzer.csproj -- export --process "MyProcess" --output "C:\Exports"
```

### Codegen Mode
Generates VB.NET code from Blue Prism XML files.

```
dotnet run --project BP-Analyzer.csproj -- codegen [--xml=<directory>] [--output=<directory>]
```

- `--xml=<directory>`     Directory with BluePrism XML files (default: xml/)
- `--output=<directory>`  Output directory (default: code/)

**Example:**
```
dotnet run --project BP-Analyzer.csproj -- codegen --xml=xml --output=code
```


### Analyze Mode

Generates SDD (Solution Design Document) files from the generated JSON files.

```
dotnet run --project BP-Analyzer.csproj -- analyze [--code=<json-dir>] [--output=<sdd-dir>] [--exclude-exception-sources=<prefix1,prefix2,...>]
```

- `--code=<json-dir>`   Directory with classes.json, dependencies.json, exceptions.json (default: output/)
- `--output=<sdd-dir>`  Output directory for SDD files (default: sdd/)
- `--exclude-exception-sources=<prefix1,prefix2,...>`  Comma-separated list of exception source prefixes to exclude from SDD exception listing (default: ERGO_,Utility_)


**Examples:**
```
# Standard (default exclusions: ERGO_, Utility_)
dotnet run --project BP-Analyzer.csproj -- analyze --code=output --output=sdd

# Mit eigenen Prefixen (z.B. nur Utility und TEST ausschließen)
dotnet run --project BP-Analyzer.csproj -- analyze --code=output --output=sdd --exclude-exception-sources=Utility_,TEST_
```




### Notes
- Internal BluePrism objects (e.g., `Blueprism.Automate*`) are automatically skipped during export.
- There are no short forms or backwards compatibility with old calls.
- For help on a mode: `BP-Analyzer <mode> --help`

## Compiling Output

The generated VB.NET files can be compiled using MSBuild or dotnet.

### Using dotnet MSBuild

```bash
# Change to output directory
cd output

# Build project
dotnet build BluePrism_Generated.vbproj
```

### Using MSBuild (Visual Studio)

```powershell
# Build
msbuild BluePrism_Generated.vbproj

# Build with Debug configuration
msbuild BluePrism_Generated.vbproj /p:Configuration=Debug

# Build with Release configuration
msbuild BluePrism_Generated.vbproj /p:Configuration=Release
```

### Compiled Output

The compiled project creates a DLL file:

```
output/bin/Debug/BluePrism_Generated.dll
# or
output/bin/Release/BluePrism_Generated.dll
```

> **Note:** The generated code is syntactically valid and can be compiled, but cannot be executed as a real Blue Prism process. Many functionalities (especially UI automation, object calls, and some stage types) are implemented as placeholders or stubs only.

## Generated Code

### Singleton Pattern

Each generated class uses the Lazy Initialization Singleton Pattern:

```vb
Public Class Utility___Environment
    Inherits BP_Base

    ' Singleton Instance
    Private Shared ReadOnly _lazyInstance As New Lazy(Of Utility___Environment)(Function() New Utility___Environment())

    Public Shared ReadOnly Property Instance As Utility___Environment
        Get
            Return _lazyInstance.Value
        End Get
    End Property

    ' ... additional class members
End Class
```

**Usage:**
```vb
Utility_Environment.Instance.Start_Process(Application:=FilePath)
```

### Exception Handling

The BP_Base template provides exception handling methods. Exceptions are raised using `Throw`:

```vb
' Raise exception (in Exception stage)
Throw New BP_Exception("System Exception", "Error message")

' Store exception (in Recover stage)
StoreException()

' Clear stored exception (in Resume stage)
ClearException()

' Get exception information
Dim exType As String = ExceptionType()
Dim exText As String = ExceptionDetail()
Dim exStage As String = ExceptionStage()

' Rethrow the stored exception
Throw GetLastException()
```

### Process Flow

The process flow is reconstructed using GoTo labels and conditions:

```vb
Public Sub Main()
    ' Start
    GoTo Start_Main_Label

Start_Main_Label: ' Main process
    ' Decision
    If condition = True Then
        GoTo True_Path_Label
    Else
        GoTo False_Path_Label
    End If

True_Path_Label: ' True path
    ' ... actions
    GoTo End_Label

False_Path_Label: ' False path
    ' ... actions
    GoTo End_Label

End_Label:
    ' Assign output parameters
End Sub
```

## File Structure

```
bp-analyzer/
├── xml/                           # BluePrism XML exports (input)
│   ├── Test Process.xml
│   ├── Microsoft Store.xml
│   └── ...
├── output/                        # Generated VB.NET files
│   ├── BP_Base.vb                # Base class template
│   ├── Test Process.vb
│   ├── Microsoft Store.vb
│   └── BluePrism_Generated.vbproj
├── templates/                     # VB.NET templates
│   ├── Template_BP_Base.vb        # Base class template
│   └── Template_BP_Project.vbproj # VB project template
├── CodeGen/                       # Code generator modules
│   ├── ClassGenerator.cs
│   ├── DataItemGenerator.cs
│   ├── MethodGenerator.cs
│   ├── TemplateManager.cs
│   ├── FlowControl/
│   │   ├── FlowController.cs
│   │   └── StageNavigator.cs
│   ├── Stages/                    # Stage generators
│   │   ├── ActionStageGenerator.cs
│   │   ├── DecisionStageGenerator.cs
│   │   ├── CalculationStageGenerator.cs
│   │   ├── ExceptionStageGenerator.cs
│   │   ├── RecoverStageGenerator.cs
│   │   ├── ResumeStageGenerator.cs
│   │   └── ... (other stages)
│   └── Utilities/
│       ├── ExpressionParser.cs
│       ├── NameSanitizer.cs
│       ├── TypeMapper.cs
│       └── ...
├── BluePrismCodeGen.cs           # Main code generator
├── BluePrismExporter.cs          # BluePrism export logic
├── ExporterCLI.cs                # CLI export commands
├── Program.cs                    # Entry point
├── BP-Analyzer.csproj            # Project file
├── BP-Analyzer.sln               # Solution file
└── README.md                     # This file
```

## Supported BluePrism Stages

| Stage Type        | Description         | Supported | Notes                               |
| ----------------- | ------------------- | --------- | ----------------------------------- |
| Start             | Method entry        | ✓         |                                     |
| End               | Method exit         | ✓         |                                     |
| Data              | Variables           | ✓         | Only env exposure                   |
| Collection        | DataTables          | ✓         |                                     |
| Action            | Object calls        | ✓         |                                     |
| Decision          | If/Else branch      | ✓         |                                     |
| Calculation       | Variable assignment | ✓         |                                     |
| Code              | Code stages         | partial   | Only VB code generates valid output |
| Note              | Comments            | ✓         |                                     |
| Exception         | Raise exception     | ✓         |                                     |
| Recover           | Store exception     | ✓         | No counter                          |
| Resume            | Resume exception    | ✓         |                                     |
| Navigate          | UI automation       | ✓         | Dummy implementation                |
| Write             | UI automation       | ✓         | Dummy implementation                |
| Read              | UI automation       | ✓         | Dummy implementation                |
| WaitStart/WaitEnd | Wait stages         | ✓         | Dummy implementation                |
| Process           | Subprocess call     | ✓         |                                     |
| Anchor            | Jump marker         | ✓         | Will be skipped in output           |
| Block             | Block container     | ✓         | Only used in exception handling     |
| LoopStart/LoopEnd | Loops               | ✓         | Uses goto statement                 |

## License

MIT License
