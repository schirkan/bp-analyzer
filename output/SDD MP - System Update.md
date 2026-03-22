# SDD: MP - System Update

This document was generated automatically by BP-Analyzer.

## Dependencies
| Process / Object | Page |
| ---------------- | ---- |
|Microsoft_Store|Launch|
|Microsoft_Store|Start_Updates|
|Microsoft_Store|Terminate|
|Microsoft_Store|Wait_Updates_Finished|
|Utility_Environment|Start_Process|
|Windows_Settings|Launch|
|Windows_Settings|Start_Updates|
|Windows_Settings|Terminate|
|Windows_Settings|Wait_Updates_Finished|

## Exceptions
| Type | Process / Object | Page | Text | Reason |
| ---- | ---------------- | ---- | ---- | ------ |
|System Exception|Microsoft_Store|Launch|"Main Window not found"||
|System Exception|Microsoft_Store|Start_Updates|"Download Header not found"||
|System Exception|Microsoft_Store|Start_Updates|"Nach Updates suchen Button not found"||
|System Exception|Windows_Settings|Launch|"Main Window not found"||
|System Exception|Windows_Settings|Start_Updates|"Download Menu not found"||
|System Exception|Windows_Settings|Wait_Updates_Finished|"Download Header not found"||
