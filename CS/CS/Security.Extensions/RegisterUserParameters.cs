using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using DevExpress.Persistent.Base.Security;

namespace Security.Extensions {
    [NonPersistent]
    [ModelDefault("Caption", "Register User")]
    [ImageName("BO_User")]
    public class RegisterUserParameters : ILogonActionParameters {
        public const string ValidationContext = "RegisterUserContext";
        [RuleRequiredField(null, ValidationContext)]
        public string UserName { get; set; }
        public string Password { get; set; }
        [RuleRequiredField(null, ValidationContext)]
        [RuleRegularExpression(null, ValidationContext, ManageUsersOnLogonController.EmailPattern)]
        public string Email { get; set; }
        public void Process(IObjectSpace objectSpace) {
            IAuthenticationStandardUser user = objectSpace.FindObject(SecurityExtensionsModule.SecuritySystemUserType, new BinaryOperator("UserName", UserName)) as IAuthenticationStandardUser;
            if (user != null)
                throw new ArgumentException("The login with the entered UserName or Email was already registered within the system");
            else
                SecurityExtensionsModule.CreateSecuritySystemUser(objectSpace, UserName, Email, Password, false);
            //throw new UserFriendlyException("A new user has successfully been registered");
        }
    }
}
