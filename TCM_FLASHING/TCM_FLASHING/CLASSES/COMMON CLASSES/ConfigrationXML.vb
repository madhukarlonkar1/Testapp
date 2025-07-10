Imports System.Xml
Imports System.IO
Imports System.Windows.Forms

Public Class ConfigrationXML

#Region "Global Variable Declaration"
    Public strGlobalFullPath As String = My.Settings.Config_File_Path
    Public strPlcConfigFullPath As String = "C:\EOL_APPLICATION\CONFIG\OFFLINE\EOL_PLC_VCID.XML"
    Public AdminUserName As String = "admin"
    Public AdminPassword As String = "admin123"
    Public TSUserName As String = "user"
    Public TSPassword As String = "user123"
    Private rootNodeName As String = "DOCUMENT"
    Dim hashTblMain As New Hashtable
    Dim objstruct_NodeNames As struct_NodeNames
    Dim objPLCstruct_NodeNames As struct_PLCNames

    ' Public objclsgetdata As clsGetData


#End Region

#Region "Enum"

    Public Enum enum_PLCNodes As Integer
        PLC = 1
        PLCBYPASS = 2
        VOLTAGESELECTION = 3
        IGNITIONSWITCH = 4
        PLCIPADDRESS = 5
    End Enum

    Private Structure struct_PLCNames
        Public PLC As String
        Public PLCBYPASS As String
        Public VOLTAGESELECTIONTAG As String
        Public IGNITIONSWITCHTAG As String
        Public PLCIPADDRESS As String
        Public PLCTOPICNAME As String
    End Structure

    Public Enum enum_Nodes As Integer
        LOGIN = 1
        MAINSERVER = 2
        LOCALSERVER = 3
        LOCALMACHINE = 4
        COMMON = 5
        FLAG = 6
        NECMDATABASE = 7
        EOLPROCESSDATABASE = 8
        ECUDETAILSDATABASE = 9
        TSUTILITYDATABASE = 10
        ENGINEDATABASE = 11
        FTPDETAILS = 12
        REQUIREDLOCALSERVER = 13
        LOCALMACHINEDATABASE = 14
        PLANTCODE = 15
        TCFLINE = 16
        MACADDRESS = 17
        IP_MAINSERVER = 18
        IP_LOCALSERVER = 19
        IP_NAME_LOCALMACHINE = 20
        SERVERNODE = 28
    End Enum

#End Region

#Region "Structure"

    Private Structure struct_NodeNames
        Public MAINSERVER As String
        Public LOCALSERVER As String
        Public LOCALMACHINE As String

        Public LOGIN As String
        Public USERID As String
        Public PASSWORD As String
        Public USERIDTS As String
        Public PASSWORDTS As String

        Public COMMON As String
        Public PRINTERADDRESS As String
        Public HSPLUSSERIALNO As String
        Public COMMUNICATIONVCI As String
        Public PASSTHRUVCI As String

        Public IP As String
        Public DATABASENAME As String

        Public NECMDATABASECONNECTIONSTRING As String
        Public ECUDETAILSDATABASECONNECTIONSTRING As String
        Public EOLPROCESSDATABASECONNECTIONSTRING As String
        Public TSUTILITYDATABASECONNECTIONSTRING As String
        Public ENGINEDATABASECONNECTIONSTRING As String
        Public FTPUSERID As String
        Public FTPPASSWORD As String
        Public FTPPORTNUMBER As String

        Public REQUIREDLOCALSERVER As String

        Public LOCALMACHINEDATABASECONNECTIONSTRING As String
        Public PLANTCODE As String
        Public TCFLINE As String
        Public MACADDRESS As String

        Public FLAG As String
    End Structure

#End Region

