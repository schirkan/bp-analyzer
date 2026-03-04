' Generated from BluePrism process: MP - Subprocess A
' Version: 1.0
' Generated: 2026-03-04 20:08:23
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

        ' Initialize local variables with initialvalue

        ' Initialize input parameters with alwaysinit

        ' Calc Char Count
        Char_Count = Len(Name)

    End Sub

    #End Region

End Class
