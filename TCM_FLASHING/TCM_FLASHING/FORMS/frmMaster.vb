Imports System.Configuration
Imports System.Deployment.Application
Imports System.Diagnostics
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Net.NetworkInformation
Imports System.Security.Cryptography.X509Certificates
Imports J2534DotNet
Imports TsOpcNet
Imports System.Drawing
Imports System.Drawing.Printing ' For PrintDocument
Imports ZXing
Imports ZXing.Common
Imports ZXing.OneD
Imports ZXing.BarcodeWriter


Public Class frmMaster
    Public Delegate Sub setTimerDelegate(ByVal blnFlag As Boolean)
    Public Delegate Sub delLabelChange(ByVal control As Label, ByVal text As String)
    'Public objClsGetData As New clsGetData(Me)
    Dim objSqlDS As New DataSet
    Public blnTrial As Boolean = False
    Public blnTrial_ED As Boolean = False
    Public DomainTrail As String = ""
    Public strIpAddress As String
    Public LogFilePath As String = ""
    Public blnConfigPressed As Boolean = False
    Public blnResetPressed As Boolean = False
    Dim objXmlConfig As New ConfigrationXML
    Public VCID_VCdatacount As New DataSet
    Private _labelBitmapToPrint As Bitmap
    Private WithEvents PrintDoc As New Printing.PrintDocument()
    Public Structure GlobalStatic
        Public SCAN_VIN_NO As String
        Public SCAN_VC_NO As String
        Public SCAN_ENGINE_NO As String
        Public DATE_TIME As Date
        Public STATION_ID As String
        Public TCF_LINE As String
        Public PLANT_CODE As String
        Public PRINTER_ADDRESS As String
        Public LOCALSERVERPINGSTATUS As Boolean
        Public PLANTSERVERPINGSTATUS As Boolean
        Public LOG_I2C1 As String
        Public LOG_I2C2 As String
        Public LOG_I2C3 As String
        Public LOG_I2C4 As String
        Public LOG_I2C5 As String
        Public LOG_I2C6 As String
        Public HSDEVICE_NO As String
        Public VERSION As String
        Public PLCBYPASS As String
        Public VOLTAGESELECTIONTAG As String
        Public IGNITIONSWITCHTAG As String
        Public PLCIPADDRESSS As String
        Public PLCTOPICNAME As String

        Public BL_NO As String
        Public FMID As String
        Public BL_VER As String
        Public BID As String

        Public NoofECUPLMCount As String
        Public NoofECUActualCount As String

        ''PASSTHRU

        Public COMMUNICATION_VCI As String
        Public PASSTHRU_VCI As String

        Public J2534VciIndex As Integer

        Public fileNames As List(Of String)()
        Public CRCfile As List(Of String)()
        Public commaSeparatedFileNames As String
        Public commaSeparatedCRCFileNames As String
        Public PartNumber As String
        Public SrNumber As String
        Public FlashResult As String
        ' Private variable to hold the generated bitmap that will be sent to the printer

    End Structure

    Public objGlobalStatic As GlobalStatic

#Region "PLC RELATED VARIABLE DECLARATION"
    Dim IgnitionTag As Integer = 2
    Dim Ignition_On As Integer = 1
    Dim CheckDelayON As Integer = 0
    Dim CheckVoltage As Integer = 0
    Public m_OpcDaServer As New TsCDaServer
    Public strTopicName As String
    Public opcGroup_PLC_N70 As TsCDaSubscription
    Public opcVoltageSelection As TsCDaSubscription
    Public opcignitionOn As TsCDaSubscription
    Public opcGenericRead As TsCDaItem()
    Public opcItem_TestStart As TsOpcNet.TsCDaItem()
    Public opcItem_TestOver As TsOpcNet.TsCDaItem()
    Public opcItem_ReRead_Error As TsOpcNet.TsCDaItem()

#End Region

#Region "Label change"
    Public Sub labelChange(ByVal control As Label, ByVal text As String)
        control.Text = text
        control.Refresh()
    End Sub