#Region "AddItemToStuctureObject"

    Private Sub AddItemToPlcObject()
        Dim strErrAppend As String = "ConfigrationXML::AddItemToStuctureObject:---- "
        Try

            objPLCstruct_NodeNames = Nothing
            objPLCstruct_NodeNames = New struct_PLCNames

            objPLCstruct_NodeNames.PLC = "PLC"

            objPLCstruct_NodeNames.PLCBYPASS = "PLCBYPASS"
            objPLCstruct_NodeNames.VOLTAGESELECTIONTAG = "VOLTAGESELECTIONTAG"
            objPLCstruct_NodeNames.IGNITIONSWITCHTAG = "IGNITIONSWITCHTAG"
            objPLCstruct_NodeNames.PLCIPADDRESS = "PLCIPADDRESS"
            objPLCstruct_NodeNames.PLCTOPICNAME = "PLCTOPICNAME"
        Catch ex As Exception
            frmMaster.WriteLog(strErrAppend & "Exception :---" & ex.Message)
        End Try

    End Sub

    Private Sub AddItemToStuctureObject()
        Dim strErrAppend As String = "ConfigrationXML::AddItemToStuctureObject:---- "
        Try
            objstruct_NodeNames = Nothing
            objstruct_NodeNames = New struct_NodeNames

            objstruct_NodeNames.COMMON = "COMMON"

            objstruct_NodeNames.MAINSERVER = "MAINSERVER"
            objstruct_NodeNames.LOCALSERVER = "LOCALSERVER"
            objstruct_NodeNames.LOCALMACHINE = "LOCALMACHINE"

            objstruct_NodeNames.IP = "IP"
            objstruct_NodeNames.LOGIN = "LOGIN"
            objstruct_NodeNames.USERID = "USERID"
            objstruct_NodeNames.PASSWORD = "PASSWORD"
            objstruct_NodeNames.USERIDTS = "USERIDTS"
            objstruct_NodeNames.PASSWORDTS = "PASSWORDTS"

            objstruct_NodeNames.COMMON = "COMMON"
            objstruct_NodeNames.PRINTERADDRESS = "PRINTERADDRESS"
            objstruct_NodeNames.HSPLUSSERIALNO = "HSPLUSSERIALNO"
            objstruct_NodeNames.COMMUNICATIONVCI = "COMMUNICATIONVCI"
            objstruct_NodeNames.PASSTHRUVCI = "PASSTHRUVCI"

            objstruct_NodeNames.NECMDATABASECONNECTIONSTRING = "CONNECTIONSTRING_NECMDATABASE"
            objstruct_NodeNames.ECUDETAILSDATABASECONNECTIONSTRING = "CONNECTIONSTRING_ECUDETAILSDATABASE"
            objstruct_NodeNames.EOLPROCESSDATABASECONNECTIONSTRING = "CONNECTIONSTRING_EOLPROCESSDATABASE"
            objstruct_NodeNames.TSUTILITYDATABASECONNECTIONSTRING = "CONNECTIONSTRING_TSUTILITYDATABASE"
            objstruct_NodeNames.ENGINEDATABASECONNECTIONSTRING = "CONNECTIONSTRING_ENGINEDATABASE"
            objstruct_NodeNames.FTPUSERID = "FTPUSERID"
            objstruct_NodeNames.FTPPASSWORD = "FTPPASSWORD"
            objstruct_NodeNames.FTPPORTNUMBER = "FTPPORTNUMBER"

            objstruct_NodeNames.REQUIREDLOCALSERVER = "REQUIREDLOCALSERVER"

            objstruct_NodeNames.LOCALMACHINEDATABASECONNECTIONSTRING = "CONNECTIONSTRING_LOCALMACHINEDATABASE"
            objstruct_NodeNames.PLANTCODE = "PLANTCODE"
            objstruct_NodeNames.TCFLINE = "TCFLINE"
            objstruct_NodeNames.MACADDRESS = "MACADDRESS"

            objstruct_NodeNames.FLAG = "FLAG"
        Catch ex As Exception
            frmMaster.WriteLog(strErrAppend & "Exception :---" & ex.Message)
        End Try
    End Sub

#End Region


