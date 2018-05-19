using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.SystemModule;

namespace Security.Extensions {
    public class ManageUsersOnLogonController : ViewController<DetailView> {
        protected const string LogonActionParametersActiveKey = "Active for ILogonActionParameters only";
        public const string EmailPattern= @"^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$";
        private SimpleAction saRestorePassword;
        private SimpleAction saRegisterUser;
        public ManageUsersOnLogonController() {
            //Dennis: Initialization of the Actions placed within the login window layout.
            saRegisterUser = CreateLogonSimpleAction("RegisterUser", "RegisterUserCategory", "Register User", "BO_User", "Register a new user within the system", typeof(RegisterUserParameters));
            saRestorePassword = CreateLogonSimpleAction("RestorePassword", "RestorePasswordCategory", "Restore Password", "Action_ResetPassword", "Restore forgotten login information", typeof(RestorePasswordParameters));
        }
        //Dennis: Ensures that our controller is active only when a user is not logged on.
        protected override void OnViewChanging(View view) {
            base.OnViewChanging(view);
            Active[ControllerActiveKey] = !SecuritySystem.IsAuthenticated;
        }
        //Dennis: Manages the activity of Actions within the logon window depending on the current context.
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            bool flag = GetLogonParametersActiveState();
            foreach (Controller item in Frame.Controllers) {
                LogonController lc = item as LogonController;
                if (lc != null) {
                    lc.AcceptAction.Active[LogonActionParametersActiveKey] = !flag;
                    lc.CancelAction.Active[LogonActionParametersActiveKey] = !flag;
                }
                else {
                    DialogController dc = item as DialogController;
                    if (dc != null) {
                        dc.AcceptAction.Active[LogonActionParametersActiveKey] = flag;
                        dc.CancelAction.Active[LogonActionParametersActiveKey] = flag;
                        ConfigureDialogController(dc);
                    }
                }
            }
        }
        //Dennis: Creates a SimpleAction using the specified parameters.
        private SimpleAction CreateLogonSimpleAction(string id, string category, string caption, string imageName, string toolTip, Type parametersType) {
            SimpleAction action = new SimpleAction(this, id, category);
            action.Caption = caption;
            action.ImageName = imageName;
            action.PaintStyle = ActionItemPaintStyle.Image;
            action.ToolTip = toolTip;
            action.Execute += CreateParametersViewDelegate;
            action.Tag = parametersType;
            return action;
        }
        //Dennis: Fires when our Actions are executed.
        private void CreateParametersViewDelegate(object sender, SimpleActionExecuteEventArgs e) {
            CreateParametersViewCore(e);
        }
        //Dennis: Configures a View used to display our parameters objects. 
        protected virtual void CreateParametersViewCore(SimpleActionExecuteEventArgs e) {
            Type parametersType = e.Action.Tag as Type;
            Guard.ArgumentNotNull(parametersType, "parametersType");
            DetailView dv = Application.CreateDetailView(ObjectSpaceInMemory.CreateNew(), Activator.CreateInstance(parametersType));
            dv.ViewEditMode = ViewEditMode.Edit;
            e.ShowViewParameters.CreatedView = dv;
            //Dennis: TODO
            //A possible issue in the framework - Controllers from ShowViewParameters are not added to the current Frame on the Web. 
            //e.ShowViewParameters.Controllers.Add(CreateDialogController());
            e.ShowViewParameters.TargetWindow = TargetWindow.Current;
        }
        protected virtual void ConfigureDialogController(DialogController dialogController) {
            dialogController.AcceptAction.Execute -= AcceptAction_Execute;
            dialogController.CancelAction.Execute -= CancelAction_Execute;
            dialogController.AcceptAction.Execute += AcceptAction_Execute;
            dialogController.CancelAction.Execute += CancelAction_Execute;
            dialogController.Tag = typeof(ILogonActionParameters);
        }
        //Dennis: Configures a DialogController that provides the Accept and Cancel Actions in the View used to display our parameters objects.
        protected DialogController CreateDialogController() {
            DialogController dialogController = Application.CreateController<DialogController>();
            ConfigureDialogController(dialogController);
            return dialogController;
        }
        //Dennis: Fires when the Accept Action is executed in the View used to display our parameters objects.
        private void AcceptAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            AcceptParameters(e.CurrentObject as ILogonActionParameters);
        }
        //Dennis: Fires when the Cancel Action is executed in the View used to display our parameters objects.
        private void CancelAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            CancelParameters(e.CurrentObject as ILogonActionParameters);
        }
        protected virtual void AcceptParameters(ILogonActionParameters parameters) {
            //Dennis: Our parameters objects provide different accepting strategies. 
            if (parameters != null)
                parameters.Process(Application.CreateObjectSpace());
            Application.LogOff();
        }
        protected virtual void CancelParameters(ILogonActionParameters parameters) {
            Application.LogOff();
        }
        //Dennis: Determines whether we are in the context of the LogonActionParametersBase object.
        protected virtual bool GetLogonParametersActiveState() {
            return View != null && View.ObjectTypeInfo != null && View.ObjectTypeInfo.Implements<ILogonActionParameters>();
        }
        public SimpleAction RestorePasswordAction {
            get { return saRestorePassword; }
        }
        public SimpleAction RegisterUserAction {
            get { return saRegisterUser; }
        }
    }
    //Dennis: A base class for our logon parameters objects.
    public interface ILogonActionParameters {
        void Process(IObjectSpace objectSpace);
    }
}