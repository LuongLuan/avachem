using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using AvaChemAdminPanelMobiAPI.Common_File;

namespace AvaChemAdminPanelMobiAPI.Controller
{
    public class DeviceController : ApiController
    {
        [Authorize]
        [HttpGet]
        [ActionName("GetDevices")]
        public IHttpActionResult GetDevices()
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                List<Device> devices = new DeviceDataConnector().GetAll(int.Parse(uid));
                response.AvaChemCode = 200;
                response.AvaChemMessage = "OK";
                response.AvaChemReturnObject = devices;
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
        [ActionName("AddDevice")]
        public IHttpActionResult AddDevice([FromBody] AddDeviceDTO addDeviceDTO)
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                if (new DeviceDataConnector().CheckFCMExists(int.Parse(uid), addDeviceDTO.fcmToken) == false)
                {
                    var prepareDevice = new Device
                    {
                        FCMToken = addDeviceDTO.fcmToken,
                        DevID = addDeviceDTO.devID,
                        Model = addDeviceDTO.model,
                        NumFailedNotif = 0,
                        UserID = int.Parse(uid),
                        SoftDelete = false
                    };
                    new DeviceDataConnector().CreateDevice(prepareDevice);
                }

                List<Device> devices = new DeviceDataConnector().GetAll(int.Parse(uid));
                response.AvaChemCode = 200;
                response.AvaChemMessage = "OK";
                response.AvaChemReturnObject = devices;
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