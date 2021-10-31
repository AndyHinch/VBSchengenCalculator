<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Me.lstDates = New System.Windows.Forms.ListBox()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.dtpReviewDate = New System.Windows.Forms.DateTimePicker()
        Me.txtNumberOfDaysInArea = New System.Windows.Forms.TextBox()
        Me.btnCalc = New System.Windows.Forms.Button()
        Me.lstPredictions = New System.Windows.Forms.ListBox()
        Me.btnPredict = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnAbout = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lstDates
        '
        Me.lstDates.FormattingEnabled = True
        Me.lstDates.ItemHeight = 15
        Me.lstDates.Location = New System.Drawing.Point(27, 46)
        Me.lstDates.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.lstDates.Name = "lstDates"
        Me.lstDates.Size = New System.Drawing.Size(256, 259)
        Me.lstDates.TabIndex = 0
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(299, 46)
        Me.btnAdd.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(82, 22)
        Me.btnAdd.TabIndex = 1
        Me.btnAdd.Text = "&Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnEdit
        '
        Me.btnEdit.Enabled = False
        Me.btnEdit.Location = New System.Drawing.Point(299, 74)
        Me.btnEdit.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(82, 22)
        Me.btnEdit.TabIndex = 2
        Me.btnEdit.Text = "&Edit"
        Me.btnEdit.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Enabled = False
        Me.btnDelete.Location = New System.Drawing.Point(299, 100)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(82, 22)
        Me.btnDelete.TabIndex = 3
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'dtpReviewDate
        '
        Me.dtpReviewDate.Location = New System.Drawing.Point(27, 337)
        Me.dtpReviewDate.Name = "dtpReviewDate"
        Me.dtpReviewDate.Size = New System.Drawing.Size(200, 23)
        Me.dtpReviewDate.TabIndex = 4
        '
        'txtNumberOfDaysInArea
        '
        Me.txtNumberOfDaysInArea.Location = New System.Drawing.Point(315, 337)
        Me.txtNumberOfDaysInArea.Name = "txtNumberOfDaysInArea"
        Me.txtNumberOfDaysInArea.ReadOnly = True
        Me.txtNumberOfDaysInArea.Size = New System.Drawing.Size(230, 23)
        Me.txtNumberOfDaysInArea.TabIndex = 5
        '
        'btnCalc
        '
        Me.btnCalc.Location = New System.Drawing.Point(234, 337)
        Me.btnCalc.Name = "btnCalc"
        Me.btnCalc.Size = New System.Drawing.Size(75, 23)
        Me.btnCalc.TabIndex = 6
        Me.btnCalc.Text = "&Calc"
        Me.btnCalc.UseVisualStyleBackColor = True
        '
        'lstPredictions
        '
        Me.lstPredictions.FormattingEnabled = True
        Me.lstPredictions.ItemHeight = 15
        Me.lstPredictions.Location = New System.Drawing.Point(401, 46)
        Me.lstPredictions.Name = "lstPredictions"
        Me.lstPredictions.Size = New System.Drawing.Size(287, 259)
        Me.lstPredictions.TabIndex = 7
        '
        'btnPredict
        '
        Me.btnPredict.Location = New System.Drawing.Point(299, 138)
        Me.btnPredict.Name = "btnPredict"
        Me.btnPredict.Size = New System.Drawing.Size(82, 23)
        Me.btnPredict.TabIndex = 8
        Me.btnPredict.Text = "&Predict"
        Me.btnPredict.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(401, 26)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(158, 15)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Remaining Dates Predictions"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(27, 26)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(232, 15)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Dates Entering and Leaving Schengen Area"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(27, 316)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(185, 15)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "Date to Calculate Remaining Days"
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(299, 187)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(82, 23)
        Me.btnSave.TabIndex = 12
        Me.btnSave.Text = "&Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnLoad
        '
        Me.btnLoad.Location = New System.Drawing.Point(299, 217)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(82, 23)
        Me.btnLoad.TabIndex = 13
        Me.btnLoad.Text = "&Load"
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(299, 249)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(82, 23)
        Me.btnNew.TabIndex = 14
        Me.btnNew.Text = "&New"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnAbout
        '
        Me.btnAbout.Location = New System.Drawing.Point(299, 281)
        Me.btnAbout.Name = "btnAbout"
        Me.btnAbout.Size = New System.Drawing.Size(82, 23)
        Me.btnAbout.TabIndex = 15
        Me.btnAbout.Text = "About"
        Me.btnAbout.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(700, 375)
        Me.Controls.Add(Me.btnAbout)
        Me.Controls.Add(Me.btnNew)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnPredict)
        Me.Controls.Add(Me.lstPredictions)
        Me.Controls.Add(Me.btnCalc)
        Me.Controls.Add(Me.txtNumberOfDaysInArea)
        Me.Controls.Add(Me.dtpReviewDate)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnEdit)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.lstDates)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "frmMain"
        Me.Text = "Schengen Calculator"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lstDates As ListBox
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnEdit As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents dtpReviewDate As DateTimePicker
    Friend WithEvents txtNumberOfDaysInArea As TextBox
    Friend WithEvents btnCalc As Button
    Friend WithEvents lstPredictions As ListBox
    Friend WithEvents btnPredict As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents btnSave As Button
    Friend WithEvents btnLoad As Button
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents btnNew As Button
    Friend WithEvents btnAbout As Button
End Class
