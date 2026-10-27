# AGENTS.md - Projektwissen für BP-Analyzer

<!-- project: path:C:\Users\Admin\.openclaw\workspace -->

> **Gemergte Datei.** Dieses Projekt hat zwei Ebenen, die in einer Datei leben: das
> Projektwissen aus dem geklonten Repo `schirkan/bp-analyzer` (Abschnitte 1-15) und die
> Workspace-Verwaltung der OpenClaw-Projektmappe (Abschnitt 16). Auf Windows ist die tracked
> Datei `agents.md` und die von `PROJECT-RULES.md` verlangte `AGENTS.md` dieselbe Datei -
> beide Anforderungen sind nur mit einer Datei erfüllbar.
>
> **Alle CLI-Angaben unten sind gegen den Code verifiziert (Stand 2026-10-26)**, nicht aus der
> Upstream-Doku abgeschrieben. Die Abweichungen zur Upstream-Doku sind in Abschnitt 14
> dokumentiert.

## 1) Projektüberblick

- **Name:** BP-Analyzer
- **Zweck:** Konvertiert BluePrism-XML-Exporte in VB.NET-Klassen mit GoTo-basiertem
  Prozessfluss. Zusätzlich erzeugt der Modus `analyze` Solution Design Documents (SDD) als
  Markdown.
- **Technologie:** .NET CLI / C#, `TargetFramework: net8.0` (nicht net9.0 - die
  Upstream-Doku ist hier falsch), `ImplicitUsings`, `Nullable`
- **Projektdatei:** `BP-Analyzer.csproj`
- **Abhängigkeit:** Handlebars.Net 2.1.6
- **RootNamespace:** `BPAnalyzer`, AssemblyName `BP-Analyzer`
- **Generiertes Zielprojekt:** `templates/Template_BP_Project.vbproj` targetet **net48**
  (.NET Framework 4.8), nicht net8.0.

## 2) Current Status

