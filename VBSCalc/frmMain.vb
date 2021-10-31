Imports System.IO

Public Class frmMain
    Private _TripDates As New TripDates
    Private _MaximumDaysInArea As Integer = 90
    Private _ReviewDaysInAreas As Integer = 180

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        Dim f As New frmDate

        f.StartDate = Today
        f.EndDate = Today.AddDays(14)


        f.ShowDialog()

        If Not f.Cancelled Then

            Dim trip As New TripDate(f.StartDate, f.EndDate)
            _TripDates.AddEntry(trip)
            LoadTripDates()
        End If

    End Sub

    Public Sub LoadTripDates()

        lstDates.Items.Clear()

        For Each Trip As TripDate In _TripDates.Entries
            lstDates.Items.Add(Trip)
        Next

        btnEdit.Enabled = False
        btnDelete.Enabled = False

    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        dtpReviewDate.Value = Today
        dtpReviewDate.MaxDate = Today.AddYears(10)
        dtpReviewDate.MinDate = Today.AddYears(-10)

    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click

        If lstDates.SelectedIndex = -1 Then
            Exit Sub
        End If

        Dim sel As TripDate = lstDates.SelectedItem

        Dim f As New frmDate

        f.StartDate = sel.StartDate
        f.EndDate = sel.EndDate


        f.ShowDialog()

        If Not f.Cancelled Then

            _TripDates.RemoveEntry(sel.StartDate)
            Dim trip As New TripDate(f.StartDate, f.EndDate)
            _TripDates.AddEntry(trip)
            LoadTripDates()
        End If

    End Sub

    Private Sub lstDates_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstDates.SelectedIndexChanged

        If lstDates.SelectedIndex <> -1 Then
            btnEdit.Enabled = True
            btnDelete.Enabled = True
        End If

    End Sub

    Private Sub btnCalc_Click(sender As Object, e As EventArgs) Handles btnCalc.Click


        Dim sc As New SchengenCalculator(_TripDates, _MaximumDaysInArea, _ReviewDaysInAreas)
        Dim DaysInArea As Integer = sc.NumberOfDaysInAreaOnDay(dtpReviewDate.Value)
        Dim DaysRemaining As Integer = _MaximumDaysInArea - DaysInArea

        If DaysRemaining >= 0 Then
            txtNumberOfDaysInArea.Text = DaysInArea.ToString & " used, " & DaysRemaining.ToString & " remaining"
        Else
            txtNumberOfDaysInArea.Text = DaysInArea.ToString & " used, " & (DaysRemaining * -1).ToString & " over!"
        End If

    End Sub

    Private Sub btnPredict_Click(sender As Object, e As EventArgs) Handles btnPredict.Click

        Dim sc As New SchengenCalculator(_TripDates, _MaximumDaysInArea, _ReviewDaysInAreas)
        Dim MinDate As Date = _TripDates.MinDate
        Dim MaxDate As Date = _TripDates.MaxDate

        MinDate = MinDate.AddDays((MinDate.Day - 1) * -1)
        MaxDate = MaxDate.AddMonths(1)
        MaxDate = MaxDate.AddDays(_ReviewDaysInAreas)
        MaxDate = MaxDate.AddDays((MaxDate.Day - 1) * -1)

        Dim d As Date = MinDate

        lstPredictions.Items.Clear()

        Do While d <= MaxDate
            Dim DaysInArea As Integer = sc.NumberOfDaysInAreaOnDay(d)
            Dim DaysRemaining As Integer = _MaximumDaysInArea - DaysInArea
            If DaysRemaining >= 0 Then
                lstPredictions.Items.Add(d.ToShortDateString & " : " & DaysInArea.ToString & " used, " & DaysRemaining.ToString & " remaining")
            Else
                lstPredictions.Items.Add(d.ToShortDateString & " : " & DaysInArea.ToString & " used, " & (DaysRemaining * -1).ToString & " over!")

            End If
            d = d.AddMonths(1)
        Loop

    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Dim json As String = _TripDates.toJson()

        SaveFileDialog1.Filter = "Trip Files (*.tripdata*)|*.tripdata"
        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            'Dim s As New StreamWriter(SaveFileDialog1.FileName)
            File.WriteAllText(SaveFileDialog1.FileName, json)
        End If

    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click

        Dim json As String

        OpenFileDialog1.Filter = "Trip Files (*.tripdata*)|*.tripdata"
        OpenFileDialog1.CheckFileExists = True
        If OpenFileDialog1.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            json = File.ReadAllText(OpenFileDialog1.FileName)

            _TripDates.loadFromJson(json)

            LoadTripDates()
        End If

    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click


        If lstDates.SelectedIndex = -1 Then
            Exit Sub
        End If

        Dim sel As TripDate = lstDates.SelectedItem

        _TripDates.RemoveEntry(sel.StartDate)

        LoadTripDates()

    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click

        _TripDates.RemoveAll

        LoadTripDates()

    End Sub

    Private Sub btnAbout_Click(sender As Object, e As EventArgs) Handles btnAbout.Click

        MessageBox.Show("Schengen Calculator" & vbCrLf &
                        "© " & Today.Year.ToString & " Andrew Hinchcliffe" & vbCrLf &
                        "License: MIT Liense" & vbCrLf &
                        "All rights reserved" & vbCrLf &
                        "You are free to distribute this application" & vbCrLf &
                        "Version : " & Application.ProductVersion.ToString, "Schengen Calculator")

    End Sub
End Class