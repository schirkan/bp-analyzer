' Generated from BluePrism process: MP - System Update
' Version: 1.0
' Generated: 2026-02-23 23:28:37

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Data

''' <summary>
''' BluePrism process: MP - System Update
''' </summary>
Public Class MP___System_Update

    #Region "Global Data Items"

    ''' <summary>
    ''' Global data item: MyText (text)
    ''' </summary>
    Public MyText As String

    #End Region

    #Region "Methods"

    ''' <summary>
    ''' Main process method (stages without subsheet)
    ''' </summary>
    Public Sub Execute()

        ' Local variables
        Dim Char_Count As Decimal
        Dim MyText As String

        ' Stage: Start (Start)
            GoTo Launch_4539a66d_7d69_4673_b8bb_5efa1232b757_Label
        ' Stage: Launch (Action)
Launch_4539a66d_7d69_4673_b8bb_5efa1232b757_Label:
            ' Action: Launch
            ' Calling: Microsoft Store.Launch()
            ' TODO: Implement action call
            GoTo Start_Updates_f23e67c2_11c0_4346_a801_5a214e91b6ff_Label

        ' Stage: Start Updates (Action)
Start_Updates_f23e67c2_11c0_4346_a801_5a214e91b6ff_Label:
            ' Action: Start Updates
            ' Calling: Microsoft Store.Start Updates()
            ' TODO: Implement action call
            GoTo Launch_b3ccbd6d_cca2_45db_9cfc_219e49f755b8_Label

        ' Stage: Launch (Action)
Launch_b3ccbd6d_cca2_45db_9cfc_219e49f755b8_Label:
            ' Action: Launch
            ' Calling: Windows Settings.Launch()
            ' TODO: Implement action call
            GoTo Start_Updates_f3c341e4_9757_4782_9b2e_ae78d124d7d9_Label

        ' Stage: Start Updates (Action)
Start_Updates_f3c341e4_9757_4782_9b2e_ae78d124d7d9_Label:
            ' Action: Start Updates
            ' Calling: Windows Settings.Start Updates()
            ' TODO: Implement action call
            GoTo Wait_Updates_Finished_c1d50530_9cb0_4086_ac25_09bcddb32444_Label

        ' Stage: Wait Updates Finished (Action)
Wait_Updates_Finished_c1d50530_9cb0_4086_ac25_09bcddb32444_Label:
            ' Action: Wait Updates Finished
            ' Calling: Microsoft Store.Wait Updates Finished()
            ' TODO: Implement action call
            GoTo Terminate_d1d55292_946c_4fa5_8ba4_aab9481ffe7c_Label

        ' Stage: Terminate (Action)
Terminate_d1d55292_946c_4fa5_8ba4_aab9481ffe7c_Label:
            ' Action: Terminate
            ' Calling: Microsoft Store.Terminate()
            ' TODO: Implement action call
            GoTo Wait_Updates_Finished_617b589a_e24f_4b18_8b8b_696abab94bf7_Label

        ' Stage: Wait Updates Finished (Action)
Wait_Updates_Finished_617b589a_e24f_4b18_8b8b_696abab94bf7_Label:
            ' Action: Wait Updates Finished
            ' Calling: Windows Settings.Wait Updates Finished()
            ' TODO: Implement action call
            GoTo Terminate_54d85639_3e6d_487a_8b8e_f3e95427e046_Label

        ' Stage: Terminate (Action)
Terminate_54d85639_3e6d_487a_8b8e_f3e95427e046_Label:
            ' Action: Terminate
            ' Calling: Windows Settings.Terminate()
            ' TODO: Implement action call
            GoTo Call_Process_A_5eb89ba8_fa28_4939_b7d7_cc29a13cf616_Label

        ' Stage: Call Process A (Process)
Call_Process_A_5eb89ba8_fa28_4939_b7d7_cc29a13cf616_Label:
            ' Call Process: Call Process A
            ' Input: Name = [MyText]
            ' Output: Char Count
            ' TODO: Implement process call
            GoTo End_1f3bb070_109c_4bbc_babf_3ef6af6b6fa2_Label

        ' Stage: Microsoft Store (Block)
Microsoft_Store_89fe04be_b70b_4a0c_b626_198848989e11_Label:
            ' Block: Microsoft Store

        ' Stage: Windows Settings (Block)
Windows_Settings_b92f4d18_19da_4082_9514_30f3f3c2af4e_Label:
            ' Block: Windows Settings

        ' Stage: Microsoft Store (Block)
Microsoft_Store_6c0ce503_9c88_4482_8ea9_01b24f00b8d2_Label:
            ' Block: Microsoft Store

        ' Stage: Windows Settings (Block)
Windows_Settings_f36526d1_fdff_42e1_98f3_40d843bfb787_Label:
            ' Block: Windows Settings

        ' Stage: End (End)
End_1f3bb070_109c_4bbc_babf_3ef6af6b6fa2_Label:

    End Sub

    ''' <summary>
    ''' BluePrism subsheet: Dummy (Type: Normal)
    ''' </summary>
    Public Sub Dummy()

        ' Stage: Start (Start)
            GoTo Note1_39c900db_07c0_4a09_a8d6_e596ebacd1dc_Label
        ' Stage: Note1 (Note)
