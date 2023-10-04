using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AvaChemAdminPanelMobiAPI.Common_File;

namespace AvaChemAdminPanelMobiAPI.Controller
{
    public class SettingsController : ApiController
    {
        [Authorize]
        [HttpGet]
        [ActionName("GetLeaveReasons")]
        public IHttpActionResult GetLeaveReasons()
        {
            AvaChemResponse response = new AvaChemResponse();
            try
            {
                List<LeaveReason> reasons = new LeaveReasonDataConnector().GetAll().ToList<LeaveReason>();
                response.AvaChemCode = 200;
                response.AvaChemMessage = "OK";
                response.AvaChemReturnObject = reasons;
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