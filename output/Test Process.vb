Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Data

''' <summary>
''' BluePrism process: Test Process
''' Version: 7.5.0.17125
''' Generated: 2026-03-13 12:24:59
''' </summary>
Public Class Test_Process
    Inherits BP_Base

    #Region "Singleton Instance"

    Private Shared ReadOnly _lazyInstance As New Lazy(Of Test_Process)(Function() New Test_Process())

    Public Shared ReadOnly Property Instance As Test_Process
        Get
            Return _lazyInstance.Value
        End Get
    End Property

    #End Region

    #Region "Global Data Items"

    Protected MyText As String
    Protected Data4 As Boolean?

    #End Region

    #Region "Methods"

    ''' <summary>
    ''' Main process entry point
    ''' </summary>
    Public Sub Main()

        ' Local variables
        Dim Char_Count As Decimal?
        Dim OutValue1 As DateTime?
        Dim OutValue2 As Boolean?

        ' Initialize variables
        If MyText Is Nothing Then
            MyText = "Hallo Welt"
        End If

        ' Variable Test
        Variable_Test(
            InData1:=MyText, 
            InData2:=123, 
            OutValue1:=OutValue1, 
            OutValue2:=OutValue2)

        ' Loop Test
        Loop_Test()

        ' Dummy
        Dummy()

        ' Call Process A
        MP_Subprocess_A.Instance.Main(
            Name:=MyText, 
            Char_Count:=Char_Count)

    End Sub

    ''' <summary>
    ''' Page: Dummy
    ''' </summary>
    Private Sub Dummy()

        ' Local variables
        Dim local_VerwSysSl As String
        Dim local_VNR As String
        Dim URL As String
        Dim Window_Title As String

        ' Initialize variables
        local_VNR = "AB123456"""

        ' MyPublicAction
        bp_demo.Instance.MyPublicAction(
            VNR:=local_VNR, 
            VerwSysSl:=local_VerwSysSl)

        ' bp demo::Get URL
        bp_demo.Instance.Get_URL(
            URL:=URL, 
            Window_Title:=Window_Title)

    End Sub

    ''' <summary>
    ''' Page: Loop_Test
    ''' </summary>
    Private Sub Loop_Test()

        ' Local variables
        Dim Values As DataTable
        Dim Name As String

        ' Initialize variables
        Name = "Martin"

        ' Initialize collections
        If Values Is Nothing Then
            Values = New DataTable()
            Values.Columns.Add("Name", GetType(String))
            Values.Columns.Add("Distance", GetType(Decimal))
            Values.Columns.Add("Similarity", GetType(Decimal))
            Values.Rows.Add("Max", 0, 0)
            Values.Rows.Add("John", 0, 0)
            Values.Rows.Add("Lisa", 0, 0)
        End If

        ' Loop Values
        Values.SelectFirstRow()

        ' Calculate Distance
        Loop_Test_Calculate_Distance:
        Utility_Strings.Instance.Calculate_Distance(
            Source:=Values.GetCurrentRow("Name").Value, 
            Target:=Name, 
            Case_Sensitive:=False, 
            Distance:=Values.GetCurrentRow("Distance").Value, 
            Similarity:=Values.GetCurrentRow("Similarity").Value)

        ' Loop Values
        If Values.SelectNextRow() Then
            GoTo Loop_Test_Calculate_Distance
        End If

    End Sub

    ''' <summary>
    ''' Auf dieser Page befinden sich mehrere Tests
    ''' </summary>
    Private Sub Variable_Test(Optional ByVal InData1 As String = Nothing, Optional ByVal InData2 As Decimal? = Nothing, Optional ByRef OutValue1 As DateTime? = Nothing, Optional ByRef OutValue2 As Boolean? = Nothing)

        ' Local variables
        Dim Data1 As DateTime?
        Dim Data2 As DateTime?
        Dim Data3 As Boolean?
        Dim Data5 As Decimal?
        Dim Data6 As Decimal?
        Dim Data7 As String
        Dim Data8 As String
        Dim Data9 As String
        Dim Data10 As TimeSpan?
        Dim Data11 As TimeSpan?
        Static MyToggle As Boolean?
        Dim Coll1 As DataTable
        Dim Coll2 As DataTable
        Dim Coll3 As DataTable
        Dim Count As Decimal?

        ' Initialize variables
        Data1 = DateTime.Parse("2026/02/01")
        Data2 = DateTime.Parse("2026-02-11 15:37:42Z")
        Data3 = False
        Data6 = 4.5
        Data9 = "something"
        Data10 = TimeSpan.Parse("10:20:33")
        Data11 = TimeSpan.Parse("1.02:03:04")
        If MyToggle Is Nothing Then
            MyToggle = False
        End If

        ' Initialize collections
        If Coll1 Is Nothing Then
            Coll1 = New DataTable()
        End If
        If Coll2 Is Nothing Then
            Coll2 = New DataTable()
            Coll2.Columns.Add("Text", GetType(String))
            Coll2.Columns.Add("Zahl", GetType(Decimal))
            Coll2.Rows.Add("aa", 2)
            Coll2.Rows.Add("bb", 3)
        End If
        If Coll3 Is Nothing Then
            Coll3 = New DataTable()
            Coll3.Columns.Add("Text", GetType(String))
            Coll3.Columns.Add("Zahl", GetType(Decimal))
            Coll3.Rows.Add("aaa", 111)
        End If

        ' Initialize local variables with input values
        If InData1 IsNot Nothing Then
            Data8 = InData1
        End If
        If InData2.HasValue Then
            Data6 = InData2.Value
        End If

        ' MyToggle?
        If MyToggle Then
            GoTo Variable_Test_Toggle_boolean_value
        End If

        ' Multi1
        Variable_Test_Multi1:
        Data5 = 1
        Data6 = 7.8
        Data8 = "tttt"

        ' Zahl?
        Select Case True
            Case Data5 = 1 ' Zahl ist 1
                GoTo Variable_Test_Collections_Count_Rows
            Case Data5 = 2 ' Zahl ist 2
                GoTo Variable_Test_Calc1
        End Select

        ' Alert1
        BP_Alert.Notify("Achtung Achtung")

        ' BE
        Throw New BP_Exception("Business Exception", "Zahl ist nicht 1 oder 2")

        ' Collections::Count Rows
        Variable_Test_Collections_Count_Rows:
        Blueprism.AutomateProcessCore.clsCollectionActions.Instance.Count_Rows(
            Collection_Name:="Coll2", 
            Count:=Count)
        GoTo End_Variable_Test

        ' Calc1
        Variable_Test_Calc1:
        Data6 = Data5 + 6
        GoTo Variable_Test_Collections_Count_Rows

        ' Toggle boolean value
        Variable_Test_Toggle_boolean_value:
        MyToggle = MyToggle = False
        GoTo Variable_Test_Multi1

        End_Variable_Test:
        OutValue1 = Data1
        OutValue2 = MyToggle

    End Sub

    #End Region

End Class
