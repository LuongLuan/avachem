using System;
using System.Collections.Generic;
using System.Linq;
using AvaChemAdminPanelMobiAPI.Common_File;

namespace AvaChemAdminPanelMobiAPI
{
    public partial class UpdateClient : System.Web.UI.Page
    {
        public const string DASHBOARD = Routes.DASHBOARD;
        public const string CLIENTS = Routes.CLIENTS;

        public Client thisClient;
        public List<Job> listJ;

        protected void Page_Load(object sender, EventArgs e)
        {
            // add this to everypage so User have to login to access to data/webpage
            // Session["admin"] the "admin" inside is a container to link to other pages
            // it's not connected to the class and dataconnector
            int role = (Convert.ToInt32(Session["role"]));
            if (Session["admin"] == null
                || !(new int[] { UserRoles.SuperAdmin.GetHashCode(), UserRoles.OverallAdmin.GetHashCode() }).Contains(role))
            {
                Response.Redirect(Routes.LOG_IN);
            }

            // when page load it will hide the succuss and error color label for the message first
            successLabel.Visible = false;
            warningLabel.Visible = false;

            // when update and value is null add if and else statement
            if (Request.QueryString["id"] != null)
            {
                //Id is a number so we have to use this to convert it to int
                int id = Convert.ToInt32(Request.QueryString["id"]);
                thisClient = new ClientDataConnector().GetClientByFirstVariable(id);

                listJ = new JobDataConnector().GetByClientID(id).ToList<Job>();
            }
            else
            {
                Response.Redirect(Routes.CLIENTS);
            }

            // 
            if (Request.QueryString["message"] == "2")
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
                tbxCompanyName.Text = thisClient.CompanyName;
                tbxPrimaryContactName.Text = thisClient.ContactNamePrimary;
                tbxPrimaryContactDetails.InnerText = thisClient.ContactDetailsPrimary;
                tbxSecondaryContactDetails.InnerText = thisClient.ContactDetailsSecondary;
                tbxSecondaryContactName.Text = thisClient.ContactNameSecondary;
                tbxLocation.Text = thisClient.Location;
            }


        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Routes.CLIENTS);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);
            if (id + "" == ""
                || tbxCompanyName.Text == ""
                || tbxPrimaryContactName.Text == ""
                || tbxPrimaryContactDetails.InnerText == ""
                || tbxLocation.Text == "")
            {
                //Response.Redirect(Utils.GetRedirectUrl(Routes.UPDATE_CLIENT, 6, new { id }), false);
                return;
            }
            try
            {

                Client prepareClient = new Client()
                {
                    ID = id,
                    CompanyName = tbxCompanyName.Text,
                    ContactNamePrimary = tbxPrimaryContactName.Text,
                    ContactDetailsPrimary = tbxPrimaryContactDetails.InnerText,
                    ContactDetailsSecondary = tbxSecondaryContactDetails.InnerText,
                    ContactNameSecondary = tbxSecondaryContactName.Text,
                    SoftDelete = false,
                    Location = tbxLocation.Text,
                };
                var updatedClient = new ClientDataConnector().UpdateClient(prepareClient);

                // Log
                Utils.Log(new CreateLogDTO
                {
                    ActionName = $@"Updates a client (ClientID: {updatedClient.ID})",
                    ActionLocation = Page.AppRelativeVirtualPath,
                    ActionType = LogActionTypes.Update.GetHashCode(),
                    ActionMeta = new LogActionMeta
                    {
                        Input = prepareClient,
                        Result = updatedClient
                    }
                }, Session);

                Response.Redirect(Utils.GetRedirectUrl(Routes.CLIENTS, 2), false);
            }
            catch (Exception)
            {
                Response.Redirect(Utils.GetRedirectUrl(Routes.UPDATE_CLIENT, 99, new { id }), false);
            }
        }
    }
}