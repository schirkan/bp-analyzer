Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Data
Imports System.Drawing

''' <summary>
''' BluePrism object: Microsoft Store
''' Version: 7.5.0.17125
''' Generated: 2026-03-11 20:28:04
''' </summary>
Public Class Microsoft_Store
    Inherits BP_Base

    #Region "Singleton Instance"

    Private Shared ReadOnly _lazyInstance As New Lazy(Of Microsoft_Store)(Function() New Microsoft_Store())

    Public Shared ReadOnly Property Instance As Microsoft_Store
        Get
            Return _lazyInstance.Value
        End Get
    End Property

    #End Region

    #Region "Global Data Items"

    ' No global data items

    #End Region

    #Region "Methods"

    ''' <summary>
    ''' Constructor (Initialize) - called when object is created
    ''' </summary>
    Public Sub New()

    End Sub

    ''' <summary>
    ''' Destructor (CleanUp) - called when object is disposed
    ''' </summary>
    Protected Overrides Sub Finalize()

    End Sub

    ''' <summary>
    ''' Page: Launch
    ''' </summary>
    Public Sub Launch()

        ' Local variables
        Dim FilePath As String

        ' Initialize variables with initialvalue
        FilePath = "ms-windows-store://updates"

        ' Start Process
        On Error GoTo Launch_Recover
        Utility_Environment.Instance.Start_Process(Application:=FilePath)
        
        ' Attach
        Launch_Attach:
        Application.Element("Microsoft Store").AttachApplication()

        ' W5
        ' Wait 5 seconds for condition with 1 choice(s)
        Select Case True
            Case Application.Element("Main Window", "b048ceed-93fb-48da-99af-7fafeec74d4e").CheckExists = True ' Main Window Check Exists
                GoTo End_Launch
        End Select

        ' SE
        Throw New BP_Exception("System Exception", "Main Window not found")

        ' Recover
        Launch_Recover:
        StoreException()

        ' Resume
        ClearException()
        Resume Launch_Attach

        End_Launch:

    End Sub

    ''' <summary>
    ''' Page: Terminate
    ''' </summary>
    Public Sub Terminate()

        ' Terminate
        Application.Element("Microsoft Store").Terminate()

    End Sub

    ''' <summary>
    ''' Page: Start_Updates
    ''' </summary>
    Public Sub Start_Updates()

        
        ' W5
        Start_Updates_W5:
        ' Wait 5 seconds for condition with 1 choice(s)
        Select Case True
            Case Application.Element("Header: Updates und Downloads", "0340b82e-9c61-45bd-a50b-f5691247a070").CheckExists = True ' Header: Updates und Downloads Check Exists
                GoTo Start_Updates_W5_2
        End Select
        
        ' SE
        Start_Updates_SE:
        Throw New BP_Exception("System Exception", "Download Header not found")

        ' W5
        Start_Updates_W5_2:
        ' Wait 5 seconds for condition with 1 choice(s)
        Select Case True
            Case Application.Element("Button: Nach Updates suchen", "2a7db9d3-154c-404c-b610-5c3ae51ecb32").CheckExists = True ' Button: Nach Updates suchen Check Exists
                GoTo Start_Updates_Click_Nach_Updates_suchen
        End Select

        ' SE
        Throw New BP_Exception("System Exception", "Nach Updates suchen Button not found")

        ' Click Nach Updates suchen
        Start_Updates_Click_Nach_Updates_suchen:
        Application.Element("Button: Nach Updates suchen").UIAButtonPress()

        ' W120
        ' Wait 120 seconds for condition with 2 choice(s)
        Select Case True
            Case Application.Element("Button: Die Überprüfung auf Updates wurde abgeschlossen.", "397fb78c-8c08-4dc0-987c-3ca33d3762a4").CheckExists = True ' Button: Die Überprüfung auf Updates wurde abgeschlossen. Check Exists
                GoTo Start_Updates_W2
            Case Application.Element("Button: Nach Updates suchen", "2a7db9d3-154c-404c-b610-5c3ae51ecb32").CheckExists = True ' Button: Nach Updates suchen Check Exists
                GoTo Start_Updates_W2
        End Select

        ' SE
        Throw New BP_Exception("System Exception", "Download Header not found")

        ' W2
        Start_Updates_W2:
        ' Wait 2 seconds for condition with 1 choice(s)
        Select Case True
            Case Application.Element("Button: Nach Updates suchen", "2a7db9d3-154c-404c-b610-5c3ae51ecb32").CheckExists = True ' Button: Nach Updates suchen Check Exists
                GoTo Start_Updates_Click_Update_All
        End Select

        ' Note1
        ' No Updates
        GoTo End_Start_Updates

        ' Click Update All
        Start_Updates_Click_Update_All:
        ' Navigate: No steps defined
        
        ' W5
        Start_Updates_W5_3:
        ' Wait 5 seconds for condition with 1 choice(s)
        Select Case True
            Case Application.Element("Button: Nach Updates suchen", "2a7db9d3-154c-404c-b610-5c3ae51ecb32").CheckExists = True ' Button: Nach Updates suchen Check Exists
                GoTo End_Start_Updates
        End Select

        ' SE
        Throw New BP_Exception("System Exception", "Nach Updates suchen Button not found")

        ' Note2
        Start_Updates_Note2:
        ' TODO
        GoTo Start_Updates_W2

        ' Note2
        ' TODO
        GoTo Start_Updates_Click_Update_All

        ' Note2
        ' TODO
        GoTo Start_Updates_W5_3

        End_Start_Updates:

    End Sub

    ''' <summary>
    ''' Page: Wait_Updates_Finished
    ''' </summary>
    Public Sub Wait_Updates_Finished()

        ' Note2
        ' TODO

    End Sub

    #End Region

    #Region "App Model"

    Protected Application As Object

    #End Region

End Class
