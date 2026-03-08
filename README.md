# BP-Analyzer

BluePrism XML to VB.NET Code Generator

This tool converts BluePrism XML export files into VB.NET classes with GoTo-based process flow.

## Table of Contents

- [BP-Analyzer](#bp-analyzer)
  - [Table of Contents](#table-of-contents)
  - [Installation](#installation)
  - [CLI Commands](#cli-commands)
    - [Code Generation](#code-generation)
      - [Basic Commands](#basic-commands)
      - [All CLI Variants](#all-cli-variants)
    - [Process Export](#process-export)
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
```

## CLI Commands

### Code Generation

Converts BluePrism XML files to VB.NET classes.

#### Basic Commands

```bash
# Default (uses "xml" and "output" directories)
dotnet run --project BP-Analyzer.csproj -- --codegen

# With explicit directories
dotnet run --project BP-Analyzer.csproj -- --codegen --xml=xml --output=output

# Short syntax
dotnet run --project BP-Analyzer.csproj -- --codegen xml output

# Single directory (only xml, output = "output")
dotnet run --project BP-Analyzer.csproj -- --codegen xml
```

#### All CLI Variants

| Command                                          | Description                           |
| ------------------------------------------------ | ------------------------------------- |
| `dotnet run -- --codegen`                        | Default: `xml/` → `output/`           |
| `dotnet run -- --codegen --xml=mydirectory`      | Custom XML directory                  |
| `dotnet run -- --codegen --output=mydirectory`   | Custom output directory               |
| `dotnet run -- --codegen --xml=xml --output=out` | Both directories                      |
| `dotnet run -- --codegen xml`                    | XML directory as positional parameter |
| `dotnet run -- --codegen xml out`                | Both as positional parameters         |

### Process Export

Exports BluePrism processes directly from Runtime Resource (requires AutomateC.exe).

**Default output directory:** `xml/`
**Default overwrite:** `yes` (files are automatically overwritten)

```bash
# Basic export (to xml/ with overwrite)
dotnet run --project BP-Analyzer.csproj -- --process="MyProcess"

# With output directory
dotnet run --project BP-Analyzer.csproj -- --process="MyProcess" --output="C:\Exports"

# With credentials
dotnet run --project BP-Analyzer.csproj -- --process="MyProcess" --user=admin --password=secret

# Disable overwrite (if needed)
dotnet run --project BP-Analyzer.csproj -- --process="MyProcess" --overwrite=no

# All together
dotnet run --project BP-Analyzer.csproj -- --process="MyProcess" --output="C:\Exports" --user=admin --password=secret --overwrite=no
```

**Note:** Internal BluePrism objects (e.g., `Blueprism.AutomateProcessCore.*`) are automatically skipped.

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
' Instead of: _utility.Start_Process(...)
' Now:
Utility___Environment.Instance.Start_Process(Application:=FilePath)
```

### Exception Handling

The BP_Base template provides exception handling methods:

```vb
' Raise exception
RaiseException("System Exception", "Error message")

' Store exception (in Recover stage)
StoreException()

' Rethrow stored exception
RethrowException()

' Get exception information
Dim exType As String = ExceptionType()
Dim exText As String = ExceptionText()
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
│   ├── _BP_Base.vb                # Base class template
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

| Stage Type        | Description         | Supported | Notes                                      |
| ----------------- | ------------------- | --------- | ------------------------------------------ |
| Start             | Method entry        | ✓         |                                            |
| End               | Method exit         | ✓         |                                            |
| Data              | Variables           | ✓         |                                            |
| Collection        | DataTables          | partial   | column definition + initialization missing |
| Action            | Object calls        | ✓         |                                            |
| Decision          | If/Else branch      | ✓         |                                            |
| Calculation       | Variable assignment | ✓         |                                            |
| Code              | Code stages         | ✓         | Only VB code generates valid output        |
| Note              | Comments            | ✓         |                                            |
| Exception         | Raise exception     | ✓         |                                            |
| Recover           | Store exception     | ✓         |                                            |
| Resume            | Resume exception    | ✓         |                                            |
| Navigate          | UI automation       | ✓         | Dummy implementation                       |
| Write             | UI automation       | ✓         | Dummy implementation                       |
| Read              | UI automation       | ✓         | Dummy implementation                       |
| Process           | Subprocess call     | ✓         |                                            |
| Anchor            | Jump marker         | ✓         | Will be skipped in output                  |
| Block             | Block container     | ✓         | Only used in exception handling            |
| WaitStart/WaitEnd | Wait stages         | ✓         |                                            |
| LoopStart/LoopEnd | Loops               | no        |                                            |

## License

MIT License
