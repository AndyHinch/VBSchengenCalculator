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

    Public ReadOnly Property Entries As IEnumerable(Of TripDate)
        Get
            Return _entries
        End Get
    End Property

    Private _entries As List(Of TripDate) = New List(Of TripDate)

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

        Return DateDiff(DateInterval.Day, _EndDate, _EndDate)

    End Function

End Class