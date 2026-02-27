Calculations and decisions
==========================

Calculations and Decisions are very similar and as such, they are edited in almost the same way. Both are based on an expression, but with two key differences. Firstly, a Calculation is an expression that can result in any value, but a Decision is an expression that must result in TRUE, or FALSE. Secondly, the result of a Calculation is stored in a Data Item, whereas the result of a Decision is not. It is used only to determine the direction the flow of a process will take.

Expressions can be created in a variety of ways. They can be typed directly into the Expression Editor, or can be built up by dragging and dropping items from either the Function List, or the Data Item List. The Function List shows all the functions available in Blue Prism. When a Function is dragged and dropped into the Expression Editor, the default function text is automatically entered on the screen. Similarly, Data Items can be dragged in from the Data Item List and their names will be automatically entered into the Expression Editor.

Functions can also be composed in the Function Builder area in the centre of the screen. When a Function is selected from the Function List it is displayed in the Function Builder, along with a description of its use and any of its parameters. Parameters can be given values either by typing directly into the fields shown, or by dragging Data Items from the Data Item List. Once the Function has been built, it can be transferred to the Expression Editor using the Paste button.

A Calculation must specify the Data Item into which the evaluated result will be stored. The 'Store Result In' field can be populated by typing in a Data Item name, or by dragging in a Data Item from the list on the right.

When an expression is complete, it should be checked for errors and this can be done by selecting the 'Process Validation' button. Blue Prism will then check the expression and highlight the likely location of the error.

Once checked, an expression can also be evaluated by selecting the Test Expression button. If any Data Items have been used in the expression, a new [Expression Test Wizard](frmExpressionTest.htm) will appear that enables temporary values to be given to each Data Item, so that a result can be obtained from the expression. If no Data Items have been used in the expression, the evaluated result will be displayed in a pop-up message. Should you wish to only test part of an expression you can select part of an expression by dragging and highlighting with the mouse. When the Expression Test Wizard appears only the selected part of the expression will be used.

Expressions
-----------

Expressions are built up from Data Items, Operators, Functions and Constants.

Data items
----------

Data Items are referenced by name and must be enclosed in square brackets, for example _\[Account Number\] ._

Operators
---------

Operators are represented by their respective symbol, for example _1 + 2_.

Functions
---------

Functions are in the form_FunctionName(parameter1,parameter2)_, for example _Mid("Hello", 1, 2)._

The syntax for most functions is self-explanatory. Below are details of the more complex functions.

### Conversion

