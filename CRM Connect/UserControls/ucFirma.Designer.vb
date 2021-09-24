<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucFirma
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucFirma))
    Me.fgFirma = New C1.Win.C1FlexGrid.C1FlexGrid()
    Me.bsFirma = New System.Windows.Forms.BindingSource(Me.components)
    Me.DsSQL_CRM_Prins = New CRM_Connect.dsSQL_CRM_Prins()
    Me.FirmaTableAdapter = New CRM_Connect.dsSQL_CRM_PrinsTableAdapters.FirmaTableAdapter()
    Me.TableAdapterManager = New CRM_Connect.dsSQL_CRM_PrinsTableAdapters.TableAdapterManager()
    CType(Me.fgFirma, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.bsFirma, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.DsSQL_CRM_Prins, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'fgFirma
    '
    Me.fgFirma.ColumnInfo = resources.GetString("fgFirma.ColumnInfo")
    Me.fgFirma.DataSource = Me.bsFirma
    Me.fgFirma.Dock = System.Windows.Forms.DockStyle.Fill
    Me.fgFirma.Location = New System.Drawing.Point(0, 0)
    Me.fgFirma.Name = "fgFirma"
    Me.fgFirma.Rows.Count = 1
    Me.fgFirma.Rows.DefaultSize = 19
    Me.fgFirma.Size = New System.Drawing.Size(735, 458)
    Me.fgFirma.TabIndex = 0
    Me.fgFirma.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver
    '
    'bsFirma
    '
    Me.bsFirma.DataMember = "Firma"
    Me.bsFirma.DataSource = Me.DsSQL_CRM_Prins
    '
    'DsSQL_CRM_Prins
    '
    Me.DsSQL_CRM_Prins.DataSetName = "dsSQL_CRM_Prins"
    Me.DsSQL_CRM_Prins.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
    '
    'FirmaTableAdapter
    '
    Me.FirmaTableAdapter.ClearBeforeFill = True
    '
    'TableAdapterManager
    '
    Me.TableAdapterManager.BackupDataSetBeforeUpdate = False
    Me.TableAdapterManager.FirmaTableAdapter = Me.FirmaTableAdapter
    Me.TableAdapterManager.ProjektTableAdapter = Nothing
    Me.TableAdapterManager.UpdateOrder = CRM_Connect.dsSQL_CRM_PrinsTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete
    '
    'ucFirma
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.Controls.Add(Me.fgFirma)
    Me.Name = "ucFirma"
    Me.Size = New System.Drawing.Size(735, 458)
    CType(Me.fgFirma, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.bsFirma, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.DsSQL_CRM_Prins, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents fgFirma As C1.Win.C1FlexGrid.C1FlexGrid
  Friend WithEvents bsFirma As System.Windows.Forms.BindingSource
  Friend WithEvents DsSQL_CRM_Prins As CRM_Connect.dsSQL_CRM_Prins
  Friend WithEvents FirmaTableAdapter As CRM_Connect.dsSQL_CRM_PrinsTableAdapters.FirmaTableAdapter
  Friend WithEvents TableAdapterManager As CRM_Connect.dsSQL_CRM_PrinsTableAdapters.TableAdapterManager

End Class
