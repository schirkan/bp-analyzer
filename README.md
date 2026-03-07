# BP-Analyzer

BluePrism XML to VB.NET Code Generator

Dieses Tool konvertiert BluePrism XML-Exportdateien in VB.NET-Klassen mit GoTo-basiertem Prozessfluss.

## Inhaltsverzeichnis

- [BP-Analyzer](#bp-analyzer)
  - [Inhaltsverzeichnis](#inhaltsverzeichnis)
  - [Installation](#installation)
  - [CLI-Aufrufe](#cli-aufrufe)
    - [Code-Generierung](#code-generierung)
      - [Grundlegende Aufrufe](#grundlegende-aufrufe)
      - [Alle CLI-Varianten](#alle-cli-varianten)
    - [Prozess-Export](#prozess-export)
      - [Alle Export-CLI-Varianten](#alle-export-cli-varianten)
  - [Ausgabe kompilieren](#ausgabe-kompilieren)
    - [Mit dotnet MSBuild](#mit-dotnet-msbuild)
    - [Mit MSBuild (Visual Studio)](#mit-msbuild-visual-studio)
    - [Kompilierte Ausgabe](#kompilierte-ausgabe)
  - [Generierter Code](#generierter-code)
    - [Singleton Pattern](#singleton-pattern)
    - [Exception Handling](#exception-handling)
    - [Prozessfluss](#prozessfluss)
  - [Dateistruktur](#dateistruktur)
  - [Unterstützte BluePrism-Stages](#unterstützte-blueprism-stages)
  - [Lizenz](#lizenz)

## Installation

```bash
# Projekt bauen
dotnet build BP-Analyzer.csproj

# Oder direkt ausführen
dotnet run --project BP-Analyzer.csproj
```

## CLI-Aufrufe

### Code-Generierung

Konvertiert BluePrism XML-Dateien zu VB.NET-Klassen.

#### Grundlegende Aufrufe

```bash
# Standard (verwendet "xml" und "output" Verzeichnisse)
dotnet run --project BP-Analyzer.csproj -- --codegen

# Mit expliziten Verzeichnissen
dotnet run --project BP-Analyzer.csproj -- --codegen --xml=xml --output=output

# Kurze Syntax
dotnet run --project BP-Analyzer.csproj -- --codegen xml output

# Einzelnes Verzeichnis (nur xml, output = "output")
dotnet run --project BP-Analyzer.csproj -- --codegen xml
```

#### Alle CLI-Varianten

| Befehl                                             | Beschreibung                               |
| -------------------------------------------------- | ------------------------------------------ |
| `dotnet run -- --codegen`                          | Standard: `xml/` → `output/`               |
| `dotnet run -- --codegen --xml=meinverzeichnis`    | Eigenes XML-Verzeichnis                    |
| `dotnet run -- --codegen --output=meinverzeichnis` | Eigenes Ausgabeverzeichnis                 |
| `dotnet run -- --codegen --xml=xml --output=out`   | Beide Verzeichnisse                        |
| `dotnet run -- --codegen xml`                      | XML-Verzeichnis als positionaler Parameter |
| `dotnet run -- --codegen xml out`                  | Beide als positionale Parameter            |

### Prozess-Export

Exportiert BluePrism-Prozesse direkt aus der Runtime Resource (erfordert AutomateC.exe).

**Standard-Ausgabeverzeichnis:** `xml/`
**Standard-Überschreiben:** `yes` (Dateien werden automatisch überschrieben)

```bash
# Grundlegender Export (nach xml/ mit Überschreiben)
dotnet run --project BP-Analyzer.csproj -- --process="MeinProzess"

# Mit Ausgabeverzeichnis
dotnet run --project BP-Analyzer.csproj -- --process="MeinProzess" --output="C:\Exporte"

# Mit Anmeldedaten
dotnet run --project BP-Analyzer.csproj -- --process="MeinProzess" --user=admin --password=geheim

# Überschreiben deaktivieren (falls benötigt)
dotnet run --project BP-Analyzer.csproj -- --process="MeinProzess" --overwrite=no

# Alles zusammen
dotnet run --project BP-Analyzer.csproj -- --process="MeinProzess" --output="C:\Exporte" --user=admin --password=geheim --overwrite=no
```

**Hinweis:** Interne BluePrism-Objekte (z.B. `Blueprism.AutomateProcessCore.*`) werden automatisch übersprungen.

## Ausgabe kompilieren

Die generierten VB.NET-Dateien können mit MSBuild oder dotnet kompiliert werden.

### Mit dotnet MSBuild

```bash
# Ins Ausgabeverzeichnis wechseln
cd output

# Projekt kompilieren
dotnet build BluePrism_Generated.vbproj
```

### Mit MSBuild (Visual Studio)

```powershell
# Kompilieren
msbuild BluePrism_Generated.vbproj

# Kompilieren mit Debug-Konfiguration
msbuild BluePrism_Generated.vbproj /p:Configuration=Debug

# Kompilieren mit Release-Konfiguration
msbuild BluePrism_Generated.vbproj /p:Configuration=Release
```

### Kompilierte Ausgabe

Das kompilierte Projekt erstellt eine DLL-Datei:

```
output/bin/Debug/BluePrism_Generated.dll
# oder
output/bin/Release/BluePrism_Generated.dll
```

## Generierter Code

### Singleton Pattern

Jede generierte Klasse verwendet das Lazy Initialization Singleton Pattern:

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

    ' ... weitere Klassenmember
End Class
```

**Verwendung:**
```vb
' Statt: _utility.Start_Process(...)
' Nun:
Utility___Environment.Instance.Start_Process(Application:=FilePath)
```

### Exception Handling

Das BP_Base-Template stellt Exception-Handling-Methoden bereit:

```vb
' Exception auslösen
RaiseException("System Exception", "Fehlermeldung")

' Exception speichern (in Recover-Stage)
StoreException()

' Gespeicherte Exception erneut auslösen
RethrowException()

' Exception-Informationen abrufen
Dim exType As String = ExceptionType()
Dim exText As String = ExceptionText()
```

### Prozessfluss

Der Prozessfluss wird mit GoTo-Labels und Bedingungen rekonstruiert:

```vb
Public Sub Main()
    ' Start
    GoTo Start_Main_Label

Start_Main_Label: ' Hauptprozess
    ' Entscheidung
    If Bedingung = True Then
        GoTo True_Path_Label
    Else
        GoTo False_Path_Label
    End If

True_Path_Label: ' True-Pfad
    ' ... Aktionen
    GoTo End_Label

False_Path_Label: ' False-Pfad
    ' ... Aktionen
    GoTo End_Label

End_Label:
    ' Ausgabe-Parameter zuweisen
End Sub
```

## Dateistruktur

```
bp-analyzer/
├── xml/                    # BluePrism XML-Exporte
│   ├── Process1.xml
│   ├── Process2.xml
│   └── ...
├── output/                 # Generierte VB.NET-Dateien
│   ├── _BP_Base.vb        # Basisklasse
│   ├── Process1.vb
│   ├── Process2.vb
│   └── BluePrism_Generated.vbproj
├── Template_BP_Base.vb    # Vorlage für Basisklasse
├── Template_BP_Project.vbproj  # Vorlage für Projektdatei
├── BluePrismCodeGen.cs    # Code-Generator
├── ExporterCLI.cs         # BluePrism CLI-Export
├── Program.cs             # Haupteinstiegspunkt
└── BP-Analyzer.csproj     # Projektdatei
```

## Unterstützte BluePrism-Stages

| Stage-Typ         | Unterstützt     | Beschreibung         |
| ----------------- | --------------- | -------------------- |
| Start             | ✓               | Methodeneingang      |
| End               | ✓               | Methodenausgang      |
| Action            | ✓               | Objekt-Aufrufe       |
| Decision          | ✓               | If/Else-Verzweigung  |
| Calculation       | ✓               | Variablenzuweisung   |
| Code              | ✓ (kommentiert) | Code-Stages          |
| Note              | ✓               | Kommentare           |
| Exception         | ✓               | Exception auslösen   |
| Recover           | ✓               | Exception speichern  |
| Resume            | ✓               | Exception fortsetzen |
| Navigate          | ✓ (kommentiert) | UI-Automatisierung   |
| Process           | ✓ (kommentiert) | Unterprozess-Aufruf  |
| Anchor            | ✓               | Sprungmarke          |
| Block             | ✓               | Block-Container      |
| WaitStart/WaitEnd | ✓ (kommentiert) | Warte-Stages         |

## Lizenz

MIT License
