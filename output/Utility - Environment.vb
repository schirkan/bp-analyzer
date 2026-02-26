' Generated from BluePrism object: Utility - Environment
' Version: 6.9.0.26970
' Generated: 2026-02-23 23:28:37
' 
' Utilities for interacting with the environment - read screen resolution, determine OS type, etc.
' 
' References:
'   - System.Data.dll
'   - System.Xml.dll
'   - System.Drawing.dll
'   - system.windows.forms.dll
'   - Microsoft.VisualBasic.dll
'   - System.Management.dll
'   - mscorlib.dll
' 
' Imports:
'   - System
'   - System.Drawing
'   - System.Windows.Forms
'   - System.Diagnostics
'   - System.Data
'   - Microsoft.VisualBasic
'   - System.Threading
'   - System.Management
'   - System.Runtime.InteropServices
'   - System.Threading.Tasks

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Data

''' <summary>
''' BluePrism object: Utility - Environment
''' </summary>
Public Class Utility___Environment

    #Region "Global Data Items"

    ' No global data items

    #End Region

    #Region "Methods"

    ''' <summary>
    ''' Constructor - initialization code from stages without subsheet
    ''' </summary>
    Public Sub New()
        ' Constructor body generated from BluePrism global stages

        ' Stage: Start (Start)
            GoTo End_9e4646d0_d8c7_44d6_a5d9_9509e3ba8883_Label
        ' Stage: new (Note)
new_faf2adc8_9198_4483_b6dc_e67e978cbf7b_Label:
            ' Note: Initialise Page

This is an optional page where you might choose to perform some initialisation tasks after your business object is loaded.

The initialise action will be called automatically immediately after loading your business object.

You will not be able to call this action from a business process, nor will it be called at any other time than after the creation of the object.

        ' Stage: Note2 (Note)
Note2_39258065_1e18_4bab_a733_c1dca96896e6_Label:
            ' Note: © 2024 Blue Prism Limited
Licensed under the Blue Prism Asset Terms for Modifiable Assets
https://portal.blueprism.com/agreements

        ' Stage: Version Info (Note)
Version_Info_0eb2c15b_34c0_40ed_b0df_53cf297023e3_Label:
            ' Note: Version: 10.1.8

        ' Stage: End (End)
End_9e4646d0_d8c7_44d6_a5d9_9509e3ba8883_Label:

    End Sub

    ''' <summary>
    ''' BluePrism subsheet: Clear Clipboard (Type: Normal)
    ''' </summary>
    Public Sub Clear_Clipboard()

        ' Stage: Start (Start)
            GoTo Set_Clipboard_d1113981_40f5_450a_8b7e_e059eed37e76_Label
        ' Stage: Set Clipboard (SubSheet)
Set_Clipboard_d1113981_40f5_450a_8b7e_e059eed37e76_Label:
            ' TODO: Implement stage type 'SubSheet'

        ' SubSheet: 
        ' Stage: End (End)
End_e13b6d4e_e05c_491e_ace6_54389e4a99fb_Label:

    End Sub

    ''' <summary>
    ''' BluePrism subsheet: Font Smoothing Enabled (Type: Normal)
    ''' </summary>
    Public Sub Font_Smoothing_Enabled(<Out> ByRef Enabled As Boolean)
        ' Local variables
        Dim Enabled_ As Boolean


        ' Stage: Start (Start)
            GoTo Get_Font_Smoothing_Enabled_96f9fe29_f641_44ab_ab0e_a4091edaec4c_Label
        ' Stage: Get Font Smoothing Enabled (Code)
Get_Font_Smoothing_Enabled_96f9fe29_f641_44ab_ab0e_a4091edaec4c_Label:
            ' Code Stage: Get Font Smoothing Enabled
            ' Original code:
            ' Enabled=System.Windows.Forms.Systeminformation.IsFontSmoothingEnabled
            ' TODO: Convert to VB.Net
            GoTo End_9150eaee_22ec_4228_803e_123c888b87e5_Label

        ' SubSheet: 
        ' Stage: End (End)
