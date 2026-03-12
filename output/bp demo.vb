Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Data
Imports System.Drawing

''' <summary>
''' BluePrism object: bp demo
''' Version: 7.5.0.17125
''' Generated: 2026-03-12 13:18:45
''' </summary>
Public Class bp_demo
    Inherits BP_Base

    #Region "Singleton Instance"

    Private Shared ReadOnly _lazyInstance As New Lazy(Of bp_demo)(Function() New bp_demo())

    Public Shared ReadOnly Property Instance As bp_demo
        Get
            Return _lazyInstance.Value
        End Get
    End Property

    #End Region

    #Region "Global Data Items"

    ' Environment (collection)
    Protected Environment As DataTable

    #End Region

    #Region "Methods"

    ''' <summary>
    ''' This page is like the class constructor
    ''' </summary>
    Public Sub New()

        ' Initialize collections
        Environment = New DataTable()
        Environment.Columns.Add("Const Value1", GetType(String))
        Environment.Rows.Add("ABC")

    End Sub

    ''' <summary>
    ''' This page is like the class destructor
    ''' </summary>
    Protected Overrides Sub Finalize()

    End Sub

    ''' <summary>
    ''' return the first two chars of a VNR
    ''' </summary>
    ''' <param name="VNR">Vertragsnummer (LF123456789)</param>
    ''' <param name="VerwSysSl">Verwaltungssystem Schlüssel</param>
    Public Sub MyPublicAction(Optional ByVal VNR As String = Nothing, Optional ByRef VerwSysSl As String = Nothing)

        ' Set VerwSysSl
        On Error GoTo MyPublicAction_Recover
        VerwSysSl = Left(VNR, 2)
        GoTo End_MyPublicAction

        ' Recover
        MyPublicAction_Recover:
        StoreException()

        ' Resume
        ClearException()
        Resume End_MyPublicAction

        End_MyPublicAction:

    End Sub

    ''' <summary>
    ''' Page: Get_URL
    ''' </summary>
    ''' <param name="URL">current URL</param>
    ''' <param name="Window_Title">current Window Title</param>
    Public Sub Get_URL(Optional ByRef URL As String = Nothing, Optional ByRef Window_Title As String = Nothing)

        ' Wait1
        ' Wait 5 seconds for condition with 1 choice(s)
        Select Case True
            Case Application.Element("URL Bar", "ad4b93a6-97c7-4fa7-acde-f6a00d96ac32").CheckExists = True ' URL Bar Check Exists
                GoTo Get_URL_Reader_URL_Title
        End Select
        GoTo End_Get_URL

        ' Reader URL+Title
        Get_URL_Reader_URL_Title:
        URL = Application.Element("URL Bar").UIAGetValue()
        Window_Title = Application.Element("Main Window").GetWindowText()

        End_Get_URL:

    End Sub

    ''' <summary>
    ''' Page: Set_URL
    ''' </summary>
    ''' <param name="URL">New URL</param>
    Public Sub Set_URL(Optional ByVal URL As String = Nothing)

        ' Initialize variables
        URL = "http://google.de"

        ' Wait1
        ' Wait 5 seconds for condition with 1 choice(s)
        Select Case True
            Case Application.Element("URL Bar", "ad4b93a6-97c7-4fa7-acde-f6a00d96ac32").CheckExists = True ' URL Bar Check Exists
                GoTo Set_URL_Writer_URL
        End Select
        GoTo End_Set_URL

        ' Writer URL
        Set_URL_Writer_URL:
        Application.Element("URL Bar").Write(URL)

        ' Send Enter
        Application.Element("Main Window").ActivateApp()
        Application.Element("URL Bar").UIASendKeys(newtext:="{ENTER}")

        End_Set_URL:

    End Sub

    ''' <summary>
    ''' concatenates value with global value
    ''' </summary>
    ''' <param name="Value">Text</param>
    Private Sub InteralAction(Optional ByVal Value As String = Nothing)

        ' value empty?
        If Value = "" Then
            GoTo InteralAction_SE
        End If

        ' Set Value
        Value = Value & Environment.GetCurrentRow("Const Value1").Value

        ' Note1
        ' This is a note in BP
        GoTo End_InteralAction

        ' SE
        InteralAction_SE:
        Throw New BP_Exception("System Exception", "Value is empty")

        ' Global Recover
        StoreException()

        ' Log Exception
        Value = "Type: " & ExceptionType() & NewLine() &
"Details: " & ExceptionDetail()

        ' Re-Throw
        Throw GetLastException()

        End_InteralAction:

    End Sub

    #End Region

    #Region "App Model"

    Protected Application As Object

    #End Region

End Class
