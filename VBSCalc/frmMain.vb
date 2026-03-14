Imports System.IO

Public Class frmMain
    Private _TripDates As New TripDates
    Private ReadOnly _MaximumDaysInArea As Integer = 90
    Private ReadOnly _ReviewDaysInAreas As Integer = 180

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Try
            Dim f As New frmDate With {
                .StartDate = Today,
                .EndDate = Today.AddDays(14)
            }

            f.ShowDialog()

            If Not f.Cancelled Then
                Dim trip As New TripDate(f.StartDate, f.EndDate)
                _TripDates.AddEntry(trip)
                LoadTripDates()
            End If
        Catch ex As Exception
            MessageBox.Show($"Error adding trip: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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

        Try
            Dim sel As TripDate = CType(lstDates.SelectedItem, TripDate)

            Dim f As New frmDate With {
                .StartDate = sel.StartDate,
                .EndDate = sel.EndDate
            }

            f.ShowDialog()

            If Not f.Cancelled Then
                _TripDates.RemoveEntry(sel.StartDate)
                Dim trip As New TripDate(f.StartDate, f.EndDate)
                _TripDates.AddEntry(trip)
                LoadTripDates()
            End If
        Catch ex As Exception
            MessageBox.Show($"Error editing trip: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub lstDates_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstDates.SelectedIndexChanged
        If lstDates.SelectedIndex <> -1 Then
            btnEdit.Enabled = True
            btnDelete.Enabled = True
        End If
    End Sub

    Private Sub btnCalc_Click(sender As Object, e As EventArgs) Handles btnCalc.Click
        Try
            Dim sc As New SchengenCalculator(_TripDates, _MaximumDaysInArea, _ReviewDaysInAreas)
            Dim DaysInArea As Integer = sc.NumberOfDaysInAreaOnDay(dtpReviewDate.Value)
            Dim DaysRemaining As Integer = _MaximumDaysInArea - DaysInArea

            If DaysRemaining >= 0 Then
                txtNumberOfDaysInArea.Text = $"{DaysInArea} used, {DaysRemaining} remaining"
            Else
                txtNumberOfDaysInArea.Text = $"{DaysInArea} used, {Math.Abs(DaysRemaining)} over!"
            End If
        Catch ex As Exception
            MessageBox.Show($"Error calculating days: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnPredict_Click(sender As Object, e As EventArgs) Handles btnPredict.Click
        Try
            Dim sc As New SchengenCalculator(_TripDates, _MaximumDaysInArea, _ReviewDaysInAreas)
            
            Dim minDateValue = _TripDates.MinDate
            Dim maxDateValue = _TripDates.MaxDate

            If Not minDateValue.HasValue OrElse Not maxDateValue.HasValue Then
                MessageBox.Show("Please add at least one trip before predicting.", "No Trips", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim MinDate As Date = minDateValue.Value
            Dim MaxDate As Date = maxDateValue.Value

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
                    lstPredictions.Items.Add($"{d.ToShortDateString} : {DaysInArea} used, {DaysRemaining} remaining")
                Else
                    lstPredictions.Items.Add($"{d.ToShortDateString} : {DaysInArea} used, {Math.Abs(DaysRemaining)} over!")
                End If
                d = d.AddMonths(1)
            Loop
        Catch ex As Exception
            MessageBox.Show($"Error predicting days: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            Dim json As String = _TripDates.ToJson()

            SaveFileDialog1.Filter = "Trip Files (*.tripdata)|*.tripdata"
            If SaveFileDialog1.ShowDialog = DialogResult.OK Then
                File.WriteAllText(SaveFileDialog1.FileName, json)
                MessageBox.Show("Trip data saved successfully.", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show($"Error saving file: {ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        Try
            OpenFileDialog1.Filter = "Trip Files (*.tripdata)|*.tripdata"
            OpenFileDialog1.CheckFileExists = True
            
            If OpenFileDialog1.ShowDialog <> DialogResult.Cancel Then
                Dim json As String = File.ReadAllText(OpenFileDialog1.FileName)
                _TripDates.LoadFromJson(json)
                LoadTripDates()
                MessageBox.Show("Trip data loaded successfully.", "Load Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show($"Error loading file: {ex.Message}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If lstDates.SelectedIndex = -1 Then
            Exit Sub
        End If

        Try
            Dim sel As TripDate = CType(lstDates.SelectedItem, TripDate)
            
            Dim result = MessageBox.Show($"Are you sure you want to delete this trip?{vbCrLf}{sel.ToString()}", 
                                        "Confirm Delete", 
                                        MessageBoxButtons.YesNo, 
                                        MessageBoxIcon.Question)
            
            If result = DialogResult.Yes Then
                _TripDates.RemoveEntry(sel.StartDate)
                LoadTripDates()
            End If
        Catch ex As Exception
            MessageBox.Show($"Error deleting trip: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Try
            Dim result = MessageBox.Show("Are you sure you want to clear all trips? This cannot be undone.", 
                                        "Confirm New", 
                                        MessageBoxButtons.YesNo, 
                                        MessageBoxIcon.Warning)
            
            If result = DialogResult.Yes Then
                _TripDates.RemoveAll()
                LoadTripDates()
            End If
        Catch ex As Exception
            MessageBox.Show($"Error clearing trips: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnAbout_Click(sender As Object, e As EventArgs) Handles btnAbout.Click
        MessageBox.Show($"Schengen Calculator{vbCrLf}" &
                        $"© {Today.Year} Andrew Hinchcliffe{vbCrLf}" &
                        $"License: MIT License{vbCrLf}" &
                        $"All rights reserved{vbCrLf}" &
                        $"You are free to distribute this application{vbCrLf}" &
                        $"Version: {Application.ProductVersion}", 
                        "Schengen Calculator", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information)
    End Sub
End Class