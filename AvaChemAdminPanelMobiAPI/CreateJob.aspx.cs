using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using AvaChemAdminPanelMobiAPI.Common_File;
using Newtonsoft.Json;

namespace AvaChemAdminPanelMobiAPI
{
    public partial class CreateJob : System.Web.UI.Page
    {
        public const string DASHBOARD = Routes.DASHBOARD;
        public const string JOBS = Routes.JOBS;
        public string convertRoles = "";
        public string dataActionTypes = "";
        public List<Description> listD = new List<Description>();

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
                successLabel.InnerText = "Job has been created";
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
                listD = new DescriptionDataConnector().GetAll().ToList();
                foreach (var client in new ClientDataConnector().GetAll().ToList<Client>())
                {
                    ddlClient.Items.Add(new ListItem($"{client.CompanyName} - {client.ContactNamePrimary}, {client.Location}", $"{client.ID}|{client.Location}"));
                }
                ddlClient.Items.Insert(0, new ListItem("-- Select Client --", ""));


                List<KeyValuePair<int, string>> kvpRoles = new List<KeyValuePair<int, string>>();
                foreach (UserRoles ur in (UserRoles[])Enum.GetValues(typeof(UserRoles)))
                {
                    kvpRoles.Add(new KeyValuePair<int, string>(ur.GetHashCode(), ur.ToString()));
                }
                convertRoles = JsonConvert.SerializeObject(kvpRoles);

                dataActionTypes = JsonConvert.SerializeObject(Enum.GetValues(typeof(DataActionTypes)), new Newtonsoft.Json.Converters.StringEnumConverter());

                foreach (var user in new UserDataConnector().GetAvailableWorkers().ToList<User>())
                {
                    string wKey = JsonConvert.SerializeObject(
                        new AssignWorkerDTO
                        {
                            UserJobID = 0,
                            ID = user.ID,
                            Name = user.Name,
                            IDNumber = user.IDNumber,
                            Phone = user.Phone,
                            Email = user.Email,
                            RoleID = user.RoleID
                        }
                    );
                    ddlWorker.Items.Add(new ListItem($"{user.Name} ({user.IDNumber})", wKey));
                }
                ddlWorker.Items.Insert(0, new ListItem("-- Select Worker --", ""));

                foreach (var vehicle in new VehicleDataConnector().GetAvailableVehicles().ToList<Vehicle>())
                {
                    string vKey = JsonConvert.SerializeObject(
                        new AssignVehicleDTO
                        {
                            JobVehicleID = 0,
                            ID = vehicle.ID,
                            Number = vehicle.Number,
                            Model = vehicle.Model,
                        }
                    );
                    string vModel = vehicle.Model != "" ? $" ({vehicle.Model})" : "";
                    ddlVehicle.Items.Add(new ListItem($"{vehicle.Number}{vModel}", vKey));
                }
                ddlVehicle.Items.Insert(0, new ListItem("-- Select Vehicle --", ""));

                var emptyList = new List<dynamic>();
                WorkerValues.Value = JsonConvert.SerializeObject(emptyList);
                VehicleValues.Value = JsonConvert.SerializeObject(emptyList);
                JobDescriptionValues.Value = JsonConvert.SerializeObject(emptyList);
                TripValues.Value = JsonConvert.SerializeObject(emptyList);

