' Generated from BluePrism object: Windows Settings
' Version: 1.0
' Generated: 2026-03-04 20:17:59

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

    ''' <summary>
    ''' Shared singleton instance
    ''' </summary>
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
    ''' Constructor - initialization code from stages without subsheet
    ''' </summary>
    Public Sub New()

    End Sub

    ''' <summary>
    ''' Destructor (CleanUp) - called when object is disposed
    ''' </summary>
    Protected Overrides Sub Finalize()

    End Sub

    ''' <summary>
    ''' BluePrism method: Launch
    ''' </summary>
    Public Sub Launch()

        ' Local variables
        Dim FilePath As String

        ' Initialize variables with initialvalue
        If FilePath Is Nothing Then
            FilePath = "ms-settings:windowsupdate"
        End If

        ' Start Process
        On Error GoTo Recover_4ffeef72_e362_489a_a654_925ac2f298f8_Label
        Utility___Environment.Instance.Start_Process(Application:=FilePath, Arguments:=Arguments, Use_Shell:=Use_Shell, Process_ID:=Process_ID, Process_Name:=Process_Name)
        Navigate_1e01d250_1eb1_4733_b192_d31129adc9ab_Label: ' Attach
        On Error GoTo Recover_4ffeef72_e362_489a_a654_925ac2f298f8_Label
        ' Navigate: UI automation
        ' TODO: Implement
        ' W5
        On Error GoTo Recover_4ffeef72_e362_489a_a654_925ac2f298f8_Label
        ' Wait: W5 (Type: WaitStart)
        ' Wait 5 seconds for condition with 1 choice(s)
        Select Case True
            Case Application.Element("4e5490be-f4da-4f82-ace1-cbf647c8b4e6").CheckExists = True ' Main Window Check Exists
                GoTo End_6aed1a70_b15a_4cce_a57c_e4afa40e3fe9_Label
            Case Else
                GoTo Exception_a633f109_5378_49d5_b8a5_8183c121fcd1_Label
        End Select

        Exception_a633f109_5378_49d5_b8a5_8183c121fcd1_Label: ' SE
        On Error GoTo Recover_4ffeef72_e362_489a_a654_925ac2f298f8_Label
        RaiseException("System Exception", "Main Window not found")

        Recover_4ffeef72_e362_489a_a654_925ac2f298f8_Label: ' Recover
        StoreException()
        ' Resume
        ClearException()
        Resume Navigate_1e01d250_1eb1_4733_b192_d31129adc9ab_Label

        End_6aed1a70_b15a_4cce_a57c_e4afa40e3fe9_Label:

    End Sub

    ''' <summary>
    ''' BluePrism method: Terminate
    ''' </summary>
    Public Sub Terminate()

        ' Terminate
        ' Navigate: UI automation
        ' TODO: Implement

    End Sub

    ''' <summary>
    ''' BluePrism method: Start_Updates
    ''' </summary>
    Public Sub Start_Updates()

        ' W5
        ' Wait: W5 (Type: WaitStart)
        ' Wait 5 seconds for condition with 2 choice(s)
        Select Case True
            Case Application.Element("b026cc78-c68f-4a73-b198-bba0c63c8ef2").CheckExists = True ' Button: Nach Updates suchen Check Exists
                GoTo Navigate_b53b3ace_8c40_4a32_8015_49a288f7a245_Label
            Case Application.Element("a98756de-6ac0-4a36-a601-a27ea429ac4f").CheckExists = True ' Label: Es wird nach Updates gesucht... Check Exists
                GoTo End_1c4fc2e7_022b_4fb2_9bef_f30a7cb3c9c3_Label
            Case Else
                GoTo Exception_7bb9abfd_827f_4596_9d92_1633bf69719f_Label
        End Select

        Exception_7bb9abfd_827f_4596_9d92_1633bf69719f_Label: ' SE
        RaiseException("System Exception", "Download Menu not found")

        Navigate_b53b3ace_8c40_4a32_8015_49a288f7a245_Label: ' Click Nach Updates suchen
        ' Navigate: UI automation
        ' TODO: Implement
        End_1c4fc2e7_022b_4fb2_9bef_f30a7cb3c9c3_Label:

    End Sub

    ''' <summary>
    ''' BluePrism method: Wait_Updates_Finished
    ''' </summary>
    Public Sub Wait_Updates_Finished()

        ' W120
        ' Wait: W120 (Type: WaitStart)
        ' Wait 120 seconds for condition with 1 choice(s)
        Select Case True
            Case Application.Element("a98756de-6ac0-4a36-a601-a27ea429ac4f").CheckExists = False ' Label: Es wird nach Updates gesucht... Check Exists
                GoTo WaitStart_ddd70967_7e0f_4c5f_afb5_6f6041fdacab_Label
            Case Else
                GoTo Exception_593a7e45_5803_476b_83c7_2a6d2cd7f909_Label
        End Select

        Exception_593a7e45_5803_476b_83c7_2a6d2cd7f909_Label: ' SE
        RaiseException("System Exception", "Download Header not found")

        WaitStart_ddd70967_7e0f_4c5f_afb5_6f6041fdacab_Label: ' W120
        ' Wait: W120 (Type: WaitStart)
        ' Wait 120 seconds for condition with 1 choice(s)
        Select Case True
            Case Application.Element("d4098baf-b2ed-491f-a65a-7dc1ece7b4cd").CheckExists = True ' Label: Sie sind auf dem neuesten Stand. Check Exists
                GoTo End_74264245_4139_4e07_9f3b_ddeb9ab8850a_Label
            Case Else
                GoTo Exception_a4d98b2d_573f_472c_806f_8a67b73efeec_Label
        End Select

        Exception_a4d98b2d_573f_472c_806f_8a67b73efeec_Label: ' SE
        RaiseException("System Exception", "Download Header not found")

        ' Note1
        ' Todo

        End_74264245_4139_4e07_9f3b_ddeb9ab8850a_Label:

    End Sub

    #End Region

End Class