#End Region

    Public ECUID As Integer
    Private Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReset.Click
        blnResetPressed = True
        CloseApplicationAndStart()
    End Sub

    Private Sub btnclose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnclose.Click
        WriteLog("*********************************************************************")
        WriteLog("-----OFFLINE_FLASHING_FormClosed-----" & DateTime.Now.ToString)
        WriteLog("*********************************************************************")
        GC.Collect()
        ''****RENAME LOG FILE BY VIN_NO & TIME ****************
        RenameLogFile()
        ''*****************************************************
        Dim pProcess As Process() = System.Diagnostics.Process.GetProcessesByName(Application.ProductName)
        If pProcess.Length > 1 Then

        End If

        Dim allProcesses As System.Diagnostics.Process() = System.Diagnostics.Process.GetProcesses()
        Dim id As Integer = 0
        For iii As Integer = 0 To allProcesses.Length - 1
            '' If DirectCast(allProcesses(iii), System.Diagnostics.Process).ProcessName.Contains("UTK OFFLINE FLASHING") Then
            If DirectCast(allProcesses(iii), System.Diagnostics.Process).ProcessName.Contains("VCID OFFLINE FLASHING") Then
                DirectCast(allProcesses(iii), System.Diagnostics.Process).Kill()
            End If
        Next

        Dim pname As String = System.Diagnostics.Process.GetCurrentProcess.ProcessName
        Dim ArrPname() As String = pname.Split("."c)
        pname = ArrPname(0)
        For iii As Integer = 0 To allProcesses.Length - 1
            If DirectCast(allProcesses(iii), System.Diagnostics.Process).ProcessName.Contains(pname) Then
                DirectCast(allProcesses(iii), System.Diagnostics.Process).Kill()
            End If
        Next
        'ReleaseDll()
    End Sub

    Sub DeleteFilesFromFolder(ByVal Folder As String)
        If Directory.Exists(Folder) Then
            For Each _file As String In Directory.GetFiles(Folder)
                File.Delete(_file)
            Next
            For Each _folder As String In Directory.GetDirectories(Folder)
                DeleteFilesFromFolder(_folder)
            Next
        End If
    End Sub

    ''Public Function WriteLog(ByVal str As String) As Boolean
    ''    Try
    ''        '*************************** CREATING FOLDER *************************************
    ''        Dim path As String
    ''        Dim LogFilePath As String = My.Settings.Log_path
    ''        path = LogFilePath + Microsoft.VisualBasic.Format(Now, " d").ToString + ""

    ''        If Directory.Exists(path) = False Then
    ''            Directory.CreateDirectory(path)
    ''        End If

    ''        Dim dt As DateTime = Directory.GetCreationTime(path)

    ''        If DateTime.Now.Subtract(dt).TotalDays > 1 Then 'Or DateTime.Now.Subtract(dt).TotalDays < 0 Then
    ''            Dim s As String  ''Delete Previous Log files
    ''            For Each s In System.IO.Directory.GetFiles(path)
    ''                System.IO.File.Delete(s)
    ''            Next s
    ''            DeleteFilesFromFolder(path)
    ''            For Each subfolder As String In Directory.GetDirectories(path)
    ''                System.IO.Directory.Delete(subfolder)
    ''            Next
    ''            Directory.Delete(path)
    ''            Directory.CreateDirectory(path)
    ''        End If


    ''        '*************************** CREATING FILE AND LOG *************************************

    ''        ''Dim strFolder As String = Microsoft.VisualBasic.Format(Now, " d").ToString 'For DateWise Folder
    ''        ''Dim strDay As String = Microsoft.VisualBasic.Format(Now, " dd_MM_yyyy").ToString
    ''        ''Dim strDayTime As String = Microsoft.VisualBasic.Format(Now, " _hhtt").ToString


    ''        ''Dim filepath As String = LogFilePath + strFolder + "\" & My.Settings.LOG_FILE_NAME & "_" + strDay + ".txt"
    ''        ''Dim destFilePath As String = ""

    ''        ''If System.IO.File.Exists(filepath) Then
    ''        ''    Dim objStream As System.IO.FileStream
    ''        ''    objStream = New System.IO.FileStream(filepath, IO.FileMode.Open)

    ''        ''    If objStream.Length > My.Settings.LOG_FILE_LENGTH Then                 'IF File Size Exeeds Limit then Copy to new File. 
    ''        ''        objStream.Close()
    ''        ''        destFilePath = LogFilePath + strFolder + "\" & My.Settings.LOG_FILE_NAME & "_" + strDay + strDayTime + ".txt"

    ''        ''        If System.IO.File.Exists(destFilePath) Then    'IF New File Already Exits then Delete it
    ''        ''            System.IO.File.Delete(destFilePath)
    ''        ''        End If
    ''        ''        System.IO.File.Copy(filepath, destFilePath)
    ''        ''        System.IO.File.Delete(filepath)
    ''        ''    End If
    ''        ''    objStream.Dispose()
    ''        ''    objStream = Nothing
    ''        ''End If

    ''        Dim strFolder As String = Microsoft.VisualBasic.Format(Now, " d").ToString 'For DateWise Folder
    ''        Dim strDay As String = Microsoft.VisualBasic.Format(Now, " dd_MM_yyyy").ToString
    ''        Dim strDayTime As String = Microsoft.VisualBasic.Format(Now, " _hhtt").ToString


    ''        Dim filepath As String = System.Configuration.ConfigurationManager.AppSettings("LOG_FILE_PATH") + strFolder + "\" & System.Configuration.ConfigurationManager.AppSettings("LOG_FILE_NAME") & "_" + strDay + ".txt"
    ''        Dim destFilePath As String = ""

    ''        If System.IO.File.Exists(filepath) Then
    ''            Dim objStream As System.IO.FileStream
    ''            objStream = New System.IO.FileStream(filepath, IO.FileMode.Open)

    ''            If objStream.Length > System.Configuration.ConfigurationManager.AppSettings("LOG_FILE_LENGTH") Then                 'IF File Size Exeeds Limit then Copy to new File. 
    ''                objStream.Close()
    ''                destFilePath = System.Configuration.ConfigurationManager.AppSettings("LOG_FILE_PATH") + strFolder + "\" & System.Configuration.ConfigurationManager.AppSettings("LOG_FILE_NAME") & "_" + strDay + strDayTime + ".txt"

    ''                If System.IO.File.Exists(destFilePath) Then    'IF New File Already Exits then Delete it
    ''                    System.IO.File.Delete(destFilePath)
    ''                End If
    ''                System.IO.File.Copy(filepath, destFilePath)
    ''                System.IO.File.Delete(filepath)
    ''            End If
    ''            objStream.Dispose()
    ''            objStream = Nothing
    ''        End If
    ''        '*************** Writing Data to Log File ***********************************
    ''        LogFilePath = filepath
    ''        Dim objWriter As New System.IO.StreamWriter(filepath, True)
    ''        objWriter.WriteLine("[" & System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:ff") & "] -" & str)
    ''        objWriter.Close()
    ''        objWriter.Dispose()
    ''        objWriter = Nothing
    ''        ' ''*************** Writing Data to Log File ***********************************
    ''        ''Dim objWriter As New System.IO.StreamWriter(filepath, True)
    ''        ''objWriter.WriteLine("[" & System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:ff") & "] -" & str)
    ''        ''objWriter.Close()
    ''        ''objWriter.Dispose()
    ''        ''objWriter = Nothing
    ''        Return True
    ''    Catch ex As Exception
    ''        Return False
    ''    End Try
    ''End Function

    Public Sub WriteLog(ByVal str As String)
        WriteToFile(str)
    End Sub



    Public Sub ChangeLableValue(ByVal str As String, ByVal Result As Boolean, ByVal intplace As Integer)
        If intplace = 1 Then
            If str.ToUpper = "SBL" Then
                If Result = True Then
                    lblSBL.Text = "Flashing Successful"
                    lblSBL.BackColor = Color.GreenYellow
                Else
                    lblSBL.Text = "Flashing Failed"
                    lblSBL.BackColor = Color.Red
                End If
            ElseIf str.ToUpper = "APP" Then
                If Result = True Then
                    lblAPP.Text = "Flashing Successful"
                    lblAPP.BackColor = Color.GreenYellow
                Else
                    lblAPP.Text = "Flashing Failed"
                    lblAPP.BackColor = Color.Red
                End If
            ElseIf str.ToUpper = "CAL" Then
                If Result = True Then
                    lblCAL.Text = "Flashing Successful"
                    lblCAL.BackColor = Color.GreenYellow
                Else
                    lblCAL.Text = "Flashing Failed"
                    lblCAL.BackColor = Color.Red
                End If
            Else
            End If
        Else
            If str.ToUpper = "SBL" Then
                lblSBL.Text = "Flashing in Progress"
                lblSBL.BackColor = Color.Yellow

                ElseIf str.ToUpper = "APP" Then

                lblAPP.Text = "Flashing in Progress"
                lblAPP.BackColor = Color.Yellow

            ElseIf str.ToUpper = "CAL" Then

                lblCAL.Text = "Flashing in Progress"
                lblCAL.BackColor = Color.Yellow

            Else
            End If
        End If

    End Sub

    Private Function WriteToFile(ByVal str As String) As Boolean
        Try
            '*************************** CREATING FOLDER *************************************
            Dim Path As String = My.Settings.Log_path
            ' path = System.Configuration.ConfigurationManager.AppSettings("LOG_FILE_PATH") + Microsoft.VisualBasic.Format(Now, " d").ToString + ""
            Path = Path + Microsoft.VisualBasic.Format(Now, " d").ToString + ""
            If Directory.Exists(Path) = False Then
                Directory.CreateDirectory(Path)
            End If
            Dim dt As DateTime = Directory.GetCreationTime(Path)
            If DateTime.Now.Subtract(dt).TotalDays > 1 Then 'Or DateTime.Now.Subtract(dt).TotalDays < 0 Then
                DeleteFilesFromFolder(Path) ''Delete Previous Log files
                Directory.Delete(Path)
                Directory.CreateDirectory(Path)
            End If
            '*************************** CREATING FILE AND LOG *************************************
            Dim strFolder As String = Microsoft.VisualBasic.Format(Now, " d").ToString 'For DateWise Folder
            Dim strDay As String = Microsoft.VisualBasic.Format(Now, " dd_MM_yyyy").ToString
            Dim strDayTime As String = Microsoft.VisualBasic.Format(Now, " _hhtt").ToString


            ''  Dim filepath As String = System.Configuration.ConfigurationManager.AppSettings("LOG_FILE_PATH") + strFolder + "\" & System.Configuration.ConfigurationManager.AppSettings("LOG_FILE_NAME") & "_" + strDay + ".txt"
            Dim filepath As String = My.Settings.Log_path + strFolder + "\" & My.Settings.LOG_FILE_NAME & "_" + strDay + ".txt"
            Dim destFilePath As String = ""

            If System.IO.File.Exists(filepath) Then
                Dim objStream As System.IO.FileStream
                objStream = New System.IO.FileStream(filepath, IO.FileMode.Open)

                If objStream.Length > My.Settings.LOG_FILE_LENGTH Then                 'IF File Size Exeeds Limit then Copy to new File. 
                    objStream.Close()
                    ''  destFilePath = System.Configuration.ConfigurationManager.AppSettings("LOG_FILE_PATH") + strFolder + "\" & System.Configuration.ConfigurationManager.AppSettings("LOG_FILE_NAME") & "_" + strDay + strDayTime + ".txt"
                    destFilePath = My.Settings.Log_path + strFolder + "\" & My.Settings.LOG_FILE_NAME & "_" + strDay + strDayTime + ".txt"
                    If System.IO.File.Exists(destFilePath) Then    'IF New File Already Exits then Delete it
                        System.IO.File.Delete(destFilePath)
                    End If
                    System.IO.File.Copy(filepath, destFilePath)
                    System.IO.File.Delete(filepath)
                End If
                objStream.Dispose()
                objStream = Nothing
            End If
            '*************** Writing Data to Log File ***********************************
            LogFilePath = filepath
            Dim objWriter As New System.IO.StreamWriter(filepath, True)
            objWriter.WriteLine("[" & System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:ff") & "] -" & str)
            objWriter.Close()
            objWriter.Dispose()
            objWriter = Nothing

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub RenameLogFile()
        Try
            If System.IO.File.Exists(LogFilePath) AndAlso blnConfigPressed = False AndAlso blnResetPressed = False Then
                'Dim objEncryptDll As New EncrpytDecrypt.EncrpytDecrypt.clsCryptoProvider
                Dim strt As String = DateTime.Now().ToString.Replace("/", "-").ToString
                strt = strt.Replace(" ", "_").ToString
                strt = strt.Replace(":", "_").ToString
                'Dim strt1 As String = strt.Replace(" ", "_").ToString
                'Dim strt2 As String = strt1.Replace(":", "-").ToString
                '''''''Log EncryptData removed Work From Home time'''''
                'objEncryptDll.EncryptData(EncryptKey.LogFileEncryptionKey, LogFilePath)
                My.Computer.FileSystem.RenameFile(LogFilePath, objGlobalStatic.SrNumber & "_" & strt & "Log" & ".txt")
            End If
        Catch ex As Exception
            WriteLog(ex.Message)
        End Try
    End Sub

    Public Structure EncryptKey
        Public Shared LogFileEncryptionKey As String = "ed6705TATALog"
        Public Shared FlashFileEncryptionKey As String = "ed6705TATAFlash"
    End Structure

    Private Function CloseLog() As Boolean
        Dim flag As Boolean
        Try
            Trace.Close()
            flag = True
        Catch ex As Exception

        End Try
        Return flag
    End Function

    'Private Sub tmrping_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrping.Tick
    '    Dim blnpingflag As Boolean
    '    ''# PINGS TO THE MAIN DATABASE SERVER -------------------------------------------------------------
    '    Try
    '        Dim MainServerIP As String = "172.27.32.49"
    '        blnpingflag = My.Computer.Network.Ping(MainServerIP)

    '        If (Not blnpingflag) Then
    '            lblPingNW.Text = "NETSTOP"
    '            lblPingNW.BackColor = Color.Red
    '            Beep()
    '        Else
    '            lblPingNW.Text = "NETOK"
    '            lblPingNW.BackColor = Color.Green
    '        End If
    '    Catch ep As System.InvalidOperationException
    '        lblPingNW.Text = "NETSTOP"
    '        lblPingNW.BackColor = Color.Red
    '        Beep()
    '    Catch ex As Exception
    '        Beep()
    '    Finally
    '        GC.Collect()
    '    End Try
    '    ''#-------------------------------------------------------------------------------------------------------------
    'End Sub

    Private Sub frmMaster_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        System.Threading.Thread.CurrentThread.Name = "MainThread"
        'Call from main form of you application in the Form_Load event
        If frmMaster.IsAlreadyOpen(Application.ProductName) Then
            WriteLog("You cannot open 2 instances of " &
         Application.ProductName & " running at same time." & "Error: Application already open")
            ''frmMaster.ActivatePrevInstance()
            Application.Exit()
        End If


        InitializeApp()
        WriteLog("-----OFFLINE_FLASHING_FORMLOAD-----" & DateTime.Now.ToString)
        Me.Invoke(New setTimerDelegate(AddressOf setTimer), New Object() {True})
        Me.Invoke(New setTimerDelegate(AddressOf setTimer), New Object() {True})
        txtPartNumber.ReadOnly = False
        Dim ColApplicationData As New Collection
        Dim ColApplicationData_Name As New Collection
        ''Dim objConfigXML As New clsPrevVc
        'objConfigXML.CreateXMLFile()
        'objConfigXML.funGetApplicationXMLData(ColApplicationData, ColApplicationData_Name)
        'Dim i As Integer = -1
        'Dim j As Integer = 5
        'For i = 1 To 5
        '    If Not String.IsNullOrEmpty(ColApplicationData("VC" & j.ToString)) Then
        '        'cmbLastFive.Items.Add(ColApplicationData("VC" & j.ToString))
        '    End If
        '    j -= 1
        'Next
        'Dim objConf As New ConfigrationXML
        'objConf.funCreateXMLFile()


        'If GetConfigData() Then
        'End If
        '*********************************************************************************
        objGlobalStatic.COMMUNICATION_VCI = "PASSTHRU"
        objGlobalStatic.PASSTHRU_VCI = "GARUDA"

        '*********************************************************
        ''For PASSTHRU VCI Detection 
        If objGlobalStatic.COMMUNICATION_VCI.ToUpper() = "PASSTHRU" Then
            Try
                Dim DeviceList As List(Of J2534Device)
                DeviceList = J2534Detect.ListDevices()
                Dim intVCICount As Integer = DeviceList.Count
                Dim intIndex As Integer = 0
                For intIndex = 0 To intVCICount - 1
                    If (objGlobalStatic.PASSTHRU_VCI = DeviceList(intIndex).Name.ToUpper) Then
                        objGlobalStatic.J2534VciIndex = intIndex
                        Exit For
                    End If
                Next intIndex
            Catch ex As Exception
                objGlobalStatic.J2534VciIndex = 0
                WriteLog("exception during finding index of garuda: " & ex.ToString())
            End Try
        End If
        '*********************************************************

        ''''''Added Config Logic for VCID and VC availability VCID or 16R
        '  lblData.Visible = False
        Dim Msg As String = ""
        'If objClsGetData.funRead_VCID_VCDATA(VCID_VCdatacount, Msg) = False Then
        '    MsgBox("PLEASE CHECK NETWORK CONNECTIVITY OR VC DATA AVAILABLE.RESET APPLICATION")
        'Else
        '    WriteLog("VCID DATA AVAILABLE ON PLANT SERVER")
        'End If
        '*********************************************************************************



        If (ApplicationDeployment.IsNetworkDeployed) Then
            Dim AD As ApplicationDeployment = ApplicationDeployment.CurrentDeployment
            Me.Text = Me.Text & " V " & AD.CurrentVersion.ToString & "   STATION ID :- " & objGlobalStatic.STATION_ID & " HS_PLUS_SERIAL_NUMBER :- " & objGlobalStatic.HSDEVICE_NO
        Else
            Me.Text = Me.Text & " V " & My.Application.Info.Version.ToString
        End If
        InsertDataIntoListView("PLEASE SCAN OR SELECT VC NUMBER.", "OK")
        Me.Refresh()
        Me.Select()
        Me.Focus()
        ''''********************MACID INTERLOCK LOGIC******************************************************
        Dim strMsg As String = ""
        ''If objClsGetData.funReadCompareMACId(clsGetData.enum_ServerType.PlantServer, strMsg) = False Then
        ''    ''txtVcNo.Text = ""
        ''    ''   TimerScanBarcode.Stop()
        ''    MsgBox("THIS MACHINE IS NOT REGISTERED OR NOT INSTALLED UPDATED VERSION  FOR ECU OFFLINE FLASHING" + Environment.NewLine + "IF YOU WANT TO USE ECU OFFLINE FLASHING" + Environment.NewLine + "PLEASE,CONTACT TO ELECTRONICS DIVISION", vbOKOnly, "ECU OFFLINE FLASHING")
        ''    btnOK.Enabled = False
        ''    btnflash.Enabled = False
        ''    cmbLastFive.Enabled = False
        ''    txtPartNumber.Enabled = False
        ''    InsertDataIntoListView("THIS MACHINE IS NOT REGISTERED OR NOT INSTALLED UPDATED VERSION FOR ECU OFFLINE FLASHING.", "NOK")
        ''    Exit Sub
        ''End If

        ''''''*********************PLC LOGIC ADDED*****************
        '''
        '''Retrive PLC values form APP.config file
        '''
        objGlobalStatic.PLCBYPASS = ConfigurationManager.AppSettings("PCLBYPASS")
        objGlobalStatic.PLCTOPICNAME = ConfigurationManager.AppSettings("PLCTOPICNAME")
        '''Retrive PLC values form APP.config file




        Dim boolFlagGroupAdded As Boolean = False
        Try
            If objGlobalStatic.PLCBYPASS.Trim().ToUpper() = "NO" Then
                strTopicName = objGlobalStatic.PLCTOPICNAME ' ''System.Configuration.ConfigurationManager.AppSettings("PLCTOPICNAME")

                Try
                    If (Not initalizeOPCServer()) Then        'FUNCTION CALL TO INITALIZE OPC SERVER
                        InsertDataIntoListView("FAILED TO INITALIZE OPC SERVER", "NOK")
                        lblPLC.BackColor = Color.DarkRed
                    Else
                        boolFlagGroupAdded = Group_Item("[" & strTopicName & "]" & objGlobalStatic.VOLTAGESELECTIONTAG, 1, "VolatageSelection", 1000, opcVoltageSelection)
                        boolFlagGroupAdded = Group_Item("[" & strTopicName & "]" & objGlobalStatic.IGNITIONSWITCHTAG, 1, "IgnitionON", 1000, opcignitionOn)
                        lblPLC.BackColor = Color.Green
                    End If
                Catch ex As Exception
                    lblPLC.BackColor = Color.DarkRed
                    WriteLog("-----FAILED TO INITALIZE OPC SERVER-----" & DateTime.Now.ToString)
                End Try

                Try
                    If objGlobalStatic.PLCBYPASS.Trim.ToUpper.Contains("NO") Then
                        WriteOPCData("0", opcVoltageSelection, "VolatageSelection")
                        WriteOPCData("0", opcignitionOn, "IgnitionSelection")
                    End If

                Catch ex As Exception

                End Try
            Else
                lblPLC.BackColor = Color.Green
                lblPLC.Text = "PLC NA"
                lblPLC.Font = New Font("Arial", 8)
            End If

        Catch ex As Exception
            WriteLog(ex.Message)
        Finally
            '' txtVcNo.Focus()
            Me.Refresh()
            Me.Select()
            Me.Focus()
        End Try
    End Sub

    Public Function GetConfigData() As Boolean
        Try
            Dim ColApplicationData As New Collection
            ColApplicationData = objXmlConfig.GetHSPlusPrinterData()
            Dim ColProductData As New Collection
            ColProductData = objXmlConfig.GetLocalMachineData()

            objGlobalStatic.HSDEVICE_NO = ColApplicationData("HS_PLUS_SERIAL_NUMBER")
            objGlobalStatic.PLANT_CODE = ColProductData("PLANT_CODE")
            objGlobalStatic.TCF_LINE = ColProductData("TCF_LINE")
            objGlobalStatic.PRINTER_ADDRESS = ColApplicationData("PRINTER_ADDRESS")


            objGlobalStatic.COMMUNICATION_VCI = ColApplicationData("COMMUNICATION_VCI")
            objGlobalStatic.PASSTHRU_VCI = ColApplicationData("PASSTHRU_VCI")



            objGlobalStatic.STATION_ID = GetIPAddress(strIpAddress) & "-" & objGlobalStatic.TCF_LINE
            If objGlobalStatic.TCF_LINE = "BENCH_TEST" Then
                '  objClsGetData.ED_Trial = True
            Else
                ' objClsGetData.ED_Trial = False
            End If

            ''''PLC data added
            Dim colPLCCollectionData As New Collection
            colPLCCollectionData = objXmlConfig.getPLCData()

            If colPLCCollectionData IsNot Nothing Then
                objGlobalStatic.PLCBYPASS = Trim(colPLCCollectionData("PLCBYPASS"))
                objGlobalStatic.PLCIPADDRESSS = Trim(colPLCCollectionData("PLCIPADDRESS"))
                objGlobalStatic.VOLTAGESELECTIONTAG = Trim(colPLCCollectionData("VOLTAGESELECTIONTAG"))
                objGlobalStatic.IGNITIONSWITCHTAG = Trim(colPLCCollectionData("IGNITIONSWITCHTAG"))
                objGlobalStatic.PLCTOPICNAME = Trim(colPLCCollectionData("PLCTOPICNAME"))
            Else
                objGlobalStatic.PLCBYPASS = "YES"
                objGlobalStatic.PLCIPADDRESSS = "192.168.1.10"
                objGlobalStatic.VOLTAGESELECTIONTAG = "N7:2"
                objGlobalStatic.IGNITIONSWITCHTAG = "N7:3"
                objGlobalStatic.PLCTOPICNAME = "OFFLINE_FLASHING"
            End If



            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub InitializeApp()
        Try
            WriteLog(vbCrLf & "	ECU OFFLINE FLASHING STATION ")
            WriteLog("=======================================================================" & vbCrLf)
            WriteLog("LOG DATE       : " & System.DateTime.Now)
            WriteLog("STATION NUMBER : " & System.Configuration.ConfigurationManager.AppSettings("STATION ID"))
            WriteLog("=======================================================================" & vbCrLf)
        Catch ex As Exception
            WriteLog("InitializeApp::" & ex.Message)
        End Try
    End Sub


    Private Function GetIPAddress(ByRef strIpAddress As String) As String
        Try
            Dim myHost As String = System.Net.Dns.GetHostName
            Dim myIPs As System.Net.IPHostEntry = System.Net.Dns.GetHostByName(myHost)
            'Dim strIpAddress As String = ""
            For Each myIP As System.Net.IPAddress In myIPs.AddressList
                strIpAddress = myIP.ToString
            Next
            Return strIpAddress
        Catch ex As Exception
            WriteLog("GetIPAddress::" & ex.Message)
            Return Nothing
        End Try
    End Function

#Region " setTimer "
    Public Sub setTimer(ByVal blnFlag As Boolean)
        tmrping.Enabled = blnFlag
        If (blnFlag) Then
            tmrping.Start()
        Else
            tmrping.Stop()
        End If
    End Sub
#End Region

    Private Sub btnConfigParam_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        blnConfigPressed = True
        tmrping.Enabled = False
        Me.Hide()
        ' frmEOL_Config.Show()
    End Sub

    Private Sub btn0_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click, Button8.Click, Button19.Click, Button18.Click, Button17.Click, Button16.Click, Button15.Click, Button14.Click, Button13.Click, Button12.Click, Button11.Click, Button10.Click
        Dim B1 As Button = DirectCast(sender, Button)
        If txtPartNumber.Text.Length < 12 Then
            txtPartNumber.Text = (txtPartNumber.Text.Trim & B1.Text).Trim
        End If
    End Sub

    Private Sub btnBackSpace_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        If Not (txtPartNumber.Text.Length = 0) Then
            Dim partLength As Integer
            partLength = txtPartNumber.Text.Trim.Length() - 1
            txtPartNumber.Text = txtPartNumber.Text.Substring(0, partLength).Trim
        End If
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        WriteLog("---btnOK_Click ------------")
        'chkTrial.Enabled = False
        Try
            If Not String.IsNullOrEmpty(txtPartNumber.Text) Then
                If txtPartNumber.Text.Length >= 32 Then

                    objGlobalStatic.PartNumber = txtPartNumber.Text.Substring(2, 12)
                    objGlobalStatic.SrNumber = txtPartNumber.Text.Substring(26, 6)
                    txtPartNumber.Text = objGlobalStatic.PartNumber
                    txtBoxSRNO.Text = objGlobalStatic.SrNumber
                    Dim folderpath As String = ""

                    Dim assemblyLocation As String = System.Reflection.Assembly.GetExecutingAssembly().Location
                    Dim assemblyDirectory As String = System.IO.Path.GetDirectoryName(assemblyLocation)

                    folderpath = System.IO.Path.GetDirectoryName(assemblyDirectory)



                    folderpath = folderpath & "\Flashing Files\" & txtPartNumber.Text.ToString()

                    If Not String.IsNullOrEmpty(folderpath) AndAlso System.IO.Directory.Exists(folderpath) Then
                        Dim files As String() = System.IO.Directory.GetFiles(folderpath)
                        Dim fileNames As New List(Of String)()
                        Dim CRCfile As New List(Of String)()

                        For Each filePath As String In files
                            If System.IO.Path.GetExtension(filePath).Equals(".S19", StringComparison.OrdinalIgnoreCase) Then
                                fileNames.Add(System.IO.Path.GetFullPath(filePath))
                            ElseIf System.IO.Path.GetExtension(filePath).Equals(".CRC", StringComparison.OrdinalIgnoreCase) Then
                                CRCfile.Add(System.IO.Path.GetFullPath(filePath))
                            End If

                        Next

                        objGlobalStatic.commaSeparatedFileNames = String.Join("; ", fileNames)
                        objGlobalStatic.commaSeparatedCRCFileNames = String.Join("; ", CRCfile)

                    Else

                    End If
                End If
            End If


            'Dim str_9th_digit As String = txtPartNumber.Text.ToString.Substring(8, 1)
            'Dim str_last_digit As String = txtPartNumber.Text.ToString.Substring(11)
            'If Not (str_last_digit = "R" OrElse str_last_digit = "L") Then
            '    MessageBox.Show("PLEASE ENETR VC ONLY")
            '    InsertDataIntoListView("PLEASE ENETR VC ONLY", "NOK")
            '    txtPartNumber.Text = ""
            '    txtPartNumber.Focus()
            '    Exit Sub
            'End If
            'objGlobalStatic.SCAN_VC_NO = txtPartNumber.Text.Trim
            'If IsNumeric(str_9th_digit) Then
            '    objGlobalStatic.SCAN_VC_NO = objGlobalStatic.SCAN_VC_NO.Substring(0, 9) & "00" & str_last_digit
            'Else
            '    objGlobalStatic.SCAN_VC_NO = objGlobalStatic.SCAN_VC_NO.Substring(0, 8) & "000" & str_last_digit
            'End If
            txtPartNumber.Enabled = False
            btnflash.Enabled = True
            txtPartNumber.Enabled = False
            grpKeyboard.Enabled = False
            'cmbLastFive.Enabled = False
            'btnConfigParam.Visible = False
            'btnConfigParam.Enabled = False
            btnOK.Visible = False
            WriteLog("DOWNLOADING ULP FILE AND DATA" & " DownloadAndFlash() CALLED")


            Dim Message As String

        Catch ex As Exception
            WriteLog("EXCEPTION DURING btnOK_Click()" & ex.Message())
        End Try

    End Sub

    Private Sub CloseApplicationAndStart_OLD()
        Try
            ''****RENAME LOG FILE BY VIN_NO & TIME ****************
            RenameLogFile()
            ''*****************************************************
            Trace.WriteLine("CloseApplicationAndStart")
            Application.Restart()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CloseApplicationAndStart()
        Try
            ''****RENAME LOG FILE BY VIN_NO & TIME ****************
            RenameLogFile()
            ''*****************************************************
            Trace.WriteLine("CloseApplicationAndStart")
            GC.Collect()
            Dim pProcess As Process() = System.Diagnostics.Process.GetProcessesByName(Application.ProductName)
            If pProcess.Length > 1 Then
            End If
            Dim pname As String = System.Diagnostics.Process.GetCurrentProcess.ProcessName
            Dim ArrPname() As String = pname.Split("."c)
            pname = ArrPname(0)
            If (pname.ToString.Equals(Application.ProductName)) Then
                Dim MYprocess As Process
                MYprocess = System.Diagnostics.Process.GetCurrentProcess
                Trace.WriteLine(Application.ExecutablePath)
                ''System.Diagnostics.Process.Start(Application.ExecutablePath)
                Application.Restart()
                Trace.WriteLine("Application Reset Pressed")
                MYprocess.Kill()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtPartNumber_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPartNumber.TextChanged
        txtPartNumber.Text = txtPartNumber.Text.ToUpper
        txtPartNumber.SelectionStart = txtPartNumber.TextLength
        If txtPartNumber.TextLength = 32 Then
            btnOK.Enabled = True
            Me.Invoke(New delLabelChange(AddressOf labelChange), New Object() {lblopmsg, "PRESS 'OK'"})
        ElseIf txtPartNumber.TextLength = 1 Then
            Me.Invoke(New delLabelChange(AddressOf labelChange), New Object() {lblerrmsg, ""})
        ElseIf txtPartNumber.TextLength < 32 Then
            Me.Invoke(New delLabelChange(AddressOf labelChange), New Object() {lblopmsg, ""})
            btnOK.Enabled = False
        End If
    End Sub

    'Private Sub cmbLastFive_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    txtPartNumber.Text = cmbLastFive.SelectedItem.ToString()
    'End Sub

#Region "Constructor"
    Public Sub New()
        InitializeComponent()
    End Sub
#End Region

#Region "Message Displaying"
    Public Sub InsertDataIntoListView(ByVal text As String, ByVal status As String)
        WriteLog("Printed to Label : " & text & " ")
        If status = "NOK" Then
            lblerrmsg.ForeColor = Color.Red
            lblerrmsg.Font = New Drawing.Font("Times New Roman", 16, FontStyle.Bold)
            Me.Invoke(New delLabelChange(AddressOf labelChange), New Object() {lblerrmsg, text})
        ElseIf status = "NOK1" Then
            lblerrmsg.ForeColor = Color.Red
            lblerrmsg.Font = New Drawing.Font("Times New Roman", 10, FontStyle.Bold Or FontStyle.Italic)
            Me.Invoke(New delLabelChange(AddressOf labelChange), New Object() {lblerrmsg, text})
        ElseIf status = "OK" Then
            lblerrmsg.ForeColor = Color.PaleGreen
            lblerrmsg.Font = New Drawing.Font("Times New Roman", 16, FontStyle.Bold)
            Me.Invoke(New delLabelChange(AddressOf labelChange), New Object() {lblerrmsg, text})
        End If
        Application.DoEvents()
    End Sub
#End Region

#Region "btnFlash"

    Private Sub btnflash_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnflash.Click
        WriteLog("------- FLASH BUTTON_Click() -------Start ")
        WriteLog("---Start Flashing---flash button click")
        Disable()
        Me.Invoke(New delLabelChange(AddressOf labelChange), New Object() {lblopmsg, ""})
        Me.Invoke(New delLabelChange(AddressOf labelChange), New Object() {lblerrmsg, ""})
        Me.Invoke(New delLabelChange(AddressOf labelChange), New Object() {lblopmsg, "FLASHING IN PROGRESS PLEASE WAIT..."})
        Me.Invoke(New delLabelChange(AddressOf labelChange), New Object() {lblerrmsg, ""})
        objGlobalStatic.FlashResult = "FAILED"


        Try

            If funSelectECUandstartRoutine() Then
                objGlobalStatic.FlashResult = "PASSED"
                InsertDataIntoListView("PRINTING IN PROGRESS", "OK")
                PrintDocument()
                InsertDataIntoListView("PRINTING SUCCESSFULL", "OK")
                Me.Invoke(New delLabelChange(AddressOf labelChange), New Object() {lblopmsg, "PLEASE WAIT UNTILL MAIN RELAY GOES OFF..."})
                If objGlobalStatic.PLCBYPASS.Trim.ToUpper.Contains("NO") Then
                    WriteOPCData("0", opcVoltageSelection, "VolatageSelection")
                    WriteOPCData("0", opcignitionOn, "IgnitionSelection")
                    System.Threading.Thread.Sleep(1000)
                    Application.DoEvents()
                    For i As Integer = 0 To 10
                        InsertDataIntoListView("FLASHING DONE,WAIT UNTILL MAIN RELAY GOES OFF", "OK")
                        System.Threading.Thread.Sleep(500)
                        Application.DoEvents()
                        InsertDataIntoListView("FLASHING DONE,WAIT UNTILL MAIN RELAY GOES OFF", "NOK")
                        System.Threading.Thread.Sleep(500)
                        Application.DoEvents()
                    Next
                Else
                    For i As Integer = 0 To 10
                        InsertDataIntoListView("FLASHING DONE,PLEASE TURN OFF THE IGNITION & WAIT UNTILL MAIN RELAY GOES OFF", "OK")
                        System.Threading.Thread.Sleep(500)
                        Application.DoEvents()
                        InsertDataIntoListView("FLASHING DONE,PLEASE TURN OFF THE IGNITION & WAIT UNTILL MAIN RELAY GOES OFF", "NOK")
                        System.Threading.Thread.Sleep(500)
                        Application.DoEvents()
                    Next
                End If
            Else
                InsertDataIntoListView("ECU FLASHING PROCESS FAILED", "NOK")
                System.Threading.Thread.Sleep(1000)
                Application.DoEvents()
            End If
        Catch ex As Exception
            WriteLog(ex.Message)
        Finally
            WriteLog("FLASHING CYCLE DONE..!")
            CloseApplicationAndStart()
        End Try
    End Sub

#End Region

#Region "Flashing Region"

    Public Function funSelectECUandstartRoutine() As Boolean
        Try
            Dim strEMSHWNO As String = ""

            WriteLog("" & DateTime.Now & "*****" & "START FLASHING" & "*********************")



            ''''Hardware Detection logic added for EMS Bosch Bifuel
            Dim EMScount As Integer = 0


            'WriteOPCData("1", opcignitionOn, "IgnitionSelection")
            'System.Threading.Thread.Sleep(1000)
            'Application.DoEvents()








            ECUID = 1

            Dim ObjEMS As Object
            Select Case ECUID
                Case 1
                    ''DCM 3.2 Flashing
                    ObjEMS = New cls_TCM(Me)
                    If ObjEMS.funStartEMS_Flashing(objSqlDS) Then
                        Return True
                    End If


                Case Else
                    ''default case
                    WriteLog("SELECT PROPER ECU TYPE AND PRESS 'OK'")
                    Me.Invoke(New delLabelChange(AddressOf labelChange), New Object() {lblopmsg, "SELECT PROPER ECU TYPE AND PRESS 'OK'"})
                    Me.Invoke(New delLabelChange(AddressOf labelChange), New Object() {lblerrmsg, "WRONG ECU TYPE SELECTED"})
                    btnOK.Visible = True
                    btnOK.Enabled = True
                    btnflash.Enabled = False
                    Return False
            End Select
        Catch ex As Exception
            WriteLog("Errror in btnFlash event " & ex.Message)
            Me.Invoke(New delLabelChange(AddressOf labelChange), New Object() {lblerrmsg, "FLASHING FAILED "})
            Me.Invoke(New delLabelChange(AddressOf labelChange), New Object() {lblopmsg, "FLASHING FAILED,TURN IGNITION SWITCH OFF,DISCONNECT ECU,PRESS 'OK'"})
            Return False
        Finally

        End Try
        WriteLog("*****************" & DateTime.Now & "*****" & "END FLASHING" & "*********************")
        Return False
    End Function

