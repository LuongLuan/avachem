public class UserRole
{
    public int ID { get; set; }
    public string RoleName { get; set; }
    public bool SoftDelete { get; set; }
    public string CRoleName { get; set; }
}


public enum UserRoles
{
    SuperAdmin = 1,
    OverallAdmin,
    CreditAdmin,
    HR,
    Driver,
    Worker,
    OTAdmin,
}