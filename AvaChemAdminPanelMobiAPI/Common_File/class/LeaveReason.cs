public class LeaveReason
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string CName { get; set; }
    public bool SoftDelete { get; set; }
}


public enum LeaveReasons
{
    Annual = 1,
    Medical
}
