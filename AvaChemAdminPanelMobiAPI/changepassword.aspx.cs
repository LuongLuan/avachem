using System;
using AvaChemAdminPanelMobiAPI.Common_File;

namespace AvaChemAdminPanelMobiAPI
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        public const string DASHBOARD = Routes.DASHBOARD;

        protected void Page_Load(object sender, EventArgs e)
        {
            // add this to everypage so User have to login to access to data/webpage
            // Session["admin"] the "admin" inside is a container to link to other pages
            // it's not connected to the class and dataconnector
            if (Session["admin"] == null) { Response.Redirect(Routes.LOG_IN); }

            // when page load it will hide the succuss and error color label for the message first
            successLabel.Visible = false;
            warningLabel.Visible = false;


            // upon creating successfully it will display the success msg with the success label
            if (Request.QueryString["message"] == "1")
            {
                successLabel.InnerText = "Password has been updated";
                successLabel.Visible = true;
            }
            else if (Request.QueryString["message"] == "2")
            {
                warningLabel.InnerText = "New password confirmation is incorrect";
                warningLabel.Visible = true;
            }
            else if (Request.QueryString["message"] == "6")
            {
                warningLabel.InnerText = "Please fill in the blanks!";
                warningLabel.Visible = true;
            }
            else if (Request.QueryString["message"] == "4")
            {
                warningLabel.InnerText = "Username or password is incorrect!";
                warningLabel.Visible = true;
            }
            else if (Request.QueryString["message"] == "99")
            {
                warningLabel.InnerText = "Something went wrong!";
                warningLabel.Visible = true;
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Routes.DASHBOARD);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (tbxPassword.Text == "" || tbxNewPassword.Text == "" || tbxComfirmNewPassword.Text == "")
            {
                //Response.Redirect(Utils.GetRedirectUrl(Routes.CHANGE_PASSWORD, 6), false);
                return;
            }
            try
            {
                if (tbxNewPassword.Text != tbxComfirmNewPassword.Text)
                {
                    Response.Redirect(Utils.GetRedirectUrl(Routes.CHANGE_PASSWORD, 2), false);
                    return;
                }

                var userSession = Session["admin"] as User;
                User thisUser = new UserDataConnector().GetUserByUsernameAndPassword(userSession.Username, Utils.Hash(tbxPassword.Text));
                if (thisUser == null)
                {
                    Response.Redirect(Utils.GetRedirectUrl(Routes.CHANGE_PASSWORD, 4), false);
                    return;

                }

                new UserDataConnector().UpdatePasswored(userSession.ID, Utils.Hash(tbxNewPassword.Text));

                // Log
                Utils.Log(new CreateLogDTO
                {
                    ActionName = $@"Changes his password",
                    ActionLocation = Page.AppRelativeVirtualPath,
                    ActionType = LogActionTypes.Update.GetHashCode(),
                    ActionMeta = null
                }, Session);

                Response.Redirect(Utils.GetRedirectUrl(Routes.CHANGE_PASSWORD, 1), false);
            }
            catch (Exception)
            {
                Response.Redirect(Utils.GetRedirectUrl(Routes.CHANGE_PASSWORD, 99), false);
            }
        }
    }
}
