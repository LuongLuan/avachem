using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using AvaChemAdminPanelMobiAPI.Common_File;
using Newtonsoft.Json;

namespace AvaChemAdminPanelMobiAPI
{

    public partial class Calendar : System.Web.UI.Page
    {
        public string list = "[]";
        public int role;

        protected void Page_Load(object sender, EventArgs e)
        {
            role = (Convert.ToInt32(Session["role"]));
            if (Session["admin"] == null
                || !(new int[] { UserRoles.SuperAdmin.GetHashCode(), UserRoles.OverallAdmin.GetHashCode(), UserRoles.HR.GetHashCode() }).Contains(role))
            {
                Response.Redirect(Routes.LOG_IN);
            }

            string requestData = JsonConvert.SerializeObject(new GetDataDTO { });
            var rawJobs = Calendar.GetJobs(requestData);
            var rawLeaves = Calendar.GetLeaves(requestData);
            //var rawOTs = Calendar.GetLeaves(requestData);

            List<FullCalendarEventDTO> listEvt = new List<FullCalendarEventDTO>();
            listEvt.AddRange(JsonConvert.DeserializeObject<List<FullCalendarEventDTO>>(rawJobs));
            listEvt.AddRange(JsonConvert.DeserializeObject<List<FullCalendarEventDTO>>(rawLeaves));
            //listEvt.AddRange(JsonConvert.DeserializeObject<List<FullCalendarEventDTO>>(rawOTs));
            list = JsonConvert.SerializeObject(listEvt);

        }

        [WebMethod]
        public static string GetJobs(string data)
        {
            try
            {
                if (data == null) throw new Exception();

                var requestData = JsonConvert.DeserializeObject<GetDataDTO>(data);


                List<FullCalendarEventDTO> events = new List<FullCalendarEventDTO>();
                foreach (Job job in new JobDataConnector().GetAll())
                {
                    List<VehicleWithJobVehicleDTO> vehicles = new JobVehicleDataConnector().GetVehiclesByParams(job.ID).ToList<VehicleWithJobVehicleDTO>();
                    List<UserWithUserJobDTO> workers = new UserJobDataConnector().GetUsersByJobID(job.ID).ToList<UserWithUserJobDTO>();

                    bool isCompleted = new JobDataConnector().CheckIsCompleted(job.ID);
                    string workingDate = job.WorkingDate.ToString("yyyy-MM-dd");
                    foreach (Trip trip in new TripDataConnector().GetByJobID(job.ID))
                    {
                        var evt = new FullCalendarEventDTO
                        {
                            id = job.ID,
                            groupId = job.ID,
                            start = $"{workingDate}T{((DateTime)trip.StartTime).ToString("HH:mm")}",
                            end = $"{workingDate}T{((DateTime)trip.EndTime).ToString("HH:mm")}",

                            url = $"{Routes.JOB_DETAILS}?id={job.ID}",

                            //title = $"#{job.JobNumber} {job.Name}",
                            title = $"{job.Name} - #{trip.JobNumber}",
                            color = isCompleted == true ? "#3598DC" : "#F59F02",
                            backgroundColor = isCompleted == true ? "#3598DC" : "#F59F02",
                            borderColor = isCompleted == true ? "#3598DC" : "#F59F02",
                            textColor = "#ffffff",
                            editable = false,
                            allDay = false,

                            extendedProps = new
                            {
                                jobId = job.ID,
                                //job = $"#{job.JobNumber} {job.Name}",
                                job = $"{job.Name} - #{trip.JobNumber}",
                                vehicles = string.Join(", ", vehicles.Select(v => $"{v.Number} ({v.Model})")),
                                workers = string.Join(", ", workers.Select(w => w.Name)),
                                isCompleted,
                            }
                        };
                        events.Add(evt);
                    }

                }

                return JsonConvert.SerializeObject(events);
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong");
            }
        }

        [WebMethod]
        public static string GetLeaves(string data)
        {
            try
            {
                if (data == null) throw new Exception();

                var requestData = JsonConvert.DeserializeObject<GetDataDTO>(data);
                List<FullCalendarEventDTO> events = new List<FullCalendarEventDTO>();

                List<LeaveTableView> leaves = new LeaveDataConnector().GetAll<LeaveTableView>(null, null, null, null, requestData.from, requestData.to, true);
                foreach (LeaveTableView leave in leaves)
                {
                    bool isCompleted = leave.StatusID != 0 && leave.StatusID != Statuses.Pending.GetHashCode();
                    events.Add(new FullCalendarEventDTO
                    {
                        id = leave.ID,
                        groupId = leave.UserID,
                        start = leave.StartedDate.ToString("yyyy-MM-dd"),
                        end = leave.EndedDate.AddDays(1).ToString("yyyy-MM-dd"),

                        url = $"{Routes.UPDATE_LEAVE}?id={leave.ID}",

                        title = $"{leave.UName} - {(LeaveReasons)leave.ReasonID}",
                        color = isCompleted == true ? "#ff9f89" : "#18cdc4",
                        backgroundColor = isCompleted == true ? "#ff9f89" : "#18cdc4",
                        borderColor = isCompleted == true ? "#ff9f89" : "#18cdc4",
                        textColor = "#ffffff",
                        editable = false,
                        allDay = true,
                    });
                }

                return JsonConvert.SerializeObject(events);
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong");
            }
        }

