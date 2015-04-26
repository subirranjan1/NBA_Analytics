using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for user_worker
/// </summary>
public class user_worker
{
	 public Boolean validateLogin(string user, string password)
    {
            return      new user_management_dao().checkLogin(user, password);
    }
     public Boolean validateChangePassword(string user, string password)
     {
         return new user_management_dao().validateUpdatePasswordLogin(user, password);
     }
     public Boolean updatePassword(string user, string oldPassword, string newPassword)
     {
         return new user_management_dao().updatePassword(user, oldPassword, newPassword);
     }
}