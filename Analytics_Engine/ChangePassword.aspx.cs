using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ChangePassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
          //ChangingPassword += new LoginCancelEventHandler(this.ChangePassword1_ChangedPassword);
   
    }
    protected void btnChangePassword_Click(object sender, EventArgs e)
    {
        user_worker userWorker = new user_worker();
        string newPassword = NewPassword.Text;
        if (CurrentPassword.Text.Equals(NewPassword.Text))
        {
            FailureText.Visible = true;
            FailureText.Text = "Old password and new password must be different.  Please try again.";
            //e.Cancel = true;
           
        }
        else if (!userWorker.validateChangePassword(UserName.Text, CurrentPassword.Text))
        {
            FailureText.Visible = true;
            FailureText.Text = "Invalid current password.  Please try again.";
        }
        else
        {
            //This line prevents the error showing up after a first failed attempt.
            FailureText.Visible = false;
        }
    }
}