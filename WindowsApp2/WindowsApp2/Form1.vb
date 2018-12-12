
Imports System.IO
Imports System.Timers


Public Class Form1

    'initialized neccesary variables
    Dim lblRecieved, lblSent As Long
    Dim lastRecieved As Long = 0
    Dim lastSent As Long = 0
    Dim Loadstr As String = ""
    Dim SecondsPassed As Integer = 0

    'estabish path for directory
    Dim dpath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "/Temp"


    'establish path for file
    Dim path As String = dpath & "/Load.txt"




    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'on Load creates a Directory for files to be saved
        If Not Directory.Exists(dpath) Then
            Directory.CreateDirectory(dpath)
        End If

        'creates file
        If Not IO.File.Exists(path) Then
            Dim fs As IO.FileStream = File.Create(path)

        End If

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick


        Call GetInfo()


        If SecondsPassed > 3 Then
            lblDownload.Text() = Int(lblRecieved - lastRecieved) / 1000 * 8 & " kb/s"

            lblUpload.Text() = Int(lblSent - lastSent) / 1000 * 8 & " kb/s"
            SecondsPassed = 0
        End If
        SecondsPassed += 1
        lastRecieved = lblRecieved
        lastSent = lblSent
    End Sub

    Public Sub GetInfo()
        Dim TempLoadstr As String
        Dim sString() As String
        Dim i As Integer = 0
        Dim word As String

        'runs window shell script to get data sent and recieved


        Shell("cmd.exe /C netstat -e>" & Chr(34) & path & Chr(34), vbHide, vbTrue)

        'if unable to run shell skips to the end
        On Error GoTo PassRead

        Dim objReader As IO.StreamReader
        If IO.File.Exists(path) = True Then
            objReader = IO.File.OpenText(path)

        Else
            MsgBox("Yo, the file isn't here!", , "Error")

        End If


        'Read the file and store info in loadstr
        Do While objReader.Peek <> -1
            i += 1
            TempLoadstr = objReader.ReadLine()

            If i = 5 Then
                Loadstr = TempLoadstr
            End If
        Loop

        'close the StreamReader
        objReader.Close()






        'remove bytes from the string and retain recieved and sent numbers
        Dim charc As Char() = {" "c}
        sString = Loadstr.Split(charc, StringSplitOptions.RemoveEmptyEntries)

        'convert received and sent nummbers to ints based on location in string array
        lblRecieved = Int(sString(1))
        lblSent = Int(sString(2))

PassRead:

    End Sub





End Class
