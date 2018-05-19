#DropDB ManageUsersOnLogonEasyTest
;#Application ManageUsersOnLogon.Win
#Application ManageUsersOnLogon.Web

*Action 'Register User'
*Action OK
*CheckValidationResult
 Message = ?*

*FillForm
 User Name = 'Sam'
 Email = 'sam@example.com'
*Action OK
*CheckValidationResult
 Message = ?*

*FillForm
 User Name = 'test'
 Email = 'test@example.com'
*Action OK

*Action 'Restore Password'
*Action OK
*CheckValidationResult
 Message = ?*
*FillForm
 Email = 'test@example.com'
*Action OK

*FillForm
 User Name = 'test'
*Action Log On
