public class JobImage
{
    public int ID { get; set; }
    public string ImageUrl { get; set; }
    public string Type { get; set; }
    public int TripID { get; set; }
    public bool SoftDelete { get; set; }
}


public class AssignJobImageDTO
{
    public int ID { get; set; }
    public string ImageUrl { get; set; }
    public string ImageName { get; set; } // HttpPostedFile
    public int? ActionType { get; set; } // from enum DataActionTypes - Utils.cs
}