<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageOffence.master" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<script type="text/javascript">
    div_content_place_holder.class = "";
    lbl = "";
</script>

        <form id="form1" runat="server">
    <div>

    
            <h1>Change Password</h1>
            <asp:Label
                id="FailureText"
                EnableViewState="false"
                Visible=""
                ForeColor="Red"
                Runat="server" />
            <br />
            <asp:Label
                ID="lblUserName"
                Text="User Name:"
                runat="server" />
            <br />
            <asp:TextBox
                id="UserName"
                Runat="server" />
            <br /><br />
            <asp:Label
                id="lblCurrentPassword"
                Text="Current Password:"
                
                Runat="server" />
            <br />
            <asp:TextBox
                id="CurrentPassword"
                TextMode="Password"
                Runat="server" />
            <br /><br />
            <asp:Label
                id="lblNewPassword"
                Text="New Password:"
                Runat="server" />
            <br />
            <asp:TextBox
                id="NewPassword"
                TextMode="Password"
                Runat="server" />
            <br /><br />
            <asp:Button
                id="btnChangePassword"
                Text="Change Password"
                Runat="server" OnClick="btnChangePassword_Click" />
      
    </div>
    </form>


</asp:Content>

