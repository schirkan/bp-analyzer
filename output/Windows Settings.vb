' Generated from BluePrism object: Windows Settings
' Version: 1.0
' Generated: 2026-03-10 19:14:43

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Data
Imports System.Drawing

''' <summary>
''' BluePrism object: Windows Settings
''' </summary>
Public Class Windows_Settings
    Inherits BP_Base

    #Region "Singleton Instance"

    Private Shared ReadOnly _lazyInstance As New Lazy(Of Windows_Settings)(Function() New Windows_Settings())

    Public Shared ReadOnly Property Instance As Windows_Settings
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
        FilePath = "ms-settings:windowsupdate"

        On Error GoTo Recover_Label

        ' Start Process
        On Error GoTo Recover_Label
        Utility___Environment.Instance.Start_Process(Application:=FilePath)
        
        ' Attach
        Navigate_Label:
        On Error GoTo Recover_Label
        Application.Element("Windows Settings").AttachApplication()

        ' W5
        On Error GoTo Recover_Label
        ' Wait 5 seconds for condition with 1 choice(s)
        Select Case True
            Case Application.Element("Main Window", "4e5490be-f4da-4f82-ace1-cbf647c8b4e6").CheckExists = True ' Main Window Check Exists
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
    ''' Page: Terminate
    ''' </summary>
    Public Sub Terminate()

        ' Terminate
        Application.Element("Windows Settings").Terminate()

    End Sub

    ''' <summary>
    ''' Page: Start_Updates
    ''' </summary>
    Public Sub Start_Updates()

        ' W5
        ' Wait 5 seconds for condition with 2 choice(s)
        Select Case True
            Case Application.Element("Button: Nach Updates suchen", "b026cc78-c68f-4a73-b198-bba0c63c8ef2").CheckExists = True ' Button: Nach Updates suchen Check Exists
                GoTo Navigate_3_Label
            Case Application.Element("Label: Es wird nach Updates gesucht...", "a98756de-6ac0-4a36-a601-a27ea429ac4f").CheckExists = True ' Label: Es wird nach Updates gesucht... Check Exists
                GoTo End_Start_Updates_Label
        End Select

        ' SE
        RaiseException("System Exception", "Download Menu not found")

        ' Click Nach Updates suchen
        Navigate_3_Label:
        Application.Element("Button: Nach Updates suchen").UIAButtonPress()
        
        End_Start_Updates_Label:

    End Sub

    ''' <summary>
    ''' Page: Wait_Updates_Finished
    ''' </summary>
    Public Sub Wait_Updates_Finished()

        ' W120
        ' Wait 120 seconds for condition with 1 choice(s)
        Select Case True
            Case Application.Element("Label: Es wird nach Updates gesucht...", "a98756de-6ac0-4a36-a601-a27ea429ac4f").CheckExists = False ' Label: Es wird nach Updates gesucht... Check Exists
                GoTo WaitStart_4_Label
        End Select

        ' SE
        RaiseException("System Exception", "Download Header not found")

        ' W120
        WaitStart_4_Label:
        ' Wait 120 seconds for condition with 1 choice(s)
        Select Case True
            Case Application.Element("Label: Sie sind auf dem neuesten Stand.", "d4098baf-b2ed-491f-a65a-7dc1ece7b4cd").CheckExists = True ' Label: Sie sind auf dem neuesten Stand. Check Exists
                GoTo End_Wait_Updates_Finished_Label
        End Select

        ' SE
        RaiseException("System Exception", "Download Header not found")

        ' Note1
        ' Todo

        End_Wait_Updates_Finished_Label:

    End Sub

    #End Region

    #Region "App Model"

    Protected Application As Object

    #End Region

End Class
