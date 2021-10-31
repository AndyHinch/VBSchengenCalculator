Public Class frmDate
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()

    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click

        If dtpStartDate.Value > dtpEndDate.Value Then
            MessageBox.Show("Start Date cannot be later than End Date.", "Schengen Calculator")
            Exit Sub
        End If

        Me.Cancelled = False
        Me.StartDate = dtpStartDate.Value
        Me.EndDate = dtpEndDate.Value
        Me.Close()

    End Sub

    Private Sub frmDate_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        dtpStartDate.MaxDate = Today.AddYears(10)
        dtpStartDate.MinDate = Today.AddYears(-10)

        dtpEndDate.MaxDate = Today.AddYears(10)
        dtpEndDate.MinDate = Today.AddYears(-10)

        dtpStartDate.Value = Me.StartDate
        dtpEndDate.Value = Me.EndDate

    End Sub
End Class