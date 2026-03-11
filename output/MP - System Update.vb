' Generated from BluePrism process: MP - System Update
' Version: 1.0
' Generated: 2026-03-11 14:08:27

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Data

''' <summary>
''' BluePrism process: MP - System Update
''' </summary>
Public Class MP_System_Update
    Inherits BP_Base

    #Region "Singleton Instance"

    Private Shared ReadOnly _lazyInstance As New Lazy(Of MP_System_Update)(Function() New MP_System_Update())

    Public Shared ReadOnly Property Instance As MP_System_Update
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
        Main_Launch:
        Microsoft_Store.Instance.Launch()
        
        ' Start Updates
        Main_Start_Updates:
        Microsoft_Store.Instance.Start_Updates()

        ' Launch
        Windows_Settings.Instance.Launch()

        ' Start Updates
        Windows_Settings.Instance.Start_Updates()
        
        ' Wait Updates Finished
        Main_Wait_Updates_Finished:
        Microsoft_Store.Instance.Wait_Updates_Finished()
        
        ' Terminate
        Main_Terminate:
        Microsoft_Store.Instance.Terminate()

        ' Wait Updates Finished
        Windows_Settings.Instance.Wait_Updates_Finished()

        ' Terminate
        Windows_Settings.Instance.Terminate()

    End Sub

    #End Region

End Class
