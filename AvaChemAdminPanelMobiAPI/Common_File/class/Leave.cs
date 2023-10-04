using System;
public class Leave
{
    public int ID { get; set; }
    public DateTime StartedDate { get; set; }
    public DateTime EndedDate { get; set; }
    public float NumDays { get; set; }
    public string Remarks { get; set; }
    public string ProofImage { get; set; }
    public int ReasonID { get; set; }
    public int StatusID { get; set; }
    public int UserID { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public bool SoftDelete { get; set; }
}

public class LeaveTableView : Leave
{
    public string UName { get; set; }
    public string UIDNumber { get; set; }
    public double ULeaveDaysLeft { get; set; }
    public double UMCDaysLeft { get; set; }
}

public class LeaveDTO : Leave
{
    public LeaveReason Reason { get; set; }
    public Status Status { get; set; }
}

public class AddLeaveDTO
{
    public string startDate { get; set; }
    public string endDate { get; set; }
    public float numDays { get; set; }
    public int reasonId { get; set; }
    public string remarks { get; set; } = null;
}


public enum LeaveProgressTypes
{
    Upcoming = 1,
    Past,
    Pending,
    Completed,
}