#End Region

#Region "Disable Controls"

    Public Sub Disable()
        Me.txtPartNumber.Enabled = False
        'Me.cmbLastFive.Enabled = False
        Me.grpKeyboard.Enabled = False
        Me.btnflash.Enabled = False
        Me.btnOK.Enabled = False
    End Sub

#End Region

    '#Region "Printing Function"

    Private Sub PrintDocument()
        Dim pd As New PrintDocument()
        AddHandler pd.PrintPage, AddressOf Me.Pd_PrintPage

        ' You can set the printer name if you have a specific label printer
        ' For example: pd.PrinterSettings.PrinterName = "Zebra LP 2844"
        ' If not set, it will use the default printer.

        Try
            pd.PrinterSettings.PrinterName = "Microsoft Print to PDF"
            pd.Print()
        Catch ex As Exception
            MessageBox.Show("An error occurred during printing: " & ex.Message, "Printing Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Pd_PrintPage(sender As Object, e As PrintPageEventArgs)
        ' Define label dimensions in hundredths of an inch (1 inch = 100 hundredths)
        ' So, 3 inches by 1 inch = 300 by 100 hundredths
        Const LABEL_WIDTH_PX As Integer = 300 ' Equivalent to 3 inches at 100 DPI (approximate)
        Const LABEL_HEIGHT_PX As Integer = 100 ' Equivalent to 1 inch at 100 DPI (approximate)

        ' Get the Graphics object for drawing
        Dim g As Graphics = e.Graphics

        ' --- Draw the Border ---
        ' The PrintPageEventArgs.MarginBounds gives you the printable area.
        ' We'll draw our label within this. For a 3x1 inch label, we'll draw it at the top-left.
        Dim labelRect As New Rectangle(e.MarginBounds.Left, e.MarginBounds.Top, LABEL_WIDTH_PX, LABEL_HEIGHT_PX)
        g.DrawRectangle(Pens.Black, labelRect)


        ' --- Barcode Generation ---
        Dim barcodeText As String = objGlobalStatic.PartNumber ' The data to encode in the barcode
        Dim barcodeWriter As New BarcodeWriter()
        barcodeWriter.Format = BarcodeFormat.QR_CODE ' Or EAN_13, QR_CODE, etc.
        barcodeWriter.Options = New EncodingOptions With {.Width = 50, .Height = 50, .Margin = 0} ' Adjust size within label

        Dim barcodeBitmap As Bitmap = Nothing
        Try
            barcodeBitmap = barcodeWriter.Write(barcodeText)
        Catch ex As Exception
            MessageBox.Show("Error generating barcode: " & ex.Message, "Barcode Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.HasMorePages = False ' Stop printing if barcode generation fails
            Return
        End Try

        ' --- Draw Barcode ---
        ' Center the barcode horizontally within the label, and position it vertically.
        Dim barcodeX As Integer = labelRect.X + 5
        Dim barcodeY As Integer = labelRect.Y + 20 ' Adjust vertical position as needed
        g.DrawImage(barcodeBitmap, barcodeX, barcodeY)

        ' --- Draw Title ---
        Dim titleText As String = "TCM FLASH STN-PNE"
        Dim titleFont As New Font("Arial", 10, FontStyle.Bold)
        Dim titleBrush As New SolidBrush(Color.Black)
        Dim titleSize As SizeF = g.MeasureString(titleText, titleFont)
        Dim titleX As Integer = labelRect.X + 5
        Dim titleY As Integer = labelRect.Y + 5 ' Position at the top, just inside the border
        g.DrawString(titleText, titleFont, titleBrush, titleX, titleY)

        ' --- Draw Title ---
        Dim Result As String = objGlobalStatic.FlashResult
        Dim ResultFont As New Font("Arial", 30, FontStyle.Bold)
        Dim ResultBrush As New SolidBrush(Color.Black)
        Dim ResultSize As SizeF = g.MeasureString(titleText, titleFont)
        Dim ResultX As Integer = labelRect.X + 60
        Dim ResultY As Integer = labelRect.Y + 23 ' Position at the top, just inside the border
        g.DrawString(Result, ResultFont, ResultBrush, ResultX, ResultY)



        ' --- Draw PartNumber  ---
        Dim PartNumber As String = "PART NO"
        Dim PartnoFont As New Font("Arial", 10, FontStyle.Bold)
        Dim PartnoBrush As New SolidBrush(Color.Black)
        Dim PartnoSize As SizeF = g.MeasureString(titleText, titleFont)
        Dim PartnoX As Integer = labelRect.X + 5
        Dim PartnoY As Integer = labelRect.Y + 69 ' Position at the top, just inside the border
        g.DrawString(PartNumber, PartnoFont, PartnoBrush, PartnoX, PartnoY)

        ' --- Draw SRNumber  ---
        Dim SRNumber As String = "SR.NO"
        Dim SRNOSize As SizeF = g.MeasureString(SRNumber, PartnoFont)
        Dim SRnoX As Integer = (labelRect.X + labelRect.Width) - (SRNOSize.Width + 50)
        Dim SRnoY As Integer = labelRect.Y + 69 ' Position at the top, just inside the border
        g.DrawString(SRNumber, PartnoFont, PartnoBrush, SRnoX, SRnoY)

        ' --- Draw SRNumber  ---
        Dim SRNumberValue As String = objGlobalStatic.SrNumber
        Dim SRNOValueSize As SizeF = g.MeasureString(SRNumberValue, PartnoFont)
        Dim SRValuenoX As Integer = (labelRect.X + labelRect.Width) - (SRNOSize.Width + 50)
        Dim SRVsluenoY As Integer = labelRect.Y + 83 ' Position at the top, just inside the border
        g.DrawString(SRNumberValue, PartnoFont, PartnoBrush, SRValuenoX, SRVsluenoY)


        ' --- Draw PartNumber value ---
        Dim txtPartNumber As String = objGlobalStatic.PartNumber
        Dim txtPartnoX As Integer = labelRect.X + 5
        Dim txtPartnoY As Integer = labelRect.Y + 83 ' Position at the top, just inside the border
        g.DrawString(txtPartNumber, PartnoFont, PartnoBrush, txtPartnoX, txtPartnoY)

        ' --- Draw Date/Time ---
        Dim dateTimeText As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        Dim dateTimeFont As New Font("Arial", 8, FontStyle.Bold)
        Dim dateTimeBrush As New SolidBrush(Color.Black)
        Dim dateTimeSize As SizeF = g.MeasureString(dateTimeText, dateTimeFont)
        Dim dateTimeX As Integer = (labelRect.X + labelRect.Width) - dateTimeSize.Width
        Dim dateTimeY As Integer = labelRect.Y + 5 ' Position at the bottom
        g.DrawString(dateTimeText, dateTimeFont, dateTimeBrush, dateTimeX, dateTimeY)

        ' No more pages to print
        e.HasMorePages = False

        ' Dispose of the barcode bitmap after use
        If Not barcodeBitmap Is Nothing Then
            barcodeBitmap.Dispose()
        End If
    End Sub
    '#End Region



#Region "PLC RELATED FUNCTION"
#Region "fnc initalizeOPCServer "
    Private Function initalizeOPCServer() As Boolean
            Try
                If (IsNothing(m_OpcDaServer)) Then
                    m_OpcDaServer = New TsOpcNet.TsCDaServer
                End If
                '' '' ''new added for rslinx activation issue
                ' ''System.Threading.Thread.Sleep(4000)
                ' ''If m_OpcDaServer.IsConnected = False Then
                ' ''    If File.Exists("C:\Program Files\Rockwell Software\RSLinx\RSLinx.exe") = True Then
                ' ''        Shell("C:\Program Files\Rockwell Software\RSLinx\RSLinx.exe")
                ' ''    Else
                ' ''        Shell("C:\Program Files (x86)\Rockwell Software\RSLinx\RSLinx.exe")
                ' ''    End If
                ' ''    m_OpcDaServer.Connect(System.Configuration.ConfigurationManager.AppSettings("OPCSERVER"), New TsOpcNet.TsOpcComputerInfo("LOCALHOST"))
                ' ''End If
                ' '' ''''''''''''''''''''
                If (Not m_OpcDaServer.IsConnected) Then
                    m_OpcDaServer.Connect(System.Configuration.ConfigurationManager.AppSettings("OPCSERVER"), New TsOpcNet.TsOpcComputerInfo("LOCALHOST"))
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Return False
            End Try
        End Function
#End Region

#Region " m_OpcDaServer_ServerShutdown "
        Private Sub m_OpcDaServer_ServerShutdown(ByVal reason As String)
            Static intAttempt As Int16 = 1
            Try
                InsertDataIntoListView("PRESS FAULT RESET BUTTON", "OK")
                m_OpcDaServer = Nothing
                While intAttempt < 6
                    InsertDataIntoListView("UNABLE TO WRITE LOG FILE." & "[ATTEMPT COUNT=" & intAttempt & "]", "NOK")

                    Shell("C:\Program Files (x86)\Rockwell Software\RSLinx\RSLinx.exe")

                    Me.Cursor = Cursors.WaitCursor      'SET WAIT CURSOR
                    System.Threading.Thread.Sleep(20000)
                    Application.DoEvents()
                    InsertDataIntoListView("INITALIZING OPC SERVER..IN PROGRESS.....", "OK") 'DISPLAYING MESSAGE OF INITALIZING OPC SERVER
                    If (Not initalizeOPCServer()) Then  'FUNCTION CALL TO INITALIZE OPC SERVER
                        InsertDataIntoListView("INITALIZATION FAILURE", "NOK")
                        intAttempt = intAttempt + 1
                    Else
                        InsertDataIntoListView("", "NOK") 'DISPLAYING BLANK MESSAGE
                        'ADD OPC GROUP ALONG WITH ITEMS

                        'addOPCGroupItem(strTopicName & "PLC_TESTRIG_READY", 101, "opcGroup_PLC_TESTRIG_READY", 1000, opcGroup_PLC_TEST_RIG_READY, AddressOf opcGroup_PLC_TEST_RIG_READY_DataChangeHandler)
                        Exit While
                    End If
                End While
            Catch ex As Exception
                If (intAttempt = 5) Then
                    InsertDataIntoListView("PLEASE RESTART THE TESTBED APPLICATION", "NOK")
                    InsertDataIntoListView("PLEASE RESTART THE TESTBED APPLICATION", "NOK")
                    Exit Sub
                Else
                    intAttempt = intAttempt + 1
                    m_OpcDaServer_ServerShutdown("[ATTEMPT COUNT=" & intAttempt & "]")
                End If
            Finally
                Me.Cursor = Cursors.Default         'SET DEFAULT CURSOR
            End Try
        End Sub
#End Region

#Region " addOPCGroupItem "
        Private Sub addOPCGroupItem(ByVal strItemTag As String, ByVal intClientHandle As String, ByVal strGroup_Name As String, ByVal intUpdateRateMiliSec As Integer, ByRef objGoup As TsOpcNet.TsCDaSubscription, ByRef m_MyDataCallback As TsOpcNet.TsCDaDataChangedHandler)
            Try
                ''#ADD GROUP TO SUBSCRIPTION--------------------------------------------------------------------
                Dim groupState As TsOpcNet.TsCDaSubscriptionState = New TsOpcNet.TsCDaSubscriptionState
                groupState.Name = strGroup_Name
                groupState.UpdateRate = intUpdateRateMiliSec
                objGoup = m_OpcDaServer.CreateSubscription(groupState)
                AddHandler objGoup.DataChanged, m_MyDataCallback

                'ADD ITEM TO SUBSTRIPTION -----------------------------------------------------------------------
                Dim itemDefs As TsOpcNet.TsCDaItem() = Array.CreateInstance(GetType(TsOpcNet.TsCDaItem), 1)
                Dim resItemResult As TsOpcNet.TsCDaItemResult()
                itemDefs(0) = New TsOpcNet.TsCDaItem
                itemDefs(0).ItemName = strItemTag
                itemDefs(0).ClientHandle = intClientHandle   'ITEM HANDLE
                resItemResult = objGoup.AddItems(itemDefs)
                If Not resItemResult(0).Result.IsSuccess Then
                    'showResult(3, "[addOPCGroupItem]:" & Mid(strItemTag, InStr(strItemTag, ".") + 1), lblError)
                Else
                    Select Case intClientHandle
                        Case 114    'FOR PC_DYNO_TEST_START TAG 
                            opcItem_TestStart = resItemResult
                        Case 115    'FOR PC_DYNO_TEST_OVER TAG 
                            opcItem_TestOver = resItemResult
                        Case 116    'FOR PC_REREAD_ERROR TAG
                            opcItem_ReRead_Error = resItemResult
                    End Select
                End If
            Catch ex As Exception
                'showResult(4, "[addOPCGroupItem]:" & strGroup_Name & "/" & Mid(strItemTag, InStr(strItemTag, ".") + 1), lblError)
            End Try
        End Sub

        Private Function Group_Item(ByVal strItemTag As String, ByVal intClientHandle As String, ByVal strGroup_Name As String, ByVal intUpdateRateMiliSec As Integer, ByRef objGroup As TsCDaSubscription) As Boolean
            Try
                Dim groupstate As TsCDaSubscriptionState = New TsCDaSubscriptionState
                groupstate.Name = strGroup_Name
                groupstate.UpdateRate = intUpdateRateMiliSec
                objGroup = m_OpcDaServer.CreateSubscription(groupstate)
                Dim res As TsCDaItemResult()
                Dim itemDefs(0) As TsCDaItem
                itemDefs(0) = New TsCDaItem
                itemDefs(0).ItemName = strItemTag
                itemDefs(0).ClientHandle = intClientHandle                      ' 100 = item handle        
                res = objGroup.AddItems(itemDefs)
                If res(0).Result.IsSuccess Then
                    If res(0).Result.IsOk Then      ' Note: Since this sample adds only one item it's required that AddItems()
                        opcGenericRead = res
                        Return True                 ' succeeds for all specified items (in this case only one).
                    Else
                        Throw New System.Exception("AddItems() method failed: " + res(0).Result.Description())
                        WriteLog("AddItems() method failed: " + res(0).Result.Description())
                        Return False
                    End If
                Else
                    Throw New System.Exception("AddItems() method failed: " + res(0).Result.Description())
                    WriteLog("AddItems() method failed: " + res(0).Result.Description())
                    Return False
                End If
            Catch exe As TsOpcResultException
                WriteLog("Group Addition Exception! " & exe.Message)
                Return False
            Catch ex As Exception
                WriteLog("Group Addition Exception! " & ex.Message)
                Return False
            End Try
        End Function

        Public Sub WriteOPCData(ByVal ItemValue As String, ByVal grpName As TsCDaSubscription, ByVal ItemName As String)
            Try
                Dim res As TsOpcItemResult()
                Dim WriteItem(0) As TsCDaItemValue
                WriteItem(0) = New TsCDaItemValue
                WriteItem(0).ItemName = ItemName
                WriteItem(0).ServerHandle = opcGenericRead(0).ServerHandle
                'txtWriteOPC.Text = data
                WriteItem(0).Value = ItemValue
                res = grpName.Write(WriteItem)
                If res(0).Result.IsError Then
                    WriteLog("Write operation failed: " + res(0).Result.Description)
                    Control.CheckForIllegalCrossThreadCalls = False
                    Return
                Else
                    If Not res(0).Result.IsOk Then
                        Control.CheckForIllegalCrossThreadCalls = False
                        WriteLog("Cannot write value : " + res(0).Result.Description)
                    Else
                        'Dim uDefaultValue As String = Me.txtWriteOPC.Text
                        'Me.txtWriteOPC.Text = uDefaultValue ' + 1
                        WriteLog("WriteOPCData:" & grpName.Name & ":" & ItemValue)
                    End If
                End If
            Catch ex As TsOpcResultException
                WriteLog("Write OPC Data Exception!" & ex.Message)
            Catch ex As Exception
                WriteLog("Write OPC Data Exception!" & ex.Message)
            End Try
        End Sub

#End Region

#Region "PC-PLC Tags"
#Region "opcGroup_PLC_N70_DataChangeHandler"
        Public Sub opcGroup_PLC_N70_DataChangeHandler(ByVal subscriptionHandle As Object, ByVal requestHandle As Object, ByVal values As TsCDaItemValueResult())
            If (values(0).Result.IsSuccess) Then
                If (values(0).Value = 1) Then

                ElseIf (values(0).Value = 2) Then

                End If
            End If

    End Sub
#End Region

#Region "opcGroup_PLC_N71_DataChangeHandler"
        Public Sub opcGroup_PLC_N71_DataChangeHandler(ByVal subscriptionHandle As Object, ByVal requestHandle As Object, ByVal values As TsCDaItemValueResult())
            If (values(0).Result.IsSuccess) Then
                If (values(0).Value = 1) Then

                ElseIf (values(0).Value = 2) Then

                End If
            End If

    End Sub

#End Region
#Region "opcGroup_PC_N72_DataChangeHandler"
        Public Sub opcGroup_PC_N72_DataChangeHandler(ByVal subscriptionHandle As Object, ByVal requestHandle As Object, ByVal values As TsCDaItemValueResult())
            If (values(0).Result.IsSuccess) Then
                If (values(0).Value = 1) Then

                End If
            End If

    End Sub
#End Region

#End Region
#End Region


#Region "CODE TO DETECT PREVIOUS INSTANCE OF APPLICATION"
    'Windows API
    Declare Function OpenIcon Lib "user32" (ByVal hwnd As Long) As Long
    Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As Long) As Long
    ' Function to check and see if an instance of the application is already open

    'Public Shared Function IsAlreadyOpen(ByVal sApp As String) As Boolean
    '    'Check running processes to see if application is already running
    '    Dim pProcess As Process() = System.Diagnostics.Process.GetProcessesByName(sApp)
    '    If pProcess.Length > 1 Then  'If > 1 then its already running
    '        Return True
    '    Else  'Not running
    '        Return False
    '    End If
    'End Function
    Public Shared Function IsAlreadyOpen(ByVal sApp As String) As Boolean
        'Check running processes to see if application is already running
        Dim pProcess As Process() = System.Diagnostics.Process.GetProcessesByName(sApp)
        If pProcess.Length > 1 Then  'If > 1 then its already running
            MessageBox.Show("Application Already in running condition,unable to open same application two times.")
            Try
                GC.Collect()
                Dim allProcesses As System.Diagnostics.Process() = System.Diagnostics.Process.GetProcesses()
                For iii As Integer = 0 To allProcesses.Length - 1
                    If DirectCast(allProcesses(iii), System.Diagnostics.Process).ProcessName.Contains(sApp) Then
                        DirectCast(allProcesses(iii), System.Diagnostics.Process).Kill()
                    End If
                Next

            Catch ex As Exception
            Finally
                GC.Collect()
            End Try
        Else  'Not running
            Return False
        End If
    End Function
    ' Procedure to activate current instance 
    ' if user is trying to open a 2nd instance of application

    Public Shared Sub ActivatePrevInstance()
        Dim lngPrevHndl As Long
        Dim lngResult As Long


        Dim oProcess As New Process 'Variable to hold individual Process
        lngPrevHndl = oProcess.MainWindowHandle.ToInt32()
        If lngPrevHndl = 0 Then
            Exit Sub 'if No previous instance found exit the the procedure
        Else
            ''If found
            lngResult = OpenIcon(lngPrevHndl) 'Restore the program.
            lngResult = SetForegroundWindow(lngPrevHndl) 'Activate the application.

            End 'End the current instance of the application.
        End If
    End Sub
#End Region


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        '' Application.Restart()
        CloseApplicationAndStart()
    End Sub


End Class
