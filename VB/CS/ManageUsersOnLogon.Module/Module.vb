Imports Microsoft.VisualBasic
Imports System
Imports Security.Extensions
Imports DevExpress.ExpressApp
Imports System.Collections.Generic
Imports ManageUsersOnLogon.Module.DatabaseUpdate

Namespace ManageUsersOnLogon.Module
	Public NotInheritable Partial Class ManageUsersOnLogonModule
		Inherits ModuleBase
		Public Sub New()
			InitializeComponent()
		End Sub
		Shared Sub New()
			SecurityExtensionsModule.CreateSecuritySystemUser = New CreateSecuritySystemUser(AddressOf Updater.CreateUser)
		End Sub
	End Class
End Namespace
