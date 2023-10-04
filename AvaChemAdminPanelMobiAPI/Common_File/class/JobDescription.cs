using System;
public class JobDescription
{
    public int ID { get; set; }
    public int JobID { get; set; }
    public int DescriptionID { get; set; }
    public bool SoftDelete { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}

public class JobDescriptionWithContent : JobDescription
{
    public string Content { get; set; }
}