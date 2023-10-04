using System;
using System.Linq;
using System.Web.UI.WebControls;
using AvaChemAdminPanelMobiAPI.Common_File;
using Newtonsoft.Json;

namespace AvaChemAdminPanelMobiAPI
{
    public partial class UpdateLeave : System.Web.UI.Page
    {
        public const string DASHBOARD = Routes.DASHBOARD;
        public const string LEAVES = Routes.LEAVES;

        public Leave thisLeave;

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

            // when update and value is null add if and else statement
            if (Request.QueryString["id"] != null)
            {
                int id = int.Parse(Request.QueryString["id"]);
                thisLeave = new LeaveDataConnector().GetLeaveByFirstVariable(id);
            }
            else
            {
                Response.Redirect(Routes.LEAVES);
            }

            // upon creating successfully it will display the success msg with the success label
            if (Request.QueryString["message"] == "2")
            {
                successLabel.InnerText = "Leave has been updated";
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
                    if (u.ID == thisLeave.UserID) item.Selected = true;
                    ddlUser.Items.Add(item);
                }
                //ddlUser.Items.Insert(0, new ListItem("-- Select User --", ""));

                ddlStatus.DataValueField = "ID";
                ddlStatus.DataTextField = "Name";
                ddlStatus.DataSource = new StatusDataConnector().GetAll();
                ddlStatus.DataBind();
                ddlStatus.SelectedValue = thisLeave.StatusID.ToString();

                ddlReason.DataValueField = "ID";
                ddlReason.DataTextField = "Name";
                ddlReason.DataSource = new LeaveReasonDataConnector().GetAll();
                ddlReason.DataBind();
                // ddlReason.Items.Insert(0, new ListItem("-- Select Reason --", ""));
                ddlReason.SelectedValue = thisLeave.ReasonID.ToString();


                dtStart.Value = thisLeave.StartedDate.ToString("yyyy-MM-dd");
                dtEnd.Value = thisLeave.EndedDate.ToString("yyyy-MM-dd");
                tbxNumDays.Value = thisLeave.NumDays.ToString();
                tbxRemarks.Value = thisLeave.Remarks;