[![Closed](Skins/Default/Stylesheets/Images/transparent.gif)ToDateTime (text)](#)

This function returns a DateTime based on the supplied text value. This DateTime object is time zone agnostic. If the supplied value does not include a time zone, it is treated as local time and the DateTime value effectively remains unchanged. If the supplied value includes a time zone, it is converted into local time during the process of removing the time zone.

#### Examples

*   ToDateTime("2023-09-29T12:00:00") returns 29/09/2023 12:00:00
*   ToDateTime("2023-09-29T12:00:00Z") in an environment with time zone of UTC returns 29/09/2023 12:00:00
*   ToDateTime("2023-09-29T12:00:00Z") in an environment with time zone of UTC+12 returns 30/09/2023 00:00:00
*   ToDateTime("2023-09-29T12:00:00Z") in an environment with time zone of UTC-5 returns 29/09/2023 07:00:00
*   ToDateTime("2023-09-29T12:00:00−02:00") in an environment with time zone of UTC returns 29/09/2023 14:00:00
*   ToDateTime("2023-09-29T12:00:00−02:00") in an environment with time zone of UTC+12 returns 30/09/2023 02:00:00
*   ToDateTime("2023-09-29T12:00:00−02:00") in an environment with time zone of UTC-5 returns 29/09/2023 09:00:00

### Date

[![Closed](Skins/Default/Stylesheets/Images/transparent.gif)AddDays (date, numDays)](#)

This function adds a certain number of days to a chosen date.

#### Parameters

The two parameters are as follows:

 

Parameter

Description

date

The date to which onto which the days should be added.

numDays

The number of days to be added.

#### Examples

AddDays("01/01/2006", 10) will return the date "11/01/2006".

#### Tips

As with the function DateAdd(), only valid dates will be returned. For example in a leap year adding one day to 28 February will result in 29 February whereas during any other year it will result in 1 March.

[![Closed](Skins/Default/Stylesheets/Images/transparent.gif)AddMonths (date, numMonths)](#)

This function adds a certain number of months to a chosen date.

#### Parameters

The two parameters are:

 

Parameter

Description

date

The date to which onto which the days should be added.

numMonths

The number of months to be added.

#### Examples

AddMonths("01/01/2006", 10) will return the date "01/11/2006".

#### Tips

As with the function DateAdd(), only valid dates will be returned. For example adding 1 month to January 31st 2005 using AddMonths("31/01/2005",1) would return "28/2/2005" (rather than returning "31/02/2005" – a date which does not exist).

[![Closed](Skins/Default/Stylesheets/Images/transparent.gif)DateAdd(interval, number,date)](#)

The recommended way to add an interval to a Date, Time, or DateTime is to add a TimeSpan

For example, _MakeDate(26,5,1999) + MakeTimeSpan(3, 0, 0, 0)_ would result in 29/05/1999.

For times, _MakeTime(12,30,0) + MakeTimeSpan(0, 1, 5, 3)_ would result in 13:35:03.

When you need to add a non-fixed interval of time that can vary depending on the time of year the DateAdd function provides some useful intervals. Each type of interval is represented by the numbers listed below:

  

Interval

DateAdd

DateDiff

0

Year

Year

1

Week

Week of year _(Calendar week)_

2

_(n/a)_

Weekday _(Full 7 day week)_

3

_(n/a)_

Second

4

Quarter

Quarter

5

Month

Month

6

_(n/a)_

Minute

7

_(n/a)_

Hour

8

_(n/a)_

Day of year

9

_(n/a)_

Day

As indicated some intervals are not applicable to the DateAdd function but were chosen so the same set of numbers could be used for the DateAdd function and the DateDiff function

For example, to add a number of months to a date use interval number 5. So to add 2 months to 26/5/1999, use _DateAdd(5, 2, MakeDate(26,5,1999))_. This would correctly return 26/7/1999.

Intervals are subtracted in a similar manner by using a negative value for the quantity of intervals, ie _DateAdd(5, -2, MakeDate(26,5,1999))_ results in 26/3/1999.

The DateAdd function won't return an invalid date. For example if you added 1 month to January 31st 2005 using DateAdd(5,1,MakeDate(31,1,2005)) the function would return 28/2/2005 (rather than returning 31/02/2005 – a date which does not exist).

[![Closed](Skins/Default/Stylesheets/Images/transparent.gif)DateDiff (interval,date1,date2)](#)

The recommended way to find the difference between two Dates, Times, or DateTimes is to subtract them

For example, _MakeDate(26,5,1999) – MakeDate(29,5,1968)_ will return a TimeSpan 11319.00:00:00

When you need to find the difference in a non-fixed interval of time that can vary depending on the time of year, the DateDiff function provides some useful intervals. These are listed in the table within the DateAdd function below. If _date2_ > _date1_ the returned number will be positive.

If Week of year is used, the return value represents the number of weeks between the first day of the week containing _date1_ and the first day of the week containing _date2_.

When interval 2 Weekday is used, the return value represents the number of full calendar weeks between the two dates (e.g. if _date1_ is a Monday then it counts the number of Mondays up to and including _date2_)

For example, if _date1_ is a Thursday 2/3/2017 and _date2_ is the following Tuesday 7/3/2017 then _DateDiff(1, date1, date2)_ using Week of year will return 1 because the first days of the respective calendar weeks are a week apart.

However _DateDiff(2, date1, date2)_ using Weekday will return 0, as there are no Thursdays between _date1_ and _date2_.

#### Parameters

The three parameters are as follows:

 

Parameter

Description

Interval

A code specifying the desired units of the return value. These values are detailed in the DateAdd function.

date1

The first of the two dates for comparison.

date2

The second of the two dates for comparison.

#### Examples

To calculate the number of weeks between 29/5/1968 and 26/5/1999 use DateDiff(1, MakeDate(29,5,1968), MakeDate(26,5,1999)). This will correctly return 1617.

[![Closed](Skins/Default/Stylesheets/Images/transparent.gif)FormatDate (Date, DateFormat)](#)

This function formats a date into a desired form, or extracts a smaller piece of information (such as the day of the week) from a full date.

#### Parameters

The two parameters are as follows:.

 

Parameter

Description

Date

The date value to be formatted as a text value.

Format

The format string, which specifies the form of the desired output. This can either be inputted as a single letter denoting a standard date format or a string denoting a custom date format.

#### Examples

*   FormatDate("02-11-2016", "D") will return "02 November 2016"
*   FormatDate("02 Nov 2016","d") will return "2/11/2016"
*   FormatDate("02-11-2016", "M") will return "2 November"
*   FormatDate("02-11-2016", "dddd") will return "Wednesday"
*   FormatDate("02/11/2016", "yyyy-MM-dd") will return "2016-11-02"
*   FormatDate("02/11/2016", "MMM dd, yyyy") will return "Nov 02, 2016"

#### Format strings

*   For a full list of Standard Date and Time Format Strings, see [https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx](https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx).
*   For further information on how to create a Custom Date and Time Format Strings, see [https://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx](https://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx).

[![Closed](Skins/Default/Stylesheets/Images/transparent.gif)FormatDateTime (Date, DateFormat)](#)

This function formats a datetime into a desired form, or extracts a smaller piece of information (such as the day of the week) from a full datetime.

#### Parameters

The two parameters are as follows:

 

Parameter

Description

Date

The date value to be formatted as a text value.

Format

The format string, which specifies the form of the desired output. This can either be inputted as a single letter denoting a standard date format or a string denoting a custom date format.

#### Examples

*   FormatDateTime("02-11-2016 09:23:43", "t") will return "09:23"
*   FormatDateTime("02 Nov 2016 09:23:43","F") will return "02 November 2016 09:23:43"
*   FormatDateTime("02-11-2016 21:23:43", "h:m tt") will return "9:23 PM"
*   FormatDateTime("02-11-2016 21:23:43", "dd/MM/yy HH:m:s") will return "02/11/16 21:23:43"

#### Format strings

*   For a full list of Standard Date and Time Format Strings, see [https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx](https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx).
*   For further information on how to create a Custom Date and Time Format Strings, see [https://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx](https://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx).

[![Closed](Skins/Default/Stylesheets/Images/transparent.gif)MakeDate (Day, Month, Year)](#)

MakeDate() creates a date from numbers.

#### Parameters

The three parameters are as follows:

 

Parameter

Description

Day

The day of the month to be used. This must be valid with respect to the chosen month; 30 is not a valid value if the month is 2 (ie February).

Month

The 1-based index of the month desired (eg 4 corresponds to April). Must not exceed 12.

Year

The year desired, eg 2001.

#### Two-digit years

For compatibility with old processes, where it was allowed, the MakeDate function will accept two-digit years and interpret them using the _2029 rule_. However, it is strongly recommended that this functionality is never used. Always give a full year, including the century. In the case where the year is retrieved from another system in two digits, find out what convention _that system_ is using and apply it as soon as you read the data.

#### Examples

*   MakeDate(21,6,2005) will return the date 21st June 2005
*   MakeDate(21,6,1995) will return the date 21st June 1995
*   MakeDate(5,12,29) will return the date 5th December 2029
*   MakeDate(5,12,30) will return the date 5th December 1930

#### Tips

The MakeDate function is the preferred way to form a new date, rather than using a string such as "02/03/2007", which has an ambiguous value depending on the current locale (ie an American would probably interpret this date differently to a Briton).

[![Closed](Skins/Default/Stylesheets/Images/transparent.gif)Now ()](#)

This function returns a standardized UTC date and time format as a [datetime data item](helpDatatypes.htm).

#### Tips

If you want to know today's date, you should use the Today() function instead. For more information, see [this Knowledge Base article](https://support.blueprism.com/en/support/solutions/articles/7000077052-how-are-dates-and-times-stored-in-the-blue-prism-database-tables- "Knowledge Base article").

[![Closed](Skins/Default/Stylesheets/Images/transparent.gif)Today ()](#)

This function returns the current local system date as a [date data item](helpDatatypes.htm).

#### Tips

If you want to know the current time together with today's date, you should use the Now() function instead.

### Number

[![Closed](Skins/Default/Stylesheets/Images/transparent.gif)DecPad (number, numplaces)](#)

This function represents a number in a text format. This is often useful when dealing with currency, when numbers such as the number 1 are more often represented as "1.00".

#### Parameters

The two parameters are:

 

Parameter

Description

number

The number to be rounded.

numPlaces

The maximum number of decimal places desired.

#### Examples

*   DecPad(1.296,2) will return the text "1.30".
*   DecPad(1.1111,2) will return the text "1.11"

#### Tips

If the natural representation has too many decimal places then the appropriate number of decimal places will be removed, whilst rounding the number to the appropriate precision.

[![Closed](Skins/Default/Stylesheets/Images/transparent.gif)Log (number, base)](#)

This function evaluates the logarithm of the number to the specified base value. For a value v and a base b, the logarithm of v to the base b – written Log(v, b) – answers the question "what is the value x which satisfies bx = v?".

#### Examples

*   Log(1000, 10) = 3 because 103 = 1000.
*   Log(1/4, 2) = -2 because 2\-2 = 1/4

[![Closed](Skins/Default/Stylesheets/Images/transparent.gif)RndDn (number, numplaces)](#)

This function rounds a number downwards to the desired number of decimal places. The data type of the value returned is number.

#### Parameters

The two parameters are:

 

Parameter

Description

number

The number to be rounded.

numPlaces

The maximum number of decimal places desired.

#### Examples

*   RndDn(9.19996,3) will return the number 9.199
*   RndDn(9.1345,2) will return the number 9.13
*   RndDn(3.14159,10) would return 3.14159 (the number is unchanged)

#### Tips

If the number is already within the desired level of precision then it will remain unchanged, and in particular it will not be reformatted (as it would using the function DecPad). See also the functions Round and RndUp.

[![Closed](Skins/Default/Stylesheets/Images/transparent.gif)RndUp (number, numplaces)](#)

This function rounds a number upwards to the desired number of decimal places; it behaves analogously to the RndDn() function.

#### Parameters

The two parameters are:

 

Parameter

Description

number

The number to be rounded.

numPlaces

The maximum number of decimal places desired.

#### Examples

*   RndUp(9.19996,3) will return the number 9.200
*   RndUp(9.1345,2) will return the number 9.14
*   RndUp(3.14159,10) would return 3.14159 (the number is unchanged)

#### Tips

If the number is already within the desired level of precision then it will remain unchanged, and in particular it will not be reformatted (as it would using the function DecPad). See also the functions Round and RndDn.

[![Closed](Skins/Default/Stylesheets/Images/transparent.gif)Round (number, numplaces)](#)

This function rounds a number to the nearest number having the desired number of decimal places.

#### Parameters

The two parameters are:

 

Parameter

Description

number

The number to be rounded.

numPlaces

The maximum number of decimal places desired.

#### Examples

*   Round(9.1345,2) will return the number 9.13
*   Round(9.19996,3) will return the number 9.200
*   Round(3.14159,10) would return 3.14159 (the number is unchanged)

#### Tips

If the number is already within the desired level of precision then it will remain unchanged, and in particular it will not be reformatted (as it would using the function DecPad). See also the functions RndUp and RndDn.

[![Closed](Skins/Default/Stylesheets/Images/transparent.gif)Sqrt (number)](#)

This function returns the square root of the supplied number. The square root of a number is the unique positive number which when multiplied by itself results in the number you started with.

#### Examples

*   Sqrt(100) returns the number 10
*   Sqrt(2) returns 1.4142135623731

### Text

[![Closed](Skins/Default/Stylesheets/Images/transparent.gif)Chr (keycode)](#)

This function returns the character represented by the supplied [ASCII code](helpASCII.htm "Link to ASCII code page").

#### Examples

*   Chr(65) returns the text "A"

#### Tips

For a full introduction to the ASCII code, please consult an external reference. A quick guide is given on the Blue Prism [ASCII code](helpASCII.htm "Link to ASCII code page").

[![Closed](Skins/Default/Stylesheets/Images/transparent.gif)InStr (string, substring)](#)

This function tests whether the smaller string _substring_ is contained in the longer string _string_. If it is, then the function returns the number of characters from the left that the first occurrence of _substring_ may be found.

#### Parameters

The two parameters are:

 

Parameter

Description

string

The larger string, from which a substring is to be extracted.

substring

The smaller string, whose presence is to be detected in the larger string.

#### Examples

Instr("Calculations are much faster with Blue Prism than with an abacus", "are") will return 14 because the word "are" first occurs at the fourteenth character.

#### Tips

If _substring_ is not found in _string_ then Instr() will return zero. Thus Instr() is a useful way of testing for the presence of a smaller string in a larger string: eg. _Instr("Apples", "Bananas") > 0_ will return FALSE.

[![Closed](Skins/Default/Stylesheets/Images/transparent.gif)Left (string, maxcharacters)](#)

This function returns the first few letters of a longer string of letters.

#### Parameters

The two parameters are:

 

Parameter

Description

string

The larger string, from which a substring is to be extracted.

maxcharacters

The maximum number of characters desired. The return value of the function will have this number of characters, unless the larger string ends first, in which case this value will become a theoretical maximum.

#### Tips

If the value maxcharacters exceeds (or indeed is equal to) the number of characters in the string, the function will simply return the string unmodified. The value supplied to maxcharacters may be zero if desired (returns empty string), but may not a negative number.

#### Examples

Left("Blue Prism empowers business users to achieve more in less time", 10) returns "Blue Prism"

[![Closed](Skins/Default/Stylesheets/Images/transparent.gif)Len (string)](#)

This function returns the number of characters in a string. For example Len("Blue Prism") returns the number 10.

[![Closed](Skins/Default/Stylesheets/Images/transparent.gif)Lower (string)](#)

This function returns the lower case representation of the supplied text. Any letters in the supplied text that are already in lower case are unchanged, while those that are not in lower case are changed.

#### Examples

*   Lower("SOFTWARE") will return the text "software"
*   Lower("aBcDeFg") will return the text "abcdefg"

#### Tips

See also the Upper() function.

[![Closed](Skins/Default/Stylesheets/Images/transparent.gif)Mid (string, startpoint, maxcharacters)](#)

This function reduces a longer string (a sequence of letters) into a shorter string contained in that string (a substring). For example, Mid() can be used to reduce the string "Customer Name: John Smith", to the substring "John Smith".

#### Parameters

The three parameters are:

 

Parameter

Description

string

The larger string, from which a substring is to be extracted.

startpoint

The 1-based index of the first letter desired.

maxcharacters

The maximum number of characters desired. The return value of the function will have this number of characters, unless the larger string ends first, in which case this value will become a theoretical maximum.

#### Tips

When the user-specified maximum length exceeds the length of the string supplied, Blue Prism will reduce this length to the length of the supplied string. Thus, to save counting too many letters, it is often convenient to write a large number in the last argument: Mid("Customer Name: John Smith", 16, 100). The string "John" could be obtained using Mid("Customer Name: John Smith",16,4). Note: The arguments are rounded off to whole integers, so Mid("abcdefg", 1.2, 1.8) would evaluate to "ab".

#### Examples

Mid("Customer Name: John Smith", 16, 25) – returns "John Smith"

[![Closed](Skins/Default/Stylesheets/Images/transparent.gif)Right (string, maxcharacters)](#)

This function returns the last few letters of a longer string of letters.

[![Closed](Skins/Default/Stylesheets/Images/transparent.gif)Upper (string)](#)

This function returns the upper case representation of the supplied text. Any letters in the supplied text that are already in upper case are unchanged, while those that are not in upper case are changed.

#### Examples

*   Upper("software") will return the text "SOFTWARE"
*   Upper("aBcDeFg") will return the text "ABCDEFG"

#### Tips

See also the Lower() function.

### Constants

Text and passwords are represented by enclosing the text in quotes, for example _"This is my text"._

Numbers are represented by typing the plain number, for example _5_ or _1.2_

Flags are represented by the words _True_ or _False_

Dates are represented as a text expression in _dd/mm/yyyy_ format, for example "12/03/2004"

### Casting

Casting occurs when an entity of one data type is placed in an expression where another data type was expected.

An example of this may be giving a number to a text function.

Len(100)

The 100 is automatically cast to a "100" text, then the Length function will return "3" since 100 has 3 characters.
