Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp
Imports DevExpress.Data.Filtering
Imports DevExpress.Persistent.Base
Imports DevExpress.ExpressApp.Model
Imports DevExpress.Persistent.Validation
Imports DevExpress.Persistent.Base.Security

Namespace Security.Extensions
	<NonPersistent, ModelDefault("Caption", "Register User"), ImageName("BO_User")> _
	Public Class RegisterUserParameters
		Implements ILogonActionParameters
		Public Const ValidationContext As String = "RegisterUserContext"
		Private privateUserName As String
		<RuleRequiredField(Nothing, ValidationContext)> _
		Public Property UserName() As String
			Get
				Return privateUserName
			End Get
			Set(ByVal value As String)
				privateUserName = value
			End Set
		End Property
		Private privatePassword As String
		Public Property Password() As String
			Get
				Return privatePassword
			End Get
			Set(ByVal value As String)
				privatePassword = value
			End Set
		End Property
		Private privateEmail As String
		<RuleRequiredField(Nothing, ValidationContext), RuleRegularExpression(Nothing, ValidationContext, ManageUsersOnLogonController.EmailPattern)> _
		Public Property Email() As String
			Get
				Return privateEmail
			End Get
			Set(ByVal value As String)
				privateEmail = value
			End Set
		End Property
		Public Sub Process(ByVal objectSpace As IObjectSpace) Implements ILogonActionParameters.Process
			Dim user As IAuthenticationStandardUser = TryCast(objectSpace.FindObject(SecurityExtensionsModule.SecuritySystemUserType, New BinaryOperator("UserName", UserName)), IAuthenticationStandardUser)
			If user IsNot Nothing Then
				Throw New ArgumentException("The login with the entered UserName or Email was already registered within the system")
			Else
				SecurityExtensionsModule.CreateSecuritySystemUser(objectSpace, UserName, Email, Password, False)
			End If
			'throw new UserFriendlyException("A new user has successfully been registered");
		End Sub
	End Class
End Namespace