#Region "[CREATE XML FILE]"
    Public Function funCreateXMLFile() As Boolean
        Dim strErrAppend As String = "ConfigrationXML::funCreateXMLFile:---- "
        Try
            If Directory.Exists(strGlobalFullPath.Remove(26)) = False Then
                Directory.CreateDirectory(strGlobalFullPath.Remove(26))
            End If
            AddItemToStuctureObject()
            If Not String.IsNullOrEmpty(strGlobalFullPath) Then
                If Not File.Exists(strGlobalFullPath) Then
                    Dim writer As New XmlTextWriter(strGlobalFullPath, System.Text.Encoding.UTF8)
                    writer.WriteStartDocument(True)
                    writer.WriteStartElement(rootNodeName)
                    subCommomCreateNode(enum_Nodes.LOGIN, writer)
                    subCommomCreateNode(enum_Nodes.SERVERNODE, writer, objstruct_NodeNames.MAINSERVER)
                    subCommomCreateNode(enum_Nodes.REQUIREDLOCALSERVER, writer)
                    Dim i As Integer = 0
                    For i = 1 To 8
                        subCommomCreateNode(enum_Nodes.SERVERNODE, writer, objstruct_NodeNames.LOCALSERVER & i)
                    Next
                    subCommomCreateNode(enum_Nodes.COMMON, writer)
                    subCommomCreateNode(enum_Nodes.LOCALMACHINE, writer, objstruct_NodeNames.LOCALMACHINE)
                    subCommomCreateNode(enum_Nodes.FLAG, writer)
                    writer.WriteEndElement()
                    writer.Close()
                End If
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            frmMaster.WriteLog(strErrAppend & "Exception :---" & ex.Message)
            Return False

        Finally
            objstruct_NodeNames = Nothing
        End Try
    End Function
#End Region

#Region "[SET DATA IN XML FILE]"

    Public Function funSetEolConfigXmlData(ByVal objNode As enum_Nodes, ByVal objCollection As Collection, Optional ByVal intlocalServerNo As Integer = 0, Optional ByVal flagValue As Boolean = False) As Boolean
        Dim strErrAppend As String = "ConfigrationXML::funSetEolConfigXmlData:---- "
        Try
            AddItemToStuctureObject()
            Select Case objNode
                Case enum_Nodes.FLAG
                    Dim XMLDoc As New XmlDocument
                    Dim rootnode As XmlNode = Nothing
                    Dim xmlnode As XmlNode = Nothing
                    XMLDoc.Load(strGlobalFullPath)
                    rootnode = XMLDoc.DocumentElement
                    Dim itr As Integer = 0
                    Dim index As Integer = 1
                    Dim xmlNodeRequired As XmlNodeList

                    itr = 0
                    index = 1
                    xmlNodeRequired = rootnode.SelectNodes(objstruct_NodeNames.FLAG)

                    For Each xmlnode In xmlNodeRequired
                        For itr = 0 To xmlnode.ChildNodes.Count - 1
                            xmlnode.ChildNodes(itr).InnerText = flagValue
                            index += 1
                        Next
                    Next

                    XMLDoc.Save(strGlobalFullPath)
                    Exit Select
                Case enum_Nodes.REQUIREDLOCALSERVER
                    Dim intServer As Integer = Convert.ToInt32(objCollection(1))
                    If intServer >= 1 And intServer <= 8 Then
                        subSetXmlData(objstruct_NodeNames.REQUIREDLOCALSERVER, objCollection)
                    Else
                        MsgBox("Please Enter the Local Server Value Between 1 to 8.")
                        Return Nothing
                        Exit Function
                    End If

                    Exit Select
                Case enum_Nodes.COMMON
                    subSetXmlData(objstruct_NodeNames.COMMON, objCollection)
                    Exit Select
                Case enum_Nodes.LOCALMACHINE
                    subSetXmlData(objstruct_NodeNames.LOCALMACHINE, objCollection)
                    Exit Select
                Case enum_Nodes.MAINSERVER
                    subSetXmlData(objstruct_NodeNames.MAINSERVER, objCollection)
                    Exit Select
                Case enum_Nodes.LOCALSERVER
                    If intlocalServerNo >= 1 And intlocalServerNo <= 8 Then
                        subSetXmlData(objstruct_NodeNames.LOCALSERVER & intlocalServerNo, objCollection)
                    Else
                        MsgBox("Please Enter the Local Server Value Between 1 to 8 as third Parameter.")
                        Return False
                        Exit Function
                    End If
                    Exit Select
            End Select
        Catch ex As Exception
            frmMaster.WriteLog(strErrAppend & "Exception :---" & ex.Message)
        Finally
            objstruct_NodeNames = Nothing
        End Try
    End Function

    Private Sub subSetXmlData(ByVal nodeName As String, ByVal objCollection As Collection)

        Dim XMLDoc As New XmlDocument
        Dim rootnode As XmlNode = Nothing
        Dim xmlnode As XmlNode = Nothing
        XMLDoc.Load(strGlobalFullPath)
        rootnode = XMLDoc.DocumentElement
        Dim itr As Integer = 0
        Dim index As Integer = 1
        Dim xmlNodeRequired As XmlNodeList

        itr = 0
        index = 1
        xmlNodeRequired = rootnode.SelectNodes(nodeName)

        For Each xmlnode In xmlNodeRequired
            For itr = 0 To xmlnode.ChildNodes.Count - 1
                xmlnode.ChildNodes(itr).InnerText = objCollection.Item(index)
                index += 1
            Next
        Next

        XMLDoc.Save(strGlobalFullPath)

    End Sub

