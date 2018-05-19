using System;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Persistent.Base.Security;

namespace ManageUsersOnLogon.Module.DatabaseUpdate {
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            #region Create Users for the Complex Security Strategy
            CreateUser(ObjectSpace, "Sam", "sam@example.com", string.Empty, true);
            CreateUser(ObjectSpace, "John", "john@example.com", string.Empty, false);
            ObjectSpace.CommitChanges();
            #endregion
        }
        public static IAuthenticationStandardUser CreateUser(IObjectSpace os, string userName, string email, string password, bool isAdministrator) {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(email))
                throw new ArgumentException("UserName and Email address are not specified!");
            SecuritySystemUser user = os.FindObject<SecuritySystemUser>(new BinaryOperator("UserName", userName));
            if (user == null) {
                user = os.CreateObject<SecuritySystemUser>();
                user.UserName = userName;
                user.IsActive = true;
                user.SetMemberValue("Email", email);
                user.SetPassword(password);
                SecuritySystemRole role = isAdministrator ? GetAdministratorRole(os) : GetDefaultRole(os);
                user.Roles.Add(role);
                user.Save();
                if (Validator.RuleSet.ValidateTarget(user, DefaultContexts.Save).State == ValidationState.Valid)
                    os.CommitChanges();
            }
            return user;
        }
        public static SecuritySystemRole GetAdministratorRole(IObjectSpace os) {
            SecuritySystemRole administratorRole = os.FindObject<SecuritySystemRole>(new BinaryOperator("Name", "Administrator"));
            if (administratorRole == null) {
                administratorRole = os.CreateObject<SecuritySystemRole>();
                administratorRole.Name = "Administrator";
                administratorRole.IsAdministrative = true;
            }
            return administratorRole;
        }
        public static SecuritySystemRole GetDefaultRole(IObjectSpace os) {
            SecuritySystemRole defaultRole = os.FindObject<SecuritySystemRole>(new BinaryOperator("Name", "Default"));
            if (defaultRole == null) {
                defaultRole = os.CreateObject<SecuritySystemRole>();
                defaultRole.Name = "Default";

                SecuritySystemTypePermissionObject securityDemoUserPermissions = os.CreateObject<SecuritySystemTypePermissionObject>();
                securityDemoUserPermissions.TargetType = typeof(SecuritySystemUser);
                defaultRole.TypePermissions.Add(securityDemoUserPermissions);

                SecuritySystemObjectPermissionsObject myDetailsPermission = os.CreateObject<SecuritySystemObjectPermissionsObject>();
                myDetailsPermission.Criteria = "[Oid] = CurrentUserId()";
                myDetailsPermission.AllowNavigate = true;
                myDetailsPermission.AllowRead = true;
                securityDemoUserPermissions.ObjectPermissions.Add(myDetailsPermission);

                SecuritySystemTypePermissionObject userPermissions = os.CreateObject<SecuritySystemTypePermissionObject>();
                userPermissions.TargetType = typeof(SecuritySystemUser);
                defaultRole.TypePermissions.Add(userPermissions);

                SecuritySystemMemberPermissionsObject ownPasswordPermission = os.CreateObject<SecuritySystemMemberPermissionsObject>();
                ownPasswordPermission.Members = "ChangePasswordOnFirstLogon; StoredPassword";
                ownPasswordPermission.AllowWrite = true;
                userPermissions.MemberPermissions.Add(ownPasswordPermission);

                SecuritySystemTypePermissionObject securityRolePermissions = os.CreateObject<SecuritySystemTypePermissionObject>();
                securityRolePermissions.TargetType = typeof(SecuritySystemRole);
                defaultRole.TypePermissions.Add(userPermissions);

                SecuritySystemObjectPermissionsObject defaultRolePermission = os.CreateObject<SecuritySystemObjectPermissionsObject>();
                defaultRolePermission.Criteria = "[Name] = 'Default'";
                defaultRolePermission.AllowNavigate = true;
                defaultRolePermission.AllowRead = true;
                securityRolePermissions.ObjectPermissions.Add(defaultRolePermission);
            }
            return defaultRole;
        }
    }
}