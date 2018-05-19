Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.DC
Imports System.Collections.Generic
Imports DevExpress.ExpressApp.Utils
Imports DevExpress.ExpressApp.Security
Imports DevExpress.Persistent.Base.Security

Namespace Security.Extensions
	Public NotInheritable Partial Class SecurityExtensionsModule
		Inherits ModuleBase
		Friend Shared SecuritySystemUserType As Type
		Public Shared CreateSecuritySystemUser As CreateSecuritySystemUser
		Public Sub New()
			InitializeComponent()
		End Sub
		Public Overrides Sub Setup(ByVal application As XafApplication)
			MyBase.Setup(application)
			AddHandler application.CreateCustomLogonWindowControllers, AddressOf application_CreateCustomLogonWindowControllers
			Guard.ArgumentNotNull(CreateSecuritySystemUser, "CreateSecuritySystemUser")
			Dim securityStrategy As SecurityStrategy = TryCast(application.Security, SecurityStrategy)
			If securityStrategy IsNot Nothing Then
				SecuritySystemUserType = securityStrategy.UserType
			End If
		End Sub
		Private Sub application_CreateCustomLogonWindowControllers(ByVal sender As Object, ByVal e As CreateCustomLogonWindowControllersEventArgs)
			Dim app As XafApplication = CType(sender, XafApplication)
			e.Controllers.Add(app.CreateController(Of ManageUsersOnLogonController)())
			e.Controllers.Add(app.CreateController(Of DevExpress.ExpressApp.Validation.ActionValidationController)())
			e.Controllers.Add(app.CreateController(Of DevExpress.ExpressApp.SystemModule.DialogController)())
		End Sub
		'Dennis: I want to avoid inheritance from SecuritySystemUser and will just add a new property dynamically.
		Public Overrides Sub CustomizeTypesInfo(ByVal typesInfo As ITypesInfo)
			MyBase.CustomizeTypesInfo(typesInfo)
			Dim ti As ITypeInfo = typesInfo.FindTypeInfo(SecuritySystemUserType)
			If ti IsNot Nothing Then
				Dim mi As IMemberInfo = ti.FindMember("Email")
				If mi Is Nothing Then
					mi = ti.CreateMember("Email", GetType(String))
				End If
			End If
		End Sub
	End Class
	Public Delegate Function CreateSecuritySystemUser(ByVal objectSpace As IObjectSpace, ByVal userName As String, ByVal email As String, ByVal password As String, ByVal isAdministrator As Boolean) As IAuthenticationStandardUser
End Namespace
