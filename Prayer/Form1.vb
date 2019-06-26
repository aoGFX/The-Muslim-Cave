Imports System.Drawing.Text
Imports System.Web.Script.Serialization
Public Class Form1
    Dim serializer As JavaScriptSerializer
    Dim jsonresult As String
    Private IsFormBeingDragged As Boolean = False
    Private MouseDownX As Integer
    Private MouseDownY As Integer
    Private Sub mousedowns(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown
        If e.Button = MouseButtons.Left Then
            IsFormBeingDragged = True
            MouseDownX = e.X
            MouseDownY = e.Y
            Me.Cursor = Cursors.Hand
        End If


    End Sub
    Private Sub VlcControl1_MouseUp(sender As Object, e As MouseEventArgs) Handles MyBase.MouseUp
        If e.Button = MouseButtons.Left Then
            IsFormBeingDragged = False
            Me.Cursor = Cursors.Default
        End If
    End Sub
    Private Sub VlcControl1_MouseMove(sender As Object, e As MouseEventArgs) Handles MyBase.MouseMove
        If IsFormBeingDragged Then
            Dim temp As Point = New Point()

            temp.X = Me.Location.X + (e.X - MouseDownX)
            temp.Y = Me.Location.Y + (e.Y - MouseDownY)
            Me.Location = temp
            temp = Nothing
        End If
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim webClient As New System.Net.WebClient
            jsonresult = webClient.DownloadString("http://api.aladhan.com/v1/hijriCalendarByCity?city=cairo&country=Egypt&method=5")

        Catch ex As Exception
            MsgBox("تأكد أنك متصل بالإنترنت")
            Me.Close()
        End Try

        Try
            Dim pfc As New PrivateFontCollection()
            pfc.AddFontFile(Application.StartupPath + "\Passist.MC")
            Label1.Font = New Font(pfc.Families(0), 12)
            Label2.Font = New Font(pfc.Families(0), 12)
            Label3.Font = New Font(pfc.Families(0), 12)
            Label4.Font = New Font(pfc.Families(0), 12)
            Label5.Font = New Font(pfc.Families(0), 12)
            Label6.Font = New Font(pfc.Families(0), 10)
            Label7.Font = New Font(pfc.Families(0), 12)
            Label8.Font = New Font(pfc.Families(0), 12)
            Label9.Font = New Font(pfc.Families(0), 12)
            Label10.Font = New Font(pfc.Families(0), 12)
            Label11.Font = New Font(pfc.Families(0), 22)
            Fajr.Font = New Font(pfc.Families(0), 18)
            Dhuhr.Font = New Font(pfc.Families(0), 18)
            Asr.Font = New Font(pfc.Families(0), 18)
            Maghrib.Font = New Font(pfc.Families(0), 18)
            Isha.Font = New Font(pfc.Families(0), 18)
            Imsak.Font = New Font(pfc.Families(0), 18)
            Sunrise.Font = New Font(pfc.Families(0), 18)
            Sunset.Font = New Font(pfc.Families(0), 18)
            higri.Font = New Font(pfc.Families(0), 14)
        Catch ex As Exception
            MsgBox("Passist.MC is missing or bad file", MsgBoxStyle.Critical)
            alarm_notif.Close()
            Me.Close()

        End Try


        IshaA.BackColor = Color.FromArgb(64, 0, 0, 0)
        MaghribA.BackColor = Color.FromArgb(64, 0, 0, 0)
        AsrA.BackColor = Color.FromArgb(64, 0, 0, 0)
        DhuhrA.BackColor = Color.FromArgb(64, 0, 0, 0)
        FagrA.BackColor = Color.FromArgb(64, 0, 0, 0)
        EmsakA.BackColor = Color.FromArgb(64, 0, 0, 0)
        RiseA.BackColor = Color.FromArgb(64, 0, 0, 0)
        SetA.BackColor = Color.FromArgb(64, 0, 0, 0)

        getdata()

    End Sub
    Dim riseh
    Dim seth
    Private Sub getdata()
        If Not jsonresult = "" Then
            Dim result As Object = New JavaScriptSerializer().Deserialize(Of Object)(jsonresult)
            Dim tod As String = DateTime.Today.Day
            Dim u = 0
            Dim i = 0
            Do Until u = 1
                If result("data")(i)("date")("gregorian")("day") = tod Then
                    u = 1
                    Dim Fajrt As String = result("data")(i)("timings")("Fajr")
                    Dim Dhuhrt As String = result("data")(i)("timings")("Dhuhr")
                    Dim Asrt As String = result("data")(i)("timings")("Asr")
                    Dim Maghribt As String = result("data")(i)("timings")("Maghrib")
                    Dim Ishat As String = result("data")(i)("timings")("Isha")
                    Dim Imsakt As String = result("data")(i)("timings")("Imsak")
                    Dim Sunriset As String = result("data")(i)("timings")("Sunrise")
                    Dim Sunsett As String = result("data")(i)("timings")("Sunset")

                    Fajr.Text = Fajrt.Replace(" (EET)", "").Trim
                    Dhuhr.Text = Dhuhrt.Replace(" (EET)", "").Trim
                    Asr.Text = Asrt.Replace(" (EET)", "").Trim
                    Maghrib.Text = Maghribt.Replace(" (EET)", "").Trim
                    Isha.Text = Ishat.Replace(" (EET)", "").Trim

                    Imsak.Text = Imsakt.Replace(" (EET)", "").Trim
                    Sunrise.Text = Sunriset.Replace(" (EET)", "").Trim
                    Sunset.Text = Sunsett.Replace(" (EET)", "").Trim

                    higri.Text = result("data")(i)("date")("hijri")("date")
                    sourcet.Text = result("data")(i)("meta")("method")("name")
                    riseh = Sunrise.Text.Split(New Char() {":"c})
                    seth = Sunset.Text.Split(New Char() {":"c})
                    setback(riseh(0), riseh(1), seth(0), seth(1))
                Else
                    i = i + 1
                End If
            Loop
        End If
    End Sub

    Private Sub setback(risehour As Integer, risemin As Integer, sethour As Integer, setmin As Integer)
        Dim timenow As Integer = TimeOfDay.Hour
        Dim timenow2 As Integer = TimeOfDay.Minute
        Dim timenowtime As Decimal = timenow & "." & timenow2
        Dim sunrisetime As Decimal = risehour & "." & risemin
        Dim sunsettime As Decimal = sethour & "." & setmin
        If (timenowtime >= sunrisetime) And (timenowtime < sunsettime) Then
            'Morning
            Me.BackgroundImage = My.Resources.back2
        Else
            'Night
            Me.BackgroundImage = My.Resources.back1
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.WindowState = FormWindowState.Minimized


    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        alarm_notif.Close()
        Me.Close()

    End Sub


    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click, Imsak.Click, EmsakA.Click
        Aselect(5)
    End Sub

    Private Sub IshaA_Click(sender As Object, e As EventArgs) Handles Label5.Click, IshaA.Click, Isha.Click
        Aselect(4)
    End Sub

    Private Sub MaghribA_Click(sender As Object, e As EventArgs) Handles MaghribA.Click, Maghrib.Click, Label4.Click
        Aselect(3)
    End Sub

    Private Sub AsrA_Click(sender As Object, e As EventArgs) Handles Label3.Click, AsrA.Click, Asr.Click
        Aselect(2)
    End Sub

    Private Sub DhuhrA_Click(sender As Object, e As EventArgs) Handles Label2.Click, DhuhrA.Click, Dhuhr.Click
        Aselect(1)
    End Sub

    Private Sub FagrA_Click(sender As Object, e As EventArgs) Handles Label1.Click, Fajr.Click, FagrA.Click
        Aselect(0)
    End Sub

    Private Sub SetA_Click(sender As Object, e As EventArgs) Handles Sunset.Click, SetA.Click, Label9.Click
        Aselect(7)
    End Sub

    Private Sub RiseA_Click(sender As Object, e As EventArgs) Handles Sunrise.Click, RiseA.Click, Label8.Click
        Aselect(6)
    End Sub


    Dim FA As Boolean = False
    Dim DA As Boolean = False
    Dim AA As Boolean = False
    Dim MA As Boolean = False
    Dim IA As Boolean = False
    Dim EA As Boolean = False
    Dim SRA As Boolean = False
    Dim SSA As Boolean = False

    Private Sub Aselect(id)

        If id = 0 Then
            If FagrA.BackColor = Color.FromArgb(128, 0, 0, 0) Then
                FagrA.BackColor = Color.FromArgb(64, 0, 0, 0)
                FA = False
            Else
                FagrA.BackColor = Color.FromArgb(128, 0, 0, 0)
                FA = True
            End If

        ElseIf id = 1 Then
            If DhuhrA.BackColor = Color.FromArgb(128, 0, 0, 0) Then
                DhuhrA.BackColor = Color.FromArgb(64, 0, 0, 0)
                DA = False
            Else
                DhuhrA.BackColor = Color.FromArgb(128, 0, 0, 0)
                DA = True
            End If
        ElseIf id = 2 Then
            If AsrA.BackColor = Color.FromArgb(128, 0, 0, 0) Then
                AsrA.BackColor = Color.FromArgb(64, 0, 0, 0)
                AA = False
            Else
                AsrA.BackColor = Color.FromArgb(128, 0, 0, 0)
                AA = True
            End If
        ElseIf id = 3 Then
            If MaghribA.BackColor = Color.FromArgb(128, 0, 0, 0) Then
                MaghribA.BackColor = Color.FromArgb(64, 0, 0, 0)
                MA = False
            Else
                MaghribA.BackColor = Color.FromArgb(128, 0, 0, 0)
                MA = True
            End If
        ElseIf id = 4 Then
            If IshaA.BackColor = Color.FromArgb(128, 0, 0, 0) Then
                IshaA.BackColor = Color.FromArgb(64, 0, 0, 0)
                IA = False
            Else
                IshaA.BackColor = Color.FromArgb(128, 0, 0, 0)
                IA = True
            End If
        ElseIf id = 5 Then
            If EmsakA.BackColor = Color.FromArgb(128, 0, 0, 0) Then
                EmsakA.BackColor = Color.FromArgb(64, 0, 0, 0)
                EA = False
            Else
                EmsakA.BackColor = Color.FromArgb(128, 0, 0, 0)
                EA = True
            End If
        ElseIf id = 6 Then
            If RiseA.BackColor = Color.FromArgb(128, 0, 0, 0) Then
                RiseA.BackColor = Color.FromArgb(64, 0, 0, 0)
                SRA = False
            Else
                RiseA.BackColor = Color.FromArgb(128, 0, 0, 0)
                SRA = True
            End If
        ElseIf id = 7 Then
            If SetA.BackColor = Color.FromArgb(128, 0, 0, 0) Then
                SetA.BackColor = Color.FromArgb(64, 0, 0, 0)
                SSA = False
            Else
                SetA.BackColor = Color.FromArgb(128, 0, 0, 0)
                SSA = True
            End If
        End If

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If FA = True Then
            Dim AlarmTime As Decimal = Fajr.Text.Replace(":", ".")
            Dim Timenoow As Decimal = Now.Hour & "." & Now.Minute
            If Timenoow = AlarmTime Then
                alarm_notif.alart("صلاة الفجر", AlarmTime)
                Timer1.Enabled = False
                Timer2.Enabled = True
            End If
        End If
        If DA = True Then
            Dim AlarmTime As Decimal = Dhuhr.Text.Replace(":", ".")
            Dim Timenoow As Decimal = Now.Hour & "." & Now.Minute
            If Timenoow = AlarmTime Then
                alarm_notif.alart("صلاة الظهر", AlarmTime)
                Timer1.Enabled = False
                Timer2.Enabled = True
            End If
        End If
        If AA = True Then
            Dim AlarmTime As Decimal = Asr.Text.Replace(":", ".")
            Dim Timenoow As Decimal = Now.Hour & "." & Now.Minute
            If Timenoow = AlarmTime Then
                alarm_notif.alart("صلاة العصر", AlarmTime)
                Timer1.Enabled = False
                Timer2.Enabled = True
            End If
        End If
        If MA = True Then
            Dim AlarmTime As Decimal = Maghrib.Text.Replace(":", ".")
            Dim Timenoow As Decimal = Now.Hour & "." & Now.Minute
            If Timenoow = AlarmTime Then
                alarm_notif.alart("صلاة المغرب", AlarmTime)
                Timer1.Enabled = False
                Timer2.Enabled = True
            End If
        End If
        If IA = True Then
            Dim AlarmTime As Decimal = Isha.Text.Replace(":", ".")
            Dim Timenoow As Decimal = Now.Hour & "." & Now.Minute
            If Timenoow = AlarmTime Then
                alarm_notif.alart("صلاة العشاء", AlarmTime)
                Timer1.Enabled = False
                Timer2.Enabled = True
            End If
        End If
        If EA = True Then
            Dim AlarmTime As Decimal = Imsak.Text.Replace(":", ".")
            Dim Timenoow As Decimal = Now.Hour & "." & Now.Minute
            If Timenoow = AlarmTime Then
                alarm_notif.alart("الإمسـاك", AlarmTime)
                Timer1.Enabled = False
                Timer2.Enabled = True
            End If
        End If
        If SRA = True Then
            Dim AlarmTime As Decimal = Sunrise.Text.Replace(":", ".")
            Dim Timenoow As Decimal = Now.Hour & "." & Now.Minute
            If Timenoow = AlarmTime Then
                alarm_notif.alart("شروق الشمس", AlarmTime)
                Timer1.Enabled = False
                Timer2.Enabled = True
            End If
        End If
        If SSA = True Then
            Dim AlarmTime As Decimal = Sunset.Text.Replace(":", ".")
            Dim Timenoow As Decimal = Now.Hour & "." & Now.Minute
            If Timenoow = AlarmTime Then
                alarm_notif.alart("غروب الشمس", AlarmTime)
                Timer1.Enabled = False
                Timer2.Enabled = True
            End If
        End If
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Timer1.Enabled = True
        Timer2.Enabled = False
    End Sub

    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        setback(riseh(0), riseh(1), seth(0), seth(1))
    End Sub

    Private Sub Timer4_Tick(sender As Object, e As EventArgs) Handles Timer4.Tick
        Timer4.Enabled = False
        MsgBox("صدقه جاريه علي روح المرحوم : حسام الدين محمد" & vbNewLine & "أدعوله بالرحمه والمغفرة")
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://github.com/aoGFX/The-Muslim-Cave")
    End Sub
End Class
