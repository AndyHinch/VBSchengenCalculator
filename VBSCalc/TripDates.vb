Imports Newtonsoft.Json

Public Class TripDates

    Public Sub AddEntry(ByVal Ent As TripDate)
        _entries.Add(Ent)
    End Sub

    Public Sub RemoveEntry(ByVal StartDate As Date)
        Dim pred As Predicate(Of TripDate) = Function(ByVal entry As TripDate)
                                                 Return (entry.StartDate = StartDate)
                                             End Function
        _entries.RemoveAll(pred)
    End Sub

    Public Sub RemoveAll()

        _entries = New List(Of TripDate)

    End Sub

    Public ReadOnly Property Entries As IEnumerable(Of TripDate)
        Get
            Return _entries
        End Get
    End Property

    Private _entries As List(Of TripDate) = New List(Of TripDate)

    Public Function MaxDate() As Date
        Dim d As New Date(1900, 1, 1)
        For Each t As TripDate In _entries
            If t.EndDate > d Then
                d = t.EndDate
            End If
        Next
        Return d
    End Function

    Public Function MinDate() As Date
        Dim d As New Date(2999, 12, 31)
        For Each t As TripDate In _entries
            If t.StartDate < d Then
                d = t.StartDate
            End If
        Next
        Return d
    End Function

    Public Function toJson() As String
        Dim jsonData As String = JsonConvert.SerializeObject(Me.Entries)
        Return jsonData

    End Function

    Public Sub loadFromJson(ByVal json As String)

        Dim Entries As List(Of TripDate) = JsonConvert.DeserializeObject(Of List(Of TripDate))(json)

        _entries = Entries
    End Sub

End Class

Public Class TripDate

    Private _StartDate As Date
    Private _EndDate As Date

    Public Property StartDate() As Date
        Get
            Return _StartDate

        End Get
        Set(value As Date)
            _StartDate = value
        End Set
    End Property

    Public Property EndDate() As Date
        Get
            Return _EndDate
        End Get
        Set(value As Date)
            _EndDate = value
        End Set
    End Property

    Public Sub New(ByVal StartDate As Date, ByVal EndDate As Date)
        _StartDate = StartDate
        _EndDate = EndDate
    End Sub

    Public Function NumberOfDays() As Integer

        Return DateDiff(DateInterval.Day, _StartDate, _EndDate) + 1

    End Function

    Public Function WasInAreaOnDate(ByVal AreaDate As Date) As Boolean

        If AreaDate >= StartDate And AreaDate <= EndDate Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Overrides Function ToString() As String
        Return "" & _StartDate.ToShortDateString & " to " & _EndDate.ToShortDateString & " (" & NumberOfDays.ToString & " days)"
    End Function

End Class