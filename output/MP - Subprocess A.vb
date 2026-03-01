' Generated from BluePrism process: MP - Subprocess A
' Version: 1.0
' Generated: 2026-03-01 15:17:44
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
    Inherits BP_Base

    #Region "Singleton Instance"

    ''' <summary>
    ''' Shared singleton instance
    ''' </summary>
    Private Shared ReadOnly _lazyInstance As New Lazy(Of MP___Subprocess_A)(Function() New MP___Subprocess_A())

    Public Shared ReadOnly Property Instance As MP___Subprocess_A
        Get
            Return _lazyInstance.Value
        End Get
    End Property

    #End Region

    #Region "Global Data Items"

    ' No global data items

    #End Region

    #Region "Methods"

    ''' <summary>
    ''' Main process method (stages without subsheet)
    ''' </summary>
    Public Sub Main(Optional ByVal Name As String = Nothing, Optional ByRef Char_Count As Decimal = Nothing)

        ' Initialize input parameters with alwaysinit

        GoTo Calculation_94e477c2_f432_419e_bffc_e912c152acfd_Label
        Calculation_94e477c2_f432_419e_bffc_e912c152acfd_Label: ' Calc Char Count
        Char_Count = Len(Name)
        GoTo End_7f8f449f_a21d_4a5f_89de_1ceb5b5b58d5_Label

        End_7f8f449f_a21d_4a5f_89de_1ceb5b5b58d5_Label:

    End Sub


    #End Region

End Class