#End Region

#Region "[GET DATA FROM XML FILE]"

    'Public Function GetMainServerData() As Collection
    '    AddItemToStuctureObject()
    '    Dim localCollection As New Collection
    '    funGetServerData(objstruct_NodeNames.MAINSERVER, localCollection)
    '    objstruct_NodeNames = Nothing
    '    Return localCollection

    'End Function

    'Public Function GetLocalServerData(ByVal RequiredServer As Integer) As Collection
    '    AddItemToStuctureObject()
    '    If RequiredServer >= 1 And RequiredServer <= 8 Then
    '    Else
    '        MsgBox("Please Enter the Local Server Value Between 1 to 8.")
    '        Return Nothing
    '        Exit Function
    '    End If
    '    Dim localCollection As New Collection
    '    funGetServerData(objstruct_NodeNames.LOCALSERVER & RequiredServer, localCollection)
    '    objstruct_NodeNames = Nothing
    '    Return localCollection
    'End Function

    Public Function GetLocalMachineData() As Collection
        AddItemToStuctureObject()
        Dim localCollection As New Collection
        funGetEolConfigXmlData(enum_Nodes.LOCALMACHINE, localCollection)
        objstruct_NodeNames = Nothing
        Return localCollection
    End Function

    Public Function GetHSPlusPrinterData() As Collection
        AddItemToStuctureObject()
        Dim localCollection As New Collection
        funGetEolConfigXmlData(enum_Nodes.COMMON, localCollection)
        objstruct_NodeNames = Nothing
        Return localCollection
    End Function

    Public Function GetRequiredServerNumber() As Integer
        AddItemToStuctureObject()
        Dim localCollection As New Collection
        funGetEolConfigXmlData(enum_Nodes.REQUIREDLOCALSERVER, localCollection)
        objstruct_NodeNames = Nothing
        Return Convert.ToInt32(localCollection("REQUIREDLOCALSERVER"))
    End Function

    Public Function GetFlagValue() As Boolean
        AddItemToStuctureObject()
        Dim localCollection As New Collection
        funGetEolConfigXmlData(enum_Nodes.FLAG, localCollection)
        objstruct_NodeNames = Nothing
        Return Convert.ToBoolean(localCollection("FLAG"))
    End Function

    Public Function funGetEolConfigXmlData(ByVal objNode As enum_Nodes, ByRef refobjCollection As Collection) As Boolean
        Dim strErrAppend As String = "ConfigrationXML::funGetEolConfigXmlData:---- "
        Try
            AddItemToStuctureObject()
            refobjCollection = Nothing
            Dim localCollection As New Collection

            Dim XMLDoc As New XmlDocument
            Dim rootnode As XmlNode = Nothing
            Dim xmlnode As XmlNode = Nothing
            XMLDoc.Load(strGlobalFullPath)
            rootnode = XMLDoc.DocumentElement

            Dim i, j As Integer
            Dim xmlMainNode As XmlNodeList

            Select Case objNode
                Case enum_Nodes.LOCALMACHINE
                    Dim arrName(4) As String
                    j = 0
                    xmlMainNode = rootnode.SelectNodes(objstruct_NodeNames.LOCALMACHINE)

                    arrName(0) = "LOCAL_MACHINE_NAME"
                    arrName(1) = "LOCAL_MACHINE_DATABASE"
                    arrName(2) = "PLANT_CODE"
                    arrName(3) = "TCF_LINE"
                    arrName(4) = "MAC_ADDRESS"

                    i = 0
                    For Each xmlnode In xmlMainNode
                        For i = 0 To xmlnode.ChildNodes.Count - 1
                            ''Replace VCID DB Connection for Comman Config file 16R and VCID
                            Dim xmlValue As String = xmlnode.ChildNodes(i).InnerText
                            localCollection.Add(xmlValue, arrName(j))
                            '' localCollection.Add(xmlnode.ChildNodes(i).InnerText, arrName(j))
                            j += 1
                        Next
                    Next

                    Exit Select

                Case enum_Nodes.COMMON
                    Dim arrName(3) As String
                    j = 0
                    xmlMainNode = rootnode.SelectNodes(objstruct_NodeNames.COMMON)

                    arrName(0) = "HS_PLUS_SERIAL_NUMBER"
                    arrName(1) = "PRINTER_ADDRESS"
                    arrName(2) = "COMMUNICATION_VCI"
                    arrName(3) = "PASSTHRU_VCI"
                    i = -1
                    For Each xmlnode In xmlMainNode
                        For i = 0 To xmlnode.ChildNodes.Count - 1
                            localCollection.Add(xmlnode.ChildNodes(i).InnerText, arrName(j))
                            j += 1
                        Next
                    Next

                    Exit Select

                Case enum_Nodes.REQUIREDLOCALSERVER
                    Dim arrName(0) As String
                    j = 0
                    xmlMainNode = rootnode.SelectNodes(objstruct_NodeNames.REQUIREDLOCALSERVER)

                    arrName(0) = "REQUIREDLOCALSERVER"

                    i = -1
                    For Each xmlnode In xmlMainNode
                        For i = 0 To xmlnode.ChildNodes.Count - 1
                            localCollection.Add(xmlnode.ChildNodes(i).InnerText, arrName(j))
                            j += 1
                        Next
                    Next
                    Exit Select

                Case enum_Nodes.FLAG
                    Dim arrName(0) As String
                    j = 0
                    xmlMainNode = rootnode.SelectNodes(objstruct_NodeNames.FLAG)

                    arrName(0) = "FLAG"

                    i = -1
                    For Each xmlnode In xmlMainNode
                        For i = 0 To xmlnode.ChildNodes.Count - 1
                            localCollection.Add(xmlnode.ChildNodes(i).InnerText, arrName(j))
                            j += 1
                        Next
                    Next
                    Exit Select

                Case enum_Nodes.LOGIN
                    xmlMainNode = rootnode.SelectNodes(objstruct_NodeNames.LOGIN)

                    i = -1
                    For Each xmlnode In xmlMainNode
                        For i = 0 To xmlnode.ChildNodes.Count - 1
                            localCollection.Add(xmlnode.ChildNodes(i).InnerText, xmlnode.ChildNodes(i).Name)
                        Next
                    Next

                    Exit Select
            End Select

            '' XMLDoc.Save(strGlobalFullPath)
            refobjCollection = localCollection

            Return True
        Catch ex As Exception
            frmMaster.WriteLog(strErrAppend & "Exception :---" & ex.Message)
            Return False
        End Try
    End Function

    'Private Function funGetServerData(ByVal objNodeName As String, ByRef refobjCollection As Collection) As Boolean
    '    Dim strErrAppend As String = "ConfigrationXML::funGetServerData:---- "
    '    Try
    '        refobjCollection = Nothing
    '        Dim localCollection As New Collection

    '        Dim XMLDoc As New XmlDocument
    '        Dim rootnode As XmlNode = Nothing
    '        Dim xmlnode As XmlNode = Nothing
    '        Dim i, j As Integer
    '        Dim xmlMainNode As XmlNodeList
    '        Dim arrName(8) As String

    '        arrName(0) = "S_IP_ADDRESS"
    '        arrName(1) = "S_NECM_CONNECTION_STRING"
    '        arrName(2) = "S_EOL_PROCESS_CONNECTION_STRING"
    '        arrName(3) = "S_ECU_DETAILS_CONNECTION_STRING"
    '        arrName(4) = "S_TS_UTILITY_CONNECTION_STRING"
    '        arrName(5) = "S_ENGINE_DB_CONNECTION_STRING"
    '        arrName(6) = "S_FTP_USER_NAME"
    '        arrName(7) = "S_FTP_PASSWORD"
    '        arrName(8) = "S_FTP_PORT_NUMBER"

    '        XMLDoc.Load(strGlobalFullPath)
    '        rootnode = XMLDoc.DocumentElement

    '        For j = 0 To arrName.Length - 1
    '            If objNodeName.Contains(objstruct_NodeNames.MAINSERVER) Then
    '                arrName(j) = "M" & arrName(j)
    '            ElseIf objNodeName.Contains(objstruct_NodeNames.LOCALSERVER) Then
    '                arrName(j) = "L" & arrName(j)
    '            End If
    '        Next

    '        i = -1
    '        j = 0
    '        xmlMainNode = rootnode.SelectNodes(objNodeName)

    '        For Each xmlnode In xmlMainNode
    '            For i = 0 To xmlnode.ChildNodes.Count - 1
    '                ''Replace VCID DB Connection for Comman Config file 16R and VCID
    '                Dim xmlLSValue As String = xmlnode.ChildNodes(i).InnerText
    '                If frmMaster.objClsGetData.VCID_Data_Present = True Then
    '                    If xmlLSValue.Contains("NECM_16R") Then
    '                        xmlLSValue = xmlLSValue.Replace("NECM_16R", "NECM_VCID")
    '                    ElseIf xmlLSValue.Contains("DB_EOL_MASTER") Then
    '                        xmlLSValue = xmlLSValue.Replace("DB_EOL_MASTER", "DB_EOL_MASTER_VCID")
    '                    End If
    '                End If
    '                localCollection.Add(xmlLSValue, arrName(j))
    '                'localCollection.Add(xmlnode.ChildNodes(i).InnerText, arrName(j))
    '                j += 1
    '            Next
    '        Next

    '        ''XMLDoc.Save(strGlobalFullPath)
    '        refobjCollection = localCollection

    '        Return True
    '    Catch ex As Exception
    '        frmMaster.WriteLog(strErrAppend & "Exception :---" & ex.Message)
    '        Return False
    '    End Try
    'End Function

