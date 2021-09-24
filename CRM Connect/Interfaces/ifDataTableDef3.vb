Public Interface ifDataTableDef3


  Event AfterSaveChanges(sender As Object, e As System.EventArgs)
  ReadOnly Property TableAdapter As Object
  ReadOnly Property HasChanges As Boolean
  ReadOnly Property HasRows As Boolean
  ReadOnly Property Table As DataTable

  ReadOnly Property SavingChanges As Boolean

  Property UserPrivileges As dsPrins3Central.UserPrivilegeEnum

  Sub SaveChanges()
  Sub Reload(Optional ReloadOptions As dsPrins3Central.ReloadOptions = dsPrins3Central.ReloadOptions.SaveChanges_AbortAtError)


End Interface
