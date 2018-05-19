Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp
Imports DevExpress.Data.Filtering
Imports DevExpress.Persistent.Base
Imports DevExpress.ExpressApp.Model
Imports System.Security.Cryptography
Imports DevExpress.Persistent.Validation
Imports DevExpress.Persistent.Base.Security

Namespace Security.Extensions
	<NonPersistent, ModelDefault("Caption", "Restore Password"), ImageName("Action_ResetPassword")> _
	Public Class RestorePasswordParameters
		Implements ILogonActionParameters
		Public Const ValidationContext As String = "RestorePasswordContext"
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
			If String.IsNullOrEmpty(Email) Then
				Throw New ArgumentException("Email address is not specified!")
			End If
			Dim user As IAuthenticationStandardUser = TryCast(objectSpace.FindObject(SecurityExtensionsModule.SecuritySystemUserType, CriteriaOperator.Parse("Email = ?", Email)), IAuthenticationStandardUser)
			If user Is Nothing Then
				Throw New ArgumentException("Cannot find registered users by the provided email address!")
			End If
			Dim randomBytes(5) As Byte
			CType(New RNGCryptoServiceProvider(), RNGCryptoServiceProvider).GetBytes(randomBytes)
			Dim password As String = Convert.ToBase64String(randomBytes)
			'Dennis: Resets the old password and generates a new one.
			user.SetPassword(password)
			user.ChangePasswordOnFirstLogon = True
			objectSpace.CommitChanges()
			EmailLoginInformation(Email, password)
		End Sub
		Public Shared Sub EmailLoginInformation(ByVal email As String, ByVal password As String)
			'Dennis: Send an email with the login details here. 
			'Refer to http://msdn.microsoft.com/en-us/library/system.net.mail.mailmessage.aspx for more details.
			'throw new UserFriendlyException("Password recovery link was sent to " + email);
		End Sub
	End Class
End Namespace
