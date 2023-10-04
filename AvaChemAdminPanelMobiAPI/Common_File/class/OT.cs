using System;
using System.Collections.Generic;

public class OT
{
    public int ID { get; set; }
    public string OTNumber { get; set; }
    public int StatusID { get; set; }
    public DateTime WorkerStartedTime { get; set; }
    public DateTime WorkerEndedTime { get; set; }
    public DateTime DriverStartedTime { get; set; }
    public DateTime DriverEndedTime { get; set; }
    //public string JobName { get; set; }
    //public int JobID { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public bool SoftDelete { get; set; }
    public int UserID { get; set; }
    public string JobNumber { get; set; }
    public int DriverID { get; set; }
}

//public class OTTableView : OT
//{
//    public string JobNumber { get; set; }
//}

public class OT_DTO : OT
{
    public Status Status { get; set; }
    //public string JobNumber { get; set; }
    public DateTime? JobWorkingDate { get; set; } = null;
    //public User User { get; set; } = null;
}
public class OTDetailsDTO : OT_DTO
{
    public List<Vehicle> Vehicles { get; set; }
    public WorkerDTO Driver { get; set; }
    public List<WorkerDTO> Crews { get; set; }
}

public class ApplyOT_DTO
{
    //public int jobId { get; set; }
    //public string jobName { get; set; }
    public string jobNumber { get; set; }
    public string driverStartTime { get; set; }
    public string driverEndTime { get; set; }
    public string workerStartTime { get; set; }
    public string workerEndTime { get; set; }
    public int driverId { get; set; }
    public int vehicleId { get; set; }
}
public class ApproveOT_DTO
{
    public int OT_ID { get; set; }
    public int statusID { get; set; }
}

public class AddOTCrewsDTO
{
    public int OT_ID { get; set; }
    public int[] userIDs { get; set; }
}


public enum OTProgressTypes
{
    Pending = 1,
    Completed = 2,
    Approved = 3,
    Rejected = 4,
}