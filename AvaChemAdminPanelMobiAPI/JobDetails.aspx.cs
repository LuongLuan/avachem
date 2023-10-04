using System;
using System.Collections.Generic;
using System.Linq;
using AvaChemAdminPanelMobiAPI.Common_File;
using Newtonsoft.Json;

namespace AvaChemAdminPanelMobiAPI
{
    public partial class JobDetails : System.Web.UI.Page
    {
        public const string DASHBOARD = Routes.DASHBOARD;
        public const string JOBS = Routes.JOBS;
        public const string JOB_REPORT = Routes.JOB_REPORT;

        public User userSession;
        public Job thisJob;
        public Client client;
        public List<UserWithUserJobDTO> listWorkers;
        public List<Vehicle> listV;
        //public List<Description> listD = new List<Description>();
        //public List<AssignDescriptionDTO> listJD = new List<AssignDescriptionDTO>();
        public List<TripDetailsDTO> listT = new List<TripDetailsDTO>();
        public bool isCompleted = false;
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

            userSession = Session["admin"] as User;

            // when page load it will hide the succuss and error color label for the message first
            successLabel.Visible = false;
            warningLabel.Visible = false;

            // when update and value is null add if and else statement
            if (Request.QueryString["id"] != null)
            {
                int id = Convert.ToInt32(Request.QueryString["id"]);
                thisJob = new JobDataConnector().GetJobByFirstVariable(id);
                client = new ClientDataConnector().GetClientByFirstVariable(thisJob.ClientID);
                listWorkers = new UserJobDataConnector().GetUsersByJobID(id).ToList<UserWithUserJobDTO>();
                listV = new JobVehicleDataConnector().GetVehiclesByParams(id).ToList<Vehicle>();
                //foreach (var jd in new JobDescriptionDataConnector().GetByJobID(id))
                //{
                //    listJD.Add(new AssignDescriptionDTO
                //    {
                //        ID = jd.DescriptionID,
                //        JobDescriptionID = jd.ID,
                //    });
                //}

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
                }

                isCompleted = new JobDataConnector().CheckIsCompleted(id);
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
                //listD = new DescriptionDataConnector().GetAll().ToList();
                //JobDescriptionValues.Value = JsonConvert.SerializeObject(listJD);
                string description = string.Join("<br />", new JobDescriptionDataConnector().GetByJobID(thisJob.ID).Select(jd => $"- {jd.Content}").ToArray());
                tbxDescription.InnerHtml = description;
                txtAdminRemarks.InnerText = thisJob.AdminRemarks;
                tbxClient.InnerHtml = $"<b>Company:</b> {client.CompanyName}, {client.Location}" +
                                        $"<br /><br /><b>Primary Contact Name:</b> {client.ContactNamePrimary}" +
                                        $"<br /><b>Primary Contact Details:</b> {client.ContactDetailsPrimary}" +
                                        $"<br /><br /><b>Secondary Contact Name:</b> {client.ContactNameSecondary}" +
                                        $"<br /><b>Secondary Contact Details:</b> {client.ContactDetailsSecondary}";
                tbxReportName.ReadOnly = thisJob.ReportName + "" != "";
                tbxReportName.Text = thisJob.ReportName;
                //tbxJobNumber.Text = thisJob.JobNumber;
                tbxInvoiceNo.Text = thisJob.InvoiceNo;
                tbxName.Text = thisJob.Name;
                dtWorkingDate.Value = thisJob.WorkingDate.ToString("yyyy-MM-dd");
                //dtStart.Value = thisJob.StartTime.ToString("HH:mm");
                //dtEnd.Value = thisJob.EndTime.ToString("HH:mm");
                tbxLocation.Text = thisJob.Location;


                //btnGeneratePDF.Attributes.Add("href", $"{Routes.JOB_REPORT}/{thisJob.ReportName}");
                //btnGeneratePDF.Attributes.Add("target", "_blank");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Routes.JOBS);
        }
    }
}