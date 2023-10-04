using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using AvaChemAdminPanelMobiAPI.Common_File;

namespace AvaChemAdminPanelMobiAPI
{
    public partial class Leaves : System.Web.UI.Page
    {
        public const string DASHBOARD = Routes.DASHBOARD;
        public const string LEAVES = Routes.LEAVES;
        public const string CREATE_LEAVE = Routes.CREATE_LEAVE;
        public const string UPDATE_LEAVE = Routes.UPDATE_LEAVE;

        public List<LeaveTableView> list;
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
                successLabel.InnerText = "Leave has been created";
                successLabel.Visible = true;
            }
            else if (Request.QueryString["message"] == "2")
            {
                successLabel.InnerText = "Leave has been updated";
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

                for (int i = 2021; i < 2026; i++)
                {
                    var item = new ListItem(i.ToString(), i.ToString());
                    ddlYear.Items.Add(item);
                }
                ddlYear.Items.Insert(0, new ListItem("-- Year --", "-1"));

                try
                {
                    int type = Convert.ToInt32(Request.QueryString["type"].NullIfWhiteSpace());
                    string search = Request.QueryString["search"].NullIfWhiteSpace();
                    int month = Convert.ToInt32(Request.QueryString["month"].NullIfWhiteSpace());
                    int year = Convert.ToInt32(Request.QueryString["year"].NullIfWhiteSpace());
                    int sort = Convert.ToInt32(Request.QueryString["sort"].NullIfWhiteSpace());
                    list = new LeaveDataConnector().GetAll<LeaveTableView>(null, null, search, type, null, null, true, null, month, year, sort).ToList<LeaveTableView>();

                    tbxSearch.Text = search;
                    ddlMonth.SelectedValue = month.ToString();
                    ddlYear.SelectedValue = year.ToString();
                    ddlSort.SelectedValue = sort.ToString();
                }
                catch (Exception)
                {
                }
            }
        }

        protected void deleteConfirm_Click(object sender, EventArgs e)
        {
            string id = Request.Form["deleteInput"];
            new LeaveDataConnector().UpdateLeaveSoftDelete(Convert.ToInt32(id));

            // Log
            Utils.Log(new CreateLogDTO
            {
                ActionName = $@"Deletes a leave (LeaveID: {id})",
                ActionLocation = Page.AppRelativeVirtualPath,
                ActionType = LogActionTypes.SoftDelete.GetHashCode(),
                ActionMeta = new LogActionMeta
                {
                    Input = new { LeaveID = id }
                }
            }, Session);

            Response.Redirect(Routes.LEAVES);
        }
        protected void lbtnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.GetRedirectUrl(Routes.LEAVES, null, new
            {
                type = Request.QueryString["type"].NullIfWhiteSpace(),
                search = tbxSearch.Text.NullIfWhiteSpace(),
                month = ddlMonth.SelectedValue.NullIfWhiteSpace() == "-1" ? null : ddlMonth.SelectedValue,
                year = ddlYear.SelectedValue.NullIfWhiteSpace() == "-1" ? null : ddlYear.SelectedValue,
                sort = ddlSort.SelectedValue.NullIfWhiteSpace() == "-1" ? null : ddlSort.SelectedValue,
            }));
        }

        protected void ddlSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.GetRedirectUrl(Routes.LEAVES, null, new
            {
                type = Request.QueryString["type"].NullIfWhiteSpace(),
                search = tbxSearch.Text.NullIfWhiteSpace(),
                month = ddlMonth.SelectedValue.NullIfWhiteSpace() == "-1" ? null : ddlMonth.SelectedValue,
                year = ddlYear.SelectedValue.NullIfWhiteSpace() == "-1" ? null : ddlYear.SelectedValue,
                sort = ddlSort.SelectedValue.NullIfWhiteSpace() == "-1" ? null : ddlSort.SelectedValue,
            }));
        }
    }
}