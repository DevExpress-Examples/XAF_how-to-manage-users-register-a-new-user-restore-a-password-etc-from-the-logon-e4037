using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using System.Security.Cryptography;
using DevExpress.Persistent.Validation;
using DevExpress.Persistent.Base.Security;

namespace Security.Extensions {
    [NonPersistent]
    [ModelDefault("Caption", "Restore Password")]
    [ImageName("Action_ResetPassword")]
    public class RestorePasswordParameters : ILogonActionParameters {
        public const string ValidationContext = "RestorePasswordContext";
        [RuleRequiredField(null, ValidationContext)]
        [RuleRegularExpression(null, ValidationContext, ManageUsersOnLogonController.EmailPattern)]
        public string Email { get; set; }
        public void Process(IObjectSpace objectSpace) {
            if (string.IsNullOrEmpty(Email))
                throw new ArgumentException("Email address is not specified!");
            IAuthenticationStandardUser user = objectSpace.FindObject(SecurityExtensionsModule.SecuritySystemUserType, CriteriaOperator.Parse("Email = ?", Email)) as IAuthenticationStandardUser;
            if (user == null)
                throw new ArgumentException("Cannot find registered users by the provided email address!");
            byte[] randomBytes = new byte[6];
            new RNGCryptoServiceProvider().GetBytes(randomBytes);
            string password = Convert.ToBase64String(randomBytes);
            //Dennis: Resets the old password and generates a new one.
            user.SetPassword(password);
            user.ChangePasswordOnFirstLogon = true;
            objectSpace.CommitChanges();
            EmailLoginInformation(Email, password);
        }
        public static void EmailLoginInformation(string email, string password) {
            //Dennis: Send an email with the login details here. 
            //Refer to http://msdn.microsoft.com/en-us/library/system.net.mail.mailmessage.aspx for more details.
            //throw new UserFriendlyException("Password recovery link was sent to " + email);
        }
    }
}
