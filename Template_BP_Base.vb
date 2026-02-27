' Template for BluePrism internal helper classes
' This file is copied to the output directory during code generation

Imports System

''' <summary>
''' Base class for all generated BluePrism classes
''' </summary>
Public Class BP_Base

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New()
    End Sub

    #Region "Exception Handling"

    ''' <summary>
    ''' Private variable to store the current exception
    ''' </summary>
    Private Shared _lastException As System.Exception

    ''' <summary>
    ''' Raises a BluePrism exception with type and message
    ''' </summary>
    ''' <param name="exceptionType">The type of exception (e.g., "System Exception")</param>
    ''' <param name="exceptionText">The exception message</param>
    Protected Shared Sub RaiseException(exceptionType As String, exceptionText As String)
        Throw New BP_Exception(exceptionType, exceptionText)
    End Sub

    ''' <summary>
    ''' Stores the current exception using Err.GetException()
    ''' Called in Recover stage to save the exception
    ''' Throws an exception if an exception is already stored
    ''' </summary>
    Protected Shared Sub StoreException()
        If _lastException IsNot Nothing Then
            Throw New Exception("Exception already stored")
        End If
        _lastException = Err.GetException()
    End Sub

    ''' <summary>
    ''' Clears the stored exception
    ''' Called in Resume stage to reset the exception
    ''' </summary>
    Protected Shared Sub ClearException()
        _lastException = Nothing
    End Sub

    ''' <summary>
    ''' Re-throws the stored exception
    ''' Called when usecurrent="yes" in Exception stage
    ''' Throws an exception if no exception is stored
    ''' </summary>
    Protected Shared Sub RethrowException()
        If _lastException Is Nothing Then
            Throw New Exception("No exception stored")
        End If
        Throw _lastException
    End Sub

    ''' <summary>
    ''' Gets the stored exception type
    ''' </summary>
    ''' <returns>The exception type</returns>
    Protected Shared Function ExceptionType() As String
        If _lastException Is Nothing Then
            Throw New Exception("No exception stored")
        End If

        If TypeOf _lastException Is BP_Exception Then
            Return DirectCast(_lastException, BP_Exception).ExceptionType
        End If

        Return "Internal Exception"
    End Function

    ''' <summary>
    ''' Gets the stored exception text
    ''' </summary>
    ''' <returns>The exception text</returns>
    Protected Shared Function ExceptionText() As String
        If _lastException Is Nothing Then
            Throw New Exception("No exception stored")
        End If

        Return _lastException.Message
    End Function

    ''' <summary>
    ''' Returns the current exception
    ''' </summary>
    Protected Shared Function ExceptionStage() As String
        Return "Exception Stage"
    End Function

    #End Region

    #Region "Conversion Functions"

    ''' <summary>
    ''' Converts text to Date
    ''' </summary>
    Protected Shared Function ToDate(Text As String) As DateTime
        Return DateTime.Parse(Text)
    End Function

    ''' <summary>
    ''' Converts text to DateTime
    ''' </summary>
    Protected Shared Function ToDateTime(Text As String) As DateTime
        Return DateTime.Parse(Text)
    End Function

    ''' <summary>
    ''' Converts TimeSpan to days
    ''' </summary>
    Protected Shared Function ToDays(Time As TimeSpan) As Double
        Return Time.TotalDays
    End Function

    ''' <summary>
    ''' Converts TimeSpan to hours
    ''' </summary>
    Protected Shared Function ToHours(Time As TimeSpan) As Double
        Return Time.TotalHours
    End Function

    ''' <summary>
    ''' Converts TimeSpan to minutes
    ''' </summary>
    Protected Shared Function ToMinutes(Time As TimeSpan) As Double
        Return Time.TotalMinutes
    End Function

    ''' <summary>
    ''' Converts text to number
    ''' </summary>
    Protected Shared Function ToNumber(Text As String) As Decimal
        Return Decimal.Parse(Text)
    End Function

    ''' <summary>
    ''' Converts TimeSpan to seconds
    ''' </summary>
    Protected Shared Function ToSeconds(Time As TimeSpan) As Double
        Return Time.TotalSeconds
    End Function

    ''' <summary>
    ''' Converts text to time
    ''' </summary>
    Protected Shared Function ToTime(Text As String) As TimeSpan
        Return TimeSpan.Parse(Text)
    End Function

    ''' <summary>
    ''' Converts bytes to string
    ''' </summary>
    Protected Shared Function Bytes(Data As Byte()) As String
        Return System.Text.Encoding.Default.GetString(Data)
    End Function

    #End Region

    #Region "Date Functions"

    ''' <summary>
    ''' Adds days to a date
    ''' </summary>
    Protected Shared Function AddDays([Date] As DateTime, Days As Integer) As DateTime
        Return [Date].AddDays(Days)
    End Function

    ''' <summary>
    ''' Adds months to a date
    ''' </summary>
    Protected Shared Function AddMonths([Date] As DateTime, Months As Integer) As DateTime
        Return [Date].AddMonths(Months)
    End Function

    ''' <summary>
    ''' Adds an interval to a date
    ''' </summary>
    Protected Shared Function DateAdd(Interval As Integer, Number As Integer, [Date] As DateTime) As DateTime
        Select Case Interval
            Case 0 ' Year
                Return [Date].AddYears(Number)
            Case 1 ' Week
                Return [Date].AddDays(Number * 7)
            Case 4 ' Quarter
                Return [Date].AddMonths(Number * 3)
            Case 5 ' Month
                Return [Date].AddMonths(Number)
            Case 9 ' Day
                Return [Date].AddDays(Number)
            Case Else
                Return [Date]
        End Select
    End Function

    ''' <summary>
    ''' Returns the difference between two dates
    ''' </summary>
    Protected Shared Function DateDiff(Interval As Integer, StartDate As DateTime, EndDate As DateTime) As Long
        Select Case Interval
            Case 0 ' Year
                Return EndDate.Year - StartDate.Year
            Case 1 ' Week
                Return CLng((EndDate - StartDate).TotalDays / 7)
            Case 3 ' Second
                Return CLng((EndDate - StartDate).TotalSeconds)
            Case 4 ' Quarter
                Return (EndDate.Year - StartDate.Year) * 4 + (EndDate.Month - StartDate.Month) \ 3
            Case 5 ' Month
                Return (EndDate.Year - StartDate.Year) * 12 + (EndDate.Month - StartDate.Month)
            Case 6 ' Minute
                Return CLng((EndDate - StartDate).TotalMinutes)
            Case 7 ' Hour
                Return CLng((EndDate - StartDate).TotalHours)
            Case 8 ' Day of year
                Return CLng((EndDate - StartDate).TotalDays)
            Case 9 ' Day
                Return CLng((EndDate - StartDate).TotalDays)
            Case Else
                Return 0
        End Select
    End Function

    ''' <summary>
    ''' Formats a date
    ''' </summary>
    Protected Shared Function FormatDate([Date] As DateTime, Format As String) As String
        Return [Date].ToString(Format)
    End Function

    ''' <summary>
    ''' Formats a datetime
    ''' </summary>
    Protected Shared Function FormatDateTime([Date] As DateTime, Format As String) As String
        Return [Date].ToString(Format)
    End Function

    ''' <summary>
    ''' Formats a UTC datetime
    ''' </summary>
    Protected Shared Function FormatUTCDateTime([Date] As DateTime, Format As String) As String
        Return [Date].ToUniversalTime().ToString(Format)
    End Function

    ''' <summary>
    ''' Returns current local time
    ''' </summary>
    Protected Shared Function LocalTime() As DateTime
        Return DateTime.Now
    End Function

    ''' <summary>
    ''' Creates a date from day, month, year
    ''' </summary>
    Protected Shared Function MakeDate(Day As Integer, Month As Integer, Year As Integer) As DateTime
        Return New DateTime(Year, Month, Day)
    End Function

    ''' <summary>
    ''' Creates a datetime
    ''' </summary>
    Protected Shared Function MakeDateTime(Day As Integer, Month As Integer, Year As Integer, Hours As Integer, Minutes As Integer, Seconds As Integer, Local As Boolean) As DateTime
        If Local Then
            Return New DateTime(Year, Month, Day, Hours, Minutes, Seconds)
        Else
            Return New DateTime(Year, Month, Day, Hours, Minutes, Seconds, DateTimeKind.Utc)
        End If
    End Function

    ''' <summary>
    ''' Creates a time
    ''' </summary>
    Protected Shared Function MakeTime(Hours As Integer, Minutes As Integer, Seconds As Integer) As TimeSpan
        Return New TimeSpan(Hours, Minutes, Seconds)
    End Function

    ''' <summary>
    ''' Creates a TimeSpan
    ''' </summary>
    Protected Shared Function MakeTimeSpan(Days As Integer, Hours As Integer, Minutes As Integer, Seconds As Integer) As TimeSpan
        Return New TimeSpan(Days, Hours, Minutes, Seconds)
    End Function

    ''' <summary>
    ''' Returns current datetime
    ''' </summary>
    Protected Shared Function Now() As DateTime
        Return DateTime.Now
    End Function

    ''' <summary>
    ''' Returns current date
    ''' </summary>
    Protected Shared Function Today() As DateTime
        Return DateTime.Today
    End Function

    ''' <summary>
    ''' Returns current UTC time
    ''' </summary>
    Protected Shared Function UTCTime() As DateTime
        Return DateTime.UtcNow
    End Function

    #End Region

    #Region "Number Functions"

    ''' <summary>
    ''' Pads a number with decimals
    ''' </summary>
    Protected Shared Function DecPad(Number As Decimal, Places As Integer) As String
        Return Number.ToString("F" & Places)
    End Function

    ''' <summary>
    ''' Returns logarithm
    ''' </summary>
    Protected Shared Function Log(Number As Double, Base As Double) As Double
        Return Math.Log(Number, Base)
    End Function

    ''' <summary>
    ''' Rounds down
    ''' </summary>
    Protected Shared Function RndDn(Number As Decimal, Places As Integer) As Decimal
        Dim factor As Decimal = CSng(Math.Pow(10, Places))
        Return Math.Floor(Number * factor) / factor
    End Function

    ''' <summary>
    ''' Rounds up
    ''' </summary>
    Protected Shared Function RndUp(Number As Decimal, Places As Integer) As Decimal
        Dim factor As Decimal = CSng(Math.Pow(10, Places))
        Return Math.Ceiling(Number * factor) / factor
    End Function

    ''' <summary>
    ''' Rounds a number
    ''' </summary>
    Protected Shared Function Round(Number As Decimal, Places As Integer) As Decimal
        Return Math.Round(Number, Places)
    End Function

    ''' <summary>
    ''' Returns square root
    ''' </summary>
    Protected Shared Function Sqrt(Number As Double) As Double
        Return Math.Sqrt(Number)
    End Function

    #End Region

    #Region "Text Functions"

    ''' <summary>
    ''' Returns character from ASCII code
    ''' </summary>
    Protected Shared Function Chr(Code As Integer) As String
        Return ChrW(Code)
    End Function

    ''' <summary>
    ''' Checks if text ends with a string
    ''' </summary>
    Protected Shared Function EndsWith(Text As String, Search As String) As Boolean
        Return Text.EndsWith(Search)
    End Function

    ''' <summary>
    ''' Returns position of substring
    ''' </summary>
    Protected Shared Function InStr(Text As String, Search As String) As Integer
        Dim pos As Integer = Text.IndexOf(Search)
        If pos = -1 Then Return 0
        Return pos + 1
    End Function

    ''' <summary>
    ''' Returns left part of string
    ''' </summary>
    Protected Shared Function Left(Text As String, Length As Integer) As String
        If Length > Text.Length Then Length = Text.Length
        Return Text.Substring(0, Length)
    End Function

    ''' <summary>
    ''' Returns length of string
    ''' </summary>
    Protected Shared Function Len(Text As String) As Integer
        Return Text.Length
    End Function

    ''' <summary>
    ''' Converts to lowercase
    ''' </summary>
    Protected Shared Function Lower(Text As String) As String
        Return Text.ToLower()
    End Function

    ''' <summary>
    ''' Returns substring
    ''' </summary>
    Protected Shared Function Mid(Text As String, StartPoint As Integer, Length As Integer) As String
        If StartPoint > Text.Length Then Return ""
        If StartPoint < 1 Then StartPoint = 1
        If StartPoint + Length > Text.Length Then Length = Text.Length - StartPoint + 1
        Return Text.Substring(StartPoint - 1, Length)
    End Function

    ''' <summary>
    ''' Returns newline
    ''' </summary>
    Protected Shared Function NewLine() As String
        Return Environment.NewLine
    End Function

    ''' <summary>
    ''' Replaces text
    ''' </summary>
    Protected Shared Function Replace(Text As String, Pattern As String, NewText As String) As String
        Return Text.Replace(Pattern, NewText)
    End Function

    ''' <summary>
    ''' Returns right part of string
    ''' </summary>
    Protected Shared Function Right(Text As String, Length As Integer) As String
        If Length > Text.Length Then Length = Text.Length
        Return Text.Substring(Text.Length - Length, Length)
    End Function

    ''' <summary>
    ''' Checks if text starts with a string
    ''' </summary>
    Protected Shared Function StartsWith(Text As String, Search As String) As Boolean
        Return Text.StartsWith(Search)
    End Function

    ''' <summary>
    ''' Trims text
    ''' </summary>
    Protected Shared Function Trim(Text As String) As String
        Return Text.Trim()
    End Function

    ''' <summary>
    ''' Trims end of text
    ''' </summary>
    Protected Shared Function TrimEnd(Text As String) As String
        Return Text.TrimEnd()
    End Function

    ''' <summary>
    ''' Trims start of text
    ''' </summary>
    Protected Shared Function TrimStart(Text As String) As String
        Return Text.TrimStart()
    End Function

    ''' <summary>
    ''' Converts to uppercase
    ''' </summary>
    Protected Shared Function Upper(Text As String) As String
        Return Text.ToUpper()
    End Function

    #End Region

    #Region "File Functions"

    ''' <summary>
    ''' Loads binary file
    ''' </summary>
    Protected Shared Function LoadBinaryFile(Filename As String) As Byte()
        Return System.IO.File.ReadAllBytes(Filename)
    End Function

    ''' <summary>
    ''' Loads text file
    ''' </summary>
    Protected Shared Function LoadTextFile(Filename As String) As String
        Return System.IO.File.ReadAllText(Filename)
    End Function

    #End Region

    #Region "Validation Functions"

    ''' <summary>
    ''' Checks if text is a date
    ''' </summary>
    Protected Shared Function IsDate(Text As String) As Boolean
        Dim result As DateTime
        Return DateTime.TryParse(Text, result)
    End Function

    ''' <summary>
    ''' Checks if text is a datetime
    ''' </summary>
    Protected Shared Function IsDateTime(Text As String) As Boolean
        Dim result As DateTime
        Return DateTime.TryParse(Text, result)
    End Function

    ''' <summary>
    ''' Checks if text is a flag
    ''' </summary>
    Protected Shared Function IsFlag(Text As String) As Boolean
        Return Text.ToLower() = "true" Or Text.ToLower() = "false"
    End Function

    ''' <summary>
    ''' Checks if text is a number
    ''' </summary>
    Protected Shared Function IsNumber(Text As String) As Boolean
        Dim result As Decimal
        Return Decimal.TryParse(Text, result)
    End Function

    ''' <summary>
    ''' Checks if text is a time
    ''' </summary>
    Protected Shared Function IsTime(Text As String) As Boolean
        Dim result As TimeSpan
        Return TimeSpan.TryParse(Text, result)
    End Function

    ''' <summary>
    ''' Checks if text is a timespan
    ''' </summary>
    Protected Shared Function IsTimeSpan(Text As String) As Boolean
        Dim result As TimeSpan
        Return TimeSpan.TryParse(Text, result)
    End Function

    #End Region

    ''' <summary>
    ''' Custom exception for BluePrism errors
    ''' </summary>
    Protected Class BP_Exception
        Inherits System.Exception

        ''' <summary>
        ''' The type of exception (e.g., "System Exception")
        ''' </summary>
        Public ReadOnly Property ExceptionType As String

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="exceptionType">The type of exception</param>
        ''' <param name="exceptionText">The exception message</param>
        Public Sub New(exceptionType As String, exceptionText As String)
            MyBase.New(exceptionText)
            Me.ExceptionType = exceptionType
        End Sub

    End Class

End Class
