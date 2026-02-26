' Generated from BluePrism object: Microsoft Store
' Version: 1.0
' Generated: 2026-02-23 23:28:37
' 
' 

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
''' BluePrism object: Microsoft Store
''' </summary>
Public Class Microsoft_Store

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
            GoTo End_00af6ff0_0baa_4db7_8388_c9a31823062d_Label
        ' Stage: End (End)
End_00af6ff0_0baa_4db7_8388_c9a31823062d_Label:

    End Sub

    ''' <summary>
    ''' Destructor (CleanUp) - called when object is disposed
    ''' </summary>
    Protected Overrides Sub Finalize()

        ' Stage: Start (Start)
            GoTo End_f033793f_07b2_4874_ab79_167fa719d87e_Label
        ' SubSheet: 
        ' Stage: End (End)
End_f033793f_07b2_4874_ab79_167fa719d87e_Label:

    End Sub

    ''' <summary>
    ''' BluePrism subsheet: Launch (Type: Normal)
    ''' </summary>
    Public Sub Launch()
        ' Local variables
        Dim FilePath As String = "C:\Program Files\WindowsApps\Microsoft.WindowsStore_22512.1401.6.0_x64__8wekyb3d8bbwe\WinStore.App.exe"


        ' Stage: Start (Start)
            GoTo Start_Process_4626cdba_27ab_40ea_af94_afb2dc3f2984_Label
        ' Stage: Start Process (Action)
Start_Process_4626cdba_27ab_40ea_af94_afb2dc3f2984_Label:
            ' Action: Start Process
            ' Calling: Utility - Environment.Start Process()
            ' TODO: Implement action call
            GoTo Attach_5a127fca_258d_4aed_9d17_ea586664d634_Label

        ' Stage: Attach (Navigate)
Attach_5a127fca_258d_4aed_9d17_ea586664d634_Label:
            ' Navigate: UI automation
            ' TODO: Implement
            GoTo W5_703731aa_fa0b_43e4_8e67_71c3256583c4_Label

        ' Stage: W5 (WaitStart)
W5_703731aa_fa0b_43e4_8e67_71c3256583c4_Label:
            ' Wait: W5 (Type: WaitStart)
            ' Wait for condition with 1 choice(s)
            ' Select Case for wait conditions:
            ' Case:  = True
            '     GoTo End_05774252_de6b_4703_9edf_5bd04116cf21_Label
            ' Case Else (Default): WaitEnd [T], go to next stage
            '     GoTo SE_427cad4f_4758_4510_801d_acbd6542f39b_Label

        ' SubSheet: 
        ' Stage: Const (Block)
Const_f4cca756_8086_4d07_87fd_b63ea6be6736_Label:
            ' Block: Const

        ' Stage: SE (Exception)
SE_427cad4f_4758_4510_801d_acbd6542f39b_Label:
            ' Throw Exception: System Exception
            ' Detail: "Main Window not found"
            Throw New Exception("Main Window not found")

        ' Stage: End (End)
End_05774252_de6b_4703_9edf_5bd04116cf21_Label:

    End Sub

    ''' <summary>
    ''' BluePrism subsheet: Terminate (Type: Normal)
    ''' </summary>
    Public Sub Terminate()

        ' Stage: Start (Start)
            GoTo Terminate_329eda4b_fa88_4114_88d7_254fdb819343_Label
        ' Stage: Terminate (Navigate)
Terminate_329eda4b_fa88_4114_88d7_254fdb819343_Label:
            ' Navigate: UI automation
            ' TODO: Implement
            GoTo End_e0c54172_5ab9_495a_b395_7f61b2c1cf38_Label

        ' SubSheet: 
        ' Stage: End (End)
End_e0c54172_5ab9_495a_b395_7f61b2c1cf38_Label:

    End Sub

    ''' <summary>
    ''' BluePrism subsheet: Start Updates (Type: Normal)
    ''' </summary>
    Public Sub Start_Updates()

        ' Stage: Start (Start)
            GoTo W5_dd4bc616_c65b_461f_b482_c40cbf80e967_Label
        ' Stage: W5 (WaitStart)
