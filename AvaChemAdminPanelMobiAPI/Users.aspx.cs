using System;
using System.Collections.Generic;
using System.Linq;
using AvaChemAdminPanelMobiAPI.Common_File;

namespace AvaChemAdminPanelMobiAPI
{
    public partial class Users : System.Web.UI.Page
    {
        public const string DASHBOARD = Routes.DASHBOARD;
        public const string USER_DETAILS = Routes.USER_DETAILS;
        public const string CREATE_USER = Routes.CREATE_USER;
        public const string UPDATE_USER = Routes.UPDATE_USER;

        public List<User> list;
        public int i = 0;
        public int role;

        protected void Page_Load(object sender, EventArgs e)
        {
            // add this to everypage so User have to login to access to data/webpage
            // Session["admin"] the "admin" inside is a container to link to other pages
            // it's not connected to the class and dataconnector
            role = (Convert.ToInt32(Session["role"]));
            if (Session["admin"] == null
                || !(new int[] { UserRoles.SuperAdmin.GetHashCode(), UserRoles.OverallAdmin.GetHashCode(), UserRoles.HR.GetHashCode() }).Contains(role))
            {
                Response.Redirect(Routes.LOG_IN);
            }

            // when page load it will hide the succuss and error color label for the message first
            successLabel.Visible = false;
            warningLabel.Visible = false;

            // upon creating successfully it will display the success msg with the success label
            if (Request.QueryString["message"] == "1")
            {
                successLabel.InnerText = "User has been created";
                successLabel.Visible = true;
            }
            else if (Request.QueryString["message"] == "2")
            {
                successLabel.InnerText = "User has been updated";
                successLabel.Visible = true;
            }
            else if (Request.QueryString["message"] == "6")
            {
                warningLabel.InnerText = "Please fill in the blanks!";
                warningLabel.Visible = true;
            }
            else if (Request.QueryString["message"] == "99")
            {
                warningLabel.InnerText = "Something went wrong!";
                warningLabel.Visible = true;
            }

            if (!IsPostBack)
            {
                try
                {
                    string search = Request.QueryString["search"].NullIfWhiteSpace();
                    list = new UserDataConnector().GetAll(null, null, search).ToList<User>();

                    tbxSearch.Text = search;
                    // if (Request.QueryString["search"] != null)
                    // {
                    //     string search = Request.QueryString["search"] as string;
                    //     tbxSearch.Text = search;
                    //     list = list.Where(u => (
                    //         u.IDNumber.ToLower().Contains(search.ToLower())
                    //         || u.Name.ToLower().Contains(search.ToLower())
                    //         || u.Email.ToLower().Contains(search.ToLower())
                    //         || u.Phone.ToLower().Contains(search.ToLower())
                    //     )).ToList();
                    // }
                }
                catch (Exception)
                {
                }
            }
        }

        public List<User> filterName(string name, List<User> list)
        {
            List<User> toReturned = new List<User>();
            foreach (User u in list)
                if ((u.Username.ToLower().Contains(name.ToLower().Trim())) || (u.Email.ToLower().Contains(name.ToLower().Trim())) || (u.Name.ToLower().Contains(name.ToLower().Trim())))
                    toReturned.Add(u);
            return toReturned;
        }

        protected void deleteConfirm_Click(object sender, EventArgs e)
        {
            string id = Request.Form["deleteInput"];
            new UserDataConnector().UpdateUserSoftDelete(Convert.ToInt32(id));

            // Log
            Utils.Log(new CreateLogDTO
            {
                ActionName = $@"Deletes an user (UserID: {id})",
                ActionLocation = Page.AppRelativeVirtualPath,
                ActionType = LogActionTypes.SoftDelete.GetHashCode(),
                ActionMeta = new LogActionMeta
                {
                    Input = new { UserID = id }
                }
            }, Session);

            Response.Redirect(Routes.USERS);
        }
        protected void lbtnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.GetRedirectUrl(Routes.USERS, null, new
            {
                search = tbxSearch.Text.NullIfWhiteSpace(),
            }));
        }
    }
}
