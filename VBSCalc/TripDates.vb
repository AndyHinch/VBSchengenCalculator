Imports Newtonsoft.Json

Public Class TripDates

    Public Sub AddEntry(ByVal Ent As TripDate)
        If Ent Is Nothing Then
            Throw New ArgumentNullException(NameOf(Ent))
        End If
        _entries.Add(Ent)
    End Sub

    Public Sub RemoveEntry(ByVal StartDate As Date)
        _entries.RemoveAll(Function(entry) entry.StartDate = StartDate)
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

    Public Function MaxDate() As Date?
        If Not _entries.Any() Then
            Return Nothing
        End If
        Return _entries.Max(Function(t) t.EndDate)
    End Function

    Public Function MinDate() As Date?
        If Not _entries.Any() Then
            Return Nothing
        End If
        Return _entries.Min(Function(t) t.StartDate)
    End Function

    Public Function ToJson() As String
        Return JsonConvert.SerializeObject(_entries)
    End Function

    Public Sub LoadFromJson(ByVal json As String)
        If String.IsNullOrWhiteSpace(json) Then
            Throw New ArgumentException("JSON data cannot be null or empty", NameOf(json))
        End If

        Try
            Dim entries As List(Of TripDate) = JsonConvert.DeserializeObject(Of List(Of TripDate))(json)
            If entries Is Nothing Then
                Throw New InvalidOperationException("Failed to deserialize trip data")
            End If
            _entries = entries
        Catch ex As JsonException
            Throw New InvalidOperationException("Invalid JSON format", ex)
        End Try
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
        If EndDate < StartDate Then
            Throw New ArgumentException("End date cannot be before start date")
        End If
        _StartDate = StartDate
        _EndDate = EndDate
    End Sub

    Public Function NumberOfDays() As Integer
        Return DateDiff(DateInterval.Day, _StartDate, _EndDate) + 1
    End Function

    Public Function WasInAreaOnDate(ByVal AreaDate As Date) As Boolean
        Return AreaDate >= StartDate AndAlso AreaDate <= EndDate
    End Function

    Public Overrides Function ToString() As String
        Return $"{_StartDate.ToShortDateString} to {_EndDate.ToShortDateString} ({NumberOfDays} days)"
    End Function

End Class