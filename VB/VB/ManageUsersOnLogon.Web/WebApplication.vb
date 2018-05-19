Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Web
Imports DevExpress.ExpressApp.Xpo

Namespace ManageUsersOnLogon.Web
	Partial Public Class ManageUsersOnLogonAspNetApplication
		Inherits WebApplication
		Private module1 As DevExpress.ExpressApp.SystemModule.SystemModule
		Private module2 As DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule
		Private module3 As ManageUsersOnLogon.Module.ManageUsersOnLogonModule
		'private ManageUsersOnLogon.Module.Web.ManageUsersOnLogonAspNetModule module4;
		Private securityModule1 As DevExpress.ExpressApp.Security.SecurityModule
		Private validationModule1 As DevExpress.ExpressApp.Validation.ValidationModule
		Private authenticationStandard1 As DevExpress.ExpressApp.Security.AuthenticationStandard
		Private securityStrategyComplex1 As DevExpress.ExpressApp.Security.SecurityStrategyComplex
		Private sqlConnection1 As System.Data.SqlClient.SqlConnection

		Public Sub New()
			InitializeComponent()
		End Sub

		Protected Overrides Sub CreateDefaultObjectSpaceProvider(ByVal args As CreateCustomObjectSpaceProviderEventArgs)
			args.ObjectSpaceProvider = New XPObjectSpaceProviderThreadSafe(args.ConnectionString, args.Connection)
		End Sub

		Private Sub ManageUsersOnLogonAspNetApplication_DatabaseVersionMismatch(ByVal sender As Object, ByVal e As DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs) Handles MyBase.DatabaseVersionMismatch
#If EASYTEST Then
			e.Updater.Update()
			e.Handled = True
#Else
			If System.Diagnostics.Debugger.IsAttached Then
				e.Updater.Update()
				e.Handled = True
			Else
				Dim message As String = "The application cannot connect to the specified database, because the latter doesn't exist or its version is older than that of the application." & Constants.vbCrLf & "This error occurred  because the automatic database update was disabled when the application was started without debugging." & Constants.vbCrLf & "To avoid this error, you should either start the application under Visual Studio in debug mode, or modify the " & "source code of the 'DatabaseVersionMismatch' event handler to enable automatic database update, " & "or manually create a database using the 'DBUpdater' tool." & Constants.vbCrLf & "Anyway, refer to the following help topics for more detailed information:" & Constants.vbCrLf & "'Update Application and Database Versions' at http://www.devexpress.com/Help/?document=ExpressApp/CustomDocument2795.htm" & Constants.vbCrLf & "'Database Security References' at http://www.devexpress.com/Help/?document=ExpressApp/CustomDocument3237.htm" & Constants.vbCrLf & "If this doesn't help, please contact our Support Team at http://www.devexpress.com/Support/Center/"

				If e.CompatibilityError IsNot Nothing AndAlso e.CompatibilityError.Exception IsNot Nothing Then
					message &= Constants.vbCrLf & Constants.vbCrLf & "Inner exception: " & e.CompatibilityError.Exception.Message
				End If
				Throw New InvalidOperationException(message)
			End If
#End If
		End Sub

		Private Sub InitializeComponent()
			Me.module1 = New DevExpress.ExpressApp.SystemModule.SystemModule()
			Me.module2 = New DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule()
			Me.sqlConnection1 = New System.Data.SqlClient.SqlConnection()
			Me.securityModule1 = New DevExpress.ExpressApp.Security.SecurityModule()
			Me.validationModule1 = New DevExpress.ExpressApp.Validation.ValidationModule()
			Me.authenticationStandard1 = New DevExpress.ExpressApp.Security.AuthenticationStandard()
			Me.securityStrategyComplex1 = New DevExpress.ExpressApp.Security.SecurityStrategyComplex()
			Me.module3 = New ManageUsersOnLogon.Module.ManageUsersOnLogonModule()
			'this.module4 = new ManageUsersOnLogon.Module.Web.ManageUsersOnLogonAspNetModule();
			CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
			' 
			' sqlConnection1
			' 
			Me.sqlConnection1.ConnectionString = "Integrated Security=SSPI;Pooling=false;Data Source=.\SQLEXPRESS;Initial Catalog=M" & "anageUsersOnLogon"
			Me.sqlConnection1.FireInfoMessageEventOnUserErrors = False
			' 
			' validationModule1
			' 
			Me.validationModule1.AllowValidationDetailsAccess = True
			' 
			' authenticationStandard1
			' 
			Me.authenticationStandard1.LogonParametersType = GetType(DevExpress.ExpressApp.Security.AuthenticationStandardLogonParameters)
			' 
			' securityStrategyComplex1
			' 
			Me.securityStrategyComplex1.Authentication = Me.authenticationStandard1
			Me.securityStrategyComplex1.RoleType = GetType(DevExpress.ExpressApp.Security.Strategy.SecuritySystemRole)
			Me.securityStrategyComplex1.UserType = GetType(DevExpress.ExpressApp.Security.Strategy.SecuritySystemUser)
			' 
			' ManageUsersOnLogonAspNetApplication
			' 
			Me.ApplicationName = "ManageUsersOnLogon"
			Me.Connection = Me.sqlConnection1
			Me.Modules.Add(Me.module1)
			Me.Modules.Add(Me.module2)
			Me.Modules.Add(Me.securityModule1)
			Me.Modules.Add(Me.validationModule1)
			Me.Modules.Add(Me.module3)
			'this.Modules.Add(this.module4);
			Me.Security = Me.securityStrategyComplex1
'			Me.DatabaseVersionMismatch += New System.EventHandler(Of DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs)(Me.ManageUsersOnLogonAspNetApplication_DatabaseVersionMismatch);
			CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

		End Sub
	End Class
End Namespace