End_9150eaee_22ec_4228_803e_123c888b87e5_Label:
            ' Set output parameters
            Enabled = Enabled_

    End Sub

    ''' <summary>
    ''' BluePrism subsheet: Get Clipboard (Type: Normal)
    ''' </summary>
    Public Sub Get_Clipboard(<Out> ByRef Clipboard As String)
        ' Local variables
        Dim Clipboard As String


        ' Stage: Start (Start)
            GoTo Get_106d82b6_72c5_41b6_af2c_3ab3709a9c4e_Label
        ' Stage: Get (Calculation)
Get_106d82b6_72c5_41b6_af2c_3ab3709a9c4e_Label:
            ' Calculation: Clipboard = GetClipboard()
            GoTo End_211e889c_ac33_46c2_8c6a_3d01c6f1ae4a_Label

        ' SubSheet: 
        ' Stage: End (End)
End_211e889c_ac33_46c2_8c6a_3d01c6f1ae4a_Label:
            ' Set output parameters
            Clipboard = Clipboard

    End Sub

    ''' <summary>
    ''' BluePrism subsheet: Get Machine Name (Type: Normal)
    ''' </summary>
    Public Sub Get_Machine_Name(<Out> ByRef Machine_Name As String)
        ' Local variables
        Dim Machine_Name As String


        ' Stage: Start (Start)
            GoTo GetMachineName_61db579f_944a_4ba8_aedc_bdf80bc31cb7_Label
        ' Stage: GetMachineName (Code)
GetMachineName_61db579f_944a_4ba8_aedc_bdf80bc31cb7_Label:
            ' Code Stage: GetMachineName
            ' Original code:
            ' machineName = Environment.MachineName
            ' TODO: Convert to VB.Net
            GoTo End_a69b53e7_8e67_4383_9b6f_28c57c9a82eb_Label

        ' SubSheet: 
        ' Stage: End (End)
End_a69b53e7_8e67_4383_9b6f_28c57c9a82eb_Label:
            ' Set output parameters
            Machine_Name = Machine_Name

    End Sub

    ''' <summary>
    ''' BluePrism subsheet: Get Screen Resolution (Type: Normal)
    ''' </summary>
    Public Sub Get_Screen_Resolution(<Out> ByRef Horizontal_Resolution As Decimal, <Out> ByRef Vertical_Resolution As Decimal)
        ' Local variables
        Dim Horizontal_Resolution As Decimal
        Dim Vertical_Resolution As Decimal


        ' Stage: Start (Start)
            GoTo GetResolution_bd2d0918_9c73_4138_90b8_538eaa06f135_Label
        ' Stage: GetResolution (Code)
GetResolution_bd2d0918_9c73_4138_90b8_538eaa06f135_Label:
            ' Code Stage: GetResolution
            ' Original code:
            ' dim S As Size = Screen.PrimaryScreen.Bounds.Size
            ' Horizontal_Resolution = S.Width
            ' Vertical_Resolution = S.Height
            ' TODO: Convert to VB.Net
            GoTo End_3348aa7d_5b76_4d3f_a590_e571a9a83494_Label

        ' SubSheet: 
        ' Stage: End (End)
End_3348aa7d_5b76_4d3f_a590_e571a9a83494_Label:
            ' Set output parameters
            Horizontal_Resolution = Horizontal_Resolution
            Vertical_Resolution = Vertical_Resolution

    End Sub

    ''' <summary>
    ''' BluePrism subsheet: Get User Name (Type: Normal)
    ''' </summary>
    Public Sub Get_User_Name(<Out> ByRef User_Name As String)
        ' Local variables
        Dim Username As String


        ' Stage: Start (Start)
            GoTo GetUserName_c731c906_58b6_4969_bd70_b7ecd7fa47ae_Label
        ' Stage: GetUserName (Code)
GetUserName_c731c906_58b6_4969_bd70_b7ecd7fa47ae_Label:
            ' Code Stage: GetUserName
            ' Original code:
            ' username = Environment.UserName
            ' TODO: Convert to VB.Net
            GoTo End_ddc4be1a_94ee_4487_b6dc_0f12e4fc40ae_Label

        ' SubSheet: 
        ' Stage: End (End)
