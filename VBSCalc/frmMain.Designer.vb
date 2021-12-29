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
        Me.dtpReviewDate = New System.Windows.Forms.DateTimePicker()
        Me.txtNumberOfDaysInArea = New System.Windows.Forms.TextBox()
        Me.btnCalc = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lstDates = New System.Windows.Forms.ListBox()
        Me.btnAbout = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnPredict = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lstPredictions = New System.Windows.Forms.ListBox()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.MonthCalendar1 = New System.Windows.Forms.MonthCalendar()
        Me.txtDateDescription = New System.Windows.Forms.TextBox()
        Me.TabControl1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dtpReviewDate
        '
        Me.dtpReviewDate.Location = New System.Drawing.Point(31, 470)
        Me.dtpReviewDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtpReviewDate.Name = "dtpReviewDate"
        Me.dtpReviewDate.Size = New System.Drawing.Size(228, 27)
        Me.dtpReviewDate.TabIndex = 4
        '
        'txtNumberOfDaysInArea
        '
        Me.txtNumberOfDaysInArea.Location = New System.Drawing.Point(360, 470)
        Me.txtNumberOfDaysInArea.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtNumberOfDaysInArea.Name = "txtNumberOfDaysInArea"
        Me.txtNumberOfDaysInArea.ReadOnly = True
        Me.txtNumberOfDaysInArea.Size = New System.Drawing.Size(262, 27)
        Me.txtNumberOfDaysInArea.TabIndex = 5
        '
        'btnCalc
        '
        Me.btnCalc.Location = New System.Drawing.Point(267, 470)
        Me.btnCalc.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnCalc.Name = "btnCalc"
        Me.btnCalc.Size = New System.Drawing.Size(86, 31)
        Me.btnCalc.TabIndex = 6
        Me.btnCalc.Text = "&Calc"
        Me.btnCalc.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(31, 442)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(235, 20)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "Date to Calculate Remaining Days"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Location = New System.Drawing.Point(12, 12)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(824, 429)
        Me.TabControl1.TabIndex = 16
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Panel1)
        Me.TabPage2.Location = New System.Drawing.Point(4, 29)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(816, 396)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Data"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.lstPredictions)
        Me.Panel1.Controls.Add(Me.btnAbout)
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Controls.Add(Me.btnLoad)
        Me.Panel1.Controls.Add(Me.btnSave)
        Me.Panel1.Controls.Add(Me.btnPredict)
        Me.Panel1.Controls.Add(Me.btnDelete)
        Me.Panel1.Controls.Add(Me.btnEdit)
        Me.Panel1.Controls.Add(Me.btnAdd)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.lstDates)
        Me.Panel1.Location = New System.Drawing.Point(-5, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(792, 396)
        Me.Panel1.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(21, 7)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(293, 20)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Dates Entering and Leaving Schengen Area"
        '
        'lstDates
        '
        Me.lstDates.FormattingEnabled = True
        Me.lstDates.ItemHeight = 20
        Me.lstDates.Location = New System.Drawing.Point(21, 33)
        Me.lstDates.Name = "lstDates"
        Me.lstDates.Size = New System.Drawing.Size(292, 344)
        Me.lstDates.TabIndex = 11
        '
        'btnAbout
        '
        Me.btnAbout.Location = New System.Drawing.Point(336, 334)
        Me.btnAbout.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnAbout.Name = "btnAbout"
        Me.btnAbout.Size = New System.Drawing.Size(94, 31)
        Me.btnAbout.TabIndex = 23
        Me.btnAbout.Text = "About"
        Me.btnAbout.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(336, 291)
        Me.btnNew.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(94, 31)
        Me.btnNew.TabIndex = 22
        Me.btnNew.Text = "&New"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnLoad
        '
        Me.btnLoad.Location = New System.Drawing.Point(336, 248)
        Me.btnLoad.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(94, 31)
        Me.btnLoad.TabIndex = 21
        Me.btnLoad.Text = "&Load"
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(336, 208)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(94, 31)
        Me.btnSave.TabIndex = 20
        Me.btnSave.Text = "&Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnPredict
        '
        Me.btnPredict.Location = New System.Drawing.Point(336, 143)
        Me.btnPredict.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPredict.Name = "btnPredict"
        Me.btnPredict.Size = New System.Drawing.Size(94, 31)
        Me.btnPredict.TabIndex = 19
        Me.btnPredict.Text = "&Predict"
        Me.btnPredict.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Enabled = False
        Me.btnDelete.Location = New System.Drawing.Point(336, 92)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(94, 29)
        Me.btnDelete.TabIndex = 18
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnEdit
        '
        Me.btnEdit.Enabled = False
        Me.btnEdit.Location = New System.Drawing.Point(336, 58)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(94, 29)
        Me.btnEdit.TabIndex = 17
        Me.btnEdit.Text = "&Edit"
        Me.btnEdit.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(336, 20)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(94, 29)
        Me.btnAdd.TabIndex = 16
        Me.btnAdd.Text = "&Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(453, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(199, 20)
        Me.Label1.TabIndex = 25
        Me.Label1.Text = "Remaining Dates Predictions"
        '
        'lstPredictions
        '
        Me.lstPredictions.FormattingEnabled = True
        Me.lstPredictions.ItemHeight = 20
        Me.lstPredictions.Location = New System.Drawing.Point(453, 33)
        Me.lstPredictions.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.lstPredictions.Name = "lstPredictions"
        Me.lstPredictions.Size = New System.Drawing.Size(327, 344)
        Me.lstPredictions.TabIndex = 24
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.txtDateDescription)
        Me.TabPage1.Controls.Add(Me.MonthCalendar1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 29)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(816, 396)
        Me.TabPage1.TabIndex = 2
        Me.TabPage1.Text = "TabPage1"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'MonthCalendar1
        '
        Me.MonthCalendar1.Location = New System.Drawing.Point(15, 21)
        Me.MonthCalendar1.Name = "MonthCalendar1"
        Me.MonthCalendar1.TabIndex = 0
        '
        'txtDateDescription
        '
        Me.txtDateDescription.Location = New System.Drawing.Point(15, 266)
        Me.txtDateDescription.Multiline = True
        Me.txtDateDescription.Name = "txtDateDescription"
        Me.txtDateDescription.ReadOnly = True
        Me.txtDateDescription.Size = New System.Drawing.Size(256, 97)
        Me.txtDateDescription.TabIndex = 1
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(854, 512)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.btnCalc)
        Me.Controls.Add(Me.txtNumberOfDaysInArea)
        Me.Controls.Add(Me.dtpReviewDate)
        Me.Name = "frmMain"
        Me.Text = "Schengen Calculator"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dtpReviewDate As DateTimePicker
    Friend WithEvents txtNumberOfDaysInArea As TextBox
    Friend WithEvents btnCalc As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnAbout As Button
    Friend WithEvents btnNew As Button
    Friend WithEvents btnLoad As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents btnPredict As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnEdit As Button
    Friend WithEvents btnAdd As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents lstDates As ListBox
    Friend WithEvents Label1 As Label
    Friend WithEvents lstPredictions As ListBox
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents txtDateDescription As TextBox
    Friend WithEvents MonthCalendar1 As MonthCalendar
End Class
