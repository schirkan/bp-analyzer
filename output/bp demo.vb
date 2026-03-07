' Generated from BluePrism object: bp demo
' Version: 1.0
' Generated: 2026-03-07 23:14:33

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
        On Error GoTo Recover_8247b8bb_c2bb_4895_9e43_99be2b466b50_Label
        VerwSysSl = Left(VNR, 2)

        GoTo End_874d3dd7_cc13_4d3f_a2ba_cef7c8de0ee7_Label

        ' Recover
        Recover_8247b8bb_c2bb_4895_9e43_99be2b466b50_Label:
        StoreException()

        ' Resume
        ClearException()
        Resume End_874d3dd7_cc13_4d3f_a2ba_cef7c8de0ee7_Label

        End_874d3dd7_cc13_4d3f_a2ba_cef7c8de0ee7_Label:

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
                GoTo Read_d7797c7b_b981_422b_8c44_04144e3bd521_Label
            Case Else
                GoTo End_7fc17c15_89e3_42d4_b02d_c2104ae9f78b_Label
        End Select

        ' Reader URL+Title
        Read_d7797c7b_b981_422b_8c44_04144e3bd521_Label:
        URL = Application.Element("URL Bar", "ad4b93a6-97c7-4fa7-acde-f6a00d96ac32").UIAGetValue()
        Window_Title = Application.Element("Main Window", "2660caf7-78b8-4335-af2e-bcf547eaf9a8").GetWindowText()
        End_7fc17c15_89e3_42d4_b02d_c2104ae9f78b_Label:

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
                GoTo Write_52efde20_3c57_40a5_b946_8b3dc32ce835_Label
            Case Else
                GoTo End_0555942f_1c7a_4c1c_aa68_6a107b4b5a52_Label
        End Select

        ' Writer URL
        Write_52efde20_3c57_40a5_b946_8b3dc32ce835_Label:
        Application.Element("URL Bar", "ad4b93a6-97c7-4fa7-acde-f6a00d96ac32").Write(URL)

        ' Send Enter
        Application.Element("Main Window", "2660caf7-78b8-4335-af2e-bcf547eaf9a8").ActivateApp()
        Application.Element("URL Bar", "ad4b93a6-97c7-4fa7-acde-f6a00d96ac32").UIASendKeys(newtext:="{ENTER}")
        End_0555942f_1c7a_4c1c_aa68_6a107b4b5a52_Label:

    End Sub

    ''' <summary>
    ''' concatenates value with global value
    ''' </summary>
    ''' <param name="Value">Text</param>
    Private Sub InteralAction(Optional ByVal Value As String = Nothing)

        ' value empty?
        On Error GoTo Recover_e58b2971_f6b9_4152_9eac_d76e7cd54a1b_Label
        If Value = "" Then
            GoTo Exception_02b6ad56_a2b5_4d81_8ec8_61d5ee417ac2_Label
        Else
            GoTo Calculation_cbfee36a_a7d4_4121_bfbd_711414022b6a_Label
        End If

        ' SE
        Exception_02b6ad56_a2b5_4d81_8ec8_61d5ee417ac2_Label:
        On Error GoTo Recover_e58b2971_f6b9_4152_9eac_d76e7cd54a1b_Label
        RaiseException("System Exception", "Value is empty")

        ' Set Value
        Calculation_cbfee36a_a7d4_4121_bfbd_711414022b6a_Label:
        On Error GoTo Recover_e58b2971_f6b9_4152_9eac_d76e7cd54a1b_Label
        Value = Value & Environment.CurrentRow("Const Value1")

        ' Note1
        ' This is a note in BP
        GoTo End_ab996bd9_e5f6_4c28_a59e_cc84ab29e58c_Label

        ' Global Recover
        Recover_e58b2971_f6b9_4152_9eac_d76e7cd54a1b_Label:
        StoreException()
        GoTo Calculation_6ad99ace_a4e2_4919_9a22_e7baa3af2bde_Label

        ' Re-Throw
        Exception_a987ff7d_de3a_4d2c_bcca_696cda7cdcb2_Label:
        On Error GoTo Recover_e58b2971_f6b9_4152_9eac_d76e7cd54a1b_Label
        RethrowException()

        ' Log Exception
        Calculation_6ad99ace_a4e2_4919_9a22_e7baa3af2bde_Label:
        On Error GoTo Recover_e58b2971_f6b9_4152_9eac_d76e7cd54a1b_Label
        Value = "Type: " & ExceptionType() & NewLine() &
"Details: " & ExceptionDetail()

        GoTo Exception_a987ff7d_de3a_4d2c_bcca_696cda7cdcb2_Label

        End_ab996bd9_e5f6_4c28_a59e_cc84ab29e58c_Label:

    End Sub

    #End Region

    #Region "App Model"

    Protected Application As Object

    #End Region

End Class
