using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using AvaChemAdminPanelMobiAPI.Common_File;
using Newtonsoft.Json;

namespace AvaChemAdminPanelMobiAPI.Controller
{
    public class UserController : ApiController
    {
        [HttpPost]
        [ActionName("ForgetPassword")]
        public IHttpActionResult ForgetPassword([FromBody] ForgetPasswordDTO forgetPasswordDTO)
        {
            AvaChemResponse response = new AvaChemResponse();
            try
            {
                User user = new UserDataConnector().GetByEmail(forgetPasswordDTO.email);
                if (user != null)
                {
                    //string newPassword = Utils.SendResetPasswordMail(forgetPasswordDTO.email);
                    //new UserDataConnector().UpdatePasswored(user.ID, Utils.Hash(newPassword));

                    response.AvaChemCode = 200;
                    response.AvaChemMessage = "OK";
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
        [ActionName("GetCurrentUser")]
        public IHttpActionResult GetCurrentUser()
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                User currentUser = new UserDataConnector().GetUsersByFirstVariable(int.Parse(uid));

                var rawUser = JsonConvert.SerializeObject(currentUser);
                UserDetailsDTO userDetailsDTO = JsonConvert.DeserializeObject<UserDetailsDTO>(rawUser);
                UserRole role = new UserRoleDataConnector().GetUserRoleByFirstVariable(currentUser.RoleID);
                userDetailsDTO.Role = role;

                response.AvaChemCode = 200;
                response.AvaChemMessage = "OK";
                response.AvaChemReturnObject = userDetailsDTO;
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
        [ActionName("UpdateProfile")]
        public IHttpActionResult UpdateProfile([FromBody] UpdateProfileDTO updateProfileDTO)
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                UserDataConnector userConnector = new UserDataConnector();
                User currentUser = userConnector.GetUsersByFirstVariable(int.Parse(uid));

                //if (currentUser.Email != updateProfileDTO.email)
                //{
                //    if (userConnector.CheckEmailExists(updateProfileDTO.email) == true)
                //    {
                //        response.AvaChemCode = 400;
                //        response.AvaChemMessage = "Email already exists";
                //        return Ok(response);
                //    }
                //}
                if (currentUser.Username != updateProfileDTO.username)
                {
                    if (userConnector.CheckUsernameExists(updateProfileDTO.username) == true)
                    {
                        response.AvaChemCode = 400;
                        response.AvaChemMessage = "Username already exists";
                        return Ok(response);
                    }
                }

                var prepareUser = new User
                {
                    ID = int.Parse(uid),
                    Name = updateProfileDTO.name ?? currentUser.Name,
                    Email = updateProfileDTO.email ?? currentUser.Email,
                    Phone = updateProfileDTO.phone ?? currentUser.Phone,
                    Username = updateProfileDTO.username ?? currentUser.Username,
                    Password = updateProfileDTO.password + "" != "" ? Utils.Hash(updateProfileDTO.password) : currentUser.Password,
                };
                var updatedUser = userConnector.UpdateUserPartial(prepareUser);

                var rawUser = JsonConvert.SerializeObject(updatedUser);
                UserDetailsDTO userDetailsDTO = JsonConvert.DeserializeObject<UserDetailsDTO>(rawUser);
                UserRole role = new UserRoleDataConnector().GetUserRoleByFirstVariable(userDetailsDTO.RoleID);
                userDetailsDTO.Role = role;

                response.AvaChemCode = 200;
                response.AvaChemMessage = "OK";
                response.AvaChemReturnObject = userDetailsDTO;
            }
            catch (Exception)
            {
                response.AvaChemCode = 400;
                response.AvaChemMessage = "Error";
            }

            return Ok(response);
        }


        [HttpPost]
        [ActionName("Register")]
        public IHttpActionResult Register([FromBody] RegisterDTO registerDTO)
        {
            AvaChemResponse response = new AvaChemResponse();
            try
            {
                //UserDataConnector userConnector = new UserDataConnector();

                //if (userConnector.CheckEmailExists(registerDTO.email) == true)
                //{
                //    response.AvaChemCode = 400;
                //    response.AvaChemMessage = "Email already exists";
                //    return Ok(response);
                //}
                //if (userConnector.CheckUsernameExists(registerDTO.username) == true)
                //{
                //    response.AvaChemCode = 400;
                //    response.AvaChemMessage = "Username already exists";
                //    return Ok(response);
                //}

                //User prepareUser = new User()
                //{
                //    RoleID = UserRoles.Guest.GetHashCode(),
                //    Username = registerDTO.username,
                //    Password = Utils.Hash(registerDTO.password),
                //    IDNumber = "",
                //    Name = registerDTO.name,
                //    DOB = DateTime.Now,
                //    Phone = "",
                //    Email = registerDTO.email,
                //    Credits = 0,
                //    LeaveDaysLeft = 0,
                //    MCDaysLeft = 0,
                //    UserStatus = 1,
                //    SoftDelete = false
                //};
                //var createdUser = userConnector.CreateUser(prepareUser);

                //var rawUser = JsonConvert.SerializeObject(createdUser);
                //UserDetailsDTO userDetailsDTO = JsonConvert.DeserializeObject<UserDetailsDTO>(rawUser);
                //UserRole role = new UserRoleDataConnector().GetUserRoleByFirstVariable(userDetailsDTO.RoleID);
                //userDetailsDTO.Role = role;

                //response.AvaChemCode = 200;
                //response.AvaChemMessage = "OK";
                //response.AvaChemReturnObject = userDetailsDTO;
                response.AvaChemCode = 400;
                response.AvaChemMessage = "Error";
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
        [ActionName("GetStaffs")]
        public IHttpActionResult GetStaffs([FromUri] int? page = null, int? per_page = null, string search = "", string roles = "")
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                List<int> userRoles = string.IsNullOrEmpty(roles) ? null : roles.Split(',').Select(int.Parse).ToList<int>();
                response.AvaChemCode = 200;
                response.AvaChemMessage = "OK";
                response.AvaChemReturnObject = new UserDataConnector().GetAll(page, per_page, search, userRoles).Select(u => new WorkerDTO
                {
                    ID = u.ID,
                    Name = u.Name,
                    Email = u.Email,
                    Phone = u.Phone,
                    RoleID = u.RoleID,
                    Credits = u.Credits
                });

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
        [ActionName("GetVehicles")]
        public IHttpActionResult GetVehicles([FromUri] int? page = null, int? per_page = null, string search = "")
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                response.AvaChemCode = 200;
                response.AvaChemMessage = "OK";
                response.AvaChemReturnObject = new VehicleDataConnector().GetAll(page, per_page, search);

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