        [WebMethod]
        public static string GetOTs(string data)
        {
            try
            {
                if (data == null) throw new Exception();

                var requestData = JsonConvert.DeserializeObject<GetDataDTO>(data);
                int? month = null;
                if (requestData.to != null)
                {
                    month = int.Parse(Convert.ToDateTime(requestData.to).ToString("MM"));
                }
                int? year = null;
                if (requestData.to != null)
                {
                    year = int.Parse(Convert.ToDateTime(requestData.to).ToString("yyyy"));
                }
                List<FullCalendarEventDTO> events = new List<FullCalendarEventDTO>();

                foreach (User user in new UserDataConnector().GetAll())
                {
                    List<OT_DTO> OTs = new UserOTDataConnector().GetOTsByParams(user.ID, null, null, null, month, year).ToList<OT_DTO>();
                    foreach (OT_DTO OT in OTs)
                    {
                        bool isCompleted = OT.StatusID != 0 && OT.StatusID != Statuses.Pending.GetHashCode();
                        var evt = new FullCalendarEventDTO
                        {
                            id = OT.ID,
                            groupId = OT.ID,
                            start = $"{OT.DriverStartedTime.ToString("yyyy-MM-ddTHH:mm")}",
                            end = $"{OT.DriverEndedTime.ToString("yyyy-MM-ddTHH:mm")}",

                            title = $"{user.Name} - #{OT.JobNumber}",
                            color = isCompleted == true ? "#6BAC34" : "#f7cb73",
                            backgroundColor = isCompleted == true ? "#6BAC34" : "#f7cb73",
                            borderColor = isCompleted == true ? "#6BAC34" : "#f7cb73",
                            textColor = "#ffffff",
                            editable = false,
                            allDay = false,
                        };
                        events.Add(evt);
                    }
                }


                return JsonConvert.SerializeObject(events);
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong");
            }
        }


        public class GetDataDTO
        {
            public string from { get; set; }
            public string to { get; set; }

        }

        public class FullCalendarEventDTO
        {
            public dynamic id { get; set; } // string or int. Will uniquely identify your event. Useful for getEventById (https://fullcalendar.io/docs/Calendar-getEventById)
            public dynamic groupId { get; set; } // string or int. Events that share a groupId will be dragged and resized together automatically

            /**
             * Determines if the event is shown in the “all-day” section of the view, if applicable. 
             * Determines if time text is displayed in the event. 
             * If this value is not specified, it will be inferred by the start and end properties
             */
            public bool allDay { get; set; }
            public string start { get; set; } // yyyy-MM-dd, When your event begins. If your event is explicitly allDay, hour, minutes, seconds and milliseconds will be ignored.
            public string end { get; set; } // yyyy-MM-dd
            //public List<int> daysOfWeek { get; set; } // The days of the week this event repeats. An array of integers representing days e.g. [0, 1] for an event that repeats on Sundays and Mondays.
            //public string startTime { get; set; } // hh:mm:ss
            //public string endTime { get; set; } // hh:mm:ss
            //public string startRecur { get; set; } // yyyy-MM-dd
            //public string endRecur { get; set; } // yyyy-MM-dd
            public string title { get; set; } // The text that will appear on an event.
            public string url { get; set; } // A URL that will be visited when this event is clicked by the user. For more information on controlling this behavior, see the eventClick callback.
            public bool interactive { get; set; } // Whether or not the event is tabbable. Defaults to true if url is present, false otherwise. See eventInteractive for more info.
            //public List<string> classNames { get; set; } // Determines which HTML classNames will be attached to the rendered event.
            public bool editable { get; set; } // Overrides the master editable option for this single event.
            //public bool startEditable { get; set; } // Overrides the master eventStartEditable option for this single event.
            //public bool durationEditable { get; set; } // Overrides the master eventDurationEditable option for this single event.
            //public bool resourceEditable { get; set; } // Overrides the master eventResourceEditable option for this single event. Requires one of the resource plugins.
            //public string resourceId { get; set; } // The string ID of a Resource. See Associating Events with Resources. Requires one of the resource plugins.
            //public List<string> resourceIds { get; set; } // An array of string IDs of Resources. See Associating Events with Resources. Requires one of the resource plugins.
            //public string display { get; set; } // Allows alternate rendering of the event, like background events. Can be 'auto' (the default), 'block', 'list-item', 'background', 'inverse-background', or 'none'. See eventDisplay.
            //public bool overlap { get; set; } // Overrides the master eventOverlap option for this single event. If false, prevents this event from being dragged/resized over other events. Also prevents other events from being dragged/resized over this event.
            //public dynamic constraint { get; set; }
            public string color { get; set; } // An alias for specifying the backgroundColor and borderColor at the same time.
            public string backgroundColor { get; set; } // Sets an event’s background color just like the calendar-wide eventBackgroundColor option.
            public string borderColor { get; set; } // Sets an event’s border color just like the calendar-wide eventBorderColor option.
            public string textColor { get; set; } // Sets an event’s text color just like the calendar-wide eventTextColor option.
            //public dynamic rrule { get; set; }
            public object extendedProps { get; set; } // A plain object with any miscellaneous properties. It will be directly transferred to the extendedProps hash in each Event Object. Often, these props are useful in event render hooks.
        }
    }

}