public class Description
{
    public int ID { get; set; }
    public string Content { get; set; }
    public bool SoftDelete { get; set; }
}
public class AssignDescriptionDTO
{

    public int ID { get; set; }
    public int JobDescriptionID { get; set; }
    public int? ActionType { get; set; } // from enum DataActionTypes - Utils.cs
}