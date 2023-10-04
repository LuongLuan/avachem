using System;
using System.Collections.Generic;
using System.Linq;
using AvaChemAdminPanelMobiAPI.Common_File;

namespace AvaChemAdminPanelMobiAPI
{
    public partial class UserDetails : System.Web.UI.Page
    {
        public const string DASHBOARD = Routes.DASHBOARD;
        public const string USERS = Routes.USERS;

        public User thisUser;
        public List<Qualification> listQ;
        //public List<Job> listJ;

        protected void Page_Load(object sender, EventArgs e)
        {
            // add this to everypage so User have to login to access to data/webpage
            // Session["admin"] the "admin" inside is a container to link to other pages
            // it's not connected to the class and dataconnector
            int role = (Convert.ToInt32(Session["role"]));
            if (Session["admin"] == null
                || !(new int[] { UserRoles.SuperAdmin.GetHashCode(), UserRoles.OverallAdmin.GetHashCode(), UserRoles.HR.GetHashCode() }).Contains(role))
            {
                Response.Redirect(Routes.LOG_IN);
            }

            if (Request.QueryString["id"] != null)
            {
                int id = Convert.ToInt32(Request.QueryString["id"]);
                thisUser = new UserDataConnector().GetUsersByFirstVariable(id);
                listQ = new QualificationDataConnector().GetByUserID(id).ToList<Qualification>();
                //listJ = new UserJobDataConnector().GetJobsByParams(id).ToList<Job>();
            }
            else
            {
                Response.Redirect(Routes.USERS);
            }


            if (!IsPostBack)
            {

                tbxRole.Text = new UserRoleDataConnector().GetUserRoleByFirstVariable(thisUser.RoleID).RoleName;
                tbxUsername.Text = thisUser.Username;
                tbxName.Text = thisUser.Name;
                tbxIDNumber.Text = thisUser.IDNumber;
                tbxEmail.Text = thisUser.Email;
                tbxPhone.Text = thisUser.Phone;
                //tbxDob.Text = thisUser.DOB.ToString("yyyy/MM/dd");
                tbxCredits.Text = thisUser.Credits.ToString();
                tbxLeaveDaysLeft.Text = thisUser.LeaveDaysLeft.ToString();
                tbxMCDaysLeft.Text = thisUser.MCDaysLeft.ToString();

            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Routes.USERS);
        }

    }
}