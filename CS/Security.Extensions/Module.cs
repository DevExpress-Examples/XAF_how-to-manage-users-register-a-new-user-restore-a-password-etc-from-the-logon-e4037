using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base.Security;

namespace Security.Extensions {
    public sealed partial class SecurityExtensionsModule : ModuleBase {
        internal static Type SecuritySystemUserType;
        public static CreateSecuritySystemUser CreateSecuritySystemUser;
        public SecurityExtensionsModule() {
            InitializeComponent();
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            application.CreateCustomLogonWindowControllers += application_CreateCustomLogonWindowControllers;
        }
        private void application_CreateCustomLogonWindowControllers(object sender, CreateCustomLogonWindowControllersEventArgs e) {
            XafApplication app = (XafApplication)sender;
            e.Controllers.Add(app.CreateController<ManageUsersOnLogonController>());
            e.Controllers.Add(app.CreateController<DevExpress.ExpressApp.Validation.ActionValidationController>());
            e.Controllers.Add(app.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>());
        }
        //Dennis: I want to avoid inheritance from SecuritySystemUser and will just add a new property dynamically.
        public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
            base.CustomizeTypesInfo(typesInfo);
            if(Application != null) {
                SecurityStrategy securityStrategy = Application.Security as SecurityStrategy;
                if(securityStrategy != null) {
                    SecuritySystemUserType = securityStrategy.UserType;
                }
                Guard.ArgumentNotNull(SecuritySystemUserType, "SecuritySystemUserType");
                ITypeInfo ti = typesInfo.FindTypeInfo(SecuritySystemUserType);
                if(ti != null) {
                    IMemberInfo mi = ti.FindMember("Email");
                    if(mi == null) {
                        mi = ti.CreateMember("Email", typeof(string));
                    }
                }
            }
        }
    }
    public delegate IAuthenticationStandardUser CreateSecuritySystemUser(IObjectSpace objectSpace, string userName, string email, string password, bool isAdministrator);
}
