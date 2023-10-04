using System;
using System.Collections.Generic;
using System.Linq;
using AvaChemAdminPanelMobiAPI.Common_File;
using Newtonsoft.Json;

namespace AvaChemAdminPanelMobiAPI
{
    public partial class UpdateOT : System.Web.UI.Page
    {
        public const string DASHBOARD = Routes.DASHBOARD;
        public const string OT = Routes.OT;
        public string convertRoles = "";
        public string dataActionTypes = "";

        public OT thisOT;
        public List<User> listDriver = new List<User>();
        public List<UserWithUserOT_DTO> listCrews = new List<UserWithUserOT_DTO>();
        //public List<AssignCrewDTO> listACrews = new List<AssignCrewDTO>();
        public List<Vehicle> listV = new List<Vehicle>();

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
                int id = int.Parse(Request.QueryString["id"]);
                thisOT = new OTDataConnector().GetOTByFirstVariable(id);

                listV = new OTVehicleDataConnector().GetVehiclesByParams(thisOT.ID).ToList<Vehicle>();
                listDriver = new List<User>
                {
                    new UserDataConnector().GetUsersByFirstVariable(thisOT.DriverID)
                };
                listCrews = new UserOTDataConnector().GetUsersByOT_ID(id).ToList<UserWithUserOT_DTO>();
                //foreach (var c in listCrews)
                //{
                //    listACrews.Add(new AssignCrewDTO
                //    {
                //        UserOT_ID = c.UserOT_ID,
                //        ID = c.ID,
                //        IDNumber = c.IDNumber,
                //        Name = c.Name,
                //        Phone = c.Phone,
                //        Email = c.Email,
                //        RoleID = c.RoleID
                //    });
                //}
            }
            else
            {
                Response.Redirect(Routes.OT);
            }

