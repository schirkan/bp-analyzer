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
- **Branch:** `main` - Upstream-Basis `35d89fa` plus eigene Commits, alle nach
  `origin/main` gepusht
- **Toolchain:** .NET SDK 10.0.402 installiert; das Projekt selbst targetet net8.0
- **Build Analyzer:** grün - 0 Warnungen, 0 Fehler
- **Build generiertes VB-Projekt:** grün - 13 Warnungen, 0 Fehler (BC42030/BC42019/BC42024
  in den generierten Dateien, erwartbar)
- **Single-EXE-Publish:** verfügbar - Publish-Profil `BP-Analyzer-win-x64`, ca. 12,6 MB
  (self-contained, single-file, trimmed, komprimiert). Details in Abschnitt 17.
- **CodeGraph:** indexiert - 60 Dateien, 804 Knoten, 1.321 Kanten
- **Working tree:** sauber
## 3) Git

- **Repo-Typ:** geklont (GitHub)
- **Pfad / URL:** `C:\Users\Admin\.openclaw\workspace\projects\bp-analyzer\`
  - Remote-URL: https://github.com/schirkan/bp-analyzer.git
- **Remote(s):** `origin` -> https://github.com/schirkan/bp-analyzer.git (fetch + push)
- **Eingerichtet am:** 2026-10-26
- **`.gitignore`-Status:** vorhanden, vom Fremd-Repo übernommen und **für dieses Projekt ergänzt**:
  - `.github/*` + `!.github/workflows/` - Git kann keine Dateien unterhalb eines
    ausgeschlossenen Verzeichnisses wieder einbeziehen, deshalb ist `.github/` selektiv
    statt als Ganzes ausgeschlossen (die Workflows müssen trackbar bleiben).
  - `publish/` - Ausgabe von `dotnet publish -o publish` (vorher nur lokal in
    `.git/info/exclude`, damit wäre es bei anderen als untracked aufgetaucht).
  - `bp-analyzer.config.json` - lokale Konfiguration; **das** Beispiel
    `bp-analyzer.config.example.json` bleibt trackbar.
  - `ci-out/`, `package/` - CI-Ausgabeverzeichnisse.

  `.codegraph/` ist weiterhin **lokal** in `.git/info/exclude` - der Index ist ein
  Werkzeug-Artefakt dieser Arbeitsumgebung und kein Projektbestandteil.

**Standard-Branch:** `main` (HEAD -> `origin/main`, Upstream: schirkan/bp-analyzer)

**Eigene Commits auf `main`:** vier Commits gegen die Upstream-Basis `35d89fa` - ein Code-Fix
(Passwort-Handler, siehe Abschnitt 14), zwei Doku-Commits (`agents.md` + `README.md`,
Commit-Identitaets-Vermerk), der Single-EXE-Publish mit Trim-Fix (`fdf639a`) und
`0c3d542` (Version 1.0.0 + Produkt-Metadaten). Alle nach `origin/main` gepusht.
Der Working tree ist damit sauber.

**Tags und Releases:**

| Tag | Commit | Release | Assets | Erzeugt |
| --- | --- | --- | --- | --- |
| `v1.0.0` | `0c3d542` | https://github.com/schirkan/bp-analyzer/releases/tag/v1.0.0 | `BP-Analyzer-win-x64.exe` (12.625.429 B), `checksums.txt` | manuell via `gh release create` |
| `v1.0.1` | `d5fd2db` | https://github.com/schirkan/bp-analyzer/releases/tag/v1.0.1 | `BP-Analyzer-win-x64.exe` (12.625.782 B), `checksums.txt` | **automatisch** via `release.yml` |
| `v1.0.2` | `72a1ab5` | https://github.com/schirkan/bp-analyzer/releases/tag/v1.0.2 | `BP-Analyzer-win-x64.exe` (12.652.651 B), `BP-Analyzer-win-x64.zip` (7.623.865 B), `checksums.txt` (182 B) | **automatisch** via `release.yml` |

**Namensstand ab Commit `HEAD`:** kuenftige Releases benennen die Dateien wieder neutral als
`BP-Analyzer.exe` und `BP-Analyzer.zip`; das fruehere `-win-x64` im Dateinamen wurde auf
Wunsch entfernt. Die historischen Releases `v1.0.0` bis `v1.0.2` behalten ihre bereits
veroeffentlichten Dateinamen.

`v1.0.1` und `v1.0.2` sind vollstaendig ueber CI erzeugt. `v1.0.2` ist das Release mit
der Konfigurationsmoeglichkeit und dem ZIP. Verifiziert: alle drei Assets heruntergeladen,
SHA256 stimmen mit `checksums.txt` ueberein, ZIP entpackt -> `codegen` + `analyze` Exit 0,
`ProductVersion` = `1.0.2+72a1ab5`, die Config aus dem ZIP wird gelesen.
`v1.0.0` bleibt das manuell erstellte Referenz-Release.

**CI-Blocker (behoben):** `.github/` stand in der getrackten `.gitignore` (Zeile 50) - ein
Workflow waere von git stillschweigend ignoriert worden. Ersetzt durch `.github/*` +
`!.github/workflows/`.

**Commit-Identitaet:** `OpenClaw <openclaw@schirkan.de>` - feste Vorgabe, alle eigenen
Commits laufen unter diesem Namen, unabhaengig von der Upstream-Historie.

**CI/CD:** GitHub Actions, zwei Workflows:

| Feld | Wert |
| --- | --- |
| **Plattform** | GitHub Actions |
| **Workflow-Datei(en)** | `.github/workflows/release.yml` |
| **Trigger** | `release.yml`: Push eines Tags `v*`, manuell |
| **Was wird gebaut** | `release.yml`: Single-File-EXE + ZIP mit Beispiel-Config + `checksums.txt` |
| **Output / Artefakte** | `release.yml`: GitHub-Release zum Tag mit den Assets `BP-Analyzer.exe`, `BP-Analyzer.zip` (EXE + Beispiel-Config) und `checksums.txt` (Hashes fuer EXE und ZIP); zusaetzlich Workflow-Artifact `BP-Analyzer` |
| **Letzter bekannter Lauf** | `release.yml`: Run 41871915033 am 2026-11-04, Tag `v1.0.2`, conclusion **success** (Release mit EXE, ZIP und Checksummen automatisch erstellt; Binary-Version = 1.0.2) |

**Version folgt dem Tag:** `release.yml` leitet die Version aus `GITHUB_REF_NAME` ab
(ohne fuehrendes `v`) und setzt sie per `-p:Version=`. Ohne das meldet das Binary
immer die fest im csproj hinterlegte Version statt der Tag-Version.

**Wichtig - Restore braucht das Publish-Profil:** `dotnet restore` muss mit
`-p:PublishProfile=BP-Analyzer-win-x64` laufen. Ohne das Profil holt der Restore die
ILLink-Assets nicht, und der nachfolgende Publish mit `--no-restore` ueberspringt das
Trimming **stillschweigend** - die EXE wiegt dann 35 MB statt 12,6 MB. Der Workflow
prueft die Groesse und bricht ab, wenn sie ueber 20 MB liegt.

**`.gitignore`:** Die Regel `.github/` wurde durch `.github/*` + `!.github/workflows/`
ersetzt. Git kann keine Dateien unterhalb eines ausgeschlossenen Verzeichnisses wieder
einbeziehen - der Ordner selbst muss selektiv ausgeschlossen werden.

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

**Bug 4 - Prozessaufrufe wurden nur bei CWD = Repo-Root aufgeloest (behoben).**
`ProcessResolver.FindProcessNameById()` suchte fest in `<CWD>\\xml` und ignorierte den
per `--xml=` uebergebenen Ordner. Rief man `codegen` aus einem anderen Arbeitsverzeichnis
auf, wurde der Prozess-Aufruf nicht aufgeloest und der Code enthielt stattdessen
`' TODO: Implement process call (processid: ...)`. Zusaetzlich fehlte die
Abhaengigkeit in `dependencies.json`, was sich in den SDD-Dateien zeigte.

Fix: `ProcessResolver.SetXmlSearchDirectory(xmlDirectory)` wird von
`BluePrismCodeGen.GenerateAll()` gesetzt; gesucht wird zuerst im uebergebenen XML-Ordner,
danach weiterhin als Fallback in `<CWD>\\xml`.

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

## 17) Konfiguration und Single-EXE-Publish

### Befehl

````powershell
dotnet publish BP-Analyzer.csproj -p:PublishProfile=BP-Analyzer-win-x64 -o publish
````

Ergebnis: eine einzelne `BP-Analyzer.exe` (win-x64, ca. 12,6 MB). Keine .NET-Runtime
auf dem Zielsystem noetig, keine weiteren Dateien noetig.

### Publish-Profil

`Properties/PublishProfiles/BP-Analyzer-win-x64.pubxml` setzt:

| Property | Wert | Bedeutung |
| --- | --- | --- |
| `RuntimeIdentifier` | `win-x64` | Zielplattform |
| `SelfContained` | `true` | .NET-Runtime wird mitgeliefert |
| `PublishSingleFile` | `true` | alles in eine EXE |
| `PublishTrimmed` | `true` | ungenutzter Code wird entfernt |
| `TrimMode` | `partial` | nur sichere Kuerzungen |
| `IncludeNativeLibrariesForSelfExtract` | `true` | Native Libs in die EXE |
| `EnableCompressionInSingleFile` | `true` | komprimiert (17,3 MB -> 12,6 MB) |
| `DebugType` | `embedded` | keine separate PDB-Datei |

Groessenvergleich (Release, win-x64):

| Variante | Groesse | Status |
| --- | --- | --- |
| `dotnet build` (framework-dependent) | ca. 0,2 MB + DLLs | braucht .NET-8-Runtime |
| self-contained, single-file, ungetrimmt | 67,9 MB | funktioniert |
| self-contained, single-file, getrimmt | 17,3 MB | funktioniert |
| + komprimiert (Profil) | 12,6 MB | funktioniert |

### Templates sind eingebettet

Die drei Templates liegen jetzt als Embedded Resources in der Assembly
(`LogicalName` = `BPAnalyzer.templates.<Dateiname>`). `CodeGen/TemplateProvider.cs` loest
Templates so auf:

1. Datei auf Platte (BaseDirectory/templates, CWD/templates, CWD) - bestehender
   Entwickler-Workflow und lokale Overrides bleiben erhalten,
2. eingebettete Ressource - macht die Single-EXE eigenstaendig.

Ohne diesen Schritt fehlten in der Single-EXE `BP_Base.vb`, `BluePrism_Generated.vbproj`
und das SDD-Template.

### JSON-Serialisierung ist trim-sicher

`CodeGen/CodeGenJsonContext.cs` ist ein `JsonSerializerContext` mit Source-Generator.
`CodeGenRegistry` serialisiert damit ueber `CodeGenJsonContext.Default.<Typ>` statt ueber
`JsonSerializer.Serialize(value, options)`. Grund: unter `PublishTrimmed=true` ist
reflection-basierte Serialisierung deaktiviert, der Aufruf bricht sonst mit
"Reflection-based serialization has been disabled for this application" ab.

Verifiziert: die drei JSON-Dateien der getrimmten EXE sind semantisch identisch zu
denen des Debug-Builds.

### Konfiguration (optional)

**Nur der `export`-Modus braucht eine Konfiguration.** `codegen` und `analyze` laufen ohne
jede Config-Datei - die Templates sind eingebettet.

Der Pfad zu Blue Prisms `AutomateC.exe` ist jetzt konfigurierbar, vorher war er fest
verdrahtet (die Zuweisung in `CliExportHandler.cs` war auskommentiert):

| Quelle | Prioritaet | Beispiel |
| --- | --- | --- |
| CLI-Argument `--automate-c=<pfad>` | 1 (hoechste) | `BP-Analyzer.exe export --process="X" --automate-c="C:\\Pfad\\AutomateC.exe"` |
| `bp-analyzer.config.json` neben der EXE, dann im CWD | 2 | `{ "automateCPath": "C:\\Pfad\\AutomateC.exe" }` |
| Eingebauter Default | 3 | `C:\\Program Files\\Blue Prism Limited\\Blue Prism Automate\\AutomateC.exe` |

Fehlt `AutomateC.exe`, bricht der Export mit klarer Meldung und Exit-Code 1 ab, statt
still zu scheitern.

**Zwei Fallen, die beim Testen aufgefallen sind:**

1. **JSON-Property-Matching ist per Default case-sensitiv.** Die Datei nutzt camelCase
   (`automateCPath`), die C#-Property heisst `AutomateCPath`. Ohne
   `PropertyNameCaseInsensitive = true` im `JsonSourceGenerationOptions` wurde der Wert
   **stillschweigend ignoriert** und der Default benutzt. Der Test sah nur aus wie
   "Config funktioniert nicht".
2. **Die Redaction maskiert Passwort-Muster auch in eigenen Dateien.** Ein `edit` mit
   `--password=***` im oldText schlug fehl, weil die Datei `...` enthaelt. Fuer
   Edits in solchen Dateien PowerShell (`[System.IO.File]::ReadAllText` + `.Replace()`)
   statt des `edit`-Tools verwenden.

Beispiel-Config: `bp-analyzer.config.example.json` im Repo. Sie wird im Release-ZIP als
`bp-analyzer.config.json` mitgeliefert.

### Smoke-Test

````powershell
# In einem leeren Verzeichnis, ohne templates- und ohne xml-Ordner:
BP-Analyzer.exe codegen --xml=<pfad> --output=code
BP-Analyzer.exe analyze --code=code --output=sdd
````

Beide Modi liefern byte-identische Ergebnisse zum Debug-Build (ausser der
Timestamp-Zeile in den generierten .vb-Dateien).
