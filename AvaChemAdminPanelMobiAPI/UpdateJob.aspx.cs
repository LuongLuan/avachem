using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using AvaChemAdminPanelMobiAPI.Common_File;
using Newtonsoft.Json;

namespace AvaChemAdminPanelMobiAPI
{
    public partial class UpdateJob : System.Web.UI.Page
    {
        public const string DASHBOARD = Routes.DASHBOARD;
        public const string JOBS = Routes.JOBS;
        public const string UPDATE_TRIP = "ABC";
        public string convertRoles = "";
        public string dataActionTypes = "";

        public Job thisJob;
        public List<Description> listD = new List<Description>();
        public List<AssignDescriptionDTO> listJD = new List<AssignDescriptionDTO>();
        public List<UserWithUserJobDTO> listWorkers;
        public List<AssignWorkerDTO> listAWorkers = new List<AssignWorkerDTO>();
        public List<VehicleWithJobVehicleDTO> listV;
        public List<AssignVehicleDTO> listAVehicles = new List<AssignVehicleDTO>();
        //public List<JobImage> listBeforeImgs = new List<JobImage>();
        //public List<AssignJobImageDTO> listABeforeImages = new List<AssignJobImageDTO>();
        //public List<JobImage> listAfterImgs = new List<JobImage>();
        //public List<AssignJobImageDTO> listAAfterImages = new List<AssignJobImageDTO>();

        public List<TripDetailsDTO> listT = new List<TripDetailsDTO>();
        public List<AssignTripDTO> listAT = new List<AssignTripDTO>();

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
                int id = Convert.ToInt32(Request.QueryString["id"]);
                thisJob = new JobDataConnector().GetJobByFirstVariable(id);
                listWorkers = new UserJobDataConnector().GetUsersByJobID(id).ToList<UserWithUserJobDTO>();
                foreach (var w in listWorkers)
                {
                    listAWorkers.Add(new AssignWorkerDTO
                    {
                        UserJobID = w.UserJobID,
                        ID = w.ID,
                        IDNumber = w.IDNumber,
                        Name = w.Name,
                        Phone = w.Phone,
                        Email = w.Email,
                        RoleID = w.RoleID
                    });
                }

                listV = new JobVehicleDataConnector().GetVehiclesByParams(id).ToList<VehicleWithJobVehicleDTO>();
                foreach (var v in listV)
                {
                    listAVehicles.Add(new AssignVehicleDTO
                    {
                        JobVehicleID = v.JobVehicleID,
                        ID = v.ID,
                        Number = v.Number,
                        Model = v.Model
                    });
                }

                //foreach (var img in new JobImageDataConnector().GetByJobID(id).ToList<JobImage>())
                //{
                //    switch (img.Type)
                //    {
                //        case "before":
                //            listBeforeImgs.Add(img);
                //            //listABeforeImages.Add(new AssignJobImageDTO { ID = img.ID, ImageUrl = img.ImageUrl, ImageName = "" });
                //            break;
                //        case "after":
                //            listAfterImgs.Add(img);
                //            //listAAfterImages.Add(new AssignJobImageDTO { ID = img.ID, ImageUrl = img.ImageUrl, ImageName = "" });
                //            break;
                //        default:
                //            break;
                //    }
                //}

                foreach (var jd in new JobDescriptionDataConnector().GetByJobID(id))
                {
                    listJD.Add(new AssignDescriptionDTO
                    {
                        ID = jd.DescriptionID,
                        JobDescriptionID = jd.ID,
                    });
                }


                int tripIndex = 0;
                foreach (var trip in new TripDataConnector().GetByJobID(id).ToList())
                {
                    tripIndex++;

                    var rawTrip = JsonConvert.SerializeObject(trip);
                    var tripDetailsDTO = JsonConvert.DeserializeObject<TripDetailsDTO>(rawTrip);

                    tripDetailsDTO.Index = tripIndex;

                    tripDetailsDTO.BeforeImages = new List<JobImage>();
                    tripDetailsDTO.AfterImages = new List<JobImage>();
                    foreach (JobImage img in new JobImageDataConnector().GetByTripID(trip.ID).ToList<JobImage>())
                    {
                        switch (img.Type)
                        {
                            case "before":
                                tripDetailsDTO.BeforeImages.Add(img);
                                break;
                            case "after":
                                tripDetailsDTO.AfterImages.Add(img);
                                break;
                            default:
                                break;
                        }
                    }
                    listT.Add(tripDetailsDTO);

                    listAT.Add(new AssignTripDTO
                    {
                        ID = trip.ID,
                        StartTime = trip.StartTime.ToString("HH:mm"),
                        EndTime = trip.EndTime.ToString("HH:mm"),
                        Index = tripIndex
                    });
                }
            }
            else
            {
                Response.Redirect(Routes.JOBS);
            }

