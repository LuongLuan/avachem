using System;
using AvaChemAdminPanelMobiAPI.Common_File;

namespace AvaChemAdminPanelMobiAPI
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // when page load it will hide the succuss and error color label for the message first
            successLabel.Visible = false;
            warningLabel.Visible = false;

            // upon creating successfully it will display the success msg with the success label
            if (Request.QueryString["message"] == "4")
            {
                warningLabel.InnerText = "Unsuccessful Login! - Username or password is incorrect";
                warningLabel.Visible = true;
            }
            else if (Request.QueryString["message"] == "8")
            {
                successLabel.InnerText = "You have successfully logged out of your account";
                successLabel.Visible = true;
            }
            else if (Request.QueryString["message"] == "99")
            {
                warningLabel.InnerText = "Something went wrong!";
                warningLabel.Visible = true;
            }

            if (!IsPostBack)
            {
                Session["admin"] = null;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                User thisUser = new UserDataConnector().GetAdminByUsernameAndPassword(tbxUsername.Text, Utils.Hash(tbxPassword.Text));
                if (thisUser == null || thisUser.Username == "" || thisUser.Password == "")
                {
                    Response.Redirect(Utils.GetRedirectUrl(Routes.LOG_IN, 4), false);
                    return;
                }


                Session["admin"] = thisUser;
                Session["role"] = thisUser.RoleID;

                Utils.Log(new CreateLogDTO
                {
                    ActionName = $@"Logged into the system",
                    ActionLocation = Page.AppRelativeVirtualPath,
                    ActionType = LogActionTypes.Others.GetHashCode(),
                    ActionMeta = new LogActionMeta
                    {
                        Input = new { Username = tbxUsername.Text }
                    }
                }, Session);

                Response.Redirect(Routes.DASHBOARD, false);
            }
            catch (Exception)
            {

                Response.Redirect(Utils.GetRedirectUrl(Routes.LOG_IN, 99), false);
            }
        }
    }
}