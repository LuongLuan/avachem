using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using AvaChemAdminPanelMobiAPI.Common_File;
using Newtonsoft.Json;

namespace AvaChemAdminPanelMobiAPI
{
    public partial class CreateUser : System.Web.UI.Page
    {
        public const string DASHBOARD = Routes.DASHBOARD;
        public const string USERS = Routes.USERS;
        public string dataActionTypes = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            // add this to everypage so User have to login to access to data/webpage
            // Session["admin"] the "admin" inside is a container to link to other pages
            // it's not connected to the class and dataconnector
            int role = (Convert.ToInt32(Session["role"]));
            if (Session["admin"] == null
                || !(new int[] { UserRoles.SuperAdmin.GetHashCode(), UserRoles.HR.GetHashCode() }).Contains(role))
            {
                Response.Redirect(Routes.LOG_IN);
            }

            // when page load it will hide the succuss and error color label for the message first
            successLabel.Visible = false;
            warningLabel.Visible = false;

            // upon creating successfully it will display the success msg with the success label
            if (Request.QueryString["message"] == "1")
            {
                successLabel.InnerText = "User has been created";
                successLabel.Visible = true;
            }
            else if (Request.QueryString["message"] == "6")
            {
                warningLabel.InnerText = "Please fill in the blanks!";
                warningLabel.Visible = true;
            }
            else if (Request.QueryString["message"] == "7")
            {
                warningLabel.InnerText = "Email already exists";
                warningLabel.Visible = true;
            }
            else if (Request.QueryString["message"] == "8")
            {
                warningLabel.InnerText = "Username already exists";
                warningLabel.Visible = true;
            }
            else if (Request.QueryString["message"] == "99")
            {
                warningLabel.InnerText = "Something went wrong!";
                warningLabel.Visible = true;
            }

            if (!IsPostBack)
            {
                ddlRole.DataValueField = "ID";
                ddlRole.DataTextField = "RoleName";
                ddlRole.DataSource = new UserRoleDataConnector().GetAll(role);
                ddlRole.DataBind();
                ddlRole.Items.Insert(0, new ListItem("-- Select Role --", ""));

                dataActionTypes = JsonConvert.SerializeObject(Enum.GetValues(typeof(DataActionTypes)), new Newtonsoft.Json.Converters.StringEnumConverter());
            }

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Routes.USERS);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (tbxUsername.Text == ""
               || tbxPassword.Text == ""
               || tbxIDNumber.Text == ""
               || tbxName.Text == ""
               //|| dtDob.Value == ""
               || tbxPhone.Text == ""
               //|| tbxEmail.Text == ""
               || ddlRole.SelectedValue == "")
            {
                //Response.Redirect(Utils.GetRedirectUrl(Routes.CREATE_USER, 6), false);
                return;
            }
            try
            {

                var userConnector = new UserDataConnector();
                //if (userConnector.CheckEmailExists(tbxEmail.Text) == true)
                //{
                //    Response.Redirect(Utils.GetRedirectUrl(Routes.CREATE_USER, 7), false);
                //    return;
                //}

                if (userConnector.CheckUsernameExists(tbxUsername.Text) == true)
                {
                    Response.Redirect(Utils.GetRedirectUrl(Routes.CREATE_USER, 8), false);
                    return;
                }

                User prepareUser = new User()
                {
                    RoleID = int.Parse(ddlRole.SelectedValue),
                    Username = tbxUsername.Text,
                    Password = Utils.Hash(tbxPassword.Text),
                    IDNumber = tbxIDNumber.Text,
                    Name = tbxName.Text,
                    //DOB = Convert.ToDateTime(dtDob.Value),
                    Phone = tbxPhone.Text,
                    Email = tbxEmail.Text,
                    Credits = Convert.ToDouble(tbxCredits.Text.NullIfWhiteSpace()),
                    LeaveDaysLeft = tbxLeaveDaysLeft.Text.NullIfWhiteSpace() == null ? 0 : float.Parse(tbxLeaveDaysLeft.Text.NullIfWhiteSpace()),
                    MCDaysLeft = tbxMCDaysLeft.Text.NullIfWhiteSpace() == null ? 0 : float.Parse(tbxMCDaysLeft.Text.NullIfWhiteSpace()),
                    UserStatus = 1,
                    SoftDelete = false
                };
                var createdUser = userConnector.CreateUser(prepareUser);

                // Log
                Utils.Log(new CreateLogDTO
                {
                    ActionName = $@"Creates an user (User: {prepareUser.Name} - {prepareUser.IDNumber})",
                    ActionLocation = Page.AppRelativeVirtualPath,
                    ActionType = LogActionTypes.Create.GetHashCode(),
                    ActionMeta = new LogActionMeta
                    {
                        Input = prepareUser,
                        Result = createdUser
                    }
                }, Session);


                if (createdUser != null)
                {
                    int[] createActionTypes = { DataActionTypes.Create.GetHashCode(), DataActionTypes.Update.GetHashCode() };

                    // Create Qualifications
                    if (QValues.Value + "" != "")
                    {
                        var qValues = JsonConvert.DeserializeObject<List<AssignQualificationDTO>>(QValues.Value);
                        foreach (AssignQualificationDTO q in qValues)
                        {
                            if (q.ActionType + "" == "") continue;
                            else
                            {
                                var nQ = new Qualification
                                {
                                    Name = q.Name,
                                    DateObtained = Convert.ToDateTime(q.DateObtained),
                                    ExpiryDate = Convert.ToDateTime(q.ExpiryDate),
                                    UserID = createdUser.ID,
                                    SoftDelete = false
                                };
                                if (Array.IndexOf(createActionTypes, q.ActionType) > -1 && q.ID < 0)
                                {
                                    new QualificationDataConnector().CreateQualification(nQ);
                                }
                                //else if (q.ActionType == DataActionTypes.Update.GetHashCode() && q.ID > 0)
                                //{
                                //    nQ.ID = q.ID;
                                //    new QualificationDataConnector().UpdateQualification(nQ);
                                //}
                            }
                        }

                    }
                }


                Response.Redirect(Utils.GetRedirectUrl(Routes.USERS, 1), false);
            }
            catch (Exception)
            {
                Response.Redirect(Utils.GetRedirectUrl(Routes.CREATE_USER, 99), false);
            }
        }
    }
}