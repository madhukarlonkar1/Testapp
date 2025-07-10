<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMaster
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMaster))
        Me.btnclose = New System.Windows.Forms.Button()
        Me.btnReset = New System.Windows.Forms.Button()
        Me.tmrping = New System.Windows.Forms.Timer(Me.components)
        Me.grpErrorMessage = New System.Windows.Forms.GroupBox()
        Me.lblerrmsg = New System.Windows.Forms.Label()
        Me.FlashingProgressBar = New System.Windows.Forms.ProgressBar()
        Me.txtPartNumber = New System.Windows.Forms.TextBox()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.grpOperatorMessage = New System.Windows.Forms.GroupBox()
        Me.lblFlashingcount = New System.Windows.Forms.Label()
        Me.lblopmsg = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnflash = New System.Windows.Forms.Button()
        Me.grpKeyboard = New System.Windows.Forms.GroupBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.Button9 = New System.Windows.Forms.Button()
        Me.Button10 = New System.Windows.Forms.Button()
        Me.Button11 = New System.Windows.Forms.Button()
        Me.Button12 = New System.Windows.Forms.Button()
        Me.Button13 = New System.Windows.Forms.Button()
        Me.Button14 = New System.Windows.Forms.Button()
        Me.Button15 = New System.Windows.Forms.Button()
        Me.Button16 = New System.Windows.Forms.Button()
        Me.Button17 = New System.Windows.Forms.Button()
        Me.Button18 = New System.Windows.Forms.Button()
        Me.Button19 = New System.Windows.Forms.Button()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.lblPLC = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtBoxSRNO = New System.Windows.Forms.TextBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.lblCAL = New System.Windows.Forms.Label()
        Me.lblAPP = New System.Windows.Forms.Label()
        Me.lblSBL = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grpErrorMessage.SuspendLayout()
        Me.grpOperatorMessage.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.grpKeyboard.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnclose
        '
        Me.btnclose.BackColor = System.Drawing.Color.LightSteelBlue
        Me.btnclose.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnclose.ForeColor = System.Drawing.Color.Black
        Me.btnclose.Location = New System.Drawing.Point(4, 63)
        Me.btnclose.Name = "btnclose"
        Me.btnclose.Size = New System.Drawing.Size(78, 41)
        Me.btnclose.TabIndex = 24
        Me.btnclose.Text = "CLOSE"
        Me.ToolTip1.SetToolTip(Me.btnclose, "CLICK TO CLOSE")
        Me.btnclose.UseVisualStyleBackColor = False
        '
        'btnReset
        '
        Me.btnReset.BackColor = System.Drawing.Color.LightSteelBlue
        Me.btnReset.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReset.ForeColor = System.Drawing.Color.Black
        Me.btnReset.Location = New System.Drawing.Point(6, 12)
        Me.btnReset.Name = "btnReset"
        Me.btnReset.Size = New System.Drawing.Size(78, 41)
        Me.btnReset.TabIndex = 23
        Me.btnReset.Text = "RESET"
        Me.ToolTip1.SetToolTip(Me.btnReset, "CLICK TO RESET")
        Me.btnReset.UseVisualStyleBackColor = False
        '
        'tmrping
        '
        Me.tmrping.Interval = 3000
        '
        'grpErrorMessage
        '
        Me.grpErrorMessage.BackColor = System.Drawing.Color.Transparent
        Me.grpErrorMessage.Controls.Add(Me.lblerrmsg)
        Me.grpErrorMessage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpErrorMessage.ForeColor = System.Drawing.Color.PaleGreen
        Me.grpErrorMessage.Location = New System.Drawing.Point(1, 186)
        Me.grpErrorMessage.Name = "grpErrorMessage"
        Me.grpErrorMessage.Size = New System.Drawing.Size(629, 50)
        Me.grpErrorMessage.TabIndex = 28
        Me.grpErrorMessage.TabStop = False
        Me.grpErrorMessage.Text = "ERROR MESSGE"
        '
        'lblerrmsg
        '
        Me.lblerrmsg.Font = New System.Drawing.Font("Verdana", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblerrmsg.ForeColor = System.Drawing.Color.Red
        Me.lblerrmsg.Location = New System.Drawing.Point(0, 16)
        Me.lblerrmsg.Name = "lblerrmsg"
        Me.lblerrmsg.Size = New System.Drawing.Size(623, 34)
        Me.lblerrmsg.TabIndex = 20
        '
        'FlashingProgressBar
        '
        Me.FlashingProgressBar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FlashingProgressBar.Location = New System.Drawing.Point(11, 51)
        Me.FlashingProgressBar.Name = "FlashingProgressBar"
        Me.FlashingProgressBar.Size = New System.Drawing.Size(474, 25)
        Me.FlashingProgressBar.TabIndex = 26
        Me.FlashingProgressBar.Visible = False
        '
        'txtPartNumber
        '
        Me.txtPartNumber.BackColor = System.Drawing.Color.LightYellow
        Me.txtPartNumber.Location = New System.Drawing.Point(6, 26)
        Me.txtPartNumber.MaxLength = 32
        Me.txtPartNumber.Name = "txtPartNumber"
        Me.txtPartNumber.Size = New System.Drawing.Size(197, 27)
        Me.txtPartNumber.TabIndex = 0
        Me.txtPartNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnOK
        '
        Me.btnOK.BackColor = System.Drawing.Color.LightSteelBlue
        Me.btnOK.Enabled = False
        Me.btnOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnOK.Font = New System.Drawing.Font("Verdana", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOK.ForeColor = System.Drawing.Color.Black
        Me.btnOK.Location = New System.Drawing.Point(522, 39)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(93, 36)
        Me.btnOK.TabIndex = 4
        Me.btnOK.Text = "OK"
        Me.ToolTip1.SetToolTip(Me.btnOK, "CLICK TO OK")
        Me.btnOK.UseVisualStyleBackColor = False
        '
        'grpOperatorMessage
        '
        Me.grpOperatorMessage.BackColor = System.Drawing.Color.Transparent
        Me.grpOperatorMessage.Controls.Add(Me.lblFlashingcount)
        Me.grpOperatorMessage.Controls.Add(Me.FlashingProgressBar)
        Me.grpOperatorMessage.Controls.Add(Me.btnOK)
        Me.grpOperatorMessage.Controls.Add(Me.lblopmsg)
        Me.grpOperatorMessage.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpOperatorMessage.ForeColor = System.Drawing.Color.PaleGreen
        Me.grpOperatorMessage.Location = New System.Drawing.Point(1, 77)
        Me.grpOperatorMessage.Name = "grpOperatorMessage"
        Me.grpOperatorMessage.Size = New System.Drawing.Size(629, 81)
        Me.grpOperatorMessage.TabIndex = 27
        Me.grpOperatorMessage.TabStop = False
        Me.grpOperatorMessage.Text = "OPERATOR INSTRUCTIONS"
        '
        'lblFlashingcount
        '
        Me.lblFlashingcount.AutoSize = True
        Me.lblFlashingcount.BackColor = System.Drawing.Color.Lavender
        Me.lblFlashingcount.Font = New System.Drawing.Font("Verdana", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFlashingcount.ForeColor = System.Drawing.Color.Red
        Me.lblFlashingcount.Location = New System.Drawing.Point(611, 330)
        Me.lblFlashingcount.Name = "lblFlashingcount"
        Me.lblFlashingcount.Size = New System.Drawing.Size(21, 29)
        Me.lblFlashingcount.TabIndex = 29
        Me.lblFlashingcount.Text = " "
        '
        'lblopmsg
        '
        Me.lblopmsg.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblopmsg.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.lblopmsg.Location = New System.Drawing.Point(6, 23)
        Me.lblopmsg.Name = "lblopmsg"
        Me.lblopmsg.Size = New System.Drawing.Size(617, 25)
        Me.lblopmsg.TabIndex = 14
        '
        'GroupBox2
        '
        Me.GroupBox2.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox2.Controls.Add(Me.txtPartNumber)
        Me.GroupBox2.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.ForeColor = System.Drawing.Color.PaleGreen
        Me.GroupBox2.Location = New System.Drawing.Point(6, 2)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(208, 69)
        Me.GroupBox2.TabIndex = 26
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "PART NUMBER"
        '
        'btnflash
        '
        Me.btnflash.BackColor = System.Drawing.Color.LightSteelBlue
        Me.btnflash.Enabled = False
        Me.btnflash.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnflash.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnflash.Font = New System.Drawing.Font("Verdana", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnflash.ForeColor = System.Drawing.Color.Black
        Me.btnflash.Location = New System.Drawing.Point(523, 9)
        Me.btnflash.Name = "btnflash"
        Me.btnflash.Size = New System.Drawing.Size(102, 38)
        Me.btnflash.TabIndex = 3
        Me.btnflash.Text = "FLASH"
        Me.ToolTip1.SetToolTip(Me.btnflash, "CLICK TO FLASH")
        Me.btnflash.UseVisualStyleBackColor = False
        '
        'grpKeyboard
        '
        Me.grpKeyboard.BackColor = System.Drawing.Color.Transparent
        Me.grpKeyboard.Controls.Add(Me.Button2)
        Me.grpKeyboard.Controls.Add(Me.Button1)
        Me.grpKeyboard.Controls.Add(Me.Button7)
        Me.grpKeyboard.Controls.Add(Me.Button8)
        Me.grpKeyboard.Controls.Add(Me.Button9)
        Me.grpKeyboard.Controls.Add(Me.Button10)
        Me.grpKeyboard.Controls.Add(Me.Button11)
        Me.grpKeyboard.Controls.Add(Me.Button12)
        Me.grpKeyboard.Controls.Add(Me.Button13)
        Me.grpKeyboard.Controls.Add(Me.Button14)
        Me.grpKeyboard.Controls.Add(Me.Button15)
        Me.grpKeyboard.Controls.Add(Me.Button16)
        Me.grpKeyboard.Controls.Add(Me.Button17)
        Me.grpKeyboard.Controls.Add(Me.Button18)
        Me.grpKeyboard.Controls.Add(Me.Button19)
        Me.grpKeyboard.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpKeyboard.ForeColor = System.Drawing.Color.PaleGreen
        Me.grpKeyboard.Location = New System.Drawing.Point(10, 375)
        Me.grpKeyboard.Name = "grpKeyboard"
        Me.grpKeyboard.Size = New System.Drawing.Size(485, 105)
        Me.grpKeyboard.TabIndex = 29
        Me.grpKeyboard.TabStop = False
        Me.grpKeyboard.Text = "KEYBOARD"
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.ForeColor = System.Drawing.Color.Black
        Me.Button2.Location = New System.Drawing.Point(181, 53)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(35, 25)
        Me.Button2.TabIndex = 19
        Me.Button2.Text = "9"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.ForeColor = System.Drawing.Color.Black
        Me.Button1.Location = New System.Drawing.Point(139, 51)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(35, 25)
        Me.Button1.TabIndex = 18
        Me.Button1.Text = "8"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Button7
        '
        Me.Button7.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Button7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(1, Byte), Integer))
        Me.Button7.Location = New System.Drawing.Point(234, 19)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(68, 28)
        Me.Button7.TabIndex = 17
        Me.Button7.Text = "BACK SPACE"
        Me.Button7.UseVisualStyleBackColor = False
        '
        'Button8
        '
        Me.Button8.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Button8.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(1, Byte), Integer))
        Me.Button8.Location = New System.Drawing.Point(232, 129)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(70, 42)
        Me.Button8.TabIndex = 16
        Me.Button8.Text = "R"
        Me.Button8.UseVisualStyleBackColor = False
        '
        'Button9
        '
        Me.Button9.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Button9.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(1, Byte), Integer))
        Me.Button9.Location = New System.Drawing.Point(156, 129)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(70, 42)
        Me.Button9.TabIndex = 15
        Me.Button9.Text = "L"
        Me.Button9.UseVisualStyleBackColor = False
        '
        'Button10
        '
        Me.Button10.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Button10.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button10.ForeColor = System.Drawing.Color.Black
        Me.Button10.Location = New System.Drawing.Point(80, 129)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(70, 42)
        Me.Button10.TabIndex = 14
        Me.Button10.Text = "9"
        Me.Button10.UseVisualStyleBackColor = False
        '
        'Button11
        '
        Me.Button11.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Button11.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button11.ForeColor = System.Drawing.Color.Black
        Me.Button11.Location = New System.Drawing.Point(4, 129)
        Me.Button11.Name = "Button11"
        Me.Button11.Size = New System.Drawing.Size(70, 42)
        Me.Button11.TabIndex = 13
        Me.Button11.Text = "8"
        Me.Button11.UseVisualStyleBackColor = False
        '
        'Button12
        '
        Me.Button12.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Button12.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button12.ForeColor = System.Drawing.Color.Black
        Me.Button12.Location = New System.Drawing.Point(94, 51)
        Me.Button12.Name = "Button12"
        Me.Button12.Size = New System.Drawing.Size(35, 26)
        Me.Button12.TabIndex = 12
        Me.Button12.Text = "7"
        Me.Button12.UseVisualStyleBackColor = False
        '
        'Button13
        '
        Me.Button13.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Button13.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button13.ForeColor = System.Drawing.Color.Black
        Me.Button13.Location = New System.Drawing.Point(52, 53)
        Me.Button13.Name = "Button13"
        Me.Button13.Size = New System.Drawing.Size(34, 24)
        Me.Button13.TabIndex = 11
        Me.Button13.Text = "6"
        Me.Button13.UseVisualStyleBackColor = False
        '
        'Button14
        '
        Me.Button14.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Button14.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button14.ForeColor = System.Drawing.Color.Black
        Me.Button14.Location = New System.Drawing.Point(6, 53)
        Me.Button14.Name = "Button14"
        Me.Button14.Size = New System.Drawing.Size(40, 24)
        Me.Button14.TabIndex = 10
        Me.Button14.Text = "5"
        Me.Button14.UseVisualStyleBackColor = False
        '
        'Button15
        '
        Me.Button15.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Button15.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button15.ForeColor = System.Drawing.Color.Black
        Me.Button15.Location = New System.Drawing.Point(180, 20)
        Me.Button15.Name = "Button15"
        Me.Button15.Size = New System.Drawing.Size(36, 28)
        Me.Button15.TabIndex = 9
        Me.Button15.Text = "4"
        Me.Button15.UseVisualStyleBackColor = False
        '
        'Button16
        '
        Me.Button16.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Button16.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button16.ForeColor = System.Drawing.Color.Black
        Me.Button16.Location = New System.Drawing.Point(135, 19)
        Me.Button16.Name = "Button16"
        Me.Button16.Size = New System.Drawing.Size(39, 28)
        Me.Button16.TabIndex = 8
        Me.Button16.Text = "3"
        Me.Button16.UseVisualStyleBackColor = False
        '
        'Button17
        '
        Me.Button17.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Button17.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button17.ForeColor = System.Drawing.Color.Black
        Me.Button17.Location = New System.Drawing.Point(92, 19)
        Me.Button17.Name = "Button17"
        Me.Button17.Size = New System.Drawing.Size(37, 28)
        Me.Button17.TabIndex = 7
        Me.Button17.Text = "2"
        Me.Button17.UseVisualStyleBackColor = False
        '
        'Button18
        '
        Me.Button18.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Button18.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button18.ForeColor = System.Drawing.Color.Black
        Me.Button18.Location = New System.Drawing.Point(52, 20)
        Me.Button18.Name = "Button18"
        Me.Button18.Size = New System.Drawing.Size(34, 27)
        Me.Button18.TabIndex = 6
        Me.Button18.Text = "1"
        Me.Button18.UseVisualStyleBackColor = False
        '
        'Button19
        '
        Me.Button19.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Button19.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button19.ForeColor = System.Drawing.Color.Black
        Me.Button19.Location = New System.Drawing.Point(2, 19)
        Me.Button19.Name = "Button19"
        Me.Button19.Size = New System.Drawing.Size(44, 28)
        Me.Button19.TabIndex = 5
        Me.Button19.Text = "0"
        Me.Button19.UseVisualStyleBackColor = False
        '
        'GroupBox4
        '
        Me.GroupBox4.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox4.Controls.Add(Me.btnclose)
        Me.GroupBox4.Controls.Add(Me.btnReset)
        Me.GroupBox4.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox4.ForeColor = System.Drawing.Color.DodgerBlue
        Me.GroupBox4.Location = New System.Drawing.Point(522, 369)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(94, 111)
        Me.GroupBox4.TabIndex = 28
        Me.GroupBox4.TabStop = False
        '
        'lblPLC
        '
        Me.lblPLC.AutoSize = True
        Me.lblPLC.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPLC.ForeColor = System.Drawing.Color.PaleGreen
        Me.lblPLC.Location = New System.Drawing.Point(444, 9)
        Me.lblPLC.Name = "lblPLC"
        Me.lblPLC.Size = New System.Drawing.Size(42, 20)
        Me.lblPLC.TabIndex = 19
        Me.lblPLC.Text = "PLC"
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox1.Controls.Add(Me.txtBoxSRNO)
        Me.GroupBox1.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.Color.PaleGreen
        Me.GroupBox1.Location = New System.Drawing.Point(220, 2)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(208, 69)
        Me.GroupBox1.TabIndex = 27
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "SR NUMBER"
        '
        'txtBoxSRNO
        '
        Me.txtBoxSRNO.BackColor = System.Drawing.Color.LightYellow
        Me.txtBoxSRNO.Location = New System.Drawing.Point(6, 26)
        Me.txtBoxSRNO.MaxLength = 12
        Me.txtBoxSRNO.Name = "txtBoxSRNO"
        Me.txtBoxSRNO.Size = New System.Drawing.Size(197, 27)
        Me.txtBoxSRNO.TabIndex = 0
        Me.txtBoxSRNO.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'GroupBox3
        '
        Me.GroupBox3.BackColor = System.Drawing.Color.Teal
        Me.GroupBox3.Controls.Add(Me.lblCAL)
        Me.GroupBox3.Controls.Add(Me.lblAPP)
        Me.GroupBox3.Controls.Add(Me.lblSBL)
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox3.ForeColor = System.Drawing.Color.PaleGreen
        Me.GroupBox3.Location = New System.Drawing.Point(1, 243)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(494, 129)
        Me.GroupBox3.TabIndex = 30
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "FLASH STATUS"
        '
        'lblCAL
        '
        Me.lblCAL.AutoSize = True
        Me.lblCAL.BackColor = System.Drawing.Color.Teal
        Me.lblCAL.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblCAL.Location = New System.Drawing.Point(181, 88)
        Me.lblCAL.Name = "lblCAL"
        Me.lblCAL.Size = New System.Drawing.Size(101, 16)
        Me.lblCAL.TabIndex = 7
        Me.lblCAL.Text = "Not Started...."
        '
        'lblAPP
        '
        Me.lblAPP.AutoSize = True
        Me.lblAPP.BackColor = System.Drawing.Color.Teal
        Me.lblAPP.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblAPP.Location = New System.Drawing.Point(181, 59)
        Me.lblAPP.Name = "lblAPP"
        Me.lblAPP.Size = New System.Drawing.Size(101, 16)
        Me.lblAPP.TabIndex = 6
        Me.lblAPP.Text = "Not Started...."
        '
        'lblSBL
        '
        Me.lblSBL.AutoSize = True
        Me.lblSBL.BackColor = System.Drawing.Color.Teal
        Me.lblSBL.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblSBL.Location = New System.Drawing.Point(181, 30)
        Me.lblSBL.Name = "lblSBL"
        Me.lblSBL.Size = New System.Drawing.Size(101, 16)
        Me.lblSBL.TabIndex = 3
        Me.lblSBL.Text = "Not Started...."
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(10, 88)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(156, 16)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "CAL_FLASH STATUS"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 59)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(158, 16)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "APP_FLASH STATUS"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Teal
        Me.Label1.ForeColor = System.Drawing.Color.PaleGreen
        Me.Label1.Location = New System.Drawing.Point(6, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(156, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "SBL_FLASH STATUS"
        '
        'frmMaster
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Teal
        Me.ClientSize = New System.Drawing.Size(639, 492)
        Me.ControlBox = False
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lblPLC)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.grpKeyboard)
        Me.Controls.Add(Me.btnflash)
        Me.Controls.Add(Me.grpErrorMessage)
        Me.Controls.Add(Me.grpOperatorMessage)
        Me.Controls.Add(Me.GroupBox2)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMaster"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "OFFLINE FLASHING"
        Me.grpErrorMessage.ResumeLayout(False)
        Me.grpOperatorMessage.ResumeLayout(False)
        Me.grpOperatorMessage.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.grpKeyboard.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnclose As System.Windows.Forms.Button
    Friend WithEvents btnReset As System.Windows.Forms.Button
    Friend WithEvents tmrping As System.Windows.Forms.Timer
    Friend WithEvents grpErrorMessage As System.Windows.Forms.GroupBox
    Friend WithEvents lblerrmsg As System.Windows.Forms.Label
    Friend WithEvents FlashingProgressBar As System.Windows.Forms.ProgressBar
    Friend WithEvents txtPartNumber As System.Windows.Forms.TextBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents grpOperatorMessage As System.Windows.Forms.GroupBox
    Friend WithEvents lblopmsg As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents grpKeyboard As System.Windows.Forms.GroupBox
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents Button8 As System.Windows.Forms.Button
    Friend WithEvents Button9 As System.Windows.Forms.Button
    Friend WithEvents Button10 As System.Windows.Forms.Button
    Friend WithEvents Button11 As System.Windows.Forms.Button
    Friend WithEvents Button12 As System.Windows.Forms.Button
    Friend WithEvents Button13 As System.Windows.Forms.Button
    Friend WithEvents Button14 As System.Windows.Forms.Button
    Friend WithEvents Button15 As System.Windows.Forms.Button
    Friend WithEvents Button16 As System.Windows.Forms.Button
    Friend WithEvents Button17 As System.Windows.Forms.Button
    Friend WithEvents Button18 As System.Windows.Forms.Button
    Friend WithEvents Button19 As System.Windows.Forms.Button
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Public WithEvents lblFlashingcount As System.Windows.Forms.Label
    Friend WithEvents btnflash As Button
    Public WithEvents lblPLC As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents txtBoxSRNO As TextBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents lblCAL As Label
    Friend WithEvents lblAPP As Label
    Friend WithEvents lblSBL As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Button2 As Button
    Friend WithEvents Button1 As Button
End Class
