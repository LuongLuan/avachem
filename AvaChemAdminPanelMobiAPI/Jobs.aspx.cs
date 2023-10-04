using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using AvaChemAdminPanelMobiAPI.Common_File;

namespace AvaChemAdminPanelMobiAPI
{
    public partial class Jobs : System.Web.UI.Page
    {
        public const string DASHBOARD = Routes.DASHBOARD;
        public const string JOBS = Routes.JOBS;
        public const string JOB_DETAILS = Routes.JOB_DETAILS;
        public const string CREATE_JOB = Routes.CREATE_JOB;
        public const string UPDATE_JOB = Routes.UPDATE_JOB;

        public List<Job> list;
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
                successLabel.InnerText = "Job has been created";
                successLabel.Visible = true;
            }
            else if (Request.QueryString["message"] == "2")
            {
                successLabel.InnerText = "Job has been updated";
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
                for (int i = 1; i < 13; i++)
                {
                    var item = new ListItem(DateTimeFormatInfo.CurrentInfo.GetMonthName(i), i.ToString());
                    ddlMonth.Items.Add(item);
                }
                ddlMonth.Items.Insert(0, new ListItem("-- Month --", "-1"));

                for (int i = 2022; i < 2023; i++)
                {
                    var item = new ListItem(i.ToString(), i.ToString());
                    ddlYear.Items.Add(item);
                }
                ddlYear.Items.Insert(0, new ListItem("-- Year --", "-1"));

                try
                {
                    //var a = ddlMonth.SelectedValue.NullIfWhiteSpace() == "-1";
                    int type = Convert.ToInt32(Request.QueryString["type"].NullIfWhiteSpace());
                    string search = Request.QueryString["search"].NullIfWhiteSpace();
                    //string from = Request.QueryString["from"].NullIfWhiteSpace();
                    //string to = Request.QueryString["to"].NullIfWhiteSpace();
                    int month = Request.QueryString["month"] == null ? DateTime.Now.Month : Convert.ToInt32(Request.QueryString["month"].NullIfWhiteSpace());
                    int year = Request.QueryString["year"] == null ? DateTime.Now.Year : Convert.ToInt32(Request.QueryString["year"].NullIfWhiteSpace());
                    list = new JobDataConnector().GetAll(null, null, search, type, month, year).ToList<Job>();

                    tbxSearch.Text = search;
                    //dtStartDate.Value = from;
                    //dtEndDate.Value = to;
                    ddlMonth.SelectedValue = month.ToString();
                    ddlYear.SelectedValue = year.ToString();
                    //if (Request.QueryString["search"] != null)
                    //{
                    //    list = list.Where(u => (
                    //        u.JobNumber.ToLower().Contains(search.ToLower())
                    //        || u.Name.ToLower().Contains(search.ToLower())
                    //        || u.Location.ToLower().Contains(search.ToLower())
                    //    )).ToList();
                    //}
                }
                catch (Exception)
                {
                }
            }
        }

        protected void deleteConfirm_Click(object sender, EventArgs e)
        {
            string id = Request.Form["deleteInput"];
            new JobDataConnector().UpdateJobSoftDelete(Convert.ToInt32(id));

            // Log
            Utils.Log(new CreateLogDTO
            {
                ActionName = $@"Deletes a job (JobID: {id})",
                ActionLocation = Page.AppRelativeVirtualPath,
                ActionType = LogActionTypes.SoftDelete.GetHashCode(),
                ActionMeta = new LogActionMeta
                {
                    Input = new { JobID = id }
                }
            }, Session);

            Response.Redirect(Routes.JOBS);
        }
        protected void lbtnSearch_Click(object sender, EventArgs e)
        {

            Response.Redirect(Utils.GetRedirectUrl(Routes.JOBS, null, new
            {
                type = Request.QueryString["type"].NullIfWhiteSpace(),
                search = tbxSearch.Text.NullIfWhiteSpace(),
                //from = dtStartDate.Value.NullIfWhiteSpace(),
                //to = dtEndDate.Value.NullIfWhiteSpace(),
                month = ddlMonth.SelectedValue.NullIfWhiteSpace() == "-1" ? "" : ddlMonth.SelectedValue,
                year = ddlYear.SelectedValue.NullIfWhiteSpace() == "-1" ? "" : ddlYear.SelectedValue,
            }));
        }
    }
}