#End Region

#Region "[CREATE XML NODES]"

    Private Sub subCommomCreateNode(ByVal objNode As enum_Nodes, ByVal writer As XmlTextWriter, Optional ByVal strNodeName As String = "")
        Select Case objNode
            Case enum_Nodes.LOGIN
                writer.WriteStartElement(objstruct_NodeNames.LOGIN)
                writer.WriteStartElement(objstruct_NodeNames.USERID)
                writer.WriteString(AdminUserName)
                writer.WriteEndElement()
                writer.WriteStartElement(objstruct_NodeNames.PASSWORD)
                writer.WriteString(AdminPassword)
                writer.WriteEndElement()
                writer.WriteStartElement(objstruct_NodeNames.USERIDTS)
                writer.WriteString(TSUserName)
                writer.WriteEndElement()
                writer.WriteStartElement(objstruct_NodeNames.PASSWORDTS)
                writer.WriteString(TSPassword)
                writer.WriteEndElement()
                writer.WriteEndElement()
                Exit Select
            Case enum_Nodes.COMMON
                writer.WriteStartElement(objstruct_NodeNames.COMMON)
                subWriteElement(objstruct_NodeNames.HSPLUSSERIALNO, writer)
                subWriteElement(objstruct_NodeNames.PRINTERADDRESS, writer)
                writer.WriteEndElement()
                Exit Select
            Case enum_Nodes.LOCALMACHINE
                writer.WriteStartElement(strNodeName)
                subWriteElement(objstruct_NodeNames.IP, writer)
                subWriteElement(objstruct_NodeNames.LOCALMACHINEDATABASECONNECTIONSTRING, writer)
                subWriteElement(objstruct_NodeNames.PLANTCODE, writer)
                subWriteElement(objstruct_NodeNames.TCFLINE, writer)
                subWriteElement(objstruct_NodeNames.MACADDRESS, writer)
                writer.WriteEndElement()
                Exit Select
            Case enum_Nodes.REQUIREDLOCALSERVER
                writer.WriteStartElement(objstruct_NodeNames.REQUIREDLOCALSERVER)
                writer.WriteString("0")
                writer.WriteEndElement()
                Exit Select
            Case enum_Nodes.FLAG
                writer.WriteStartElement(objstruct_NodeNames.FLAG)
                writer.WriteString("FALSE")
                writer.WriteEndElement()
                Exit Select
            Case enum_Nodes.SERVERNODE
                writer.WriteStartElement(strNodeName)
                subWriteElement(objstruct_NodeNames.IP, writer)
                subWriteElement(objstruct_NodeNames.NECMDATABASECONNECTIONSTRING, writer)
                subWriteElement(objstruct_NodeNames.EOLPROCESSDATABASECONNECTIONSTRING, writer)
                subWriteElement(objstruct_NodeNames.ECUDETAILSDATABASECONNECTIONSTRING, writer)
                subWriteElement(objstruct_NodeNames.TSUTILITYDATABASECONNECTIONSTRING, writer)
                subWriteElement(objstruct_NodeNames.ENGINEDATABASECONNECTIONSTRING, writer)
                subWriteElement(objstruct_NodeNames.FTPUSERID, writer)
                subWriteElement(objstruct_NodeNames.FTPPASSWORD, writer)
                subWriteElement(objstruct_NodeNames.FTPPORTNUMBER, writer)
                writer.WriteEndElement()
                Exit Select
        End Select
    End Sub

    Private Sub subWriteElement(ByVal elementName As String, ByVal writer As XmlTextWriter)
        writer.WriteStartElement(elementName)
        writer.WriteEndElement()
    End Sub

