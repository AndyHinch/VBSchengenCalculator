Public Class SchengenCalculator

    Private ReadOnly _TripDates As TripDates
    Private ReadOnly _MaximumDaysInArea As Integer
    Private ReadOnly _ReviewDaysInAreas As Integer

    Public Sub New(ByVal TripDates As TripDates, ByVal MaximumDaysInArea As Integer, ByVal ReviewDaysInAreas As Integer)
        If TripDates Is Nothing Then
            Throw New ArgumentNullException(NameOf(TripDates))
        End If
        If MaximumDaysInArea <= 0 Then
            Throw New ArgumentException("Maximum days in area must be positive", NameOf(MaximumDaysInArea))
        End If
        If ReviewDaysInAreas <= 0 Then
            Throw New ArgumentException("Review days in areas must be positive", NameOf(ReviewDaysInAreas))
        End If

        _TripDates = TripDates
        _MaximumDaysInArea = MaximumDaysInArea
        _ReviewDaysInAreas = ReviewDaysInAreas
    End Sub

    Public Function NumberOfDaysInAreaOnDay(ByVal ReviewDay As Date) As Integer
        Dim startDate As Date = ReviewDay.AddDays(-_ReviewDaysInAreas)
        Dim endDate As Date = ReviewDay

        Dim totalDays As Integer = 0

        For Each trip As TripDate In _TripDates.Entries
            Dim overlapStart As Date = If(trip.StartDate > startDate, trip.StartDate, startDate)
            Dim overlapEnd As Date = If(trip.EndDate < endDate, trip.EndDate, endDate)

            If overlapStart <= overlapEnd Then
                totalDays += DateDiff(DateInterval.Day, overlapStart, overlapEnd) + 1
            End If
        Next

        Return totalDays
    End Function

End Class
