using System;
public class UserJob
{
    public int ID { get; set; }
    public int UserID { get; set; }
    public int JobID { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public bool SoftDelete { get; set; }
}