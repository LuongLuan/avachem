using System;
using System.Collections.Generic;

public class Job
{
    public int ID { get; set; }
    //public string JobNumber { get; set; }
    public DateTime WorkingDate { get; set; }
    public string Name { get; set; }
    public string AdminRemarks { get; set; }
    public string Location { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public bool SoftDelete { get; set; }
    public int ClientID { get; set; }
    public string ReportName { get; set; }
    public string InvoiceNo { get; set; }


    //public DateTime StartTime { get; set; }
    //public DateTime EndTime { get; set; }

    //public DateTime? WorkerStartedTime { get; set; }
    //public DateTime? WorkerEndedTime { get; set; }
    //public string Remarks { get; set; }
    //public string CustomerSignatureImage { get; set; }

}

public class JobDTO : Job
{
    public string Description { get; set; }
    public int NumTrips { get; set; }
    //public Status Status { get; set; }
}
public class JobDetailsDTO : JobDTO
{
    public List<WorkerDTO> Workers { get; set; }
    public List<Vehicle> Vehicles { get; set; }
    public Client Client { get; set; }
    public List<TripDetailsDTO> Trips { get; set; }
    public bool IsCompleted { get; set; }
    //public List<JobImage> Images { get; set; }
}

public class JobLiteDTO
{
    public int ID { get; set; }
    //public string JobNumber { get; set; }
    public string Name { get; set; }
}

//public class SubmitJobDTO
//{
//    public int id { get; set; }
//    public string startTime { get; set; }
//    public string endTime { get; set; }
//    public string remarks { get; set; }
//    public List<int> deleteImageIds { get; set; }
//}


public enum JobProgressTypes
{
    Pending = 1,
    Completed = 2,
}

