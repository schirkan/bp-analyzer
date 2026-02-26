' Generated from BluePrism object: Windows Settings
' Version: 1.0
' Generated: 2026-02-23 23:28:37
' 
' References:
'   - System.dll
'   - System.Data.dll
'   - System.Xml.dll
'   - System.Drawing.dll
' 
' Imports:
'   - System
'   - System.Drawing
'   - System.Data

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Data

''' <summary>
''' BluePrism object: Windows Settings
''' </summary>
Public Class Windows_Settings

    #Region "Global Data Items"

    ' No global data items

    #End Region

    #Region "Methods"

    ''' <summary>
    ''' Constructor - initialization code from stages without subsheet
    ''' </summary>
    Public Sub New()
        ' Constructor body generated from BluePrism global stages

        ' Stage: Start (Start)
            GoTo End_076175b6_1dbb_424c_9b83_b9c525bc6c8e_Label
        ' Stage: End (End)
End_076175b6_1dbb_424c_9b83_b9c525bc6c8e_Label:

    End Sub

    ''' <summary>
    ''' Destructor (CleanUp) - called when object is disposed
    ''' </summary>
    Protected Overrides Sub Finalize()

        ' Stage: Start (Start)
            GoTo End_12a1042d_0c0f_4a78_a8de_07dcb3acabba_Label
        ' SubSheet: 
        ' Stage: End (End)
End_12a1042d_0c0f_4a78_a8de_07dcb3acabba_Label:

    End Sub

    ''' <summary>
    ''' BluePrism subsheet: Launch (Type: Normal)
    ''' </summary>
    Public Sub Launch()
        ' Local variables
        Dim FilePath As String = "ms-settings:windowsupdate"


        ' Stage: Start (Start)
            GoTo Start_Process_e6083316_bbfc_449a_876a_f3a3ca77566b_Label
        ' Stage: Start Process (Action)
Start_Process_e6083316_bbfc_449a_876a_f3a3ca77566b_Label:
            ' Action: Start Process
            ' Calling: Utility - Environment.Start Process()
            ' TODO: Implement action call
            GoTo Attach_1e01d250_1eb1_4733_b192_d31129adc9ab_Label

        ' Stage: Attach (Navigate)
Attach_1e01d250_1eb1_4733_b192_d31129adc9ab_Label:
            ' Navigate: UI automation
            ' TODO: Implement
            GoTo W5_dbd18af1_b077_4a8d_b57a_b93974da4696_Label

        ' Stage: W5 (WaitStart)
W5_dbd18af1_b077_4a8d_b57a_b93974da4696_Label:
            ' Wait: W5 (Type: WaitStart)
            ' Wait for condition with 1 choice(s)
            ' Select Case for wait conditions:
            ' Case:  = True
            '     GoTo End_6aed1a70_b15a_4cce_a57c_e4afa40e3fe9_Label
            ' Case Else (Default): WaitEnd [T], go to next stage
            '     GoTo SE_a633f109_5378_49d5_b8a5_8183c121fcd1_Label

        ' SubSheet: 
        ' Stage: Const (Block)
Const_058184bb_720b_403a_80b6_2621c821dd45_Label:
            ' Block: Const

        ' Stage: SE (Exception)
SE_a633f109_5378_49d5_b8a5_8183c121fcd1_Label:
            ' Throw Exception: System Exception
            ' Detail: "Main Window not found"
            Throw New Exception("Main Window not found")

        ' Stage: Recover (Recover)
Recover_4ffeef72_e362_489a_a654_925ac2f298f8_Label:
            ' Recover from error
            GoTo Resume_22279869_9f1d_4c2a_b01d_a3d3405d3240_Label

        ' Stage: Resume (Resume)
Resume_22279869_9f1d_4c2a_b01d_a3d3405d3240_Label:
            ' Resume
            GoTo Attach_1e01d250_1eb1_4733_b192_d31129adc9ab_Label

        ' Stage: Block1 (Block)
Block1_6653c04e_1ac3_4573_9dd9_5291e9bf8c57_Label:
            ' Block: Block1

        ' Stage: End (End)
