' Generated from BluePrism object: bp demo
' Version: 1.0
' Generated: 2026-03-06 21:52:19
' 
' This page is like the class constructor

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

    ''' <summary>
    ''' Shared singleton instance
    ''' </summary>
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
    ''' Constructor - initialization code from stages without subsheet
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

        Recover_8247b8bb_c2bb_4895_9e43_99be2b466b50_Label: ' Recover
        StoreException()
        ' Resume
        ClearException()
        Resume End_874d3dd7_cc13_4d3f_a2ba_cef7c8de0ee7_Label

        End_874d3dd7_cc13_4d3f_a2ba_cef7c8de0ee7_Label:

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

        Exception_02b6ad56_a2b5_4d81_8ec8_61d5ee417ac2_Label: ' SE
        On Error GoTo Recover_e58b2971_f6b9_4152_9eac_d76e7cd54a1b_Label
        RaiseException("System Exception", "Value is empty")

        Calculation_cbfee36a_a7d4_4121_bfbd_711414022b6a_Label: ' Set Value
        On Error GoTo Recover_e58b2971_f6b9_4152_9eac_d76e7cd54a1b_Label
        Value = Value & Environment.CurrentRow("Const Value1")
        ' Note1
        ' This is a note in BP
        GoTo End_ab996bd9_e5f6_4c28_a59e_cc84ab29e58c_Label

        Recover_e58b2971_f6b9_4152_9eac_d76e7cd54a1b_Label: ' Global Recover
        StoreException()
        GoTo Calculation_6ad99ace_a4e2_4919_9a22_e7baa3af2bde_Label

        Exception_a987ff7d_de3a_4d2c_bcca_696cda7cdcb2_Label: ' Re-Throw
        On Error GoTo Recover_e58b2971_f6b9_4152_9eac_d76e7cd54a1b_Label
        RethrowException()

        Calculation_6ad99ace_a4e2_4919_9a22_e7baa3af2bde_Label: ' Log Exception
        On Error GoTo Recover_e58b2971_f6b9_4152_9eac_d76e7cd54a1b_Label
        Value = "Type: " & ExceptionType() & NewLine() &
"Details: " & ExceptionDetail()
        GoTo Exception_a987ff7d_de3a_4d2c_bcca_696cda7cdcb2_Label

        End_ab996bd9_e5f6_4c28_a59e_cc84ab29e58c_Label:

    End Sub

    #End Region

End Class
