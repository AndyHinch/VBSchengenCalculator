Public Class frmDate
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()

    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click

        Me.Cancelled = False
        Me.StartDate = dtpStartDate.Value
        Me.EndDate = dtpEndDate.Value
        Me.Close()

    End Sub

    Private Sub frmDate_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class