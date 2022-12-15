'This module's imports and settings.
Option Compare Binary
Option Explicit On
Option Infer Off
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Convert
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Environment
Imports System.Linq
Imports System.Math
Imports System.Runtime.InteropServices.Marshal
Imports System.Windows.Forms

'This module contains this program's core procedures.
Public Module CoreModule

   'This structure defines a seam.
   Private Structure SeamStr
      Public Energy As Integer            'Defines the seam's energy.
      Public Indexes As List(Of Integer)  'Defines the indexes of the seam's pixels.
   End Structure

   Private Const ARGB_SIZE As Integer = &H4%   'Defines the number of bytes in an alpha, red, green, and blue color value.

   'This procedure returns the best vertical seam.
   Private Function BestVerticalSeam(Pixels As List(Of Byte), Stride As Integer, Height As Integer) As SeamStr
      Try
         Dim BestSeam As SeamStr = VerticalSeam(Pixels, Stride, Height, 0)
         Dim Seam As New SeamStr With {.Energy = 0, .Indexes = New List(Of Integer)}

         If BestSeam.Energy = 0 Then Return BestSeam

         For x As Integer = ARGB_SIZE To Stride - ARGB_SIZE Step ARGB_SIZE
            Seam = VerticalSeam(Pixels, Stride, Height, x)
            If Seam.Energy < BestSeam.Energy Then BestSeam = Seam
         Next x

         Return BestSeam
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try

      Return Nothing
   End Function

   'This procedure returns the difference between the two specified colors.
   Private Function ColorDifference(Color1() As Byte, Color2() As Byte) As Integer
      Try
         Dim Difference As Integer = 0

         For Index As Integer = Color1.GetLowerBound(0) To Color1.GetUpperBound(0)
            Difference += Abs(ToInt32(Color2(Index)) - ToInt32(Color1(Index)))
         Next Index

         Return CInt(Difference / 3)
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try

      Return Nothing
   End Function

   'This procedure returns the index of the lowest difference.
   Private Function GetLowest(Differences As List(Of Integer)) As Integer
      Try
         Dim Lowest As Integer = 1

         For Index As Integer = 0 To Differences.Count - 1
            If Differences(Index) < Differences(Lowest) Then Lowest = Index
         Next Index

         Return Lowest
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try

      Return Nothing
   End Function

   'This procedure handles any errors that occur.
   Public Sub HandleError(ExceptionO As Exception)
      Try
         If MessageBox.Show(ExceptionO.Message, My.Application.Info.Title, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) = DialogResult.Cancel Then InterfaceWindow.Close()
      Catch
         [Exit](0)
      End Try
   End Sub

   'This procedure resizes the specified image.
   Public Function ResizeImage(ResizedImage As Bitmap, Resizes As Integer, Vertical As Boolean) As Bitmap
      Try
         Dim BitmapDataO As BitmapData = Nothing
         Dim BitmapStride As Integer = Nothing
         Dim Buffer() As Byte = Nothing
         Dim ImagePixels As New List(Of Byte)

         If Vertical Then ResizedImage.RotateFlip(RotateFlipType.Rotate90FlipXY)
         If ResizedImage.Width + Resizes < 1 Then Resizes = -(ResizedImage.Width - 1)

         With ResizedImage
            BitmapDataO = .LockBits(New Rectangle(0, 0, .Width, .Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppPArgb)
            BitmapStride = BitmapDataO.Stride
            ReDim Buffer(BitmapStride * .Height)
            Copy(BitmapDataO.Scan0, Buffer, Buffer.GetLowerBound(0), Buffer.GetUpperBound(0))
            .UnlockBits(BitmapDataO)
            ImagePixels = Buffer.ToList
         End With

         If Resizes > 0 Then
            For Resize As Integer = 1 To Abs(Resizes)
               With BestVerticalSeam(ImagePixels, BitmapStride, ResizedImage.Height)
                  .Indexes.Sort()
                  .Indexes.Reverse()
                  .Indexes.ForEach(Sub(Index As Integer) ImagePixels.InsertRange(Index + ARGB_SIZE, ImagePixels.GetRange(Index, ARGB_SIZE)))
               End With

               BitmapStride += ARGB_SIZE
            Next Resize
         Else
            For Resize As Integer = 1 To Abs(Resizes)
               With BestVerticalSeam(ImagePixels, BitmapStride, ResizedImage.Height)
                  .Indexes.Sort()
                  .Indexes.Reverse()
                  .Indexes.ForEach(Sub(Index As Integer) ImagePixels.RemoveRange(Index, ARGB_SIZE))
               End With

               BitmapStride -= ARGB_SIZE
            Next Resize
         End If

         ResizedImage = New Bitmap(CInt(BitmapStride / ARGB_SIZE), ResizedImage.Height)

         With ResizedImage
            Buffer = ImagePixels.ToArray()
            BitmapDataO = .LockBits(New Rectangle(0, 0, .Width, .Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppPArgb)
            Copy(Buffer, Buffer.GetLowerBound(0), BitmapDataO.Scan0, Buffer.GetUpperBound(0))
            .UnlockBits(BitmapDataO)
         End With

         If Vertical Then ResizedImage.RotateFlip(RotateFlipType.Rotate270FlipXY)

         Return ResizedImage
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try

      Return Nothing
   End Function

   'This procedure returns a vertical seam through the specified locked bitmap.
   Private Function VerticalSeam(Pixels As List(Of Byte), Stride As Integer, Height As Integer, x As Integer) As SeamStr
      Try
         Dim CurrentIndex As Integer = Nothing
         Dim Differences As New List(Of Integer)({0, 0, 0})
         Dim NextIndex As Integer = Nothing
         Dim Lowest As Integer = Nothing
         Dim Seam As New SeamStr With {.Energy = 0, .Indexes = New List(Of Integer)}

         For y As Integer = 0 To Height - 1
            CurrentIndex = (y * Stride) + x

            NextIndex = (y * Stride) + (x - ARGB_SIZE)
            Differences(0) = If(x >= ARGB_SIZE, ColorDifference({Pixels(CurrentIndex), Pixels(CurrentIndex + 1), Pixels(CurrentIndex + 2)}, {Pixels(NextIndex), Pixels(NextIndex + 1), Pixels(NextIndex + 2)}), Byte.MaxValue)
            NextIndex = ((y + 1) * Stride) + x
            Differences(1) = If(y < Height - 1, ColorDifference({Pixels(CurrentIndex), Pixels(CurrentIndex + 1), Pixels(CurrentIndex + 2)}, {Pixels(NextIndex), Pixels(NextIndex + 1), Pixels(NextIndex + 2)}), Byte.MaxValue)
            NextIndex = (y * Stride) + (x + ARGB_SIZE)
            Differences(2) = If(x + ARGB_SIZE < Stride, ColorDifference({Pixels(CurrentIndex), Pixels(CurrentIndex + 1), Pixels(CurrentIndex + 2)}, {Pixels(NextIndex), Pixels(NextIndex + 1), Pixels(NextIndex + 2)}), Byte.MaxValue)
            Lowest = GetLowest(Differences)
            Seam.Energy += Differences(Lowest)
            Seam.Indexes.Add(CurrentIndex)
            If Lowest = 0 Then
               x -= ARGB_SIZE
            ElseIf Lowest = 2 Then
               x += ARGB_SIZE
            End If
         Next y

         Return Seam
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try

      Return Nothing
   End Function
End Module
