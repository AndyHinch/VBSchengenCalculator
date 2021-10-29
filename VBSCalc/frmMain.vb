Public Class frmMain
    Private _TripDates As New TripDates

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        Dim f As New frmDate

        f.StartDate = Today
        f.EndDate = Today.AddDays(14)


        f.ShowDialog()

        If Not f.Cancelled Then
            lstDates.Items.Add("" & f.StartDate.ToShortDateString & " to " & f.EndDate.ToShortDateString)
            Dim trip As New TripDate(f.StartDate, f.EndDate)
            _TripDates.AddEntry(trip)
        End If

    End Sub
End Class