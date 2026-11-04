# BP-Analyzer

BluePrism XML to VB.NET Code Generator

This tool converts BluePrism XML export files into VB.NET classes with GoTo-based process flow.

## Table of Contents

- [BP-Analyzer](#bp-analyzer)
  - [Table of Contents](#table-of-contents)
  - [Installation](#installation)
    - [Single-file executable](#single-file-executable)
    - [Configuration](#configuration)
    - [Release](#release)
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

Building the analyzer requires the .NET 8 SDK or a compatible newer SDK. The
analyzer targets `net8.0`; generated VB.NET projects target .NET Framework 4.8
and need the corresponding reference assemblies/developer pack to build.

```bash
# Build project
dotnet build BP-Analyzer.csproj

# Or run directly
dotnet run --project BP-Analyzer.csproj

# run codegen and build output
dotnet run --project BP-Analyzer.csproj -- codegen --xml=xml --output=code
dotnet build code/BluePrism_Generated.vbproj
```

### Single-file executable

A standalone Windows executable can be published. It needs no .NET runtime and no
additional files - the templates are embedded in the binary.

```powershell
dotnet publish BP-Analyzer.csproj -p:PublishProfile=BP-Analyzer-win-x64 -o publish
```

This produces a single `publish/BP-Analyzer.exe` (win-x64, approx. 12.6 MB).
The profile uses `SelfContained`, `PublishSingleFile`, `PublishTrimmed` and
`EnableCompressionInSingleFile`.

### Configuration

The `codegen` and `analyze` modes need no configuration - the templates are
embedded in the executable. Only the `export` mode needs the path to Blue Prism's `AutomateC.exe`,
which defaults to the standard installation path.

Override it per call:

```powershell
BP-Analyzer.exe export --process="MyProcess" --automate-c="C:\Path\To\AutomateC.exe"
```

Or put a `bp-analyzer.config.json` next to the executable (a ready-to-edit example
is shipped in `bp-analyzer.config.example.json` and included in the release ZIP):

```json
{
  "automateCPath": "C:\\Path\\To\\AutomateC.exe"
}
```

Precedence: `--automate-c` > `bp-analyzer.config.json` > built-in default.
The config is optional, including for `export` if Blue Prism is installed at the
default path. The executable directory is searched first, then the current
working directory. An unreadable/invalid config prints a warning and falls back
to the CLI override or built-in default.

### Release

The only workflow is `.github/workflows/release.yml`. It runs automatically on
pushes of version tags matching `v*`, not on branch pushes or pull requests.
A manual run is also available; select an existing version tag, not `main`,
because the binary version is derived from that tag.

Push a new version tag (for example `v1.0.3`) after committing and pushing the
intended changes. The workflow builds a self-contained Windows x64 executable,
checks its size, packages the config, and creates a GitHub release with:

- `BP-Analyzer.exe` — the original publish output, without renaming
- `BP-Analyzer.zip` — `BP-Analyzer.exe` and `bp-analyzer.config.json`
- `checksums.txt` — SHA256 hashes for both the EXE and ZIP

The same files are also uploaded as workflow artifact `BP-Analyzer`.
The target remains Windows x64 despite the neutral filenames. Releases up to
`v1.0.2` retain their historical `-win-x64` filenames.

```bash
git tag -a v1.0.3 -m "BP-Analyzer v1.0.3"
git push origin v1.0.3
```

The release notes are kept in `RELEASE_NOTES.md` in the repository and should
be reviewed before creating a tag. The `<sha256>` placeholder in
that file is replaced with the checksum of the built binary.


## CLI Usage

The CLI supports three main modes:

- **export**   – Exports Blue Prism processes as XML
- **codegen**  – Generates VB.NET code from Blue Prism XML
- **analyze**  – Analyzes Blue Prism processes

If started without parameters, an interactive menu is shown. Missing parameters
for the selected mode are prompted interactively. `export` without options also
prompts; `codegen` and `analyze` prompt for each missing directory. In scripts,
always supply both input and output directories to avoid waiting for input.
Named options use `--name=value`; positional arguments are also accepted.

---

### Export Mode
Exports a Blue Prism process as an XML file.

```
dotnet run --project BP-Analyzer.csproj -- export --process=<name> [--output=<path>] [--user=<username>] [--password=<password>] [--overwrite=<yes|no>]
```

- `--process=<name>`        Name of the process to export (**required**)
- `--output=<path>`         Output directory (default: xml/)
- `--user=<username>`       Blue Prism username
- `--password=<password>`   Blue Prism password (required with `--user`; without `--user` SSO is used)
- `--overwrite=<yes|no>`    Overwrite existing files (default: yes)
- `--automate-c=<path>`     Override the path to Blue Prism's `AutomateC.exe`

**Example:**
```
dotnet run --project BP-Analyzer.csproj -- export --process="MyProcess" --output="C:\Exports"
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

- `--code=<json-dir>`   Directory with classes.json, dependencies.json, exceptions.json (default: code/)
- `--output=<sdd-dir>`  Output directory for SDD files (default: sdd/)
- `--exclude-exception-sources=<prefix1,prefix2,...>`  Comma-separated list of exception source prefixes to exclude from SDD exception listing (default: ERGO_,Utility_)


**Examples:**
```
# Standard (default exclusions: ERGO_, Utility_)
dotnet run --project BP-Analyzer.csproj -- analyze --code=code --output=sdd

# Mit eigenen Prefixen (z.B. nur Utility und TEST ausschließen)
dotnet run --project BP-Analyzer.csproj -- analyze --code=code --output=sdd --exclude-exception-sources=Utility_,TEST_
```




### Notes
- Internal BluePrism objects (e.g., `Blueprism.Automate*`) are automatically skipped during export.
- There are no short forms or backwards compatibility with old calls.
- General help: `BP-Analyzer.exe help` (also `--help` or `-h`).
- Known limitation: mode-specific `--help` is not handled consistently. `export --help`
  reports the missing process with usage and exit code 1; `codegen --help` and
  `analyze --help` can prompt and execute instead. Use the option reference above.

## Compiling Output

The generated VB.NET files can be compiled using MSBuild or dotnet.

### Using dotnet MSBuild

```bash
# Change to the generated code directory
cd code

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
code/bin/Debug/net48/BluePrism_Generated.dll
# or
code/bin/Release/net48/BluePrism_Generated.dll
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
Utility___Environment.Instance.Start_Process(Application:=FilePath)
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
├── code/                          # Generated VB.NET files + JSON + vbproj
│   ├── BP_Base.vb                # Base class template
│   ├── Test_Process.vb
│   ├── Microsoft_Store.vb
│   ├── classes.json
│   ├── dependencies.json
│   ├── exceptions.json
│   └── BluePrism_Generated.vbproj
├── sdd/                           # Generated SDD documents
│   └── SDD Template Test Process.md
├── templates/                     # VB.NET templates
│   ├── Template_BP_Base.vb        # Base class template
│   ├── Template_BP_Project.vbproj # VB project template
│   └── SDD_Process_Template.md    # SDD template
├── CodeGen/                       # Code generator modules
│   ├── ClassGenerator.cs
│   ├── CodeGenRegistry.cs
│   ├── DataItemGenerator.cs
│   ├── MethodGenerator.cs
│   ├── TemplateManager.cs
│   ├── TemplateProvider.cs        # Disk override / embedded template lookup
│   ├── CodeGenJsonContext.cs      # Trim-safe JSON serialization
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
├── BluePrismAnalyzer.cs          # SDD generation from JSON
├── CliExportHandler.cs           # export mode
├── CliCodeGenHandler.cs          # codegen mode
├── CliAnalyzeHandler.cs          # analyze mode
├── Program.cs                    # Entry point
├── AnalyzerConfig.cs             # Optional export configuration
├── bp-analyzer.config.example.json # Config included in release ZIP
├── .github/workflows/release.yml  # Tag-triggered releases
├── Properties/PublishProfiles/   # Windows x64 single-file profile
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
| WaitStart          | Wait stage          | ✓         | Dummy implementation; WaitEnd is structural |
| Process           | Subprocess call     | ✓         |                                     |
| SubSheet          | Subsheet call       | ✓         |                                     |
| Alert             | Alert stage         | ✓         |                                     |
| ChoiceStart       | Choice branch       | ✓         |                                     |
| MultipleCalculation | Multi assignment   | ✓         |                                     |
| Anchor            | Jump marker         | ✓         | Will be skipped in output           |
| Block             | Block container     | ✓         | Only used in exception handling     |
| LoopStart/LoopEnd | Loops               | ✓         | Uses goto statement                 |

## License

MIT License
