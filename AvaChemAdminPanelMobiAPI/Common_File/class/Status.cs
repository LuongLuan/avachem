public class Status
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string CName { get; set; }
    public bool SoftDelete { get; set; }
    public string Color { get; set; }
}


public enum Statuses
{
    Pending = 1,
    Approved,
    Rejected
}