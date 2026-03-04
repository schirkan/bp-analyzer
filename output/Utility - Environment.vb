' Generated from BluePrism object: Utility - Environment
' Version: 6.9.0.26970
' Generated: 2026-03-04 20:17:58
' 
' Utilities for interacting with the environment - read screen resolution, determine OS type, etc.

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Diagnostics
Imports Microsoft.VisualBasic
Imports System.Threading
Imports System.Management
Imports System.Runtime.InteropServices
Imports System.Threading.Tasks

''' <summary>
''' BluePrism object: Utility - Environment
''' </summary>
Public Class Utility___Environment
    Inherits BP_Base

    #Region "Singleton Instance"

    ''' <summary>
    ''' Shared singleton instance
    ''' </summary>
    Private Shared ReadOnly _lazyInstance As New Lazy(Of Utility___Environment)(Function() New Utility___Environment())

    Public Shared ReadOnly Property Instance As Utility___Environment
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
    ''' Constructor - initialization code from stages without subsheet
    ''' </summary>
    Public Sub New()

        GoTo End_9e4646d0_d8c7_44d6_a5d9_9509e3ba8883_Label
        ' new
        ' Initialise Page
        ' This is an optional page where you might choose to perform some initialisation tasks after your business object is loaded.
        ' The initialise action will be called automatically immediately after loading your business object.
        ' You will not be able to call this action from a business process, nor will it be called at any other time than after the creation of the object.

        ' Note2
        ' © 2024 Blue Prism Limited
        ' Licensed under the Blue Prism Asset Terms for Modifiable Assets
        ' https://portal.blueprism.com/agreements

        ' Version Info
        ' Version: 10.1.8

        End_9e4646d0_d8c7_44d6_a5d9_9509e3ba8883_Label:

    End Sub

    ''' <summary>
    ''' Clears the clipboard contents.
    ''' </summary>
    Public Sub Clear_Clipboard()

        ' Set Clipboard
        ' TODO: Implement stage type 'SubSheet'

    End Sub

    ''' <summary>
    ''' Gets the font smoothing setting for the current environment.
    ''' </summary>
    ''' <param name="Enabled">True if font smoothing is enabled</param>
    Public Sub Font_Smoothing_Enabled(Optional ByRef Enabled As Boolean = Nothing)

        ' Local variables
        Dim Enabled_ As Boolean

        ' Get Font Smoothing Enabled
        ' Code Stage: Get Font Smoothing Enabled
        ' Original code:
        ' Enabled=System.Windows.Forms.Systeminformation.IsFontSmoothingEnabled
        ' TODO: Convert to VB.Net
        
        Enabled = Enabled_

    End Sub

    ''' <summary>
    ''' Gets the contents of the clipboard.
    ''' </summary>
    ''' <param name="Clipboard">The value from the clipboard.</param>
    Public Sub Get_Clipboard(Optional ByRef Clipboard As String = Nothing)

        ' Get
        Clipboard = GetClipboard()

    End Sub

    ''' <summary>
    ''' BluePrism method: Get_Machine_Name
    ''' </summary>
    ''' <param name="Machine_Name">The hostname of the machine running this action</param>
    Public Sub Get_Machine_Name(Optional ByRef Machine_Name As String = Nothing)

        ' GetMachineName
        ' Code Stage: GetMachineName
        ' Original code:
        ' machineName = Environment.MachineName
        ' TODO: Convert to VB.Net

    End Sub

    ''' <summary>
    ''' Gets the resolution of the screen in pixels for the current environment.
    ''' </summary>
    ''' <param name="Horizontal_Resolution">The number of pixels in the horizontal screen axis</param>
    ''' <param name="Vertical_Resolution">The number of pixels in the vertical screen axis</param>
    Public Sub Get_Screen_Resolution(Optional ByRef Horizontal_Resolution As Decimal = Nothing, Optional ByRef Vertical_Resolution As Decimal = Nothing)

        ' GetResolution
        ' Code Stage: GetResolution
        ' Original code:
        ' dim S As Size = Screen.PrimaryScreen.Bounds.Size
        ' Horizontal_Resolution = S.Width
        ' Vertical_Resolution = S.Height
        ' TODO: Convert to VB.Net

    End Sub

    ''' <summary>
    ''' BluePrism method: Get_User_Name
    ''' </summary>
    ''' <param name="User_Name">The name of the logged in user in the current system</param>
    Public Sub Get_User_Name(Optional ByRef User_Name As String = Nothing)

        ' Local variables
        Dim Username As String

        ' GetUserName
        ' Code Stage: GetUserName
        ' Original code:
        ' username = Environment.UserName
        ' TODO: Convert to VB.Net
        
        User_Name = Username

    End Sub

    ''' <summary>
    ''' If you provide just the Process Name, all processes with the given name will be terminated. If you provide the Process Name and  the Process ID (or just the Process ID), the Process ID takes precendence and only that specific process will be terminated.
    ''' </summary>
    ''' <param name="Process_Name">The name of the process to kill</param>
    ''' <param name="Process_ID">The unique numeric identifier of a specific process on the system.</param>
    Public Sub Kill_Process(Optional ByVal Process_Name As String = Nothing, Optional ByVal Process_ID As Decimal = Nothing)

        ' Kill Process1
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

    End Sub

    ''' <summary>
    ''' Get memory statistics for a given set of processes including the working set and the virtual memory size.
    ''' </summary>
    ''' <param name="Process_Names">The names of the processes to get memory statistics for</param>
    ''' <param name="Process_Statistics">A collection of process statistics including the working set and the virtual memory size</param>
    Public Sub Read_Memory_Stats(Optional ByVal Process_Names As DataTable = Nothing, Optional ByRef Process_Statistics As DataTable = Nothing)

        ' Local variables
        Dim Processes As DataTable

        ' Initialize local variables with input values
        Processes = Process_Names

        ' Get Stats
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

    End Sub

    ''' <summary>
    ''' Gets the size of the working set for the given process.
    ''' </summary>
    ''' <param name="Process_Name">The name of the process you want stats for</param>
    ''' <param name="Working_Set">The working set number holding memory stats for your process</param>
    Public Sub Read_Process_Working_Set(Optional ByVal Process_Name As String = Nothing, Optional ByRef Working_Set As Decimal = Nothing)

        ' Initialize variables with initialvalue
        If Working_Set Is Nothing Then
            Working_Set = "0"
        End If

        ' Get Memory Set
        ' Code Stage: Get Memory Set
        ' Original code:
        ' For Each P As Process in Process.GetProcesses()
        ' If P.ProcessName = Proc Then
        ' Working_Set += P.WorkingSet64
        ' End If
        ' Next
        ' TODO: Convert to VB.Net

    End Sub

    ''' <summary>
    ''' Run a process and wait until completion or timeout.
    ''' </summary>
    ''' <param name="Application">The application or shortcut to start</param>
    ''' <param name="Arguments">Optional - any arguments needed to run the application</param>
    ''' <param name="Working_Folder">Optional - the application working folder</param>
    ''' <param name="Timeout">How long to wait for the application to finish. Default is 10 seconds</param>
    ''' <param name="Ignore_Timeout">Flag that indicates the Digital Worker should wait for the process to complete indefinitely. Default is False.</param>
    Public Sub Run_Process_Until_Ended(Optional ByVal Application As String = Nothing, Optional ByVal Arguments As String = Nothing, Optional ByVal Working_Folder As String = Nothing, Optional ByVal Timeout As TimeSpan = Nothing, Optional ByVal Ignore_Timeout As Boolean = Nothing)

        ' Local variables
        Dim Fail_Datetime_Reached_ As Boolean

        ' Initialize variables with initialvalue
        If Timeout Is Nothing Then
            Timeout = TimeSpan.Parse("0.00:00:10")
        End If
        If Fail_Datetime_Reached_ Is Nothing Then
            Fail_Datetime_Reached_ = False
        End If
        If Ignore_Timeout Is Nothing Then
            Ignore_Timeout = False
        End If

        ' Run Process
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
        ' Timed Out?
        If Fail_Datetime_Reached_ Then
            GoTo Exception_433c4289_a312_4554_bc27_6ee3c7ace153_Label
        Else
            GoTo End_f6878e25_301c_4540_a15d_7a83dfc57a75_Label
        End If

        Exception_433c4289_a312_4554_bc27_6ee3c7ace153_Label: ' System Exception
        RaiseException("System Exception", "Application " & [Application] & " was still running after the maximum time period")

        ' Note3
        ' 20201006
        ' The Ignore Timeout flag was added to addres an issue with using a TimeSpan to specify the timeout value. If the user wants the process to wait indefinitely for the process to complete the timeout value needs to be -1. However, you cannot create a TimeSpan with a millisecond value of -1 using the Blue Prism MakeTimeSpan() function. To address this, we added a flag that controls how to code stage handled the timeout value. By doing this we don't break existing deployments that actually make use of the TimeSpan data type for inputting the timeout.

        End_f6878e25_301c_4540_a15d_7a83dfc57a75_Label:

    End Sub

    ''' <summary>
    ''' Sets the contents of the clipboard.
    ''' </summary>
    ''' <param name="Clipboard">The value to set the clipboard to.</param>
    Public Sub Set_Clipboard(Optional ByVal Clipboard As String = Nothing)

        ' Is Empty?
        If Len(Trim(Clipboard)) = 0 Then
            GoTo Calculation_bcb5c983_a5fb_4b30_b14f_7a1509fff980_Label
        Else
            GoTo Code_54826c34_2991_4964_b0a3_0e0a2f951f29_Label
        End If

        Calculation_bcb5c983_a5fb_4b30_b14f_7a1509fff980_Label: ' Set Value
        Clipboard = Chr(0)
        Code_54826c34_2991_4964_b0a3_0e0a2f951f29_Label: ' Set
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

    End Sub

    ''' <summary>
    ''' Starts a process directly with the given arguments.
    ''' </summary>
    ''' <param name="Application">The application or short cut to start</param>
    ''' <param name="Arguments">Any arguments needed for the app</param>
    ''' <param name="Use_Shell">Optional. Boolean value that determines whether the Windows shell should be used to launch the process. Default value is True. If you experience issue launching a process, try changing this value to False.</param>
    ''' <param name="Process_ID">The unique identifier of the new process.</param>
    ''' <param name="Process_Name">The the name of the process.</param>
    Public Sub Start_Process(Optional ByVal Application As String = Nothing, Optional ByVal Arguments As String = Nothing, Optional ByVal Use_Shell As Boolean = Nothing, Optional ByRef Process_ID As Decimal = Nothing, Optional ByRef Process_Name As String = Nothing)

        ' Initialize variables with initialvalue
        If Use_Shell Is Nothing Then
            Use_Shell = True
        End If

        ' Start Process
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

    End Sub

    ''' <summary>
    ''' Starts a process with the given arguments, reads from the standard output and standard error until the process exits, and outputs them in seperate data items.
    ''' </summary>
    ''' <param name="Process_Name">The name of the process to start.</param>
    ''' <param name="Arguments">The arguments that coincide with the process.</param>
    ''' <param name="Timeout">Optional: The number of milliseconds to wait for the process to exit. Default value is -1 which waits indefinitely.</param>
    Public Sub Start_Process_Read_Stderr_and_Stdout(Optional ByVal Process_Name As String = Nothing, Optional ByVal Arguments As String = Nothing, Optional ByVal Timeout As Decimal = Nothing, Optional ByRef Standard_Output As String = Nothing, Optional ByRef Standard_Error As String = Nothing)

        ' Initialize variables with initialvalue
        If Timeout Is Nothing Then
            Timeout = -1
        End If

        ' Run Process read Output
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

    End Sub

    ''' <summary>
    ''' Sets the resolution of the screen in pixels for the current environment.
    ''' </summary>
    ''' <param name="Horizontal_Resolution">The number of pixels in the horizontal screen axis</param>
    ''' <param name="Vertical_Resolution">The number of pixels in the vertical screen axis</param>
    ''' <param name="Success">A Flag indicating success or failure of the action.</param>
    ''' <param name="Return_Code">The numeric return code returned by the operating system.</param>
    Public Sub Set_Screen_Resolution(Optional ByVal Vertical_Resolution As Decimal = Nothing, Optional ByVal Horizontal_Resolution As Decimal = Nothing, Optional ByRef Success As Boolean = Nothing, Optional ByRef Return_Code As Decimal = Nothing)

        ' Local variables
        Dim Timeout As TimeSpan

        ' Initialize variables with initialvalue
        If Timeout Is Nothing Then
            Timeout = TimeSpan.Parse("0.00:00:30")
        End If

        ' Set Screen Resolution
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
        ' Success?
        If Success = True Then
            GoTo End_c6bd6956_f378_418e_8880_30d84f5acad6_Label
        Else
            GoTo MultipleCalculation_d317d7bf_2ed3_4b48_8e0c_62e5598f7a11_Label
        End If

        MultipleCalculation_d317d7bf_2ed3_4b48_8e0c_62e5598f7a11_Label: ' Clear Dimensions
        Horizontal_Resolution = 0
        Vertical_Resolution = 0
        GoTo End_c6bd6956_f378_418e_8880_30d84f5acad6_Label

        ' Note1
        ' Note: Make sure you only pass in the screen resolution that is supported

        End_c6bd6956_f378_418e_8880_30d84f5acad6_Label:

    End Sub

    ''' <summary>
    ''' Attach to a process and wait for it to complete.
    ''' </summary>
    ''' <param name="Maximum_wait_time__seconds_">The maximum time to wait for the process to complete</param>
    ''' <param name="Process_Name">The name of the process to attach to</param>
    ''' <param name="Found_">True=process found, false=process not found</param>
    Public Sub Wait_for_Process(Optional ByVal Maximum_wait_time__seconds_ As Decimal = Nothing, Optional ByVal Process_Name As String = Nothing, Optional ByRef Found_ As Boolean = Nothing)

        ' Initialize variables with initialvalue
        If Maximum_wait_time__seconds_ Is Nothing Then
            Maximum_wait_time__seconds_ = 0
        End If
        If Found_ Is Nothing Then
            Found_ = False
        End If

        ' Wait for process
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

    End Sub

    ''' <summary>
    ''' Waits for a process with a given name has displayed a window with the given title.
    ''' </summary>
    ''' <param name="Process_Name">The name of the process to attach to</param>
    ''' <param name="Window_Title">The title of the window within the process</param>
    ''' <param name="Wait">The maximum amount of time to wait for</param>
    Public Sub Wait_for_Process_Window(Optional ByVal Process_Name As String = Nothing, Optional ByVal Window_Title As String = Nothing, Optional ByVal Wait As Decimal = Nothing, Optional ByRef Found As Boolean = Nothing)

        ' Initialize variables with initialvalue
        If Found Is Nothing Then
            Found = False
        End If
        If Wait Is Nothing Then
            Wait = 0
        End If

        Code_d3b1986a_fa70_41e9_a8af_cd2ffc78342e_Label: ' Find Process
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
        ' Found?
        If Found Then
            GoTo End_b6fdc874_6a8d_405b_946e_10b6b47a6ae1_Label
        Else
            GoTo Decision_a5225d0b_41b3_4ed2_9f7c_31461a1f262a_Label
        End If

        Decision_a5225d0b_41b3_4ed2_9f7c_31461a1f262a_Label: ' Wait?
        If Wait>0 Then
            GoTo Calculation_ba11e756_90e9_4cd7_a787_a41c2818e4b6_Label
        Else
            GoTo End_b6fdc874_6a8d_405b_946e_10b6b47a6ae1_Label
        End If

        Calculation_ba11e756_90e9_4cd7_a787_a41c2818e4b6_Label: ' Count Down
        Wait = Wait-0.5
        ' Wait
        ' Wait: Wait (Type: WaitStart)
        ' Wait 0.5 seconds for condition with 0 choice(s)
        Select Case True
            Case Else
                GoTo Code_d3b1986a_fa70_41e9_a8af_cd2ffc78342e_Label
        End Select

        End_b6fdc874_6a8d_405b_946e_10b6b47a6ae1_Label:

    End Sub

    ''' <summary>
    ''' Destructor (CleanUp) - called when object is disposed
    ''' </summary>
    Protected Overrides Sub Finalize()

    End Sub

    #End Region

End Class