End_ddc4be1a_94ee_4487_b6dc_0f12e4fc40ae_Label:
            ' Set output parameters
            User_Name = Username

    End Sub

    ''' <summary>
    ''' BluePrism subsheet: Kill Process (Type: Normal)
    ''' </summary>
    Public Sub Kill_Process(ByVal Process_Name As String, ByVal Process_ID As Decimal)
        ' Local variables
        Dim Process_Name As String
        Dim Process_ID As Decimal


        ' Stage: Start (Start)
            ' Initialize input parameters
            GoTo Kill_Process1_697bc06f_0d64_430d_a5ea_b3fd02b746c8_Label
        ' Stage: Kill Process1 (Code)
Kill_Process1_697bc06f_0d64_430d_a5ea_b3fd02b746c8_Label:
            ' Code Stage: Kill Process1
            ' Original code:
            ' Try
            ' If (Len(Trim(Process_Name)) > 0) And (Process_ID = 0) Then
            ' For Each p As System.Diagnostics.Process in System.Diagnostics.Process.GetProcessesByName(Process_Name)
            ' p.Kill
            ' Next
            ' ElseIf (Process_ID > 0) Then
            ' KillProcessAndChildren(Convert.ToInt32(Process_ID))
            ' End If
            ' Catch ex As Exception
            ' End Try
            ' TODO: Convert to VB.Net
            GoTo End_e9c6833a_30a3_4233_840b_35d605e44199_Label

        ' SubSheet: 
        ' Stage: Input (Block)
Input_45fc84ee_d467_4eb1_bf86_5e843f8298cb_Label:
            ' Block: Input

        ' Stage: End (End)
End_e9c6833a_30a3_4233_840b_35d605e44199_Label:

    End Sub

    ''' <summary>
    ''' BluePrism subsheet: Read Memory Stats (Type: Normal)
    ''' </summary>
    Public Sub Read_Memory_Stats(ByVal Process_Names As DataTable, <Out> ByRef Process_Statistics As DataTable)
        ' Local variables
        Dim Processes As DataTable
        Dim Process_Statistics As DataTable


        ' Stage: Start (Start)
            ' Initialize input parameters
            GoTo Get_Stats_c64eaa40_85d5_42b9_bcfe_0e314328bccb_Label
        ' Stage: Get Stats (Code)
Get_Stats_c64eaa40_85d5_42b9_bcfe_0e314328bccb_Label:
            ' Code Stage: Get Stats
            ' Original code:
            ' GC.Collect()
            ' Process_Statistics = New DataTable
            ' process_statistics.Columns.Add("Process Name", GetType(String))
            ' process_statistics.Columns.Add("PID", GetType(Decimal))
            ' process_statistics.Columns.Add("Working Set", GetType(Decimal))
            ' process_statistics.Columns.Add("Virtual Memory", GetType(Decimal))
            ' For Each R As DataRow in Processes.Rows
            ' dim ProcName As String = CStr(R("Process Name"))
            ' Dim PID As Integer = CInt(R("PID"))
            ' For Each P As Process in Process.GetProcesses()
            ' If P.ProcessName = ProcName OrElse P.ID = PID Then
            ' Process_Statistics.Rows.Add(New Object() {P.ProcessName, P.ID, P.WorkingSet64, P.VirtualMemorySize64})
            ' End If
            ' Next
            ' Next
            ' TODO: Convert to VB.Net
            GoTo End_83912618_be8e_4f5a_860a_03cc449519c4_Label

        ' SubSheet: 
        ' Stage: End (End)
