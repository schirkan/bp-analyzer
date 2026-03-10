' Generated from BluePrism object: Utility - Strings
' Version: 6.5.1.14401
' Generated: 2026-03-10 19:14:42

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.XML
Imports System.Diagnostics
Imports Microsoft.VisualBasic
Imports system.text.regularexpressions
Imports Microsoft.VisualBasic.FileIO

''' <summary>
''' BluePrism object: Utility - Strings
''' </summary>
Public Class Utility___Strings
    Inherits BP_Base

    #Region "Singleton Instance"

    Private Shared ReadOnly _lazyInstance As New Lazy(Of Utility___Strings)(Function() New Utility___Strings())

    Public Shared ReadOnly Property Instance As Utility___Strings
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
    ''' Utility functions for manipulating text.
    ''' </summary>
    Public Sub New()

        GoTo End__Label

        ' new
        ' Initialise Page
        ' This is an optional page where you might choose to perform some initialisation tasks after your business object is loaded.
        ' The initialise action will be called automatically immediately after loading your business object.
        ' You will not be able to call this action from a business process, nor will it be called at any other time than after the creation of the object.

        ' Note2
        ' © 2022 Blue Prism Limited
        ' Licensed under the Blue Prism Asset License and Support Terms
        ' https://digitalexchange.blueprism.com/fileMedia/download/a9111324-3192-43ff-9166-566620ca1182

        ' Note3
        ' Version 10.0

        End__Label:

    End Sub

    ''' <summary>
    ''' Destructor (CleanUp) - called when object is disposed
    ''' </summary>
    Protected Overrides Sub Finalize()

        GoTo End_Clean_Up_Label

        ' new
        ' Clean Up Page
        ' This is an optional page where you might choose to perform some finalisation (or "cleanup") tasks as your business object is closed down.
        ' The cleanup action will be called automatically immediately after closing your business object at the end of a business process.
        ' You will not be able to call this action from a business process, nor will it be called at any other time than before the disposal of the business object.

        End_Clean_Up_Label:

    End Sub

    ''' <summary>
    ''' An implementation of the Levenshtein distance algorithm. This is used to calculate the distance between two strings. In this case, distance refers to the number of edits needed to make one string match another. 
    ''' 
    ''' Example: The distance between "Sam" and "Samantha" would be 5. That means 5 edits would be necessary to make either of these strings match the other. You could remove 5 characters from "Samantha" or add 5 characters to "Sam". 
    ''' 
    ''' This is based on the Microsoft TechNet article: https://social.technet.microsoft.com/wiki/contents/articles/26805.c-calculating-percentage-similarity-of-2-strings.aspx
    ''' </summary>
    ''' <param name="Source">The string to compare against the Target string.</param>
    ''' <param name="Target">The string that is being compared against.</param>
    ''' <param name="Case_Sensitive">A flag indicating whether the comparison should take the case (i.e. Upper vs Lower) into account. Default value is True.</param>
    ''' <param name="Distance">The distance between the two strings.</param>
    ''' <param name="Similarity">The percentage similarity of the two input strings.</param>
    Public Sub Calculate_Distance(Optional ByVal Source As String = Nothing, Optional ByVal Target As String = Nothing, Optional ByVal Case_Sensitive As Boolean? = Nothing, Optional ByRef Distance As Decimal? = Nothing, Optional ByRef Similarity As Decimal? = Nothing)

        ' Initialize variables with initialvalue
        Similarity = 0
        Case_Sensitive = True

        ' Check Input
        If (Len(Trim(Source)) > 0) AND (Len(Trim(Target)) > 0) Then
            GoTo Code_Label
        End If

        ' Invalid Input
        RaiseException("Invalid Input Parameter", "Please review and correct your input. Values must be provided for both Source and Target.")

        ' Calculate Levenshtein Distance
        Code_Label:
        CodeStage_Calculate_Levenshtein_Distance(source:=Source, target:=Target, caseSensitive:=Case_Sensitive, distance:=Distance, similarity:=Similarity)

    End Sub

    ''' <summary>
    ''' Compares two items of text read using Font Recognition for equality, where "equality" means that the two text samples match following the removal of conflicting font characters.
    ''' </summary>
    ''' <param name="Sample_1">The first item to be compared</param>
    ''' <param name="Sample_2">The second item to be compared</param>
    ''' <param name="Conflicting_Characters">A collection of conflicting font characters, as read from a read stage for the font of interest</param>
    ''' <param name="Samples_Equal">Indicates whether the samples are equal, once conflicting characters are removed</param>
    ''' <param name="Amended_Sample_1">The first sample, with conflicting characters removed</param>
    ''' <param name="Amended_Sample_2">The second sample, with conflicting characters removed</param>
    Public Sub Compare_Font_Text(Optional ByVal Sample_1 As String = Nothing, Optional ByVal Sample_2 As String = Nothing, Optional ByVal Conflicting_Characters As DataTable = Nothing, Optional ByRef Samples_Equal As Boolean? = Nothing, Optional ByRef Amended_Sample_1 As String = Nothing, Optional ByRef Amended_Sample_2 As String = Nothing)

        ' For Each Character Group
        Conflicting_Characters.SelectFirstRow()
        
        ' Delete from Sample 1
        SubSheet_Label:
        Delete_Characters(Text_Sample:=Sample_1, Characters_to_Delete:=Conflicting_Characters.GetCurrentRow("Character Group").Value, Amended_Sample:=Sample_1)

        ' Delete from Sample 2
        Delete_Characters(Text_Sample:=Sample_2, Characters_to_Delete:=Conflicting_Characters.GetCurrentRow("Character Group").Value, Amended_Sample:=Sample_2)

        ' Next Character Group
        If Conflicting_Characters.SelectNextRow() Then
            GoTo SubSheet_Label
        End If

        ' Determine Equality
        Samples_Equal = Sample_1 = Sample_2

        Amended_Sample_1 = Sample_1
        Amended_Sample_2 = Sample_2

    End Sub

    ''' <summary>
    ''' Removes the given characters from the text.
    ''' </summary>
    ''' <param name="Text_Sample">The piece of text to be operated on</param>
    ''' <param name="Characters_to_Delete">A string of characters to be deleted from the Text Sample</param>
    ''' <param name="Amended_Sample">The amended sample, with the characters deleted</param>
    Public Sub Delete_Characters(Optional ByVal Text_Sample As String = Nothing, Optional ByVal Characters_to_Delete As String = Nothing, Optional ByRef Amended_Sample As String = Nothing)

        ' Delete
        CodeStage_Delete(Text_Sample:=Text_Sample, Characters_to_Delete:=Characters_to_Delete, Amended_Sample:=Amended_Sample)

    End Sub

    ''' <summary>
    ''' Escapes characters to be sent via the sendkeys method, to ensure that all characters are interpreted literally. If left unescaped, some characters such as % carry a special meaning rather than their literal value.
    ''' </summary>
    ''' <param name="Sendkeys_Text">The text to be escaped</param>
    ''' <param name="Escaped_Sendkeys_Text">The escaped sendkeys text, which can be sent via the sendkeys method without fear of misinterpretation</param>
    Public Sub Escape_Sendkeys_String(Optional ByVal Sendkeys_Text As String = Nothing, Optional ByRef Escaped_Sendkeys_Text As String = Nothing)

        ' Escape Text
        CodeStage_Escape_Text(SendKeys_Text:=Sendkeys_Text, Escaped_Sendkeys_Text:=Escaped_Sendkeys_Text)

    End Sub

    ''' <summary>
    ''' Returns a collection with groups as columns and matches as rows.
    ''' </summary>
    ''' <param name="Regex_Pattern">The regex pattern you want to search with.</param>
    ''' <param name="Text_To_Perform_Search_On">The text you want to perform the regex search on.</param>
    ''' <param name="Singleline">OPTIONAL: (Default False) Perform search in “Singleline” mode?</param>
    ''' <param name="Ignore_Case">OPTIONAL: (Default False) Perform search in “Ignore Case” mode?</param>
    ''' <param name="Explicit_Capture">OPTIONAL: (Default False) Perform search in “Explicit Capture” mode?</param>
    ''' <param name="Regex_Matches">A collection of regex search results.</param>
    ''' <param name="Success">True if there was a match. False if not.</param>
    Public Sub Extract_Regex_All_Matches(Optional ByVal Regex_Pattern As String = Nothing, Optional ByVal Text_To_Perform_Search_On As String = Nothing, Optional ByVal Singleline As Boolean? = Nothing, Optional ByVal Ignore_Case As Boolean? = Nothing, Optional ByVal Explicit_Capture As Boolean? = Nothing, Optional ByRef Regex_Matches As DataTable = Nothing, Optional ByRef Success As Boolean? = Nothing)

        ' Initialize variables with initialvalue
        Singleline = False
        Ignore_Case = False
        Explicit_Capture = False

        ' Extract All Matches
        CodeStage_Extract_All_Matches(Regex_Pattern:=Regex_Pattern, Text_To_Perform_Search_On:=Text_To_Perform_Search_On, Singleline:=Singleline, Ignore_Case:=Ignore_Case, Explicit_Capture:=Explicit_Capture, Regex_Matches:=Regex_Matches, Success:=Success)

    End Sub

    ''' <summary>
    ''' Use this action to extract named capture groups. The group names should specified in the input Collection in the 'Name' column.
    ''' </summary>
    ''' <param name="Regex_Pattern">The regex pattern to apply</param>
    ''' <param name="Target_String">The target string to which apply the pattern and extract values</param>
    ''' <param name="Named_Values">A collection of named values to extract from the target string. Note, the Collection should contain two text columns defined as Name and Value.</param>
    ''' <param name="Named_Values">Results of the named values extracted from the regex</param>
    Public Sub Extract_Regex_Values(Optional ByVal Regex_Pattern As String = Nothing, Optional ByVal Target_String As String = Nothing, Optional ByRef Named_Values As DataTable = Nothing)

        ' Initialize variables with initialvalue
        Regex_Pattern = "(?<Lower>\d+)\s*(-|to)\s*(?<Upper>\d+)"
        Target_String = "100-200"

        ' Extract Values
        CodeStage_Extract_Values(Regex_Pattern:=Regex_Pattern, Target_String:=Target_String, Named_Values:=Named_Values, Named_Values_Out:=Named_Values)

    End Sub

    ''' <summary>
    ''' Formats a number into comma-delimited triplets, as appropriate. Eg 123456.789 becomes 123,456.789
    ''' </summary>
    ''' <param name="Input_Number">The number to format</param>
    ''' <param name="Formatted_Currency_Numbers">The resulting formatted number</param>
    Public Sub Format_Number(Optional ByVal Input_Number As Decimal? = Nothing, Optional ByRef Formatted_Currency_Numbers As String = Nothing)

        ' Local variables
        Dim Formatted_Currency_Value As String

        ' Initialize variables with initialvalue
        Input_Number = 987654123456.789

        ' Format
        CodeStage_Format(Input:=Input_Number, Output:=Formatted_Currency_Value)

        Formatted_Currency_Numbers = Formatted_Currency_Value

    End Sub

    ''' <summary>
    ''' Generates a new globally unique identifier as text.
    ''' </summary>
    ''' <param name="GUID">The new guid</param>
    Public Sub Generate_New_GUID(Optional ByRef GUID As String = Nothing)

        ' Get GUID
        CodeStage_Get_GUID(id:=GUID)

    End Sub

    ''' <summary>
    ''' Turns a collection into a list of comma seperated values.
    ''' </summary>
    ''' <param name="Input_Collection">The collection to be converted to CSV</param>
    ''' <param name="Collection_CSV">The CSV representation of the Input Collection</param>
    Public Sub Get_Collection_as_CSV(Optional ByVal Input_Collection As DataTable = Nothing, Optional ByRef Collection_CSV As String = Nothing)

        ' Local variables
        Dim Output_CSV As String

        ' Get Collection as Delimited Text
        Get_Collection_as_Delimited_Text(Input_Collection:=Input_Collection, Delimiter_Character:=",", Output_Delimited_Text:=Output_CSV)

        Collection_CSV = Output_CSV

    End Sub

    ''' <summary>
    ''' Turns a collection into a list of values separated by the provided delimiter character.
    ''' </summary>
    ''' <param name="Input_Collection">The collection to be converted to CSV</param>
    ''' <param name="Delimiter_Character">The character used for delimiting fields in the string</param>
    ''' <param name="Output_Delimited_Text">The delimited text representation of the Input Collection</param>
    Public Sub Get_Collection_as_Delimited_Text(Optional ByVal Input_Collection As DataTable = Nothing, Optional ByVal Delimiter_Character As String = Nothing, Optional ByRef Output_Delimited_Text As String = Nothing)

        ' Serialise to Delimited Text
        CodeStage_Serialise_to_Delimited_Text(Input_Collection:=Input_Collection, Delimiter:=Delimiter_Character, Output_CSV:=Output_Delimited_Text)

    End Sub

    ''' <summary>
    ''' Turns a list of comma seperated values into a collection.
    ''' </summary>
    ''' <param name="CSV">The CSV to be converted into a collection</param>
    ''' <param name="First_Row_Is_Header">Indicates whether the first row of the CSV file should be treated as headers</param>
    ''' <param name="Schema">Optional. A collection of column names. If left blank the column names will be taken from the first row. </param>
    ''' <param name="Output_Collection">The collection converted from CSV</param>
    Public Sub Get_CSV_As_Collection(Optional ByVal CSV As String = Nothing, Optional ByVal First_Row_Is_Header As Boolean? = Nothing, Optional ByVal Schema As DataTable = Nothing, Optional ByRef Output_Collection As DataTable = Nothing)

        ' Initialize variables with initialvalue
        First_Row_Is_Header = False

        ' Get Delimited Text As Collection
        Get_Delimited_Text_As_Collection(First_Row_Is_Header:=First_Row_Is_Header, Schema:=Schema, Delimiter_Character:=",", Delimited_Text:=CSV, Output_Collection:=Output_Collection)

    End Sub

    ''' <summary>
    ''' Turns a list of seperated values into a collection.
    ''' </summary>
    ''' <param name="Delimited_Text">The delimited text to be converted into a collection</param>
    ''' <param name="First_Row_Is_Header">Indicates whether the first row of the CSV file should be treated as headers</param>
    ''' <param name="Schema">Optional. A collection of column names. If left blank the column names will be taken from the first row. </param>
    ''' <param name="Delimiter_Character">The character used for delimiting fields in the string</param>
    ''' <param name="Output_Collection">The collection converted from CSV</param>
    Public Sub Get_Delimited_Text_As_Collection(Optional ByVal Delimited_Text As String = Nothing, Optional ByVal First_Row_Is_Header As Boolean? = Nothing, Optional ByVal Schema As DataTable = Nothing, Optional ByVal Delimiter_Character As String = Nothing, Optional ByRef Output_Collection As DataTable = Nothing)

        ' Initialize variables with initialvalue
        First_Row_Is_Header = False

        ' Parse Delimited String
        CodeStage_Parse_Delimited_String(DelimitedText:=Delimited_Text, Schema:=Schema, FirstRowIsHeader:=First_Row_Is_Header, Delimiter:=Delimiter_Character, outputCollection:=Output_Collection)

    End Sub

    ''' <summary>
    ''' Gets the text representing the two newline characters used under windows (Carriage return followed by Line feed)
    ''' </summary>
    ''' <param name="Newline_Character">The new line text</param>
    Public Sub Get_Newline_Character(Optional ByRef Newline_Character As String = Nothing)

        ' Get Newline Character
        CodeStage_Get_Newline_Character(Newline_Character:=Newline_Character)

    End Sub

    ''' <summary>
    ''' Gets the value of an xml attribute with a given name from an xml document fragment.
    ''' </summary>
    ''' <param name="XML">The xml document to get attributes from</param>
    ''' <param name="Attribute_Name">The name of the xml attribute to get</param>
    ''' <param name="Value">The text value of the requested attribute</param>
    Public Sub Get_XML_Attribute(Optional ByVal XML As String = Nothing, Optional ByVal Attribute_Name As String = Nothing, Optional ByRef Value As String = Nothing)

        ' Local variables
        Dim Attribute_Value As String

        ' Initialize variables with initialvalue
        XML = "<iGrading><Response transactionID=""de3dc0b1-6b22-4b67-a13a-d42fff6188b9"" status=""Success"" method=""SubmitPotentialGradingWithImages"" description="""" /></iGrading>"
        Attribute_Name = "method"

        ' Get Attribute
        CodeStage_Get_Attribute(XML:=XML, Attribute:=Attribute_Name, Value:=Attribute_Value)

        Value = Attribute_Value

    End Sub

    ''' <summary>
    ''' Gets a collection of xml elements with a given name from an xml document.
    ''' </summary>
    ''' <param name="XML">The xml to get elements from</param>
    ''' <param name="Element_Name">The name of the xml elements to get</param>
    ''' <param name="Elements">The collection of xml elements that match the given name and for each element its outer xml</param>
    Public Sub Get_XML_Elements(Optional ByVal XML As String = Nothing, Optional ByVal Element_Name As String = Nothing, Optional ByRef Elements As DataTable = Nothing)

        ' Initialize variables with initialvalue
        XML = "<?xml version=""1.0"" encoding=""utf-8"" ?> <PotentialGrading>  <ServiceIdentifier>ABC</ServiceIdentifier>  <Episode ID=""a97fe424-0d1f-4e7c-9e9a-9b3c9e03d594"">   <Patient DateOfBirth=""18/02/1970"" Gender=""M"" PartPostcode=""YO51"" EthnicOrigin=""W"" Country=""United Kingdom""    Region=""North Yorkshire"" RegisteredBlind=""0"" RegisteredPartiallySighted=""1"" HealthProvider=""PCT""    Insurance=""Cheap as Chips Insurance"">The patient ID goes here</Patient>   <Screening Date=""2007/09/06"" GradingCodeSet=""NGC"" ImageCount=""4"">    <Item Code=""Examiner Classification"" Value=""1"" />    <Item Code=""Eye Screening Urgency"" Value=""2"" />    <Item Code=""Opthalmologist Care"" Value=""2"" />    <Item Code=""Technical Gradability Code"" Value=""1"" />    <Item Code=""01 Visual Acuity Pinhole"" Value=""0"" Laterality=""right"" />    <Item Code=""01 Visual Acuity Pinhole"" Value=""0"" Laterality=""left"" />    <Item Code=""02 Visual Acuity Spectacles"" Value=""0"" Laterality=""right"" />    <Item Code=""02 Visual Acuity Spectacles"" Value=""0"" Laterality=""left"" />    <Item Code=""03 Visual Acuity Standard"" Value=""1"" Laterality=""right"" />    <Item Code=""03 Visual Acuity Standard"" Value=""1"" Laterality=""left"" />    <Item Code=""04 Visual Acuity"" Value="""" Laterality=""right"">Free text entry</Item>    <Item Code=""04 Visual Acuity"" Value="""" Laterality=""left"">Free text entry</Item>    <Item Code=""06 Dilation"" Value=""1"" Laterality=""right"" />    <Item Code=""06 Dilation"" Value=""1"" Laterality=""left"" />    <Image Length=""104644"" CameraID=""Camera1"" CameraModelID=""CameraModel1"" CaptureDateTime=""2007/09/06T01:54:59""     Eye=""L"">1121017(5)L.jpg</Image>    <Image Length=""107565"" CameraID=""Camera2"" CameraModelID=""CameraModel2"" CaptureDateTime=""2007/09/06T01:54:59""     Eye=""L"">1121017(6)L.jpg</Image>    <Image Length=""107441"" CameraID=""Camera3"" CameraModelID=""CameraModel3"" CaptureDateTime=""2007/09/06T01:54:59""     Eye=""R"">1121017(5)R.jpg</Image>    <Image Length=""112472"" CameraID=""Camera4"" CameraModelID=""CameraModel4"" CaptureDateTime=""2007/09/06T01:54:59""     Eye=""R"">1121017(6)R.jpg</Image>   </Screening>   <Notes>Free text notes go here</Notes>  </Episode> </PotentialGrading> "
        Element_Name = "Image"

        ' Blank XML?
        If Len(Trim(XML)) = 0 Then
            GoTo End_Get_XML_Elements_Label
        End If

        ' Get Elements
        CodeStage_Get_Elements(XML:=XML, Element:=Element_Name, Elements:=Elements)
        
        End_Get_XML_Elements_Label:

    End Sub

    ''' <summary>
    ''' Tests to see if one peice of text contains another peice of sub text.
    ''' </summary>
    ''' <param name="Text">Text to search in</param>
    ''' <param name="Search_String">The text to search for</param>
    ''' <param name="Start_Byte">Where in the string to search from</param>
    ''' <param name="Compare_Method">1=text, 0=binary</param>
    ''' <param name="Position">The index of the sub text within the text or -1 if not found</param>
    Public Sub InStr(Optional ByVal Text As String = Nothing, Optional ByVal Search_String As String = Nothing, Optional ByVal Start_Byte As Decimal? = Nothing, Optional ByVal Compare_Method As Decimal? = Nothing, Optional ByRef Position As Decimal? = Nothing)

        ' Initialize variables with initialvalue
        Start_Byte = 0
        Compare_Method = 1

        ' InStr
        CodeStage_InStr(InText:=Text, Search_String:=Search_String, Start_Byte:=Start_Byte, Compare_Method:=Compare_Method, Position:=Position)
        GoTo End_InStr_Label

        ' Note1
        ' Inputs

        ' Note1
        ' Outputs

        End_InStr_Label:

    End Sub

    ''' <summary>
    ''' Tests to see if one peice of text contains another peice of sub text but matches in reverse.
    ''' </summary>
    ''' <param name="Text">Text to search in</param>
    ''' <param name="Search_String">The text to search for</param>
    ''' <param name="Start_Byte">Where in the string to search from</param>
    ''' <param name="Compare_Method">1=text, 0=binary</param>
    ''' <param name="Position">The index of the sub text within the text from the end or -1 if not found</param>
    Public Sub InStrRev(Optional ByVal Text As String = Nothing, Optional ByVal Search_String As String = Nothing, Optional ByVal Start_Byte As Decimal? = Nothing, Optional ByVal Compare_Method As Decimal? = Nothing, Optional ByRef Position As Decimal? = Nothing)

        ' Initialize variables with initialvalue
        Start_Byte = -1
        Compare_Method = 1

        ' InStrRev
        CodeStage_InStrRev(InText:=Text, Search_String:=Search_String, Start_Byte:=Start_Byte, Compare_Method:=Compare_Method, Position:=Position)
        GoTo End_InStrRev_Label

        ' Note1
        ' Inputs

        ' Note1
        ' Outputs

        End_InStrRev_Label:

    End Sub

    ''' <summary>
    ''' Joins values from a collection into multiline text.
    ''' </summary>
    ''' <param name="Values">The text values collection to join</param>
    ''' <param name="Trim_Lines">Set true to apply trimming to the lines</param>
    ''' <param name="Joined_Text">The resulting joined text</param>
    Public Sub Join_Lines(Optional ByVal Values As DataTable = Nothing, Optional ByVal Trim_Lines As Boolean? = Nothing, Optional ByRef Joined_Text As String = Nothing)

        ' Local variables
        Dim Join_Character As String

        ' Get Carriage Return
        CodeStage_Get_Carriage_Return(Join_Character:=Join_Character)

        ' Join Text
        Join_Text(Values:=Values, Join_Character:=Join_Character, Trim_Values:=Trim_Lines, Joined_Text:=Joined_Text)

    End Sub

    ''' <summary>
    ''' Joins values from a collection into a single line of text using a given delimiter between values.
    ''' </summary>
    ''' <param name="Values">The text values collection to join</param>
    ''' <param name="Join_Character">The delimeter between text values</param>
    ''' <param name="Trim_Values">Set true to apply trimming to the values</param>
    ''' <param name="Joined_Text">The resulting joined text</param>
    Public Sub Join_Text(Optional ByVal Values As DataTable = Nothing, Optional ByVal Join_Character As String = Nothing, Optional ByVal Trim_Values As Boolean? = Nothing, Optional ByRef Joined_Text As String = Nothing)

        ' Initialize variables with initialvalue
        Trim_Values = False

        ' Reset Output
        Joined_Text = ""

        ' For Each Value
        Values.SelectFirstRow()
        
        ' Trim?
        Decision_3_Label:
        If Trim_Values Then
            GoTo Calculation_3_Label
        End If
        
        ' Append Value
        Calculation_4_Label:
        Joined_Text = Joined_Text & Values.GetCurrentRow("Item Value").Value & Join_Character

        ' Next Value
        If Values.SelectNextRow() Then
            GoTo Decision_3_Label
        End If
        GoTo End_Join_Text_Label

        ' Do Trim
        Calculation_3_Label:
        Values.GetCurrentRow("Item Value").Value = Trim(Values.GetCurrentRow("Item Value").Value)
        GoTo Calculation_4_Label

        End_Join_Text_Label:

    End Sub

    ''' <summary>
    ''' Ensures that a string (usually a number) is of fixed width, by padding with a special character on the left.
    ''' </summary>
    ''' <param name="Input_String">The string to pad</param>
    ''' <param name="Target_Width">The total number of characters required after padding</param>
    ''' <param name="Padding_Character">The character to pad with</param>
    ''' <param name="Padded_String">The resultant padded string</param>
    Public Sub PadLeft(Optional ByVal Input_String As String = Nothing, Optional ByVal Target_Width As Decimal? = Nothing, Optional ByVal Padding_Character As String = Nothing, Optional ByRef Padded_String As String = Nothing)

        ' Initialize variables with initialvalue
        Input_String = "123"
        Target_Width = 5

        ' Blank Padding Character?
        If Len(Padding_Character) = 0 Then
            GoTo Calculation_5_Label
        End If
        
        ' Long Enough?
        Decision_5_Label:
        If Len(Input_String) >= Target_Width Then
            GoTo End_PadLeft_Label
        End If

        ' Insert Padding
        Input_String = Padding_Character & Input_String
        GoTo Decision_5_Label

        ' Use Space for Padding
        Calculation_5_Label:
        Padding_Character = " "
        GoTo Decision_5_Label

        End_PadLeft_Label:
        Padded_String = Input_String

    End Sub

    ''' <summary>
    ''' Use this action to replace a sub string(s) within another string using a Regular Expression.
    ''' </summary>
    ''' <param name="Pattern">The regular expression to use to match for replacement.</param>
    ''' <param name="Input_Data">The string to perform the replacement pattern match on.</param>
    ''' <param name="Replacement_Data">The string to use as the replacement data.</param>
    ''' <param name="Max_Count">OPTIONAL: The maximum number of replacements to perform. Default value is unlimited (-1).</param>
    ''' <param name="Start_Position">OPTIONAL: The starting position with the input data to start the replacement match. Default value is position 0.</param>
    ''' <param name="Output_Data">The new string data after the replacement has been performed.</param>
    Public Sub Regex_Replace(Optional ByVal Pattern As String = Nothing, Optional ByVal Input_Data As String = Nothing, Optional ByVal Replacement_Data As String = Nothing, Optional ByVal Max_Count As Decimal? = Nothing, Optional ByVal Start_Position As Decimal? = Nothing, Optional ByRef Output_Data As String = Nothing)

        ' Initialize variables with initialvalue
        Max_Count = -1
        Start_Position = 0

        ' Check Max Count
        If Max_Count >= -1 Then
            GoTo Decision_7_Label
        End If
        
        ' Invalid Input Data
        Exception_2_Label:
        RaiseException("Invalid Input Parameter", "Please verify your input data.")

        ' Check Start Pos
        Decision_7_Label:
        If Start_Position >= 0 Then
            GoTo Code_16_Label
        End If
        GoTo Exception_2_Label

        ' Perform Replace
        Code_16_Label:
        CodeStage_Perform_Replace(pattern:=Pattern, input:=Input_Data, replacement:=Replacement_Data, max:=Max_Count, start:=Start_Position, output:=Output_Data)

    End Sub

    ''' <summary>
    ''' Trims non word characters from the begining and end of the text. Non word characters are any character that is NOT in the ranges a-z A-Z _ and 0-9
    ''' </summary>
    ''' <param name="Text">The text to remove the non word characters from</param>
    ''' <param name="Trimmed_Text">The text with the non word characters removed</param>
    Public Sub Remove_Non_word_Characters(Optional ByVal Text As String = Nothing, Optional ByRef Trimmed_Text As String = Nothing)

        ' Initialize variables with initialvalue
        Text = "    qwerqwer    "

        ' Trim
        CodeStage_Trim(Text:=Text, Trimmed_Text:=Trimmed_Text)

    End Sub

    ''' <summary>
    ''' Replaces the specified existing characters in the text with the new characetrs.
    ''' </summary>
    ''' <param name="Text_Sample">The piece of text to be operated on</param>
    ''' <param name="Characters_to_Replace">A string of characters to be replaced in the Text Sample</param>
    ''' <param name="Replacement_Characters">The new characters to add to the Text Sample in place the original characters.</param>
    ''' <param name="Amended_Sample">The amended sample, with the characters replaced</param>
    Public Sub Replace_Characters(Optional ByVal Text_Sample As String = Nothing, Optional ByVal Characters_to_Replace As String = Nothing, Optional ByVal Replacement_Characters As String = Nothing, Optional ByRef Amended_Sample As String = Nothing)

        ' Replace
        CodeStage_Replace(Text_Sample:=Text_Sample, Characters_to_Replace:=Characters_to_Replace, Replacement_Characters:=Replacement_Characters, Amended_Sample:=Amended_Sample)

    End Sub

    ''' <summary>
    ''' Splits multiple line text into a collection text values with a single row per line.
    ''' </summary>
    ''' <param name="Text_to_Split">The text to split</param>
    ''' <param name="Split_Values">The resulting collection containing the split values</param>
    Public Sub Split_Lines(Optional ByVal Text_to_Split As String = Nothing, Optional ByRef Split_Values As DataTable = Nothing)

        ' Split
        CodeStage_Split(Text_to_Split:=Text_to_Split, Split_Values:=Split_Values)

    End Sub

    ''' <summary>
    ''' Splits text into lines of a given length using word boundries to find the split point.
    ''' </summary>
    ''' <param name="Text_to_Split">The single line of text that needs to be split</param>
    ''' <param name="Maximum_Line_Length">The maximum length of the line</param>
    ''' <param name="Split_Strictly_by_Length">Set true if the line should be split at character boundries instead of words</param>
    Public Sub Split_Lines_by_Length(Optional ByVal Text_to_Split As String = Nothing, Optional ByVal Maximum_Line_Length As Decimal? = Nothing, Optional ByVal Split_Strictly_by_Length As Boolean? = Nothing, Optional ByRef Line_Count As Decimal? = Nothing, Optional ByRef Split_Lines As DataTable = Nothing)

        ' Initialize variables with initialvalue
        Text_to_Split = "aaaa bbbb cccc dddd eeee ffff gggg hhhh iiii jjjj kkkk llll mmmm nnnn oooo pppp"
        Maximum_Line_Length = 6
        Split_Strictly_by_Length = False

        ' Split Lines By Length
        CodeStage_Split_Lines_By_Length(Text_to_Split:=Text_to_Split, Maximum_Line_Length:=Maximum_Line_Length, Strict_Split:=Split_Strictly_by_Length, Split_Lines:=Split_Lines, Line_Count:=Line_Count)

    End Sub

    ''' <summary>
    ''' Splits text with a given delimiter into a collection of text values.
    ''' </summary>
    ''' <param name="Text_to_Split">The text to split</param>
    ''' <param name="Split_Char">The split delimiter</param>
    ''' <param name="Collection_Field_Name">The name of the field for the resulting collection</param>
    ''' <param name="Split_Values">The resulting collection containing the split values</param>
    Public Sub Split_Text(Optional ByVal Text_to_Split As String = Nothing, Optional ByVal Split_Char As String = Nothing, Optional ByVal Collection_Field_Name As String = Nothing, Optional ByRef Split_Values As DataTable = Nothing)

        ' Split Text
        CodeStage_Split_Text(Text_to_Split:=Text_to_Split, Split_Char:=Split_Char, Collection_Field_Name:=Collection_Field_Name, Split_Values:=Split_Values)

    End Sub

    ''' <summary>
    ''' Check if a given Text value matches a regular expression.
    ''' </summary>
    ''' <param name="Regex_Pattern">The regex pattern to apply</param>
    ''' <param name="Target_String">The target string to which apply the pattern and extract values</param>
    ''' <param name="Matched_">Whether or not the regex match</param>
    Public Sub Test_Regex_Match(Optional ByVal Regex_Pattern As String = Nothing, Optional ByVal Target_String As String = Nothing, Optional ByRef Matched_ As Boolean? = Nothing)

        ' Local variables
        Dim Regex_Match As Boolean?

        ' Test Regex Match1
        CodeStage_Test_Regex_Match1(Regex_Pattern:=Regex_Pattern, Target_String:=Target_String, Regex_Match:=Regex_Match)

        Matched_ = Regex_Match

    End Sub

    #End Region

    #Region "App Model"

    Protected Application As Object

    #End Region

    #Region "Global Code"

        Public Function GetDataTable(ByVal ColumnNamesCSV As String, ByVal ColumnTypesCSV As String) As DataTable
        
        	Dim objTable As DataTable
        	Dim objColumn As DataColumn
        	Dim aColumnNames As String() = ColumnNamesCSV.Split(",")
        	Dim aColumnTypes As String() = ColumnTypesCSV.Split(",")
        
        	Try
        		objTable = New DataTable
        		For i As Integer = 0 To aColumnNames.Length - 1
        			objColumn = New DataColumn 
        			objColumn.DataType = System.Type.GetType(aColumnTypes(i).Trim)
        			objColumn.ColumnName = aColumnNames(i).Trim
        			objTable.Columns.Add(objColumn)
        		Next
        		
        	Catch e As Exception
        		objTable = nothing	
        	End Try
        
        	Return objTable
        
        End Function
        
        Private Function SplitStringInto( _
         ByVal fldName As String, _
         ByVal txt As String, _
         ByVal ParamArray splitters() As String) As DataTable
        	Dim dt As New DataTable()
        	dt.Columns.Add(fldName, GetType(String))
        
        	For Each s As String In txt.Split(splitters, StringSplitOptions.None)
        		dt.Rows.Add(New Object() {s})
        	Next
        
        	Return dt
        End Function
        
        Public Shared Function ParseCsvToList(ByVal csv As String, ByVal delimiter As String) As List(Of String())
        	Dim result = New List(Of String())()
        
        	Using sr As New StringReader(csv)
        		Using lineParser As New TextFieldParser(sr)
        			lineParser.TextFieldType = FieldType.Delimited
        			lineParser.SetDelimiters(delimiter)
        			While Not lineParser.EndOfData
        
        				Dim fields As String() = lineParser.ReadFields()
        				result.Add(fields)
        			End While
        		End Using
        	End Using
        
        	Return result
        End Function
        
        
        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Serialises a datatable to the supplied stream.
        ''' </summary>
        ''' <param name="Writer">The stream writr to which the datatable should
        ''' be serialised. Eg this may correspond to a file stream.</param>
        ''' <param name="Table">The datatable to be serialised.</param>
        ''' <param name="IncludeHeaderRow">When true, the column headers will
        ''' be wrtten out on the first row.</param>
        ''' -----------------------------------------------------------------------------
        Public Shared Sub WriteDataTable(ByVal Writer As StringBuilder, ByVal Table As DataTable, ByVal IncludeHeaderRow As Boolean, ByVal delimiter As String)
        	If (delimiter.Trim().Length = 0)
        		delimiter = ","
        	End If
        
        	If IncludeHeaderRow Then
        		For i As Integer = 0 To Table.Columns.Count - 1
        			WriteItem(Writer, Table.Columns(i).ColumnName)
        			If i < Table.Columns.Count - 1 Then
        				Writer.Append(delimiter)
        			Else
        				Writer.Append(vbCrLf)
        			End If
        		Next
        	End If
        
        	For Each Row As DataRow In Table.Rows
        		For i As Integer = 0 To Table.Columns.Count - 1
        			WriteItem(Writer, Row(i).ToString)
        			If i < Table.Columns.Count - 1 Then
        				Writer.Append(delimiter)
        			Else
        				Writer.Append(vbCrLf)
        			End If
        		Next
        	Next
        End Sub
        
        
        
        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Writes a csv data cell, escaping quotes and new lines where necessary.
        ''' </summary>
        ''' <param name="Writer">The writer to which the item should be written.</param>
        ''' <param name="Value">The value to be written.</param>
        ''' -----------------------------------------------------------------------------
        Private Shared Sub WriteItem(ByVal Writer As StringBuilder, ByVal Value As String)
        	If Value.IndexOfAny(("""," & vbCrLf).ToCharArray) > -1 Then
        		Writer.Append("""" & Value.Replace("""", """""") & """")
        	Else
        		Writer.Append(Value)
        	End If
        End Sub
        
        
        'Splits text into lines of approximately equal length, looking for
        'gaps between words as splitting points in order to avoid ugly
        'line splitting in the middle of words. Useful for mainframe memos
        'where a long message needs to be broken into lines of up to 80
        'characters.
        '
        Private Shared Function SplitTextByLengthEngine(Texttosplit As String, MaxLineLength As Integer) As List(of String)
        	Dim RetVal as New List(Of String)
        	MaxLineLength =  Math.Min(MaxLineLength, TexttoSplit.Length)
        
        	'We look for the last space within (MaxLineLength + 1) and then work backwards
        	'(always by at least one) to find the last non-space character. We can then
        	'chop at this point, assuming such exists. Otherwise we just chop at the
        	'requested line length accepting we will be splitting a word.
        	Dim LastIndex as integer = TexttoSplit.Substring(0, Math.Min(MaxLineLength + 1, TextToSplit.Length)).LastIndexOf(" ")
        
        	If TextToSplit.Length <= MaxLineLength OrElse LastIndex = -1 Then
        		RetVal.Add(TexttoSplit.Substring(0, MaxLineLength))
        		Dim RemainingText As String = TexttoSplit.SubString(MaxLineLength,TextToSplit.Length - MaxLineLength).Trim()
        		If RemainingText.Length > 0 Then RetVal.AddRange(SplitTextByLengthEngine(RemainingText, MaxLineLength))
        	Else
        		'Track backwards to find previous non-space character
        		Dim Index As Integer = LastIndex - 1
        		While Index >= 0 Andalso TextToSplit.SubString(Index, 1) = " "
        			Index -=1
        		End While
        		If Index >= 0 Then
        			RetVal.Add(TextToSplit.SubString(0, Index + 1))
        			Dim RemainingText As String = TexttoSplit.SubString(Index + 1, TextToSplit.Length - (Index + 1)).Trim()
        			If RemainingText.Length > 0 Then RetVal.AddRange(SplitTextByLengthEngine(RemainingText, MaxLineLength))
        		Else
        			'Must all be spaces. We assume this are to be ignored
        		End If
        	End If
        
        	Return RetVal
        End Function
        
        Private Shared Function CreateRegexOptions(Singleline As Boolean, IgnoreCase As Boolean, IgnoreWhitespace As Boolean, CultureInvariant As Boolean, ExplicitCapture As Boolean, RightToLeft As Boolean, ECMAScript As Boolean) As RegexOptions
                If Singleline Then
                    CreateRegexOptions = CreateRegexOptions Or RegexOptions.Singleline
                End If
        
                If IgnoreCase Then
                    CreateRegexOptions = CreateRegexOptions Or RegexOptions.IgnoreCase
                End If
        
                If IgnoreWhitespace Then
                    CreateRegexOptions = CreateRegexOptions Or RegexOptions.IgnorePatternWhitespace
                End If
        
                If CultureInvariant Then
                    CreateRegexOptions = CreateRegexOptions Or RegexOptions.CultureInvariant
                End If
        
                If ExplicitCapture Then
                    CreateRegexOptions = CreateRegexOptions Or RegexOptions.ExplicitCapture
                End If
        
                If RightToLeft Then
                    CreateRegexOptions = CreateRegexOptions Or RegexOptions.RightToLeft
                End If
        
                If ECMAScript Then
                    CreateRegexOptions = CreateRegexOptions Or RegexOptions.ECMAScript
                End If
        
                Return CreateRegexOptions
            End Function

    #End Region

    #Region "Code Stages"

    ''' <summary>
    ''' Get GUID
    ''' </summary>
    Private Sub CodeStage_Get_GUID(Optional ByRef id As String = Nothing)

        dim g as new Guid
        id = guid.newguid.tostring

    End Sub

    ''' <summary>
    ''' Trim
    ''' </summary>
    Private Sub CodeStage_Trim(Optional ByVal Text As String = Nothing, Optional ByRef Trimmed_Text As String = Nothing)

        
        dim r as new regex("^\W*|\W*$")
        Trimmed_Text = r.replace(text, "")

    End Sub

    ''' <summary>
    ''' Split
    ''' </summary>
    Private Sub CodeStage_Split(Optional ByVal Text_to_Split As String = Nothing, Optional ByRef Split_Values As DataTable = Nothing)

        Split_Values = SplitStringInto("Value", _
         Text_to_Split, vbCrLf, vbLf, vbCr)

    End Sub

    ''' <summary>
    ''' Split Text
    ''' </summary>
    Private Sub CodeStage_Split_Text(Optional ByVal Text_to_Split As String = Nothing, Optional ByVal Split_Char As String = Nothing, Optional ByVal Collection_Field_Name As String = Nothing, Optional ByRef Split_Values As DataTable = Nothing)

        Split_Values = SplitStringInto( _
         Collection_Field_Name, _
         Text_to_Split, Split_Char)

    End Sub

    ''' <summary>
    ''' Get Carriage Return
    ''' </summary>
    Private Sub CodeStage_Get_Carriage_Return(Optional ByRef Join_Character As String = Nothing)

        Join_Character = vbcrlf

    End Sub

    ''' <summary>
    ''' Format
    ''' </summary>
    Private Sub CodeStage_Format(Optional ByVal Input As Decimal? = Nothing, Optional ByRef Output As String = Nothing)

        Output = Input.ToString("N")

    End Sub

    ''' <summary>
    ''' Get Elements
    ''' </summary>
    Private Sub CodeStage_Get_Elements(Optional ByVal XML As String = Nothing, Optional ByVal Element As String = Nothing, Optional ByRef Elements As DataTable = Nothing)

        
        dim table as datatable = GetDataTable("XML", "System.String")
        dim row as datarow
        dim doc as new xmldocument
        dim list as xmlnodelist
        
        doc.loadxml(xml)
        list = doc.getelementsbytagname(element)
        
        for each n as xmlnode in list
        	row = table.newrow()
        	row("XML") = n.outerxml
        	table.rows.Add(row)
        next
        
        Elements = table

    End Sub

    ''' <summary>
    ''' Get Attribute
    ''' </summary>
    Private Sub CodeStage_Get_Attribute(Optional ByVal XML As String = Nothing, Optional ByVal Attribute As String = Nothing, Optional ByRef Value As String = Nothing)

        
        dim i as integer = XML.indexof(Attribute)
        
        if i > 0 then
        	i += Attribute.length + 2
        	value = XML.substring(i)
        	value = value.substring(0, value.indexof(""""))
        else
        	value = ""
        end if

    End Sub

    ''' <summary>
    ''' Split Lines By Length
    ''' </summary>
    Private Sub CodeStage_Split_Lines_By_Length(Optional ByVal Text_to_Split As String = Nothing, Optional ByVal Maximum_Line_Length As Decimal? = Nothing, Optional ByVal Strict_Split As Boolean? = Nothing, Optional ByRef Split_Lines As DataTable = Nothing, Optional ByRef Line_Count As Decimal? = Nothing)

        Dim Values as List(Of String) = Nothing
        If Strict_Split Then
              Values = New List(Of String)
              While Text_to_Split.Length > 0
                    Dim NewLine As String = Text_to_Split.SubString(0, Math.Min(Maximum_Line_Length, Text_to_Split.Length))
                    NewLine = NewLine.Trim()
                    Values.Add(NewLine)
        
                    If Text_to_Split.Length > NewLine.Length Then
                          Text_to_Split = Text_to_Split.SubString(NewLine.Length, Text_to_Split.Length - NewLine.Length)
                    Else
                          Text_to_Split = ""
                    End If
                    Text_to_Split = Text_to_Split.Trim()
              End While
        Else
              Values = SplitTextByLengthEngine(Text_to_Split, Maximum_Line_length)
        End If
        
        Split_Lines = New DataTable()
        Split_Lines.Columns.Add("Line Text", GetType(String))
        For Each s as String in Values
              Split_Lines.Rows.Add(New Object() {s})
        Next
        
        Line_Count = Values.Count

    End Sub

    ''' <summary>
    ''' Get Newline Character
    ''' </summary>
    Private Sub CodeStage_Get_Newline_Character(Optional ByRef Newline_Character As String = Nothing)

        Newline_Character = VbCrLf

    End Sub

    ''' <summary>
    ''' Delete
    ''' </summary>
    Private Sub CodeStage_Delete(Optional ByVal Text_Sample As String = Nothing, Optional ByVal Characters_to_Delete As String = Nothing, Optional ByRef Amended_Sample As String = Nothing)

        For Each C as Char in Characters_To_Delete.ToCharArray()
        	Text_Sample = Text_Sample.Replace(C, "")
        Next
        
        Amended_Sample = Text_Sample

    End Sub

    ''' <summary>
    ''' Escape Text
    ''' </summary>
    Private Sub CodeStage_Escape_Text(Optional ByVal SendKeys_Text As String = Nothing, Optional ByRef Escaped_Sendkeys_Text As String = Nothing)

        Escaped_Sendkeys_Text = Regex.Replace(SendKeys_Text, "[\[\]{}+^%~()]", "{$0}")

    End Sub

    ''' <summary>
    ''' Extract Values
    ''' </summary>
    Private Sub CodeStage_Extract_Values(Optional ByVal Regex_Pattern As String = Nothing, Optional ByVal Target_String As String = Nothing, Optional ByVal Named_Values As DataTable = Nothing, Optional ByRef Named_Values_Out As DataTable = Nothing)

        For Each Row As DataRow in Named_Values.Rows
        	Row("Value") = ""
        Next
        
        Dim R as New Regex(Regex_Pattern, RegexOptions.SingleLine)
        Dim M as Match = R.Match(Target_String)
        If M IsNot Nothing AndAlso M.Success Then
        	If M.Groups IsNot Nothing AndAlso M.Groups.Count > 0 Then
        		For Each Row As DataRow in Named_Values.Rows
        			Dim GroupName As String = CStr(Row("Name"))
        			Dim G As Group = M.Groups(GroupName)
        			If G.Success Then
        				Row("Value") = G.Value
        			End If
        		Next
        	End If
        End If
        
        Named_Values_Out = Named_Values

    End Sub

    ''' <summary>
    ''' InStr
    ''' </summary>
    Private Sub CodeStage_InStr(Optional ByVal InText As String = Nothing, Optional ByVal Search_String As String = Nothing, Optional ByVal Start_Byte As Decimal? = Nothing, Optional ByVal Compare_Method As Decimal? = Nothing, Optional ByRef Position As Decimal? = Nothing)

          Position = Microsoft.VisualBasic.InStr(Start_Byte,InText, Search_String, 1)

    End Sub

    ''' <summary>
    ''' InStrRev
    ''' </summary>
    Private Sub CodeStage_InStrRev(Optional ByVal InText As String = Nothing, Optional ByVal Search_String As String = Nothing, Optional ByVal Start_Byte As Decimal? = Nothing, Optional ByVal Compare_Method As Decimal? = Nothing, Optional ByRef Position As Decimal? = Nothing)

          Position = Microsoft.VisualBasic.InStrRev(InText, Search_String, Start_Byte, 1)

    End Sub

    ''' <summary>
    ''' Test Regex Match1
    ''' </summary>
    Private Sub CodeStage_Test_Regex_Match1(Optional ByVal Regex_Pattern As String = Nothing, Optional ByVal Target_String As String = Nothing, Optional ByRef Regex_Match As Boolean? = Nothing)

        
        Dim R as New Regex(Regex_Pattern, RegexOptions.SingleLine)
        Dim M as Match = R.Match(Target_String)
        Regex_Match =  M IsNot Nothing AndAlso M.Success

    End Sub

    ''' <summary>
    ''' Serialise to Delimited Text
    ''' </summary>
    Private Sub CodeStage_Serialise_to_Delimited_Text(Optional ByVal Input_Collection As DataTable = Nothing, Optional ByVal Delimiter As String = Nothing, Optional ByRef Output_CSV As String = Nothing)

        
        Dim SB As New StringBuilder
        WriteDataTable(SB, Input_Collection, True, Delimiter)
        Output_CSV = SB.ToString()

    End Sub

    ''' <summary>
    ''' Parse Delimited String
    ''' </summary>
    Private Sub CodeStage_Parse_Delimited_String(Optional ByVal DelimitedText As String = Nothing, Optional ByVal Schema As DataTable = Nothing, Optional ByVal FirstRowIsHeader As Boolean? = Nothing, Optional ByVal Delimiter As String = Nothing, Optional ByRef outputCollection As DataTable = Nothing)

        Const SchemaColumnName As String = "Column Name"
        Const DefaultColumnName As String = "Column "
        Const nonSchemaHeadingIndex As Integer = 0
        
        Dim emptySchema As Boolean = Schema Is Nothing OrElse Schema.Rows.Count = 0
        
        Dim csvValuesList = ParseCsvToList(DelimitedText, Delimiter)
        
        ' If we want to parse with no schema and want the first row be used as headings 
        ' we need to know what the headings will be.
        Dim nonSchemaHeadings = csvValuesList(nonSchemaHeadingIndex)
        
        ' Arrange the column headings into the table first.
        If emptySchema Then
        	For Each columnHeader As String In nonSchemaHeadings
        		Dim colName As String = If(FirstRowIsHeader, columnHeader,
        											 DefaultColumnName & outputCollection.Columns.Count)
        		outputCollection.Columns.Add(colName, GetType(String))
        	Next
        Else
        	For Each columnHeader As DataRow In Schema.Rows
        		Dim colName As String = columnHeader(SchemaColumnName).ToString
        		outputCollection.Columns.Add(colName, GetType(String))
        	Next
        End If
        
        ' If the first row is being used for headings then skip those headings / values in csvValuesList.
        Dim startListIndex As Integer = If(FirstRowIsHeader, nonSchemaHeadingIndex + 1, nonSchemaHeadingIndex)
        
        ' Insert the csv values into the table row by row.
        For i As Integer = startListIndex To csvValuesList.Count - 1
        	Dim currentRow As Datarow = outputCollection.NewRow
        	outputCollection.Rows.Add(currentRow)
        
        	Dim csvArray = csvValuesList(i)
        	For columnIndex As Integer = 0 To csvArray.Length - 1
        		currentRow.Item(columnIndex) = csvArray(columnIndex)
        	Next
        Next

    End Sub

    ''' <summary>
    ''' Replace
    ''' </summary>
    Private Sub CodeStage_Replace(Optional ByVal Text_Sample As String = Nothing, Optional ByVal Characters_to_Replace As String = Nothing, Optional ByVal Replacement_Characters As String = Nothing, Optional ByRef Amended_Sample As String = Nothing)

        For Each C as Char in Characters_To_Replace.ToCharArray()
        	Text_Sample = Text_Sample.Replace(C, Replacement_Characters)
        Next
        
        Amended_Sample = Text_Sample

    End Sub

    ''' <summary>
    ''' Extract All Matches
    ''' </summary>
    Private Sub CodeStage_Extract_All_Matches(Optional ByVal Regex_Pattern As String = Nothing, Optional ByVal Text_To_Perform_Search_On As String = Nothing, Optional ByVal Singleline As Boolean? = Nothing, Optional ByVal Ignore_Case As Boolean? = Nothing, Optional ByVal Explicit_Capture As Boolean? = Nothing, Optional ByRef Regex_Matches As DataTable = Nothing, Optional ByRef Success As Boolean? = Nothing)

        Dim fullMatchColumnName As String = "Full Match"
        
        Regex_Matches = New DataTable()
        Regex_Matches.Columns.Add(fullMatchColumnName, GetType(String))
        
        Dim regexOptionConfiguration As RegexOptions = CreateRegexOptions(Singleline, Ignore_Case, False, False, Explicit_Capture, False, False)
        Dim regexObject As New Regex(Regex_Pattern, regexOptionConfiguration)
        Dim regexMatches As MatchCollection = regexObject.Matches(Text_To_Perform_Search_On)
        
        Success = regexMatches IsNot Nothing AndAlso regexMatches.Count > 0
        If Success Then
        
        	Dim groupNames() As String = regexObject.GetGroupNames()
        	groupNames(0) = fullMatchColumnName
        
        	For groupIndex As Integer = 1 To groupNames.GetUpperBound(0)
        		Regex_Matches.Columns.Add(groupNames(groupIndex), GetType(String))
        	Next
        
        	For Each regexMatch As Match In regexMatches
        
        		Dim resultRow As DataRow = Regex_Matches.NewRow()
        		Dim groupIndex As Integer = 0
        
        		For Each regexGroup As Group In regexMatch.Groups
        				resultRow(groupNames(groupIndex)) = regexGroup.Value			
        			groupIndex += 1
        		Next
        		Regex_Matches.Rows.Add(resultRow)
        	Next
        End If

    End Sub

    ''' <summary>
    ''' Perform Replace
    ''' </summary>
    Private Sub CodeStage_Perform_Replace(Optional ByVal pattern As String = Nothing, Optional ByVal input As String = Nothing, Optional ByVal replacement As String = Nothing, Optional ByVal max As Decimal? = Nothing, Optional ByVal start As Decimal? = Nothing, Optional ByRef output As String = Nothing)

        Dim rgx As New Regex(pattern)
        
        If (max > 0) And (start > 0) Then
        	output = rgx.Replace(input, replacement, CInt(max), CInt(start))
        ElseIf (max > 0) Then
        	output = rgx.Replace(input, replacement, CInt(max))
        Else
        	output = rgx.Replace(input, replacement)
        End If

    End Sub

    ''' <summary>
    ''' Calculate Levenshtein Distance
    ''' </summary>
    Private Sub CodeStage_Calculate_Levenshtein_Distance(Optional ByVal source As String = Nothing, Optional ByVal target As String = Nothing, Optional ByVal caseSensitive As Boolean? = Nothing, Optional ByRef distance As Decimal? = Nothing, Optional ByRef similarity As Decimal? = Nothing)

        '*****************************************************
        ' Yes, GoTo statements are generally frowned upon, but
        ' in this case they work well.
        '*****************************************************
        Dim sourceCharCount As Integer = source.Length
        Dim targetCharCount As Integer = target.Length
        Dim numOfEdits As Integer(,) = New Integer(sourceCharCount + 1 - 1, targetCharCount + 1 - 1) {}
        Dim i As Integer = 0
        Dim j As Integer = 0
        
        If (caseSensitive = False) Then
        	source = source.ToLower()
        	target = target.ToLower()
        End If
        
        ' There are various checks that we would normally perform on the input strings, but which aren't required here because
        ' of the Decision stage check to ensure input was actually provied.
        '
        ' Are the string identical? If they are, there's no reason to calculate a distance becauase we already no it's 0 (zero).
        If source = target Then
        	distance = 0
        	Goto JumpPoint
        End If
        
        While i <= sourceCharCount
        	numOfEdits(i, 0) = Math.Min(System.Threading.Interlocked.Increment(i), i - 1)
        End While
        
        While j <= targetCharCount
        	numOfEdits(0, j) = Math.Min(System.Threading.Interlocked.Increment(j), j - 1)
        End While
        
        For i = 1 To sourceCharCount
        	For j = 1 To targetCharCount
        		Dim cost As Integer = If((target(j - 1) = source(i - 1)), 0, 1)
        		numOfEdits(i, j) = Math.Min(Math.Min(numOfEdits(i - 1, j) + 1, numOfEdits(i, j - 1) + 1), numOfEdits(i - 1, j - 1) + cost)
        	Next
        Next
        
        distance = numOfEdits(sourceCharCount, targetCharCount)
        
        JumpPoint:
        
        similarity = Math.Round(1.0 - (distance / CDec(Math.Max(source.Length, target.Length))),4)

    End Sub

    #End Region

End Class
