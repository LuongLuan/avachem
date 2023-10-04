using System;
using AvaChemAdminPanelMobiAPI.Common_File;

namespace AvaChemAdminPanelMobiAPI
{
    public partial class Profile : System.Web.UI.Page
    {
        public const string DASHBOARD = Routes.DASHBOARD;
        public const string PROFILE = Routes.PROFILE;

        public User thisUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            // add this to everypage so User have to login to access to data/webpage
            // Session["admin"] the "admin" inside is a container to link to other pages
            // it's not connected to the class and dataconnector
            if (Session["admin"] == null) { Response.Redirect(Routes.LOG_IN); }

            // when page load it will hide the succuss and error color label for the message first
            successLabel.Visible = false;
            warningLabel.Visible = false;

            try
            {
                var userSession = Session["admin"] as User;
                thisUser = new UserDataConnector().GetUsersByFirstVariable(userSession.ID);
            }
            catch (Exception)
            {
                Response.Redirect(Routes.DASHBOARD);
            }

            //
            if (Request.QueryString["message"] == "2")
            {
                successLabel.InnerText = "Profile has been updated";
                successLabel.Visible = true;
            }
            else if (Request.QueryString["message"] == "6")
            {

                warningLabel.InnerText = "Please fill in the blanks!";
                warningLabel.Visible = true;
            }
            else if (Request.QueryString["message"] == "7")
            {
                warningLabel.InnerText = "Email already exists";
                warningLabel.Visible = true;
            }
            else if (Request.QueryString["message"] == "8")
            {
                warningLabel.InnerText = "Username already exists";
                warningLabel.Visible = true;
            }
            else if (Request.QueryString["message"] == "99")
            {
                warningLabel.InnerText = "Something went wrong!";
                warningLabel.Visible = true;
            }

            if (!IsPostBack)
            {
                tbxUsername.Text = thisUser.Username;
                tbxIDNumber.Text = thisUser.IDNumber;
                tbxName.Text = thisUser.Name;
                tbxPhone.Text = thisUser.Phone;
                tbxEmail.Text = thisUser.Email;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Routes.DASHBOARD);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var userSession = Session["admin"] as User;

            if (tbxUsername.Text == ""
                || tbxIDNumber.Text == ""
                || tbxName.Text == ""
                || tbxPhone.Text == ""
                //|| tbxEmail.Text == ""
                )
            {
                //Response.Redirect(Utils.GetRedirectUrl(Routes.PROFILE, 6), false);
                return;
            }
            try
            {

                var userConnector = new UserDataConnector();
                //if (thisUser.Email != tbxEmail.Text)
                //{
                //    if (userConnector.CheckEmailExists(tbxEmail.Text) == true)
                //    {
                //        Response.Redirect(Utils.GetRedirectUrl(Routes.PROFILE, 7), false);
                //        return;
                //    }
                //}

                if (thisUser.Username != tbxUsername.Text)
                {
                    if (userConnector.CheckUsernameExists(tbxUsername.Text) == true)
                    {
                        Response.Redirect(Utils.GetRedirectUrl(Routes.PROFILE, 8), false);
                        return;
                    }
                }

                User prepareUser = new User()
                {
                    ID = userSession.ID,
                    RoleID = thisUser.RoleID,
                    Username = tbxUsername.Text,
                    Password = thisUser.Password,
                    IDNumber = tbxIDNumber.Text,
                    Name = tbxName.Text,
                    //DOB = thisUser.DOB,
                    Phone = tbxPhone.Text,
                    Email = tbxEmail.Text,
                    Credits = thisUser.Credits,
                    LeaveDaysLeft = thisUser.LeaveDaysLeft,
                    MCDaysLeft = thisUser.MCDaysLeft,
                    UserStatus = 1,
                    SoftDelete = false
                };
                new UserDataConnector().UpdateUser(prepareUser);

                // Log
                Utils.Log(new CreateLogDTO
                {
                    ActionName = $@"Updates his profile",
                    ActionLocation = Page.AppRelativeVirtualPath,
                    ActionType = LogActionTypes.Update.GetHashCode(),
                    ActionMeta = new LogActionMeta
                    {
                        Input = prepareUser
                    }
                }, Session);

                Response.Redirect(Utils.GetRedirectUrl(Routes.PROFILE, 2), false);
            }
            catch (Exception)
            {
                Response.Redirect(Utils.GetRedirectUrl(Routes.PROFILE, 99), false);
            }
        }
    }
}