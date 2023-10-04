using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvaChemAdminPanelMobiAPI.Common_File;
using Newtonsoft.Json;

namespace AvaChemAdminPanelMobiAPI
{
    public partial class UpdateUser : System.Web.UI.Page
    {
        public const string DASHBOARD = Routes.DASHBOARD;
        public const string USERS = Routes.USERS;
        public string dataActionTypes = "";

        public User thisUser;
        public List<Qualification> listQ;
        private List<AssignQualificationDTO> listCQ = new List<AssignQualificationDTO>();

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

            // when update and value is null add if and else statement
            if (Request.QueryString["id"] != null)
            {
                int id = Convert.ToInt32(Request.QueryString["id"]);
                thisUser = new UserDataConnector().GetUsersByFirstVariable(id);

                if (role == UserRoles.HR.GetHashCode() && !(new int[] { UserRoles.Worker.GetHashCode(), UserRoles.Driver.GetHashCode() }.Contains(thisUser.RoleID)))
                {
                    Response.Redirect(Routes.LOG_IN);
                }

                listQ = new QualificationDataConnector().GetByUserID(id).ToList<Qualification>();
                foreach (var q in listQ)
                {
                    listCQ.Add(new AssignQualificationDTO
                    {
                        ID = q.ID,
                        Name = q.Name,
                        DateObtained = q.DateObtained.ToString("yyyy-MM-dd"),
                        ExpiryDate = q.ExpiryDate.ToString("yyyy-MM-dd")
                    });
                }
            }
            else
            {
                Response.Redirect(Routes.USERS);
            }

            //
            if (Request.QueryString["message"] == "2")
            {
                successLabel.InnerText = "User has been updated";
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

                ddlRole.SelectedValue = thisUser.RoleID.ToString();
                tbxUsername.Text = thisUser.Username;
                tbxPassword.Text = thisUser.Password;
                tbxIDNumber.Text = thisUser.IDNumber;
                tbxName.Text = thisUser.Name;
                //dtDob.Value = thisUser.DOB.ToString("yyyy-MM-dd");
                tbxPhone.Text = thisUser.Phone;
                tbxEmail.Text = thisUser.Email;
                tbxCredits.Text = thisUser.Credits.ToString();
                tbxLeaveDaysLeft.Text = thisUser.LeaveDaysLeft.ToString();
                tbxMCDaysLeft.Text = thisUser.MCDaysLeft.ToString();

                dataActionTypes = JsonConvert.SerializeObject(Enum.GetValues(typeof(DataActionTypes)), new Newtonsoft.Json.Converters.StringEnumConverter());

                QValues.Value = JsonConvert.SerializeObject(listCQ);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Routes.USERS);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);
            if (id + "" == ""
                || tbxUsername.Text == ""
                || tbxPassword.Text == ""
                || tbxIDNumber.Text == ""
                || tbxName.Text == ""
                //|| dtDob.Value == ""
                || tbxPhone.Text == ""
                //|| tbxEmail.Text == ""
                || ddlRole.SelectedValue == "")
            {
                //Response.Redirect(Utils.GetRedirectUrl(Routes.UPDATE_USER, 6, new { id }), false);
                return;
            }
            try
            {

                var userConnector = new UserDataConnector();
                //if (thisUser.Email != tbxEmail.Text)
                //{
                //    if (userConnector.CheckEmailExists(tbxEmail.Text) == true)
                //    {
                //        Response.Redirect(Utils.GetRedirectUrl(Routes.UPDATE_USER, 7, new { id }), false);
                //        return;
                //    }
                //}

                if (thisUser.Username != tbxUsername.Text)
                {
                    if (userConnector.CheckUsernameExists(tbxUsername.Text) == true)
                    {
                        Response.Redirect(Utils.GetRedirectUrl(Routes.UPDATE_USER, 8, new { id }), false);
                        return;
                    }
                }

                User prepareUser = new User()
                {
                    ID = id,
                    RoleID = int.Parse(ddlRole.SelectedValue),
                    Username = tbxUsername.Text,
                    Password = thisUser.Password != tbxPassword.Text ? Utils.Hash(tbxPassword.Text) : thisUser.Password,
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

                var updatedUser = userConnector.UpdateUser(prepareUser);

                // Log
                Utils.Log(new CreateLogDTO
                {
                    ActionName = $@"Updates an user (UserID: {prepareUser.ID})",
                    ActionLocation = Page.AppRelativeVirtualPath,
                    ActionType = LogActionTypes.Update.GetHashCode(),
                    ActionMeta = new LogActionMeta
                    {
                        Input = prepareUser,
                        Result = updatedUser
                    }
                }, Session);

                if (updatedUser != null)
                {
                    int[] createActionTypes = { DataActionTypes.Create.GetHashCode(), DataActionTypes.Update.GetHashCode() };

                    // Update Qualifications
                    if (QValues.Value + "" != "")
                    {
                        var qValues = JsonConvert.DeserializeObject<List<AssignQualificationDTO>>(QValues.Value);
                        foreach (AssignQualificationDTO q in qValues)
                        {
                            if (q.ActionType + "" == "") continue;
                            if (q.ActionType == DataActionTypes.Hide.GetHashCode() && q.ID > 0)
                            {
                                new QualificationDataConnector().UpdateQualificationSoftDelete(q.ID);
                            }
                            else
                            {
                                var nQ = new Qualification
                                {
                                    Name = q.Name,
                                    DateObtained = Convert.ToDateTime(q.DateObtained),
                                    ExpiryDate = Convert.ToDateTime(q.ExpiryDate),
                                    UserID = updatedUser.ID,
                                    SoftDelete = false
                                };
                                if (Array.IndexOf(createActionTypes, q.ActionType) > -1 && q.ID < 0)
                                {
                                    new QualificationDataConnector().CreateQualification(nQ);
                                }
                                else if (q.ActionType == DataActionTypes.Update.GetHashCode() && q.ID > 0)
                                {
                                    nQ.ID = q.ID;
                                    new QualificationDataConnector().UpdateQualification(nQ);
                                }
                            }
                        }

                    }
                }

                // Refresh credits in mobile app (background task in BE)
                if (updatedUser.Credits != thisUser.Credits)
                {
                    Task.Run(async () =>
                    {
                        await Utils.PushNotif(updatedUser.ID, null, new FCMDataBody
                        {
                            code = FCMDataCodes.REFRESH_CREDITS
                        });
                    }).ConfigureAwait(false);
                }


                Response.Redirect(Utils.GetRedirectUrl(Routes.USERS, 2), false);
            }
            catch (Exception)
            {
                Response.Redirect(Utils.GetRedirectUrl(Routes.UPDATE_USER, 99, new { id }), false);
            }
        }
    }
}