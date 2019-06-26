Imports System.Drawing.Text
Imports System.Runtime.InteropServices

Public Class alarm_notif
    Private Sub alarm_notif_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim screenWidth As Integer = Screen.PrimaryScreen.Bounds.Width
        Dim screenHeight As Integer = Screen.PrimaryScreen.Bounds.Height
        Dim newh = screenHeight - Me.Height - TaskbarInfo.Height
        Dim neww = screenWidth - Me.Width
        Me.Location = New Point(neww, newh)
        Dim pfc As New PrivateFontCollection()
        pfc.AddFontFile(Application.StartupPath + "\Passist.MC")
        Label1.Font = New Font(pfc.Families(0), 16)
        Label2.Font = New Font(pfc.Families(0), 12)
        Button1.Font = New Font(pfc.Families(0), 14)
    End Sub
    Public Sub alart(Name As String, time As String)
        time = time.Replace(".", ":")
        Try
            My.Computer.Audio.Play("Passist2.MC", AudioPlayMode.Background)
        Catch ex As Exception
            MsgBox("Passist2.MC File is Missing or bad file", MsgBoxStyle.Critical)
        End Try

        Label1.Text = Name
        Label2.Text = time
        Me.Show()
    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        My.Computer.Audio.Stop()
        Me.Close()
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub
End Class

Public NotInheritable Class TaskbarInfo

    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function FindWindow(ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    End Function

    <DllImport("shell32.dll", SetLastError:=True)>
    Public Shared Function SHAppBarMessage(ByVal dwMessage As ABM, <[In]()> ByRef pData As APPBARDATA) As IntPtr
    End Function

    Enum ABM As UInteger
        [New] = &H0
        Remove = &H1
        QueryPos = &H2
        SetPos = &H3
        GetState = &H4
        GetTaskbarPos = &H5
        Activate = &H6
        GetAutoHideBar = &H7
        SetAutoHideBar = &H8
        WindowPosChanged = &H9
        SetState = &HA
    End Enum

    Enum ABE As UInteger
        Left = 0
        Top = 1
        Right = 2
        Bottom = 3
    End Enum

    <StructLayout(LayoutKind.Sequential)>
    Structure APPBARDATA
        Public cbSize As UInteger
        Public hWnd As IntPtr
        Public uCallbackMessage As UInteger
        Public uEdge As ABE
        Public rc As RECT
        Public lParam As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Structure RECT
        Public left As Integer
        Public top As Integer
        Public right As Integer
        Public bottom As Integer
    End Structure

    Public Shared Function Height() As Integer
        Dim taskbarHandle As IntPtr = FindWindow("Shell_TrayWnd", Nothing)

        Dim data As New APPBARDATA()
        data.cbSize = CUInt(Marshal.SizeOf(GetType(APPBARDATA)))
        data.hWnd = taskbarHandle
        Dim result As IntPtr = SHAppBarMessage(ABM.GetTaskbarPos, data)

        If result = IntPtr.Zero Then
            Throw New InvalidOperationException()
        End If

        Return Rectangle.FromLTRB(data.rc.left, data.rc.top, data.rc.right, data.rc.bottom).Height
    End Function

End Class