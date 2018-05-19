Imports Microsoft.VisualBasic
Imports System
Namespace ManageUsersOnLogon.Win
	Partial Public Class ManageUsersOnLogonWindowsFormsApplication
		''' <summary> 
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary> 
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Component Designer generated code"

		''' <summary> 
		''' Required method for Designer support - do not modify 
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.module1 = New DevExpress.ExpressApp.SystemModule.SystemModule()
			Me.module2 = New DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule()
			Me.module3 = New ManageUsersOnLogon.Module.ManageUsersOnLogonModule()
			Me.sqlConnection1 = New System.Data.SqlClient.SqlConnection()
			Me.securityModule1 = New DevExpress.ExpressApp.Security.SecurityModule()
			Me.securityStrategyComplex1 = New DevExpress.ExpressApp.Security.SecurityStrategyComplex()
			Me.authenticationStandard1 = New DevExpress.ExpressApp.Security.AuthenticationStandard()
			Me.validationModule1 = New DevExpress.ExpressApp.Validation.ValidationModule()
			Me.securityExtensionsModule1 = New Security.Extensions.SecurityExtensionsModule()
			CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
			' 
			' sqlConnection1
			' 
			Me.sqlConnection1.ConnectionString = "Integrated Security=SSPI;Pooling=false;Data Source=.\SQLEXPRESS;Initial Catalog=M" & "anageUsersOnLogon"
			Me.sqlConnection1.FireInfoMessageEventOnUserErrors = False
			' 
			' securityStrategyComplex1
			' 
			Me.securityStrategyComplex1.Authentication = Me.authenticationStandard1
			Me.securityStrategyComplex1.RoleType = GetType(DevExpress.ExpressApp.Security.Strategy.SecuritySystemRole)
			Me.securityStrategyComplex1.UserType = GetType(DevExpress.ExpressApp.Security.Strategy.SecuritySystemUser)
			' 
			' authenticationStandard1
			' 
			Me.authenticationStandard1.LogonParametersType = GetType(DevExpress.ExpressApp.Security.AuthenticationStandardLogonParameters)
			' 
			' validationModule1
			' 
			Me.validationModule1.AllowValidationDetailsAccess = True
			' 
			' ManageUsersOnLogonWindowsFormsApplication
			' 
			Me.ApplicationName = "ManageUsersOnLogon"
			Me.Connection = Me.sqlConnection1
			Me.Modules.Add(Me.module1)
			Me.Modules.Add(Me.module2)
			Me.Modules.Add(Me.securityModule1)
			Me.Modules.Add(Me.validationModule1)
			Me.Modules.Add(Me.securityExtensionsModule1)
			Me.Modules.Add(Me.module3)
			Me.Security = Me.securityStrategyComplex1
'			Me.DatabaseVersionMismatch += New System.EventHandler(Of DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs)(Me.ManageUsersOnLogonWindowsFormsApplication_DatabaseVersionMismatch);
'			Me.CustomizeLanguagesList += New System.EventHandler(Of DevExpress.ExpressApp.CustomizeLanguagesListEventArgs)(Me.ManageUsersOnLogonWindowsFormsApplication_CustomizeLanguagesList);
			CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

		End Sub

		#End Region

		Private module1 As DevExpress.ExpressApp.SystemModule.SystemModule
		Private module2 As DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule
		Private module3 As ManageUsersOnLogon.Module.ManageUsersOnLogonModule
		'private ManageUsersOnLogon.Module.Win.ManageUsersOnLogonWindowsFormsModule module4;
		Private sqlConnection1 As System.Data.SqlClient.SqlConnection
		Private securityModule1 As DevExpress.ExpressApp.Security.SecurityModule
		Private securityStrategyComplex1 As DevExpress.ExpressApp.Security.SecurityStrategyComplex
		Private authenticationStandard1 As DevExpress.ExpressApp.Security.AuthenticationStandard
		Private validationModule1 As DevExpress.ExpressApp.Validation.ValidationModule
		Private securityExtensionsModule1 As Security.Extensions.SecurityExtensionsModule
	End Class
End Namespace
