using System;
using System.Collections.Generic;

public class Trip
{
    public int ID { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime? WorkerStartedTime { get; set; }
    public DateTime? WorkerEndedTime { get; set; }
    public string Remarks { get; set; }
    public string CustomerSignatureImage { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public bool SoftDelete { get; set; }
    public int JobID { get; set; }
    public string JobNumber { get; set; }
}

public class TripDetailsDTO : Trip
{
    public List<JobImage> BeforeImages { get; set; }
    public List<JobImage> AfterImages { get; set; }
    public int Index { get; set; }
    //public string JobName { get; set; }
}

public class AssignTripDTO
{
    public int ID { get; set; }
    public string JobNumber { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string WorkerStartedTime { get; set; }
    public string WorkerEndedTime { get; set; }
    public string Remarks { get; set; }
    public List<int> DeleteImgs { get; set; }
    public int Index { get; set; }
    public int? ActionType { get; set; } // from enum DataActionTypes - Utils.cs
}

public class SubmitTripDTO
{
    public int id { get; set; }
    public string jobNumber { get; set; }
    public string startTime { get; set; }
    public string endTime { get; set; }
    public string remarks { get; set; }
    public List<int> deleteImageIds { get; set; }
}