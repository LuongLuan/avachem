using System;
using System.Linq;
using AvaChemAdminPanelMobiAPI.Common_File;


namespace AvaChemAdminPanelMobiAPI
{
    public partial class CreateVehicle : System.Web.UI.Page
    {
        public const string DASHBOARD = Routes.DASHBOARD;
        public const string VEHICLES = Routes.VEHICLES;

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

            //
            if (Request.QueryString["message"] == "1")
            {
                successLabel.InnerText = "Vehicle has been created";
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
            Response.Redirect(Routes.VEHICLES);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (tbxNumber.Value == "")
            {
                //Response.Redirect(Utils.GetRedirectUrl(Routes.CREATE_VEHICLE, 6), false);
                return;
            }
            try
            {

                Vehicle prepareVehicle = new Vehicle()
                {
                    Number = tbxNumber.Value,
                    Model = tbxModel.Value,
                    SoftDelete = false
                };
                var createdVehicle = new VehicleDataConnector().CreateVehicle(prepareVehicle);

                // Log
                Utils.Log(new CreateLogDTO
                {
                    ActionName = $@"Creates a vehicle (Number: {createdVehicle.Number})",
                    ActionLocation = Page.AppRelativeVirtualPath,
                    ActionType = LogActionTypes.Create.GetHashCode(),
                    ActionMeta = new LogActionMeta
                    {
                        Input = prepareVehicle,
                        Result = createdVehicle
                    }
                }, Session);

                Response.Redirect(Utils.GetRedirectUrl(Routes.VEHICLES, 1), false);
            }
            catch (Exception)
            {
                Response.Redirect(Utils.GetRedirectUrl(Routes.CREATE_VEHICLE, 99), false);
            }

        }
    }
}