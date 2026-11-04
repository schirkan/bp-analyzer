# BP-Analyzer

Converts BluePrism XML exports into VB.NET classes with GoTo-based process flow,
and generates Solution Design Documents (SDD) as Markdown.

## Changes in v1.0.3

- Keep the published executable name `BP-Analyzer.exe` and ship `BP-Analyzer.zip`.
- Release builds run on version-tag pushes, not on branch pushes or pull requests.
- Refresh the documentation for configuration, embedded templates and CLI limitations.

## Download

Target platform: **Windows x64**. Neutral filenames do not imply other platforms.

| Asset | Size | Notes |
| --- | --- | --- |
| `BP-Analyzer.exe` | ~12.6 MB | self-contained, single file, trimmed |
| `checksums.txt` | a few hundred bytes | SHA256 of the binary and the ZIP |
| `BP-Analyzer.zip` | ~7.6 MB | the exe plus the example config file |

No .NET runtime required on the target machine. The templates are embedded in the
binary, so `codegen` and `analyze` work without any further files.

### Configuration (optional)

Only the `export` mode needs a path to Blue Prism's `AutomateC.exe`. The default
points at the standard installation path; override it if Blue Prism lives
elsewhere:

```
BP-Analyzer.exe export --process="MyProcess" --automate-c="C:\Path\To\AutomateC.exe"
```

Or put `bp-analyzer.config.json` next to the executable:

```json
{
  "automateCPath": "C:\\Path\\To\\AutomateC.exe"
}
```

Precedence: `--automate-c` > `bp-analyzer.config.json` > built-in default.
The config is optional even for `export` when the default installation path is correct.
The executable directory is searched first, then the current working directory.
The ZIP asset contains `BP-Analyzer.exe` plus a ready-to-edit `bp-analyzer.config.json`.

### Verify the download

```
Get-FileHash BP-Analyzer.exe -Algorithm SHA256
```

Expected:

```
<sha256>
```

## Usage

```
BP-Analyzer.exe help

BP-Analyzer.exe codegen --xml=xml --output=code
BP-Analyzer.exe analyze --code=code --output=sdd
BP-Analyzer.exe export --process="MyProcess" --output="C:\Exports" --user=admin --password=***
```

Running `BP-Analyzer.exe` without arguments opens an interactive menu
(1) Export, 2) Codegen, 3) Analyze.

## Build from source

```
dotnet publish BP-Analyzer.csproj -p:PublishProfile=BP-Analyzer-win-x64 -o publish
```

Profile settings: self-contained, single-file, trimmed (`TrimMode=partial`),
compressed, embedded debug symbols.

## Notes

- The analyzer targets **.NET 8**; building from source requires a compatible SDK.
- The generated VB.NET project targets **.NET Framework 4.8** (net48).
- Use `--name=value` for named options; in scripts supply input and output directories
  explicitly to avoid interactive prompts.
- General help works with `BP-Analyzer.exe help`. Mode-specific `--help` is not
  handled consistently and can prompt/execute rather than only show help.
- The publish log may contain Handlebars warning `IL2104`. Existing smoke tests passed,
  but that does not guarantee every possible template is trim-safe.
- The generated code is syntactically valid and compiles, but cannot be executed as a
  real BluePrism process. UI automation, object calls and some stage types are
  placeholders or stubs.
- `export` requires Blue Prism's `AutomateC.exe`. The default path is
  `C:\Program Files\Blue Prism Limited\Blue Prism Automate\AutomateC.exe`,
  but it can be overridden via `--automate-c` or `bp-analyzer.config.json`.
