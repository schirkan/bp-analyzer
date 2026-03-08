' Generated from BluePrism object: Utility - Environment
' Version: 6.9.0.26970
' Generated: 2026-03-08 00:19:21

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
    ''' Utilities for interacting with the environment - read screen resolution, determine OS type, etc.
    ''' </summary>
    Public Sub New()

        GoTo End__Label

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

        End__Label:

    End Sub

    ''' <summary>
    ''' Clears the clipboard contents.
    ''' </summary>
    Public Sub Clear_Clipboard()

        ' Set Clipboard
        Set_Clipboard(Clipboard:=Chr(0))

    End Sub

    ''' <summary>
    ''' Gets the font smoothing setting for the current environment.
    ''' </summary>
    ''' <param name="Enabled">True if font smoothing is enabled</param>
    Public Sub Font_Smoothing_Enabled(Optional ByRef Enabled As Boolean? = Nothing)

        ' Local variables
        Dim Enabled_ As Boolean?

        ' Get Font Smoothing Enabled
        CodeStage_Get_Font_Smoothing_Enabled(Enabled:=Enabled_)

        Enabled = Enabled_

    End Sub

    ''' <summary>
    ''' Gets the contents of the clipboard.
    ''' </summary>
    ''' <param name="Clipboard">The value from the clipboard.</param>
    Public Sub Get_Clipboard(Optional ByRef Clipboard As String = Nothing)

        
        ' Get
        Calculation_Label:
        Clipboard = GetClipboard()

    End Sub

    ''' <summary>
    ''' BluePrism page: Get_Machine_Name
    ''' </summary>
    ''' <param name="Machine_Name">The hostname of the machine running this action</param>
    Public Sub Get_Machine_Name(Optional ByRef Machine_Name As String = Nothing)

        ' GetMachineName
        CodeStage_GetMachineName(machineName:=Machine_Name)

    End Sub

    ''' <summary>
    ''' Gets the resolution of the screen in pixels for the current environment.
    ''' </summary>
    ''' <param name="Horizontal_Resolution">The number of pixels in the horizontal screen axis</param>
    ''' <param name="Vertical_Resolution">The number of pixels in the vertical screen axis</param>
    Public Sub Get_Screen_Resolution(Optional ByRef Horizontal_Resolution As Decimal? = Nothing, Optional ByRef Vertical_Resolution As Decimal? = Nothing)

        ' GetResolution
        CodeStage_GetResolution(Horizontal_Resolution:=Horizontal_Resolution, Vertical_Resolution:=Vertical_Resolution)

    End Sub

    ''' <summary>
    ''' BluePrism page: Get_User_Name
    ''' </summary>
    ''' <param name="User_Name">The name of the logged in user in the current system</param>
    Public Sub Get_User_Name(Optional ByRef User_Name As String = Nothing)

        ' Local variables
        Dim Username As String

        ' GetUserName
        CodeStage_GetUserName(username:=Username)

        User_Name = Username

    End Sub

    ''' <summary>
    ''' If you provide just the Process Name, all processes with the given name will be terminated. If you provide the Process Name and  the Process ID (or just the Process ID), the Process ID takes precendence and only that specific process will be terminated.
    ''' </summary>
    ''' <param name="Process_Name">The name of the process to kill</param>
    ''' <param name="Process_ID">The unique numeric identifier of a specific process on the system.</param>
    Public Sub Kill_Process(Optional ByVal Process_Name As String = Nothing, Optional ByVal Process_ID As Decimal? = Nothing)

        ' Kill Process1
        CodeStage_Kill_Process1(Process_Name:=Process_Name, Process_ID:=Process_ID)

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
        If Process_Names IsNot Nothing Then
            Processes = Process_Names
        End If

        ' Get Stats
        CodeStage_Get_Stats(Processes:=Processes, Process_Statistics:=Process_Statistics)

    End Sub

    ''' <summary>
    ''' Gets the size of the working set for the given process.
    ''' </summary>
    ''' <param name="Process_Name">The name of the process you want stats for</param>
    ''' <param name="Working_Set">The working set number holding memory stats for your process</param>
    Public Sub Read_Process_Working_Set(Optional ByVal Process_Name As String = Nothing, Optional ByRef Working_Set As Decimal? = Nothing)

        ' Initialize variables with initialvalue
        Working_Set = "0"

        ' Get Memory Set
        CodeStage_Get_Memory_Set(Proc:=Process_Name, Working_Set:=Working_Set)

    End Sub

    ''' <summary>
    ''' Run a process and wait until completion or timeout.
    ''' </summary>
    ''' <param name="Application">The application or shortcut to start</param>
    ''' <param name="Arguments">Optional - any arguments needed to run the application</param>
    ''' <param name="Working_Folder">Optional - the application working folder</param>
    ''' <param name="Timeout">How long to wait for the application to finish. Default is 10 seconds</param>
    ''' <param name="Ignore_Timeout">Flag that indicates the Digital Worker should wait for the process to complete indefinitely. Default is False.</param>
    Public Sub Run_Process_Until_Ended(Optional ByVal Application As String = Nothing, Optional ByVal Arguments As String = Nothing, Optional ByVal Working_Folder As String = Nothing, Optional ByVal Timeout As TimeSpan? = Nothing, Optional ByVal Ignore_Timeout As Boolean? = Nothing)

        ' Local variables
        Dim Fail_Datetime_Reached_ As Boolean?

        ' Initialize variables with initialvalue
        Timeout = TimeSpan.Parse("0.00:00:10")
        Fail_Datetime_Reached_ = False
        Ignore_Timeout = False

        ' Run Process
        CodeStage_Run_Process(appn:=Application, args:=Arguments, dir:=Working_Folder, timeout:=Timeout, ignoreTimeout:=Ignore_Timeout, timedOut:=Fail_Datetime_Reached_)

        ' Timed Out?
        If Fail_Datetime_Reached_ Then
            GoTo Exception_Label
        Else
            GoTo End_Run_Process_Until_Ended_Label
        End If

        ' System Exception
        Exception_Label:
        RaiseException("System Exception", "Application " & [Application] & " was still running after the maximum time period")

        ' Note3
        ' 20201006
        ' The Ignore Timeout flag was added to addres an issue with using a TimeSpan to specify the timeout value. If the user wants the process to wait indefinitely for the process to complete the timeout value needs to be -1. However, you cannot create a TimeSpan with a millisecond value of -1 using the Blue Prism MakeTimeSpan() function. To address this, we added a flag that controls how to code stage handled the timeout value. By doing this we don't break existing deployments that actually make use of the TimeSpan data type for inputting the timeout.

        End_Run_Process_Until_Ended_Label:

    End Sub

    ''' <summary>
    ''' Sets the contents of the clipboard.
    ''' </summary>
    ''' <param name="Clipboard">The value to set the clipboard to.</param>
    Public Sub Set_Clipboard(Optional ByVal Clipboard As String = Nothing)

        ' Is Empty?
        If Len(Trim(Clipboard)) = 0 Then
            GoTo Calculation_2_Label
        Else
            GoTo Code_9_Label
        End If

        ' Set Value
        Calculation_2_Label:
        Clipboard = Chr(0)
        
        ' Set
        Code_9_Label:
        CodeStage_Set(Clipboard:=Clipboard)

    End Sub

    ''' <summary>
    ''' Starts a process directly with the given arguments.
    ''' </summary>
    ''' <param name="Application">The application or short cut to start</param>
    ''' <param name="Arguments">Any arguments needed for the app</param>
    ''' <param name="Use_Shell">Optional. Boolean value that determines whether the Windows shell should be used to launch the process. Default value is True. If you experience issue launching a process, try changing this value to False.</param>
    ''' <param name="Process_ID">The unique identifier of the new process.</param>
    ''' <param name="Process_Name">The the name of the process.</param>
    Public Sub Start_Process(Optional ByVal Application As String = Nothing, Optional ByVal Arguments As String = Nothing, Optional ByVal Use_Shell As Boolean? = Nothing, Optional ByRef Process_ID As Decimal? = Nothing, Optional ByRef Process_Name As String = Nothing)

        ' Initialize variables with initialvalue
        Use_Shell = True

        ' Start Process
        CodeStage_Start_Process(Application:=Application, Arguments:=Arguments, Use_Shell:=Use_Shell, id:=Process_ID, name:=Process_Name)

    End Sub

    ''' <summary>
    ''' Starts a process with the given arguments, reads from the standard output and standard error until the process exits, and outputs them in seperate data items.
    ''' </summary>
    ''' <param name="Process_Name">The name of the process to start.</param>
    ''' <param name="Arguments">The arguments that coincide with the process.</param>
    ''' <param name="Timeout">Optional: The number of milliseconds to wait for the process to exit. Default value is -1 which waits indefinitely.</param>
    Public Sub Start_Process_Read_Stderr_and_Stdout(Optional ByVal Process_Name As String = Nothing, Optional ByVal Arguments As String = Nothing, Optional ByVal Timeout As Decimal? = Nothing, Optional ByRef Standard_Output As String = Nothing, Optional ByRef Standard_Error As String = Nothing)

        ' Initialize variables with initialvalue
        Timeout = -1

        ' Run Process read Output
        CodeStage_Run_Process_read_Output(Argument:=Arguments, Process_Name:=Process_Name, Timeout:=Timeout, Standard_Output:=Standard_Output, Standard_Error:=Standard_Error)

    End Sub

    ''' <summary>
    ''' Sets the resolution of the screen in pixels for the current environment.
    ''' </summary>
    ''' <param name="Horizontal_Resolution">The number of pixels in the horizontal screen axis</param>
    ''' <param name="Vertical_Resolution">The number of pixels in the vertical screen axis</param>
    ''' <param name="Success">A Flag indicating success or failure of the action.</param>
    ''' <param name="Return_Code">The numeric return code returned by the operating system.</param>
    Public Sub Set_Screen_Resolution(Optional ByRef Horizontal_Resolution As Decimal? = Nothing, Optional ByRef Vertical_Resolution As Decimal? = Nothing, Optional ByRef Success As Boolean? = Nothing, Optional ByRef Return_Code As Decimal? = Nothing)

        ' Local variables
        Dim Timeout As TimeSpan?

        ' Initialize variables with initialvalue
        Timeout = TimeSpan.Parse("0.00:00:30")

        ' Set Screen Resolution
        CodeStage_Set_Screen_Resolution(Horizontal:=Horizontal_Resolution, Vertical:=Vertical_Resolution, Success:=Success, Return_Code:=Return_Code)

        ' Success?
        If Success = True Then
            GoTo End_Set_Screen_Resolution_Label
        Else
            GoTo MultipleCalculation_Label
        End If

        ' Clear Dimensions
        MultipleCalculation_Label:
        Horizontal_Resolution = 0
        Vertical_Resolution = 0
        GoTo End_Set_Screen_Resolution_Label

        ' Note1
        ' Note: Make sure you only pass in the screen resolution that is supported

        End_Set_Screen_Resolution_Label:

    End Sub

    ''' <summary>
    ''' Attach to a process and wait for it to complete.
    ''' </summary>
    ''' <param name="Maximum_wait_time__seconds_">The maximum time to wait for the process to complete</param>
    ''' <param name="Process_Name">The name of the process to attach to</param>
    ''' <param name="Found_">True=process found, false=process not found</param>
    Public Sub Wait_for_Process(Optional ByVal Maximum_wait_time__seconds_ As Decimal? = Nothing, Optional ByVal Process_Name As String = Nothing, Optional ByRef Found_ As Boolean? = Nothing)

        ' Initialize variables with initialvalue
        Maximum_wait_time__seconds_ = 0
        Found_ = False

        ' Wait for process
        CodeStage_Wait_for_process(Process_Name:=Process_Name, Max_Wait:=Maximum_wait_time__seconds_, Found_:=Found_)

    End Sub

    ''' <summary>
    ''' Waits for a process with a given name has displayed a window with the given title.
    ''' </summary>
    ''' <param name="Process_Name">The name of the process to attach to</param>
    ''' <param name="Window_Title">The title of the window within the process</param>
    ''' <param name="Wait">The maximum amount of time to wait for</param>
    Public Sub Wait_for_Process_Window(Optional ByVal Process_Name As String = Nothing, Optional ByVal Window_Title As String = Nothing, Optional ByVal Wait As Decimal? = Nothing, Optional ByRef Found As Boolean? = Nothing)

        ' Initialize variables with initialvalue
        Found = False
        Wait = 0

        
        ' Find Process
        Code_14_Label:
        CodeStage_Find_Process(Process_Name:=Process_Name, Window_Title:=Window_Title, Found:=Found)

        ' Found?
        If Found Then
            GoTo End_Wait_for_Process_Window_Label
        Else
            GoTo Decision_5_Label
        End If

        ' Wait?
        Decision_5_Label:
        If Wait>0 Then
            GoTo Calculation_3_Label
        Else
            GoTo End_Wait_for_Process_Window_Label
        End If

        ' Count Down
        Calculation_3_Label:
        Wait = Wait-0.5

        ' Wait
        ' Wait 0.5 seconds for condition with 0 choice(s)
        Select Case True
            Case Else
                GoTo Code_14_Label
        End Select

        End_Wait_for_Process_Window_Label:

    End Sub

    ''' <summary>
    ''' Destructor (CleanUp) - called when object is disposed
    ''' </summary>
    Protected Overrides Sub Finalize()

    End Sub

    #End Region

    #Region "App Model"

    Protected Application As Object

    #End Region

    #Region "Global Code"

        <DllImport("kernel32.dll", SetLastError:=True)> _
        Private Shared Function CreateToolhelp32Snapshot(ByVal dwFlags As UInteger, ByVal th32ProcessID As UInteger) As IntPtr
        End Function
        
        <DllImport("kernel32.dll")> _
        Private Shared Function Process32First(ByVal hSnapshot As IntPtr, ByRef lppe As PROCESSENTRY32) As Boolean
        End Function
        
        <DllImport("kernel32.dll")> _
        Private Shared Function Process32Next(ByVal hSnapshot As IntPtr, ByRef lppe As PROCESSENTRY32) As Boolean
        End Function
        
        <StructLayout(LayoutKind.Sequential)>
        Public Structure PROCESSENTRY32
        	Public dwSize As UInteger
        	Public cntUsage As UInteger
        	Public th32ProcessID As UInteger
        	Public th32DefaultHeapID As IntPtr
        	Public th32ModuleID As UInteger
        	Public cntThreads As UInteger
        	Public th32ParentProcessID As UInteger
        	Public pcPriClassBase As Integer
        	Public dwFlags As UInteger
        	<MarshalAs(UnmanagedType.ByValTStr, SizeConst:=260)>
        	Public szExeFile As String
        End Structure
        
        Shared TH32CS_SNAPPROCESS As UInteger = 2
        
        <StructLayout(LayoutKind.Sequential)> _
        Public Structure DEVMODE1
        	<MarshalAs(UnmanagedType.ByValTStr, SizeConst:=32)> _
        	Public dmDeviceName As String
        	Public dmSpecVersion As Short
        	Public dmDriverVersion As Short
        	Public dmSize As Short
        	Public dmDriverExtra As Short
        	Public dmFields As Integer
        	Public dmOrientation As Short
        	Public dmPaperSize As Short
        	Public dmPaperLength As Short
        	Public dmPaperWidth As Short
        	Public dmScale As Short
        	Public dmCopies As Short
        	Public dmDefaultSource As Short
        	Public dmPrintQuality As Short
        	Public dmColor As Short
        	Public dmDuplex As Short
        	Public dmYResolution As Short
        	Public dmTTOption As Short
        	Public dmCollate As Short
        	<MarshalAs(UnmanagedType.ByValTStr,  SizeConst:=32)> _
        	Public dmFormName As String
        	Public dmLogPixels As Short
        	Public dmBitsPerPel As Short
        	Public dmPelsWidth As Integer
        	Public dmPelsHeight As Integer
        	Public dmDisplayFlags As Integer
        	Public dmDisplayFrequency As Integer
        	Public dmICMMethod As Integer
        	Public dmICMIntent As Integer
        	Public dmMediaType As Integer
        	Public dmDitherType As Integer
        	Public dmReserved1 As Integer
        	Public dmReserved2 As Integer
        	Public dmPanningWidth As Integer
        	Public dmPanningHeight As Integer
        End Structure
        
        Class User_32
        	<DllImport("user32.dll")> _
        	Public Shared Function EnumDisplaySettings(ByVal deviceName As String, ByVal modeNum As Integer, ByRef devMode As DEVMODE1) As Integer
        	End Function
        
        	<DllImport("user32.dll")> _
        	Public Shared Function ChangeDisplaySettings(ByRef devMode As DEVMODE1, ByVal flags As Integer) As Integer
        	End Function
        
        	Public Const ENUM_CURRENT_SETTINGS As Integer = -1
        	Public Const CDS_UPDATEREGISTRY As Integer = 1
        	Public Const CDS_TEST As Integer = 2
        	Public Const DISP_CHANGE_SUCCESSFUL As Integer = 0
        	Public Const DISP_CHANGE_RESTART As Integer = 1
        	Public Const DISP_CHANGE_FAILED As Integer = -1
        End Class
        
        Private Shared Function GetParentProcess(iCurrentPid As Integer ) As Process
        	Dim iParentPid As Integer = 0
        	Dim oHnd As IntPtr = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0)
        	If oHnd = IntPtr.Zero Then Return Nothing
        	Dim oProcInfo As PROCESSENTRY32 = New PROCESSENTRY32()
        	oProcInfo.dwSize = CUInt(System.Runtime.InteropServices.Marshal.SizeOf(GetType(PROCESSENTRY32)))
        	If Process32First(oHnd, oProcInfo) = False Then Return Nothing
        
        	Do
        		If iCurrentPid = oProcInfo.th32ProcessID Then iParentPid = CInt(oProcInfo.th32ParentProcessID)
        	Loop While iParentPid = 0 AndAlso Process32Next(oHnd, oProcInfo)
        
        	If iParentPid > 0 Then
        		Return Process.GetProcessById(iParentPid)
        	Else
        		Return Nothing
        	End If
        End Function
        
        ' Internal subroutine to kill a specific process and any child processes it owns.
        Private Shared Sub KillProcessAndChildren(ByVal pid As Integer)
            Dim processSearcher As ManagementObjectSearcher = New ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" & pid)
            Dim processCollection As ManagementObjectCollection = processSearcher.[Get]()
        
            Try
                Dim proc As Process = Process.GetProcessById(pid)
                If Not proc.HasExited Then proc.Kill()
            Catch __unusedArgumentException1__ As ArgumentException
            End Try
        
            If processCollection IsNot Nothing Then
        
                For Each mo As ManagementObject In processCollection
                    KillProcessAndChildren(Convert.ToInt32(mo("ProcessID")))
                Next
            End If
        End Sub

    #End Region

    #Region "Code Stages"

    ''' <summary>
    ''' GetResolution
    ''' </summary>
    Private Sub CodeStage_GetResolution(Optional ByRef Horizontal_Resolution As Decimal? = Nothing, Optional ByRef Vertical_Resolution As Decimal? = Nothing)

        dim S As Size = Screen.PrimaryScreen.Bounds.Size
        Horizontal_Resolution = S.Width
        Vertical_Resolution = S.Height

    End Sub

    ''' <summary>
    ''' Kill Process1
    ''' </summary>
    Private Sub CodeStage_Kill_Process1(Optional ByVal Process_Name As String = Nothing, Optional ByVal Process_ID As Decimal? = Nothing)

        Try
        	If (Len(Trim(Process_Name)) > 0) And (Process_ID = 0) Then
        		For Each p As System.Diagnostics.Process in System.Diagnostics.Process.GetProcessesByName(Process_Name)
        			p.Kill
        		Next
        	ElseIf (Process_ID > 0) Then
        		KillProcessAndChildren(Convert.ToInt32(Process_ID))
        	End If
        Catch ex As Exception
        End Try

    End Sub

    ''' <summary>
    ''' Get Stats
    ''' </summary>
    Private Sub CodeStage_Get_Stats(Optional ByVal Processes As DataTable = Nothing, Optional ByRef Process_Statistics As DataTable = Nothing)

        GC.Collect()
        Process_Statistics = New DataTable
        process_statistics.Columns.Add("Process Name", GetType(String))
        process_statistics.Columns.Add("PID", GetType(Decimal))
        process_statistics.Columns.Add("Working Set", GetType(Decimal))
        process_statistics.Columns.Add("Virtual Memory", GetType(Decimal))
        
        For Each R As DataRow in Processes.Rows
        	dim ProcName As String = CStr(R("Process Name"))
        	Dim PID As Integer = CInt(R("PID"))
        	For Each P As Process in Process.GetProcesses()
        		If P.ProcessName = ProcName OrElse P.ID = PID Then
        			Process_Statistics.Rows.Add(New Object() {P.ProcessName, P.ID, P.WorkingSet64, P.VirtualMemorySize64})
        		End If
        	Next
        Next

    End Sub

    ''' <summary>
    ''' Get Memory Set
    ''' </summary>
    Private Sub CodeStage_Get_Memory_Set(Optional ByVal Proc As String = Nothing, Optional ByRef Working_Set As Decimal? = Nothing)

        
        For Each P As Process in Process.GetProcesses()
        	If P.ProcessName = Proc Then
        		Working_Set += P.WorkingSet64
        	End If
        Next

    End Sub

    ''' <summary>
    ''' Get Font Smoothing Enabled
    ''' </summary>
    Private Sub CodeStage_Get_Font_Smoothing_Enabled(Optional ByRef Enabled As Boolean? = Nothing)

        Enabled=System.Windows.Forms.Systeminformation.IsFontSmoothingEnabled

    End Sub

    ''' <summary>
    ''' Start Process
    ''' </summary>
    Private Sub CodeStage_Start_Process(Optional ByVal Application As String = Nothing, Optional ByVal Arguments As String = Nothing, Optional ByVal Use_Shell As Boolean? = Nothing, Optional ByRef id As Decimal? = Nothing, Optional ByRef name As String = Nothing)

        Dim processName As String = Application
        
        Dim process As New Process() With {
        	.StartInfo = New ProcessStartInfo() With {
        		.FileName = processName,
        		.Arguments = Arguments,
        		.UseShellExecute = Use_Shell
        	}
        }
        
        process.Start()
        
        id = Convert.ToInt32(process.Id)
        name = process.ProcessName

    End Sub

    ''' <summary>
    ''' Run Process
    ''' </summary>
    Private Sub CodeStage_Run_Process(Optional ByVal appn As String = Nothing, Optional ByVal args As String = Nothing, Optional ByVal dir As String = Nothing, Optional ByVal timeout As TimeSpan? = Nothing, Optional ByVal ignoreTimeout As Boolean? = Nothing, Optional ByRef timedOut As Boolean? = Nothing)

        Dim timeoutInMillisec as Integer
        Dim startTime as Date = Date.Now
        Dim info as New ProcessStartInfo(appn)
        
        timedOut = False
        
        If args <> "" Then info.Arguments = args
        If dir <> "" Then info.WorkingDirectory = dir
        
        ' 20211006
        ' Adjusted the logic to account for situations where the use wants the Digital Worker to
        ' wait indefinitely until the specified process completes.
        If (ignoreTimeout) Then
        	timeoutInMillisec = -1 ' Infinite wait
        Else
        	timeoutInMillisec = CInt(timeout.TotalMilliseconds)
        End If
        
        Using proc As Process = Process.Start(info)
        	timedOut = Not proc.WaitForExit(timeoutInMillisec)
        End Using

    End Sub

    ''' <summary>
    ''' Wait for process
    ''' </summary>
    Private Sub CodeStage_Wait_for_process(Optional ByVal Process_Name As String = Nothing, Optional ByVal Max_Wait As Decimal? = Nothing, Optional ByRef Found_ As Boolean? = Nothing)

        Try
        	Found_ = False
        	Dim bProcFound as Boolean = False
        	Dim iLoop as Integer = 0
        	Dim myProcesses() As System.Diagnostics.Process
        	Dim instance As System.Diagnostics.Process
        	Do While bProcFound = False AND iLoop < Max_Wait
        		myProcesses = System.Diagnostics.Process.GetProcessesByName(Process_Name)
        		For Each instance In myProcesses
        			bProcFound = True
        			Found_ = True
        		Next
        		Threading.Thread.Sleep(1000)
        		iloop += 1
        	Loop
        Catch ex As Exception
        End Try

    End Sub

    ''' <summary>
    ''' Find Process
    ''' </summary>
    Private Sub CodeStage_Find_Process(Optional ByVal Process_Name As String = Nothing, Optional ByVal Window_Title As String = Nothing, Optional ByRef Found As Boolean? = Nothing)

        try
        
        for each p as system.diagnostics.process in system.diagnostics.process.getprocessesbyname(process_name)
        
        	if p.mainwindowtitle.trim.tolower like window_title.trim.tolower then
        		found = true
        		exit sub
        	end if
        
        next
        
        catch e as exception
        end try

    End Sub

    ''' <summary>
    ''' Set
    ''' </summary>
    Private Sub CodeStage_Set(Optional ByVal Clipboard As String = Nothing)

        'System.Windows.Forms.Clipboard.SetDataObject(Clipboard)
        
        Dim thread As New Thread(
        	Sub(ByVal data As Object)
        		System.Windows.Forms.Clipboard.SetText(data)
        	End Sub
        )
        thread.SetApartmentState(ApartmentState.STA)
        thread.Start(Clipboard)
        thread.join()

    End Sub

    ''' <summary>
    ''' GetUserName
    ''' </summary>
    Private Sub CodeStage_GetUserName(Optional ByRef username As String = Nothing)

        
        username = Environment.UserName

    End Sub

    ''' <summary>
    ''' GetMachineName
    ''' </summary>
    Private Sub CodeStage_GetMachineName(Optional ByRef machineName As String = Nothing)

        
        machineName = Environment.MachineName

    End Sub

    ''' <summary>
    ''' Run Process read Output
    ''' </summary>
    Private Sub CodeStage_Run_Process_read_Output(Optional ByVal Argument As String = Nothing, Optional ByVal Process_Name As String = Nothing, Optional ByVal Timeout As Decimal? = Nothing, Optional ByRef Standard_Output As String = Nothing, Optional ByRef Standard_Error As String = Nothing)

        ' create a Process object
        Dim startInfo As New ProcessStartInfo()
        startInfo.FileName = Process_Name
        startInfo.Arguments = Argument
        startInfo.RedirectStandardOutput = True
        startInfo.RedirectStandardError = True
        startInfo.UseShellExecute = False
        startInfo.CreateNoWindow = False
        
        Dim process As New Process()
        process.StartInfo = startInfo
        
        ' add handlers to read stdout and stderr content
        Dim stdOutput As String = ""
        Dim stdError As String = ""
        AddHandler process.OutputDataReceived, Sub(sender, args)
        											If args.Data IsNot Nothing Then
        												stdOutput += args.Data
        											End If
        										End Sub
        AddHandler process.ErrorDataReceived, Sub(sender, args)
        											If args.Data IsNot Nothing Then
        												stdError += args.Data
        											End If
        										End Sub
        
        ' run process until exit or timeout
        Try
        	process.Start()
        	process.BeginOutputReadLine()
        	process.BeginErrorReadLine()
        
        	Dim timeoutTask As Task = Task.Delay(timeout)
        	Dim processExitTask As Task = Task.Run(Sub() process.WaitForExit())
        
        	If Task.WhenAny(timeoutTask, processExitTask).Result Is timeoutTask Then
        		Dim timeoutMessage As String = Environment.NewLine & "Timeout reached: " & timeout & "ms"
        		Standard_Output = stdOutput & timeoutMessage
        		Standard_Error += stdError & timeoutMessage
        		process.Kill()
        	Else
        		process.WaitForExit()
        		Standard_Output = stdOutput
        		Standard_Error = stdError
        	End If
        	
        
        Catch ex As Exception
        	Standard_Output = stdOutput
        	Standard_Error = stdError & Environment.NewLine & "Code stage error: " & ex.Message
        
        Finally
        	If process IsNot Nothing Then
        		process.Dispose()
        	End If
        End Try

    End Sub

    ''' <summary>
    ''' Set Screen Resolution
    ''' </summary>
    Private Sub CodeStage_Set_Screen_Resolution(Optional ByVal Horizontal As Decimal? = Nothing, Optional ByVal Vertical As Decimal? = Nothing, Optional ByRef Success As Boolean? = Nothing, Optional ByRef Return_Code As Decimal? = Nothing)

        ' Set the default return value.
        Success = False
        Return_Code = -2
        
        Dim screen As Screen = screen.PrimaryScreen
        Dim iWidth As Integer = Horizontal
        Dim iHeight As Integer = Vertical
        Dim dm As DEVMODE1 = New DEVMODE1
        
        dm.dmDeviceName = New String(New Char(32) {})
        dm.dmFormName = New String(New Char(32) {})
        dm.dmSize = CType(Marshal.SizeOf(dm), Short)
        
        Dim modeIndex as Integer = 0
        Do While (User_32.EnumDisplaySettings(Nothing, modeIndex, dm) > 0)
        	If ((dm.dmPelsWidth = iWidth) And (dm.dmPelsHeight = iHeight)) Then
        		Dim iRet As Integer = User_32.ChangeDisplaySettings(dm, User_32.CDS_TEST)
        		If iRet = User_32.DISP_CHANGE_FAILED Then
        			Success = False
        		Else
        			iRet = User_32.ChangeDisplaySettings(dm, User_32.CDS_UPDATEREGISTRY)
        		
        			Select Case iRet
        				Case User_32.DISP_CHANGE_SUCCESSFUL
        					' Success!!
        					Success = True
        				Case Else
        					Success = False
        			End Select
        		End If	
        		Return_Code = iRet
        		Exit Do
        	End If
        	modeIndex = modeIndex + 1
        Loop

    End Sub

    #End Region

End Class
