using System;
using System.Linq;
using AvaChemAdminPanelMobiAPI.Common_File;

namespace AvaChemAdminPanelMobiAPI
{
    public partial class UpdateVehicle : System.Web.UI.Page
    {
        public const string DASHBOARD = Routes.DASHBOARD;
        public const string VEHICLES = Routes.VEHICLES;

        public Vehicle thisVehicle;

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
                thisVehicle = new VehicleDataConnector().GetVehicleByFirstVariable(id);
            }
            else
            {
                Response.Redirect(Routes.VEHICLES);
            }

            //
            if (Request.QueryString["message"] == "2")
            {
                successLabel.InnerText = "Vehicle has been updated";
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

            //do not add in Id and softdelete!!
            // for date and int have to convert them to string by adding .ToString();
            //for delete btn part
            if (!IsPostBack)
            {

                tbxNumber.Value = thisVehicle.Number;
                tbxModel.Value = thisVehicle.Model;

            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Routes.VEHICLES);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);
            if (id + "" == ""
                || tbxNumber.Value == "")
            {
                //Response.Redirect(Utils.GetRedirectUrl(Routes.UPDATE_VEHICLE, 6, new { id }), false);
                return;
            }
            try
            {

                Vehicle prepareVehicle = new Vehicle()
                {
                    ID = id,
                    Number = tbxNumber.Value,
                    Model = tbxModel.Value,
                    SoftDelete = false
                };
                var updatedVehicle = new VehicleDataConnector().UpdateVehicle(prepareVehicle);

                // Log
                Utils.Log(new CreateLogDTO
                {
                    ActionName = $@"Updates a vehicle (VehicleID: {prepareVehicle.ID})",
                    ActionLocation = Page.AppRelativeVirtualPath,
                    ActionType = LogActionTypes.Update.GetHashCode(),
                    ActionMeta = new LogActionMeta
                    {
                        Input = prepareVehicle,
                        Result = updatedVehicle
                    }
                }, Session);

                Response.Redirect(Utils.GetRedirectUrl(Routes.VEHICLES, 2), false);
            }
            catch (Exception)
            {
                Response.Redirect(Utils.GetRedirectUrl(Routes.UPDATE_VEHICLE, 99, new { id }), false);
            }
        }
    }
}