Note1_39c900db_07c0_4a09_a8d6_e596ebacd1dc_Label:
            ' Note: Nothing here
            GoTo End_5fa21eeb_0828_4758_8d6f_16361a445fa1_Label

        ' SubSheet: 
        ' Stage: End (End)
End_5fa21eeb_0828_4758_8d6f_16361a445fa1_Label:

    End Sub

    ''' <summary>
    ''' BluePrism subsheet: Variable Test (Type: Normal)
    ''' </summary>
    Public Sub Variable_Test()
        ' Local variables
        Dim Data1 As DateTime = DateTime.Parse("2026/02/01")
        Dim Data2 As DateTime = DateTime.Parse("2026-02-11 15:37:42Z")
        Dim Data3 As Boolean = False
        Dim Data4 As Boolean
        Dim Data5 As Decimal
        Dim Data6 As Decimal = 4.5
        Dim Data7 As String
        Dim Data8 As String
        Dim Data9 As String = "something"
        Dim Data10 As TimeSpan = TimeSpan.Parse("10:20:33")
        Dim Data11 As TimeSpan = TimeSpan.Parse("1.02:03:04")
        Dim MyToggle As Boolean = False
        Dim Coll1 As DataTable
        Dim Coll2 As DataTable
        Dim Coll3 As DataTable


        ' Stage: Start (Start)
            GoTo MyToggle__4a05cdbe_f075_4c4f_972b_7e63880a1bb6_Label
        ' Stage: MyToggle? (Decision)
MyToggle__4a05cdbe_f075_4c4f_972b_7e63880a1bb6_Label:
            ' Decision: If [MyToggle] Then
                GoTo Toggle_boolean_value_da6d4891_f555_46b1_b752_73b463373de8_Label
            Else
                GoTo Multi1_acac267a_d52f_473a_8266_3116177d865f_Label
            End If

        ' Stage: Toggle boolean value (Calculation)
Toggle_boolean_value_da6d4891_f555_46b1_b752_73b463373de8_Label:
            ' Calculation: MyToggle = [MyToggle] = False
            GoTo Multi1_acac267a_d52f_473a_8266_3116177d865f_Label

        ' Stage: Multi1 (MultipleCalculation)
Multi1_acac267a_d52f_473a_8266_3116177d865f_Label:
            ' Data5 = 123
            ' Data6 = 7.8
            ' Data8 = "tttt"
            GoTo End_71b0a2dc_32e2_4505_aff3_7368fe044655_Label

        ' SubSheet: 
        ' Stage: End (End)
End_71b0a2dc_32e2_4505_aff3_7368fe044655_Label:

    End Sub


    #End Region

End Class
