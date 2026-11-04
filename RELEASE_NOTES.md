# BP-Analyzer

Converts BluePrism XML exports into VB.NET classes with GoTo-based process flow,
and generates Solution Design Documents (SDD) as Markdown.

## Download

| Asset | Size | Notes |
| --- | --- | --- |
| `BP-Analyzer-win-x64.exe` | ~12.6 MB | self-contained, single file, trimmed |
| `checksums.txt` | 91 B | SHA256 of the binary |
| `BP-Analyzer-win-x64.zip` | ~13 MB | the exe plus the example config file |

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
The ZIP asset contains the executable plus a ready-to-edit example config.

### Verify the download

```
Get-FileHash BP-Analyzer-win-x64.exe -Algorithm SHA256
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

- The generated VB.NET project targets **.NET Framework 4.8** (net48).
- The generated code is syntactically valid and compiles, but cannot be executed as a
  real BluePrism process. UI automation, object calls and some stage types are
  placeholders or stubs.
- `export` requires Blue Prism's `AutomateC.exe`. The path is hard-coded to
  `C:\Program Files\Blue Prism Limited\Blue Prism Automate\AutomateC.exe`.
