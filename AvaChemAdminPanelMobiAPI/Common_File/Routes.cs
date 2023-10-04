using System.Collections.Generic;

namespace AvaChemAdminPanelMobiAPI.Common_File
{
    public static class Routes
    {
        // Auth
        public const string LOG_IN = "login";

        // Home and profile
        public const string DASHBOARD = "dashboard";
        public const string PROFILE = "profile";
        public const string CHANGE_PASSWORD = "change-password";

        // Create
        public const string CREATE_CLIENT = "create-client";
        public const string CREATE_CREDIT = "create-credit";
        public const string CREATE_JOB = "create-job";
        public const string CREATE_LEAVE = "create-leave";
        public const string CREATE_OT = "create-OT";
        public const string CREATE_USER = "create-user";
        public const string CREATE_VEHICLE = "create-vehicle";

        // Update
        public const string UPDATE_CLIENT = "update-client";
        public const string UPDATE_CREDIT = "update-credit";
        public const string UPDATE_JOB = "update-job";
        public const string UPDATE_LEAVE = "update-leave";
        public const string UPDATE_OT = "update-OT";
        public const string UPDATE_USER = "update-user";
        public const string UPDATE_VEHICLE = "update-vehicle";

        // Listing
        public const string CALENDAR = "calendar";
        public const string CLIENTS = "clients";
        public const string CREDITS = "credits";
        public const string JOBS = "jobs";
        public const string LEAVES = "leaves";
        public const string OT = "OT";
        public const string USERS = "users";
        public const string VEHICLES = "vehicles";


        // Details
        public const string USER_DETAILS = "user-details";
        public const string JOB_DETAILS = "job-details";
        public const string JOB_REPORT = "report";

        public static List<RouteSettings> CustomRoutes()
        {
            var routes = new List<RouteSettings>
            {
                new RouteSettings
                {
                    RouteName = "ChangePassword",
                    RouteUrl = Routes.CHANGE_PASSWORD,
                    PhysicalFile = "~/ChangePassword.aspx"
                },

                new RouteSettings
                {
                    RouteName = "Overtime",
                    RouteUrl = Routes.OT,
                    PhysicalFile = "~/Overtime.aspx"
                },

                new RouteSettings
                {
                    RouteName = "CreateClient",
                    RouteUrl= Routes.CREATE_CLIENT,
                    PhysicalFile= "~/CreateClient.aspx"
                },
                new RouteSettings
                {
                    RouteName= "CreateCredit",
                    RouteUrl= Routes.CREATE_CREDIT,
                    PhysicalFile= "~/CreateCredit.aspx"
                },
                new RouteSettings
                {
                    RouteName = "CreateJob",
                    RouteUrl = Routes.CREATE_JOB,
                    PhysicalFile = "~/CreateJob.aspx"
                },
                new RouteSettings
                {
                    RouteName = "CreateLeave",
                    RouteUrl = Routes.CREATE_LEAVE,
                    PhysicalFile = "~/CreateLeave.aspx"
                },
                new RouteSettings
                {
                    RouteName = "CreateOT",
                    RouteUrl = Routes.CREATE_OT,
                    PhysicalFile = "~/CreateOT.aspx"
                },
                new RouteSettings
                {
                    RouteName = "CreateUser",
                    RouteUrl = Routes.CREATE_USER,
                    PhysicalFile = "~/CreateUser.aspx"
                },
                new RouteSettings
                {
                    RouteName = "CreateVehicle",
                    RouteUrl = Routes.CREATE_VEHICLE,
                    PhysicalFile = "~/CreateVehicle.aspx"
                },

                new RouteSettings
                {
                    RouteName = "UpdateClient",
                    RouteUrl = Routes.UPDATE_CLIENT,
                    PhysicalFile = "~/UpdateClient.aspx"
                },
                new RouteSettings
                {
                    RouteName = "UpdateCredit",
                    RouteUrl = Routes.UPDATE_CREDIT,
                    PhysicalFile = "~/UpdateCredit.aspx"
                },
                new RouteSettings
                {
                    RouteName = "UpdateJob",
                    RouteUrl = Routes.UPDATE_JOB,
                    PhysicalFile = "~/UpdateJob.aspx"
                },
                new RouteSettings
                {
                    RouteName = "UpdateLeave",
                    RouteUrl = Routes.UPDATE_LEAVE,
                    PhysicalFile = "~/UpdateLeave.aspx"
                },
                new RouteSettings
                {
                    RouteName = "UpdateOT",
                    RouteUrl = Routes.UPDATE_OT,
                    PhysicalFile = "~/UpdateOT.aspx"
                },
                new RouteSettings
                {
                    RouteName = "UpdateUser",
                    RouteUrl = Routes.UPDATE_USER,
                    PhysicalFile = "~/UpdateUser.aspx"
                },
                new RouteSettings
                {
                    RouteName = "UpdateVehicle",
                    RouteUrl = Routes.UPDATE_VEHICLE,
                    PhysicalFile = "~/UpdateVehicle.aspx"
                },

                new RouteSettings
                {
                    RouteName = "UserDetails",
                    RouteUrl = Routes.USER_DETAILS,
                    PhysicalFile = "~/UserDetails.aspx"
                },
                new RouteSettings
                {
                    RouteName = "JobDetails",
                    RouteUrl = Routes.JOB_DETAILS,
                    PhysicalFile = "~/JobDetails.aspx"
                }
            };

            return routes;
        }
    }
    public class RouteSettings
    {
        public string RouteName { get; set; }
        public string RouteUrl { get; set; }
        public string PhysicalFile { get; set; }
    }
}