End_83912618_be8e_4f5a_860a_03cc449519c4_Label:
            ' Set output parameters
            Process_Statistics = Process_Statistics

    End Sub

    ''' <summary>
    ''' BluePrism subsheet: Read Process Working Set (Type: Normal)
    ''' </summary>
    Public Sub Read_Process_Working_Set(ByVal Process_Name As String, <Out> ByRef Working_Set As Decimal)
        ' Local variables
        Dim Process_Name As String
        Dim Working_Set As String = "0"


        ' Stage: Start (Start)
            ' Initialize input parameters
            GoTo Get_Memory_Set_92e54184_9c46_425a_a681_ca15d5f38671_Label
        ' Stage: Get Memory Set (Code)
Get_Memory_Set_92e54184_9c46_425a_a681_ca15d5f38671_Label:
            ' Code Stage: Get Memory Set
            ' Original code:
            ' For Each P As Process in Process.GetProcesses()
            ' If P.ProcessName = Proc Then
            ' Working_Set += P.WorkingSet64
            ' End If
            ' Next
            ' TODO: Convert to VB.Net
            GoTo End_d9a48cd9_620d_4b45_87bb_cb1a223d729e_Label

        ' SubSheet: 
        ' Stage: End (End)
End_d9a48cd9_620d_4b45_87bb_cb1a223d729e_Label:
            ' Set output parameters
            Working_Set = Working_Set

    End Sub

    ''' <summary>
    ''' BluePrism subsheet: Run Process Until Ended (Type: Normal)
    ''' </summary>
    Public Sub Run_Process_Until_Ended(ByVal Application As String, ByVal Arguments As String, ByVal Working_Folder As String, ByVal Timeout As TimeSpan, ByVal Ignore_Timeout As Boolean)
        ' Local variables
        Dim Application As String
        Dim Arguments As String
        Dim Timeout As TimeSpan = TimeSpan.Parse("0.00:00:10")
        Dim Fail_Datetime_Reached_ As Boolean = False
        Dim Working_Folder As String
        Dim Ignore_Timeout As Boolean = False


        ' Stage: Start (Start)
            ' Initialize input parameters
            GoTo Run_Process_0b84d4bb_deea_40c5_96eb_6adcf59fe7cf_Label
        ' Stage: Run Process (Code)
Run_Process_0b84d4bb_deea_40c5_96eb_6adcf59fe7cf_Label:
            ' Code Stage: Run Process
            ' Original code:
            ' Dim timeoutInMillisec as Integer
            ' Dim startTime as Date = Date.Now
            ' Dim info as New ProcessStartInfo(appn)
            ' timedOut = False
            ' If args <> "" Then info.Arguments = args
            ' If dir <> "" Then info.WorkingDirectory = dir
            ' ' 20211006
            ' ' Adjusted the logic to account for situations where the use wants the Digital Worker to
            ' ' wait indefinitely until the specified process completes.
            ' If (ignoreTimeout) Then
            ' timeoutInMillisec = -1 ' Infinite wait
            ' Else
            ' timeoutInMillisec = CInt(timeout.TotalMilliseconds)
            ' End If
            ' Using proc As Process = Process.Start(info)
            ' timedOut = Not proc.WaitForExit(timeoutInMillisec)
            ' End Using
            ' TODO: Convert to VB.Net
            GoTo Timed_Out__7fa022c9_fce8_496a_a40b_9119406000db_Label

        ' Stage: Timed Out? (Decision)
Timed_Out__7fa022c9_fce8_496a_a40b_9119406000db_Label:
            ' Decision: If [Fail Datetime Reached?] Then
                GoTo System_Exception_433c4289_a312_4554_bc27_6ee3c7ace153_Label
            Else
                GoTo End_f6878e25_301c_4540_a15d_7a83dfc57a75_Label
            End If

        ' Stage: System Exception (Exception)
System_Exception_433c4289_a312_4554_bc27_6ee3c7ace153_Label:
            ' Throw Exception: System Exception
            ' Detail: "Application " & [Application] & " was still running after the maximum time period"
            Throw New Exception("Application " & [Application] & " was still running after the maximum time period")

        ' SubSheet: 
        ' Stage: Input Args (Block)
Input_Args_dc49d63c_0e29_422f_b99c_4689f5b5afd1_Label:
            ' Block: Input Args

        ' Stage: Variables (Block)
Variables_91242c6b_8086_4fd1_99ea_efb0549f2517_Label:
            ' Block: Variables

        ' Stage: Note3 (Note)
Note3_b9cc5d73_bc01_42f2_9e63_b22a31808b03_Label:
            ' Note: 20201006
The Ignore Timeout flag was added to addres an issue with using a TimeSpan to specify the timeout value. If the user wants the process to wait indefinitely for the process to complete the timeout value needs to be -1. However, you cannot create a TimeSpan with a millisecond value of -1 using the Blue Prism MakeTimeSpan() function. To address this, we added a flag that controls how to code stage handled the timeout value. By doing this we don't break existing deployments that actually make use of the TimeSpan data type for inputting the timeout.

        ' Stage: End (End)
