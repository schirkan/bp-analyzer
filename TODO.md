# TODO

=> for example xml see "bp demo.xml"

- Exception Handling
  - Info:
    - jede Stage kann in genau einem Block sein, wenn siue in keinem block ist, gehört die Stage direkt zu page
    - Es kann in jedem Block und auf der Page jeweils genau eine "Recover" stage geben.
    - Ein Paar aus Revocer + Resume Stage sind mit einem Exception Handling mit Try + Catch vergleichbar.
  - In der Codegenerierung:
    - [ ] stage type "Block" werden nicht im code gerendert.
    - [ ] erzeuge Algorithmus um zu berechnen, ob sich eine Stage in einem Block befindet
      - Block hat Koordinaten und Größenangabe: <display x="-30" y="-75" w="180" h="60" />
      - Jede Stage hat Koordinaten <display x="105" y="-45" />
      - Ermittle, ob die Koordinaten innerhalb des Vierecks liegen
    - [ ] use "On Error GoTo xxx" for exception handling in every stage

- Methoden Signatur
  - [x] Sichtbarkeit korrigieren (public vs private)
  - [x] In Prozessen ist nur die Main Page public - alle anderen Methode sind private
  - [x] In Objekt können alle Methoden public sein (erkennbar am Parameter published="False/True")
  - [x] Methoden Kommentar soll extrahiert werden aus SubSheetInfo > narrative
    ```<stage stageid="322b5384-901a-489e-8f42-5059acf253be" name="MyPublicAction" type="SubSheetInfo">
    <subsheetid>88732eb0-daa0-4bbe-9a36-39f2f23b00f9</subsheetid>
    <narrative>return the first two chars of a VNR</narrative>
    <display x="-150" y="-75" w="150" h="90" />
    </stage>
    ```

- [x] Collections implementieren
  - wie andere DataItems
  - Datetyp ist DataTable
  - kann initialwerte enthalten

- Cleanup
  - [x] remove comment ```' Stage: Start (Start)```
  - [x] remove comment ```' Stage: End (End)```
  - [x] remove comment ```' Constructor body generated from BluePrism global stages```

- [x] globale Collections
  - Logik analog zu DataItems. Sind global, außer sie haben "<private />" tag.

- [x] Vereinheitlichung der Logik zur Generierung der Methodensignatur
  - betrifft GenerateSubsheetsAsMethods + GenerateMainMethod + GenerateConstructor
  - Refactoring für gleiche Logik für In- und output parameter

- [x] Implement action call
  - sanitize class name
  - erstelle eine Instanz der Klasse als private klassen variable
    - ´´´' Calling: Microsoft Store.Terminate()´´´
    - Dim Microsoft_Store = new Microsoft_Store();
  - Beim Aufruf die in und out Parameter übergeben.

- [x] action call signature
  - Passing Arguments by Name

- [x] einheitliche Kommentare für alle Methoden
  - Main Page, Konstruktur, Destructor, Page
  - für Main Page: "narrative" property am <process> Element
  - alle anderen aus <narrative> unterhalb der <SubSheetInfo>
  - Methoden-Kommentar soll auch Input und Output Parameter beschreiben