            // 
            if (Request.QueryString["message"] == "2")
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
                listD = new DescriptionDataConnector().GetAll().ToList();
                txtAdminRemarks.InnerText = thisJob.AdminRemarks;

                foreach (var client in new ClientDataConnector().GetAll().ToList<Client>())
                {
                    var item = new ListItem($"{client.CompanyName} - {client.ContactNamePrimary}, {client.Location}", $"{client.ID}|{client.Location}");
                    if (client.ID == thisJob.ClientID)
                    {
                        item.Selected = true;
                        tbxLocation.Text = client.Location;

                    }
                    ddlClient.Items.Add(item);
                }
                ddlClient.Items.Insert(0, new ListItem("-- Select Client --", ""));
                //ddlClient.SelectedValue = thisJob.ClientID.ToString();

                //tbxJobNumber.Text = thisJob.JobNumber;
                tbxInvoiceNo.Text = thisJob.InvoiceNo;
                tbxName.Text = thisJob.Name;
                dtWorkingDate.Value = thisJob.WorkingDate.ToString("yyyy-MM-dd");
                //dtStart.Value = thisJob.StartTime.ToString("HH:mm");
                //dtEnd.Value = thisJob.EndTime.ToString("HH:mm");


                List<KeyValuePair<int, string>> kvpRoles = new List<KeyValuePair<int, string>>();
                foreach (UserRoles ur in (UserRoles[])Enum.GetValues(typeof(UserRoles)))
                {
                    kvpRoles.Add(new KeyValuePair<int, string>(ur.GetHashCode(), ur.ToString()));
                }
                convertRoles = JsonConvert.SerializeObject(kvpRoles);

                dataActionTypes = JsonConvert.SerializeObject(Enum.GetValues(typeof(DataActionTypes)), new Newtonsoft.Json.Converters.StringEnumConverter());

                foreach (var user in new UserDataConnector().GetAvailableWorkers(
                    thisJob.ID,
                    new string[] { thisJob.WorkingDate.ToString("yyyy-MM-ddTHH:mm:ss") },
                    "", "").ToList<User>())
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

                foreach (var vehicle in new VehicleDataConnector().GetAvailableVehicles(
                    thisJob.ID,
                    new string[] { thisJob.WorkingDate.ToString("yyyy-MM-ddTHH:mm:ss") },
                    "", "").ToList<Vehicle>())
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


