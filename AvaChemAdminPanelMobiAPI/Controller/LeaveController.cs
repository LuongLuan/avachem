using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AvaChemAdminPanelMobiAPI.Common_File;
using Newtonsoft.Json;

namespace AvaChemAdminPanelMobiAPI.Controller
{
    public class LeaveController : ApiController
    {
        [Authorize]
        [HttpGet]
        [ActionName("GetLeaves")]
        public IHttpActionResult GetLeaves([FromUri] int? page = null, int? per_page = null, int? type = null)
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                List<Leave> leaves = new LeaveDataConnector().GetAll<Leave>(page, per_page, null, type, null, null, null, int.Parse(uid));
                var rawLeaves = JsonConvert.SerializeObject(leaves);
                List<LeaveDTO> leaveDTOs = JsonConvert.DeserializeObject<List<LeaveDTO>>(rawLeaves);
                foreach (LeaveDTO l in leaveDTOs)
                {
                    l.Reason = new LeaveReasonDataConnector().GetLeaveReasonByFirstVariable(l.ReasonID);
                    l.Status = new StatusDataConnector().GetStatusByFirstVariable(l.StatusID);
                }

                response.AvaChemCode = 200;
                response.AvaChemMessage = "OK";
                response.AvaChemReturnObject = leaveDTOs;
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
        [ActionName("GetLeaveDetails")]
        public IHttpActionResult GetLeaveDetails([FromUri] int id)
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                Leave leave = new LeaveDataConnector().GetLeaveByFirstVariable(id);
                if (leave != null && leave.UserID == int.Parse(uid))
                {
                    var rawLeave = JsonConvert.SerializeObject(leave);
                    LeaveDTO leaveDTO = JsonConvert.DeserializeObject<LeaveDTO>(rawLeave);
                    leaveDTO.Reason = new LeaveReasonDataConnector().GetLeaveReasonByFirstVariable(leaveDTO.ReasonID);
                    leaveDTO.Status = new StatusDataConnector().GetStatusByFirstVariable(leaveDTO.StatusID);

                    response.AvaChemCode = 200;
                    response.AvaChemMessage = "OK";
                    response.AvaChemReturnObject = leaveDTO;
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
        [ActionName("UploadLeaveImage")]
        public async Task<IHttpActionResult> UploadLeaveImage([FromUri] int id)
        {
            var identity = System.Web.HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                List<ImageUploadDTO> imageUploadDTOs = new List<ImageUploadDTO>();
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.AllKeys.Length != 0
                    && new LeaveDataConnector().CheckUserValid(id, int.Parse(uid)) == true)
                {
                    string uploadedImageURL = "";
                    foreach (string fKey in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[fKey];
                        if (postedFile != null && postedFile.ContentLength > 0)
                        {
                            uploadedImageURL = await Utils.UploadFile($"leave{id}", postedFile, SupportedFileTypes.Image);
                            imageUploadDTOs.Add(new ImageUploadDTO
                            {
                                FileKey = fKey,
                                FileName = postedFile.FileName,
                                Url = uploadedImageURL
                            });
                        }
                    }

                    new LeaveDataConnector().UpdateLeaveImageByID(id, uploadedImageURL);

                    response.AvaChemMessage = "OK";
                    response.AvaChemCode = 200;
                    response.AvaChemReturnObject = imageUploadDTOs;
                }
                else
                {
                    response.AvaChemMessage = "Error";
                    response.AvaChemCode = 400;
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
        [ActionName("AddLeave")]
        public IHttpActionResult AddLeave([FromBody] AddLeaveDTO addLeaveDTO)
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                List<JobLiteDTO> jobs = new UserJobDataConnector().GetJobsByDate(int.Parse(uid), addLeaveDTO.startDate, addLeaveDTO.endDate);
                if (jobs.Count > 0)
                {
                    //string jobNumbers = String.Join(", ", jobs.Select(j => $"#{j.JobNumber}").ToArray());
                    string jobNames = String.Join(", ", jobs.Select(j => $"{j.Name}").ToArray());
                    response.AvaChemCode = 400;
                    response.AvaChemMessage = "Error";
                    response.AvaChemReturnObject = new
                    {
                        message = "You have a job assigned during the selected period",
                        cnMessage = "您在选定的时期内分配了工作",
                        //metaMessage = $"(Service Memo / Delivery Order Number {jobNumbers})"
                        metaMessage = $"(Job: {jobNames})"
                    };
                }
                else
                {
                    var prepareLeave = new Leave
                    {
                        StatusID = Statuses.Pending.GetHashCode(),
                        StartedDate = Convert.ToDateTime(addLeaveDTO.startDate),
                        EndedDate = Convert.ToDateTime(addLeaveDTO.endDate),
                        NumDays = addLeaveDTO.numDays,
                        ReasonID = addLeaveDTO.reasonId,
                        Remarks = addLeaveDTO.remarks ?? "",
                        UserID = int.Parse(uid),
                        SoftDelete = false
                    };
                    var createdLeave = new LeaveDataConnector().CreateLeave(prepareLeave);

                    var rawLeave = JsonConvert.SerializeObject(createdLeave);
                    LeaveDTO leaveDTO = JsonConvert.DeserializeObject<LeaveDTO>(rawLeave);
                    leaveDTO.Reason = new LeaveReasonDataConnector().GetLeaveReasonByFirstVariable(leaveDTO.ReasonID);
                    leaveDTO.Status = new StatusDataConnector().GetStatusByFirstVariable(leaveDTO.StatusID);

                    response.AvaChemCode = 200;
                    response.AvaChemMessage = "OK";
                    response.AvaChemReturnObject = leaveDTO;
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