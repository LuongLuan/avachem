using System;
using System.Linq;
using System.Web.UI.WebControls;
using AvaChemAdminPanelMobiAPI.Common_File;
using Newtonsoft.Json;

namespace AvaChemAdminPanelMobiAPI
{
    public partial class UpdateCredit : System.Web.UI.Page
    {
        public const string DASHBOARD = Routes.DASHBOARD;
        public const string CREDITS = Routes.CREDITS;

        public Credit thisCredit;

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

            // when update and value is null add if and else statement
            if (Request.QueryString["id"] != null)
            {
                Response.Redirect(Routes.CREDITS); // Hide this page

                //Id is a number so we have to use this to convert it to int
                int id = Convert.ToInt32(Request.QueryString["id"]);
                thisCredit = new CreditDataConnector().GetCreditByFirstVariable(id);
            }
            else
            {
                Response.Redirect(Routes.CREDITS);
            }

            //
            if (Request.QueryString["message"] == "2")
            {
                successLabel.InnerText = "Credits has been updated";
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

            //do not add in Id and softdelete!!
            // for date and int have to convert them to string by adding .ToString();
            //for delete btn part
            if (!IsPostBack)
            {
                foreach (var u in new UserDataConnector().GetAll())
                {
                    var uKey = new UserWithCreditsDTO { ID = u.ID, Name = u.Name, Credits = u.Credits };
                    var item = new ListItem($"{u.Name} ({u.IDNumber})", JsonConvert.SerializeObject(uKey));
                    if (u.ID == thisCredit.UserID) item.Selected = true;
                    ddlUser.Items.Add(item);
                }
                //ddlUser.Items.Insert(0, new ListItem("-- Select User --", ""));

                tbxAmount.Value = thisCredit.Amount.ToString();
                tbxDescription.Value = thisCredit.Description;

            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Routes.CREDITS);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);
            if (id + "" == ""
                || ddlUser.SelectedValue == ""
                || Type.Value == ""
                || tbxAmount.Value == ""
                || tbxDescription.Value == "")
            {
                //Response.Redirect(Utils.GetRedirectUrl(Routes.UPDATE_CREDIT, 6, new { id }), false);
                return;
            }
            try
            {

                var selectedUser = JsonConvert.DeserializeObject<UserWithCreditsDTO>(ddlUser.SelectedValue);
                Credit prepareCredit = new Credit()
                {
                    ID = id,
                    UserID = selectedUser.ID,
                    Amount = Convert.ToDouble($"{Type.Value}{Math.Abs(Convert.ToDouble(tbxAmount.Value.NullIfWhiteSpace()))}"),
                    Description = tbxDescription.Value,
                    SoftDelete = false
                };
                var updatedCredit = new CreditDataConnector().UpdateCredit(prepareCredit);

                // Log
                Utils.Log(new CreateLogDTO
                {
                    ActionName = $@"Updates a credit record (CreditID: {updatedCredit.ID})",
                    ActionLocation = Page.AppRelativeVirtualPath,
                    ActionType = LogActionTypes.Update.GetHashCode(),
                    ActionMeta = new LogActionMeta
                    {
                        Input = prepareCredit,
                        Result = updatedCredit
                    }
                }, Session);


                Response.Redirect(Utils.GetRedirectUrl(Routes.CREDITS, 2), false);
            }
            catch (Exception)
            {
                Response.Redirect(Utils.GetRedirectUrl(Routes.UPDATE_CREDIT, 99, new { id }), false);
            }

        }
    }

    public class UserWithCreditsDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Credits { get; set; }
    }
}