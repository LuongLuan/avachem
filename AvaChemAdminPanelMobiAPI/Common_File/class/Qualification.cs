using System;
public class Qualification
{
    public int ID { get; set; }
    public string Name { get; set; }
    public DateTime DateObtained { get; set; }
    public DateTime ExpiryDate { get; set; }
    public int UserID { get; set; }
    public bool SoftDelete { get; set; }
}
public class QualificationWithStatus : Qualification
{
    public int DaysLeft { get; set; }
}


public class AssignQualificationDTO
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string DateObtained { get; set; }
    public string ExpiryDate { get; set; }


    public int? ActionType { get; set; } // from enum DataActionTypes - Utils.cs
}

public class EditQualificationDTO
{
    public int id { get; set; }
    public string name { get; set; }
    public string dateObtained { get; set; }
    public string expiryDate { get; set; }
}

//public enum QualificationStatuses
//{
//    Available = 1,
//    Expired,
//    Near
//}