End_f6878e25_301c_4540_a15d_7a83dfc57a75_Label:

    End Sub

    ''' <summary>
    ''' BluePrism subsheet: Set Clipboard (Type: Normal)
    ''' </summary>
    Public Sub Set_Clipboard(ByVal Clipboard As String)
        ' Local variables
        Dim Clipboard As String


        ' Stage: Start (Start)
            ' Initialize input parameters
            GoTo Is_Empty__1e934dc5_5507_4854_bf49_df3d1e55998f_Label
        ' Stage: Is Empty? (Decision)
Is_Empty__1e934dc5_5507_4854_bf49_df3d1e55998f_Label:
            ' Decision: If Len(Trim([Clipboard])) = 0 Then
                GoTo Set_Value_bcb5c983_a5fb_4b30_b14f_7a1509fff980_Label
            Else
                GoTo Set_54826c34_2991_4964_b0a3_0e0a2f951f29_Label
            End If

        ' Stage: Set Value (Calculation)
Set_Value_bcb5c983_a5fb_4b30_b14f_7a1509fff980_Label:
            ' Calculation: Clipboard = Chr(0)
            GoTo Set_54826c34_2991_4964_b0a3_0e0a2f951f29_Label

        ' Stage: Set (Code)
Set_54826c34_2991_4964_b0a3_0e0a2f951f29_Label:
            ' Code Stage: Set
            ' Original code:
            ' 'System.Windows.Forms.Clipboard.SetDataObject(Clipboard)
            ' Dim thread As New Thread(
            ' Sub(ByVal data As Object)
            ' System.Windows.Forms.Clipboard.SetText(data)
            ' End Sub
            ' )
            ' thread.SetApartmentState(ApartmentState.STA)
            ' thread.Start(Clipboard)
            ' thread.join()
            ' TODO: Convert to VB.Net
            GoTo End_d2b91dd7_f7a4_45e5_8b5c_8f867e6be05c_Label

        ' SubSheet: 
        ' Stage: Input (Block)
Input_ca8701cd_9470_4aaf_9bab_6b0b35610381_Label:
            ' Block: Input

        ' Stage: End (End)
End_d2b91dd7_f7a4_45e5_8b5c_8f867e6be05c_Label:

    End Sub

    ''' <summary>
    ''' BluePrism subsheet: Start Process (Type: Normal)
    ''' </summary>
    Public Sub Start_Process(ByVal Application As String, ByVal Arguments As String, ByVal Use_Shell As Boolean, <Out> ByRef Process_ID As Decimal, <Out> ByRef Process_Name As String)
        ' Local variables
        Dim Application As String
        Dim Arguments As String
        Dim Process_ID As Decimal
        Dim Process_Name As String
        Dim Use_Shell As Boolean = True


        ' Stage: Start (Start)
            ' Initialize input parameters
            GoTo Start_Process_8fd5d005_95ec_43c3_acae_de13b914eb3e_Label
        ' Stage: Start Process (Code)
Start_Process_8fd5d005_95ec_43c3_acae_de13b914eb3e_Label:
            ' Code Stage: Start Process
            ' Original code:
            ' Dim processName As String = Application
            ' Dim process As New Process() With {
            ' .StartInfo = New ProcessStartInfo() With {
            ' .FileName = processName,
            ' .Arguments = Arguments,
            ' .UseShellExecute = Use_Shell
            ' }
            ' }
            ' process.Start()
            ' id = Convert.ToInt32(process.Id)
            ' name = process.ProcessName
            ' TODO: Convert to VB.Net
            GoTo End_9c57d68b_5d25_4c88_8bb1_8f7c6aa327d3_Label

        ' SubSheet: 
        ' Stage: Input (Block)
Input_9d967be0_455d_446f_8d7d_7f18f61f2a08_Label:
            ' Block: Input

        ' Stage: Output (Block)
Output_d58ed3fd_2939_4a66_bde7_6b06b5f710d5_Label:
            ' Block: Output

        ' Stage: End (End)
