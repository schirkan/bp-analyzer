# AGENTS – Projektwissen für BP-Analyzer

Diese Datei sammelt alle für den Assistenz-Agenten relevanten Informationen aus dem bisherigen Verlauf und den Projektdateien.
Passe diese Datei selbstständig an, wenn sich relevante Änderungen ergeben.

## 1) Projektüberblick

- **Name:** BP-Analyzer
- **Zweck:** Konvertiert BluePrism-XML-Exporte in VB.NET-Klassen mit GoTo-basiertem Prozessfluss.
- **Technologie:** .NET CLI / C# (TargetFramework: `net9.0`)
- **Projektdatei:** `BP-Analyzer.csproj`

## 2) Build- und Start-Befehle

### Projekt bauen

```bash
dotnet build BP-Analyzer.csproj
```

### Projekt ausführen

```bash
dotnet run --project BP-Analyzer.csproj
```

## 3) Relevante CLI-Befehle (Nutzung des Tools)

### Code-Generierung (XML -> VB.NET)

```bash
# Standard: xml/ -> output/
dotnet run --project BP-Analyzer.csproj -- --codegen

# Mit expliziten Verzeichnissen
dotnet run --project BP-Analyzer.csproj -- --codegen --xml=xml --output=output

# Kurzformen
dotnet run --project BP-Analyzer.csproj -- --codegen xml output
dotnet run --project BP-Analyzer.csproj -- --codegen xml
```

### Prozess-Export (BluePrism Runtime Resource)

```bash
# Basis
dotnet run --project BP-Analyzer.csproj -- --process="MeinProzess"

# Mit Ausgabepfad
dotnet run --project BP-Analyzer.csproj -- --process="MeinProzess" --output="C:\Exporte"

# Mit Credentials
dotnet run --project BP-Analyzer.csproj -- --process="MeinProzess" --user=admin --password=geheim

# Überschreiben aktivieren
dotnet run --project BP-Analyzer.csproj -- --process="MeinProzess" --overwrite=yes
```

## 4) Generierte Ausgabe kompilieren

```bash
cd output
dotnet build BluePrism_Generated.vbproj
```

Alternative (Visual Studio MSBuild):

```powershell
msbuild BluePrism_Generated.vbproj
msbuild BluePrism_Generated.vbproj /p:Configuration=Debug
msbuild BluePrism_Generated.vbproj /p:Configuration=Release
```

Erwartetes Build-Artefakt:

- `output/bin/Debug/BluePrism_Generated.dll`
- oder `output/bin/Release/BluePrism_Generated.dll`

## 5) Hilfreiche Befehle zur Fehleranalyse

### Allgemeine Build-/Run-Diagnose

```bash
# Vollständiger Build mit Fehlerausgabe
dotnet build BP-Analyzer.csproj -v minimal

# Ausführen inkl. CLI-Args (Beispiel) und direkte Fehlerausgabe
dotnet run --project BP-Analyzer.csproj -- --codegen 2>&1
```

### Sauberer Neuaufbau

```bash
dotnet clean BP-Analyzer.csproj
dotnet build BP-Analyzer.csproj
```

### Ausgabeprojekt prüfen

```bash
dotnet build output/BluePrism_Generated.vbproj -v minimal
```

### Git-basierte Analyse

```bash
git status
git diff
git log --oneline -n 20
```

## 6) Wichtige Projektdateien

- `Program.cs` – Einstiegspunkt / CLI-Verarbeitung
- `BluePrismCodeGen.cs` – Kern der XML->VB.NET Codegenerierung
- `ExporterCLI.cs` – Prozess-Export über BluePrism CLI
- `BluePrismExporter.cs` – Exportlogik
- `templates/Template_BP_Base.vb` – Basisklassen-Template
- `templates/Template_BP_Project.vbproj` – VB-Projekt-Template
- `xml/` – Input-XML-Dateien
- `output/` – generierte VB-Dateien + `BluePrism_Generated.vbproj`

## 7) Funktionaler Stand (aus TODO.md)

Bereits umgesetzt (Auszug aus TODO):

- Exception-Handling inkl. Recover/Resume/Rethrow-Logik
- Methodensignaturen (Sichtbarkeit in Prozess vs. Objekt)
- Collections (lokal + global)
- Action-Call-Implementierung inkl. Named Arguments
- Einheitliche Methodenkommentare
- Überarbeitete Input/Output-Initialisierung
- WaitStart/WaitEnd + Choice-Expression-Logik

## 8) Hinweise für zukünftige Agent-Arbeit

### Suchen in Dateien

**WICHTIG:** Bei der Verwendung von `search_files` muss der `path`-Parameter immer ein konkretes Verzeichnis oder eine konkrete Datei sein.

Falsch (keine Ergebnisse):
```bash
search_files(path="output", regex="TODO")
```

Richtig (findet Ergebnisse):
```bash
search_files(path="output/Microsoft Store.vb", regex="TODO")
```

oder:

```bash
# Zuerst mit genauem Pfad suchen
search_files(path="output", file_pattern="*.vb", regex="TODO")
```

**Hintergrund:** Der `search_files`-Tool verhält sich bei rekursiven Suchen in Verzeichnissen unvorhersehbar. Bei spezifischen Suchen (z.B. nach "TODO" in generierten .vb-Dateien) sollte immer ein konkreter Dateipfad oder `file_pattern` verwendet werden.

### Weitere Tipps

- Bei Problemen zuerst `dotnet build BP-Analyzer.csproj` ausführen.
- Bei Codegen-Problemen mit kleinen XML-Beispielen arbeiten (z. B. `xml/bp demo.xml`).
- Änderungen an Generatorlogik immer gegen Ausgabe in `output/*.vb` verifizieren.
- Für Regressionscheck: Codegen erneut ausführen und `git diff` auf `output/` prüfen.
