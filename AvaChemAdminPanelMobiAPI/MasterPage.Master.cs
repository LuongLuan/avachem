using System;
using System.Web;
using AvaChemAdminPanelMobiAPI.Common_File;

namespace AvaChemAdminPanelMobiAPI
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        public const string DASHBOARD = Routes.DASHBOARD;
        public const string USERS = Routes.USERS;
        public const string JOBS = Routes.JOBS;
        public const string OT = Routes.OT;
        public const string LEAVES = Routes.LEAVES;
        public const string CREDITS = Routes.CREDITS;
        public const string CLIENTS = Routes.CLIENTS;
        public const string VEHICLES = Routes.VEHICLES;
        public const string CALENDAR = Routes.CALENDAR;
        public const string PROFILE = Routes.PROFILE;
        public const string CHANGE_PASSWORD = Routes.CHANGE_PASSWORD;
        public const string LOG_IN = Routes.LOG_IN;

        public User u;

        public int role;

        protected void Page_Load(object sender, EventArgs e)
        {
            // first code redirect user to session/login page if failed to login(wrong username and password)
            if (Session["admin"] == null) { Response.Redirect(Routes.LOG_IN); }
            u = Session["admin"] as User;
            role = Convert.ToInt32(Session["role"]);

            //on page load to highlight current webpage icon
            CheckWhichNavToHighlight();
        }

        /// <summary>
        /// to highlight selected icon
        /// </summary>
        protected void CheckWhichNavToHighlight()
        {
            if (HttpContext.Current.Request.Url.AbsoluteUri.Contains(Routes.DASHBOARD))
            {
                resetLinks();
                navTabHome.Attributes["class"] = "nav-item start active open";
                navArrowHome.Attributes["class"] = "arrow open";
            }
            else if (HttpContext.Current.Request.Url.AbsoluteUri.Contains(Routes.USERS))
            {
                resetLinks();
                navTabUsers.Attributes["class"] = "nav-item start active open";
                navArrowUsers.Attributes["class"] = "arrow open";
            }
            else if (HttpContext.Current.Request.Url.AbsoluteUri.Contains(Routes.CREDITS))
            {
                resetLinks();
                navTabCredits.Attributes["class"] = "nav-item start active open";
                navArrowCredit.Attributes["class"] = "arrow open";
            }
            else if (HttpContext.Current.Request.Url.AbsoluteUri.Contains(Routes.LEAVES))
            {
                resetLinks();
                navTabLeaves.Attributes["class"] = "nav-item start active open";
                navArrowLeaveList.Attributes["class"] = "arrow open";
            }
            else if (HttpContext.Current.Request.Url.AbsoluteUri.Contains(Routes.JOBS))
            {
                resetLinks();
                navTabJobs.Attributes["class"] = "nav-item start active open";
                navArrowJob.Attributes["class"] = "arrow open";
            }
            else if (HttpContext.Current.Request.Url.AbsoluteUri.Contains(Routes.OT))
            {
                resetLinks();
                navTabOT.Attributes["class"] = "nav-item start active open";
                navArrowOT.Attributes["class"] = "arrow open";
            }
            else if (HttpContext.Current.Request.Url.AbsoluteUri.Contains(Routes.CLIENTS))
            {
                resetLinks();
                navTabClients.Attributes["class"] = "nav-item start active open";
                navArrowClients.Attributes["class"] = "arrow open";
            }
            else if (HttpContext.Current.Request.Url.AbsoluteUri.Contains(Routes.VEHICLES))
            {
                resetLinks();
                navTabVehicles.Attributes["class"] = "nav-item start active open";
                navArrowVehicles.Attributes["class"] = "arrow open";
            }
            else if (HttpContext.Current.Request.Url.AbsoluteUri.Contains(Routes.CALENDAR))
            {
                resetLinks();
                navTabCalendar.Attributes["class"] = "nav-item start active open";
                navArrowCalendar.Attributes["class"] = "arrow open";
            }
        }

        protected void resetLinks()
        {
            navTabHome.Attributes["class"] = "nav-item";
            navArrowHome.Attributes["class"] = "arrow open";

            navTabUsers.Attributes["class"] = "nav-item";
            navArrowUsers.Attributes["class"] = "arrow open";

            navTabCredits.Attributes["class"] = "nav-item";
            navArrowCredit.Attributes["class"] = "arrow open";

            navTabLeaves.Attributes["class"] = "nav-item";
            navArrowLeaveList.Attributes["class"] = "arrow open";

            navTabJobs.Attributes["class"] = "nav-item";
            navArrowJob.Attributes["class"] = "arrow open";

            navTabOT.Attributes["class"] = "nav-item";
            navArrowOT.Attributes["class"] = "arrow open";

            navTabClients.Attributes["class"] = "nav-item";
            navArrowClients.Attributes["class"] = "arrow open";

            navTabVehicles.Attributes["class"] = "nav-item";
            navArrowVehicles.Attributes["class"] = "arrow open";

            navTabCalendar.Attributes["class"] = "nav-item";
            navArrowCalendar.Attributes["class"] = "arrow open";
        }


        protected void lbtnLogOut_Click(object sender, EventArgs e)
        {

            Session["admin"] = null;
            Response.Redirect(Utils.GetRedirectUrl(Routes.LOG_IN, 8));
        }
        protected void lbtnProfile_Click(object sender, EventArgs e)
        {
            if (Session["admin"] != null)
            {
                Response.Redirect(Routes.PROFILE);
            }

        }
        protected void lbtnChangePassword_Click(object sender, EventArgs e)
        {
            if (Session["admin"] != null)
            {
                Response.Redirect(Routes.CHANGE_PASSWORD);
            }
        }
    }
}