End_6aed1a70_b15a_4cce_a57c_e4afa40e3fe9_Label:

    End Sub

    ''' <summary>
    ''' BluePrism subsheet: Terminate (Type: Normal)
    ''' </summary>
    Public Sub Terminate()

        ' Stage: Start (Start)
            GoTo Terminate_c0522690_c8cc_4709_8dc8_251d04f2b9e6_Label
        ' Stage: Terminate (Navigate)
Terminate_c0522690_c8cc_4709_8dc8_251d04f2b9e6_Label:
            ' Navigate: UI automation
            ' TODO: Implement
            GoTo End_c3e50599_76d3_41e6_aaca_7e038ba5b207_Label

        ' SubSheet: 
        ' Stage: End (End)
End_c3e50599_76d3_41e6_aaca_7e038ba5b207_Label:

    End Sub

    ''' <summary>
    ''' BluePrism subsheet: Start Updates (Type: Normal)
    ''' </summary>
    Public Sub Start_Updates()

        ' Stage: Start (Start)
            GoTo W5_0da68cf3_200e_4ccf_95fa_0175699daf6d_Label
        ' Stage: W5 (WaitStart)
W5_0da68cf3_200e_4ccf_95fa_0175699daf6d_Label:
            ' Wait: W5 (Type: WaitStart)
            ' Wait for condition with 2 choice(s)
            ' Select Case for wait conditions:
            ' Case:  = True
            '     GoTo Click_Nach_Updates_suchen_b53b3ace_8c40_4a32_8015_49a288f7a245_Label
            ' Case:  = True
            '     GoTo End_1c4fc2e7_022b_4fb2_9bef_f30a7cb3c9c3_Label
            ' Case Else (Default): WaitEnd [T], go to next stage
            '     GoTo SE_7bb9abfd_827f_4596_9d92_1633bf69719f_Label

        ' SubSheet: 
        ' Stage: SE (Exception)
SE_7bb9abfd_827f_4596_9d92_1633bf69719f_Label:
            ' Throw Exception: System Exception
            ' Detail: "Download Menu not found"
            Throw New Exception("Download Menu not found")

        ' Stage: Click Nach Updates suchen (Navigate)
Click_Nach_Updates_suchen_b53b3ace_8c40_4a32_8015_49a288f7a245_Label:
            ' Navigate: UI automation
            ' TODO: Implement
            GoTo Anchor1_276d7232_e5be_40f5_9314_ff008b228bd8_Label

        ' Stage: End (End)
End_1c4fc2e7_022b_4fb2_9bef_f30a7cb3c9c3_Label:

    End Sub

    ''' <summary>
    ''' BluePrism subsheet: Wait Updates Finished (Type: Normal)
    ''' </summary>
    Public Sub Wait_Updates_Finished()

        ' Stage: Start (Start)
            GoTo W120_fe44beea_7779_433a_b6ab_7ada27117a21_Label
        ' Stage: W120 (WaitStart)
W120_fe44beea_7779_433a_b6ab_7ada27117a21_Label:
            ' Wait: W120 (Type: WaitStart)
            ' Wait for condition with 1 choice(s)
            ' Select Case for wait conditions:
            ' Case:  = False
            '     GoTo W120_ddd70967_7e0f_4c5f_afb5_6f6041fdacab_Label
            ' Case Else (Default): WaitEnd [T], go to next stage
            '     GoTo SE_593a7e45_5803_476b_83c7_2a6d2cd7f909_Label

        ' SubSheet: 
        ' Stage: SE (Exception)
SE_593a7e45_5803_476b_83c7_2a6d2cd7f909_Label:
            ' Throw Exception: System Exception
            ' Detail: "Download Header not found"
            Throw New Exception("Download Header not found")

        ' Stage: W120 (WaitStart)
W120_ddd70967_7e0f_4c5f_afb5_6f6041fdacab_Label:
            ' Wait: W120 (Type: WaitStart)
            ' Wait for condition with 1 choice(s)
            ' Select Case for wait conditions:
            ' Case:  = True
            '     GoTo End_74264245_4139_4e07_9f3b_ddeb9ab8850a_Label
            ' Case Else (Default): WaitEnd [T], go to next stage
            '     GoTo SE_a4d98b2d_573f_472c_806f_8a67b73efeec_Label

        ' Stage: SE (Exception)
SE_a4d98b2d_573f_472c_806f_8a67b73efeec_Label:
            ' Throw Exception: System Exception
            ' Detail: "Download Header not found"
            Throw New Exception("Download Header not found")

        ' Stage: Note1 (Note)
Note1_94e2ca8f_1591_4135_9c8e_da6a38d1da07_Label:
            ' Note: Todo

        ' Stage: End (End)
End_74264245_4139_4e07_9f3b_ddeb9ab8850a_Label:

    End Sub


    #End Region

End Class
