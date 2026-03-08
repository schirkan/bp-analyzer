' Generated from BluePrism object: bp demo
' Version: 1.0
' Generated: 2026-03-08 00:19:21

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Data
Imports System.Drawing

''' <summary>
''' BluePrism object: bp demo
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

    ''' <summary>
    ''' Global data item: Environment (collection)
    ''' </summary>
    Public Environment As DataTable

    #End Region

    #Region "Methods"

    ''' <summary>
    ''' This page is like the class constructor
    ''' </summary>
    Public Sub New()

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
        On Error GoTo Recover_Label
        VerwSysSl = Left(VNR, 2)
        GoTo End_MyPublicAction_Label

        ' Recover
        Recover_Label:
        StoreException()

        ' Resume
        ClearException()
        Resume End_MyPublicAction_Label

        End_MyPublicAction_Label:

    End Sub

    ''' <summary>
    ''' BluePrism page: Get_URL
    ''' </summary>
    ''' <param name="URL">current URL</param>
    ''' <param name="Window_Title">current Window Title</param>
    Public Sub Get_URL(Optional ByRef URL As String = Nothing, Optional ByRef Window_Title As String = Nothing)

        ' Wait1
        ' Wait 5 seconds for condition with 1 choice(s)
        Select Case True
            Case Application.Element("URL Bar", "ad4b93a6-97c7-4fa7-acde-f6a00d96ac32").CheckExists = True ' URL Bar Check Exists
                GoTo Read_Label
            Case Else
                GoTo End_Get_URL_Label
        End Select

        ' Reader URL+Title
        Read_Label:
        URL = Application.Element("URL Bar", "ad4b93a6-97c7-4fa7-acde-f6a00d96ac32").UIAGetValue()
        Window_Title = Application.Element("Main Window", "2660caf7-78b8-4335-af2e-bcf547eaf9a8").GetWindowText()
        
        End_Get_URL_Label:

    End Sub

    ''' <summary>
    ''' BluePrism page: Set_URL
    ''' </summary>
    ''' <param name="URL">New URL</param>
    Public Sub Set_URL(Optional ByRef URL As String = Nothing, Optional ByRef Window_Title As String = Nothing)

        ' Initialize variables with initialvalue
        URL = "http://google.de"

        ' Wait1
        ' Wait 5 seconds for condition with 1 choice(s)
        Select Case True
            Case Application.Element("URL Bar", "ad4b93a6-97c7-4fa7-acde-f6a00d96ac32").CheckExists = True ' URL Bar Check Exists
                GoTo Write_Label
            Case Else
                GoTo End_Set_URL_Label
        End Select

        ' Writer URL
        Write_Label:
        Application.Element("URL Bar", "ad4b93a6-97c7-4fa7-acde-f6a00d96ac32").Write(URL)

        ' Send Enter
        Application.Element("Main Window", "2660caf7-78b8-4335-af2e-bcf547eaf9a8").ActivateApp()
        Application.Element("URL Bar", "ad4b93a6-97c7-4fa7-acde-f6a00d96ac32").UIASendKeys(newtext:="{ENTER}")
        
        End_Set_URL_Label:

    End Sub

    ''' <summary>
    ''' concatenates value with global value
    ''' </summary>
    ''' <param name="Value">Text</param>
    Private Sub InteralAction(Optional ByVal Value As String = Nothing)

        ' value empty?
        On Error GoTo Recover_2_Label
        If Value = "" Then
            GoTo Exception_Label
        Else
            GoTo Calculation_2_Label
        End If

        ' SE
        Exception_Label:
        On Error GoTo Recover_2_Label
        RaiseException("System Exception", "Value is empty")

        ' Set Value
        Calculation_2_Label:
        On Error GoTo Recover_2_Label
        Value = Value & Environment.CurrentRow("Const Value1")

        ' Note1
        ' This is a note in BP
        GoTo End_InteralAction_Label

        ' Global Recover
        Recover_2_Label:
        StoreException()
        GoTo Calculation_3_Label

        ' Re-Throw
        Exception_2_Label:
        On Error GoTo Recover_2_Label
        RethrowException()

        ' Log Exception
        Calculation_3_Label:
        On Error GoTo Recover_2_Label
        Value = "Type: " & ExceptionType() & NewLine() &
"Details: " & ExceptionDetail()
        GoTo Exception_2_Label

        End_InteralAction_Label:

    End Sub

    #End Region

    #Region "App Model"

    Protected Application As Object

    #End Region

End Class
