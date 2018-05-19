Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Utils
Imports DevExpress.ExpressApp.Actions
Imports DevExpress.ExpressApp.Editors
Imports DevExpress.ExpressApp.Templates
Imports DevExpress.ExpressApp.SystemModule

Namespace Security.Extensions
	Public Class ManageUsersOnLogonController
		Inherits ViewController(Of DetailView)
		Protected Const LogonActionParametersActiveKey As String = "Active for ILogonActionParameters only"
		Public Const EmailPattern As String= "^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$"
		Private saRestorePassword As SimpleAction
		Private saRegisterUser As SimpleAction
		Public Sub New()
			'Dennis: Initialization of the Actions placed within the login window layout.
			saRegisterUser = CreateLogonSimpleAction("RegisterUser", "RegisterUserCategory", "Register User", "BO_User", "Register a new user within the system", GetType(RegisterUserParameters))
			saRestorePassword = CreateLogonSimpleAction("RestorePassword", "RestorePasswordCategory", "Restore Password", "Action_ResetPassword", "Restore forgotten login information", GetType(RestorePasswordParameters))
		End Sub
		'Dennis: Ensures that our controller is active only when a user is not logged on.
		Protected Overrides Sub OnViewChanging(ByVal view As View)
			MyBase.OnViewChanging(view)
			Active(ControllerActiveKey) = Not SecuritySystem.IsAuthenticated
		End Sub
		'Dennis: Manages the activity of Actions within the logon window depending on the current context.
		Protected Overrides Sub OnViewControlsCreated()
			MyBase.OnViewControlsCreated()
			Dim flag As Boolean = GetLogonParametersActiveState()
			For Each item As Controller In Frame.Controllers
				Dim lc As LogonController = TryCast(item, LogonController)
				If lc IsNot Nothing Then
					lc.AcceptAction.Active(LogonActionParametersActiveKey) = Not flag
					lc.CancelAction.Active(LogonActionParametersActiveKey) = Not flag
				Else
					Dim dc As DialogController = TryCast(item, DialogController)
					If dc IsNot Nothing Then
						dc.AcceptAction.Active(LogonActionParametersActiveKey) = flag
						dc.CancelAction.Active(LogonActionParametersActiveKey) = flag
						ConfigureDialogController(dc)
					End If
				End If
			Next item
		End Sub
		'Dennis: Creates a SimpleAction using the specified parameters.
		Private Function CreateLogonSimpleAction(ByVal id As String, ByVal category As String, ByVal caption As String, ByVal imageName As String, ByVal toolTip As String, ByVal parametersType As Type) As SimpleAction
			Dim action As New SimpleAction(Me, id, category)
			action.Caption = caption
			action.ImageName = imageName
			action.PaintStyle = ActionItemPaintStyle.Image
			action.ToolTip = toolTip
			AddHandler action.Execute, AddressOf CreateParametersViewDelegate
			action.Tag = parametersType
			Return action
		End Function
		'Dennis: Fires when our Actions are executed.
		Private Sub CreateParametersViewDelegate(ByVal sender As Object, ByVal e As SimpleActionExecuteEventArgs)
			CreateParametersViewCore(e)
		End Sub
		'Dennis: Configures a View used to display our parameters objects. 
		Protected Overridable Sub CreateParametersViewCore(ByVal e As SimpleActionExecuteEventArgs)
			Dim parametersType As Type = TryCast(e.Action.Tag, Type)
			Guard.ArgumentNotNull(parametersType, "parametersType")
			Dim dv As DetailView = Application.CreateDetailView(ObjectSpaceInMemory.CreateNew(), Activator.CreateInstance(parametersType))
			dv.ViewEditMode = ViewEditMode.Edit
			e.ShowViewParameters.CreatedView = dv
			'Dennis: TODO
			'A possible issue in the framework - Controllers from ShowViewParameters are not added to the current Frame on the Web. 
			'e.ShowViewParameters.Controllers.Add(CreateDialogController());
			e.ShowViewParameters.TargetWindow = TargetWindow.Current
		End Sub
		Protected Overridable Sub ConfigureDialogController(ByVal dialogController As DialogController)
			RemoveHandler dialogController.AcceptAction.Execute, AddressOf AcceptAction_Execute
			RemoveHandler dialogController.CancelAction.Execute, AddressOf CancelAction_Execute
			AddHandler dialogController.AcceptAction.Execute, AddressOf AcceptAction_Execute
			AddHandler dialogController.CancelAction.Execute, AddressOf CancelAction_Execute
			dialogController.Tag = GetType(ILogonActionParameters)
		End Sub
		'Dennis: Configures a DialogController that provides the Accept and Cancel Actions in the View used to display our parameters objects.
		Protected Function CreateDialogController() As DialogController
			Dim dialogController As DialogController = Application.CreateController(Of DialogController)()
			ConfigureDialogController(dialogController)
			Return dialogController
		End Function
		'Dennis: Fires when the Accept Action is executed in the View used to display our parameters objects.
		Private Sub AcceptAction_Execute(ByVal sender As Object, ByVal e As SimpleActionExecuteEventArgs)
			AcceptParameters(TryCast(e.CurrentObject, ILogonActionParameters))
		End Sub
		'Dennis: Fires when the Cancel Action is executed in the View used to display our parameters objects.
		Private Sub CancelAction_Execute(ByVal sender As Object, ByVal e As SimpleActionExecuteEventArgs)
			CancelParameters(TryCast(e.CurrentObject, ILogonActionParameters))
		End Sub
		Protected Overridable Sub AcceptParameters(ByVal parameters As ILogonActionParameters)
			'Dennis: Our parameters objects provide different accepting strategies. 
			If parameters IsNot Nothing Then
				parameters.Process(Application.CreateObjectSpace())
			End If
			Application.LogOff()
		End Sub
		Protected Overridable Sub CancelParameters(ByVal parameters As ILogonActionParameters)
			Application.LogOff()
		End Sub
		'Dennis: Determines whether we are in the context of the LogonActionParametersBase object.
		Protected Overridable Function GetLogonParametersActiveState() As Boolean
			Return View IsNot Nothing AndAlso View.ObjectTypeInfo IsNot Nothing AndAlso View.ObjectTypeInfo.Implements(Of ILogonActionParameters)()
		End Function
		Public ReadOnly Property RestorePasswordAction() As SimpleAction
			Get
				Return saRestorePassword
			End Get
		End Property
		Public ReadOnly Property RegisterUserAction() As SimpleAction
			Get
				Return saRegisterUser
			End Get
		End Property
	End Class
	'Dennis: A base class for our logon parameters objects.
	Public Interface ILogonActionParameters
		Sub Process(ByVal objectSpace As IObjectSpace)
	End Interface
End Namespace