                WorkerValues.Value = JsonConvert.SerializeObject(listAWorkers);
                VehicleValues.Value = JsonConvert.SerializeObject(listAVehicles);
                //BeforeImgValues.Value = JsonConvert.SerializeObject(listABeforeImages);
                //AfterImgValues.Value = JsonConvert.SerializeObject(listAAfterImages);
                JobDescriptionValues.Value = JsonConvert.SerializeObject(listJD);
                TripValues.Value = JsonConvert.SerializeObject(listAT);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Routes.JOBS);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);
            if (id + "" == ""
                || ddlClient.SelectedValue == ""
                || tbxName.Text == ""
                || dtWorkingDate.Value == ""
                //|| dtStart.Value == ""
                //|| dtEnd.Value == ""
                //|| tbxLocation.Text == ""
                )
            {
                //Response.Redirect(Utils.GetRedirectUrl(Routes.UPDATE_JOB, 6, new { id }), false);
                return;
            }
            try
            {

                //string cusSignatureImageUrl = thisJob.CustomerSignatureImage;
                //if (fuSignatureUpload.PostedFile != null && fuSignatureUpload.PostedFile.ContentLength > 0)
                //{
                //    cusSignatureImageUrl = await Utils.UploadFile($"sign{id}", fuSignatureUpload.PostedFile, SupportedFileTypes.Image);
                //}
                Job prepareJob = new Job()
                {
                    ID = id,
                    ClientID = Convert.ToInt32(ddlClient.SelectedValue.Split('|').First()),
                    //JobNumber = tbxJobNumber.Text,
                    InvoiceNo = tbxInvoiceNo.Text,
                    Name = tbxName.Text,
                    WorkingDate = Convert.ToDateTime(dtWorkingDate.Value),
                    Location = tbxLocation.Text,
                    AdminRemarks = txtAdminRemarks.InnerText,
                    SoftDelete = false,
                };
                var updatedJob = new JobDataConnector().UpdateJob(prepareJob);

                // Log
                Utils.Log(new CreateLogDTO
                {
                    ActionName = $@"Updates a job (JobID: {prepareJob.ID})",
                    ActionLocation = Page.AppRelativeVirtualPath,
                    ActionType = LogActionTypes.Update.GetHashCode(),
                    ActionMeta = new LogActionMeta
                    {
                        Input = prepareJob,
                        Result = updatedJob
                    }
                }, Session);

                if (updatedJob != null)
                {

                    var workerValues = JsonConvert.DeserializeObject<List<AssignWorkerDTO>>(WorkerValues.Value);
                    foreach (AssignWorkerDTO w in workerValues)
                    {
                        if (w.ActionType == DataActionTypes.Create.GetHashCode() && w.UserJobID == 0)
                        {
                            new UserJobDataConnector().CreateUserJob(new UserJob { JobID = updatedJob.ID, UserID = w.ID, SoftDelete = false });
                        }
                        else if (w.ActionType == DataActionTypes.Hide.GetHashCode() && w.UserJobID > 0)
                        {
                            new UserJobDataConnector().UpdateUserJobSoftDelete(w.UserJobID);
                        }

                        // Refresh jobs in mobile app (background task in BE)
                        System.Runtime.CompilerServices.ConfiguredTaskAwaitable bgT = Task.Run(async () =>
                        {
                            // bool isCompleted = updatedJob.WorkerStartedTime + "" != "" && updatedJob.WorkerEndedTime + "" != "" && updatedJob.CustomerSignatureImage + "" != "";
                            await Utils.PushNotif(w.ID, null, new FCMDataBody
                            {
                                code = FCMDataCodes.UPDATE_JOB,
                                jobId = updatedJob.ID,
                            });
                        }).ConfigureAwait(false);
                    }

                    Utils.Log(new CreateLogDTO
                    {
                        //ActionName = $@"Assigns users to job (Job: #{updatedJob.JobNumber} {updatedJob.Name})",
                        ActionName = $@"Assigns users to job (Job: {updatedJob.Name})",
                        ActionLocation = Page.AppRelativeVirtualPath,
                        ActionType = LogActionTypes.Update.GetHashCode(),
                        ActionMeta = new LogActionMeta
                        {
                            Input = new { JobID = updatedJob.ID, Workers = WorkerValues.Value }
                        }
                    }, Session);


                    var vehicleValues = JsonConvert.DeserializeObject<List<AssignVehicleDTO>>(VehicleValues.Value);
                    foreach (AssignVehicleDTO v in vehicleValues)
                    {
                        if (v.ActionType == DataActionTypes.Create.GetHashCode() && v.JobVehicleID == 0)
                        {
                            new JobVehicleDataConnector().CreateJobVehicle(new JobVehicle { JobID = updatedJob.ID, VehicleID = v.ID, SoftDelete = false });
                        }
                        else if (v.ActionType == DataActionTypes.Hide.GetHashCode() && v.JobVehicleID > 0)
                        {
                            new JobVehicleDataConnector().UpdateJobVehicleSoftDelete(v.JobVehicleID);
                        }
                    }

                    Utils.Log(new CreateLogDTO
                    {
                        //ActionName = $@"Assigns vehicles to job (Job: #{updatedJob.JobNumber} {updatedJob.Name})",
                        ActionName = $@"Assigns vehicles to job (Job: {updatedJob.Name})",
                        ActionLocation = Page.AppRelativeVirtualPath,
                        ActionType = LogActionTypes.Update.GetHashCode(),
                        ActionMeta = new LogActionMeta
                        {
                            Input = new { JobID = updatedJob.ID, Vehicles = VehicleValues.Value }
                        }
                    }, Session);


                    var jobDescriptionValues = JsonConvert.DeserializeObject<List<AssignDescriptionDTO>>(JobDescriptionValues.Value);
                    foreach (AssignDescriptionDTO jd in jobDescriptionValues)
                    {
                        if (jd.ActionType == DataActionTypes.Create.GetHashCode() && jd.JobDescriptionID == 0)
                        {
                            new JobDescriptionDataConnector().CreateJobDescription(new JobDescription { JobID = updatedJob.ID, DescriptionID = jd.ID, SoftDelete = false });
                        }
                        else if (jd.ActionType == DataActionTypes.Hide.GetHashCode() && jd.JobDescriptionID > 0)
                        {
                            new JobDescriptionDataConnector().UpdateJobDescriptionSoftDelete(jd.JobDescriptionID);
                        }
                    }

                    var tripValues = JsonConvert.DeserializeObject<List<AssignTripDTO>>(TripValues.Value);
                    foreach (AssignTripDTO trip in tripValues)
                    {
                        if (trip.ActionType == DataActionTypes.Create.GetHashCode() && trip.ID <= 0 && trip.StartTime + "" != "" && trip.EndTime + "" != "")
                        {
                            new TripDataConnector().CreateTrip(new Trip
                            {
                                JobID = updatedJob.ID,
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
                                JobID = updatedJob.ID,
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


                    //var beforeImgValues = JsonConvert.DeserializeObject<List<AssignJobImageDTO>>(BeforeImgValues.Value);
                    //var beforeImgsNeedUpdate = new List<string>();
                    //foreach (AssignJobImageDTO jImg in beforeImgValues)
                    //{
                    //    if (jImg.ActionType == DataActionTypes.Hide.GetHashCode() && jImg.ID > 0)
                    //    {
                    //        new JobImageDataConnector().UpdateJobImageSoftDelete(jImg.ID);
                    //    }
                    //    else if (jImg.ActionType == DataActionTypes.Create.GetHashCode() && jImg.ID <= 0)
                    //    {
                    //        beforeImgsNeedUpdate.Add(jImg.ImageName);
                    //    }
                    //}
                    //if (fuBeforeImage.HasFiles)
                    //{
                    //    foreach (HttpPostedFile uploadedBeforeImage in fuBeforeImage.PostedFiles)
                    //    {
                    //        if (beforeImgsNeedUpdate.Contains(uploadedBeforeImage.FileName) && uploadedBeforeImage.ContentLength > 0)
                    //        {
                    //            string uploadedBeforeImageUrl = await Utils.UploadFile($"before{updatedJob.ID}", uploadedBeforeImage, SupportedFileTypes.Image);
                    //            new JobImageDataConnector().CreateJobImage(new JobImage
                    //            {
                    //                ImageUrl = uploadedBeforeImageUrl,
                    //                Type = "before",
                    //                JobID = updatedJob.ID,
                    //                SoftDelete = false,
                    //            });
                    //        }
                    //    }
                    //}

                    //var afterImgValues = JsonConvert.DeserializeObject<List<AssignJobImageDTO>>(AfterImgValues.Value);
                    //var afterImgsNeedUpdate = new List<string>();
                    //foreach (AssignJobImageDTO jImg in afterImgValues)
                    //{
                    //    if (jImg.ActionType == DataActionTypes.Hide.GetHashCode() && jImg.ID > 0)
                    //    {
                    //        new JobImageDataConnector().UpdateJobImageSoftDelete(jImg.ID);
                    //    }
                    //    else if (jImg.ActionType == DataActionTypes.Create.GetHashCode() && jImg.ID <= 0)
                    //    {
                    //        afterImgsNeedUpdate.Add(jImg.ImageName);
                    //    }
                    //}
                    //if (fuAfterImage.HasFiles)
                    //{
                    //    foreach (HttpPostedFile uploadedAfterImage in fuAfterImage.PostedFiles)
                    //    {
                    //        if (afterImgsNeedUpdate.Contains(uploadedAfterImage.FileName) && uploadedAfterImage.ContentLength > 0)
                    //        {
                    //            string uploadedAfterImageUrl = await Utils.UploadFile($"after{updatedJob.ID}", uploadedAfterImage, SupportedFileTypes.Image);
                    //            new JobImageDataConnector().CreateJobImage(new JobImage
                    //            {
                    //                ImageUrl = uploadedAfterImageUrl,
                    //                Type = "after",
                    //                JobID = updatedJob.ID,
                    //                SoftDelete = false,
                    //            });
                    //        }
                    //    }
                    //}
                }

                Response.Redirect(Utils.GetRedirectUrl(Routes.JOBS, 2), false);
            }
            catch (Exception)
            {
                Response.Redirect(Utils.GetRedirectUrl(Routes.UPDATE_JOB, 99, new { id }), false);
            }
        }
    }
}