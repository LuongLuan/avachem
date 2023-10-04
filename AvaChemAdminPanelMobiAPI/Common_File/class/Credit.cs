using System;
using System.Collections.Generic;

public class Credit
{
    public int ID { get; set; }
    public double Amount { get; set; }
    public string Description { get; set; }
    public int UserID { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public bool SoftDelete { get; set; }
}


public class CreditTableView : Credit
{
    public string UName { get; set; }
    public string UIDNumber { get; set; }
    public double UCredits { get; set; }
}

public class CreditLogsAndCount<T> where T : Credit
{
    public List<T> Credits { get; set; }
    public int Count { get; set; }
}

public class UpdateCreditsDTO
{
    public int userId { get; set; }
    public int amount { get; set; }
    public string description { get; set; }
}