Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp.Updating
Imports DevExpress.Persistent.Validation
Imports DevExpress.ExpressApp.Security.Strategy
Imports DevExpress.Persistent.Base.Security

Namespace ManageUsersOnLogon.Module.DatabaseUpdate
	Public Class Updater
		Inherits ModuleUpdater
		Public Sub New(ByVal objectSpace As IObjectSpace, ByVal currentDBVersion As Version)
			MyBase.New(objectSpace, currentDBVersion)
		End Sub
		Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
			MyBase.UpdateDatabaseAfterUpdateSchema()
'			#Region "Create Users for the Complex Security Strategy"
			CreateUser(ObjectSpace, "Sam", "sam@example.com", String.Empty, True)
			CreateUser(ObjectSpace, "John", "john@example.com", String.Empty, False)
			ObjectSpace.CommitChanges()
'			#End Region
		End Sub
		Public Shared Function CreateUser(ByVal os As IObjectSpace, ByVal userName As String, ByVal email As String, ByVal password As String, ByVal isAdministrator As Boolean) As IAuthenticationStandardUser
			If String.IsNullOrEmpty(userName) OrElse String.IsNullOrEmpty(email) Then
				Throw New ArgumentException("UserName and Email address are not specified!")
			End If
			Dim user As SecuritySystemUser = os.FindObject(Of SecuritySystemUser)(New BinaryOperator("UserName", userName))
			If user Is Nothing Then
				user = os.CreateObject(Of SecuritySystemUser)()
				user.UserName = userName
				user.IsActive = True
				user.SetMemberValue("Email", email)
				user.SetPassword(password)
				Dim role As SecuritySystemRole = If(isAdministrator, GetAdministratorRole(os), GetDefaultRole(os))
				user.Roles.Add(role)
				user.Save()
				If Validator.RuleSet.ValidateTarget(user, DefaultContexts.Save).State = ValidationState.Valid Then
					os.CommitChanges()
				End If
			End If
			Return user
		End Function
		Public Shared Function GetAdministratorRole(ByVal os As IObjectSpace) As SecuritySystemRole
			Dim administratorRole As SecuritySystemRole = os.FindObject(Of SecuritySystemRole)(New BinaryOperator("Name", "Administrator"))
			If administratorRole Is Nothing Then
				administratorRole = os.CreateObject(Of SecuritySystemRole)()
				administratorRole.Name = "Administrator"
				administratorRole.IsAdministrative = True
			End If
			Return administratorRole
		End Function
		Public Shared Function GetDefaultRole(ByVal os As IObjectSpace) As SecuritySystemRole
			Dim defaultRole As SecuritySystemRole = os.FindObject(Of SecuritySystemRole)(New BinaryOperator("Name", "Default"))
			If defaultRole Is Nothing Then
				defaultRole = os.CreateObject(Of SecuritySystemRole)()
				defaultRole.Name = "Default"

				Dim securityDemoUserPermissions As SecuritySystemTypePermissionObject = os.CreateObject(Of SecuritySystemTypePermissionObject)()
				securityDemoUserPermissions.TargetType = GetType(SecuritySystemUser)
				defaultRole.TypePermissions.Add(securityDemoUserPermissions)

				Dim myDetailsPermission As SecuritySystemObjectPermissionsObject = os.CreateObject(Of SecuritySystemObjectPermissionsObject)()
				myDetailsPermission.Criteria = "[Oid] = CurrentUserId()"
				myDetailsPermission.AllowNavigate = True
				myDetailsPermission.AllowRead = True
				securityDemoUserPermissions.ObjectPermissions.Add(myDetailsPermission)

				Dim userPermissions As SecuritySystemTypePermissionObject = os.CreateObject(Of SecuritySystemTypePermissionObject)()
				userPermissions.TargetType = GetType(SecuritySystemUser)
				defaultRole.TypePermissions.Add(userPermissions)

				Dim ownPasswordPermission As SecuritySystemMemberPermissionsObject = os.CreateObject(Of SecuritySystemMemberPermissionsObject)()
				ownPasswordPermission.Members = "ChangePasswordOnFirstLogon; StoredPassword"
				ownPasswordPermission.AllowWrite = True
				userPermissions.MemberPermissions.Add(ownPasswordPermission)

				Dim securityRolePermissions As SecuritySystemTypePermissionObject = os.CreateObject(Of SecuritySystemTypePermissionObject)()
				securityRolePermissions.TargetType = GetType(SecuritySystemRole)
				defaultRole.TypePermissions.Add(userPermissions)

				Dim defaultRolePermission As SecuritySystemObjectPermissionsObject = os.CreateObject(Of SecuritySystemObjectPermissionsObject)()
				defaultRolePermission.Criteria = "[Name] = 'Default'"
				defaultRolePermission.AllowNavigate = True
				defaultRolePermission.AllowRead = True
				securityRolePermissions.ObjectPermissions.Add(defaultRolePermission)
			End If
			Return defaultRole
		End Function
	End Class
End Namespace