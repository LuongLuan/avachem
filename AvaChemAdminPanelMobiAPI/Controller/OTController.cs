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
    public class OTController : ApiController
    {
        [Authorize]
        [HttpGet]
        [ActionName("GetOTs")]
        public IHttpActionResult GetOTs([FromUri] int? page = null, int? per_page = null, int? type = null, int? month = null, int? year = null, int? staff_id = null, string search = "")
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                int? userID = int.Parse(uid);
                User currentUser = new UserDataConnector().GetUsersByFirstVariable((int)userID);
                if (currentUser.RoleID == UserRoles.OTAdmin.GetHashCode()) userID = staff_id;
                List<OT_DTO> OT_DTOs = new UserOTDataConnector().GetOTsByParams(userID, page, per_page, type, month, year, currentUser.RoleID == UserRoles.OTAdmin.GetHashCode() ? search : "").ToList<OT_DTO>();

                response.AvaChemCode = 200;
                response.AvaChemMessage = "OK";
                response.AvaChemReturnObject = OT_DTOs;
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
        [ActionName("GetOTDetails")]
        public IHttpActionResult GetOTDetails([FromUri] int id)
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                User currentUser = new UserDataConnector().GetUsersByFirstVariable(int.Parse(uid));

                OT OT = new OTDataConnector().GetOTByFirstVariable(id);
                var rawOT = JsonConvert.SerializeObject(OT);
                OTDetailsDTO OTDetailsDTO = JsonConvert.DeserializeObject<OTDetailsDTO>(rawOT);
                if (OTDetailsDTO != null)
                {
                    OTDetailsDTO.Status = new StatusDataConnector().GetStatusByFirstVariable(OTDetailsDTO.StatusID);

                    //if (OTDetailsDTO.JobID != 0)
                    //{
                    //    Job job = new JobDataConnector().GetJobByFirstVariable(OTDetailsDTO.JobID);
                    //    OTDetailsDTO.JobWorkingDate = job.WorkingDate;
                    //    //OTDetailsDTO.JobNumber = job.JobNumber;
                    //    OTDetailsDTO.JobNumber = "";
                    //}

                    Job job = new JobDataConnector().GetByJobNumber(OTDetailsDTO.JobNumber);
                    if (job != null) OTDetailsDTO.JobWorkingDate = job.WorkingDate;

                    OTDetailsDTO.Vehicles = new OTVehicleDataConnector().GetVehiclesByParams(OTDetailsDTO.ID).ToList<Vehicle>();

                    User driver = new UserDataConnector().GetUsersByFirstVariable(OTDetailsDTO.DriverID);
                    OTDetailsDTO.Driver = new WorkerDTO
                    {
                        ID = driver.ID,
                        Name = driver.Name,
                        Email = driver.Email,
                        Phone = driver.Phone,
                        RoleID = driver.RoleID,
                        Credits = driver.Credits
                    };

                    var isExist = currentUser.RoleID == UserRoles.OTAdmin.GetHashCode();
                    List<WorkerDTO> crews = new List<WorkerDTO>();
                    foreach (User u in new UserOTDataConnector().GetUsersByOT_ID(OTDetailsDTO.ID).ToList<User>())
                    {
                        crews.Add(new WorkerDTO
                        {
                            ID = u.ID,
                            Name = u.Name,
                            Email = u.Email,
                            Phone = u.Phone,
                            RoleID = u.RoleID,
                            Credits = u.Credits
                        });

                        if (!isExist && u.ID == int.Parse(uid))
                        {
                            isExist = true;
                        }
                    }
                    OTDetailsDTO.Crews = crews;


                    if (isExist == false)
                    {
                        response.AvaChemCode = 400;
                        response.AvaChemMessage = "Error";
                    }
                    else
                    {


                        response.AvaChemCode = 200;
                        response.AvaChemMessage = "OK";
                        response.AvaChemReturnObject = OTDetailsDTO;
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
        [HttpPost]
        [ActionName("ApplyOT")]
        public IHttpActionResult ApplyOT([FromBody] ApplyOT_DTO applyOT_DTO)
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                //JobLiteDTO j = new UserJobDataConnector().GetJobByThisUser(int.Parse(uid), applyOT_DTO.jobId);
                //if (applyOT_DTO.jobId != 0 && j == null)
                //{
                //    response.AvaChemMessage = "Error";
                //    response.AvaChemCode = 400;
                //}
                //else
                //{
                var prepareOT = new OT
                {
                    StatusID = Statuses.Pending.GetHashCode(),
                    WorkerStartedTime = Convert.ToDateTime(applyOT_DTO.workerStartTime),
                    WorkerEndedTime = Convert.ToDateTime(applyOT_DTO.workerEndTime),
                    DriverStartedTime = Convert.ToDateTime(applyOT_DTO.driverStartTime),
                    DriverEndedTime = Convert.ToDateTime(applyOT_DTO.driverEndTime),
                    //JobName = applyOT_DTO.jobName ?? (j != null ? j.Name : ""),
                    //JobID = applyOT_DTO.jobId != 0 ? applyOT_DTO.jobId : 0,
                    UserID = int.Parse(uid),
                    SoftDelete = false,
                    JobNumber = applyOT_DTO.jobNumber,
                    DriverID = applyOT_DTO.driverId,
                };
                var createdOT = new OTDataConnector().CreateOT(prepareOT);

                var prepareUserOT = new UserOT
                {
                    OT_ID = createdOT.ID,
                    UserID = int.Parse(uid),
                    SoftDelete = false,
                };
                new UserOTDataConnector().CreateUserOT(prepareUserOT);

                var prepareOTVehicle = new OTVehicle
                {
                    OT_ID = createdOT.ID,
                    VehicleID = applyOT_DTO.vehicleId,
                    SoftDelete = false,
                };
                new OTVehicleDataConnector().CreateOTVehicle(prepareOTVehicle);

                var rawOT = JsonConvert.SerializeObject(createdOT);
                OT_DTO OT_DTO = JsonConvert.DeserializeObject<OT_DTO>(rawOT);
                OT_DTO.Status = new StatusDataConnector().GetStatusByFirstVariable(OT_DTO.StatusID);

                response.AvaChemCode = 200;
                response.AvaChemMessage = "OK";
                response.AvaChemReturnObject = OT_DTO;

                Task.Run(() =>
                {
                    Utils.SendApplyOTMail(new UserDataConnector().GetUsersByFirstVariable(int.Parse(uid)).Name);
                }).ConfigureAwait(false);
                //}
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
        [ActionName("AddOTCrews")]
        public IHttpActionResult AddOTCrews([FromBody] AddOTCrewsDTO addOTCrewsDTO)
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                if (new UserOTDataConnector().CheckHasThisUser(addOTCrewsDTO.OT_ID, int.Parse(uid)) == false)
                {
                    response.AvaChemMessage = "Error";
                    response.AvaChemCode = 400;
                }
                else
                {
                    foreach (int crewID in addOTCrewsDTO.userIDs)
                    {
                        if (new UserOTDataConnector().CheckHasThisUser(addOTCrewsDTO.OT_ID, crewID) == true) continue;
                        var prepareUserOT = new UserOT
                        {
                            OT_ID = addOTCrewsDTO.OT_ID,
                            UserID = crewID,
                            SoftDelete = false
                        };
                        new UserOTDataConnector().CreateUserOT(prepareUserOT);
                    }

                    response.AvaChemCode = 200;
                    response.AvaChemMessage = "OK";
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
        [HttpPatch]
        [ActionName("ApproveOT")]
        public IHttpActionResult ApproveOT([FromBody] ApproveOT_DTO approveOT_DTO)
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                User currentUser = new UserDataConnector().GetUsersByFirstVariable(int.Parse(uid));
                if (currentUser == null
                    || currentUser.RoleID != UserRoles.OTAdmin.GetHashCode()
                    || new StatusDataConnector().GetStatusByFirstVariable(approveOT_DTO.statusID) == null)
                {
                    response.AvaChemMessage = "Error";
                    response.AvaChemCode = 400;
                }
                else
                {
                    var updatedOT = new OTDataConnector().UpdateOTStatus(approveOT_DTO.OT_ID, approveOT_DTO.statusID);
                    if (updatedOT == null)
                    {
                        response.AvaChemMessage = "Error";
                        response.AvaChemCode = 400;
                        return Ok(response);
                    }

                    var rawOT = JsonConvert.SerializeObject(updatedOT);
                    OTDetailsDTO OTDetailsDTO = JsonConvert.DeserializeObject<OTDetailsDTO>(rawOT);
                    OTDetailsDTO.Status = new StatusDataConnector().GetStatusByFirstVariable(OTDetailsDTO.StatusID);

                    //if (OTDetailsDTO.JobID != 0)
                    //{
                    //    Job job = new JobDataConnector().GetJobByFirstVariable(OTDetailsDTO.JobID);
                    //    OTDetailsDTO.JobWorkingDate = job.WorkingDate;
                    //    //OTDetailsDTO.JobNumber = job.JobNumber;
                    //    OTDetailsDTO.JobNumber = "";
                    //}
                    Job job = new JobDataConnector().GetByJobNumber(OTDetailsDTO.JobNumber);
                    if (job != null) OTDetailsDTO.JobWorkingDate = job.WorkingDate;


                    OTDetailsDTO.Vehicles = new OTVehicleDataConnector().GetVehiclesByParams(OTDetailsDTO.ID).ToList<Vehicle>();

                    User driver = new UserDataConnector().GetUsersByFirstVariable(OTDetailsDTO.DriverID);
                    OTDetailsDTO.Driver = new WorkerDTO
                    {
                        ID = driver.ID,
                        Name = driver.Name,
                        Email = driver.Email,
                        Phone = driver.Phone,
                        RoleID = driver.RoleID,
                        Credits = driver.Credits
                    };

                    List<WorkerDTO> crews = new List<WorkerDTO>();
                    foreach (User u in new UserOTDataConnector().GetUsersByOT_ID(OTDetailsDTO.ID).ToList<User>())
                    {
                        crews.Add(new WorkerDTO
                        {
                            ID = u.ID,
                            Name = u.Name,
                            Email = u.Email,
                            Phone = u.Phone,
                            RoleID = u.RoleID,
                            Credits = u.Credits
                        });
                    }
                    OTDetailsDTO.Crews = crews;


                    response.AvaChemCode = 200;
                    response.AvaChemMessage = "OK";
                    response.AvaChemReturnObject = OTDetailsDTO;
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