                //if (Request.QueryString["date"] != null)
                //{
                //    dtWorkingDate.Value = Request.QueryString["date"];
                //}
            }

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Routes.JOBS);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ddlClient.SelectedValue == ""
                || tbxName.Text == ""
                || dtWorkingDate.Value == ""
                //|| dtStart.Value == ""
                //|| dtEnd.Value == ""
                //|| tbxLocation.Text == ""
                )
            {
                //Response.Redirect(Utils.GetRedirectUrl(Routes.CREATE_JOB, 6), false);
                return;
            }
            try
            {
                foreach (string workingDate in Array.ConvertAll(dtWorkingDate.Value.Split(','), str => str.Trim()))
                {
                    DateTime _dtWorkingDate = Convert.ToDateTime(workingDate);
                    InsertJob(_dtWorkingDate);

                    int numMonths = Convert.ToInt32(tbxNumMonthsRepeated.Text.NullIfWhiteSpace());
                    if (numMonths > 0)
                    {
                        for (int i = 0; i < numMonths; i++)
                        {
                            DateTime newDtWorkingDate = _dtWorkingDate.AddMonths(i + 1);
                            InsertJob(newDtWorkingDate);
                        }
                    }
                }

                Response.Redirect(Utils.GetRedirectUrl(Routes.JOBS, 1), false);
            }
            catch (Exception)
            {
                Response.Redirect(Utils.GetRedirectUrl(Routes.CREATE_JOB, 99), false);
            }


        }

        private void InsertJob(DateTime workingDate)
        {
            Job prepareJob = new Job()
            {
                ClientID = Convert.ToInt32(ddlClient.SelectedValue.Split('|').First()),
                //JobNumber = tbxJobNumber.Text,
                InvoiceNo = tbxInvoiceNo.Text,
                Name = tbxName.Text,
                WorkingDate = workingDate,
                //WorkingDate = Convert.ToDateTime(dtWorkingDate.Value),
                Location = tbxLocation.Text,
                AdminRemarks = txtAdminRemarks.InnerText,
                SoftDelete = false,
            };
            var createdJob = new JobDataConnector().CreateJob(prepareJob);

            // Log
            Utils.Log(new CreateLogDTO
            {
                //ActionName = $@"Creates a job (Job: #{createdJob.JobNumber} {createdJob.Name})",
                ActionName = $@"Creates a job (Job: {createdJob.Name})",
                ActionLocation = Page.AppRelativeVirtualPath,
                ActionType = LogActionTypes.Create.GetHashCode(),
                ActionMeta = new LogActionMeta
                {
                    Input = prepareJob,
                    Result = createdJob
                }
            }, Session);

            if (createdJob != null)
            {

                var workerValues = JsonConvert.DeserializeObject<List<AssignWorkerDTO>>(WorkerValues.Value);
                foreach (AssignWorkerDTO w in workerValues)
                {
                    if (w.ActionType == DataActionTypes.Create.GetHashCode() && w.UserJobID == 0)
                    {
                        new UserJobDataConnector().CreateUserJob(new UserJob { JobID = createdJob.ID, UserID = w.ID, SoftDelete = false });


                        // Refresh jobs in mobile app (background task in BE)
                        Task.Run(async () =>
                        {
                            await Utils.PushNotif(w.ID, null, new FCMDataBody
                            {
                                code = FCMDataCodes.UPDATE_JOB,
                                jobId = createdJob.ID
                            });
                        }).ConfigureAwait(false);
                    }
                    // else if (w.ActionType == DataActionTypes.Hide.GetHashCode() && w.UserJobID > 0)
                    // {
                    //     new UserJobDataConnector().UpdateUserJobSoftDelete(w.UserJobID);
                    // }
                }

                if (workerValues.Count > 0)
                {
                    Utils.Log(new CreateLogDTO
                    {
                        //ActionName = $@"Assigns users to job (Job: #{createdJob.JobNumber} {createdJob.Name})",
                        ActionName = $@"Assigns users to job (Job: {createdJob.Name})",
                        ActionLocation = Page.AppRelativeVirtualPath,
                        ActionType = LogActionTypes.Update.GetHashCode(),
                        ActionMeta = new LogActionMeta
                        {
                            Input = new { JobID = createdJob.ID, Workers = WorkerValues.Value }
                        }
                    }, Session);
                }


                var vehicleValues = JsonConvert.DeserializeObject<List<AssignVehicleDTO>>(VehicleValues.Value);
                foreach (AssignVehicleDTO v in vehicleValues)
                {
                    if (v.ActionType == DataActionTypes.Create.GetHashCode() && v.JobVehicleID == 0)
                    {
                        new JobVehicleDataConnector().CreateJobVehicle(new JobVehicle { JobID = createdJob.ID, VehicleID = v.ID, SoftDelete = false });
                    }
                    // else if (v.ActionType == DataActionTypes.Hide.GetHashCode() && v.JobVehicleID > 0)
                    // {
                    //     new JobVehicleDataConnector().UpdateJobVehicleSoftDelete(v.JobVehicleID);
                    // }
                }

                if (vehicleValues.Count > 0)
                {
                    Utils.Log(new CreateLogDTO
                    {
                        //ActionName = $@"Assigns vehicles to job (Job: #{createdJob.JobNumber} {createdJob.Name})",
                        ActionName = $@"Assigns vehicles to job (Job: {createdJob.Name})",
                        ActionLocation = Page.AppRelativeVirtualPath,
                        ActionType = LogActionTypes.Update.GetHashCode(),
                        ActionMeta = new LogActionMeta
                        {
                            Input = new { JobID = createdJob.ID, Vehicles = VehicleValues.Value }
                        }
                    }, Session);
                }

                var jobDescriptionValues = JsonConvert.DeserializeObject<List<AssignDescriptionDTO>>(JobDescriptionValues.Value);
                foreach (AssignDescriptionDTO jd in jobDescriptionValues)
                {
                    if (jd.ActionType == DataActionTypes.Create.GetHashCode() && jd.JobDescriptionID == 0)
                    {
                        new JobDescriptionDataConnector().CreateJobDescription(new JobDescription { JobID = createdJob.ID, DescriptionID = jd.ID, SoftDelete = false });
                    }
                    //else if (jd.ActionType == DataActionTypes.Hide.GetHashCode() && jd.JobDescriptionID > 0)
                    //{
                    //    new JobDescriptionDataConnector().UpdateJobDescriptionSoftDelete(jd.JobDescriptionID);
                    //}
                }

                var tripValues = JsonConvert.DeserializeObject<List<AssignTripDTO>>(TripValues.Value);
                foreach (AssignTripDTO trip in tripValues)
                {
                    if (trip.ActionType == DataActionTypes.Create.GetHashCode() && trip.ID <= 0 && trip.StartTime + "" != "" && trip.EndTime + "" != "")
                    {
                        new TripDataConnector().CreateTrip(new Trip
                        {
                            JobID = createdJob.ID,
                            StartTime = Convert.ToDateTime(trip.StartTime),
                            EndTime = Convert.ToDateTime(trip.EndTime),
                            SoftDelete = false
                        });
                    }
                    else if (trip.ActionType == DataActionTypes.Update.GetHashCode() && trip.ID > 0)
                    {
                        Trip newTrip = new Trip
                        {
                            ID = trip.ID,
                            JobID = createdJob.ID,
                            SoftDelete = false
                        };

                        if (trip.StartTime + "" != "")
                        {
                            newTrip.StartTime = Convert.ToDateTime(trip.StartTime);
                        }
                        if (trip.EndTime + "" != "")
                        {
                            newTrip.EndTime = Convert.ToDateTime(trip.EndTime);
                        }
                        if (trip.WorkerStartedTime + "" != "")
                        {
                            newTrip.WorkerStartedTime = Convert.ToDateTime(trip.WorkerStartedTime);
                        }
                        if (trip.WorkerEndedTime + "" != "")
                        {
                            newTrip.WorkerEndedTime = Convert.ToDateTime(trip.WorkerEndedTime);
                        }
                        if (trip.Remarks + "" != "")
                        {
                            newTrip.Remarks = trip.Remarks;
                        }
                        if (trip.JobNumber + "" != "")
                        {
                            newTrip.JobNumber = trip.JobNumber;
                        }

                        new TripDataConnector().UpdateTrip(newTrip);
                    }
                    else if (trip.ActionType == DataActionTypes.Hide.GetHashCode() && trip.ID > 0)
                    {
                        new TripDataConnector().UpdateTripSoftDelete(trip.ID);
                    }

                    if (trip.DeleteImgs != null && trip.DeleteImgs.Count > 0)
                    {
                        foreach (var imgID in trip.DeleteImgs)
                        {
                            new JobImageDataConnector().UpdateJobImageSoftDelete(imgID);
                        }
                    }
                }

            }
        }
    }
}