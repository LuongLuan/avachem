using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using AvaChemAdminPanelMobiAPI.Common_File;

namespace AvaChemAdminPanelMobiAPI.Controller
{
    public class TrainingController : ApiController
    {
        [Authorize]
        [HttpGet]
        [ActionName("GetTrainings")]
        public IHttpActionResult GetTrainings()
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                List<QualificationWithStatus> qualifications = new QualificationDataConnector().GetByUserID(int.Parse(uid));
                response.AvaChemCode = 200;
                response.AvaChemMessage = "OK";
                response.AvaChemReturnObject = qualifications;
            }
            catch (Exception)
            {
                response.AvaChemCode = 400;
                response.AvaChemMessage = "Error";
            }

            return Ok(response);
        }
        [Authorize]
        [HttpDelete]
        [ActionName("DeleteTrainings")]
        public IHttpActionResult DeleteTrainings([FromUri] string ids)
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                List<int> qIDs = ids.Split(',').ToList<string>().Select(int.Parse).ToList<int>();
                foreach (int qID in qIDs)
                {
                    if (new QualificationDataConnector().CheckBelongToThisUser(qID, int.Parse(uid)) == true)
                    {
                        new QualificationDataConnector().UpdateQualificationSoftDelete(qID);
                    }
                }
                List<QualificationWithStatus> qualifications = new QualificationDataConnector().GetByUserID(int.Parse(uid));
                response.AvaChemCode = 200;
                response.AvaChemMessage = "OK";
                response.AvaChemReturnObject = qualifications;
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
        [ActionName("AddTraining")]
        public IHttpActionResult AddTraining([FromBody] EditQualificationDTO editQualificationDTO)
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                new QualificationDataConnector().CreateQualification(new Qualification
                {
                    Name = editQualificationDTO.name,
                    DateObtained = Convert.ToDateTime(editQualificationDTO.dateObtained),
                    ExpiryDate = Convert.ToDateTime(editQualificationDTO.expiryDate),
                    UserID = int.Parse(uid),
                    SoftDelete = false,
                });
                List<QualificationWithStatus> qualifications = new QualificationDataConnector().GetByUserID(int.Parse(uid));
                response.AvaChemCode = 200;
                response.AvaChemMessage = "OK";
                response.AvaChemReturnObject = qualifications;
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
        [ActionName("EditTrainings")]
        public IHttpActionResult EditTrainings([FromBody] List<EditQualificationDTO> editQualificationDTOs)
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                foreach (EditQualificationDTO editQualificationDTO in editQualificationDTOs)
                {
                    if (editQualificationDTO.id + "" != "" && editQualificationDTO.id != 0)
                    {
                        var updateQ = new Qualification
                        {
                            ID = editQualificationDTO.id,
                            Name = editQualificationDTO.name,
                            UserID = int.Parse(uid)
                        };
                        if (editQualificationDTO.dateObtained + "" != "")
                        {
                            updateQ.DateObtained = Convert.ToDateTime(editQualificationDTO.dateObtained);
                        }
                        if (editQualificationDTO.expiryDate + "" != "")
                        {
                            updateQ.ExpiryDate = Convert.ToDateTime(editQualificationDTO.expiryDate);
                        }
                        new QualificationDataConnector().UpdateQualification(updateQ);
                    }
                }
                List<QualificationWithStatus> qualifications = new QualificationDataConnector().GetByUserID(int.Parse(uid));
                response.AvaChemCode = 200;
                response.AvaChemMessage = "OK";
                response.AvaChemReturnObject = qualifications;
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
        [ActionName("CheckExpires")]
        public IHttpActionResult CheckExpires()
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                List<QualificationWithStatus> qualifications = new QualificationDataConnector().GetExpires(int.Parse(uid));

                string message = String.Join(", ", qualifications.Select(q => $"{q.Name} Expiry").ToArray());
                string metaMessage = String.Join("\n", qualifications.Select(q =>
                {
                    string expired = q.DaysLeft <= 0 ? "has expired" : $"will expire in {q.DaysLeft} days";
                    return $"Your {q.Name.ToLower()} {expired}, please renew it.";
                }).ToArray());
                response.AvaChemCode = 200;
                response.AvaChemMessage = "OK";
                response.AvaChemReturnObject = new
                {
                    message = message,
                    cnMessage = "",
                    metaMessage = metaMessage
                };
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