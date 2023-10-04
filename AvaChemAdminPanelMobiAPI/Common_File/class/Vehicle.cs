public class Vehicle
{
    public int ID { get; set; }
    public string Model { get; set; }
    public string Number { get; set; }
    public bool SoftDelete { get; set; }
    // public int UserID { get; set; }
}

public class VehicleWithJobVehicleDTO : Vehicle
{
    public int JobVehicleID { get; set; }
}

public class AssignVehicleDTO
{
    public int ID { get; set; }
    public string Model { get; set; }
    public string Number { get; set; }

    public int JobVehicleID { get; set; }
    public int? ActionType { get; set; } // from enum DataActionTypes - Utils.cs
}