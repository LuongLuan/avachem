using System;
public class Device
{
    public int ID { get; set; }
    public string FCMToken { get; set; }
    public string DevID { get; set; }
    public string Model { get; set; }
    public int NumFailedNotif { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public bool SoftDelete { get; set; }
    public int UserID { get; set; }
}

public class AddDeviceDTO
{
    public string fcmToken { get; set; }
    public string devID { get; set; }
    public string model { get; set; }
}