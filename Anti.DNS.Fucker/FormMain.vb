﻿Imports System.IO
Imports System.Runtime.Serialization.Formatters
Imports System.Runtime.Serialization.Formatters.Binary

Public Class FormMain
    Private TableLayoutPanelHead As TableLayoutPanel
    Private CheckBoxHeadSelectAll As CheckBox
    Private LabelHeadDomainName As Label
    Private LabelHeadGetIPv6Address As Label
    Private LabelHeadGetIPv4Address As Label
    Private LabelHeadEnable As Label

    Private TableLayoutPanelList As TableLayoutPanelWithDoubleBuffer

    Private ToolStrip As ToolStrip
    Private ToolStripLabelTitle As ToolStripLabel
    Private ToolStripButtonAdd As ToolStripButton
    Private ToolStripButtonRemove As ToolStripButton
    Private ToolStripButtonQuit As ToolStripButton
    Private ToolStripButtonSaveAs As ToolStripButton
    Private ToolStripButtonEnable As ToolStripButton
    Private ToolStripButtonDisable As ToolStripButton
    Private ToolStripButtonIPv4Enable As ToolStripButton
    Private ToolStripButtonIPv6Enable As ToolStripButton
    Private ToolStripButtonIPv6Disable As ToolStripButton
    Private ToolStripButtonIPv4Disable As ToolStripButton
    Private ToolStripButtonOpen As ToolStripButton
    Private ToolStripButtonRun As ToolStripButton

    Private StatusStrip As StatusStrip

    Private TopPadding As Padding = New Padding(0, 7, 0, 0)
    Private CheckBoxPadding As Padding = New Padding(4, 0, 0, 0)
    Private TransparentColor As Color = Color.FromArgb(0, Color.Black)
    Private MouseDownLocation As Point
    Private FormLastLocation As Point
    Private ItemHeight As Integer = 27

    Private Configuration As Configuration
    Private ConfigurationPath As String = Application.StartupPath & "\Configuration.xml"

    Private ThreadBackground As Threading.Thread

    Public Sub New()
        InitializeComponent()

        With Me
            .Width = 420
            .Height = 400
            .Text = "Anti-DNSFucker"
            .ControlBox = False
            .FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        End With

        Configuration = New Configuration
        If Configuration.Load(ConfigurationPath) Then
        End If

        InitializeToolStrip()
        InitializeStatusStrip()
        InitializeTableLayoutPanelHead()
        InitializeTableLayoutPanelList()

        ToolStripButtonUpdate()

        CheckForIllegalCrossThreadCalls = False

        ThreadBackground = New Threading.Thread(AddressOf BackgroundProcess)
        ThreadBackground.Start()
    End Sub

    Private Sub BackgroundProcess()
        With TableLayoutPanelList
            For i As Integer = 0 To .RowCount - 1
                If Not CType(.GetControlFromPosition(0, i), CheckBox).Checked Then
                    Continue For
                End If

                If Not CType(.GetControlFromPosition(0, i), CheckBox).Tag Is Nothing Then
                    Continue For
                End If

                Dim IP As New IP
                IP.Resolve(CType(.GetControlFromPosition(2, i), TextBox).Text)
                CType(.GetControlFromPosition(0, i), CheckBox).Tag = IP

            Next
        End With
    End Sub

    Private Sub InitializeToolStrip()
        ToolStripLabelTitle = New ToolStripLabel
        With ToolStripLabelTitle
            .Text = Me.Text

            AddHandler .MouseDown, AddressOf ToolStrip_MouseDown
            AddHandler .MouseUp, AddressOf ToolStrip_MouseUp
            AddHandler .MouseMove, AddressOf ToolStrip_MouseMove
        End With

        ToolStripButtonAdd = New ToolStripButton
        With ToolStripButtonAdd
            .Image = Icons.Add
            .ToolTipText = "Add New Domain Name"
            AddHandler .Click, AddressOf ToolStripButton_Click
        End With

        ToolStripButtonRemove = New ToolStripButton
        With ToolStripButtonRemove
            .Image = Icons.Remove
            .ToolTipText = "Remove Selected Items"
            AddHandler .Click, AddressOf ToolStripButton_Click
        End With

        ToolStripButtonQuit = New ToolStripButton
        With ToolStripButtonQuit
            .Image = Icons.Quit
            .ToolTipText = "Quit"
            AddHandler .Click, AddressOf ToolStripButton_Click
        End With

        ToolStripButtonSaveAs = New ToolStripButton
        With ToolStripButtonSaveAs
            .Image = Icons.SaveAs
            .ToolTipText = "Save Configuration to File"
            AddHandler .Click, AddressOf ToolStripButton_Click
        End With

        ToolstripButtonEnable = New ToolStripButton
        With ToolstripButtonEnable
            .Image = Icons.Enable
            .ToolTipText = "Enable Selected Items"
            AddHandler .Click, AddressOf ToolStripButton_Click
        End With

        ToolstripButtonDisable = New ToolStripButton
        With ToolstripButtonDisable
            .Image = Icons.Disable
            .ToolTipText = "Disable Selected Items"
            AddHandler .Click, AddressOf ToolStripButton_Click
        End With

        ToolStripButtonIPv4Enable = New ToolStripButton
        With ToolStripButtonIPv4Enable
            .Image = Icons.IPv4Enable
            .ToolTipText = "Get IPv4 Addresses"
            AddHandler .Click, AddressOf ToolStripButton_Click
        End With

        ToolStripButtonIPv4Disable = New ToolStripButton
        With ToolStripButtonIPv4Disable
            .Image = Icons.IPv4Disable
            .ToolTipText = "Don't Get IPv4 Addresses"
            AddHandler .Click, AddressOf ToolStripButton_Click
        End With

        ToolStripButtonIPv6Enable = New ToolStripButton
        With ToolStripButtonIPv6Enable
            .Image = Icons.IPv6Enable
            .ToolTipText = "Get IPv6 Addresses"
            AddHandler .Click, AddressOf ToolStripButton_Click
        End With

        ToolStripButtonIPv6Disable = New ToolStripButton
        With ToolStripButtonIPv6Disable
            .Image = Icons.IPv6Disable
            .ToolTipText = "Don't Get IPv6 Addresses"
            AddHandler .Click, AddressOf ToolStripButton_Click
        End With

        ToolStripButtonOpen = New ToolStripButton
        With ToolStripButtonOpen
            .Image = Icons.Open
            .ToolTipText = "Open Configuration"
            AddHandler .Click, AddressOf ToolStripButton_Click
        End With

        ToolStripButtonRun = New ToolStripButton
        With ToolStripButtonRun
            .Image = Icons.Run
            .ToolTipText = "Run"
            AddHandler .Click, AddressOf ToolStripButton_Click
        End With


        ToolStrip = New ToolStrip
        With ToolStrip
            .GripStyle = ToolStripGripStyle.Hidden
            .RenderMode = ToolStripRenderMode.Professional
            .Parent = Me
            .BackColor = Color.Transparent

            .Items.Add(ToolStripLabelTitle)
            .Items.Add(New ToolStripSeparator)
            .Items.Add(ToolStripButtonAdd)
            .Items.Add(ToolStripButtonRemove)
            .Items.Add(New ToolStripSeparator)
            .Items.Add(ToolstripButtonEnable)
            .Items.Add(ToolstripButtonDisable)
            .Items.Add(New ToolStripSeparator)
            .Items.Add(ToolStripButtonIPv4Enable)
            .Items.Add(ToolStripButtonIPv4Disable)
            .Items.Add(ToolStripButtonIPv6Enable)
            .Items.Add(ToolStripButtonIPv6Disable)
            .Items.Add(New ToolStripSeparator)
            .Items.Add(ToolStripButtonOpen)
            .Items.Add(ToolStripButtonSaveAs)
            .Items.Add(New ToolStripSeparator)
            .Items.Add(ToolStripButtonRun)
            .Items.Add(New ToolStripSeparator)
            .Items.Add(ToolStripButtonQuit)

            AddHandler .MouseDown, AddressOf ToolStrip_MouseDown
            AddHandler .MouseUp, AddressOf ToolStrip_MouseUp
            AddHandler .MouseMove, AddressOf ToolStrip_MouseMove
        End With
    End Sub

    Private Sub ToolStripButton_Click(sender As Object, e As EventArgs)
        If sender Is ToolStripButtonQuit Then
            SaveConfiguration()
            End
        End If

        If sender Is ToolStripButtonAdd Then
            AddNewItem(True)
            Exit Sub
        End If

        If sender Is ToolStripButtonRemove Then
            RemoveSelectedItems()
            Exit Sub
        End If

        If sender Is ToolStripButtonEnable Then
            SetEnableOfSelectedItems(True)
            Exit Sub
        End If

        If sender Is ToolStripButtonDisable Then
            SetEnableOfSelectedItems(False)
            Exit Sub
        End If

        If sender Is ToolStripButtonIPv4Enable Then
            SetIPvXEnableOfSelectedItems(4, True)
            Exit Sub
        End If

        If sender Is ToolStripButtonIPv4Disable Then
            SetIPvXEnableOfSelectedItems(4, False)
            Exit Sub
        End If

        If sender Is ToolStripButtonIPv6Enable Then
            SetIPvXEnableOfSelectedItems(6, True)
            Exit Sub
        End If

        If sender Is ToolStripButtonIPv6Disable Then
            SetIPvXEnableOfSelectedItems(6, False)
            Exit Sub
        End If

        If sender Is ToolStripButtonSaveAs Then
            SaveConfigurationToFile()
            Exit Sub
        End If

        If sender Is ToolStripButtonOpen Then
            LoadConfigurationFromFile
            Exit Sub
        End If
    End Sub

    Private Sub SaveConfigurationToFile()
        Dim SaveFileDialog As New SaveFileDialog
        With SaveFileDialog
            .FileName = ""
            .Filter = "Configuration File|*.cfg"
        End With

        If Not SaveFileDialog.ShowDialog = DialogResult.OK Then
            Exit Sub
        End If

        SaveConfiguration(SaveFileDialog.FileName)
    End Sub

    Private Sub LoadConfigurationFromFile()
        Dim OpenFileDialog As New OpenFileDialog
        With OpenFileDialog
            .FileName = ""
            .Filter = "Configuration File|*.cfg"
            .Multiselect = False
        End With

        If Not OpenFileDialog.ShowDialog = DialogResult.OK Then
            Exit Sub
        End If

        Configuration = New Configuration
        If Not Configuration.Load(OpenFileDialog.FileName) Then
            Exit Sub
        End If

        Configuration.ConfigFileFullName = ConfigurationPath
        FillTableLayoutPanelListFromConfiguration()
    End Sub

    Private Sub FillTableLayoutPanelListFromConfiguration()
        Dim DataTable As New DataTable
        If Not Configuration.GetConfig(TableNames.DomainNameList, DataTable) Then
            Exit Sub
        End If

        With TableLayoutPanelList
            .Controls.Clear()
            .RowStyles.Clear()
            .RowCount = 0

            For Each Row As DataRow In DataTable.Rows
                AddNewItem()
                With TableLayoutPanelList
                    CType(.GetControlFromPosition(1, .RowCount - 1), CheckBox).Checked = Row(LabelHeadEnable.Text)
                    CType(.GetControlFromPosition(2, .RowCount - 1), TextBox).Text = Row(LabelHeadDomainName.Text)
                    CType(.GetControlFromPosition(3, .RowCount - 1), CheckBox).Checked = Row(LabelHeadGetIPv4Address.Text)
                    CType(.GetControlFromPosition(4, .RowCount - 1), CheckBox).Checked = Row(LabelHeadGetIPv6Address.Text)
                End With
            Next
        End With
    End Sub

    Private Sub SetIPvXEnableOfSelectedItems(ByVal IPvX As Integer, ByVal Enable As Boolean)
        Dim ColumnIndex As Integer = If(IPvX = 4, 3, 4)

        With TableLayoutPanelList
            For i As Integer = 0 To .RowCount - 1
                If Not CType(.GetControlFromPosition(0, i), CheckBox).Checked Then
                    Continue For
                End If

                CType(.GetControlFromPosition(ColumnIndex, i), CheckBox).Checked = Enable
            Next
        End With
    End Sub

    Private Sub SetEnableOfSelectedItems(ByVal Enable As Boolean)
        With TableLayoutPanelList
            For i As Integer = 0 To .RowCount - 1
                If Not CType(.GetControlFromPosition(0, i), CheckBox).Checked Then
                    Continue For
                End If

                CType(.GetControlFromPosition(1, i), CheckBox).Checked = Enable
            Next
        End With

    End Sub


    Private Sub SaveConfiguration(Optional Path As String = "")
        Dim DataTable As New DataTable
        DataTable.TableName = TableNames.DomainNameList
        DataTable.Columns.Add(LabelHeadEnable.Text)
        DataTable.Columns.Add(LabelHeadDomainName.Text)
        DataTable.Columns.Add(LabelHeadGetIPv4Address.Text)
        DataTable.Columns.Add(LabelHeadGetIPv6Address.Text)

        For i As Integer = 0 To TableLayoutPanelList.RowCount - 1
            Dim Row(3) As String
            Row(0) = CType(TableLayoutPanelList.GetControlFromPosition(1, i), CheckBox).Checked
            Row(1) = CType(TableLayoutPanelList.GetControlFromPosition(2, i), TextBox).Text
            Row(2) = CType(TableLayoutPanelList.GetControlFromPosition(3, i), CheckBox).Checked
            Row(3) = CType(TableLayoutPanelList.GetControlFromPosition(4, i), CheckBox).Checked
            DataTable.Rows.Add(Row)
        Next

        Configuration.SetConfig(DataTable)

        If Path.Trim = "" Then
            Configuration.Save()
        Else
            Configuration.SaveAs(Path)
        End If
    End Sub

    Private Sub AddNewItem(Optional ByVal Enable As Boolean = False)
        With TableLayoutPanelList
            .SuspendLayout()
            .RowStyles.Add(New RowStyle(SizeType.Absolute, ItemHeight))

            Dim CheckBoxSelected As New CheckBox
            With CheckBoxSelected
                .Text = ""
                .Padding = CheckBoxPadding
                AddHandler .CheckedChanged, AddressOf CheckBoxSelected_CheckedChanged
            End With

            Dim CheckBoxEnable As New CheckBox
            With CheckBoxEnable
                .Text = ""
                .Padding = CheckBoxPadding
                .Checked = Enable
                AddHandler .CheckedChanged, AddressOf CheckBoxEnable_CheckedChanged
            End With

            Dim TextBoxDomainName As New TextBoxWithWaterMark
            With TextBoxDomainName
                .WaterMarkText = "Please Input Domain Name."
                .Dock = DockStyle.Fill
                .Enabled = Enable
            End With

            Dim CheckBoxGetIPv4Address As New CheckBox
            With CheckBoxGetIPv4Address
                .Text = ""
                .Padding = CheckBoxPadding
                .Enabled = Enable
            End With

            Dim CheckBoxGetIPv6Address As New CheckBox
            With CheckBoxGetIPv6Address
                .Text = ""
                .Padding = CheckBoxPadding
                .Enabled = Enable
            End With

            Dim Index As Integer = .RowCount
            .Controls.Add(CheckBoxSelected, 0, Index)
            .Controls.Add(CheckBoxEnable, 1, Index)
            .Controls.Add(TextBoxDomainName, 2, Index)
            .Controls.Add(CheckBoxGetIPv4Address, 3, Index)
            .Controls.Add(CheckBoxGetIPv6Address, 4, Index)

            .RowCount += 1
            Dim MaxHeight As Integer = Me.ClientSize.Height - TableLayoutPanelHead.Height - ToolStrip.Height - StatusStrip.Height
            .Height = .RowCount * ItemHeight
            If .Height > MaxHeight Then
                .Height = MaxHeight
            End If

            .ResumeLayout()
            .ScrollControlIntoView(CheckBoxSelected)
        End With
    End Sub

    Private Sub CheckBoxSelected_CheckedChanged(sender As Object, e As EventArgs)
        If TableLayoutPanelList.RowCount = 0 Then
            CheckBoxHeadSelectAll.Checked = False
            Exit Sub
        End If

        RemoveHandler CheckBoxHeadSelectAll.CheckedChanged, AddressOf CheckBoxHeadSelectAll_CheckedChanged

        Dim CurrentCheckState As Boolean = CType(TableLayoutPanelList.GetControlFromPosition(0, 0), CheckBox).Checked
        Dim IsConsistent As Boolean = True

        For i As Integer = 1 To TableLayoutPanelList.RowCount - 1
            If Not CurrentCheckState = CType(TableLayoutPanelList.GetControlFromPosition(0, i), CheckBox).Checked Then
                IsConsistent = False
                Exit For
            End If
        Next

        If IsConsistent Then
            CheckBoxHeadSelectAll.CheckState = If(CurrentCheckState, CheckState.Checked, CheckState.Unchecked)
            CheckBoxHeadSelectAll.Checked = CurrentCheckState
        Else
            CheckBoxHeadSelectAll.CheckState = CheckState.Indeterminate
        End If

        AddHandler CheckBoxHeadSelectAll.CheckedChanged, AddressOf CheckBoxHeadSelectAll_CheckedChanged

        ToolStripButtonUpdate()
    End Sub

    Private Sub ToolStripButtonUpdate()
        Dim Enable As Boolean = ExistSelectedItems()
        ToolStripButtonRemove.Enabled = Enable
        ToolStripButtonEnable.Enabled = Enable
        ToolStripButtonDisable.Enabled = Enable
        ToolStripButtonIPv4Enable.Enabled = Enable
        ToolStripButtonIPv4Disable.Enabled = Enable
        ToolStripButtonIPv6Enable.Enabled = Enable
        ToolStripButtonIPv6Disable.Enabled = Enable
    End Sub

    Private Sub CheckBoxEnable_CheckedChanged(sender As Object, e As EventArgs)
        For i As Integer = 0 To TableLayoutPanelList.RowCount - 1
            If sender Is TableLayoutPanelList.GetControlFromPosition(1, i) Then
                TableLayoutPanelList.GetControlFromPosition(2, i).Enabled = CType(sender, CheckBox).Checked
                TableLayoutPanelList.GetControlFromPosition(3, i).Enabled = CType(sender, CheckBox).Checked
                TableLayoutPanelList.GetControlFromPosition(4, i).Enabled = CType(sender, CheckBox).Checked

                TableLayoutPanelList.Refresh()
            End If
        Next
    End Sub

    Private Sub ToolStrip_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs)
        ' Change the cursor shape.
        Me.Cursor = Cursors.SizeAll
        ' Save the location of mouse.
        Me.MouseDownLocation = e.Location
        ' Save the location of FormMain.
        Me.FormLastLocation = Me.Location
    End Sub

    Private Sub ToolStrip_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs)
        ' Recover the cursor shape.
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub ToolStrip_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs)
        ' If the mouse button is not pressed, exit this sub.
        If Not Me.Cursor Is Cursors.SizeAll Then
            Exit Sub
        End If

        ' Update the location of the FormMain.
        Me.Location = New Point(Me.Location.X - Me.MouseDownLocation.X + e.Location.X,
                                Me.Location.Y - Me.MouseDownLocation.Y + e.Location.Y)
    End Sub

    Private Sub InitializeStatusStrip()
        StatusStrip = New StatusStrip
        With StatusStrip
            .Parent = Me
            .SizingGrip = False
        End With
    End Sub

    Private Sub InitializeTableLayoutPanelHead()
        CheckBoxHeadSelectAll = New CheckBox
        With CheckBoxHeadSelectAll
            .Text = ""
            '.ThreeState = True
            .Padding = CheckBoxPadding
            AddHandler .CheckedChanged, AddressOf CheckBoxHeadSelectAll_CheckedChanged
        End With

        LabelHeadDomainName = New Label
        With LabelHeadDomainName
            .Text = "Domain Name"
            .Padding = TopPadding
            .BackColor = TransparentColor
        End With

        LabelHeadGetIPv4Address = New Label
        With LabelHeadGetIPv4Address
            .Text = "Get IPv4"
            .Padding = TopPadding
            .BackColor = TransparentColor
        End With

        LabelHeadGetIPv6Address = New Label
        With LabelHeadGetIPv6Address
            .Text = "Get IPv6"
            .Padding = TopPadding
            .BackColor = TransparentColor
        End With

        LabelHeadEnable = New Label
        With LabelHeadEnable
            .Text = "Enable"
            .Padding = TopPadding
            .BackColor = TransparentColor
        End With

        TableLayoutPanelHead = New TableLayoutPanel
        With TableLayoutPanelHead
            .Width = Me.ClientSize.Width - System.Windows.Forms.SystemInformation.VerticalScrollBarWidth
            .Height = ItemHeight
            .BackColor = Me.BackColor
            .Location = New Point(0, ToolStrip.Height)
            .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
            '.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single

            .RowStyles.Add(New RowStyle(SizeType.Absolute, .Height))
            .ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 1))
            .ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 2))
            .ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 5))
            .ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 2))
            .ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 2))
            .ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, System.Windows.Forms.SystemInformation.VerticalScrollBarWidth))
            .Controls.Add(CheckBoxHeadSelectAll, 0, 0)
            .Controls.Add(LabelHeadEnable, 1, 0)
            .Controls.Add(LabelHeadDomainName, 2, 0)
            .Controls.Add(LabelHeadGetIPv4Address, 3, 0)
            .Controls.Add(LabelHeadGetIPv6Address, 4, 0)
            .Parent = Me

            AddHandler .Paint, AddressOf TableLayoutPanelHead_Paint
        End With
    End Sub

    Private Sub CheckBoxHeadSelectAll_CheckedChanged(sender As Object, e As EventArgs)
        For i As Integer = 0 To TableLayoutPanelList.RowCount - 1
            With CType(TableLayoutPanelList.GetControlFromPosition(0, i), CheckBox)
                RemoveHandler .CheckedChanged, AddressOf CheckBoxSelected_CheckedChanged
                .Checked = CheckBoxHeadSelectAll.Checked
                AddHandler .CheckedChanged, AddressOf CheckBoxSelected_CheckedChanged
            End With
        Next
        ToolStripButtonUpdate()
    End Sub

    Private Sub TableLayoutPanelHead_Paint(sender As Object, e As PaintEventArgs)
        Dim Graphics As Graphics = TableLayoutPanelHead.CreateGraphics
        Dim Pen As New Pen(Color.LightGray, 1)
        Dim LeftPadding As Integer = 0

        With TableLayoutPanelHead
            Graphics.DrawLine(Pen, New Point(0, .Height - 1), New Point(.Width, .Height - 1))
            ' Graphics.DrawLine(Pen, New Point(0, -1), New Point(.Width, -1))

            For i As Integer = 0 To .ColumnStyles.Count - 2
                LeftPadding += .GetColumnWidths(i)
                Graphics.DrawLine(Pen, New Point(LeftPadding, .Height - 1), New Point(LeftPadding, 0))
            Next
        End With
    End Sub

    Private Sub TableLayoutPanelList_Paint(sender As Object, e As PaintEventArgs)
        With TableLayoutPanelList
            .SuspendLayout()
            Dim ColumnWidths() As Integer = .GetColumnWidths()
            If Not ColumnWidths.Count = .ColumnStyles.Count - 1 Then
                Exit Sub
            End If

            Dim Graphics As Graphics = TableLayoutPanelList.CreateGraphics
            Dim Pen As New Pen(Color.LightGray, 1)
            Dim LeftPadding As Integer = 0

            For i As Integer = 0 To ColumnWidths.Count - 2
                LeftPadding += ColumnWidths(i)
                Graphics.DrawLine(Pen, New Point(LeftPadding, .Height - 1), New Point(LeftPadding, 0))
            Next
            .ResumeLayout()
        End With
    End Sub

    Private Sub InitializeTableLayoutPanelList()
        TableLayoutPanelList = New TableLayoutPanelWithDoubleBuffer
        With TableLayoutPanelList
            '.BackColor = Color.Red
            .Location = New Point(0, TableLayoutPanelHead.Height + ToolStrip.Height)
            .Width = Me.ClientSize.Width
            '.Controls.Clear()
            '.RowStyles.Clear()
            '.RowCount = 0

            '.Height = Me.ClientSize.Height - TableLayoutPanelHead.Height - ToolStrip.Height - StatusStrip.Height
            .Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right
            '.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            '.BorderStyle = BorderStyle.FixedSingle

            .HorizontalScroll.Maximum = 0
            .AutoScroll = False
            .HorizontalScroll.Visible = False
            .HorizontalScroll.Enabled = False
            .AutoScroll = True

            For Each ColumnStyle As ColumnStyle In TableLayoutPanelHead.ColumnStyles
                .ColumnStyles.Add(New ColumnStyle(ColumnStyle.SizeType, ColumnStyle.Width))
            Next

            .ColumnCount = 5

            AddHandler .ClientSizeChanged, AddressOf TableLayoutPanelList_ClientSizeChanged
            'AddHandler .Paint, AddressOf TableLayoutPanelList_Paint
            .Parent = Me
        End With

        FillTableLayoutPanelListFromConfiguration()
    End Sub

    Private Sub TableLayoutPanelList_ClientSizeChanged()
        With TableLayoutPanelList
            If .VerticalScroll.Visible = True Then
                .Padding = New Padding(0)
            Else
                .Padding = New Padding(0, 0, SystemInformation.VerticalScrollBarWidth, 0)
            End If
        End With
    End Sub

    Private Sub RemoveSelectedItems()
        Dim RowCount As Integer = 0

        With TableLayoutPanelList
            For i As Integer = 0 To .RowCount - 1
                If Not CType(.GetControlFromPosition(0, i), CheckBox).Checked Then
                    Continue For
                End If

                For j As Integer = 0 To .ColumnCount - 1
                    .Controls.Remove(.GetControlFromPosition(j, i))
                Next

                RowCount += 1
            Next

            For i As Integer = 0 To .RowCount - 1
                If Not .GetControlFromPosition(0, i) Is Nothing Then
                    Continue For
                End If

                Dim NextNonEmptyIndex As Integer = NextNonEmptyIndexOfTableLayoutPanelList(i)
                If NextNonEmptyIndex < 0 Then
                    Exit For
                End If

                For j As Integer = 0 To .ColumnCount - 1
                    .SetRow(.GetControlFromPosition(j, NextNonEmptyIndex), i)
                Next
            Next

            For i As Integer = 0 To RowCount - 1
                .RowStyles.RemoveAt(RowCount - 1 - i)
                .RowCount -= 1
            Next

            Dim Position As Point = .AutoScrollPosition
            .AutoScroll = False
            .AutoScroll = True
            .AutoScrollPosition = New Point(-Position.X, -Position.Y)
        End With

        ToolStripButtonUpdate()
        CheckBoxSelected_CheckedChanged(Nothing, Nothing)
    End Sub

    Private Function NextNonEmptyIndexOfTableLayoutPanelList(ByVal StartIndex As Integer) As Integer
        For i As Integer = StartIndex + 1 To TableLayoutPanelList.RowCount - 1
            If Not TableLayoutPanelList.GetControlFromPosition(0, i) Is Nothing Then
                Return i
            End If
        Next

        Return -1
    End Function

    Private Function ExistSelectedItems() As Boolean
        With TableLayoutPanelList
            For i As Integer = 0 To .RowCount - 1
                If CType(.GetControlFromPosition(0, i), CheckBox).Checked Then
                    Return True
                End If
            Next
        End With

        Return False
    End Function

End Class
