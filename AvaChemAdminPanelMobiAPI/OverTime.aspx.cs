using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using AvaChemAdminPanelMobiAPI.Common_File;
using Newtonsoft.Json;

namespace AvaChemAdminPanelMobiAPI
{
    public partial class Overtime : System.Web.UI.Page
    {
        public const string DASHBOARD = Routes.DASHBOARD;
        public const string OVERTIME = Routes.OT;
        public const string CREATE_OT = Routes.CREATE_OT;
        public const string UPDATE_OT = Routes.UPDATE_OT;

        public List<OT> list;
        public int i = 0;
        public int role;

        protected void Page_Load(object sender, EventArgs e)
        {
            // add this to everypage so User have to login to access to data/webpage
            // Session["admin"] the "admin" inside is a container to link to other pages
            // it's not connected to the class and dataconnector
            role = (Convert.ToInt32(Session["role"]));
            if (Session["admin"] == null
                || !(new int[] { UserRoles.SuperAdmin.GetHashCode(), UserRoles.OverallAdmin.GetHashCode(), UserRoles.HR.GetHashCode() }).Contains(role))
            {
                Response.Redirect(Routes.LOG_IN);
            }

            // when page load it will hide the succuss and error color label for the message first
            successLabel.Visible = false;
            warningLabel.Visible = false;

            // upon creating successfully it will display the success msg with the success label
            if (Request.QueryString["message"] == "1")
            {
                successLabel.InnerText = "OT has been created";
                successLabel.Visible = true;
            }
            else if (Request.QueryString["message"] == "2")
            {
                successLabel.InnerText = "OT has been updated";
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
                for (int i = 1; i < 13; i++)
                {
                    var item = new ListItem(DateTimeFormatInfo.CurrentInfo.GetMonthName(i), i.ToString());
                    ddlMonth.Items.Add(item);
                }
                ddlMonth.Items.Insert(0, new ListItem("-- Month --", "-1"));

                for (int i = 2022; i < 2023; i++)
                {
                    var item = new ListItem(i.ToString(), i.ToString());
                    ddlYear.Items.Add(item);
                }
                ddlYear.Items.Insert(0, new ListItem("-- Year --", "-1"));

                try
                {
                    int type = Convert.ToInt32(Request.QueryString["type"].NullIfWhiteSpace());
                    string search = Request.QueryString["search"].NullIfWhiteSpace();
                    int month = Request.QueryString["month"] == null ? DateTime.Now.Month : Convert.ToInt32(Request.QueryString["month"].NullIfWhiteSpace());
                    int year = Request.QueryString["year"] == null ? DateTime.Now.Year : Convert.ToInt32(Request.QueryString["year"].NullIfWhiteSpace());
                    int sort = Convert.ToInt32(Request.QueryString["sort"].NullIfWhiteSpace());
                    list = new OTDataConnector().GetAll(null, null, null, type, month, year, sort).ToList<OT>();

                    if (search != null)
                    {
                        List<OTDetailsDTO> OTDetailsDTOs = new List<OTDetailsDTO>();
                        foreach (var OT in list)
                        {
                            var rawOT = JsonConvert.SerializeObject(OT);
                            OTDetailsDTO OTDetailsDTO = JsonConvert.DeserializeObject<OTDetailsDTO>(rawOT);
                            //if (OT.JobID == 0)
                            //{
                            //    List<WorkerDTO> crews = new List<WorkerDTO>();
                            //    User u = new UserDataConnector().GetUsersByFirstVariable(OT.UserID);
                            //    crews.Add(new WorkerDTO
                            //    {
                            //        ID = u.ID,
                            //        Name = u.Name,
                            //        Email = u.Email,
                            //        Phone = u.Phone,
                            //        RoleID = u.RoleID,
                            //        Credits = u.Credits
                            //    });
                            //    OTDetailsDTO.Crews = crews;
                            //}
                            //else
                            //{
                            //    List<WorkerDTO> crews = new List<WorkerDTO>();
                            //    foreach (User u in new UserOTDataConnector().GetUsersByOT_ID(OTDetailsDTO.ID).ToList<User>())
                            //    {
                            //        crews.Add(new WorkerDTO
                            //        {
                            //            ID = u.ID,
                            //            Name = u.Name,
                            //            Email = u.Email,
                            //            Phone = u.Phone,
                            //            RoleID = u.RoleID,
                            //            Credits = u.Credits
                            //        });
                            //    }
                            //    OTDetailsDTO.Crews = crews;
                            //}
                            List<WorkerDTO> crews = new List<WorkerDTO>();
                            foreach (User u in new UserOTDataConnector().GetUsersByOT_ID(OTDetailsDTO.ID).ToList<User>())
                            {
                                crews.Add(new WorkerDTO
                                {
                                    ID = u.ID,
                                    Name = u.Name,
                                    Email = u.Email,
                                    Phone = u.Phone,
                                    RoleID = u.RoleID,
                                    Credits = u.Credits
                                });
                            }
                            OTDetailsDTO.Crews = crews;

                            OTDetailsDTOs.Add(OTDetailsDTO);
                        }


                        list = JsonConvert.DeserializeObject<List<OT>>(JsonConvert.SerializeObject(OTDetailsDTOs.Where(OT =>
                           OT.Crews.Where(crew => (
                               crew.Name.ToLower().Contains(search.ToLower())
                           )).Count() > 0
                        ).ToList()));
                    }


                    tbxSearch.Text = search;
                    ddlMonth.SelectedValue = month.ToString();
                    ddlYear.SelectedValue = year.ToString();
                    ddlSort.SelectedValue = sort.ToString();

                }
                catch (Exception)
                {
                }
            }
        }

