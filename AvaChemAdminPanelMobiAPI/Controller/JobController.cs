using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AvaChemAdminPanelMobiAPI.Common_File;
using Newtonsoft.Json;

namespace AvaChemAdminPanelMobiAPI.Controller
{
    public class JobController : ApiController
    {
        [Authorize]
        [HttpGet]
        [ActionName("GetJobNumbers")]
        public IHttpActionResult GetJobNumbers()
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                List<JobLiteDTO> jobNumbers = new List<JobLiteDTO>();
                //List<Job> jobs = new UserJobDataConnector().GetJobsByUserID(int.Parse(uid)).ToList<Job>();
                //foreach (Job j in jobs)
                //{
                //    jobNumbers.Add(new JobLiteDTO { ID = j.ID, JobNumber = j.JobNumber, Name = j.Name });
                //}
                response.AvaChemCode = 200;
                response.AvaChemMessage = "OK";
                response.AvaChemReturnObject = jobNumbers;
            }
            catch (Exception)
            {
                response.AvaChemCode = 400;
                response.AvaChemMessage = "Error";
            }

            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        [ActionName("GetJobs")]
        public IHttpActionResult GetJobs([FromUri] int? page = null, int? per_page = null, int? type = null, string from = "")
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                List<Job> jobs = new UserJobDataConnector().GetJobsByParams(int.Parse(uid), page, per_page, null, type, from).ToList<Job>();
                var rawJobs = JsonConvert.SerializeObject(jobs);
                List<JobDTO> jobDTOs = JsonConvert.DeserializeObject<List<JobDTO>>(rawJobs).Select(j =>
                {
                    j.Description = ""; // string.Join(", ", new JobDescriptionDataConnector().GetByJobID(j.ID).Select(jd => jd.Content).ToArray())
                    j.NumTrips = new TripDataConnector().CountByJobID(j.ID);
                    return j;
                }).ToList();
                //foreach (JobDTO j in jobDTOs)
                //{
                //}

