using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using AvaChemAdminPanelMobiAPI.Common_File;
using Newtonsoft.Json;

namespace AvaChemAdminPanelMobiAPI
{
    /// <summary>
    /// Summary description for AjaxHandler
    /// </summary>
    [WebService]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class AjaxHandler : System.Web.Services.WebService
    {

        [WebMethod]
        public List<Vehicle> GetAvailableVehicles(string data)
        {
            try
            {
                if (data == null) throw new Exception();

                var requestData = JsonConvert.DeserializeObject<GetAvailableVehiclesDTO>(data);
                List<Vehicle> listVehicles = new VehicleDataConnector().GetAvailableVehicles(
                    requestData.jobID,
                    Array.ConvertAll(requestData.workingDates.Split(','), str => str.Trim()),
                    requestData.start,
                    requestData.end
                ).ToList<Vehicle>();

                return listVehicles;
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong");
            }
        }
        public class GetAvailableVehiclesDTO
        {
            public int jobID { get; set; }
            public string workingDates { get; set; }
            public string start { get; set; }
            public string end { get; set; }
        }

        [WebMethod]
        public List<Vehicle> GetVehicles(string data)
        {
            try
            {
                if (data == null) throw new Exception();

                var requestData = JsonConvert.DeserializeObject<GetVehiclesDTO>(data);
                List<Vehicle> listVehicles = new JobVehicleDataConnector().GetVehiclesByParams(requestData.jobID).ToList<Vehicle>();

                return listVehicles;
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong");
            }
        }
        public class GetVehiclesDTO
        {
            public int jobID { get; set; }
        }


        [WebMethod]
        public List<User> GetAvailableWorkers(string data)
        {
            try
            {
                if (data == null) throw new Exception();

                var requestData = JsonConvert.DeserializeObject<GetAvailableWorkersDTO>(data);
                List<User> listWorkers = new UserDataConnector().GetAvailableWorkers(
                    requestData.jobID,
                    Array.ConvertAll(requestData.workingDates.Split(','), str => str.Trim()),
                    requestData.start,
                    requestData.end
                ).ToList<User>();

                return listWorkers;
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong");
            }
        }
        public class GetAvailableWorkersDTO
        {
            public int jobID { get; set; }
            public string workingDates { get; set; }
            public string start { get; set; }
            public string end { get; set; }
        }



        [WebMethod]
        public List<UserWithUserOT_DTO> GetCrewsByOT_ID(string data)
        {
            try
            {
                if (data == null) throw new Exception();

                var requestData = JsonConvert.DeserializeObject<GetCrewsDTO>(data);
                List<UserWithUserOT_DTO> listCrews = new UserOTDataConnector().GetUsersByOT_ID(requestData.oid).ToList<UserWithUserOT_DTO>();

                return listCrews;
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong");
            }
        }
        public class GetCrewsDTO
        {
            public int oid { get; set; }
        }



        [WebMethod]
        public List<UserWithUserJobDTO> GetWorkersByJobID(string data)
        {
            try
            {
                if (data == null) throw new Exception();

                var requestData = JsonConvert.DeserializeObject<GetWorkersDTO>(data);
                List<UserWithUserJobDTO> listWorkers = new UserJobDataConnector().GetUsersByJobID(requestData.jobID).ToList<UserWithUserJobDTO>();

                return listWorkers;
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong");
            }
        }
        public class GetWorkersDTO
        {
            public int jobID { get; set; }
        }



        [WebMethod]
        public List<JobLiteDTO> GetJobsByDate(string data)
        {
            try
            {
                if (data == null) throw new Exception();

                var requestData = JsonConvert.DeserializeObject<GetJobsByDateDTO>(data);
                List<JobLiteDTO> jobs = new UserJobDataConnector().GetJobsByDate(requestData.uid, requestData.from, requestData.to);
                return jobs;
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong");
            }
        }
        public class GetJobsByDateDTO
        {
            public int uid { get; set; }
            public string from { get; set; }
            public string to { get; set; }

        }



        [WebMethod]
        public List<Leave> GetLeavesByDate(string data)
        {
            try
            {
                if (data == null) throw new Exception();

                var requestData = JsonConvert.DeserializeObject<GetLeaveByDateDTO>(data);
                List<Leave> leaves = new LeaveDataConnector().GetByParams<Leave>(Array.ConvertAll(requestData.workingDates.Split(','), str => str.Trim()), requestData.uid);

                return leaves;
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong");
            }
        }
        public class GetLeaveByDateDTO
        {
            public int? uid { get; set; }
            public string workingDates { get; set; }
            //public string to { get; set; }

        }



        [WebMethod]
        public object UpdateReportName(string data)
        {
            try
            {
                if (data == null) throw new Exception();

                var requestData = JsonConvert.DeserializeObject<UpdateReportNameDTO>(data);
                string reportName = new JobDataConnector().UpdateReportName(requestData.jobID, requestData.reportName);

                Utils.Log(new CreateLogDTO
                {
                    ActionName = $@"{(UserRoles)requestData.RoleID} ""{requestData.Name}"": Update report name for job (JobID: {requestData.jobID})",
                    ActionLocation = requestData.ActionLocation,
                    ActionType = LogActionTypes.Create.GetHashCode(),
                    ActionMeta = new LogActionMeta
                    {
                        Input = new { JobID = requestData.jobID, ReportName = requestData.reportName },
                        Result = new { ReportName = reportName }
                    },
                    ByUserID = requestData.ByUserID,
                });

                return new { reportName };
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong");
            }
        }
        public class UpdateReportNameDTO
        {
            public int jobID { get; set; }
            public string reportName { get; set; }


            public string ActionLocation { get; set; }
            public int ByUserID { get; set; }
            public int RoleID { get; set; }
            public string Name { get; set; }

        }


        [WebMethod]
        public string ExportOTasCSV(string data)
        {
            try
            {
                ExportOTasCSV_DTO requestData = new ExportOTasCSV_DTO();

                if (data + "" != "") requestData = JsonConvert.DeserializeObject<ExportOTasCSV_DTO>(data);

                List<OT> listOT = new OTDataConnector().GetAll(requestData.page, requestData.per_page, requestData.search, requestData.type, requestData.month, requestData.year, requestData.sort).ToList<OT>();

                string csv = string.Empty;

                string header = "Service Memo / Delivery Order Number,Status,Driver's Start Time,Driver's End Time,Client's Job Start Time,Client's Job End Time,Vehicle,Driver,OT Crews";
                csv += header;
                //Add new line.
                csv += "\r\n";

                string row = string.Empty;
                foreach (OT item in listOT)
                {
                    var listVehicles = new OTVehicleDataConnector().GetVehiclesByParams(item.ID).ToList<VehicleWithOTVehicleDTO>().Select(v =>
                    {
                        string vModel = v.Model != "" ? $" ({v.Model})" : "";
                        return $"{v.Number}{vModel}";
                    });
                    var listDriver = new List<string>
                    {
                        new UserDataConnector().GetUsersByFirstVariable(item.DriverID).Name
                    };
                    var listCrews = new UserOTDataConnector().GetUsersByOT_ID(item.ID).ToList<UserWithUserOT_DTO>().Select(c => c.Name);

                    //row += item.JobName + (item.JobNumber + "" != "" ? $" (#{item.JobNumber})" : "") + ",";
                    row += item.JobNumber + ",";
                    row += ((Statuses)item.StatusID).ToString() + ",";
                    row += item.DriverStartedTime.ToShortTimeString() + ",";
                    row += item.DriverEndedTime.ToShortTimeString() + ",";
                    row += item.WorkerStartedTime.ToShortTimeString() + ",";
                    row += item.WorkerEndedTime.ToShortTimeString() + ",";
                    row += $"{string.Join("; ", listVehicles)}" + ",";
                    row += $"{string.Join("; ", listDriver)}" + ",";
                    row += $"{string.Join("; ", listCrews)}";
                    //Add new line.
                    row += "\r\n";
                }
                csv += row;

                return csv;
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong");
            }
        }
        public class ExportOTasCSV_DTO
        {
            public int? page { get; set; } = null;
            public int? per_page { get; set; } = null;
            public string search { get; set; } = null;
            public int? type { get; set; } = null;
            public int? month { get; set; } = null;
            public int? year { get; set; } = null;
            public int? sort { get; set; } = null;
        }
    }
}