#End Region

#Region "[GetMACAddress]"
    Public Function GetMACAddress()
        Dim strErrAppend As String = "ConfigrationXML::GetMACAddress:---- "
        frmMaster.WriteLog("---------------------------GetMACAddress-----------------------------------")
        Try
            Dim mgmtClass As New System.Management.ManagementClass("Win32_NetworkAdapterConfiguration")
            Dim mgmtObjectColl As System.Management.ManagementObjectCollection = mgmtClass.GetInstances()
            Dim strMACAddress As String = [String].Empty
            For Each mgmtObject As System.Management.ManagementObject In mgmtObjectColl
                If strMACAddress = [String].Empty Then
                    ' only return MAC Address from first card 
                    If CBool(mgmtObject("IPEnabled")) = True Then
                        strMACAddress = mgmtObject("MacAddress").ToString()
                        Trace.WriteLine(" MacAddress Found")
                    End If
                End If
                mgmtObject.Dispose()
            Next
            strMACAddress = strMACAddress.Replace(":", "")
            Return strMACAddress
        Catch ex As Exception
            frmMaster.WriteLog(strErrAppend & "Exception :---" & ex.Message)
            MessageBox.Show("MAC Address" & " " & ex.Message, "EXCEPTION", MessageBoxButtons.OK)
            Return "wrong"
        End Try

    End Function
