' Generated from BluePrism process: Test Process
' Version: 1.0
' Generated: 2026-03-10 19:14:42

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Data

''' <summary>
''' BluePrism process: Test Process
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

    ''' <summary>
    ''' Global data item: MyText (text)
    ''' </summary>
    Public MyText As String

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

        ' Initialize variables with initialvalue
        If MyText Is Nothing Then
            MyText = "Hallo Welt"
        End If

        ' Variable Test
        Variable_Test(InData1:=MyText, InData2:=123, OutValue1:=OutValue1, OutValue2:=OutValue2)

        ' Loop Test
        Loop_Test()

        ' Dummy
        Dummy()

        ' Call Process A
        MP___Subprocess_A.Instance.Main(Name:=MyText, Char_Count:=Char_Count)

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

        ' Initialize variables with initialvalue
        local_VNR = "AB123456"""

        ' MyPublicAction
        bp_demo.Instance.MyPublicAction(VNR:=local_VNR, VerwSysSl:=local_VerwSysSl)

        ' bp demo::Get URL
        bp_demo.Instance.Get_URL(URL:=URL, Window_Title:=Window_Title)

    End Sub

    ''' <summary>
    ''' Page: Loop_Test
    ''' </summary>
    Private Sub Loop_Test()

        ' Local variables
        Dim Values As DataTable
        Dim Name As String

        ' Initialize variables with initialvalue
        Name = "Martin"

        ' Loop Values
        Values.SelectFirstRow()
        
        ' Calculate Distance
        Action_3_Label:
        Utility___Strings.Instance.Calculate_Distance(Source:=Values.GetCurrentRow("Name").Value, Target:=Name, Case_Sensitive:=False, Distance:=Values.GetCurrentRow("Distance").Value, Similarity:=Values.GetCurrentRow("Similarity").Value)

        ' Loop Values
        If Values.SelectNextRow() Then
            GoTo Action_3_Label
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

        ' Initialize variables with initialvalue
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

        ' Initialize local variables with input values
        If InData1 IsNot Nothing Then
            Data8 = InData1
        End If
        If InData2.HasValue Then
            Data6 = InData2.Value
        End If

        ' MyToggle?
        If MyToggle Then
            GoTo Calculation_Label
        End If
        
        ' Multi1
        MultipleCalculation_Label:
        Data5 = 1
        Data6 = 7.8
        Data8 = "tttt"

        ' Zahl?
        Select Case True
            Case Data5 = 1 ' Zahl ist 1
                GoTo Action_4_Label
            Case Data5 = 2 ' Zahl ist 2
                GoTo Calculation_2_Label
        End Select

        ' Alert1
        BP_Alert.Notify("Achtung Achtung")

        ' BE
        RaiseException("Business Exception", "Zahl ist nicht 1 oder 2")

        ' Collections::Count Rows
        Action_4_Label:
        Blueprism_AutomateProcessCore_clsCollectionActions.Instance.Count_Rows(Collection_Name:="Coll2", Count:=Count)
        GoTo End_Variable_Test_Label

        ' Calc1
        Calculation_2_Label:
        Data6 = Data5 + 6
        GoTo Action_4_Label

        ' Toggle boolean value
        Calculation_Label:
        MyToggle = MyToggle = False
        GoTo MultipleCalculation_Label

        End_Variable_Test_Label:
        OutValue1 = Data1
        OutValue2 = MyToggle

    End Sub

    #End Region

End Class
