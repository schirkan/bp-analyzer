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

- [x] implement stage type=WaitStart in combination with stage type=WaitEnd
  - the corresponding WaitEnd for a WaitStart can be found by the identical <groupid> inside the stages
  - the WaitEnd stage represents the "Case Else" of the generated "Select" and contains only a "GoTo xxx" statement
  - sample:
    ```
      <stage stageid="204296cd-f9ae-43ef-81b9-db2aabed1b48" name="T" type="WaitEnd">
        <subsheetid>6e59731b-6dcb-4471-a50b-edcb373114df</subsheetid>
        ...
        <onsuccess>8c5feb38-8094-4c3f-a292-ac967710a2e8</onsuccess>
        <groupid>20221061-c786-41b4-9428-6f34b8358e9e</groupid>
      </stage>
      <stage stageid="a61f62ae-7993-470f-8d1f-cffdcd5bea5b" name="W120" type="WaitStart">
        <subsheetid>6e59731b-6dcb-4471-a50b-edcb373114df</subsheetid>
        ...
        <groupid>20221061-c786-41b4-9428-6f34b8358e9e</groupid>
        <choices>
          ...
        </choices>
        <timeout>120</timeout>
      </stage>
    ```
    
- [x] implement expression for choices
  - sample xml:
    ```
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
    ```
  - expected output:
    ```
      Application.Element("397fb78c-8c08-4dc0-987c-3ca33d3762a4").CheckExists = True
    ```
  - Implemented in GenerateWaitChoiceExpression() method
  - Supports comparetype (Equal, NotEqual, LessThan, GreaterThan, LessThanOrEqual, GreaterThanOrEqual)
  - Uses reply attribute as comparison value (default True)

- [x] implement stage type=process similar to stage type=action
  - sample xml:
    ```
      <stage stageid="5eb89ba8-fa28-4939-b7d7-cc29a13cf616" name="Call Process A" type="Process">
        <loginhibit onsuccess="true" />
        <display x="-30" y="390" />
        <inputs>
          <input type="text" name="Name" friendlyname="Name" expr="[MyText]" />
        </inputs>
        <outputs>
          <output type="number" name="Char Count" friendlyname="Char Count" stage="Char Count" />
        </outputs>
        <onsuccess>1f3bb070-109c-4bbc-babf-3ef6af6b6fa2</onsuccess>
        <processid>42b5169c-1fde-4a1a-b912-4d1249805188</processid>
      </stage>
    ```
  - expected output:
    ```
      MP___Subprocess_A.Instance.Main(...);
    ```
  - to find the process name ("MP___Subprocess_A" in the example) you need to search for the processid in all xml files to find the process definition (first line in process xml file) like:
    ```
      <process name="MP - Subprocess A" version="1.0" bpversion="7.5.0.17125" cpeversion="10.0.110.0" narrative="This is a test subprocess" byrefcollection="true" processrunningmessage="" disableversioning="false" preferredid="42b5169c-1fde-4a1a-b912-4d1249805188">
    ```
  - Implemented in GenerateProcessStage() method
  - Uses FindProcessNameById() to search for processid in all XML files
  - Supports input/output parameters with named arguments

- [x] in all methods all params should be optional
- [x] implement stage type=Navigate
- [x] implement stage type=Reader
- [x] implement stage type=Writer
- [x] implement stage type=Code
- [x] fix static variables
  - if data stage does not have <alwaysinit />: change variable definition from "Dim" to "Static"
- [x] implement input stage
  - sample xml:
    ```    
    <stage stageid="ffbd078e-2ea0-4795-8ab7-31c0c6a38edd" name="Start" type="Start">
      <subsheetid>da65086d-4794-4b77-a06e-0c67a6dcf0d8</subsheetid>
      <display x="15" y="-105" />
      <inputs>
        <input type="text" name="InData1" stage="Data8" />
        <input type="number" name="InData2" stage="Data6" />
      </inputs>
      <onsuccess>4a05cdbe-f075-4c4f-972b-7e63880a1bb6</onsuccess>
    </stage>
    ```
  - expected code after "Local variables" and before "Initialize local variables with alwaysinit":
    ```
    ' Initialize local variables with input values
    Data8 = InData1
    Data6 = InData2
    ```

- [x] implement global code
- [x] code stage with params
- [x] implement multicalc
  - reuse codegen logic from sinlge calc
  - sample:
        ' Data5 = 123
        ' Data6 = 7.8
        ' Data8 = "tttt"
- [x] implement stage type LoopStart and LoopEnd
- [x] implement stage type Collection
- [x] fix stage type block (on error)
- [ ] generate method to evaluate expressions
  - MyToggle = DataBinder.Eval(Me, "[MyToggle] = False")
- [ ] implement app model xml
- [ ] add missing system methods to Template_BP_Base.vb
  - ...
