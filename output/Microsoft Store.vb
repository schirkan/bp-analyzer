' Generated from BluePrism object: Microsoft Store
' Version: 1.0
' Generated: 2026-02-27 20:40:11
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
Imports System.Drawing

''' <summary>
''' BluePrism object: Microsoft Store
''' </summary>
Public Class Microsoft_Store
    Inherits BP_Base

    #Region "Singleton Instance"

    ''' <summary>
    ''' Shared singleton instance
    ''' </summary>
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
    ''' Constructor - initialization code from stages without subsheet
    ''' </summary>
    Public Sub New()

        GoTo End_00af6ff0_0baa_4db7_8388_c9a31823062d_Label
        End_00af6ff0_0baa_4db7_8388_c9a31823062d_Label:

    End Sub

    ''' <summary>
    ''' Destructor (CleanUp) - called when object is disposed
    ''' </summary>
    Protected Overrides Sub Finalize()

        GoTo End_f033793f_07b2_4874_ab79_167fa719d87e_Label
        End_f033793f_07b2_4874_ab79_167fa719d87e_Label:

    End Sub

    ''' <summary>
    ''' BluePrism method: Launch
    ''' </summary>
    Public Sub Launch()

        ' Local variables
        Dim FilePath As String

        ' Initialize local variables with alwaysinit
        If FilePath Is Nothing OrElse FilePath.Equals("") Then
            FilePath = "C:\Program Files\WindowsApps\Microsoft.WindowsStore_22512.1401.6.0_x64__8wekyb3d8bbwe\WinStore.App.exe"
        End If

        GoTo Action_4626cdba_27ab_40ea_af94_afb2dc3f2984_Label
        Action_4626cdba_27ab_40ea_af94_afb2dc3f2984_Label: ' Start Process
        Utility___Environment.Instance.Start_Process(Application:=FilePath, Arguments:=Arguments, Use_Shell:=Use_Shell, Process_ID:=Process_ID, Process_Name:=Process_Name)
        GoTo Navigate_5a127fca_258d_4aed_9d17_ea586664d634_Label

        Navigate_5a127fca_258d_4aed_9d17_ea586664d634_Label: ' Attach
        ' Navigate: UI automation
        ' TODO: Implement
        GoTo WaitStart_703731aa_fa0b_43e4_8e67_71c3256583c4_Label

        WaitStart_703731aa_fa0b_43e4_8e67_71c3256583c4_Label: ' W5
        ' Wait: W5 (Type: WaitStart)
        ' Wait 5 seconds for condition with 1 choice(s)
        Select Case True
            Case:  = True
                GoTo End_05774252_de6b_4703_9edf_5bd04116cf21_Label
        End Select

        Exception_427cad4f_4758_4510_801d_acbd6542f39b_Label: ' SE
        RaiseException("System Exception", "Main Window not found")

        End_05774252_de6b_4703_9edf_5bd04116cf21_Label:

    End Sub

    ''' <summary>
    ''' BluePrism method: Terminate
    ''' </summary>
    Public Sub Terminate()

        GoTo Navigate_329eda4b_fa88_4114_88d7_254fdb819343_Label
        Navigate_329eda4b_fa88_4114_88d7_254fdb819343_Label: ' Terminate
        ' Navigate: UI automation
        ' TODO: Implement
        GoTo End_e0c54172_5ab9_495a_b395_7f61b2c1cf38_Label

        End_e0c54172_5ab9_495a_b395_7f61b2c1cf38_Label:

    End Sub

    ''' <summary>
    ''' BluePrism method: Start_Updates
    ''' </summary>
    Public Sub Start_Updates()

        GoTo WaitStart_dd4bc616_c65b_461f_b482_c40cbf80e967_Label
        WaitStart_dd4bc616_c65b_461f_b482_c40cbf80e967_Label: ' W5
        ' Wait: W5 (Type: WaitStart)
        ' Wait 5 seconds for condition with 1 choice(s)
        Select Case True
            Case:  = True
                GoTo Navigate_f9be3a31_fdf8_4827_8df3_8ab0fbd5d302_Label
        End Select

        Exception_9cb9d9fb_2550_4008_8323_a49fc050de05_Label: ' SE
        RaiseException("System Exception", "Download Menu not found")

        Navigate_f9be3a31_fdf8_4827_8df3_8ab0fbd5d302_Label: ' GoTo Downloads
        ' Navigate: UI automation
        ' TODO: Implement
        GoTo WaitStart_d07aa237_9fbd_4313_a6a4_d8d82bbf1420_Label

        WaitStart_d07aa237_9fbd_4313_a6a4_d8d82bbf1420_Label: ' W5
        ' Wait: W5 (Type: WaitStart)
        ' Wait 5 seconds for condition with 1 choice(s)
        Select Case True
            Case:  = True
                GoTo WaitStart_473090d3_a621_4fc2_8436_25fc69a82067_Label
        End Select

        Exception_e8b3fd9d_bd7e_4f39_aee3_2bc8c8b1239b_Label: ' SE
        RaiseException("System Exception", "Download Header not found")

        Navigate_fe0746e7_430a_43f1_8c72_9c40763f31bb_Label: ' Click Nach Updates suchen
        ' Navigate: UI automation
        ' TODO: Implement
        GoTo WaitStart_a61f62ae_7993_470f_8d1f_cffdcd5bea5b_Label

        WaitStart_473090d3_a621_4fc2_8436_25fc69a82067_Label: ' W5
        ' Wait: W5 (Type: WaitStart)
        ' Wait 5 seconds for condition with 1 choice(s)
        Select Case True
            Case:  = True
                GoTo Navigate_fe0746e7_430a_43f1_8c72_9c40763f31bb_Label
        End Select

        Exception_ce2d7bf4_fff3_4d86_a6f3_01dd3c9742db_Label: ' SE
        RaiseException("System Exception", "Nach Updates suchen Button not found")

        WaitStart_a61f62ae_7993_470f_8d1f_cffdcd5bea5b_Label: ' W120
        ' Wait: W120 (Type: WaitStart)
        ' Wait 120 seconds for condition with 2 choice(s)
        Select Case True
            Case:  = True
                GoTo WaitStart_cf9ecce9_11c0_48ca_a533_6c59e4ae1d17_Label
            Case:  = True
                GoTo WaitStart_cf9ecce9_11c0_48ca_a533_6c59e4ae1d17_Label
        End Select

        Exception_8c5feb38_8094_4c3f_a292_ac967710a2e8_Label: ' SE
        RaiseException("System Exception", "Download Header not found")

        Navigate_7a1602d1_74e3_482c_9a3f_58d3e85baf51_Label: ' Click Update All
        ' Navigate: UI automation
        ' TODO: Implement
        GoTo WaitStart_a027ef03_3f55_4ae4_ae16_cee7f7dc29f1_Label

        WaitStart_cf9ecce9_11c0_48ca_a533_6c59e4ae1d17_Label: ' W2
        ' Wait: W2 (Type: WaitStart)
        ' Wait 2 seconds for condition with 1 choice(s)
        Select Case True
            Case:  = True
                GoTo Navigate_7a1602d1_74e3_482c_9a3f_58d3e85baf51_Label
        End Select

        WaitStart_a027ef03_3f55_4ae4_ae16_cee7f7dc29f1_Label: ' W5
        ' Wait: W5 (Type: WaitStart)
        ' Wait 5 seconds for condition with 1 choice(s)
        Select Case True
            Case:  = True
                GoTo End_23ba7d3f_cc9b_4bf6_bb28_545408febcae_Label
        End Select

        Exception_1bcc03a0_82b4_44d7_b34f_9edda37ef8ac_Label: ' SE
        RaiseException("System Exception", "Nach Updates suchen Button not found")

        Note_95342886_536d_4cc5_93cf_9fb9f009ef0b_Label: ' Note1
        ' No Updates
        GoTo End_23ba7d3f_cc9b_4bf6_bb28_545408febcae_Label

        Note_c7f1bf75_f44c_4526_9fa1_d99452938648_Label: ' Note2
        ' TODO
        GoTo WaitStart_cf9ecce9_11c0_48ca_a533_6c59e4ae1d17_Label

        Note_5c9a6e81_d341_4ba5_a12c_fadb62049062_Label: ' Note2
        ' TODO
        GoTo Navigate_7a1602d1_74e3_482c_9a3f_58d3e85baf51_Label

        Note_ff41673e_6a98_4243_94d4_a816f5a50240_Label: ' Note2
        ' TODO
        GoTo WaitStart_a027ef03_3f55_4ae4_ae16_cee7f7dc29f1_Label

        End_23ba7d3f_cc9b_4bf6_bb28_545408febcae_Label:

    End Sub

    ''' <summary>
    ''' BluePrism method: Wait_Updates_Finished
    ''' </summary>
    Public Sub Wait_Updates_Finished()

        GoTo Note_83e2dc68_fc7d_48c2_94d3_066baf1e9d29_Label
        Note_83e2dc68_fc7d_48c2_94d3_066baf1e9d29_Label: ' Note2
        ' TODO
        GoTo End_e4e938c4_17f2_4c92_9fa0_491ffd390327_Label

        End_e4e938c4_17f2_4c92_9fa0_491ffd390327_Label:

    End Sub


    #End Region

End Class
