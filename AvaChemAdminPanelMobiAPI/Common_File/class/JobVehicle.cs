using System;
public class JobVehicle
{
    public int ID { get; set; }
    public int JobID { get; set; }
    public int VehicleID { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public bool SoftDelete { get; set; }
}