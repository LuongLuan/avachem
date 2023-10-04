using System;
using System.Collections.Generic;
using System.Linq;
using AvaChemAdminPanelMobiAPI.Common_File;

namespace AvaChemAdminPanelMobiAPI
{
    public partial class Clients : System.Web.UI.Page
    {
        public const string DASHBOARD = Routes.DASHBOARD;
        public const string CREATE_CLIENT = Routes.CREATE_CLIENT;
        public const string UPDATE_CLIENT = Routes.UPDATE_CLIENT;

        public List<Client> list;
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
                successLabel.InnerText = "Client has been created";
                successLabel.Visible = true;
            }
            else if (Request.QueryString["message"] == "2")
            {
                successLabel.InnerText = "Client has been updated";
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
                    list = new ClientDataConnector().GetAll(null, null, search);

                    tbxSearch.Text = search;

                }
                catch (Exception)
                {
                }
            }
        }

        protected void deleteConfirm_Click(object sender, EventArgs e)
        {
            string id = Request.Form["deleteInput"];
            new ClientDataConnector().UpdateClientSoftDelete(Convert.ToInt32(id));

            // Log
            Utils.Log(new CreateLogDTO
            {
                ActionName = $@"Deletes a client (ClientID: {id})",
                ActionLocation = Page.AppRelativeVirtualPath,
                ActionType = LogActionTypes.SoftDelete.GetHashCode(),
                ActionMeta = new LogActionMeta
                {
                    Input = new { ClientID = id }
                }
            }, Session);

            Response.Redirect(Routes.CLIENTS);
        }
        protected void lbtnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.GetRedirectUrl(Routes.CLIENTS, null, new
            {
                search = tbxSearch.Text.NullIfWhiteSpace(),
            }));
        }
    }
}