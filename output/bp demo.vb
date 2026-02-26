' Generated from BluePrism object: bp demo
' Version: 1.0
' Generated: 2026-02-26 23:12:56
' 
' This page is like the class constructor
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

''' <summary>
''' BluePrism object: bp demo
''' </summary>
Public Class bp_demo

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

            GoTo End_5b1a9c0b_03f0_4ed1_8597_8334052f27eb_Label
End_5b1a9c0b_03f0_4ed1_8597_8334052f27eb_Label:

    End Sub

    ''' <summary>
    ''' This page is like the class destructor
    ''' </summary>
    Protected Overrides Sub Finalize()

            GoTo End_29330249_fa1d_4eb4_bffa_92c11f327cf0_Label
End_29330249_fa1d_4eb4_bffa_92c11f327cf0_Label:

    End Sub

    ''' <summary>
    ''' return the first two chars of a VNR
    ''' </summary>
    ''' <param name="VNR">Vertragsnummer (LF123456789)</param>
    ''' <param name="VerwSysSl">Verwaltungssystem Schlüssel</param>
    Public Sub MyPublicAction(ByVal VNR As String, <Out> ByRef VerwSysSl As String)

            GoTo Set_VerwSysSl_402fa590_a68f_412f_9662_7d9affd096bf_Label
Set_VerwSysSl_402fa590_a68f_412f_9662_7d9affd096bf_Label:
        ' Set VerwSysSl (Calculation)
            VerwSysSl = Left(VNR, 2)
            GoTo End_874d3dd7_cc13_4d3f_a2ba_cef7c8de0ee7_Label

Try_Catch_c877d646_25e3_4180_9c5e_1c9c63d26da7_Label:
        ' Try Catch (Block)
            ' Block: Try Catch

Recover_8247b8bb_c2bb_4895_9e43_99be2b466b50_Label:
        ' Recover (Recover)
            ' Recover from error
            GoTo Resume_4dbccd73_b4b2_4f8b_a16b_66912e5cf37e_Label

Resume_4dbccd73_b4b2_4f8b_a16b_66912e5cf37e_Label:
        ' Resume (Resume)
            ' Resume
            GoTo End_874d3dd7_cc13_4d3f_a2ba_cef7c8de0ee7_Label

End_874d3dd7_cc13_4d3f_a2ba_cef7c8de0ee7_Label:

    End Sub

    ''' <summary>
    ''' concatenates value with global value
    ''' </summary>
    ''' <param name="Value">Text</param>
    Private Sub InteralAction(ByVal Value As String)

            GoTo value_empty__7315b947_a7a8_4eaa_884e_5af170b6f804_Label
value_empty__7315b947_a7a8_4eaa_884e_5af170b6f804_Label:
        ' value empty? (Decision)
            If Value = "" Then
                GoTo SE_02b6ad56_a2b5_4d81_8ec8_61d5ee417ac2_Label
            Else
                GoTo Set_Value_cbfee36a_a7d4_4121_bfbd_711414022b6a_Label
            End If

SE_02b6ad56_a2b5_4d81_8ec8_61d5ee417ac2_Label:
        ' SE (Exception)
            Throw New System_Exception("Value is empty")

Set_Value_cbfee36a_a7d4_4121_bfbd_711414022b6a_Label:
        ' Set Value (Calculation)
            Value = Value + Environment.CurrentRow("Const Value1")
            GoTo Note1_99058154_0d65_43d7_9e58_48bc288b3c5e_Label

Note1_99058154_0d65_43d7_9e58_48bc288b3c5e_Label:
        ' Note1 (Note)
            ' Note: This is a note in BP
            GoTo End_ab996bd9_e5f6_4c28_a59e_cc84ab29e58c_Label

End_ab996bd9_e5f6_4c28_a59e_cc84ab29e58c_Label:

    End Sub


    #End Region

End Class