            // upon creating successfully it will display the success msg with the success label
            if (Request.QueryString["message"] == "2")
            {
                successLabel.InnerText = "OT has been updated";
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
                //foreach (var j in new JobDataConnector().GetAll())
                //{
                //    string jKey = JsonConvert.SerializeObject(
                //        new JobLiteDTO
                //        {
                //            ID = j.ID,
                //            Name = j.Name,
                //        }
                //    );
                //    var item = new ListItem($"#{j.JobNumber} {j.Name}", jKey);
                //    if (j.ID == thisOT.JobID) item.Selected = true;
                //    ddlJob.Items.Add(item);
                //}
                //ddlJob.Items.Insert(0, new ListItem("-- No Service Memo / Delivery Order Number --", "0"));

                ddlStatus.DataValueField = "ID";
                ddlStatus.DataTextField = "Name";
                ddlStatus.DataSource = new StatusDataConnector().GetAll();
                ddlStatus.DataBind();
                ddlStatus.SelectedValue = thisOT.StatusID.ToString();

                //tbxJobName.Text = thisOT.JobName;
                dtDStart.Value = thisOT.DriverStartedTime.ToString("hh:mm");
                dtDEnd.Value = thisOT.DriverEndedTime.ToString("hh:mm");
                dtWStart.Value = thisOT.WorkerStartedTime.ToString("hh:mm");
                dtWEnd.Value = thisOT.WorkerEndedTime.ToString("hh:mm");
                tbxJobNumber.Text = thisOT.JobNumber;

                List<KeyValuePair<int, string>> kvpRoles = new List<KeyValuePair<int, string>>();
                foreach (UserRoles ur in (UserRoles[])Enum.GetValues(typeof(UserRoles)))
                {
                    kvpRoles.Add(new KeyValuePair<int, string>(ur.GetHashCode(), ur.ToString()));
                }
                convertRoles = JsonConvert.SerializeObject(kvpRoles);

                dataActionTypes = JsonConvert.SerializeObject(Enum.GetValues(typeof(DataActionTypes)), new Newtonsoft.Json.Converters.StringEnumConverter());

                //if (thisOT.JobID != 0)
                //{
                //    foreach (var worker in new UserJobDataConnector().GetUsersByJobID(thisOT.JobID).ToList<User>())
                //    {
                //        string cKey = JsonConvert.SerializeObject(
                //            new AssignCrewDTO
                //            {
                //                UserOT_ID = 0,
                //                ID = worker.ID,
                //                Name = worker.Name,
                //                IDNumber = worker.IDNumber,
                //                Phone = worker.Phone,
                //                Email = worker.Email,
                //                RoleID = worker.RoleID
                //            }
                //        );
                //        ddlCrew.Items.Add(new ListItem($"{worker.Name} ({worker.IDNumber})", cKey));
                //    }
                //    ddlCrew.Items.Insert(0, new ListItem("-- Select OT Crew --", ""));

                //    listV = new JobVehicleDataConnector().GetVehiclesByParams(thisOT.JobID).ToList<Vehicle>();
                //}

                //CrewValues.Value = JsonConvert.SerializeObject(listACrews);
            }

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Routes.OT);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);
            if (id + "" == ""
                || ddlStatus.SelectedValue == ""
                //|| tbxJobName.Text == ""
                || dtDStart.Value == ""
                || dtDEnd.Value == ""
                || dtWStart.Value == ""
                || dtWEnd.Value == "")
            {
                //Response.Redirect(Utils.GetRedirectUrl(Routes.UPDATE_OT, 6, new { id }), false);
                return;
            }
            try
            {

                OT prepareOT = new OT()
                {
                    ID = id,
                    StatusID = int.Parse(ddlStatus.SelectedValue),
                    DriverStartedTime = Convert.ToDateTime(dtDStart.Value),
                    DriverEndedTime = Convert.ToDateTime(dtDEnd.Value),
                    WorkerStartedTime = Convert.ToDateTime(dtWStart.Value),
                    WorkerEndedTime = Convert.ToDateTime(dtWEnd.Value),
                    //JobID = ddlJob.SelectedValue == "0" ? 0 : JsonConvert.DeserializeObject<JobLiteDTO>(ddlJob.SelectedValue).ID,
                    //JobName = tbxJobName.Text,
                    SoftDelete = false,
                    JobNumber = tbxJobNumber.Text,
                    DriverID = thisOT.DriverID,
                };
                var updatedOT = new OTDataConnector().UpdateOT(prepareOT);

                // Log
                Utils.Log(new CreateLogDTO
                {
                    ActionName = $@"Updates an OT (OT_ID: {updatedOT.ID})",
                    ActionLocation = Page.AppRelativeVirtualPath,
                    ActionType = LogActionTypes.Update.GetHashCode(),
                    ActionMeta = new LogActionMeta
                    {
                        Input = prepareOT,
                        Result = updatedOT
                    }
                }, Session);

                if (updatedOT != null)
                {
                    //var crewValues = JsonConvert.DeserializeObject<List<AssignCrewDTO>>(CrewValues.Value);
                    //foreach (AssignCrewDTO c in crewValues)
                    //{
                    //    if (c.ActionType == DataActionTypes.Create.GetHashCode() && c.UserOT_ID == 0)
                    //    {
                    //        new UserOTDataConnector().CreateUserOT(new UserOT { OT_ID = updatedOT.ID, UserID = c.ID, SoftDelete = false });
                    //    }
                    //    else if (c.ActionType == DataActionTypes.Hide.GetHashCode() && c.UserOT_ID > 0)
                    //    {
                    //        new UserOTDataConnector().UpdateUserOTSoftDelete(c.UserOT_ID);
                    //    }
                    //}

                    //Utils.Log(new CreateLogDTO
                    //{
                    //    ActionName = $@"Assigns users to OT (OT_ID: {updatedOT.ID} - #{updatedOT.JobNumber})",
                    //    ActionLocation = Page.AppRelativeVirtualPath,
                    //    ActionType = LogActionTypes.Update.GetHashCode(),
                    //    ActionMeta = new LogActionMeta
                    //    {
                    //        Input = new { OT_ID = updatedOT.ID, Crews = CrewValues.Value }
                    //    }
                    //}, Session);
                }

                Response.Redirect(Utils.GetRedirectUrl(Routes.OT, 2), false);
            }
            catch (Exception)
            {
                Response.Redirect(Utils.GetRedirectUrl(Routes.UPDATE_OT, 99, new { id }), false);
            }
        }
    }
}