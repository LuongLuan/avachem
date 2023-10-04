using System;
using System.Linq;
using System.Web.UI.WebControls;
using AvaChemAdminPanelMobiAPI.Common_File;
using Newtonsoft.Json;

namespace AvaChemAdminPanelMobiAPI
{
    public partial class CreateLeave : System.Web.UI.Page
    {
        public const string DASHBOARD = Routes.DASHBOARD;
        public const string LEAVES = Routes.LEAVES;

        protected void Page_Load(object sender, EventArgs e)
        {
            // add this to everypage so User have to login to access to data/webpage
            // Session["admin"] the "admin" inside is a container to link to other pages
            // it's not connected to the class and dataconnector
            int role = (Convert.ToInt32(Session["role"]));
            if (Session["admin"] == null
                || !(new int[] { UserRoles.SuperAdmin.GetHashCode(), UserRoles.OverallAdmin.GetHashCode() }).Contains(role))
            {
                Response.Redirect(Routes.LOG_IN);
            }

            // when page load it will hide the succuss and error color label for the message first
            successLabel.Visible = false;
            warningLabel.Visible = false;

            // upon creating successfully it will display the success msg with the success label
            if (Request.QueryString["message"] == "1")
            {
                successLabel.InnerText = "Leave has been created";
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
                    var uKey = new UserWithLeaveDaysLeftDTO { ID = u.ID, Name = u.Name, LeaveDaysLeft = u.LeaveDaysLeft, MCDaysLeft = u.MCDaysLeft };
                    var item = new ListItem($"{u.Name} ({u.IDNumber})", JsonConvert.SerializeObject(uKey));
                    ddlUser.Items.Add(item);
                }
                ddlUser.Items.Insert(0, new ListItem("-- Select User --", ""));

                ddlStatus.DataValueField = "ID";
                ddlStatus.DataTextField = "Name";
                ddlStatus.DataSource = new StatusDataConnector().GetAll();
                ddlStatus.DataBind();

                ddlReason.DataValueField = "ID";
                ddlReason.DataTextField = "Name";
                ddlReason.DataSource = new LeaveReasonDataConnector().GetAll();
                ddlReason.DataBind();
                ddlReason.Items.Insert(0, new ListItem("-- Select Reason --", ""));
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Routes.LEAVES);
        }
        protected async void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ddlUser.SelectedValue == ""
                || ddlStatus.SelectedValue == ""
                || ddlReason.SelectedValue == ""
                || dtStart.Value == ""
                || dtEnd.Value == ""
                || tbxNumDays.Value == "")
            {
                //Response.Redirect(Utils.GetRedirectUrl(Routes.CREATE_LEAVE, 6), false);
                return;
            }
            try
            {

                var selectedUser = JsonConvert.DeserializeObject<UserWithLeaveDaysLeftDTO>(ddlUser.SelectedValue);
                Leave prepareLeave = new Leave()
                {
                    UserID = selectedUser.ID,
                    StatusID = int.Parse(ddlStatus.SelectedValue),
                    ReasonID = int.Parse(ddlReason.SelectedValue),
                    StartedDate = Convert.ToDateTime(dtStart.Value),
                    EndedDate = Convert.ToDateTime(dtEnd.Value),
                    NumDays = float.Parse(tbxNumDays.Value.ToString()),
                    Remarks = tbxRemarks.InnerText,
                    ProofImage = "", // proofImageUrl
                    SoftDelete = false
                };
                var createdLeave = new LeaveDataConnector().CreateLeave(prepareLeave);

                // Log
                Utils.Log(new CreateLogDTO
                {
                    ActionName = $@"Applies a leave (Reason: {((LeaveReasons)createdLeave.ReasonID).ToString()})",
                    ActionLocation = Page.AppRelativeVirtualPath,
                    ActionType = LogActionTypes.Create.GetHashCode(),
                    ActionMeta = new LogActionMeta
                    {
                        Input = prepareLeave,
                        Result = createdLeave
                    }
                }, Session);

                if (createdLeave != null)
                {
                    string proofImageUrl = "";
                    if (fuUpload.PostedFile != null && fuUpload.PostedFile.ContentLength > 0)
                    {
                        proofImageUrl = await Utils.UploadFile($"leave{createdLeave.ID}", fuUpload.PostedFile, SupportedFileTypes.Image);
                    }
                    new LeaveDataConnector().UpdateLeaveImageByID(createdLeave.ID, proofImageUrl);
                }

                // Decrease the days of user
                if (createdLeave.StatusID == Statuses.Approved.GetHashCode())
                {
                    if (createdLeave.ReasonID == LeaveReasons.Medical.GetHashCode())
                    {
                        new UserDataConnector().UpdateLeaveDays(
                            createdLeave.UserID,
                            UpdateLeaveDaysType.Decrease,
                            null,
                            createdLeave.NumDays
                        );
                    }
                    else
                    {
                        new UserDataConnector().UpdateLeaveDays(
                            createdLeave.UserID,
                            UpdateLeaveDaysType.Decrease,
                            createdLeave.NumDays,
                            null
                        );
                    }

                    // Log
                    var fieldName = createdLeave.ReasonID == LeaveReasons.Medical.GetHashCode() ? "MC" : "leave";
                    Utils.Log(new CreateLogDTO
                    {
                        ActionName = $@"Decrease {fieldName} days left of user (UserID: {createdLeave.UserID} - {selectedUser.Name})",
                        ActionLocation = Page.AppRelativeVirtualPath,
                        ActionType = LogActionTypes.Update.GetHashCode(),
                        ActionMeta = new LogActionMeta
                        {
                            Input = new { createdLeave.UserID, createdLeave.NumDays }
                        }
                    }, Session);
                }

                Response.Redirect(Utils.GetRedirectUrl(Routes.LEAVES, 1), false);
            }
            catch (Exception)
            {
                Response.Redirect(Utils.GetRedirectUrl(Routes.CREATE_LEAVE, 99), false);
            }

        }

    }
}