W5_dd4bc616_c65b_461f_b482_c40cbf80e967_Label:
            ' Wait: W5 (Type: WaitStart)
            ' Wait for condition with 1 choice(s)
            ' Select Case for wait conditions:
            ' Case:  = True
            '     GoTo GoTo_Downloads_f9be3a31_fdf8_4827_8df3_8ab0fbd5d302_Label
            ' Case Else (Default): WaitEnd [T], go to next stage
            '     GoTo SE_9cb9d9fb_2550_4008_8323_a49fc050de05_Label

        ' SubSheet: 
        ' Stage: SE (Exception)
SE_9cb9d9fb_2550_4008_8323_a49fc050de05_Label:
            ' Throw Exception: System Exception
            ' Detail: "Download Menu not found"
            Throw New Exception("Download Menu not found")

        ' Stage: GoTo Downloads (Navigate)
GoTo_Downloads_f9be3a31_fdf8_4827_8df3_8ab0fbd5d302_Label:
            ' Navigate: UI automation
            ' TODO: Implement
            GoTo W5_d07aa237_9fbd_4313_a6a4_d8d82bbf1420_Label

        ' Stage: W5 (WaitStart)
W5_d07aa237_9fbd_4313_a6a4_d8d82bbf1420_Label:
            ' Wait: W5 (Type: WaitStart)
            ' Wait for condition with 1 choice(s)
            ' Select Case for wait conditions:
            ' Case:  = True
            '     GoTo W5_473090d3_a621_4fc2_8436_25fc69a82067_Label
            ' Case Else (Default): WaitEnd [T], go to next stage
            '     GoTo SE_e8b3fd9d_bd7e_4f39_aee3_2bc8c8b1239b_Label

        ' Stage: SE (Exception)
SE_e8b3fd9d_bd7e_4f39_aee3_2bc8c8b1239b_Label:
            ' Throw Exception: System Exception
            ' Detail: "Download Header not found"
            Throw New Exception("Download Header not found")

        ' Stage: Click Nach Updates suchen (Navigate)
Click_Nach_Updates_suchen_fe0746e7_430a_43f1_8c72_9c40763f31bb_Label:
            ' Navigate: UI automation
            ' TODO: Implement
            GoTo W120_a61f62ae_7993_470f_8d1f_cffdcd5bea5b_Label

        ' Stage: W5 (WaitStart)
W5_473090d3_a621_4fc2_8436_25fc69a82067_Label:
            ' Wait: W5 (Type: WaitStart)
            ' Wait for condition with 1 choice(s)
            ' Select Case for wait conditions:
            ' Case:  = True
            '     GoTo Click_Nach_Updates_suchen_fe0746e7_430a_43f1_8c72_9c40763f31bb_Label
            ' Case Else (Default): WaitEnd [T], go to next stage
            '     GoTo SE_ce2d7bf4_fff3_4d86_a6f3_01dd3c9742db_Label

        ' Stage: SE (Exception)
SE_ce2d7bf4_fff3_4d86_a6f3_01dd3c9742db_Label:
            ' Throw Exception: System Exception
            ' Detail: "Nach Updates suchen Button not found"
            Throw New Exception("Nach Updates suchen Button not found")

        ' Stage: W120 (WaitStart)
W120_a61f62ae_7993_470f_8d1f_cffdcd5bea5b_Label:
            ' Wait: W120 (Type: WaitStart)
            ' Wait for condition with 2 choice(s)
            ' Select Case for wait conditions:
            ' Case:  = True
            '     GoTo W2_cf9ecce9_11c0_48ca_a533_6c59e4ae1d17_Label
            ' Case:  = True
            '     GoTo W2_cf9ecce9_11c0_48ca_a533_6c59e4ae1d17_Label
            ' Case Else (Default): WaitEnd [T], go to next stage
            '     GoTo SE_8c5feb38_8094_4c3f_a292_ac967710a2e8_Label

        ' Stage: SE (Exception)