End_9c57d68b_5d25_4c88_8bb1_8f7c6aa327d3_Label:
            ' Set output parameters
            Process_ID = Process_ID
            Process_Name = Process_Name

    End Sub

    ''' <summary>
    ''' BluePrism subsheet: Start Process Read Stderr and Stdout (Type: Normal)
    ''' </summary>
    Public Sub Start_Process_Read_Stderr_and_Stdout(ByVal Process_Name As String, ByVal Arguments As String, ByVal Timeout As Decimal, <Out> ByRef Standard_Output As String, <Out> ByRef Standard_Error As String)
        ' Local variables
        Dim Arguments As String
        Dim Standard_Output As String
        Dim Standard_Error As String
        Dim Process_Name As String
        Dim Timeout As Decimal = -1


        ' Stage: Start (Start)
            ' Initialize input parameters
            GoTo Run_Process_read_Output_d8552129_84eb_4d65_8bee_d06820fb6e5f_Label
        ' Stage: Run Process read Output (Code)
Run_Process_read_Output_d8552129_84eb_4d65_8bee_d06820fb6e5f_Label:
            ' Code Stage: Run Process read Output
            ' Original code:
            ' ' create a Process object
            ' Dim startInfo As New ProcessStartInfo()
            ' startInfo.FileName = Process_Name
            ' startInfo.Arguments = Argument
            ' startInfo.RedirectStandardOutput = True
            ' startInfo.RedirectStandardError = True
            ' startInfo.UseShellExecute = False
            ' startInfo.CreateNoWindow = False
            ' Dim process As New Process()
            ' process.StartInfo = startInfo
            ' ' add handlers to read stdout and stderr content
            ' Dim stdOutput As String = ""
            ' Dim stdError As String = ""
            ' AddHandler process.OutputDataReceived, Sub(sender, args)
            ' If args.Data IsNot Nothing Then
            ' stdOutput += args.Data
            ' End If
            ' End Sub
            ' AddHandler process.ErrorDataReceived, Sub(sender, args)
            ' If args.Data IsNot Nothing Then
            ' stdError += args.Data
            ' End If
            ' End Sub
            ' ' run process until exit or timeout
            ' Try
            ' process.Start()
            ' process.BeginOutputReadLine()
            ' process.BeginErrorReadLine()
            ' Dim timeoutTask As Task = Task.Delay(timeout)
            ' Dim processExitTask As Task = Task.Run(Sub() process.WaitForExit())
            ' If Task.WhenAny(timeoutTask, processExitTask).Result Is timeoutTask Then
            ' Dim timeoutMessage As String = Environment.NewLine & "Timeout reached: " & timeout & "ms"
            ' Standard_Output = stdOutput & timeoutMessage
            ' Standard_Error += stdError & timeoutMessage
            ' process.Kill()
            ' Else
            ' process.WaitForExit()
            ' Standard_Output = stdOutput
            ' Standard_Error = stdError
            ' End If
            ' Catch ex As Exception
            ' Standard_Output = stdOutput
            ' Standard_Error = stdError & Environment.NewLine & "Code stage error: " & ex.Message
            ' Finally
            ' If process IsNot Nothing Then
            ' process.Dispose()
            ' End If
            ' End Try
            ' TODO: Convert to VB.Net
            GoTo End_e9699401_5328_4e3b_ad11_bd29880236e4_Label

        ' SubSheet: 
        ' Stage: Inputs (Block)
Inputs_1fe26b2b_6b1d_4e63_9679_97779a9da64f_Label:
            ' Block: Inputs

        ' Stage: Outputs (Block)
Outputs_679cebbb_478d_4647_9cf9_6459799440aa_Label:
            ' Block: Outputs

        ' Stage: End (End)
End_e9699401_5328_4e3b_ad11_bd29880236e4_Label:
            ' Set output parameters
            Standard_Output = Standard_Output
            Standard_Error = Standard_Error

    End Sub

    ''' <summary>
    ''' BluePrism subsheet: Set Screen Resolution (Type: Normal)
    ''' </summary>
    Public Sub Set_Screen_Resolution(ByVal Vertical_Resolution As Decimal, ByVal Horizontal_Resolution As Decimal, <Out> ByRef Horizontal_Resolution As Decimal, <Out> ByRef Vertical_Resolution As Decimal, <Out> ByRef Success As Boolean, <Out> ByRef Return_Code As Decimal)
        ' Local variables
        Dim Horizontal_Resolution As Decimal
        Dim Vertical_Resolution As Decimal
        Dim Timeout As TimeSpan = TimeSpan.Parse("0.00:00:30")
        Dim Success As Boolean
        Dim Return_Code As Decimal


        ' Stage: Start (Start)
            ' Initialize input parameters
            GoTo Set_Screen_Resolution_e060664b_b423_4175_b38c_3ea531203b49_Label
        ' Stage: Set Screen Resolution (Code)