#End Region

#Region "Connection String Make/Return Function"

    Public Function funCreateConnectionString(ByVal strdataSourceAsIpAddress As String, ByVal strUserName As String, ByVal strPassword As String, ByVal strDataBaseName As String, ByRef strConnectionString As String) As Boolean
        Dim strErrAppend As String = "ConfigrationXML::funCreateConnectionString:---- "
        Try
            Dim blnErr As Boolean = False
            Dim err As String = ""
            strConnectionString = ""

            If String.IsNullOrEmpty(strdataSourceAsIpAddress) Then
                blnErr = True
                err = "DataBase IP Address, "
            End If

            If String.IsNullOrEmpty(strUserName) Then
                blnErr = True
                err = err & "User Name, "
            End If

            If String.IsNullOrEmpty(strPassword) Then
                blnErr = True
                err = err & "Password, "
            End If

            If String.IsNullOrEmpty(strDataBaseName) Then
                blnErr = True
                err = err & "DataBase Name, "
            End If

            If blnErr = True Then
                MsgBox("Following Fields Can Not be Empty :- " & err)
                Return False
            Else
                strConnectionString = "data source=" & strdataSourceAsIpAddress & ";uid=" & strUserName & ";password=" & strPassword & " ;Initial Catalog=" & strDataBaseName & " ;Connection Timeout=120"
                Return True
            End If
        Catch ex As Exception
            frmMaster.WriteLog(strErrAppend & "Exception :---" & ex.Message)
            Return False
        End Try
    End Function

    Public Function funRecoverIpAddrUidPwdFromConnectionString(ByVal strConnectionString As String, ByRef strdataSourceAsIpAddress As String, ByRef strUserName As String, ByRef strPassword As String, ByRef strDataBaseName As String) As Boolean
        Dim strErrAppend As String = "ConfigrationXML::funRecoverIpAddrUidPwdFromConnectionString:---- "
        Try
            Dim arrStr() As String = strConnectionString.Split(";")
            Dim arrChildStr() As String

            strdataSourceAsIpAddress = ""
            strUserName = ""
            strPassword = ""
            strDataBaseName = ""

            If Not arrStr.Length = 5 Then
                frmMaster.WriteLog(strErrAppend & "Invalid Connection String")
                Return False
            End If

            arrChildStr = arrStr(0).Split("=")
            If funValidateConnAndReturnValue(arrChildStr, "data source", strdataSourceAsIpAddress) = False Then
                Exit Function
            End If
            arrChildStr = arrStr(1).Split("=")
            If funValidateConnAndReturnValue(arrChildStr, "uid", strUserName) = False Then
                Exit Function
            End If
            arrChildStr = arrStr(2).Split("=")
            If funValidateConnAndReturnValue(arrChildStr, "password", strPassword) = False Then
                Exit Function
            End If
            arrChildStr = arrStr(3).Split("=")
            If funValidateConnAndReturnValue(arrChildStr, "Initial Catalog", strDataBaseName) = False Then
                Exit Function
            End If
            frmMaster.WriteLog(strErrAppend & "Values:--- data source=" & strdataSourceAsIpAddress & ";uid=" & strUserName & ";password=" & strPassword & " ;Initial Catalog=" & strConnectionString & " ;Connection Timeout=120")
            Return True
        Catch ex As Exception
            MsgBox("Error :- " & ex.Message)
            frmMaster.WriteLog(strErrAppend & "Exception :---" & ex.Message)
            Return False
        End Try
    End Function

    Private Function funValidateConnAndReturnValue(ByVal strArrChild() As String, ByVal validRef As String, ByRef value As String) As Boolean
        Dim strErrAppend As String = "ConfigrationXML::funValidateConnAndReturnValue:---- "
        Try

            If Not strArrChild(0).Trim.ToLower = validRef.Trim.ToLower Then
                frmMaster.WriteLog(strErrAppend & "Invalid Connection String, Compareing " & strArrChild(0).Trim.ToLower & "," & validRef.Trim.ToLower)
                Return False
            End If

            If String.IsNullOrEmpty(strArrChild(1)) Then
                frmMaster.WriteLog(strErrAppend & "Invalid Connection String Value Is Empty")
                Return False
            End If

            value = strArrChild(1).Trim
            Return True
        Catch ex As Exception
            frmMaster.WriteLog(strErrAppend & "Exception :---" & ex.Message)
            Return False
        End Try

    End Function

