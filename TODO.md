# TODO

=> for example xml see "bp demo.xml"

- Exception Handling
  - Info:
    - jede Stage kann in genau einem Block sein, wenn sie in keinem block ist, gehört die Stage direkt zur Page (gilt für alle Methoden/Subsheet/Main/Konstruktur/Destruktur)
    - Es kann in jedem Block und auf der Page jeweils genau eine "Recover" stage geben.
    - Ein Paus aus Revocer + Resume Stage sind mit einem Exception Handling mit Try + Catch vergleichbar.
  - In der Codegenerierung:
    - [x] stage type "Block" werden nicht im code gerendert.
    - [x] erzeuge Algorithmus um zu berechnen, ob sich eine Stage in einem Block befindet
      - Block hat Koordinaten und Größenangabe: <display x="-30" y="-75" w="180" h="60" />
      - Jede Stage hat Koordinaten <display x="105" y="-45" />
      - Ermittle, ob die Koordinaten innerhalb des Vierecks liegen
    - [x] use "On Error GoTo recover_onsuccess_Label" for exception handling in every stage that is within a block or page that has a recover stage
      - [x] line add after Stage-Label
    - [x] use "Resume resume_onsuccess_Label" for stage type=Resume
    - [x] Labels vereinfacht (ohne Stage-Namen)
    - [x] Zentrale Methode für Label-Generierung
    - [x] Exceptions can be rethrown, if usecurrent="yes".
      - sample xml:
      ```
      <stage stageid="a987ff7d-de3a-4d2c-bcca-696cda7cdcb2" name="Re-Throw" type="Exception">
        <subsheetid>d448f24d-bfbf-42c9-9ee8-35bcdb734774</subsheetid>
        <display x="-150" y="180" />
        <exception localized="yes" type="" detail="" usecurrent="yes" />
      </stage>
      ```
      - [x] create a new method in Base-Class "RethrowException" and use this if usecurrent="yes"
      - [x] create a new method in Base-Class "StoreException" and use this in recover stage to save the current exception in a private class variable

- Methoden Signatur
  - [x] Sichtbarkeit korrigieren (public vs private)
  - [x] In Prozessen ist nur die Main Page public - alle anderen Methode sind private
  - [x] In Objekt können alle Methoden public sein (erkennbar am Parameter published="False/True")
  - [x] Methoden Kommentar soll extrahiert werden aus SubSheetInfo > narrative
    ```
    <stage stageid="322b5384-901a-489e-8f42-5059acf253be" name="MyPublicAction" type="SubSheetInfo">
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

- [x] Input/Output Parameter Initialisierung überarbeiten
  - Trenne Definition (Dim xxx as String) und Initialisierung (Set xxx = "test")
  - Alle Variablen (lokal, global, in, out) können in der Start-Stage initialisiert werden
  - Initial-Werte werden nur gesetzt wenn <alwaysinit /> existiert
  - Bei Input-Variablen muss zusätzlich der übergebene Wert leer sein

- [ ] implement stage type=WaitStart in combination with stage type=WaitEnd
  - the corresponding WaitEnd for a WaitStart can be found by the identical <groupid> inside the stages
  - the WaitEnd stage represents the "Case Else" of the generated "Select" and contains only a "GoTo xxx" statement
  - sample:
      ```
       <stage stageid="204296cd-f9ae-43ef-81b9-db2aabed1b48" name="T" type="WaitEnd">
          <subsheetid>6e59731b-6dcb-4471-a50b-edcb373114df</subsheetid>
          <loginhibit />
          <display x="120" y="165" w="30" h="30" />
          <onsuccess>8c5feb38-8094-4c3f-a292-ac967710a2e8</onsuccess>
          <groupid>20221061-c786-41b4-9428-6f34b8358e9e</groupid>
        </stage>
        <stage stageid="a61f62ae-7993-470f-8d1f-cffdcd5bea5b" name="W120" type="WaitStart">
          <subsheetid>6e59731b-6dcb-4471-a50b-edcb373114df</subsheetid>
          <loginhibit />
          <display x="-30" y="165" w="30" h="30" />
          <groupid>20221061-c786-41b4-9428-6f34b8358e9e</groupid>
          <choices>
            <choice reply="True" expression="True">
              <name>Button: Die Überprüfung auf Updates wurde abgeschlossen. Check Exists</name>
              <distance>45</distance>
              <ontrue>adcc59e6-060b-43ad-aebe-73dda98e8443</ontrue>
              <element id="397fb78c-8c08-4dc0-987c-3ca33d3762a4" />
              <condition>
                <id>CheckExists</id>
              </condition>
              <comparetype>Equal</comparetype>
            </choice>
            <choice reply="True" expression="True">
              <name>Button: Nach Updates suchen Check Exists</name>
              <distance>105</distance>
              <ontrue>1eb4d7bb-9dc0-40c3-b482-a9ff64ad0586</ontrue>
              <element id="2a7db9d3-154c-404c-b610-5c3ae51ecb32" />
              <condition>
                <id>CheckExists</id>
              </condition>
              <comparetype>Equal</comparetype>
            </choice>
          </choices>
          <timeout>120</timeout>
        </stage>
      ```