        protected void deleteConfirm_Click(object sender, EventArgs e)
        {
            string id = Request.Form["deleteInput"];
            new OTDataConnector().UpdateOTSoftDelete(Convert.ToInt32(id));
            new UserOTDataConnector().UpdateSoftDeleteByOT_ID(Convert.ToInt32(id));

            // Log
            Utils.Log(new CreateLogDTO
            {
                ActionName = $@"Deletes an OT (OT_ID: {id})",
                ActionLocation = Page.AppRelativeVirtualPath,
                ActionType = LogActionTypes.SoftDelete.GetHashCode(),
                ActionMeta = new LogActionMeta
                {
                    Input = new { OT_ID = id }
                }
            }, Session);

            Response.Redirect(Routes.OT);
        }

        protected void lbtnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.GetRedirectUrl(Routes.OT, null, new
            {
                type = Request.QueryString["type"].NullIfWhiteSpace(),
                search = tbxSearch.Text.NullIfWhiteSpace(),
                month = ddlMonth.SelectedValue.NullIfWhiteSpace() == "-1" ? "" : ddlMonth.SelectedValue,
                year = ddlYear.SelectedValue.NullIfWhiteSpace() == "-1" ? "" : ddlYear.SelectedValue,
                sort = ddlSort.SelectedValue.NullIfWhiteSpace() == "-1" ? null : ddlSort.SelectedValue,
            }));
        }

        protected void ddlSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.GetRedirectUrl(Routes.OT, null, new
            {
                type = Request.QueryString["type"].NullIfWhiteSpace(),
                search = tbxSearch.Text.NullIfWhiteSpace(),
                month = ddlMonth.SelectedValue.NullIfWhiteSpace() == "-1" ? "" : ddlMonth.SelectedValue,
                year = ddlYear.SelectedValue.NullIfWhiteSpace() == "-1" ? "" : ddlYear.SelectedValue,
                sort = ddlSort.SelectedValue.NullIfWhiteSpace() == "-1" ? null : ddlSort.SelectedValue,
            }));
        }
    }
}