                response.AvaChemCode = 200;
                response.AvaChemMessage = "OK";
                response.AvaChemReturnObject = jobDTOs;
            }
            catch (Exception)
            {
                response.AvaChemCode = 400;
                response.AvaChemMessage = "Error";
            }

            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        [ActionName("GetJobDetails")]
        public IHttpActionResult GetJobDetails([FromUri] int id)
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                Job job = new JobDataConnector().GetJobByFirstVariable(id);
                var rawJob = JsonConvert.SerializeObject(job);
                JobDetailsDTO jobDetailsDTO = JsonConvert.DeserializeObject<JobDetailsDTO>(rawJob);
                if (jobDetailsDTO != null)
                {
                    var isExist = false;
                    List<WorkerDTO> workers = new List<WorkerDTO>();
                    foreach (User u in new UserJobDataConnector().GetUsersByJobID(jobDetailsDTO.ID).ToList<User>())
                    {
                        workers.Add(new WorkerDTO
                        {
                            ID = u.ID,
                            Name = u.Name,
                            Email = u.Email,
                            Phone = u.Phone,
                            RoleID = u.RoleID,
                            Credits = u.Credits
                        });

                        if (u.ID == int.Parse(uid))
                        {
                            isExist = true;
                        }
                    }

                    if (isExist == false)
                    {
                        response.AvaChemCode = 400;
                        response.AvaChemMessage = "Error";
                    }
                    else
                    {
                        jobDetailsDTO.Description = string.Join(", ", new JobDescriptionDataConnector().GetByJobID(jobDetailsDTO.ID).Select(jd => jd.Content).ToArray());

                        jobDetailsDTO.Workers = workers;
                        jobDetailsDTO.Vehicles = new JobVehicleDataConnector().GetVehiclesByParams(jobDetailsDTO.ID).ToList<Vehicle>();

                        jobDetailsDTO.Client = new ClientDataConnector().GetClientByFirstVariable(jobDetailsDTO.ClientID);

                        jobDetailsDTO.Trips = new List<TripDetailsDTO>();
                        int tripIndex = 0;
                        foreach (Trip trip in new TripDataConnector().GetByJobID(jobDetailsDTO.ID).ToList())
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
                            jobDetailsDTO.Trips.Add(tripDetailsDTO);
                        }

                        jobDetailsDTO.NumTrips = jobDetailsDTO.Trips.Count;

                        jobDetailsDTO.IsCompleted = new JobDataConnector().CheckIsCompleted(jobDetailsDTO.ID);

                        response.AvaChemCode = 200;
                        response.AvaChemMessage = "OK";
                        response.AvaChemReturnObject = jobDetailsDTO;
                    }
                }
                else
                {
                    response.AvaChemCode = 400;
                    response.AvaChemMessage = "Error";
                }
            }
            catch (Exception)
            {
                response.AvaChemCode = 400;
                response.AvaChemMessage = "Error";
            }

            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        [ActionName("GetTripDetails")]
        public IHttpActionResult GetTripDetails([FromUri] int id)
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                Trip trip = new TripDataConnector().GetTripByFirstVariable(id);
                var rawTrip = JsonConvert.SerializeObject(trip);
                TripDetailsDTO tripDetailsDTO = JsonConvert.DeserializeObject<TripDetailsDTO>(rawTrip);
                if (tripDetailsDTO != null)
                {
                    int tripIndex = 0;
                    foreach (Trip _trip in new TripDataConnector().GetByJobID(tripDetailsDTO.JobID))
                    {
                        tripIndex++;
                        if (_trip.ID == tripDetailsDTO.ID) { break; }
                    }

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

                    //Job job = new JobDataConnector().GetJobByFirstVariable(tripDetailsDTO.JobID);
                    //if (job != null)
                    //{
                    //    tripDetailsDTO.JobName = job.Name;
                    //}

                    response.AvaChemCode = 200;
                    response.AvaChemMessage = "OK";
                    response.AvaChemReturnObject = tripDetailsDTO;
                }
                else
                {
                    response.AvaChemCode = 400;
                    response.AvaChemMessage = "Error";
                }
            }
            catch (Exception)
            {
                response.AvaChemCode = 400;
                response.AvaChemMessage = "Error";
            }

            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        [ActionName("UploadJobImage")]
        public async Task<IHttpActionResult> UploadJobImage([FromUri] int id)
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                if (new UserDataConnector().IsDriver(int.Parse(uid)) == false)
                {
                    response.AvaChemMessage = "Error";
                    response.AvaChemCode = 400;
                    response.AvaChemReturnObject = new
                    {
                        message = "Only driver has the permission to submit the photos",
                        cnMessage = "",
                        metaMessage = ""
                    };
                }
                //else if (new UserJobDataConnector().CheckHasThisUser(id, int.Parse(uid)) == false)
                //{
                //    response.AvaChemMessage = "Error";
                //    response.AvaChemCode = 400;
                //}
                else if (HttpContext.Current.Request.Files.AllKeys.Length == 0)
                {
                    response.AvaChemMessage = "Error";
                    response.AvaChemCode = 400;
                }
                else
                {
                    var httpRequest = HttpContext.Current.Request;
                    List<ImageUploadDTO> imageUploadDTOs = new List<ImageUploadDTO>();

                    foreach (string fKey in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[fKey];
                        var imgType = Regex.Replace(fKey, @"[\d-]", string.Empty);
                        if (postedFile != null && postedFile.ContentLength > 0)
                        {
                            string uploadedImageURL = await Utils.UploadFile($"{imgType}{id}", postedFile, SupportedFileTypes.Image);
                            imageUploadDTOs.Add(new ImageUploadDTO
                            {
                                FileKey = fKey,
                                FileName = postedFile.FileName,
                                Url = uploadedImageURL
                            });
                            new JobImageDataConnector().CreateJobImage(new JobImage
                            {
                                ImageUrl = uploadedImageURL,
                                Type = imgType,
                                TripID = id,
                                SoftDelete = false,
                            });

                        }
                    }

                    response.AvaChemMessage = "OK";
                    response.AvaChemCode = 200;
                    response.AvaChemReturnObject = imageUploadDTOs;
                }
            }
            catch (Exception)
            {
                response.AvaChemMessage = "Error";
                response.AvaChemCode = 400;
            }

            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        [ActionName("UploadCustomerSignature")]
        public async Task<IHttpActionResult> UploadCustomerSignature([FromUri] int id)
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                if (new UserDataConnector().IsDriver(int.Parse(uid)) == false)
                {
                    response.AvaChemMessage = "Error";
                    response.AvaChemCode = 400;
                    response.AvaChemReturnObject = new
                    {
                        message = "Only driver has the permission to submit the photos",
                        cnMessage = "",
                        metaMessage = ""
                    };
                }
                //else if (new UserJobDataConnector().CheckHasThisUser(id, int.Parse(uid)) == false)
                //{
                //    response.AvaChemMessage = "Error";
                //    response.AvaChemCode = 400;
                //}
                else if (HttpContext.Current.Request.Files.AllKeys.Length == 0)
                {
                    response.AvaChemMessage = "Error";
                    response.AvaChemCode = 400;
                }
                else
                {
                    var httpRequest = HttpContext.Current.Request;
                    List<ImageUploadDTO> imageUploadDTOs = new List<ImageUploadDTO>();
                    string uploadedImageURL = "";
                    foreach (string fKey in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[fKey];
                        if (postedFile != null && postedFile.ContentLength > 0)
                        {
                            uploadedImageURL = await Utils.UploadFile($"sign{id}", postedFile, SupportedFileTypes.Image);
                            imageUploadDTOs.Add(new ImageUploadDTO
                            {
                                FileKey = fKey,
                                FileName = postedFile.FileName,
                                Url = uploadedImageURL
                            });

                        }
                    }

                    new TripDataConnector().UpdateSignatureByID(id, uploadedImageURL);

                    response.AvaChemMessage = "OK";
                    response.AvaChemCode = 200;
                    response.AvaChemReturnObject = imageUploadDTOs;
                }
            }
            catch (Exception)
            {
                response.AvaChemMessage = "Error";
                response.AvaChemCode = 400;
            }

            return Ok(response);
        }

        [Authorize]
        [HttpPatch]
        [ActionName("SubmitJob")]
        public IHttpActionResult SubmitJob([FromBody] SubmitTripDTO submitTripDTO)
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                if (new UserDataConnector().IsDriver(int.Parse(uid)) == false)
                {
                    response.AvaChemMessage = "Error";
                    response.AvaChemCode = 400;
                    response.AvaChemReturnObject = new
                    {
                        message = "Only driver has the permission to submit job",
                        cnMessage = "",
                        metaMessage = ""
                    };
                }
                else
                {
                    Trip prepareTrip = new TripDataConnector().GetTripByFirstVariable(submitTripDTO.id);
                    if (new UserJobDataConnector().CheckHasThisUser(prepareTrip.JobID, int.Parse(uid)) == false)
                    {
                        response.AvaChemMessage = "Error";
                        response.AvaChemCode = 400;
                    }
                    if (prepareTrip != null)
                    {
                        prepareTrip.WorkerStartedTime = submitTripDTO.startTime + "" != "" ? Convert.ToDateTime(submitTripDTO.startTime) : prepareTrip.WorkerStartedTime;
                        prepareTrip.WorkerEndedTime = submitTripDTO.endTime + "" != "" ? Convert.ToDateTime(submitTripDTO.endTime) : prepareTrip.WorkerEndedTime;
                        prepareTrip.Remarks = submitTripDTO.remarks ?? prepareTrip.Remarks;
                        prepareTrip.JobNumber = submitTripDTO.jobNumber ?? prepareTrip.JobNumber;
                    }
                    new TripDataConnector().UpdateTripPartial(prepareTrip);
                    var updatedJob = new JobDataConnector().GetJobByFirstVariable(prepareTrip.JobID);

                    if (submitTripDTO.deleteImageIds != null)
                    {
                        foreach (int imgId in submitTripDTO.deleteImageIds)
                        {
                            new JobImageDataConnector().UpdateJobImageSoftDelete(imgId);
                        }
                    }

                    var rawJob = JsonConvert.SerializeObject(updatedJob);
                    JobDetailsDTO jobDetailsDTO = JsonConvert.DeserializeObject<JobDetailsDTO>(rawJob);

                    List<WorkerDTO> workers = new List<WorkerDTO>();
                    foreach (User u in new UserJobDataConnector().GetUsersByJobID(jobDetailsDTO.ID).ToList<User>())
                    {
                        workers.Add(new WorkerDTO
                        {
                            ID = u.ID,
                            Name = u.Name,
                            Email = u.Email,
                            Phone = u.Phone,
                            Credits = u.Credits
                        });

                        if (u.ID == int.Parse(uid)) continue;
                        // Refresh jobs in mobile app (background task in BE)
                        Task.Run(async () =>
                        {
                            // bool isCompleted = updatedTrip.WorkerStartedTime + "" != "" && updatedTrip.WorkerEndedTime + "" != "" && updatedTrip.CustomerSignatureImage + "" != "";
                            await Utils.PushNotif(u.ID, null, new FCMDataBody
                            {
                                code = FCMDataCodes.UPDATE_JOB,
                                jobId = updatedJob.ID,
                            });
                        }).ConfigureAwait(false);
                    }

                    jobDetailsDTO.Description = string.Join(", ", new JobDescriptionDataConnector().GetByJobID(jobDetailsDTO.ID).Select(jd => jd.Content).ToArray());

                    jobDetailsDTO.Workers = workers;
                    jobDetailsDTO.Vehicles = new JobVehicleDataConnector().GetVehiclesByParams(jobDetailsDTO.ID).ToList<Vehicle>();

                    jobDetailsDTO.Client = new ClientDataConnector().GetClientByFirstVariable(jobDetailsDTO.ClientID);

                    jobDetailsDTO.Trips = new List<TripDetailsDTO>();
                    int tripIndex = 0;
                    foreach (Trip trip in new TripDataConnector().GetByJobID(jobDetailsDTO.ID).ToList())
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
                        jobDetailsDTO.Trips.Add(tripDetailsDTO);
                    }

                    jobDetailsDTO.IsCompleted = new JobDataConnector().CheckIsCompleted(jobDetailsDTO.ID);

                    response.AvaChemCode = 200;
                    response.AvaChemMessage = "OK";
                    response.AvaChemReturnObject = jobDetailsDTO;
                }
            }
            catch (Exception)
            {
                response.AvaChemCode = 400;
                response.AvaChemMessage = "Error";
            }

            return Ok(response);
        }
    }
}