#End Region

    Public Function getPLCData() As Collection
        'AddItemToStuctureObject()
        Dim PlcCollection As New Collection
        funGetPlcConfigXmlData(enum_PLCNodes.PLC, PlcCollection)
        objPLCstruct_NodeNames = Nothing
        Return PlcCollection
    End Function

    Public Function funGetPlcConfigXmlData(ByVal objNode As enum_PLCNodes, ByRef refobjCollection As Collection) As Boolean
        Dim strErrAppend As String = "ConfigrationXML::funGetEolConfigXmlData:---- "
        Try
            AddItemToPlcObject()
            refobjCollection = Nothing
            Dim localCollection As New Collection

            Dim XMLDoc As New XmlDocument
            Dim rootnode As XmlNode = Nothing
            Dim xmlnode As XmlNode = Nothing
            XMLDoc.Load(strPlcConfigFullPath)
            rootnode = XMLDoc.DocumentElement

            Dim i, j As Integer
            Dim xmlMainNode As XmlNodeList

            Select Case objNode
                Case enum_PLCNodes.PLC
                    Dim arrName(4) As String
                    j = 0
                    xmlMainNode = rootnode.SelectNodes(objPLCstruct_NodeNames.PLC)
                    'arrName(0) = "PLC"
                    arrName(0) = "PLCBYPASS"
                    arrName(1) = "VOLTAGESELECTIONTAG"
                    arrName(2) = "IGNITIONSWITCHTAG"
                    arrName(3) = "PLCIPADDRESS"
                    arrName(4) = "PLCTOPICNAME"


                    'objPLCstruct_NodeNames.PLC = "PLC"


                    i = 0
                    For Each xmlnode In xmlMainNode
                        For i = 0 To xmlnode.ChildNodes.Count - 1
                            localCollection.Add(xmlnode.ChildNodes(i).InnerText, arrName(j))
                            j += 1
                        Next
                    Next

                    Exit Select


            End Select

            '' XMLDoc.Save(strGlobalFullPath)
            refobjCollection = localCollection

            Return True
        Catch ex As Exception
            frmMaster.WriteLog(strErrAppend & "Exception :---" & ex.Message)
            Return False
        End Try
    End Function

End Class
