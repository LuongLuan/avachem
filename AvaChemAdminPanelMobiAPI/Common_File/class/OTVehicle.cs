public class OTVehicle
{
    public int ID { get; set; }
    public int OT_ID { get; set; }
    public int VehicleID { get; set; }
    public bool SoftDelete { get; set; }
}
public class VehicleWithOTVehicleDTO : Vehicle
{
    public int OTVehicleID { get; set; }
}