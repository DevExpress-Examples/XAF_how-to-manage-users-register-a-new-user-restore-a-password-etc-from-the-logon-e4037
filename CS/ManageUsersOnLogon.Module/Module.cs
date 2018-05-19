using System;
using Security.Extensions;
using DevExpress.ExpressApp;
using System.Collections.Generic;
using ManageUsersOnLogon.Module.DatabaseUpdate;

namespace ManageUsersOnLogon.Module {
    public sealed partial class ManageUsersOnLogonModule : ModuleBase {
        public ManageUsersOnLogonModule() {
            InitializeComponent();
        }
        static ManageUsersOnLogonModule() {
            SecurityExtensionsModule.CreateSecuritySystemUser = Updater.CreateUser;
        }
    }
}
