' Generated from BluePrism process: MP - System Update
' Version: 1.0
' Generated: 2026-03-06 21:52:19

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

        ' Initialize variables with initialvalue
        If MyText Is Nothing Then
            MyText = "Hallo Welt"
        End If

        ' Launch
        Microsoft_Store.Instance.Launch()
        ' Start Updates
        Microsoft_Store.Instance.Start_Updates()
        ' Launch
        Windows_Settings.Instance.Launch()
        ' Start Updates
        Windows_Settings.Instance.Start_Updates()
        ' Wait Updates Finished
        Microsoft_Store.Instance.Wait_Updates_Finished()
        ' Terminate
        Microsoft_Store.Instance.Terminate()
        ' Wait Updates Finished
        Windows_Settings.Instance.Wait_Updates_Finished()
        ' Terminate
        Windows_Settings.Instance.Terminate()
        ' Call Process A
        MP___Subprocess_A.Instance.Main(Name:=MyText, Char_Count:=Char_Count)

    End Sub

    ''' <summary>
    ''' BluePrism page: Dummy
    ''' </summary>
    Private Sub Dummy()

        ' Local variables
        Dim VerwSysSl As String
        Dim VNR As String

        ' Initialize variables with initialvalue
        If VNR Is Nothing Then
            VNR = "AB123456"""
        End If

        ' MyPublicAction
        bp_demo.Instance.MyPublicAction(VNR:=VNR, VerwSysSl:=VerwSysSl)

    End Sub

    ''' <summary>
    ''' BluePrism page: Variable_Test
    ''' </summary>
    Private Sub Variable_Test(Optional ByVal InData1 As String = Nothing, Optional ByVal InData2 As Decimal = Nothing, Optional ByRef OutValue1 As DateTime = Nothing, Optional ByRef OutValue2 As Boolean = Nothing)

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
        Static MyToggle As Boolean
        Dim Coll1 As DataTable
        Dim Coll2 As DataTable
        Dim Coll3 As DataTable

        ' Initialize variables with initialvalue
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
        If MyToggle Is Nothing Then
            MyToggle = False
        End If

        ' Initialize local variables with input values
        Data8 = InData1
        Data6 = InData2

        ' MyToggle?
        If MyToggle Then
            GoTo Calculation_da6d4891_f555_46b1_b752_73b463373de8_Label
        Else
            GoTo MultipleCalculation_acac267a_d52f_473a_8266_3116177d865f_Label
        End If

        Calculation_da6d4891_f555_46b1_b752_73b463373de8_Label: ' Toggle boolean value
        MyToggle = MyToggle = False
        MultipleCalculation_acac267a_d52f_473a_8266_3116177d865f_Label: ' Multi1
        Data5 = 123
        Data6 = 7.8
        Data8 = "tttt"
        
        OutValue1 = Data1
        OutValue2 = MyToggle

    End Sub

    #End Region

End Class
