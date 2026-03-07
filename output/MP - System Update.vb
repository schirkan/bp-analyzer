' Generated from BluePrism process: MP - System Update
' Version: 1.0
' Generated: 2026-03-07 23:14:33

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Data

''' <summary>
''' BluePrism process: MP - System Update
''' </summary>
Public Class MP___System_Update
    Inherits BP_Base

    #Region "Singleton Instance"

    Private Shared ReadOnly _lazyInstance As New Lazy(Of MP___System_Update)(Function() New MP___System_Update())

    Public Shared ReadOnly Property Instance As MP___System_Update
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
    ''' Start Update of: Windows, Microsoft Store, Steam etc.
    ''' </summary>
    Public Sub Main()

        ' Launch
        Microsoft_Store.Instance.Launch()

        ' Start Updates
        Microsoft_Store.Instance.Start_Updates()

        ' Launch
        Windows_Settings.Instance.Launch()

        ' Start Updates
        Windows_Settings.Instance.Start_Updates()

        ' Wait Updates Finished
        Microsoft_Store.Instance.Wait_Updates_Finished()

        ' Terminate
        Microsoft_Store.Instance.Terminate()

        ' Wait Updates Finished
        Windows_Settings.Instance.Wait_Updates_Finished()

        ' Terminate
        Windows_Settings.Instance.Terminate()

    End Sub

    #End Region

End Class