Set_Screen_Resolution_e060664b_b423_4175_b38c_3ea531203b49_Label:
            ' Code Stage: Set Screen Resolution
            ' Original code:
            ' ' Set the default return value.
            ' Success = False
            ' Return_Code = -2
            ' Dim screen As Screen = screen.PrimaryScreen
            ' Dim iWidth As Integer = Horizontal
            ' Dim iHeight As Integer = Vertical
            ' Dim dm As DEVMODE1 = New DEVMODE1
            ' dm.dmDeviceName = New String(New Char(32) {})
            ' dm.dmFormName = New String(New Char(32) {})
            ' dm.dmSize = CType(Marshal.SizeOf(dm), Short)
            ' Dim modeIndex as Integer = 0
            ' Do While (User_32.EnumDisplaySettings(Nothing, modeIndex, dm) > 0)
            ' If ((dm.dmPelsWidth = iWidth) And (dm.dmPelsHeight = iHeight)) Then
            ' Dim iRet As Integer = User_32.ChangeDisplaySettings(dm, User_32.CDS_TEST)
            ' If iRet = User_32.DISP_CHANGE_FAILED Then
            ' Success = False
            ' Else
            ' iRet = User_32.ChangeDisplaySettings(dm, User_32.CDS_UPDATEREGISTRY)
            ' Select Case iRet
            ' Case User_32.DISP_CHANGE_SUCCESSFUL
            ' ' Success!!
            ' Success = True
            ' Case Else
            ' Success = False
            ' End Select
            ' End If
            ' Return_Code = iRet
            ' Exit Do
            ' End If
            ' modeIndex = modeIndex + 1
            ' Loop
            ' TODO: Convert to VB.Net
            GoTo Success__a06cb66a_a25f_4170_9364_4fa4104a9393_Label

        ' Stage: Success? (Decision)
Success__a06cb66a_a25f_4170_9364_4fa4104a9393_Label:
            ' Decision: If [Success] = True Then
                GoTo End_c6bd6956_f378_418e_8880_30d84f5acad6_Label
            Else
                GoTo Clear_Dimensions_d317d7bf_2ed3_4b48_8e0c_62e5598f7a11_Label
            End If

        ' Stage: Clear Dimensions (MultipleCalculation)
Clear_Dimensions_d317d7bf_2ed3_4b48_8e0c_62e5598f7a11_Label:
            ' Horizontal Resolution = 0
            ' Vertical Resolution = 0
            GoTo End_c6bd6956_f378_418e_8880_30d84f5acad6_Label

        ' SubSheet: 
        ' Stage: Note1 (Note)
Note1_d4a94b84_c140_4d74_9ece_7b75b2548a30_Label:
            ' Note: Note: Make sure you only pass in the screen resolution that is supported

        ' Stage: End (End)
