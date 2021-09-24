<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucProjekt
    Inherits System.Windows.Forms.UserControl

    'UserControl überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucProjekt))
    Me.fgProjekt = New C1.Win.C1FlexGrid.C1FlexGrid()
    Me.bsProjekt = New System.Windows.Forms.BindingSource(Me.components)
    Me.DsSQL_CRM_Prins = New CRM_Connect.dsSQL_CRM_Prins()
    Me.ProjektTableAdapter = New CRM_Connect.dsSQL_CRM_PrinsTableAdapters.ProjektTableAdapter()
    Me.TableAdapterManager = New CRM_Connect.dsSQL_CRM_PrinsTableAdapters.TableAdapterManager()
    CType(Me.fgProjekt, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.bsProjekt, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.DsSQL_CRM_Prins, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'fgProjekt
    '
    Me.fgProjekt.ColumnInfo = resources.GetString("fgProjekt.ColumnInfo")
    Me.fgProjekt.DataSource = Me.bsProjekt
    Me.fgProjekt.Dock = System.Windows.Forms.DockStyle.Fill
    Me.fgProjekt.Location = New System.Drawing.Point(0, 0)
    Me.fgProjekt.Name = "fgProjekt"
    Me.fgProjekt.Rows.Count = 1
    Me.fgProjekt.Rows.DefaultSize = 19
    Me.fgProjekt.Size = New System.Drawing.Size(800, 400)
    Me.fgProjekt.TabIndex = 0
    Me.fgProjekt.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver
    '
    'bsProjekt
    '
    Me.bsProjekt.DataMember = "Projekt"
    Me.bsProjekt.DataSource = Me.DsSQL_CRM_Prins
    '
    'DsSQL_CRM_Prins
    '
    Me.DsSQL_CRM_Prins.DataSetName = "dsSQL_CRM_Prins"
    Me.DsSQL_CRM_Prins.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
    '
    'ProjektTableAdapter
    '
    Me.ProjektTableAdapter.ClearBeforeFill = True
    '
    'TableAdapterManager
    '
    Me.TableAdapterManager.BackupDataSetBeforeUpdate = False
    Me.TableAdapterManager.FirmaTableAdapter = Nothing
    Me.TableAdapterManager.ProjektTableAdapter = Me.ProjektTableAdapter
    Me.TableAdapterManager.UpdateOrder = CRM_Connect.dsSQL_CRM_PrinsTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete
    '
    'ucProjekt
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.Controls.Add(Me.fgProjekt)
    Me.Name = "ucProjekt"
    Me.Size = New System.Drawing.Size(800, 400)
    CType(Me.fgProjekt, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.bsProjekt, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.DsSQL_CRM_Prins, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents fgProjekt As C1.Win.C1FlexGrid.C1FlexGrid
  Friend WithEvents bsProjekt As System.Windows.Forms.BindingSource
  Friend WithEvents DsSQL_CRM_Prins As CRM_Connect.dsSQL_CRM_Prins
  Friend WithEvents ProjektTableAdapter As CRM_Connect.dsSQL_CRM_PrinsTableAdapters.ProjektTableAdapter
  Friend WithEvents TableAdapterManager As CRM_Connect.dsSQL_CRM_PrinsTableAdapters.TableAdapterManager

End Class
