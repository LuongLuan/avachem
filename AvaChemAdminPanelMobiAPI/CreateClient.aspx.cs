using System;
using System.Linq;
using AvaChemAdminPanelMobiAPI.Common_File;

namespace AvaChemAdminPanelMobiAPI
{
    public partial class CreateClient : System.Web.UI.Page
    {
        public const string DASHBOARD = Routes.DASHBOARD;
        public const string CLIENTS = Routes.CLIENTS;

        public Client thisClient;

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

            // upon creating successfully it will display the success msg with the success label
            if (Request.QueryString["message"] == "1")
            {
                successLabel.InnerText = "Client has been created";
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

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Routes.CLIENTS);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (tbxCompanyName.Text == ""
                || tbxPrimaryContactName.Text == ""
                || tbxPrimaryContactDetails.InnerText == ""
                || tbxLocation.Text == "")
            {
                //Response.Redirect(Utils.GetRedirectUrl(Routes.CREATE_CLIENT, 6), false);
                return;
            }
            try
            {

                Client prepareClient = new Client()
                {
                    CompanyName = tbxCompanyName.Text,
                    ContactNamePrimary = tbxPrimaryContactName.Text,
                    ContactDetailsPrimary = tbxPrimaryContactDetails.InnerText,
                    ContactDetailsSecondary = tbxSecondaryContactDetails.InnerText,
                    ContactNameSecondary = tbxSecondaryContactName.Text,
                    SoftDelete = false,
                    Location = tbxLocation.Text,
                };
                var createdClient = new ClientDataConnector().CreateClient(prepareClient);

                // Log
                Utils.Log(new CreateLogDTO
                {
                    ActionName = $@"Creates a client (Client: {createdClient.ContactNamePrimary})",
                    ActionLocation = Page.AppRelativeVirtualPath,
                    ActionType = LogActionTypes.Create.GetHashCode(),
                    ActionMeta = new LogActionMeta
                    {
                        Input = prepareClient,
                        Result = createdClient
                    }
                }, Session);

                Response.Redirect(Utils.GetRedirectUrl(Routes.CLIENTS, 1), false);
            }
            catch (Exception)
            {
                Response.Redirect(Utils.GetRedirectUrl(Routes.CREATE_CLIENT, 99), false);
            }
        }
    }
}