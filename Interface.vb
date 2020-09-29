'This module's imports and settings.
Option Compare Binary
Option Explicit On
Option Infer Off
Option Strict On

Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Environment
Imports System.Media
Imports System.Windows.Forms

'This module contains this program's main interface.
Public Class InterfaceWindow
   Private CurrentImage As Bitmap = Nothing  'Contains the current image.
   Private StartTime As DateTime = Nothing   'Contains the moment at which the resizing of an image was started.

   Private WithEvents Resizer As New BackgroundWorker With {.WorkerReportsProgress = False, .WorkerSupportsCancellation = False} 'Contains the backgroundworker that handles the image resizing.

   'This procedure initializes this window.
   Public Sub New()
      Try
         InitializeComponent()

         DisplayStatus()

         With My.Computer.Screen.WorkingArea
            Me.Size = New Size(CInt(.Width / 1.5), CInt(.Height / 1.5))
         End With

         My.Application.ChangeCulture("en-US")

         CurrentImageBox.Size = Me.ClientSize

         CurrentImage = New Bitmap(CurrentImageBox.Width, CurrentImageBox.Height)
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure displays information about this program and help.
   Private Sub HelpMenu_Click(sender As Object, e As EventArgs) Handles HelpMenu.Click
      Try
         MessageBox.Show($"{My.Application.Info.Description}{NewLine}{NewLine}" &
                        $"Help - keys:{NewLine}" &
                        $"H = horizontal resizing.{NewLine}" &
                        $"P = paste image from clipboard.{NewLine}" &
                        $"L = load an image.{NewLine}" &
                        $"V = vertical resizing.{NewLine}{NewLine}" &
                        $"Resizing - specify:...{NewLine}" &
                        $"...values less than zero to ""remove"".{NewLine}" &
                        "...values greater than zero to ""add"".",
                        My.Application.Info.Title, MessageBoxButtons.OK, MessageBoxIcon.Information)
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure handles the user's key strokes.
   Private Sub InterfaceWindow_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
      Try
         Dim FileDialog As New OpenFileDialog

         If Not Resizer.IsBusy Then
            Select Case e.KeyChar.ToString().ToUpper()
               Case "L"
                  If FileDialog.ShowDialog = DialogResult.OK Then
                     CurrentImage = New Bitmap(FileDialog.FileName)
                     CurrentImageBox.Image = CurrentImage
                     DisplayStatus()
                  End If
               Case "P"
                  If My.Computer.Clipboard.ContainsImage Then
                     CurrentImage = New Bitmap(My.Computer.Clipboard.GetImage)
                     CurrentImageBox.Image = CurrentImage
                     DisplayStatus()
                  End If
               Case "H", "V"
                  Me.Cursor = Cursors.WaitCursor

                  Resizer.RunWorkerAsync(e.KeyChar)
                  Do While (Resizer.IsBusy AndAlso Me.Visible)
                     Application.DoEvents()
                  Loop
                  CurrentImageBox.Image = CurrentImage

                  Me.Cursor = Cursors.Default
            End Select
         End If
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure closes this program.
   Private Sub QuitMenu_Click(sender As Object, e As EventArgs) Handles QuitMenu.Click
      Try
         Me.Close()
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure gives the command to change the current image's size as specified by the user.
   Private Sub Resizer_DoWork(sender As Object, e As DoWorkEventArgs) Handles Resizer.DoWork
      Try
         Static Resizes As Integer = 1

         Select Case e.Argument.ToString().ToUpper()
            Case "H"
               Integer.TryParse(InputBox("Number of columns to add/remove:", , Resizes.ToString()), Resizes)
               If Resizes = 0 Then Exit Sub
               StartTime = My.Computer.Clock.LocalTime
               CurrentImage = ResizeImage(CurrentImage, Resizes, Vertical:=False)
            Case "V"
               Integer.TryParse(InputBox("Number of rows to add/remove:", , Resizes.ToString()), Resizes)
               If Resizes = 0 Then Exit Sub
               StartTime = My.Computer.Clock.LocalTime
               CurrentImage = ResizeImage(CurrentImage, Resizes, Vertical:=True)
         End Select
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure notifies the user when the resizing of the image has finished.
   Private Sub Resizer_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles Resizer.RunWorkerCompleted
      Try
         Dim EndTime As DateTime = My.Computer.Clock.LocalTime

         DisplayStatus($"Resizing took {EndTime.Subtract(StartTime).TotalMilliseconds} millisecond(s).")

         My.Computer.Audio.PlaySystemSound(SystemSounds.Beep)
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure displays the specified status information.
   Private Sub DisplayStatus(Optional Status As String = Nothing)
      Try
         With My.Application.Info
            Me.Text = $"{ .Title} "
#If PLATFORM = "x86" Then
            Me.Text &= "x86"
#Else
            Me.Text &= "x64"
#End If
            Me.Text &= $" v{ .Version} - by: { .CompanyName}"
#If DEBUG Then
            Me.Text &= " (DEBUG) "
#End If
            If CurrentImage IsNot Nothing Then Me.Text &= $" - Image size: {CurrentImage.Width}x{CurrentImage.Height}"
            If Not Status = Nothing Then Me.Text &= $" - {Status}"
         End With
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub
End Class
