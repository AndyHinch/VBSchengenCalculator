Public Class SchengenCalculator

    Private _TripDates As TripDates
    Private _MaximumDaysInArea As Integer
    Private _ReviewDaysInAreas As Integer

    Public Sub New(ByVal TripDates As TripDates, ByVal MaximumDaysInArea As Integer, ByVal ReviewDaysInAreas As Integer)
        _TripDates = TripDates
        _MaximumDaysInArea = MaximumDaysInArea
        _ReviewDaysInAreas = ReviewDaysInAreas
    End Sub

    Public Function NumberOfDaysInAreaOnDay(ByVal ReviewDay As Date) As Integer
        Dim NoOfDays As Integer = 0
        Dim CalcDate As Date = ReviewDay.AddDays(_ReviewDaysInAreas * -1)

        While CalcDate <= ReviewDay.AddDays(0)
            For Each Trip As TripDate In _TripDates.Entries
                If Trip.WasInAreaOnDate(CalcDate) Then
                    NoOfDays += 1
                End If
            Next
            CalcDate = CalcDate.AddDays(1)
        End While

        Return NoOfDays
    End Function

End Class