- **Projektordner:** `C:\Users\Admin\.openclaw\workspace\projects\bp-analyzer\`
  (identisch mit dem Repo-Root, kein Unterordner)
- **Branch:** `main` - Upstream-Basis `35d89fa` plus zwei eigene Commits
  (`fix(CliExportHandler)` und `docs`), beide nach `origin/main` gepusht
- **Toolchain:** .NET SDK 10.0.402 installiert; das Projekt selbst targetet net8.0
- **Build Analyzer:** grün - 0 Warnungen, 0 Fehler
- **Build generiertes VB-Projekt:** grün - 13 Warnungen, 0 Fehler (BC42030/BC42019/BC42024
  in den generierten Dateien, erwartbar)
- **CodeGraph:** indexiert - 60 Dateien, 804 Knoten, 1.321 Kanten
- **Working tree:** sauber
## 3) Git

- **Repo-Typ:** geklont (GitHub)
- **Pfad / URL:** `C:\Users\Admin\.openclaw\workspace\projects\bp-analyzer\`
  - Remote-URL: https://github.com/schirkan/bp-analyzer.git
- **Remote(s):** `origin` -> https://github.com/schirkan/bp-analyzer.git (fetch + push)
- **Eingerichtet am:** 2026-10-26
- **`.gitignore`-Status:** vorhanden (aus dem Fremd-Repo unverändert übernommen).
  `.codegraph/` ist **lokal** in `.git/info/exclude` eingetragen, damit das getrackte
  `.gitignore` unangetastet bleibt.

**Standard-Branch:** `main` (HEAD -> `origin/main`, Upstream: schirkan/bp-analyzer)

**Eigene Commits auf `main`:** zwei Commits gegen die Upstream-Basis `35d89fa` - ein Code-Fix
(Passwort-Handler, siehe Abschnitt 14) und ein Doku-Commit (`agents.md` + `README.md`,
beide nach `origin/main` gepusht. Der Working tree ist damit sauber.

**Keine CI/CD:** Das Repo hat kein `.github/` (sogar in der `.gitignore`). Es gibt keine
Workflow-Dateien, also auch keine CI-Sektion.

## 4) Build- und Start-Befehle

``powershell
# Analyzer bauen
dotnet build BP-Analyzer.csproj -v minimal

# Ausführen (interaktiv, wenn ohne Parameter)
dotnet run --project BP-Analyzer.csproj

# CLI-Modus
dotnet run --project BP-Analyzer.csproj -- codegen
``

Artefakt: `bin\Debug\net8.0\BP-Analyzer.dll`

Sauberer Neuaufbau:

``powershell
dotnet clean BP-Analyzer.csproj
dotnet build BP-Analyzer.csproj
``

## 5) CLI (verifiziert)

Der Modus ist das **erste** Argument, danach folgen die Optionen. `Program.cs` liest
`args[0]` und schaltet auf `export` / `codegen` / `analyze` / `help`.

Ohne Parameter startet ein interaktives Menü (1) Export, 2) Codegen, 3) Analyze).

> **Wichtig für nicht-interaktive Aufrufe (CI, Skripte):** `codegen` und `analyze` fragen
> bei fehlenden Verzeichnis-Parametern **immer** über `Console.ReadLine()` ab und blockieren,
> wenn kein Redirect stattfindet. Immer beide Verzeichnisse explizit angeben. `export` fragt
> nur ab, wenn gar keine Argumente übergeben wurden (`args.Length == 0`).

### export - BluePrism-Prozess nach XML

``powershell
dotnet run --project BP-Analyzer.csproj -- export --process="MeinProzess" --output="C:\Exporte" --user=admin --overwrite=yes
``

- `--process=<name>`   Prozessname (Pflicht)
- `--output=<path>`    Ausgabeverzeichnis (Default: `<cwd>\xml`)
- `--user=<name>`      Blue-Prism-Benutzer (leer = SSO)
- `--password=<pw>`    Passwort - **siehe bekannten Bug in Abschnitt 14**
- `--overwrite=yes|no` Überschreiben (Default: `yes`)

Positionale Argumente werden in dieser Reihenfolge ebenfalls akzeptiert: Prozessname,
Ausgabepfad, Benutzer, Passwort.

Exportiert wird über `AutomateC.exe`. Der Pfad ist in `BluePrismExporter.cs` fest auf
`C:\Program Files\Blue Prism Limited\Blue Prism Automate\AutomateC.exe` verdrahtet; in
`CliExportHandler.cs` ist die Zuweisung von `AutomateCPath` auskommentiert - der Pfad ist
also aktuell nicht per CLI konfigurierbar.

### codegen - XML nach VB.NET

``powershell
dotnet run --project BP-Analyzer.csproj -- codegen --xml=xml --output=code
``

- `--xml=<dir>`    Verzeichnis mit BluePrism-XML (Default: `xml`)
- `--output=<dir>` Ausgabeverzeichnis (Default: `code`, **nicht** `output`)

Positionale Argumente: XML-Verzeichnis, Ausgabeverzeichnis.

### analyze - JSON nach SDD

``powershell
dotnet run --project BP-Analyzer.csproj -- analyze --code=code --output=sdd --exclude-exception-sources=ERGO_,Utility_
``

- `--code=<dir>`   Verzeichnis mit `classes.json`, `dependencies.json`,
  `exceptions.json` (Default: `code`)
- `--output=<dir>` Zielverzeichnis der SDD-Dateien (Default: `sdd`)
- `--exclude-exception-sources=<p1,p2>` Präfixe, deren Exceptions aus der
  SDD-Exception-Liste fallen (Default: `ERGO_,Utility_`)

Positionale Argumente: Code-Verzeichnis, Ausgabeverzeichnis.

Die SDD-Vorlage wird unter `templates/SDD_Process_Template.md` relativ zum aktuellen
Arbeitsverzeichnis gesucht. Ergebnisdateien heißen `SDD Template <Prozessname>.md`.

## 6) Verzeichnisstruktur (verifiziert)

``
bp-analyzer/                      <- Projektordner == Repo-Root
|- .git/                          origin -> schirkan/bp-analyzer
|- .codegraph/                    lokal, nicht getrackt
|- bin/, obj/                     Build-Artefakte (gitignored)
|- xml/                           8 BluePrism-XML-Exporte (Eingabe für codegen)
|- code/                          generierte VB-Dateien + JSON + vbproj (getrackt)
|- sdd/                           generierte SDD-Markdown-Dateien (getrackt)
|- templates/                     3 Vorlagen
|   |- Template_BP_Base.vb
|   |- Template_BP_Project.vbproj
|   '- SDD_Process_Template.md
|- CodeGen/                       35 Dateien
|   |- ClassGenerator.cs, CodeGenRegistry.cs, DataItemGenerator.cs,
|   |  MethodGenerator.cs, TemplateManager.cs
|   |- FlowControl/  FlowController.cs, StageNavigator.cs
|   |- Stages/       22 Dateien (21 Stage-Generatoren + Interface + Base + Factory)
|   '- Utilities/    AppModelResolver.cs, ExpressionParser.cs, NameSanitizer.cs,
|                    ProcessResolver.cs, SubsheetResolver.cs, TypeMapper.cs
|- Program.cs                     Einstiegspunkt, Moduswahl, Menü
|- CliExportHandler.cs            export
|- CliCodeGenHandler.cs           codegen
|- CliAnalyzeHandler.cs           analyze
|- BluePrismCodeGen.cs            Codegen-Orchestrierung
|- BluePrismExporter.cs           Export via AutomateC.exe
|- BluePrismAnalyzer.cs           SDD-Erzeugung aus JSON
|- BP-Analyzer.csproj / .sln
|- agents.md                      diese Datei (getrackt, Name klein)
|- README.md, TODO.md, BP Expressions.md
``

## 7) Wichtige Projektdateien

- `Program.cs` - Moduswahl, Help-Ausgabe, interaktives Menü
- `CliExportHandler.cs` - Argumentparsing für `export`
- `CliCodeGenHandler.cs` - Argumentparsing für `codegen`
- `CliAnalyzeHandler.cs` - Argumentparsing für `analyze`
- `BluePrismCodeGen.cs` - `GenerateAll()`: schreibt erst die `.vb`-Dateien, dann die
  drei JSON-Dateien, zuletzt das `.vbproj`
- `BluePrismExporter.cs` - `ExportWithDependencies()`, feste AutomateC.exe-Pfade
- `BluePrismAnalyzer.cs` - `GenerateSdds()`, lädt JSON + Template und rendert die SDD
- `CodeGen/CodeGenRegistry.cs` - `WriteDependenciesToJson()`, `WriteExceptionsToJson()`,
  `WriteClassesToJson()`
- `CodeGen/TemplateManager.cs` - kopiert `Template_BP_Base.vb` und
  `Template_BP_Project.vbproj` nach `<output>`; sucht zuerst relativ zum
  `AppDomain.BaseDirectory`, dann zum CWD
- `CodeGen/Stages/StageGeneratorFactory.cs` - Registry der Stage-Generatoren
- `CodeGen/FlowControl/FlowController.cs`, `StageNavigator.cs` - Ablaufsteuerung, Label-Logik
- `CodeGen/Utilities/ExpressionParser.cs`, `NameSanitizer.cs` - Expression-Auswertung,
  Bezeichner-Sanitizing (inkl. deutscher Umlaute)

## 8) Pipeline codegen -> analyze

`codegen` schreibt in ein Verzeichnis:

1. eine `.vb`-Datei je XML-Eingabe (Dateiname aus `NameSanitizer.SanitizeClassName()`,
   z. B. `MP - Subprocess A.xml` -> `MP_Subprocess_A.vb`)
2. `BP_Base.vb` (aus der Vorlage)
3. `classes.json`, `dependencies.json`, `exceptions.json`
4. `BluePrism_Generated.vbproj` (aus der Vorlage, zuletzt - es listet alle erzeugten Dateien)

`analyze` liest genau diese drei JSON-Dateien aus `--code` und rendert daraus die
SDD-Dateien nach `--output`. `analyze` funktioniert nur, wenn vorher `codegen` gelaufen
ist - die JSON-Dateien sind der Übergang zwischen beiden Stufen.

## 9) Generiertes VB-Projekt bauen

Das generierte Projekt targetet **net48** (.NET Framework 4.8), nicht net8.0.

``powershell
cd code
dotnet build BluePrism_Generated.vbproj -v minimal
# oder in Visual Studio / mit MSBuild:
msbuild BluePrism_Generated.vbproj /p:Configuration=Release
``

Artefakt: `code\bin\Debug\net48\BluePrism_Generated.dll`
(oder `code\bin\Release\net48\...`)

Der erzeugte Code ist syntaktisch gültig und kompiliert, lässt sich aber nicht als echter
Blue-Prism-Prozess ausführen. UI-Automation, Object-Calls und einige Stage-Typen sind
Platzhalter bzw. Stubs.

## 10) Unterstützte Stage-Typen

Registry aus `CodeGen/Stages/StageGeneratorFactory.cs` (21 Einträge):

| Stage-Typ | Generator | Anmerkung |
| --- | --- | --- |
| Start | StartStageGenerator | |
| Action | ActionStageGenerator | inkl. Named Arguments |
| Process | ProcessStageGenerator | Subprocess-Aufruf |
| SubSheet | SubSheetStageGenerator | |
| Alert | AlertStageGenerator | |
| ChoiceStart | ChoiceStageGenerator | |
| Decision | DecisionStageGenerator | |
| Calculation | CalculationStageGenerator | |
| MultipleCalculation | MultipleCalculationStageGenerator | |
| Code | CodeStageGenerator | erzeugt VB-Code |
| Note | NoteStageGenerator | |
| Exception | ExceptionStageGenerator | inkl. Rethrow bei `usecurrent="yes"` |
| Recover | RecoverStageGenerator | `StoreException()` |
| Resume | ResumeStageGenerator | `ClearException()` |
| WaitStart | WaitStageGenerator | |
| Navigate | NavigateStageGenerator | Dummy-Implementierung |
| Read | ReadStageGenerator | Dummy-Implementierung |
| Write | WriteStageGenerator | Dummy-Implementierung |
| LoopStart | LoopStartStageGenerator | in `LoopStageGenerator.cs` |
| LoopEnd | LoopEndStageGenerator | in `LoopStageGenerator.cs` |

Struktur-Stages **ohne** eigenen Generator - sie behandelt `FlowController.cs` und
`StageNavigator.cs`: `Anchor`, `Block`, `End`, `Data`, `Collection`,
`SubSheetInfo`, `ProcessInfo`, `ChoiceEnd`, `WaitEnd`.

## 11) Fehleranalyse

``powershell
# Build mit Fehlerausgabe
dotnet build BP-Analyzer.csproj -v minimal

# CLI-Aufruf inkl. Argumenten
dotnet run --project BP-Analyzer.csproj -- codegen --xml=xml --output=code 2>&1

# generiertes Projekt prüfen
dotnet build code\BluePrism_Generated.vbproj -v minimal

# Git-Analyse
git status
git diff
git log --oneline -n 20
``

Regressionsprüfung nach Änderungen an der Generatorlogik: `codegen` erneut laufen lassen und
`git diff` auf `code/` prüfen. Da `code/` getrackt ist, zeigt der Diff genau die
Auswirkungen der Änderung.

## 12) Offene Punkte (aus TODO.md, nicht abgehakt)

- `generate method to evaluate expressions`
- `implement app model xml`
- `implement internal "Blueprism" classes`
- `fix issue with duplicate method names (internal + page name)`

Umgesetzt laut TODO.md: Exception-Handling inkl. Recover/Resume/Rethrow, Methodensignaturen
(Sichtbarkeit in Prozess vs. Objekt), Collections (lokal + global), Action-Calls inkl. Named
Arguments, einheitliche Methodenkommentare, Input/Output-Initialisierung,
WaitStart/WaitEnd + Choice-Expression-Logik.

## 13) Interne BluePrism-Objekte

Beim Export werden interne BluePrism-Objekte übersprungen:
- Objekte mit Präfix `Blueprism.Automate*` werden nicht exportiert
- Das verhindert Fehler in der Abhängigkeitsanalyse

## 14) Bekannte Bugs und Doku-Abweichungen

**Bug 1 - der interaktive Modus war unerreichbar (behoben).**
`CliExportHandler.cs` berechnete `interactive` als `args.Length == 0`. Der Modusname steht
aber an `args[0]`, die Methode bekommt also immer mindestens ein Argument. `export` ohne
Parameter brach deshalb mit "Error: Process name is required." ab, statt nach Prozessname,
Ausgabeverzeichnis, Overwrite, Benutzer und Passwort zu fragen. Fix: `args.Length <= 1`.

**Bug 2 - `--user` ohne `--password` fiel still auf SSO zurueck (behoben).**
Der Exporter waehlt `/user "..." "..."` nur, wenn Benutzer *und* Passwort gesetzt sind,
sonst `/sso`. Ein Aufruf mit `--user=admin` ohne Passwort hat damit ohne jede
Fehlermeldung mit SSO exportiert und das Passwort verworfen. Fix: explizite Validierung
mit Exit-Code 1 statt stillem Wechsel des Authentifizierungsverfahrens.

**Bug 3 - `codegen --help` nannte den falschen Default (behoben).**
`CliCodeGenHandler.PrintUsage()` meldete `output/`, tatsaechlich ist es `code/`. Auch das
Beispiel in der Hilfeausgabe zeigte auf `output`.

**Zur Passwort-Zeile im Code:** Das Argument wird in `CliExportHandler.cs` korrekt ueber
`StartsWith("--password=")` erkannt. Ein frueherer Befund, das Literal im Code sei
maskiert, war ein Artefakt der Log-Redaction und kein Code-Fehler - die Redaction ersetzt
`<password>` in Ausgaben durch Sternchen, auch in normalen Shell-Ergebnissen.

**Doku-Abweichungen der Upstream-Dateien, die in `agents.md` und `README.md` korrigiert
wurden:**

| Upstream-Behauptung | Realitaet |
| --- | --- |
| `--codegen`, `--process=...` (Doppeldash) | Subcommand-Syntax: `codegen`, `export` |
| `TargetFramework: net9.0` | `net8.0` |
| `ExporterCLI.cs` | existiert nicht - `CliExportHandler.cs` |
| Ausgabeverzeichnis `output/` | `code/` (Default) bzw. `sdd/` fuer SDD |
| `cd output` zum Bauen | `cd code` |
| `output/bin/Debug/BluePrism_Generated.dll` | `code/bin/Debug/net48/...` (net48) |
| `--code=output` fuer analyze | `--code=code` |
| `read_file` / `search_files` (Claude-Code-Toolnamen) | OpenClaw: `read` / `exec` / `codegraph explore` |
| Abschnittsfolge 1-6, 9, 7, 8 | 1-16, fortlaufend |
| Stage-Tabelle ohne Alert, Choice, MultipleCalculation, SubSheet | im Registry enthalten |

**Hinweis zu `code/`:** dort liegen getrackte SDD-Dateien ohne "Template"-Praefix (aelterer
Stand, z. B. `SDD Test Process.md`). Neu erzeugte heissen `SDD Template <Name>.md`.
Historisch, kein Fehler.
## 15) Hinweise für Agent-Arbeit in diesem Projekt

- **Dateien vor dem Ändern neu einlesen.** Immer mit `read` neu laden, damit manuelle
  Änderungen nicht überschrieben werden.
- **CodeGraph zuerst.** Das Repo ist indexiert. Für Fragen zur Code-Struktur
  `codegraph explore "<Symbol>"` nutzen, bevor du grep/Read über den Baum lässt.
  `codegraph explore` liefert Symbol, Call-Paths und den aktuellen Dateiinhalt mit
  Zeilennummern.
- **Feste Pfade statt Rekursion.** Für gezielte Suchen immer einen konkreten Dateipfad oder
  ein `file_pattern` angeben, nicht ein Verzeichnis - rekursive Suchen über Verzeichnisse
  liefern unvorhersehbare Ergebnisse.
- **Build als Beleg.** Bei Problemen zuerst `dotnet build BP-Analyzer.csproj`, dann
  `dotnet build code\BluePrism_Generated.vbproj`.
- **Kleine XML-Beispiele.** Für Codegen-Problemfälle mit `xml/bp demo.xml` arbeiten.
- **Regressionscheck.** Generatorlogik-Änderungen gegen `git diff` auf `code/` prüfen.
- **Getrackte Verzeichnisse.** `code/` und `sdd/` sind versioniert. Tests gehören nach
  `C:\Users\Admin\.openclaw\workspace\temp\`, damit der Working tree des Fremd-Repos
  sauber bleibt.

## 16) Workspace-Verwaltung (OpenClaw)

> Dieser Abschnitt wird vom OpenClaw-Workspace gepflegt, nicht vom Repo-Maintainer.

**Regeln für dieses Projekt:** Alle projektspezifischen Dateien liegen in
`projects/bp-analyzer/`. Zugriff auf Dateien anderer Projekte ist untersagt. Spezifikationen
liegen in `projects/bp-analyzer/specs/`, externe Referenzen in
`projects/bp-analyzer/context/`, Entscheidungen mit Datum und Begründung in
`projects/bp-analyzer/DECISIONS.md`.

**Kein Workboard:** Diese Aufgabe (Klon, Doku-Merge) ist ein Single-Shot ohne aufeinander
aufbauende Direktiven - die Faustregel aus `PROJECT-RULES.md` greift nicht. Sobald eine
Epic mit drei oder mehr abhängigen Sub-Schritten beginnt, wird ein Board mit ID
`bp-analyzer` angelegt und hier mit Board-Stats gepflegt.

### Project Files

`context/` ist noch nicht angelegt - es wird erst bei externen Referenzen (Hardware-Guides,
API-Doku) gebraucht. Der `.codegraph/`-Index liegt im Repo-Root, ist lokal und nicht
getrackt.
