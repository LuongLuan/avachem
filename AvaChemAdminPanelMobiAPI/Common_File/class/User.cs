using System;
public class User
{
    public int ID { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    //public int Gender { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    //public DateTime DOB { get; set; }
    //public string ProfilePicture { get; set; }
    public double Credits { get; set; }
    public float LeaveDaysLeft { get; set; }
    public float MCDaysLeft { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public int UserStatus { get; set; }
    public int RoleID { get; set; }
    public bool SoftDelete { get; set; }
    public string IDNumber { get; set; }
    public string CheckLogin { get; set; }
}

public class UserDetailsDTO : User
{
    public UserRole Role { get; set; }
}

public class UserWithUserJobDTO : User
{
    public int UserJobID { get; set; }
}

public class UserWithJobDTO : User
{
    public int JobID { get; set; }
}

public class UserWithUserOT_DTO : User
{
    public int UserOT_ID { get; set; }
}

public class WorkerDTO
{
    public int ID { get; set; }
    public string IDNumber { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public int RoleID { get; set; }
    public double Credits { get; set; }
}

public class AssignWorkerDTO : WorkerDTO
{
    public int UserJobID { get; set; }

    public int? ActionType { get; set; } // from enum DataActionTypes - Utils.cs
}

public class AssignCrewDTO : WorkerDTO
{
    public int UserOT_ID { get; set; }

    public int? ActionType { get; set; } // from enum DataActionTypes - Utils.cs
}

public class ForgetPasswordDTO
{
    public string email { get; set; }
}

public class UpdateProfileDTO
{
    public string name { get; set; }
    public string email { get; set; }
    public string phone { get; set; }
    public string username { get; set; }
    public string password { get; set; }
}


public class RegisterDTO
{
    public string name { get; set; }
    public string email { get; set; }
    public string username { get; set; }
    public string password { get; set; }
}