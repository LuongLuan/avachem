using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using AvaChemAdminPanelMobiAPI.Common_File;
using Newtonsoft.Json;

namespace AvaChemAdminPanelMobiAPI
{
    public partial class CreateCredit : System.Web.UI.Page
    {
        public const string DASHBOARD = Routes.DASHBOARD;
        public const string CREDITS = Routes.CREDITS;


        protected void Page_Load(object sender, EventArgs e)
        {
            // add this to everypage so User have to login to access to data/webpage
            // Session["admin"] the "admin" inside is a container to link to other pages
            // it's not connected to the class and dataconnector
            int role = (Convert.ToInt32(Session["role"]));
            if (Session["admin"] == null
                || !(new int[] { UserRoles.SuperAdmin.GetHashCode(), UserRoles.OverallAdmin.GetHashCode(), UserRoles.CreditAdmin.GetHashCode() }).Contains(role))
            {
                Response.Redirect(Routes.LOG_IN);
            }

            // when page load it will hide the succuss and error color label for the message first
            successLabel.Visible = false;
            warningLabel.Visible = false;

            // upon creating successfully it will display the success msg with the success label
            if (Request.QueryString["message"] == "1")
            {
                successLabel.InnerText = "Credit has been created";
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
                foreach (var u in new UserDataConnector().GetAll())
                {
                    var uKey = new UserWithCreditsDTO { ID = u.ID, Name = u.Name, Credits = u.Credits };
                    ddlUser.Items.Add(new ListItem($"{u.Name} ({u.IDNumber})", JsonConvert.SerializeObject(uKey)));
                }
                ddlUser.Items.Insert(0, new ListItem("-- Select User --", ""));
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Routes.CREDITS);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ddlUser.SelectedValue == ""
                || Type.Value == ""
                || tbxAmount.Value == ""
                || tbxDescription.Value == "")
            {
                //Response.Redirect(Utils.GetRedirectUrl(Routes.CREATE_CREDIT, 6), false);
                return;
            }
            try
            {

                var selectedUser = JsonConvert.DeserializeObject<UserWithCreditsDTO>(ddlUser.SelectedValue);
                Credit prepareCredit = new Credit()
                {
                    UserID = selectedUser.ID,
                    Amount = Convert.ToDouble($"{Type.Value}{Math.Abs(Convert.ToDouble(tbxAmount.Value.NullIfWhiteSpace()))}"),
                    Description = tbxDescription.Value,
                    SoftDelete = false
                };
                var createdCredit = new CreditDataConnector().CreateCredit(prepareCredit);

                // Log
                Utils.Log(new CreateLogDTO
                {
                    ActionName = $@"Creates a credit record (Amount: {createdCredit.Amount})",
                    ActionLocation = Page.AppRelativeVirtualPath,
                    ActionType = LogActionTypes.Create.GetHashCode(),
                    ActionMeta = new LogActionMeta
                    {
                        Input = prepareCredit,
                        Result = createdCredit
                    }
                }, Session);

                new UserDataConnector().UpdateCredits(
                    createdCredit.UserID,
                    Math.Abs(createdCredit.Amount),
                    createdCredit.Amount >= 0 ? UpdateCreditsType.Increase : UpdateCreditsType.Decrease
                );

                // Log
                Utils.Log(new CreateLogDTO
                {
                    ActionName = $@"Updates credits for user (UserID: {createdCredit.UserID} - {selectedUser.Name})",
                    ActionLocation = Page.AppRelativeVirtualPath,
                    ActionType = LogActionTypes.Update.GetHashCode(),
                    ActionMeta = new LogActionMeta
                    {
                        Input = new { createdCredit.UserID, createdCredit.Amount }
                    }
                }, Session);


                // Refresh credits in mobile app (background task in BE)
                Task.Run(async () =>
                {
                    await Utils.PushNotif(createdCredit.UserID, null, new FCMDataBody
                    {
                        code = FCMDataCodes.REFRESH_CREDITS
                    });
                }).ConfigureAwait(false);


                Response.Redirect(Utils.GetRedirectUrl(Routes.CREDITS, 2), false);
            }
            catch (Exception)
            {
                Response.Redirect(Utils.GetRedirectUrl(Routes.CREATE_CREDIT, 99), false);
            }

        }
    }
}