End_c6bd6956_f378_418e_8880_30d84f5acad6_Label:
            ' Set output parameters
            Horizontal_Resolution = Horizontal_Resolution
            Vertical_Resolution = Vertical_Resolution
            Success = Success
            Return_Code = Return_Code

    End Sub

    ''' <summary>
    ''' BluePrism subsheet: Wait for Process (Type: Normal)
    ''' </summary>
    Public Sub Wait_for_Process(ByVal Maximum_wait_time__seconds_ As Decimal, ByVal Process_Name As String, <Out> ByRef Found_ As Boolean)
        ' Local variables
        Dim Maximum_wait_time__seconds_ As Decimal = 0
        Dim Found_ As Boolean = False
        Dim Process_Name As String


        ' Stage: Start (Start)
            ' Initialize input parameters
            GoTo Wait_for_process_dfe0f2dc_5806_4cf9_82f3_269f9b06b03f_Label
        ' Stage: Wait for process (Code)
Wait_for_process_dfe0f2dc_5806_4cf9_82f3_269f9b06b03f_Label:
            ' Code Stage: Wait for process
            ' Original code:
            ' Try
            ' Found_ = False
            ' Dim bProcFound as Boolean = False
            ' Dim iLoop as Integer = 0
            ' Dim myProcesses() As System.Diagnostics.Process
            ' Dim instance As System.Diagnostics.Process
            ' Do While bProcFound = False AND iLoop < Max_Wait
            ' myProcesses = System.Diagnostics.Process.GetProcessesByName(Process_Name)
            ' For Each instance In myProcesses
            ' bProcFound = True
            ' Found_ = True
            ' Next
            ' Threading.Thread.Sleep(1000)
            ' iloop += 1
            ' Loop
            ' Catch ex As Exception
            ' End Try
            ' TODO: Convert to VB.Net
            GoTo End_55ae0a29_1ee0_4fd1_b077_eeb2e6f1e9dd_Label

        ' SubSheet: 
        ' Stage: Input (Block)
Input_a8e06466_b698_4e3a_8cbb_400cd5ef2ec1_Label:
            ' Block: Input

        ' Stage: Output (Block)
Output_0f92247e_231b_4e64_9766_9c660ec5d9fb_Label:
            ' Block: Output

        ' Stage: End (End)
End_55ae0a29_1ee0_4fd1_b077_eeb2e6f1e9dd_Label:
            ' Set output parameters
            Found_ = Found_

    End Sub

    ''' <summary>
    ''' BluePrism subsheet: Wait for Process Window (Type: Normal)
    ''' </summary>
    Public Sub Wait_for_Process_Window(ByVal Process_Name As String, ByVal Window_Title As String, ByVal Wait As Decimal, <Out> ByRef Found As Boolean)
        ' Local variables
        Dim Process_Name As String
        Dim Window_Title As String
        Dim Found As Boolean = False
        Dim Wait As Decimal = 0


        ' Stage: Start (Start)
            ' Initialize input parameters
            GoTo Find_Process_d3b1986a_fa70_41e9_a8af_cd2ffc78342e_Label
        ' Stage: Find Process (Code)
Find_Process_d3b1986a_fa70_41e9_a8af_cd2ffc78342e_Label:
            ' Code Stage: Find Process
            ' Original code:
            ' try
            ' for each p as system.diagnostics.process in system.diagnostics.process.getprocessesbyname(process_name)
            ' if p.mainwindowtitle.trim.tolower like window_title.trim.tolower then
            ' found = true
            ' exit sub
            ' end if
            ' next
            ' catch e as exception
            ' end try
            ' TODO: Convert to VB.Net
            GoTo Found__d0d526bd_c3dd_4eb6_bbe7_2d46cca6e06f_Label

        ' Stage: Found? (Decision)
Found__d0d526bd_c3dd_4eb6_bbe7_2d46cca6e06f_Label:
            ' Decision: If [Found] Then
                GoTo End_b6fdc874_6a8d_405b_946e_10b6b47a6ae1_Label
            Else
                GoTo Wait__a5225d0b_41b3_4ed2_9f7c_31461a1f262a_Label
            End If

        ' Stage: Wait? (Decision)
Wait__a5225d0b_41b3_4ed2_9f7c_31461a1f262a_Label:
            ' Decision: If [Wait]>0 Then
                GoTo Count_Down_ba11e756_90e9_4cd7_a787_a41c2818e4b6_Label
            Else
                GoTo End_b6fdc874_6a8d_405b_946e_10b6b47a6ae1_Label
            End If

        ' Stage: Count Down (Calculation)
Count_Down_ba11e756_90e9_4cd7_a787_a41c2818e4b6_Label:
            ' Calculation: Wait = [Wait]-0.5
            GoTo Wait_4498756a_e9e1_4bef_86e6_3b7ed91a7439_Label

        ' Stage: Wait (WaitStart)
Wait_4498756a_e9e1_4bef_86e6_3b7ed91a7439_Label:
            ' Wait: Wait (Type: WaitStart)
            ' Wait indefinitely
            ' Case Else (Default): GoTo Find_Process_d3b1986a_fa70_41e9_a8af_cd2ffc78342e_Label

        ' SubSheet: 
        ' Stage: End (End)
End_b6fdc874_6a8d_405b_946e_10b6b47a6ae1_Label:
            ' Set output parameters
            Found = Found

    End Sub

    ''' <summary>
    ''' Destructor (CleanUp) - called when object is disposed
    ''' </summary>
    Protected Overrides Sub Finalize()

        ' Stage: Start (Start)
            GoTo End_8b25238a_79e9_40d3_80f6_810d9937f1da_Label
        ' SubSheet: 
        ' Stage: End (End)
End_8b25238a_79e9_40d3_80f6_810d9937f1da_Label:

    End Sub


    #End Region

End Class
