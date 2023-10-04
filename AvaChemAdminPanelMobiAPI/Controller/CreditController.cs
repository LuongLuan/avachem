using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AvaChemAdminPanelMobiAPI.Common_File;
using Newtonsoft.Json;

namespace AvaChemAdminPanelMobiAPI.Controller
{
    public class CreditController : ApiController
    {
        [Authorize]
        [HttpGet]
        [ActionName("GetCredits")]
        public IHttpActionResult GetCredits([FromUri] int? page = null, int? per_page = null, int? month = null, int? year = null,
                                                int? staff_id = null, string search = "", int? sort = null)
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                int? userID = int.Parse(uid);
                User currentUser = new UserDataConnector().GetUsersByFirstVariable((int)userID);
                if (currentUser.RoleID == UserRoles.CreditAdmin.GetHashCode()) userID = staff_id;
                CreditLogsAndCount<CreditTableView> creditLogsAndCount = new CreditDataConnector().GetAll<CreditTableView>(page, per_page, search, null, null, currentUser.RoleID == UserRoles.CreditAdmin.GetHashCode(), userID, month, year, sort);
                response.AvaChemCode = 200;
                response.AvaChemMessage = "OK";
                response.AvaChemReturnObject = creditLogsAndCount;
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
        [ActionName("UpdateCredits")]
        public IHttpActionResult UpdateCredits([FromBody] UpdateCreditsDTO updateCreditsDTO)
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var uid = identity.FindFirst(ClaimTypes.UserData).Value;

            AvaChemResponse response = new AvaChemResponse();
            try
            {
                User currentUser = new UserDataConnector().GetUsersByFirstVariable(int.Parse(uid));
                if (currentUser == null
                    || currentUser.RoleID != UserRoles.CreditAdmin.GetHashCode())
                {
                    response.AvaChemMessage = "Error";
                    response.AvaChemCode = 400;
                    return Ok(response);
                }

                Credit prepareCredit = new Credit()
                {
                    UserID = updateCreditsDTO.userId,
                    Amount = updateCreditsDTO.amount,
                    Description = updateCreditsDTO.description,
                    SoftDelete = false
                };
                var createdCredit = new CreditDataConnector().CreateCredit(prepareCredit);

                UserDataConnector userDataConnector = new UserDataConnector();
                userDataConnector.UpdateCredits(
                    createdCredit.UserID,
                    Math.Abs(createdCredit.Amount),
                    createdCredit.Amount >= 0 ? UpdateCreditsType.Increase : UpdateCreditsType.Decrease
                );

                // Refresh credits in mobile app (background task in BE)
                Task.Run(async () =>
                {
                    await Utils.PushNotif(createdCredit.UserID, null, new FCMDataBody
                    {
                        code = FCMDataCodes.REFRESH_CREDITS
                    });
                }).ConfigureAwait(false);

                response.AvaChemCode = 200;
                response.AvaChemMessage = "OK";

                User u = userDataConnector.GetUsersByFirstVariable(createdCredit.UserID);
                var rawCredit = JsonConvert.SerializeObject(createdCredit);
                CreditTableView creditDTO = JsonConvert.DeserializeObject<CreditTableView>(rawCredit);
                creditDTO.UName = u.Name;
                creditDTO.UIDNumber = u.IDNumber;
                creditDTO.UCredits = u.Credits;

                response.AvaChemReturnObject = creditDTO;
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