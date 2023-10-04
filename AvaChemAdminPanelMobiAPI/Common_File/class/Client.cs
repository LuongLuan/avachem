using System;
public class Client
{
    public int ID { get; set; }
    public string CompanyName { get; set; }
    public string ContactNamePrimary { get; set; }
    public string ContactDetailsPrimary { get; set; }
    public string ContactDetailsSecondary { get; set; }
    public string ContactNameSecondary { get; set; }
    public bool SoftDelete { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string Location { get; set; }
}