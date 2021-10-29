<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDate
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker()
        Me.dtpEndDate = New System.Windows.Forms.DateTimePicker()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'dtpStartDate
        '
        Me.dtpStartDate.Location = New System.Drawing.Point(32, 29)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(250, 27)
        Me.dtpStartDate.TabIndex = 1
        '
        'dtpEndDate
        '
        Me.dtpEndDate.Location = New System.Drawing.Point(32, 83)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Size = New System.Drawing.Size(250, 27)
        Me.dtpEndDate.TabIndex = 2
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(375, 29)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(94, 29)
        Me.btnOK.TabIndex = 3
        Me.btnOK.Text = "&OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(375, 80)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(94, 29)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'frmDate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(518, 146)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.dtpEndDate)
        Me.Controls.Add(Me.dtpStartDate)
        Me.Name = "frmDate"
        Me.Text = "frmDate"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents dtpStartDate As DateTimePicker
    Friend WithEvents dtpEndDate As DateTimePicker
    Friend WithEvents btnOK As Button
    Friend WithEvents btnCancel As Button

    Public StartDate As Date
    Public EndDate As Date
    Public Cancelled As Boolean = True

End Class
