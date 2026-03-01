' Generated from BluePrism process: MP - System Update
' Version: 1.0
' Generated: 2026-03-01 15:17:44

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Data

''' <summary>
''' BluePrism process: MP - System Update
''' </summary>
Public Class MP___System_Update
    Inherits BP_Base

    #Region "Singleton Instance"

    ''' <summary>
    ''' Shared singleton instance
    ''' </summary>
    Private Shared ReadOnly _lazyInstance As New Lazy(Of MP___System_Update)(Function() New MP___System_Update())

    Public Shared ReadOnly Property Instance As MP___System_Update
        Get
            Return _lazyInstance.Value
        End Get
    End Property

    #End Region

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
    Public Sub Main()

        ' Local variables
        Dim Char_Count As Decimal
        Dim MyText As String

        ' Initialize local variables with alwaysinit

        GoTo Action_4539a66d_7d69_4673_b8bb_5efa1232b757_Label
        Action_4539a66d_7d69_4673_b8bb_5efa1232b757_Label: ' Launch
        Microsoft_Store.Instance.Launch()
        GoTo Action_f23e67c2_11c0_4346_a801_5a214e91b6ff_Label

        Action_f23e67c2_11c0_4346_a801_5a214e91b6ff_Label: ' Start Updates
        Microsoft_Store.Instance.Start_Updates()
        GoTo Action_b3ccbd6d_cca2_45db_9cfc_219e49f755b8_Label

        Action_b3ccbd6d_cca2_45db_9cfc_219e49f755b8_Label: ' Launch
        Windows_Settings.Instance.Launch()
        GoTo Action_f3c341e4_9757_4782_9b2e_ae78d124d7d9_Label

        Action_f3c341e4_9757_4782_9b2e_ae78d124d7d9_Label: ' Start Updates
        Windows_Settings.Instance.Start_Updates()
        GoTo Action_c1d50530_9cb0_4086_ac25_09bcddb32444_Label

        Action_c1d50530_9cb0_4086_ac25_09bcddb32444_Label: ' Wait Updates Finished
        Microsoft_Store.Instance.Wait_Updates_Finished()
        GoTo Action_d1d55292_946c_4fa5_8ba4_aab9481ffe7c_Label

        Action_d1d55292_946c_4fa5_8ba4_aab9481ffe7c_Label: ' Terminate
        Microsoft_Store.Instance.Terminate()
        GoTo Action_617b589a_e24f_4b18_8b8b_696abab94bf7_Label

        Action_617b589a_e24f_4b18_8b8b_696abab94bf7_Label: ' Wait Updates Finished
        Windows_Settings.Instance.Wait_Updates_Finished()
        GoTo Action_54d85639_3e6d_487a_8b8e_f3e95427e046_Label

        Action_54d85639_3e6d_487a_8b8e_f3e95427e046_Label: ' Terminate
        Windows_Settings.Instance.Terminate()
        GoTo Process_5eb89ba8_fa28_4939_b7d7_cc29a13cf616_Label

        Process_5eb89ba8_fa28_4939_b7d7_cc29a13cf616_Label: ' Call Process A
        MP___Subprocess_A.Instance.Main(Name:=MyText, Char_Count:=Char_Count)
        GoTo End_1f3bb070_109c_4bbc_babf_3ef6af6b6fa2_Label

        End_1f3bb070_109c_4bbc_babf_3ef6af6b6fa2_Label:

    End Sub

    ''' <summary>
    ''' BluePrism method: Dummy
    ''' </summary>
    Private Sub Dummy()

        ' Local variables
        Dim VerwSysSl As String
        Dim VNR As String

        ' Initialize local variables with alwaysinit
        If VNR Is Nothing Then
            VNR = "AB123456"""
        End If

        GoTo Action_0a4a56fe_a66f_4922_b04d_b26c81313910_Label
        Action_0a4a56fe_a66f_4922_b04d_b26c81313910_Label: ' MyPublicAction
        bp_demo.Instance.MyPublicAction(VNR:=VNR, VerwSysSl:=VerwSysSl)
        GoTo End_5fa21eeb_0828_4758_8d6f_16361a445fa1_Label

        End_5fa21eeb_0828_4758_8d6f_16361a445fa1_Label:

    End Sub

    ''' <summary>
    ''' BluePrism method: Variable_Test
    ''' </summary>
    Private Sub Variable_Test()

        ' Local variables
        Dim Data1 As DateTime
        Dim Data2 As DateTime
        Dim Data3 As Boolean
        Dim Data4 As Boolean
        Dim Data5 As Decimal
        Dim Data6 As Decimal
        Dim Data7 As String
        Dim Data8 As String
        Dim Data9 As String
        Dim Data10 As TimeSpan
        Dim Data11 As TimeSpan
        Dim MyToggle As Boolean
        Dim Coll1 As DataTable
        Dim Coll2 As DataTable
        Dim Coll3 As DataTable

        ' Initialize local variables with alwaysinit
        If Data1 Is Nothing Then
            Data1 = DateTime.Parse("2026/02/01")
        End If
        If Data2 Is Nothing Then
            Data2 = DateTime.Parse("2026-02-11 15:37:42Z")
        End If
        If Data3 Is Nothing Then
            Data3 = False
        End If
        If Data6 Is Nothing Then
            Data6 = 4.5
        End If
        If Data9 Is Nothing Then
            Data9 = "something"
        End If
        If Data10 Is Nothing Then
            Data10 = TimeSpan.Parse("10:20:33")
        End If
        If Data11 Is Nothing Then
            Data11 = TimeSpan.Parse("1.02:03:04")
        End If

        GoTo Decision_4a05cdbe_f075_4c4f_972b_7e63880a1bb6_Label
        Decision_4a05cdbe_f075_4c4f_972b_7e63880a1bb6_Label: ' MyToggle?
        If MyToggle Then
            GoTo Calculation_da6d4891_f555_46b1_b752_73b463373de8_Label
        Else
            GoTo MultipleCalculation_acac267a_d52f_473a_8266_3116177d865f_Label
        End If

        Calculation_da6d4891_f555_46b1_b752_73b463373de8_Label: ' Toggle boolean value
        MyToggle = MyToggle = False
        GoTo MultipleCalculation_acac267a_d52f_473a_8266_3116177d865f_Label

        MultipleCalculation_acac267a_d52f_473a_8266_3116177d865f_Label: ' Multi1
        ' Data5 = 123
        ' Data6 = 7.8
        ' Data8 = "tttt"
        GoTo End_71b0a2dc_32e2_4505_aff3_7368fe044655_Label

        End_71b0a2dc_32e2_4505_aff3_7368fe044655_Label:

    End Sub


    #End Region

End Class
