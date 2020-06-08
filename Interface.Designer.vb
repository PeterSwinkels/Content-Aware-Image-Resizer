<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InterfaceWindow
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
      Me.CurrentImageBox = New System.Windows.Forms.PictureBox()
      Me.MenuBar = New System.Windows.Forms.MenuStrip()
      Me.ProgramMainMenu = New System.Windows.Forms.ToolStripMenuItem()
      Me.HelpMenu = New System.Windows.Forms.ToolStripMenuItem()
      Me.QuitMenu = New System.Windows.Forms.ToolStripMenuItem()
      CType(Me.CurrentImageBox, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.MenuBar.SuspendLayout()
      Me.SuspendLayout()
      '
      'CurrentImageBox
      '
      Me.CurrentImageBox.Location = New System.Drawing.Point(0, 27)
      Me.CurrentImageBox.Name = "CurrentImageBox"
      Me.CurrentImageBox.Size = New System.Drawing.Size(100, 50)
      Me.CurrentImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
      Me.CurrentImageBox.TabIndex = 2
      Me.CurrentImageBox.TabStop = False
      '
      'MenuBar
      '
      Me.MenuBar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ProgramMainMenu})
      Me.MenuBar.Location = New System.Drawing.Point(0, 0)
      Me.MenuBar.Name = "MenuBar"
      Me.MenuBar.Size = New System.Drawing.Size(311, 24)
      Me.MenuBar.TabIndex = 3
      '
      'ProgramMainMenu
      '
      Me.ProgramMainMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpMenu, Me.QuitMenu})
      Me.ProgramMainMenu.Name = "ProgramMainMenu"
      Me.ProgramMainMenu.Size = New System.Drawing.Size(65, 20)
      Me.ProgramMainMenu.Text = "&Program"
      '
      'HelpMenu
      '
      Me.HelpMenu.Name = "HelpMenu"
      Me.HelpMenu.ShortcutKeys = System.Windows.Forms.Keys.F1
      Me.HelpMenu.Size = New System.Drawing.Size(152, 22)
      Me.HelpMenu.Text = "&Help"
      '
      'QuitMenu
      '
      Me.QuitMenu.Name = "QuitMenu"
      Me.QuitMenu.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Q), System.Windows.Forms.Keys)
      Me.QuitMenu.Size = New System.Drawing.Size(152, 22)
      Me.QuitMenu.Text = "&Quit"
      '
      'InterfaceWindow
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.AutoScroll = True
      Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
      Me.ClientSize = New System.Drawing.Size(311, 262)
      Me.Controls.Add(Me.CurrentImageBox)
      Me.Controls.Add(Me.MenuBar)
      Me.DoubleBuffered = True
      Me.KeyPreview = True
      Me.MainMenuStrip = Me.MenuBar
      Me.Name = "InterfaceWindow"
      Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
      CType(Me.CurrentImageBox, System.ComponentModel.ISupportInitialize).EndInit()
      Me.MenuBar.ResumeLayout(False)
      Me.MenuBar.PerformLayout()
      Me.ResumeLayout(False)
      Me.PerformLayout()

   End Sub
   Friend WithEvents CurrentImageBox As System.Windows.Forms.PictureBox
   Friend WithEvents MenuBar As System.Windows.Forms.MenuStrip
   Friend WithEvents ProgramMainMenu As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents HelpMenu As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents QuitMenu As System.Windows.Forms.ToolStripMenuItem

End Class
