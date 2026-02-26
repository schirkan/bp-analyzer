' Generated from BluePrism object: Windows Settings
' Version: 1.0
' Generated: 2026-02-26 23:12:56
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

            GoTo End_076175b6_1dbb_424c_9b83_b9c525bc6c8e_Label
End_076175b6_1dbb_424c_9b83_b9c525bc6c8e_Label:

    End Sub

    ''' <summary>
    ''' Destructor (CleanUp) - called when object is disposed
    ''' </summary>
    Protected Overrides Sub Finalize()

            GoTo End_12a1042d_0c0f_4a78_a8de_07dcb3acabba_Label
End_12a1042d_0c0f_4a78_a8de_07dcb3acabba_Label:

    End Sub

    ''' <summary>
    ''' BluePrism method: Launch
    ''' </summary>
    Public Sub Launch()

        ' Local variables
        Dim FilePath As String = "ms-settings:windowsupdate"

            GoTo Start_Process_e6083316_bbfc_449a_876a_f3a3ca77566b_Label
Start_Process_e6083316_bbfc_449a_876a_f3a3ca77566b_Label:
        ' Start Process (Action)
            ' Start Process
            ' Calling: Utility - Environment.Start Process()
            Utility___Environment.Start_Process(Application:=[FilePath], Arguments:=[Arguments], Use_Shell:=[Use Shell], Process_ID:=[Process ID], Process_Name:=[Process Name])
            GoTo Attach_1e01d250_1eb1_4733_b192_d31129adc9ab_Label

Attach_1e01d250_1eb1_4733_b192_d31129adc9ab_Label:
        ' Attach (Navigate)
            ' Navigate: UI automation
            ' TODO: Implement
            GoTo W5_dbd18af1_b077_4a8d_b57a_b93974da4696_Label

W5_dbd18af1_b077_4a8d_b57a_b93974da4696_Label:
        ' W5 (WaitStart)
            ' Wait: W5 (Type: WaitStart)
            ' Wait for condition with 1 choice(s)
            ' Select Case for wait conditions:
            ' Case:  = True
            '     GoTo End_6aed1a70_b15a_4cce_a57c_e4afa40e3fe9_Label

Const_058184bb_720b_403a_80b6_2621c821dd45_Label:
        ' Const (Block)
            ' Block: Const

SE_a633f109_5378_49d5_b8a5_8183c121fcd1_Label:
        ' SE (Exception)
            Throw New System_Exception("Main Window not found")

Recover_4ffeef72_e362_489a_a654_925ac2f298f8_Label:
        ' Recover (Recover)
            ' Recover from error
            GoTo Resume_22279869_9f1d_4c2a_b01d_a3d3405d3240_Label

Resume_22279869_9f1d_4c2a_b01d_a3d3405d3240_Label:
        ' Resume (Resume)
            ' Resume
            GoTo Attach_1e01d250_1eb1_4733_b192_d31129adc9ab_Label

Block1_6653c04e_1ac3_4573_9dd9_5291e9bf8c57_Label:
        ' Block1 (Block)
            ' Block: Block1

End_6aed1a70_b15a_4cce_a57c_e4afa40e3fe9_Label:

    End Sub

    ''' <summary>
    ''' BluePrism method: Terminate
    ''' </summary>
    Public Sub Terminate()

            GoTo Terminate_c0522690_c8cc_4709_8dc8_251d04f2b9e6_Label
Terminate_c0522690_c8cc_4709_8dc8_251d04f2b9e6_Label:
        ' Terminate (Navigate)
            ' Navigate: UI automation
            ' TODO: Implement
            GoTo End_c3e50599_76d3_41e6_aaca_7e038ba5b207_Label

End_c3e50599_76d3_41e6_aaca_7e038ba5b207_Label:

    End Sub

    ''' <summary>
    ''' BluePrism method: Start_Updates
    ''' </summary>
    Public Sub Start_Updates()

            GoTo W5_0da68cf3_200e_4ccf_95fa_0175699daf6d_Label
W5_0da68cf3_200e_4ccf_95fa_0175699daf6d_Label:
        ' W5 (WaitStart)
            ' Wait: W5 (Type: WaitStart)
            ' Wait for condition with 2 choice(s)
            ' Select Case for wait conditions:
            ' Case:  = True
            '     GoTo Click_Nach_Updates_suchen_b53b3ace_8c40_4a32_8015_49a288f7a245_Label
            ' Case:  = True
            '     GoTo End_1c4fc2e7_022b_4fb2_9bef_f30a7cb3c9c3_Label

SE_7bb9abfd_827f_4596_9d92_1633bf69719f_Label:
        ' SE (Exception)
            Throw New System_Exception("Download Menu not found")

Click_Nach_Updates_suchen_b53b3ace_8c40_4a32_8015_49a288f7a245_Label:
        ' Click Nach Updates suchen (Navigate)
            ' Navigate: UI automation
            ' TODO: Implement
            GoTo Anchor1_276d7232_e5be_40f5_9314_ff008b228bd8_Label

End_1c4fc2e7_022b_4fb2_9bef_f30a7cb3c9c3_Label:

    End Sub

    ''' <summary>
    ''' BluePrism method: Wait_Updates_Finished
    ''' </summary>
    Public Sub Wait_Updates_Finished()

            GoTo W120_fe44beea_7779_433a_b6ab_7ada27117a21_Label
W120_fe44beea_7779_433a_b6ab_7ada27117a21_Label:
        ' W120 (WaitStart)
            ' Wait: W120 (Type: WaitStart)
            ' Wait for condition with 1 choice(s)
            ' Select Case for wait conditions:
            ' Case:  = False
            '     GoTo W120_ddd70967_7e0f_4c5f_afb5_6f6041fdacab_Label

SE_593a7e45_5803_476b_83c7_2a6d2cd7f909_Label:
        ' SE (Exception)
            Throw New System_Exception("Download Header not found")

W120_ddd70967_7e0f_4c5f_afb5_6f6041fdacab_Label:
        ' W120 (WaitStart)
            ' Wait: W120 (Type: WaitStart)
            ' Wait for condition with 1 choice(s)
            ' Select Case for wait conditions:
            ' Case:  = True
            '     GoTo End_74264245_4139_4e07_9f3b_ddeb9ab8850a_Label

SE_a4d98b2d_573f_472c_806f_8a67b73efeec_Label:
        ' SE (Exception)
            Throw New System_Exception("Download Header not found")

Note1_94e2ca8f_1591_4135_9c8e_da6a38d1da07_Label:
        ' Note1 (Note)
            ' Note: Todo

End_74264245_4139_4e07_9f3b_ddeb9ab8850a_Label:

    End Sub


    #End Region

End Class