                if (thisLeave.ProofImage + "" != "")
                {
                    lnkImage.NavigateUrl = thisLeave.ProofImage;
                    imgProof.ImageUrl = thisLeave.ProofImage;
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Routes.LEAVES);
        }
        protected async void btnSubmit_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);
            if (id + "" == ""
                || ddlUser.SelectedValue == ""
                || ddlStatus.SelectedValue == ""
                || ddlReason.SelectedValue == ""
                || dtStart.Value == ""
                || dtEnd.Value == ""
                || tbxNumDays.Value == "")
            {
                //Response.Redirect(Utils.GetRedirectUrl(Routes.UPDATE_LEAVE, 6, new { id }), false);
                return;
            }
            try
            {

                string proofImageUrl = thisLeave.ProofImage;
                if (fuUpload.PostedFile != null && fuUpload.PostedFile.ContentLength > 0)
                {
                    proofImageUrl = await Utils.UploadFile($"leave{id}", fuUpload.PostedFile, SupportedFileTypes.Image);
                }

                var selectedUser = JsonConvert.DeserializeObject<UserWithLeaveDaysLeftDTO>(ddlUser.SelectedValue);
                Leave prepareLeave = new Leave()
                {
                    ID = id,
                    UserID = selectedUser.ID,
                    StatusID = int.Parse(ddlStatus.SelectedValue),
                    ReasonID = int.Parse(ddlReason.SelectedValue),
                    StartedDate = Convert.ToDateTime(dtStart.Value),
                    EndedDate = Convert.ToDateTime(dtEnd.Value),
                    NumDays = float.Parse(tbxNumDays.Value.ToString()),
                    Remarks = tbxRemarks.InnerText,
                    ProofImage = proofImageUrl,
                    SoftDelete = false
                };
                var updatedLeave = new LeaveDataConnector().UpdateLeave(prepareLeave);

                // Log
                Utils.Log(new CreateLogDTO
                {
                    ActionName = $@"Updates a leave (LeaveID: {prepareLeave.ID})",
                    ActionLocation = Page.AppRelativeVirtualPath,
                    ActionType = LogActionTypes.Update.GetHashCode(),
                    ActionMeta = new LogActionMeta
                    {
                        Input = prepareLeave,
                        Result = updatedLeave
                    }
                }, Session);


                // Decrease the days of user
                if ((thisLeave.StatusID == Statuses.Pending.GetHashCode() || thisLeave.StatusID == Statuses.Rejected.GetHashCode())
                    && updatedLeave.StatusID == Statuses.Approved.GetHashCode())
                {
                    if (updatedLeave.ReasonID == LeaveReasons.Medical.GetHashCode())
                    {
                        new UserDataConnector().UpdateLeaveDays(
                            updatedLeave.UserID,
                            UpdateLeaveDaysType.Decrease,
                            null,
                            updatedLeave.NumDays
                        );
                    }
                    else
                    {
                        new UserDataConnector().UpdateLeaveDays(
                            updatedLeave.UserID,
                            UpdateLeaveDaysType.Decrease,
                            updatedLeave.NumDays,
                            null
                        );
                    }

                    // Log
                    var fieldName = updatedLeave.ReasonID == LeaveReasons.Medical.GetHashCode() ? "MC" : "leave";
                    Utils.Log(new CreateLogDTO
                    {
                        ActionName = $@"Decrease {fieldName} days left of user (UserID: {updatedLeave.UserID} - {selectedUser.Name})",
                        ActionLocation = Page.AppRelativeVirtualPath,
                        ActionType = LogActionTypes.Update.GetHashCode(),
                        ActionMeta = new LogActionMeta
                        {
                            Input = new { updatedLeave.UserID, updatedLeave.NumDays }
                        }
                    }, Session);
                }

                // Recover the days for user
                if (thisLeave.StatusID == Statuses.Approved.GetHashCode()
                    && (updatedLeave.StatusID == Statuses.Pending.GetHashCode() || updatedLeave.StatusID == Statuses.Rejected.GetHashCode()))
                {
                    if (updatedLeave.ReasonID == LeaveReasons.Medical.GetHashCode())
                    {
                        new UserDataConnector().UpdateLeaveDays(
                            updatedLeave.UserID,
                            UpdateLeaveDaysType.Increase,
                            null,
                            updatedLeave.NumDays
                        );
                    }
                    else
                    {
                        new UserDataConnector().UpdateLeaveDays(
                            updatedLeave.UserID,
                            UpdateLeaveDaysType.Increase,
                            updatedLeave.NumDays,
                            null
                        );
                    }

                    // Log
                    var fieldName = updatedLeave.ReasonID == LeaveReasons.Medical.GetHashCode() ? "MC" : "leave";
                    Utils.Log(new CreateLogDTO
                    {
                        ActionName = $@"Increase {fieldName} days left of user (UserID: {updatedLeave.UserID} - {selectedUser.Name})",
                        ActionLocation = Page.AppRelativeVirtualPath,
                        ActionType = LogActionTypes.Update.GetHashCode(),
                        ActionMeta = new LogActionMeta
                        {
                            Input = new { updatedLeave.UserID, updatedLeave.NumDays }
                        }
                    }, Session);
                }


                Response.Redirect(Utils.GetRedirectUrl(Routes.LEAVES, 2), false);
            }
            catch (Exception)
            {
                Response.Redirect(Utils.GetRedirectUrl(Routes.UPDATE_LEAVE, 99, new { id }), false);
            }

        }
    }

    public class UserWithLeaveDaysLeftDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public float LeaveDaysLeft { get; set; }
        public float MCDaysLeft { get; set; }
    }
}