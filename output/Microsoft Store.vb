' Generated from BluePrism object: Microsoft Store
' Version: 1.0
' Generated: 2026-03-10 15:43:20

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Data
Imports System.Drawing

''' <summary>
''' BluePrism object: Microsoft Store
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
    ''' BluePrism page: Launch
    ''' </summary>
    Public Sub Launch()

        ' Local variables
        Dim FilePath As String

        ' Initialize variables with initialvalue
        FilePath = "ms-windows-store://updates"

        On Error GoTo Recover_Label

        ' Start Process
        On Error GoTo Recover_Label
        Utility___Environment.Instance.Start_Process(Application:=FilePath)
        
        ' Attach
        Navigate_Label:
        On Error GoTo Recover_Label
        Application.Element("Microsoft Store", "330860ce-038b-499b-9a6c-c1e8140f72a2").AttachApplication()

        ' W5
        On Error GoTo Recover_Label
        ' Wait 5 seconds for condition with 1 choice(s)
        Select Case True
            Case Application.Element("Main Window", "b048ceed-93fb-48da-99af-7fafeec74d4e").CheckExists = True ' Main Window Check Exists
                GoTo End_Launch_Label
        End Select

        ' SE
        On Error GoTo Recover_Label
        RaiseException("System Exception", "Main Window not found")

        ' Recover
        Recover_Label:
        StoreException()

        ' Resume
        ClearException()
        Resume Navigate_Label

        End_Launch_Label:

    End Sub

    ''' <summary>
    ''' BluePrism page: Terminate
    ''' </summary>
    Public Sub Terminate()

        ' Terminate
        Application.Element("Microsoft Store", "330860ce-038b-499b-9a6c-c1e8140f72a2").Terminate()

    End Sub

    ''' <summary>
    ''' BluePrism page: Start_Updates
    ''' </summary>
    Public Sub Start_Updates()

        ' W5
        ' Wait 5 seconds for condition with 1 choice(s)
        Select Case True
            Case Application.Element("Header: Updates und Downloads", "0340b82e-9c61-45bd-a50b-f5691247a070").CheckExists = True ' Header: Updates und Downloads Check Exists
                GoTo WaitStart_3_Label
        End Select

        ' SE
        RaiseException("System Exception", "Download Header not found")

        ' W5
        WaitStart_3_Label:
        ' Wait 5 seconds for condition with 1 choice(s)
        Select Case True
            Case Application.Element("Button: Nach Updates suchen", "2a7db9d3-154c-404c-b610-5c3ae51ecb32").CheckExists = True ' Button: Nach Updates suchen Check Exists
                GoTo Navigate_3_Label
        End Select

        ' SE
        RaiseException("System Exception", "Nach Updates suchen Button not found")

        ' Click Nach Updates suchen
        Navigate_3_Label:
        Application.Element("Button: Nach Updates suchen", "2a7db9d3-154c-404c-b610-5c3ae51ecb32").UIAButtonPress()

        ' W120
        ' Wait 120 seconds for condition with 2 choice(s)
        Select Case True
            Case Application.Element("Button: Die Überprüfung auf Updates wurde abgeschlossen.", "397fb78c-8c08-4dc0-987c-3ca33d3762a4").CheckExists = True ' Button: Die Überprüfung auf Updates wurde abgeschlossen. Check Exists
                GoTo WaitStart_5_Label
            Case Application.Element("Button: Nach Updates suchen", "2a7db9d3-154c-404c-b610-5c3ae51ecb32").CheckExists = True ' Button: Nach Updates suchen Check Exists
                GoTo WaitStart_5_Label
        End Select

        ' SE
        RaiseException("System Exception", "Download Header not found")

        ' W2
        WaitStart_5_Label:
        ' Wait 2 seconds for condition with 1 choice(s)
        Select Case True
            Case Application.Element("Button: Nach Updates suchen", "2a7db9d3-154c-404c-b610-5c3ae51ecb32").CheckExists = True ' Button: Nach Updates suchen Check Exists
                GoTo Navigate_4_Label
        End Select

        ' Note1
        ' No Updates
        GoTo End_Start_Updates_Label

        ' Click Update All
        Navigate_4_Label:
        ' Navigate: No steps defined
        
        ' W5
        WaitStart_6_Label:
        ' Wait 5 seconds for condition with 1 choice(s)
        Select Case True
            Case Application.Element("Button: Nach Updates suchen", "2a7db9d3-154c-404c-b610-5c3ae51ecb32").CheckExists = True ' Button: Nach Updates suchen Check Exists
                GoTo End_Start_Updates_Label
        End Select

        ' SE
        RaiseException("System Exception", "Nach Updates suchen Button not found")

        ' Note2
        ' TODO
        GoTo WaitStart_5_Label

        ' Note2
        ' TODO
        GoTo Navigate_4_Label

        ' Note2
        ' TODO
        GoTo WaitStart_6_Label

        End_Start_Updates_Label:

    End Sub

    ''' <summary>
    ''' BluePrism page: Wait_Updates_Finished
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
