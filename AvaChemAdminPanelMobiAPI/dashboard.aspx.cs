using System;
using AvaChemAdminPanelMobiAPI.Common_File;

namespace AvaChemAdminPanelMobiAPI
{

    public partial class Dashboard : System.Web.UI.Page
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

        public int userCount;
        public int jobCount;
        public int OTCount;
        public int leaveCount;
        public int creditCount;
        public int clientCount;
        public int vehicleCount;

        public int role;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["admin"] == null) { Response.Redirect(Routes.LOG_IN); }
            role = (Convert.ToInt32(Session["role"]));

            // when page load it will hide the succuss and error color label for the message first
            successLabel.Visible = false;
            warningLabel.Visible = false;

            userCount = new UserDataConnector().CountAll();
            jobCount = new JobDataConnector().CountAll();
            OTCount = new OTDataConnector().CountAll();
            leaveCount = new LeaveDataConnector().CountAll();
            creditCount = new CreditDataConnector().CountAll();
            clientCount = new ClientDataConnector().CountAll();
            vehicleCount = new VehicleDataConnector().CountAll();

        }
    }
}
