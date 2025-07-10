
Imports System.IO
Imports System.Xml
Imports Reprogramming_Dll
Imports Reprogramming_Dll.DLLConstants
Imports Reprogramming_Dll.DLLConstants.CommInterface
Imports CommInterface.ECUCOMM
Imports CommInterface.EOLCommErrors.EOLCommError
Imports CommInterface

Partial Public Class cls_TCM

#Region "Varaible Declaration"

    Dim objfrmmaster As frmMaster
    Dim blnBeforFlashIdentification As Boolean = True
    Private WithEvents mmReproEx As New ReprogrammerEx
    Private blnWriteLog As Boolean = True
    Private WithEvents objDllrepro As Reprogramming_Dll.ECUFunctions
    Private objECUCom As New ECUCOMM
    Private dtTestRoutine As DataTable
    Private Status As String = ""
    Private PRE_FALSHED As Boolean = False
    Private blnHWNOMismatch As Boolean = False
    Public blnReadECUDataAfterFlash As Boolean = False
    Dim strResponse As String

#End Region

    Public Sub New(ByRef FrmMaster As Form)
        objfrmmaster = FrmMaster
    End Sub

    Private Sub InitReflashingProgress(ByVal Maximum As Integer) Handles mmReproEx.InitProgressBar
        objfrmmaster.FlashingProgressBar.Minimum = 0
        objfrmmaster.FlashingProgressBar.Maximum = Maximum
    End Sub

    Private Sub ShowReflashingProgress(ByVal Value As Integer) Handles mmReproEx.UpdateProgressBar
        objfrmmaster.FlashingProgressBar.Value = Value
    End Sub

    Public Sub AddDebugString(ByVal strDebugData As String) Handles mmReproEx.DebugString
        If blnWriteLog = True Then
            objfrmmaster.WriteLog(strDebugData & " TCM")
        End If
    End Sub

    Public Sub ChangeLable(ByVal str As String, ByVal Result As Boolean, ByVal intplace As Integer) Handles mmReproEx.UpdateLable
        objfrmmaster.ChangeLableValue(str, Result, intplace)
    End Sub

    Public Function funStartEMS_Flashing(ByRef objSqlDs As DataSet) As Boolean
        Try

            AddDebugString("************TCM Reprogramming Started... ******************")


            ' Dim strCalSWFilePath As String = ""
            Dim SupportFilePath() As String
            Dim SupportFileName() As String
            Dim Flash_File_Path As String = ""
            ''If objfrmmaster.objClsGetData.objEnum_Server_Design = clsGetData.Enum_Server_DesignData.NecM Then
            ''    strCalSWFilePath = Flash_File_Path & objSqlDs.Tables("File_Master").Rows(0)("DOMAIN_NAME") & "\" & objSqlDs.Tables("File_Master").Rows(0)("FILE_VERSION") & "\" & objSqlDs.Tables("File_Master").Rows(0).Item("FM_NAME")
            ''Else
            ''    ReDim SupportFileName(10)
            ''    ReDim SupportFilePath(10)
            ''    Dim SupportfileIndex As Integer = 0

            ''    If objSqlDs.Tables.Contains("SupFile_Master") = True Then
            ''        For i As Integer = 0 To objSqlDs.Tables("SupFile_Master").Rows.Count - 1
            ''            If objSqlDs.Tables("File_Master").Rows(0)("FMID").ToString() = objSqlDs.Tables("SupFile_Master").Rows(i)("FMID") Then
            ''                SupportFileName(SupportfileIndex) = objSqlDs.Tables("SupFile_Master").Rows(i)("Sup_File")
            ''                SupportFilePath(SupportfileIndex) = Flash_File_Path & objSqlDs.Tables("File_Master").Rows(0)("DOMAIN_NAME") & objfrmmaster.DomainTrail & "\" & objSqlDs.Tables("File_Master").Rows(0)("FILE_VERSION") & "\" & SupportFileName(SupportfileIndex)
            ''                SupportfileIndex = SupportfileIndex + 1
            ''            End If
            ''        Next
            ''    End If

            'Dim XMLFilePath As String = ""
            '    For i As Integer = 0 To SupportFileName.Length - 1
            '        If SupportFileName(i) = Nothing Then
            '            Exit For
            '        Else
            '            Select Case True
            '                Case SupportFileName(i).ToString.ToLower.Contains(".ulp")
            '                    strCalSWFilePath = SupportFilePath(i).ToString
            '                    Exit For
            '            End Select
            '        End If
            '    Next
            'End If

            objfrmmaster.InsertDataIntoListView("ECU FLASHING IN PROGRESS PLEASE WAIT....", "OK")

            If funFlashFile(objfrmmaster.objGlobalStatic.commaSeparatedFileNames, objfrmmaster.objGlobalStatic.commaSeparatedCRCFileNames) Then
                objfrmmaster.InsertDataIntoListView("ECU FLASH SUCCESSFULL", "OK")
                Application.DoEvents()
            Else
                objfrmmaster.InsertDataIntoListView("FLASHING FAILED", "NOK")
                Application.DoEvents()
                Return False
            End If

            ''''PLC Power Latch added
            Dim intCounter_IGNLatchTime As Integer = 20
            Dim intMin_IGNOnTime As Integer = 10
            Dim intIGN_DownTime As Integer = intCounter_IGNLatchTime
            Dim PowerLatchStart As DateTime = Nothing
            PowerLatchStart = DateTime.Now

            Try
                If frmMaster.objGlobalStatic.PLCBYPASS.Trim.ToUpper.Contains("NO") Then
                    '' frmMaster.WriteOPCData("0", frmMaster.opcVoltageSelection, "VolatageSelection")
                    frmMaster.WriteOPCData("0", frmMaster.opcignitionOn, "IgnitionSelection")
                    System.Threading.Thread.Sleep(1000)
                    objfrmmaster.InsertDataIntoListView("EMS ECU POWERLATCH PROCESS IN PROGRESS PLEASE WAIT.... ", "NOK")
                    Application.DoEvents()

                    ''If funCheckCommStatus() Then
                    ''    objfrmmaster.InsertDataIntoListView("EMS ECU POWERLATCH PROCESS DONE SUCCESSFULLY.... ", "OK")
                    ''Else
                    ''    objfrmmaster.InsertDataIntoListView("POWER-LATCH CYCLE FAILED.", "NOK")
                    ''    Return False
                    ''End If

                    Do While True
                        Dim intIGNdiff As Integer = DateDiff(DateInterval.Second, PowerLatchStart, DateTime.Now)
                        intIGNdiff = 20 - intIGNdiff

                        If intIGNdiff >= 5 Then
                            'frmIGN.Visible = True
                            'frmIGN.lblcountmsg.ForeColor = Color.Black
                            'frmIGN.lblcountmsg.Text = "EMS ECU POWER TURNED OFF CONDITION PLEASE WAIT FOR::"
                            'frmIGN.lblCounter.Visible = True
                            'frmIGN.lblCounter.ForeColor = Color.Black
                            'frmIGN.lblCounter.Text = intIGNdiff
                            ''  Application.DoEvents()
                        Else
                            'frmIGN.Visible = True
                            'frmIGN.lblCounter.ForeColor = Color.Red
                            'frmIGN.lblcountmsg.ForeColor = Color.Red
                            'frmIGN.lblcountmsg.Text = "EMS ECU POWER TURNED ON::"
                            'frmIGN.lblCounter.Text = intIGNdiff
                            '''  Application.DoEvents()

                        End If

                        If intIGNdiff <= 2 Then
                            'frmIGN.Visible = False
                            'frmIGN.Close()
                            Exit Do
                        End If
                        ''  System.Threading.Thread.Sleep(200)
                        'frmIGN.Refresh()
                    Loop
                    'frmIGN.Visible = False
                    'frmIGN.Close()

                    '' frmMaster.WriteOPCData("0", frmMaster.opcVoltageSelection, "VolatageSelection")
                    frmMaster.WriteOPCData("1", frmMaster.opcignitionOn, "IgnitionSelection")
                    System.Threading.Thread.Sleep(1000)
                    Application.DoEvents()

                    For i1 As Integer = 1 To 5
                        System.Threading.Thread.Sleep(950)
                        Application.DoEvents()
                    Next

                Else
                    ' ''User dependent powerlatch
                    Do While True
                        Dim intIGNdiff As Integer = DateDiff(DateInterval.Second, PowerLatchStart, DateTime.Now)
                        intIGNdiff = 20 - intIGNdiff

                        If intIGNdiff >= 7 Then
                            'frmIGN.Visible = True
                            'frmIGN.lblcountmsg.ForeColor = Color.Black
                            'frmIGN.lblcountmsg.Text = "PLEASE TURN OFF THE IGNITION SWITCH FOR:: "
                            'frmIGN.lblCounter.Visible = True
                            'frmIGN.lblCounter.ForeColor = Color.Black
                            'frmIGN.lblCounter.Text = intIGNdiff

                            ''  Application.DoEvents()
                        Else
                            'frmIGN.Visible = True
                            'frmIGN.lblCounter.ForeColor = Color.Red
                            'frmIGN.lblcountmsg.ForeColor = Color.Red
                            'frmIGN.lblcountmsg.Text = " PLEASE TURN ON THE IGNITION SWITCH IN:: "
                            'frmIGN.lblCounter.Text = intIGNdiff
                            ''  Application.DoEvents()

                        End If

                        If intIGNdiff <= 2 Then
                            'frmIGN.Visible = False
                            'frmIGN.Close()
                            Exit Do
                        End If
                        ''  System.Threading.Thread.Sleep(200)
                        'frmIGN.Refresh()
                    Loop
                    'frmIGN.Visible = False
                    'frmIGN.Close()
                End If

            Catch ex As Exception
                AddDebugString(ex.Message)
                AddDebugString("Powerlatch Process Exception")
            End Try
            '''''''

            objfrmmaster.InsertDataIntoListView("EMS ECU STARTING COMMUNICATION IN PROGRESS,PLEASE WAIT.", "OK")
            If funInit() Then
                objfrmmaster.InsertDataIntoListView("EMS ECU INIT SUCCESSFUL", "OK")
                System.Threading.Thread.Sleep(1000)
                Application.DoEvents()
            Else
                objfrmmaster.InsertDataIntoListView("EMS ECU INIT FAILED,CHECK THE CONNECTION SETUP FOR THE ECU.", "NOK")
                System.Threading.Thread.Sleep(1000)
                Application.DoEvents()
                Return False
            End If

            objfrmmaster.InsertDataIntoListView("EMS ECU IDENTIFICATION DATA READING IN PROGRESS,PLEASE WAIT.", "OK")
            'If funReadECUIdentification(objSqlDs.Tables("File_Master")) Then
            '    objfrmmaster.InsertDataIntoListView("ECU DATA MATCHED", "OK")
            '    System.Threading.Thread.Sleep(1000)
            '    Application.DoEvents()
            'Else
            '    objfrmmaster.InsertDataIntoListView("ECU DATA MISMATCH", "NOK")
            '    Application.DoEvents()
            '    Dim result As DialogResult = MessageBox.Show("ECU FLASHED DATA MISMATCH. " &
            '                        "ECU REFLASHING REQUIRED. " &
            '                        "RESET APPLICATION. ",
            '                        "CRITICAL WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2, MessageBoxOptions.RightAlign, False)
            '    If result = DialogResult.OK Then
            '        objfrmmaster.InsertDataIntoListView("ECU REFLASHING PROCESS ABORT BY USER", "")
            '        Return False
            '        'Else
            '        '    objfrmmaster.InsertDataIntoListView("ECU FLASHED WITH CORRECT DATA FOR THIS SELECTION. USER HAD SELECTED - YES", "NOK")
            '        '    System.Threading.Thread.Sleep(1000)
            '        '    Application.DoEvents()
            '    End If
            '    Return False
            'End If

            objfrmmaster.InsertDataIntoListView("ECU FLASHING SUCCESSFUL....", "OK")
            System.Threading.Thread.Sleep(1000)
            Application.DoEvents()


            Return True
        Catch ex As Exception
            AddDebugString(ex.Message)
            Return False
        Finally
            objfrmmaster.InsertDataIntoListView("EMS ECU STOP COMMUNICATION IN PROGRESS", "OK")
            Application.DoEvents()
            System.Threading.Thread.Sleep(1000)
            Application.DoEvents()
            If funExit() Then
                objfrmmaster.InsertDataIntoListView("EMS ECU STOP COMMUNICATION SUCCESSFUL", "OK")
                Application.DoEvents()
                System.Threading.Thread.Sleep(1000)
                Application.DoEvents()
            Else
                objfrmmaster.InsertDataIntoListView("EMS ECU STOP COMMUNICATION FAILED", "NOK")
                Application.DoEvents()
                System.Threading.Thread.Sleep(1000)
                Application.DoEvents()
            End If
        End Try
    End Function

    Private Function funFlashFile(ByVal mReproFileName As String, ByVal mReproCRCFileName As String) As Boolean
        Try
            'If funExit() Then

            'End If
            AddDebugString("CALIBRATION FILE PATH::" & mReproFileName)

            Dim startFlash, endFlash As DateTime
            Dim mVciType As CommInterface.CONVERTER_TYPE = J2534_UDS_ON_CAN
            Dim mEcuAdd As String = "7E9"
            Dim mTesterAdd As String = "7E1"
            Dim mNewBaud As String = "500000"
            Dim mToolSign As String = " Man"
            Dim mProgDate As String = (Today.Year).ToString("0000") & Today.Month.ToString("00") & Today.Day.ToString("00")
            Dim mKLineInterface As String = "0"
            Dim mKLineDevice As String = "0"
            Dim mProgShopCode As String = " Man"
            Dim mnuShowData As Boolean = False

            Dim ErrorCode As Long = 0

            Dim boolFlash As Boolean = False

            Dim timeToFlash As Integer = 0
            startFlash = DateTime.Now

            objfrmmaster.FlashingProgressBar.Visible = True
            'objfrmmaster.picWait.Visible = True
            mmReproEx.DisplayMessages = False

            mmReproEx = New ReprogrammerDiangoComm
            mmReproEx.DisplayMessages = False
            boolFlash = CType(mmReproEx, ReprogrammerDiangoComm).InitReflashing(mKLineDevice,
                                                            mReproFileName,
                                                            Convert.ToInt64(mEcuAdd, 16),
                                                            Convert.ToInt64(mTesterAdd, 16),
                                                            mNewBaud,
                                                            mToolSign,
                                                            mProgDate,
                                                            mProgShopCode,
                                                            mVciType,
                                                            mnuShowData,
                                                            ErrorCode, mReproCRCFileName)


            If boolFlash = True Then
                Application.DoEvents()
                boolFlash = mmReproEx.StartReflashing(ErrorCode, 1)
                If boolFlash = True Then
                    AddDebugString("EMS Flashing Successful")
                    endFlash = DateTime.Now
                    timeToFlash = DateDiff(DateInterval.Second, startFlash, endFlash)
                Else
                    AddDebugString("EMS Flashing Failed")
                    Return False
                End If
            Else
                AddDebugString("EMS Flashing Failed")
                Return False
            End If
            Return True
        Catch ex As Exception
            AddDebugString(ex.Message)
            Return False
        Finally
            blnWriteLog = True
            objfrmmaster.FlashingProgressBar.Visible = False
            'objfrmmaster.picWait.Visible = False
        End Try
    End Function

    Protected Function funInit() As Boolean
        Try
            AddDebugString("*******STARTING ECU COMMUNICATION********************************************************")
            Dim objresult As String = ""

            objDllrepro = Nothing

            Dim CommInitCounter As Integer = 0
            For CommInitCounter = 0 To 1

                'If objDllrepro Is Nothing Then
                '    objDllrepro = DLLInit()
                'End If

                'If objDllInhouse._IsCommunicationStarted(4000) Then
                '    '' If objDllInhouse._GetSecurityAccess(Status, ECUFunctions.EnumSecurityAccessType.type_0_1) Then
                '    AddDebugString("INIT SUCCESSFUL")
                '    Exit For

                '    ''  Else
                '    '' AddDebugString("SECURITY ACCESS FAILED")
                '    ''  End If
                'Else
                '    AddDebugString("INIT FAILED")
                'End If

                If CommInitCounter = 3 Then
                    Return False
                End If
            Next
            Return True
        Catch ex As Exception
            AddDebugString(ex.Message)
            Return False
        End Try
    End Function

    'Protected Function funReadECUIdentification(ByRef dtECUParam As DataTable) As Boolean
    '    Try
    '        Dim dtECUIden As New DataTable
    '        Dim ECU_TMLPART_CONTAINER_NUMBER As String = ""
    '        Dim ECU_TMLHARDWARE_NUMBER As String = ""
    '        Dim ECU_CAL_ID As String = ""
    '        Dim ECU_TMLSOFTWARE_NUMBER As String = ""
    '        Dim ECU_TMLSOFTWARE_VERSION As String = ""
    '        Dim ECU_TMLHARDWARE_VERSION As String = ""
    '        Dim ECU_TMLPART_CONTAINER_VERSION As String = ""
    '        Dim strList As New List(Of String)
    '        strList.Add(49)

    '        ''  If objDllInhouse._GetAllIdentificationData(dtECUIden) = False Then
    '        If objDllrepro._GetSingleVehicleParameterData(strList, dtECUIden) = False Then
    '            AddDebugString("READ ECU IDENTIFICATION DATA FAILED.")
    '            Return False
    '        Else
    '            ECU_TMLHARDWARE_NUMBER = dtECUIden.Rows(0).Item("Value").ToString()
    '            ECU_TMLHARDWARE_NUMBER = dtECUIden.Rows(0).Item("Value").ToString().Substring(0, 12)
    '            ECU_TMLHARDWARE_VERSION = dtECUIden.Rows(0).Item("Value").ToString().Substring(13, 2)
    '            AddDebugString("ECU_TMLHARDWARE_NUMBER READING SUCCESSFUL.")
    '        End If
    '        strList.Add(48)
    '        If objDllInhouse._GetSingleVehicleParameterData(strList, dtECUIden) = False Then
    '            AddDebugString("READ ECU IDENTIFICATION DATA FAILED.")
    '            Return False
    '        Else
    '            ECU_TMLPART_CONTAINER_NUMBER = dtECUIden.Rows(0).Item("Value").ToString().Substring(0, 12)
    '            ECU_TMLPART_CONTAINER_VERSION = dtECUIden.Rows(0).Item("Value").ToString().Substring(13, 2)
    '            AddDebugString("ECU_TMLPART_CONTAINER_NUMBER READING SUCCESSFUL.")
    '        End If
    '        strList.Add(47)
    '        If objDllInhouse._GetSingleVehicleParameterData(strList, dtECUIden) = False Then
    '            AddDebugString("READ ECU IDENTIFICATION DATA FAILED.")
    '            Return False
    '        Else
    '            ECU_TMLSOFTWARE_NUMBER = dtECUIden.Rows(0).Item("Value").ToString().Substring(0, 12)
    '            ECU_TMLSOFTWARE_VERSION = dtECUIden.Rows(0).Item("Value").ToString().Substring(13, 2)
    '            AddDebugString("ECU_TMLSOFTWARE_NUMBER READING SUCCESSFUL.")
    '        End If

    '        If funECUIden_Compare1(dtECUParam, ECU_TMLSOFTWARE_NUMBER, ECU_CAL_ID, ECU_TMLHARDWARE_NUMBER, ECU_TMLPART_CONTAINER_NUMBER, ECU_TMLHARDWARE_VERSION, ECU_TMLPART_CONTAINER_VERSION, ECU_TMLSOFTWARE_VERSION) = False Then
    '            Return False
    '        End If

    '        Return True
    '    Catch ex As Exception
    '        AddDebugString(ex.Message)
    '        Return False
    '    End Try
    'End Function

    'Private Function funECUIden_Compare1(ByRef dtECUidenServer As DataTable, ByVal ECU_TMLSOFTWARE_NUMBER As String, ByVal ECU_CAL_ID As String, ByVal ECU_TMLHARDWARE_NUMBER As String, ByVal ECU_TMLPART_CONTAINER_NUMBER As String, ByVal ECU_TMLHARDWARE_VERSION As String, ByVal ECU_TMLPART_CONTAINER_VERSION As String, ByVal ECU_TMLSOFTWARE_VERSION As String) As Boolean
    '    Try
    '        Dim SERVER_TMLSOFTWARE_NUMBER As String = ""
    '        Dim SERVER_CAL_ID As String = ""
    '        Dim SERVER_TMLHARDWARE_NUMBER As String = ""
    '        Dim SERVER_TMLPART_CONTAINER_NUMBER As String = ""
    '        Dim SERVER_TMLSOFTWARE_VERSION As String = ""
    '        Dim SERVER_TMLHARDWARE_VERSION As String = ""
    '        Dim SERVER_TMLPART_CONTAINER_VERSION As String = ""


    '        Dim DataMatch As Boolean = True

    '        If dtECUidenServer Is Nothing OrElse dtECUidenServer.Rows.Count = 0 Then
    '            objfrmmaster.InsertDataIntoListView("ECU PARAMETER NOT AVAILABLE AGAINST SCAN VC NUMBER", "NOK")
    '            AddDebugString("ECU PARAMETER NOT AVAILABLE AGAINST SCAN VC NUMBER.")
    '            Return True
    '        End If


    '        For i As Integer = 0 To dtECUidenServer.Rows.Count - 1
    '            If (dtECUidenServer.Rows(i).Item("ECUTYPE").ToString = "EMS Delphi DCM 7.1") Then
    '                SERVER_TMLSOFTWARE_NUMBER = dtECUidenServer.Rows(i).Item("SW_NO").ToString
    '                SERVER_CAL_ID = dtECUidenServer.Rows(i).Item("F7")
    '                SERVER_TMLHARDWARE_NUMBER = dtECUidenServer.Rows(i).Item("HARDWARENO").ToString
    '                SERVER_TMLPART_CONTAINER_NUMBER = dtECUidenServer.Rows(i).Item("PART_NO").ToString
    '                SERVER_TMLSOFTWARE_VERSION = dtECUidenServer.Rows(i).Item("SW_VERSION").ToString
    '                SERVER_TMLHARDWARE_VERSION = dtECUidenServer.Rows(i).Item("HARDWAREVERSION").ToString
    '                SERVER_TMLPART_CONTAINER_VERSION = dtECUidenServer.Rows(i).Item("Part_No_Version").ToString
    '            End If

    '        Next

    '        If SERVER_TMLPART_CONTAINER_VERSION.Contains("_") Then
    '            Dim arrContainerver() As String = SERVER_TMLPART_CONTAINER_VERSION.Split("_")

    '            If arrContainerver(0).Length = 1 Then
    '                SERVER_TMLPART_CONTAINER_VERSION = 0 & arrContainerver(0)
    '            ElseIf arrContainerver(0).Length >= 2 Then
    '                SERVER_TMLPART_CONTAINER_VERSION = arrContainerver(0)
    '            End If
    '        Else
    '            SERVER_TMLPART_CONTAINER_VERSION = SERVER_TMLPART_CONTAINER_VERSION
    '        End If

    '        '' ECU_SW_Ver_Number_Server = ECU_Software_Number

    '        If SERVER_TMLSOFTWARE_VERSION.Contains("_") Then
    '            Dim arrContainerver() As String = SERVER_TMLSOFTWARE_VERSION.Split("_")
    '            If arrContainerver(0).Length = 1 Then
    '                SERVER_TMLSOFTWARE_VERSION = 0 & arrContainerver(0)
    '            ElseIf arrContainerver(0).Length >= 2 Then
    '                SERVER_TMLSOFTWARE_VERSION = arrContainerver(0)
    '            End If
    '        Else
    '            SERVER_TMLSOFTWARE_VERSION = SERVER_TMLSOFTWARE_VERSION
    '        End If


    '        If SERVER_TMLHARDWARE_VERSION.Contains("_") Then
    '            Dim arrContainerver() As String = SERVER_TMLHARDWARE_VERSION.Split("_")
    '            If arrContainerver(0).Length = 1 Then
    '                SERVER_TMLHARDWARE_VERSION = 0 & arrContainerver(0)
    '            ElseIf arrContainerver(0).Length >= 2 Then
    '                SERVER_TMLHARDWARE_VERSION = arrContainerver(0)
    '            End If
    '        Else
    '            SERVER_TMLHARDWARE_VERSION = SERVER_TMLHARDWARE_VERSION
    '        End If



    '        If ECU_TMLHARDWARE_NUMBER = "" Then

    '        Else
    '            If Val(SERVER_TMLHARDWARE_NUMBER.Trim()) = Val(ECU_TMLHARDWARE_NUMBER.Trim()) Then
    '                '' dtResult.Rows(intIndex)("HW_NO") = ECU_TMLHARDWARE_NUMBER.Trim()
    '                AddDebugString("ECU & SERVER HARDWARE NUMBER SAME")
    '                AddDebugString("ECU HARDWARE NUMBER: " & ECU_TMLHARDWARE_NUMBER & ".")
    '                AddDebugString("SERVER HARDWARE NUMBER: " & SERVER_TMLHARDWARE_NUMBER & ".")
    '            Else
    '                DataMatch = False
    '                AddDebugString(" BEFORE FLASH ECU DATA MISMATCH")
    '                AddDebugString(" BEFORE FLASH ECU HARDWARE NUMBER: " & ECU_TMLHARDWARE_NUMBER & ".")
    '                AddDebugString("SERVER HARDWARE NUMBER: " & SERVER_TMLHARDWARE_NUMBER & ".")
    '                If blnBeforFlashIdentification = False Then
    '                    AddDebugString("ECU DATA MISMATCH")
    '                    AddDebugString("ECU HARDWARE NUMBER: " & ECU_TMLHARDWARE_NUMBER & ".")
    '                    AddDebugString("SERVER HARDWARE NUMBER: " & SERVER_TMLHARDWARE_NUMBER & ".")
    '                    DataMatch = False
    '                    blnHWNOMismatch = True
    '                End If
    '            End If
    '        End If


    '        If ECU_TMLHARDWARE_VERSION = "" Then

    '        Else
    '            If SERVER_TMLHARDWARE_VERSION.Trim() = ECU_TMLHARDWARE_VERSION.Trim() Then
    '                '' dtResult.Rows(intIndex)("HW_NO") = ECU_TMLHARDWARE_NUMBER.Trim()
    '                AddDebugString("ECU & SERVER HARDWARE VERSION SAME")
    '                AddDebugString("ECU HARDWARE VERSION: " & ECU_TMLHARDWARE_VERSION & ".")
    '                AddDebugString("SERVER HARDWARE VERSION: " & SERVER_TMLHARDWARE_VERSION & ".")
    '            Else
    '                DataMatch = False
    '                AddDebugString(" BEFORE FLASH ECU DATA MISMATCH")
    '                AddDebugString(" BEFORE FLASH ECU HARDWARE VERSION: " & ECU_TMLHARDWARE_VERSION & ".")
    '                AddDebugString("SERVER HARDWARE VERSION: " & SERVER_TMLHARDWARE_VERSION & ".")
    '                If blnBeforFlashIdentification = False Then
    '                    AddDebugString("ECU DATA MISMATCH")
    '                    AddDebugString("ECU HARDWARE VERSION: " & ECU_TMLHARDWARE_VERSION & ".")
    '                    AddDebugString("SERVER HARDWARE VERSION: " & SERVER_TMLHARDWARE_VERSION & ".")
    '                    DataMatch = False
    '                End If
    '            End If
    '        End If

    '        If ECU_TMLPART_CONTAINER_NUMBER = "" Then

    '        Else
    '            If Val(SERVER_TMLPART_CONTAINER_NUMBER.Trim()) = Val(ECU_TMLPART_CONTAINER_NUMBER.Trim()) Then
    '                ''  dtResult.Rows(intIndex)("TML_PART_NO") = ECU_TMLPART_CONTAINER_NUMBER.Trim()
    '                AddDebugString("ECU & SERVER TMLPART NUMBER SAME")
    '                AddDebugString("ECU TMLPART NUMBER: " & ECU_TMLPART_CONTAINER_NUMBER & ".")
    '                AddDebugString("SERVER TMLPART NUMBER: " & SERVER_TMLPART_CONTAINER_NUMBER & ".")
    '            Else
    '                DataMatch = False
    '                AddDebugString("BEFORE FLASH ECU DATA MISMATCH")
    '                AddDebugString(" BEFORE FLASH ECU TMLPART NUMBER: " & ECU_TMLPART_CONTAINER_NUMBER & ".")
    '                AddDebugString("SERVER TMLPART NUMBER: " & SERVER_TMLPART_CONTAINER_NUMBER & ".")
    '                If blnBeforFlashIdentification = False Then
    '                    AddDebugString("ECU DATA MISMATCH")
    '                    AddDebugString("ECU TMLPART NUMBER: " & ECU_TMLPART_CONTAINER_NUMBER & ".")
    '                    AddDebugString("SERVER TMLPART NUMBER: " & SERVER_TMLPART_CONTAINER_NUMBER & ".")
    '                    DataMatch = False
    '                End If
    '            End If
    '        End If


    '        If ECU_TMLPART_CONTAINER_VERSION = "" Then

    '        Else
    '            If SERVER_TMLPART_CONTAINER_VERSION.Trim() = ECU_TMLPART_CONTAINER_VERSION.Trim() Then
    '                AddDebugString("ECU & SERVER TMLPART VERSION SAME")
    '                AddDebugString("ECU TMLPART VERSION: " & ECU_TMLPART_CONTAINER_VERSION & ".")
    '                AddDebugString("SERVER TMLPART VERSION: " & SERVER_TMLPART_CONTAINER_VERSION & ".")
    '            Else
    '                DataMatch = False
    '                AddDebugString("BEFORE FLASH ECU DATA MISMATCH")
    '                AddDebugString(" BEFORE FLASH ECU TMLPART VERSION: " & ECU_TMLPART_CONTAINER_VERSION & ".")
    '                AddDebugString("SERVER TMLPART VERSION: " & SERVER_TMLPART_CONTAINER_VERSION & ".")
    '                If blnBeforFlashIdentification = False Then
    '                    AddDebugString("ECU DATA MISMATCH")
    '                    AddDebugString("ECU TMLPART VERSION: " & ECU_TMLPART_CONTAINER_VERSION & ".")
    '                    AddDebugString("SERVER TMLPART VERSION: " & SERVER_TMLPART_CONTAINER_VERSION & ".")
    '                    DataMatch = False
    '                End If
    '            End If
    '        End If


    '        If ECU_TMLSOFTWARE_NUMBER = "" Then

    '        Else
    '            If Val(SERVER_TMLSOFTWARE_NUMBER.Trim()) = Val(ECU_TMLSOFTWARE_NUMBER.Trim()) Then
    '                AddDebugString("ECU & SERVER TML SOFTWARE NUMBER SAME")
    '                AddDebugString("ECU TML SOFTWARE NO: " & ECU_TMLSOFTWARE_NUMBER & ".")
    '                AddDebugString("SERVER TML SOFTWARE NO: " & SERVER_TMLSOFTWARE_NUMBER & ".")
    '            Else
    '                DataMatch = False
    '                AddDebugString("BEFORE FLASH ECU DATA MISMATCH")
    '                AddDebugString("BEFORE FLASH ECU TML SOFTWARE NO: " & ECU_TMLSOFTWARE_NUMBER & ".")
    '                AddDebugString("SERVER TML SOFTWARE NO: " & SERVER_TMLSOFTWARE_NUMBER.ToString() & ".")
    '                If blnBeforFlashIdentification = False Then
    '                    AddDebugString("ECU DATA MISMATCH")
    '                    AddDebugString("ECU TML SOFTWARE NO: " & ECU_TMLSOFTWARE_NUMBER & ".")
    '                    AddDebugString("SERVER TML SOFTWARE NO: " & SERVER_TMLSOFTWARE_NUMBER.ToString() & ".")
    '                    DataMatch = False
    '                End If
    '            End If
    '        End If


    '        If ECU_TMLSOFTWARE_VERSION = "" Then

    '        Else
    '            If SERVER_TMLSOFTWARE_VERSION.Trim() = ECU_TMLSOFTWARE_VERSION.Trim() Then
    '                AddDebugString("ECU & SERVER TML SOFTWARE VERSION SAME")
    '                AddDebugString("ECU TML SOFTWARE VERSION: " & ECU_TMLSOFTWARE_VERSION & ".")
    '                AddDebugString("SERVER TML SOFTWARE VERSION: " & SERVER_TMLSOFTWARE_VERSION & ".")
    '            Else
    '                DataMatch = False
    '                AddDebugString("BEFORE FLASH ECU DATA MISMATCH")
    '                AddDebugString("BEFORE FLASH ECU TML SOFTWARE VERSION: " & ECU_TMLSOFTWARE_VERSION & ".")
    '                AddDebugString("SERVER TML SOFTWARE VERSION: " & SERVER_TMLSOFTWARE_VERSION.ToString() & ".")
    '                If blnBeforFlashIdentification = False Then
    '                    AddDebugString("ECU DATA MISMATCH")
    '                    AddDebugString("ECU TML SOFTWARE VERSION: " & ECU_TMLSOFTWARE_VERSION & ".")
    '                    AddDebugString("SERVER TML SOFTWARE VERSION: " & SERVER_TMLSOFTWARE_VERSION.ToString() & ".")
    '                    DataMatch = False
    '                End If
    '            End If
    '        End If
    '        If DataMatch Then
    '            AddDebugString("IDENTIFICATION OK")
    '            PRE_FALSHED = True
    '        End If
    '        Application.DoEvents()
    '        Return DataMatch
    '    Catch ex As Exception
    '        AddDebugString(ex.Message)
    '        AddDebugString(ex.StackTrace)
    '        AddDebugString("IDENTIFICATION FAILED")
    '        Return False
    '    End Try
    'End Function

    'Public Function DLLInit() As Object
    '    objDllrepro = New Reprogramming_Dll.Reprogrammer
    '    objECUCom = New ECUCOMM
    '    objDllrepro._SetDiagnocommProperties(objECUCom)
    '    objDllInhouse._StartComm = AddressOf StartDiagnocomComm
    '    objDllInhouse._StopComm = AddressOf StopDiagnocomComm
    '    objDllInhouse._SendTxGetRx = AddressOf SendRequestReceiveResponse
    '    objDiagCom.P2MAxResponsePending = 5000
    '    objDiagCom.ChangedBaudRate = 500000

    '    If frmMaster.objGlobalStatic.COMMUNICATION_VCI.ToUpper() = "PASSTHRU" Then
    '        objDiagCom.CommunicationMode = EOLCommError.CONVERTER_TYPE.J2534_UDS_VIA_CAN
    '        objDiagCom.J2534VciIndex = frmMaster.objGlobalStatic.J2534VciIndex
    '        objDiagCom.PendingReceiveTimeoutInterval = 120000
    '        objDiagCom.ResponsePendingHandling = 120000
    '        objDiagCom.ReadResponseDelay = 120000
    '        objDiagCom.ReceiveTimeoutInterval = 5000
    '    Else
    '        objDiagCom.CommunicationMode = EOLCommError.CONVERTER_TYPE.SAMDIAX_UDS_VIA_CAN
    '    End If

    '    Return objDllInhouse
    'End Function

    Private Function StartDiagnocomComm(ByVal timeOutInterval As Integer) As Boolean
        For i As Integer = 0 To 4
            Dim DcomErrors As EOLCommError = objECUCom.StartCommunication(timeOutInterval)
            Select Case DcomErrors
                Case EOLCommError.BAD_START_COMMUNICATIONS_POSITIVE_RESPONSE_CHECKSUM
                    Exit Select
                Case EOLCommError.BUSY_SENDING_OTHER_REQUEST
                    Exit Select
                Case EOLCommError.CHECK_ECU_RESPONSE_FAILED
                    Exit Select
                Case EOLCommError.COMMUNICATION_EXCEPTION
                    Exit Select
                Case EOLCommError.COMMUNICATION_IS_ALREADY_RUNNING
                    Exit Select
                Case EOLCommError.COMMUNICATION_SESSION_NOT_STARTED
                    Exit Select
                Case EOLCommError.CONSECUTIVE_RESPONSE_RECEIVED
                    Exit Select
                Case EOLCommError.CONVERSION_ARRAY_TO_STRING_FAILED
                    Exit Select
                Case EOLCommError.CONVERSION_STRING_TO_ARRAY_FAILED
                    Exit Select
                Case EOLCommError.CONVERTER_FAILED_TO_BRING_KLINE_AND_LLINE_LOW
                    Exit Select
                Case EOLCommError.CONVERTER_FAILED_TO_BRING_KLINE_LOW
                    Exit Select
                Case EOLCommError.CONVERTER_FAILED_TO_BRING_LLINE_LOW
                    Exit Select
                Case EOLCommError.CONVERTER_MCU_ROM_CHECKSUM_ERROR
                    Exit Select
                Case EOLCommError.ECU_COMMUNICATION_NOT_ACTIVE
                    Exit Select
                Case EOLCommError.ERROR_IN_READING_STATUS_CODE_FROM_SILICON_ENGINE
                    Exit Select
                Case EOLCommError.FILE_NOT_FOUND
                    Exit Select
                Case EOLCommError.GENERAL_FAILED
                    Exit Select
                Case EOLCommError.GENERAL_FAIL_IN_RECEIVE_RESPONSE
                    Exit Select
                Case EOLCommError.GENERAL_FAIL_IN_SEND_REQUEST
                    Exit Select
                Case EOLCommError.HANDLE_IS_NOT_GAINED
                    Exit Select
                Case EOLCommError.HEX_TO_STRING_CONVERSION_ERROR
                    Exit Select
                Case EOLCommError.IMPROPER_INVERSE_ADDRESS_RECEIVED
                    Exit Select
                Case EOLCommError.IMPROPER_START_COMMUNICATIONS_POSITIVE_RESPONSE
                    Exit Select
                Case EOLCommError.IMPROPER_SYNC_BYTE_RECEIVED
                    Exit Select
                Case EOLCommError.INTERNAL_EXCEPTION_IS_OCCURED
                    Exit Select
                Case EOLCommError.INVALID_COMMUNICATION_MODE
                    Exit Select
                Case EOLCommError.INVALID_INTERBAYTE_DELAY
                    Exit Select
                Case EOLCommError.INVALID_KEY_BYTE1
                    Exit Select
                Case EOLCommError.INVALID_RX_BUFF
                    Exit Select
                Case EOLCommError.INVALID_SILICON_ENGINE_RESPONSE_HEADER
                    Exit Select
                Case EOLCommError.INVALID_TX_BUFF
                    Exit Select
                Case EOLCommError.KLINE_AND_LLINE_FAILED_TO_RETURN_HIGH
                    Exit Select
                Case EOLCommError.KLINE_AND_LLINE_LOW_FOR_1_SECOND
                    Exit Select
                Case EOLCommError.KLINE_FAILED_TO_RETURN_HIGH
                    Exit Select
                Case EOLCommError.KLINE_LOW_FOR_1_SECOND
                    Exit Select
                Case EOLCommError.LLINE_FAILED_TO_RETURN_HIGH
                    Exit Select
                Case EOLCommError.LLINE_LOW_FOR_1_SECOND
                    Exit Select
                Case EOLCommError.NO_RESPONSE_RECEIVED_TO_INITIALIZATION
                    Exit Select
                Case EOLCommError.PARAMETER_1_INCORRECT
                    Exit Select
                Case EOLCommError.PARAMETER_2_INCORRECT
                    Exit Select
                Case EOLCommError.PARAMETER_3_INCORRECT
                    Exit Select
                Case EOLCommError.PARAMETER_4_INCORRECT
                    Exit Select
                Case EOLCommError.PARAMETER_5_INCORRECT
                    Exit Select
                Case EOLCommError.PARAMETER_LOADING_ERROR
                    Exit Select
                Case EOLCommError.PORT_IS_NOT_OPEN
                    Exit Select
                Case EOLCommError.PREVIOUS_REQUEST_IS_BEING_PROCESSED
                    Exit Select
                Case EOLCommError.RECEIVED_INVALID_CHECKSUM_FROM_PC
                    Exit Select
                Case EOLCommError.RECEIVED_INVALID_COMMAND_FROM_PC
                    Exit Select
                Case EOLCommError.RECEIVE_BUFFER_OVERFLOW_FROM_PC_TXD
                    Exit Select
                Case EOLCommError.RECEIVING_FAILED
                    Exit Select
                Case EOLCommError.RESPONSE_PENDIG_RECEIVED
                    Exit Select
                Case EOLCommError.RX_CHECKSUM_NOT_MATCHED
                    Exit Select
                Case EOLCommError.SENDING_FAILED
                    Exit Select
                Case EOLCommError.SILICON_ENGINE_RESPONSE_CHECKSUM_INVALID
                    Exit Select
                Case EOLCommError.START_COMMUNICATION_FAILED
                    Exit Select
                Case EOLCommError.START_COMMUNICATION_TIMEOUT
                    Exit Select
                Case EOLCommError.START_SESSION_FAILED_NEGATIVE_RESPONSE
                    Exit Select
                Case EOLCommError.STOP_COMMUNICATION_FAILED
                    Exit Select
                Case EOLCommError.STRING_TO_HEX_CONVERSION_ERROR
                    Exit Select
                Case EOLCommError.SUCCESSFUL
                    strResponse = "Successful"
                    Return True
                Case EOLCommError.TIMEOUT_WHILE_RECEPTION
                    Exit Select
                Case EOLCommError.TRANSMIT_BUFFER_OVERFLOW_FROM_PC_RXD
                    Exit Select
                Case EOLCommError.UNABLE_TO_OPEN_THE_PORT
                    Exit Select
                Case EOLCommError.UNABLE_TO_START_COMMUNICATION
                    Exit Select
                Case EOLCommError.UNHANDLED_EXCEPTIONOCCURED
                    Exit Select
                Case EOLCommError.UNKNOWN
                    Exit Select
                Case EOLCommError.UNKNOW_STATUS_CODE_FROM_SILICON_ENGINE
                    Exit Select
                Case Else
                    Exit Select
            End Select
            strResponse = DcomErrors.ToString()
        Next

        Return False

    End Function

    Private Function StopDiagnocomComm(ByVal respRequired As Boolean) As Boolean
        Dim DcomErrors As EOLCommError = objECUCom.StopCommunication(True)
        Select Case DcomErrors
            Case EOLCommError.BAD_START_COMMUNICATIONS_POSITIVE_RESPONSE_CHECKSUM
                Exit Select
            Case EOLCommError.BUSY_SENDING_OTHER_REQUEST
                Exit Select
            Case EOLCommError.CHECK_ECU_RESPONSE_FAILED
                Exit Select
            Case EOLCommError.COMMUNICATION_EXCEPTION
                Exit Select
            Case EOLCommError.COMMUNICATION_IS_ALREADY_RUNNING
                Exit Select
            Case EOLCommError.COMMUNICATION_SESSION_NOT_STARTED
                Exit Select
            Case EOLCommError.CONSECUTIVE_RESPONSE_RECEIVED
                Exit Select
            Case EOLCommError.CONVERSION_ARRAY_TO_STRING_FAILED
                Exit Select
            Case EOLCommError.CONVERSION_STRING_TO_ARRAY_FAILED
                Exit Select
            Case EOLCommError.CONVERTER_FAILED_TO_BRING_KLINE_AND_LLINE_LOW
                Exit Select
            Case EOLCommError.CONVERTER_FAILED_TO_BRING_KLINE_LOW
                Exit Select
            Case EOLCommError.CONVERTER_FAILED_TO_BRING_LLINE_LOW
                Exit Select
            Case EOLCommError.CONVERTER_MCU_ROM_CHECKSUM_ERROR
                Exit Select
            Case EOLCommError.ECU_COMMUNICATION_NOT_ACTIVE
                Exit Select
            Case EOLCommError.ERROR_IN_READING_STATUS_CODE_FROM_SILICON_ENGINE
                Exit Select
            Case EOLCommError.FILE_NOT_FOUND
                Exit Select
            Case EOLCommError.GENERAL_FAILED
                Exit Select
            Case EOLCommError.GENERAL_FAIL_IN_RECEIVE_RESPONSE
                Exit Select
            Case EOLCommError.GENERAL_FAIL_IN_SEND_REQUEST
                Exit Select
            Case EOLCommError.HANDLE_IS_NOT_GAINED
                Exit Select
            Case EOLCommError.HEX_TO_STRING_CONVERSION_ERROR
                Exit Select
            Case EOLCommError.IMPROPER_INVERSE_ADDRESS_RECEIVED
                Exit Select
            Case EOLCommError.IMPROPER_START_COMMUNICATIONS_POSITIVE_RESPONSE
                Exit Select
            Case EOLCommError.IMPROPER_SYNC_BYTE_RECEIVED
                Exit Select
            Case EOLCommError.INTERNAL_EXCEPTION_IS_OCCURED
                Exit Select
            Case EOLCommError.INVALID_COMMUNICATION_MODE
                Exit Select
            Case EOLCommError.INVALID_INTERBAYTE_DELAY
                Exit Select
            Case EOLCommError.INVALID_KEY_BYTE1
                Exit Select
            Case EOLCommError.INVALID_RX_BUFF
                Exit Select
            Case EOLCommError.INVALID_SILICON_ENGINE_RESPONSE_HEADER
                Exit Select
            Case EOLCommError.INVALID_TX_BUFF
                Exit Select
            Case EOLCommError.KLINE_AND_LLINE_FAILED_TO_RETURN_HIGH
                Exit Select
            Case EOLCommError.KLINE_AND_LLINE_LOW_FOR_1_SECOND
                Exit Select
            Case EOLCommError.KLINE_FAILED_TO_RETURN_HIGH
                Exit Select
            Case EOLCommError.KLINE_LOW_FOR_1_SECOND
                Exit Select
            Case EOLCommError.LLINE_FAILED_TO_RETURN_HIGH
                Exit Select
            Case EOLCommError.LLINE_LOW_FOR_1_SECOND
                Exit Select
            Case EOLCommError.NO_RESPONSE_RECEIVED_TO_INITIALIZATION
                Exit Select
            Case EOLCommError.PARAMETER_1_INCORRECT
                Exit Select
            Case EOLCommError.PARAMETER_2_INCORRECT
                Exit Select
            Case EOLCommError.PARAMETER_3_INCORRECT
                Exit Select
            Case EOLCommError.PARAMETER_4_INCORRECT
                Exit Select
            Case EOLCommError.PARAMETER_5_INCORRECT
                Exit Select
            Case EOLCommError.PARAMETER_LOADING_ERROR
                Exit Select
            Case EOLCommError.PORT_IS_NOT_OPEN
                Exit Select
            Case EOLCommError.PREVIOUS_REQUEST_IS_BEING_PROCESSED
                Exit Select
            Case EOLCommError.RECEIVED_INVALID_CHECKSUM_FROM_PC
                Exit Select
            Case EOLCommError.RECEIVED_INVALID_COMMAND_FROM_PC
                Exit Select
            Case EOLCommError.RECEIVE_BUFFER_OVERFLOW_FROM_PC_TXD
                Exit Select
            Case EOLCommError.RECEIVING_FAILED
                Exit Select
            Case EOLCommError.RESPONSE_PENDIG_RECEIVED
                Exit Select
            Case EOLCommError.RX_CHECKSUM_NOT_MATCHED
                Exit Select
            Case EOLCommError.SENDING_FAILED
                Exit Select
            Case EOLCommError.SILICON_ENGINE_RESPONSE_CHECKSUM_INVALID
                Exit Select
            Case EOLCommError.START_COMMUNICATION_FAILED
                Exit Select
            Case EOLCommError.START_COMMUNICATION_TIMEOUT
                Exit Select
            Case EOLCommError.START_SESSION_FAILED_NEGATIVE_RESPONSE
                Exit Select
            Case EOLCommError.STOP_COMMUNICATION_FAILED
                Exit Select
            Case EOLCommError.STRING_TO_HEX_CONVERSION_ERROR
                Exit Select
            Case EOLCommError.SUCCESSFUL
                strResponse = "Successful"
                Return True
            Case EOLCommError.TIMEOUT_WHILE_RECEPTION
                Exit Select
            Case EOLCommError.TRANSMIT_BUFFER_OVERFLOW_FROM_PC_RXD
                Exit Select
            Case EOLCommError.UNABLE_TO_OPEN_THE_PORT
                Exit Select
            Case EOLCommError.UNABLE_TO_START_COMMUNICATION
                Exit Select
            Case EOLCommError.UNHANDLED_EXCEPTIONOCCURED
                Exit Select
            Case EOLCommError.UNKNOWN
                Exit Select
            Case EOLCommError.UNKNOW_STATUS_CODE_FROM_SILICON_ENGINE
                Exit Select
            Case Else
                Exit Select
        End Select
        strResponse = DcomErrors.ToString()
        Return False
    End Function

    Private Function SendRequestReceiveResponse(ByVal Tx As [Byte](), ByRef Rx As [Byte]()) As Boolean
        Try
            Dim DcomErrors As EOLCommError = objECUCom.SendRequestReceiveResponse(Tx, Rx, 15)
            writeEMSDiagLog(Tx, Rx)
            Select Case DcomErrors
                Case EOLCommError.BAD_START_COMMUNICATIONS_POSITIVE_RESPONSE_CHECKSUM
                    Exit Select
                Case EOLCommError.BUSY_SENDING_OTHER_REQUEST
                    Exit Select
                Case EOLCommError.CHECK_ECU_RESPONSE_FAILED
                    Exit Select
                Case EOLCommError.COMMUNICATION_EXCEPTION
                    Exit Select
                Case EOLCommError.COMMUNICATION_IS_ALREADY_RUNNING
                    Exit Select
                Case EOLCommError.COMMUNICATION_SESSION_NOT_STARTED
                    Exit Select
                Case EOLCommError.CONSECUTIVE_RESPONSE_RECEIVED
                    Exit Select
                Case EOLCommError.CONVERSION_ARRAY_TO_STRING_FAILED
                    Exit Select
                Case EOLCommError.CONVERSION_STRING_TO_ARRAY_FAILED
                    Exit Select
                Case EOLCommError.CONVERTER_FAILED_TO_BRING_KLINE_AND_LLINE_LOW
                    Exit Select
                Case EOLCommError.CONVERTER_FAILED_TO_BRING_KLINE_LOW
                    Exit Select
                Case EOLCommError.CONVERTER_FAILED_TO_BRING_LLINE_LOW
                    Exit Select
                Case EOLCommError.CONVERTER_MCU_ROM_CHECKSUM_ERROR
                    Exit Select
                Case EOLCommError.ECU_COMMUNICATION_NOT_ACTIVE
                    Exit Select
                Case EOLCommError.ERROR_IN_READING_STATUS_CODE_FROM_SILICON_ENGINE
                    Exit Select
                Case EOLCommError.FILE_NOT_FOUND
                    Exit Select
                Case EOLCommError.GENERAL_FAILED
                    Exit Select
                Case EOLCommError.GENERAL_FAIL_IN_RECEIVE_RESPONSE
                    Exit Select
                Case EOLCommError.GENERAL_FAIL_IN_SEND_REQUEST
                    Exit Select
                Case EOLCommError.HANDLE_IS_NOT_GAINED
                    Exit Select
                Case EOLCommError.HEX_TO_STRING_CONVERSION_ERROR
                    Exit Select
                Case EOLCommError.IMPROPER_INVERSE_ADDRESS_RECEIVED
                    Exit Select
                Case EOLCommError.IMPROPER_START_COMMUNICATIONS_POSITIVE_RESPONSE
                    Exit Select
                Case EOLCommError.IMPROPER_SYNC_BYTE_RECEIVED
                    Exit Select
                Case EOLCommError.INTERNAL_EXCEPTION_IS_OCCURED
                    Exit Select
                Case EOLCommError.INVALID_COMMUNICATION_MODE
                    Exit Select
                Case EOLCommError.INVALID_INTERBAYTE_DELAY
                    Exit Select
                Case EOLCommError.INVALID_KEY_BYTE1
                    Exit Select
                Case EOLCommError.INVALID_RX_BUFF
                    Exit Select
                Case EOLCommError.INVALID_SILICON_ENGINE_RESPONSE_HEADER
                    Exit Select
                Case EOLCommError.INVALID_TX_BUFF
                    Exit Select
                Case EOLCommError.KLINE_AND_LLINE_FAILED_TO_RETURN_HIGH
                    Exit Select
                Case EOLCommError.KLINE_AND_LLINE_LOW_FOR_1_SECOND
                    Exit Select
                Case EOLCommError.KLINE_FAILED_TO_RETURN_HIGH
                    Exit Select
                Case EOLCommError.KLINE_LOW_FOR_1_SECOND
                    Exit Select
                Case EOLCommError.LLINE_FAILED_TO_RETURN_HIGH
                    Exit Select
                Case EOLCommError.LLINE_LOW_FOR_1_SECOND
                    Exit Select
                Case EOLCommError.NO_RESPONSE_RECEIVED_TO_INITIALIZATION
                    Exit Select
                Case EOLCommError.PARAMETER_1_INCORRECT
                    Exit Select
                Case EOLCommError.PARAMETER_2_INCORRECT
                    Exit Select
                Case EOLCommError.PARAMETER_3_INCORRECT
                    Exit Select
                Case EOLCommError.PARAMETER_4_INCORRECT
                    Exit Select
                Case EOLCommError.PARAMETER_5_INCORRECT
                    Exit Select
                Case EOLCommError.PARAMETER_LOADING_ERROR
                    Exit Select
                Case EOLCommError.PORT_IS_NOT_OPEN
                    Exit Select
                Case EOLCommError.PREVIOUS_REQUEST_IS_BEING_PROCESSED
                    Exit Select
                Case EOLCommError.RECEIVED_INVALID_CHECKSUM_FROM_PC
                    Exit Select
                Case EOLCommError.RECEIVED_INVALID_COMMAND_FROM_PC
                    Exit Select
                Case EOLCommError.RECEIVE_BUFFER_OVERFLOW_FROM_PC_TXD
                    Exit Select
                Case EOLCommError.RECEIVING_FAILED
                    Exit Select
                Case EOLCommError.RESPONSE_PENDIG_RECEIVED
                    Exit Select
                Case EOLCommError.RX_CHECKSUM_NOT_MATCHED
                    Exit Select
                Case EOLCommError.SENDING_FAILED
                    Exit Select
                Case EOLCommError.SILICON_ENGINE_RESPONSE_CHECKSUM_INVALID
                    Exit Select
                Case EOLCommError.START_COMMUNICATION_FAILED
                    Exit Select
                Case EOLCommError.START_COMMUNICATION_TIMEOUT
                    Exit Select
                Case EOLCommError.START_SESSION_FAILED_NEGATIVE_RESPONSE
                    Exit Select
                Case EOLCommError.STOP_COMMUNICATION_FAILED
                    Exit Select
                Case EOLCommError.STRING_TO_HEX_CONVERSION_ERROR
                    Exit Select
                Case EOLCommError.SUCCESSFUL
                    Return True
                Case EOLCommError.TIMEOUT_WHILE_RECEPTION
                    Exit Select
                Case EOLCommError.TRANSMIT_BUFFER_OVERFLOW_FROM_PC_RXD
                    Exit Select
                Case EOLCommError.UNABLE_TO_OPEN_THE_PORT
                    Exit Select
                Case EOLCommError.UNABLE_TO_START_COMMUNICATION
                    Exit Select
                Case EOLCommError.UNHANDLED_EXCEPTIONOCCURED
                    Exit Select
                Case EOLCommError.UNKNOWN
                    Exit Select
                Case EOLCommError.UNKNOW_STATUS_CODE_FROM_SILICON_ENGINE
                    Exit Select
                Case Else
                    Exit Select
            End Select
        Catch generatedExceptionName As Exception
            Throw
        End Try

        Return False
    End Function

    Public Function ByteArrayToString(ByVal byteArray As Byte(), ByVal startIndex As Integer, ByVal Length As Integer) As String
        Dim result As String = ""
        Try
            Dim array__1 As Byte() = New Byte(Length - 1) {}
            Array.Copy(byteArray, startIndex, array__1, 0, Length)
            Dim text As String = BitConverter.ToString(array__1)
            result = text.Replace("-", " ")
        Catch ex As Exception
            AddDebugString(ex.Message)
        End Try
        Return result
    End Function

    Private Sub writeEMSDiagLog(ByVal TxData As Byte(), ByVal RxData As Byte())
        Dim strTxData As String = ByteArrayToString(TxData, 0, TxData.Length)
        Dim strRxData As String = ByteArrayToString(RxData, 0, RxData.Length)
        AddDebugString("Tx Data :: " & strTxData & " ")
        AddDebugString("Rx Data :: " & strRxData & " ")
    End Sub

    Protected Function funExit() As Boolean
        Try
            If Not objDllrepro Is Nothing Then
                'If objDllrepro._IsCommunicationStopped(True) Then

                'End If
                objDllrepro = Nothing
                GC.Collect()
            End If
            Return True
        Catch ex As Exception
            AddDebugString("EXCEPTION WHILE funExit:" & ex.Message)
            Return False
        End Try
    End Function

    'Protected Function funCheckCommStatus() As Boolean
    '    Try
    '        For i As Integer = 0 To 120

    '            If IsNothing(objDllInhouse) Then
    '                'DLLInit()
    '            End If
    '            objfrmmaster.InsertDataIntoListView("PLEASE WAIT FOR POWER CUT TO ECU." & i, "NOK")
    '            Application.DoEvents()
    '            System.Threading.Thread.Sleep(1000)
    '            Dim strResponse As String = ""
    '            If objDllInhouse._IsCommunicationStarted(4000) Then
    '                AddDebugString("Communication Running")
    '                AddDebugString("ECU IN INIT MODE.RESPONSE-" & i)
    '                funExit()
    '                ''If objDllInhouse._GetSecurityAccess(Status, ECUFunctions.EnumSecurityAccessType.type_0_1) Then
    '                ''    AddDebugString("Communication Running")
    '                ''    AddDebugString("ECU IN INIT MODE.RESPONSE-" & i)
    '                ''Else
    '                ''    AddDebugString("security failed communication stop")
    '                ''    AddDebugString("ECU IN STOPPED MODE.RESPONSE-" & i)
    '                ''    funExit()
    '                ''    Exit For
    '                ''    Return True
    '                ''End If

    '                If i = 119 Then
    '                    AddDebugString("FOR i-" & i)
    '                    objfrmmaster.InsertDataIntoListView("POWER-LATCH CYCLE FAILED.", "NOK")
    '                    'obj_Flashing_Diagnostics.strFNDRemark = "POWER-LATCH CYCLE FAILED,ECU RESPONSE-" & strResponse
    '                    'obj_Flashing_Diagnostics.SetRemarkStatus_Global(obj_Flashing_Diagnostics.strFNDRemark, Flashing_Diagnostics.Enum_SetRemark.FLASHING_, False, obj_Flashing_Diagnostics.intEMSIndex)
    '                    Application.DoEvents()
    '                    Return False
    '                End If
    '            Else
    '                AddDebugString("ECU IN STOPPED MODE.RESPONSE-" & i)
    '                funExit()
    '                Exit For
    '                Return True
    '            End If
    '        Next
    '        Return True
    '    Catch ex As Exception
    '        AddDebugString("Exception in funCheckCommStatus()-" & ex.Message)
    '    End Try
    'End Function

End Class
