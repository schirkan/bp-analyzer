' Generated from BluePrism process: MP - Subprocess A
' Version: 1.0
' Generated: 2026-02-26 23:12:56
' 
' This is a test subprocess

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Data

''' <summary>
''' BluePrism process: MP - Subprocess A
''' </summary>
Public Class MP___Subprocess_A

    #Region "Global Data Items"

    ' No global data items

    #End Region

    #Region "Methods"

    ''' <summary>
    ''' Main process entry point
    ''' </summary>
    Public Sub Main(ByVal Name As String, <Out> ByRef Char_Count As Decimal)

            GoTo Calc_Char_Count_94e477c2_f432_419e_bffc_e912c152acfd_Label
Calc_Char_Count_94e477c2_f432_419e_bffc_e912c152acfd_Label:
        ' Calc Char Count (Calculation)
            Char_Count = Len(Name)
            GoTo End_7f8f449f_a21d_4a5f_89de_1ceb5b5b58d5_Label

Input_b98e988f_253d_4985_8ec5_3a4496dde0ed_Label:
        ' Input (Block)
            ' Block: Input

Output_1c6d589e_bba8_424b_9662_a6319af88527_Label:
        ' Output (Block)
            ' Block: Output

End_7f8f449f_a21d_4a5f_89de_1ceb5b5b58d5_Label:

    End Sub

    #End Region

End Class