SE_8c5feb38_8094_4c3f_a292_ac967710a2e8_Label:
            ' Throw Exception: System Exception
            ' Detail: "Download Header not found"
            Throw New Exception("Download Header not found")

        ' Stage: Click Update All (Navigate)
Click_Update_All_7a1602d1_74e3_482c_9a3f_58d3e85baf51_Label:
            ' Navigate: UI automation
            ' TODO: Implement
            GoTo W5_a027ef03_3f55_4ae4_ae16_cee7f7dc29f1_Label

        ' Stage: W2 (WaitStart)
W2_cf9ecce9_11c0_48ca_a533_6c59e4ae1d17_Label:
            ' Wait: W2 (Type: WaitStart)
            ' Wait for condition with 1 choice(s)
            ' Select Case for wait conditions:
            ' Case:  = True
            '     GoTo Click_Update_All_7a1602d1_74e3_482c_9a3f_58d3e85baf51_Label
            ' Case Else (Default): WaitEnd [T], go to next stage
            '     GoTo Note1_95342886_536d_4cc5_93cf_9fb9f009ef0b_Label

        ' Stage: W5 (WaitStart)
W5_a027ef03_3f55_4ae4_ae16_cee7f7dc29f1_Label:
            ' Wait: W5 (Type: WaitStart)
            ' Wait for condition with 1 choice(s)
            ' Select Case for wait conditions:
            ' Case:  = True
            '     GoTo End_23ba7d3f_cc9b_4bf6_bb28_545408febcae_Label
            ' Case Else (Default): WaitEnd [T], go to next stage
            '     GoTo SE_1bcc03a0_82b4_44d7_b34f_9edda37ef8ac_Label

        ' Stage: SE (Exception)
SE_1bcc03a0_82b4_44d7_b34f_9edda37ef8ac_Label:
            ' Throw Exception: System Exception
            ' Detail: "Nach Updates suchen Button not found"
            Throw New Exception("Nach Updates suchen Button not found")

        ' Stage: Note1 (Note)
Note1_95342886_536d_4cc5_93cf_9fb9f009ef0b_Label:
            ' Note: No Updates
            GoTo End_71865602_eb83_4093_98b0_64791f08cf36_Label

        ' Stage: Note2 (Note)
Note2_c7f1bf75_f44c_4526_9fa1_d99452938648_Label:
            ' Note: TODO
            GoTo W2_cf9ecce9_11c0_48ca_a533_6c59e4ae1d17_Label

        ' Stage: Note2 (Note)
Note2_5c9a6e81_d341_4ba5_a12c_fadb62049062_Label:
            ' Note: TODO
            GoTo Click_Update_All_7a1602d1_74e3_482c_9a3f_58d3e85baf51_Label

        ' Stage: Note2 (Note)
Note2_ff41673e_6a98_4243_94d4_a816f5a50240_Label:
            ' Note: TODO
            GoTo W5_a027ef03_3f55_4ae4_ae16_cee7f7dc29f1_Label

        ' Stage: End (End)
End_23ba7d3f_cc9b_4bf6_bb28_545408febcae_Label:

    End Sub

    ''' <summary>
    ''' BluePrism subsheet: Wait Updates Finished (Type: Normal)
    ''' </summary>
    Public Sub Wait_Updates_Finished()

        ' Stage: Start (Start)
            GoTo Note2_83e2dc68_fc7d_48c2_94d3_066baf1e9d29_Label
        ' Stage: Note2 (Note)
Note2_83e2dc68_fc7d_48c2_94d3_066baf1e9d29_Label:
            ' Note: TODO
            GoTo End_e4e938c4_17f2_4c92_9fa0_491ffd390327_Label

        ' SubSheet: 
        ' Stage: End (End)
End_e4e938c4_17f2_4c92_9fa0_491ffd390327_Label:

    End Sub


    #End Region

End Class
