' Generated from BluePrism object: Microsoft Store
' Version: 1.0
' Generated: 2026-03-06 23:30:49

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Data
Imports System.Drawing

''' <summary>
''' BluePrism object: Microsoft Store
''' </summary>
Public Class Microsoft_Store
    Inherits BP_Base

    #Region "Singleton Instance"

    Private Shared ReadOnly _lazyInstance As New Lazy(Of Microsoft_Store)(Function() New Microsoft_Store())

    Public Shared ReadOnly Property Instance As Microsoft_Store
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
    ''' Constructor (Initialize) - called when object is created
    ''' </summary>
    Public Sub New()

    End Sub

    ''' <summary>
    ''' Destructor (CleanUp) - called when object is disposed
    ''' </summary>
    Protected Overrides Sub Finalize()

    End Sub

    ''' <summary>
    ''' BluePrism page: Launch
    ''' </summary>
    Public Sub Launch()

        ' Local variables
        Dim FilePath As String

        ' Initialize variables with initialvalue
        If FilePath Is Nothing Then
            FilePath = "C:\Program Files\WindowsApps\Microsoft.WindowsStore_22512.1401.6.0_x64__8wekyb3d8bbwe\WinStore.App.exe"
        End If

        ' Start Process
        Utility___Environment.Instance.Start_Process(Application:=FilePath)
        ' Attach
        Application.Element("Microsoft Store", "330860ce-038b-499b-9a6c-c1e8140f72a2").AttachApplication()
        ' W5
        ' Wait 5 seconds for condition with 1 choice(s)
        Select Case True
            Case Application.Element("Main Window", "b048ceed-93fb-48da-99af-7fafeec74d4e").CheckExists = True ' Main Window Check Exists
                GoTo End_05774252_de6b_4703_9edf_5bd04116cf21_Label
            Case Else
                GoTo Exception_427cad4f_4758_4510_801d_acbd6542f39b_Label
        End Select

        Exception_427cad4f_4758_4510_801d_acbd6542f39b_Label: ' SE
        RaiseException("System Exception", "Main Window not found")

        End_05774252_de6b_4703_9edf_5bd04116cf21_Label:

    End Sub

    ''' <summary>
    ''' BluePrism page: Terminate
    ''' </summary>
    Public Sub Terminate()

        ' Terminate
        Application.Element("Microsoft Store", "330860ce-038b-499b-9a6c-c1e8140f72a2").Terminate()

    End Sub

    ''' <summary>
    ''' BluePrism page: Start_Updates
    ''' </summary>
    Public Sub Start_Updates()

        ' W5
        ' Wait 5 seconds for condition with 1 choice(s)
        Select Case True
            Case Application.Element("Downloads", "4bb2d38e-758b-428a-8f6e-a0b6638ab5e3").CheckExists = True ' Downloads Check Exists
                GoTo Navigate_f9be3a31_fdf8_4827_8df3_8ab0fbd5d302_Label
            Case Else
                GoTo Exception_9cb9d9fb_2550_4008_8323_a49fc050de05_Label
        End Select

        Exception_9cb9d9fb_2550_4008_8323_a49fc050de05_Label: ' SE
        RaiseException("System Exception", "Download Menu not found")

        Navigate_f9be3a31_fdf8_4827_8df3_8ab0fbd5d302_Label: ' GoTo Downloads
        Application.Element("Downloads", "4bb2d38e-758b-428a-8f6e-a0b6638ab5e3").UIAAddToSelection()
        ' W5
        ' Wait 5 seconds for condition with 1 choice(s)
        Select Case True
            Case Application.Element("Header: Updates und Downloads", "0340b82e-9c61-45bd-a50b-f5691247a070").CheckExists = True ' Header: Updates und Downloads Check Exists
                GoTo WaitStart_473090d3_a621_4fc2_8436_25fc69a82067_Label
            Case Else
                GoTo Exception_e8b3fd9d_bd7e_4f39_aee3_2bc8c8b1239b_Label
        End Select

        Exception_e8b3fd9d_bd7e_4f39_aee3_2bc8c8b1239b_Label: ' SE
        RaiseException("System Exception", "Download Header not found")

        Navigate_fe0746e7_430a_43f1_8c72_9c40763f31bb_Label: ' Click Nach Updates suchen
        Application.Element("Button: Nach Updates suchen", "2a7db9d3-154c-404c-b610-5c3ae51ecb32").UIAButtonPress()
        GoTo WaitStart_a61f62ae_7993_470f_8d1f_cffdcd5bea5b_Label

        WaitStart_473090d3_a621_4fc2_8436_25fc69a82067_Label: ' W5
        ' Wait 5 seconds for condition with 1 choice(s)
        Select Case True
            Case Application.Element("Button: Nach Updates suchen", "2a7db9d3-154c-404c-b610-5c3ae51ecb32").CheckExists = True ' Button: Nach Updates suchen Check Exists
                GoTo Navigate_fe0746e7_430a_43f1_8c72_9c40763f31bb_Label
            Case Else
                GoTo Exception_ce2d7bf4_fff3_4d86_a6f3_01dd3c9742db_Label
        End Select

        Exception_ce2d7bf4_fff3_4d86_a6f3_01dd3c9742db_Label: ' SE
        RaiseException("System Exception", "Nach Updates suchen Button not found")

        WaitStart_a61f62ae_7993_470f_8d1f_cffdcd5bea5b_Label: ' W120
        ' Wait 120 seconds for condition with 2 choice(s)
        Select Case True
            Case Application.Element("Button: Die Überprüfung auf Updates wurde abgeschlossen.", "397fb78c-8c08-4dc0-987c-3ca33d3762a4").CheckExists = True ' Button: Die Überprüfung auf Updates wurde abgeschlossen. Check Exists
                GoTo WaitStart_cf9ecce9_11c0_48ca_a533_6c59e4ae1d17_Label
            Case Application.Element("Button: Nach Updates suchen", "2a7db9d3-154c-404c-b610-5c3ae51ecb32").CheckExists = True ' Button: Nach Updates suchen Check Exists
                GoTo WaitStart_cf9ecce9_11c0_48ca_a533_6c59e4ae1d17_Label
            Case Else
                GoTo Exception_8c5feb38_8094_4c3f_a292_ac967710a2e8_Label
        End Select

        Exception_8c5feb38_8094_4c3f_a292_ac967710a2e8_Label: ' SE
        RaiseException("System Exception", "Download Header not found")

        Navigate_7a1602d1_74e3_482c_9a3f_58d3e85baf51_Label: ' Click Update All
        ' Navigate: No steps defined
        GoTo WaitStart_a027ef03_3f55_4ae4_ae16_cee7f7dc29f1_Label

        WaitStart_cf9ecce9_11c0_48ca_a533_6c59e4ae1d17_Label: ' W2
        ' Wait 2 seconds for condition with 1 choice(s)
        Select Case True
            Case Application.Element("Button: Nach Updates suchen", "2a7db9d3-154c-404c-b610-5c3ae51ecb32").CheckExists = True ' Button: Nach Updates suchen Check Exists
                GoTo Navigate_7a1602d1_74e3_482c_9a3f_58d3e85baf51_Label
            Case Else
                GoTo Note_95342886_536d_4cc5_93cf_9fb9f009ef0b_Label
        End Select

        WaitStart_a027ef03_3f55_4ae4_ae16_cee7f7dc29f1_Label: ' W5
        ' Wait 5 seconds for condition with 1 choice(s)
        Select Case True
            Case Application.Element("Button: Nach Updates suchen", "2a7db9d3-154c-404c-b610-5c3ae51ecb32").CheckExists = True ' Button: Nach Updates suchen Check Exists
                GoTo End_23ba7d3f_cc9b_4bf6_bb28_545408febcae_Label
            Case Else
                GoTo Exception_1bcc03a0_82b4_44d7_b34f_9edda37ef8ac_Label
        End Select

        Exception_1bcc03a0_82b4_44d7_b34f_9edda37ef8ac_Label: ' SE
        RaiseException("System Exception", "Nach Updates suchen Button not found")

        Note_95342886_536d_4cc5_93cf_9fb9f009ef0b_Label: ' Note1
        ' No Updates
        GoTo End_23ba7d3f_cc9b_4bf6_bb28_545408febcae_Label

        ' Note2
        ' TODO
        GoTo WaitStart_cf9ecce9_11c0_48ca_a533_6c59e4ae1d17_Label

        ' Note2
        ' TODO
        GoTo Navigate_7a1602d1_74e3_482c_9a3f_58d3e85baf51_Label

        ' Note2
        ' TODO
        GoTo WaitStart_a027ef03_3f55_4ae4_ae16_cee7f7dc29f1_Label

        End_23ba7d3f_cc9b_4bf6_bb28_545408febcae_Label:

    End Sub

    ''' <summary>
    ''' BluePrism page: Wait_Updates_Finished
    ''' </summary>
    Public Sub Wait_Updates_Finished()

        ' Note2
        ' TODO

    End Sub

    #End Region

End Class
