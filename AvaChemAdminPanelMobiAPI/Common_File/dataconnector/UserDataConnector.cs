using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

public class UserDataConnector
{


    public UserDataConnector()
    {
    }
    public User CreateUser(User AvaChemUsers)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "INSERT INTO [dbo].[AvaChem_User]" +
                "([Username],[Password],[Name],[Email],[Phone],[DOB],[Credits],[LeaveDaysLeft],[MCDaysLeft],[CreatedDate],[UpdatedDate],[UserStatus],[RoleID],[SoftDelete],[IDNumber])" +
                "Values(@Username,@Password,@Name,@Email,@Phone,@DOB,@Credits,@LeaveDaysLeft,@MCDaysLeft,@CreatedDate,@UpdatedDate,@UserStatus,@RoleID,@SoftDelete,@IDNumber)";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@Username", AvaChemUsers.Username ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Password", AvaChemUsers.Password ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Name", AvaChemUsers.Name ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Email", AvaChemUsers.Email ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Phone", AvaChemUsers.Phone ?? DBNull.Value.ToString());
            //sqlCommand.Parameters.AddWithValue("@DOB", AvaChemUsers.DOB.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@DOB", DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Credits", AvaChemUsers.Credits.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@LeaveDaysLeft", AvaChemUsers.LeaveDaysLeft);
            sqlCommand.Parameters.AddWithValue("@MCDaysLeft", AvaChemUsers.MCDaysLeft);
            sqlCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@UserStatus", AvaChemUsers.UserStatus == 0 ? 0 : AvaChemUsers.UserStatus);
            sqlCommand.Parameters.AddWithValue("@RoleID", AvaChemUsers.RoleID == 0 ? 0 : AvaChemUsers.RoleID);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemUsers.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@IDNumber", AvaChemUsers.IDNumber ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return GetUsersByLastInsertedID();
        }
    }
    public User GetAdminByUsernameAndPassword(string username, string password)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT [ID],[Username],[Password],[Name],[Gender],[Email],[Phone],[DOB],[ProfilePicture],[Credits],[LeaveDaysLeft],[MCDaysLeft],[CreatedDate],[UpdatedDate],[UserStatus],[RoleID],[SoftDelete],[IDNumber] FROM [dbo].[AvaChem_User] " +
                "WHERE [Username]=@username AND [Password]=@password " +
                "AND [RoleID] IN (@Admin,@OverallAdmin,@CreditAdmin,@HR,@OTAdmin)";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@username", username);
            sqlCommand.Parameters.AddWithValue("@password", password);
            sqlCommand.Parameters.AddWithValue("@Admin", UserRoles.SuperAdmin.GetHashCode());
            sqlCommand.Parameters.AddWithValue("@OverallAdmin", UserRoles.OverallAdmin.GetHashCode());
            sqlCommand.Parameters.AddWithValue("@CreditAdmin", UserRoles.CreditAdmin.GetHashCode());
            sqlCommand.Parameters.AddWithValue("@HR", UserRoles.HR.GetHashCode());
            sqlCommand.Parameters.AddWithValue("@OTAdmin", UserRoles.OTAdmin.GetHashCode());
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new User()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Username = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    Password = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    Name = sdr[3].ToString() ?? DBNull.Value.ToString(),
                    //Gender = Convert.ToInt32(sdr[4].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[4].ToString()),
                    Email = sdr[5].ToString() ?? DBNull.Value.ToString(),
                    Phone = sdr[6].ToString() ?? DBNull.Value.ToString(),
                    //DOB = sdr[7].ToString() + "" != "" ? Convert.ToDateTime(sdr[7].ToString() ?? DBNull.Value.ToString()) : DateTime.Now,
                    //ProfilePicture = sdr[8].ToString() ?? DBNull.Value.ToString(),
                    Credits = Convert.ToDouble(sdr[9].ToString() ?? DBNull.Value.ToString()),
                    LeaveDaysLeft = float.Parse(sdr[10].ToString()),
                    MCDaysLeft = float.Parse(sdr[11].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[12].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[13].ToString() ?? DBNull.Value.ToString()),
                    UserStatus = Convert.ToInt32(sdr[14].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[14].ToString()),
                    RoleID = Convert.ToInt32(sdr[15].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[15].ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[16].ToString() ?? DBNull.Value.ToString()),
                    IDNumber = sdr[17].ToString() ?? DBNull.Value.ToString()
                };
            }
            return null;
        }
    }
    public User GetUserByUsernameAndPassword(string username, string password)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };

            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT [ID],[Username],[Email],[Password],[RoleID],[SoftDelete] FROM [dbo].[AvaChem_User] " +
                                        "WHERE [Username]=@username AND [Password]=@password AND [SoftDelete]=0 " +
                                        "AND [RoleID] IN (@Driver,@Worker,@OTAdmin,@CreditAdmin)";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@username", username);
            sqlCommand.Parameters.AddWithValue("@password", password);
            sqlCommand.Parameters.AddWithValue("@Driver", UserRoles.Driver.GetHashCode());
            sqlCommand.Parameters.AddWithValue("@Worker", UserRoles.Worker.GetHashCode());
            sqlCommand.Parameters.AddWithValue("@OTAdmin", UserRoles.OTAdmin.GetHashCode());
            sqlCommand.Parameters.AddWithValue("@CreditAdmin", UserRoles.CreditAdmin.GetHashCode());
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new User()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Username = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    Email = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    Password = sdr[3].ToString() ?? DBNull.Value.ToString(),
                    RoleID = Convert.ToInt32(sdr[4].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[4].ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[5].ToString() ?? DBNull.Value.ToString())

                };
            }
            return null;
        }
    }
    public User GetUsersByFirstVariable(int ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[Username],[Password],[Name],[Gender],[Email],[Phone],[DOB],[ProfilePicture],[Credits],[LeaveDaysLeft],[MCDaysLeft],[CreatedDate],[UpdatedDate],[UserStatus],[RoleID],[SoftDelete],[IDNumber] FROM [dbo].[AvaChem_User] WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new User()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Username = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    Password = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    Name = sdr[3].ToString() ?? DBNull.Value.ToString(),
                    //Gender = Convert.ToInt32(sdr[4].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[4].ToString()),
                    Email = sdr[5].ToString() ?? DBNull.Value.ToString(),
                    Phone = sdr[6].ToString() ?? DBNull.Value.ToString(),
                    //DOB = Convert.ToDateTime(sdr[7].ToString() ?? DBNull.Value.ToString()),
                    //ProfilePicture = sdr[8].ToString() ?? DBNull.Value.ToString(),
                    Credits = Convert.ToDouble(sdr[9].ToString() ?? DBNull.Value.ToString()),
                    LeaveDaysLeft = float.Parse(sdr[10].ToString()),
                    MCDaysLeft = float.Parse(sdr[11].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[12].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[13].ToString() ?? DBNull.Value.ToString()),
                    UserStatus = Convert.ToInt32(sdr[14].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[14].ToString()),
                    RoleID = Convert.ToInt32(sdr[15].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[15].ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[16].ToString() ?? DBNull.Value.ToString()),
                    IDNumber = sdr[17].ToString() ?? DBNull.Value.ToString()
                };
            }
            return null;
        }
    }
    public User GetUsersByLastInsertedID()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[Username],[Password],[Name],[Gender],[Email],[Phone],[DOB],[ProfilePicture],[Credits],[LeaveDaysLeft],[MCDaysLeft],[CreatedDate],[UpdatedDate],[UserStatus],[RoleID],[SoftDelete],[IDNumber] FROM [dbo].[AvaChem_User] WHERE [ID]=(SELECT MAX(ID) FROM [AvaChem_User]) AND [SoftDelete]=0";
            sqlCommand.Parameters.Clear();
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new User()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Username = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    Password = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    Name = sdr[3].ToString() ?? DBNull.Value.ToString(),
                    //Gender = Convert.ToInt32(sdr[4].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[4].ToString()),
                    Email = sdr[5].ToString() ?? DBNull.Value.ToString(),
                    Phone = sdr[6].ToString() ?? DBNull.Value.ToString(),
                    //DOB = Convert.ToDateTime(sdr[7].ToString() ?? DBNull.Value.ToString()),
                    //ProfilePicture = sdr[8].ToString() ?? DBNull.Value.ToString(),
                    Credits = Convert.ToDouble(sdr[9].ToString() ?? DBNull.Value.ToString()),
                    LeaveDaysLeft = float.Parse(sdr[10].ToString()),
                    MCDaysLeft = float.Parse(sdr[11].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[12].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[13].ToString() ?? DBNull.Value.ToString()),
                    UserStatus = Convert.ToInt32(sdr[14].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[14].ToString()),
                    RoleID = Convert.ToInt32(sdr[15].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[15].ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[16].ToString() ?? DBNull.Value.ToString()),
                    IDNumber = sdr[17].ToString() ?? DBNull.Value.ToString(),
                };
            }
            return null;
        }
    }
    public User GetByEmail(string email)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT [ID],[Username],[Password],[Name],[Gender],[Email],[Phone],[DOB],[ProfilePicture],[Credits],[LeaveDaysLeft],[MCDaysLeft],[CreatedDate],[UpdatedDate],[UserStatus],[RoleID],[SoftDelete],[IDNumber] FROM [dbo].[AvaChem_User] WHERE [SoftDelete]=0 AND [Email]=@Email";
            sqlCommand.Parameters.AddWithValue("@Email", email);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new User()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Username = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    Password = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    Name = sdr[3].ToString() ?? DBNull.Value.ToString(),
                    //Gender = Convert.ToInt32(sdr[4].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[4].ToString()),
                    Email = sdr[5].ToString() ?? DBNull.Value.ToString(),
                    Phone = sdr[6].ToString() ?? DBNull.Value.ToString(),
                    //DOB = Convert.ToDateTime(sdr[7].ToString() ?? DBNull.Value.ToString()),
                    //ProfilePicture = sdr[8].ToString() ?? DBNull.Value.ToString(),
                    Credits = Convert.ToDouble(sdr[9].ToString() ?? DBNull.Value.ToString()),
                    LeaveDaysLeft = float.Parse(sdr[10].ToString()),
                    MCDaysLeft = float.Parse(sdr[11].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[12].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[13].ToString() ?? DBNull.Value.ToString()),
                    UserStatus = Convert.ToInt32(sdr[14].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[14].ToString()),
                    RoleID = Convert.ToInt32(sdr[15].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[15].ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[16].ToString() ?? DBNull.Value.ToString()),
                    IDNumber = sdr[17].ToString() ?? DBNull.Value.ToString()
                };
            }
            return null;
        }
    }
    public List<User> GetAll(int? page = null, int? per_page = null, string search = "", List<int> roles = null)
    {
        List<User> finalListToReturn = new List<User>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT [ID],[Username],[Password],[Name],[Gender],[Email],[Phone],[DOB],[ProfilePicture],[Credits],[LeaveDaysLeft],[MCDaysLeft],[CreatedDate],[UpdatedDate],[UserStatus],[RoleID],[SoftDelete],[IDNumber] " +
                                        "FROM [dbo].[AvaChem_User] WHERE [SoftDelete]=0 " +
                                        (roles == null ? $"AND [RoleID] NOT IN ({UserRoles.SuperAdmin.GetHashCode()}) " : $"AND [RoleID] IN ({string.Join(",", roles)}) ") +
                                        (search + "" == "" ? ""
                                            : "AND ( [IDNumber] Like @Search OR [Name] Like @Search OR [Email] Like @Search OR [Phone] Like @Search ) ") +
                                        ((page + "" == "" || per_page + "" == "") ? "ORDER BY [ID] DESC" : "ORDER BY [ID] DESC OFFSET (@Offset) ROWS FETCH NEXT @PerPage ROWS ONLY");
            sqlCommand.Parameters.Clear();
            if (search + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@Search", $"%{search}%");
            }
            if (page + "" != "" && per_page + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@Offset", (page - 1) * per_page);
                sqlCommand.Parameters.AddWithValue("@PerPage", per_page);
            }
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                User dl = new User()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Username = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    Password = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    Name = sdr[3].ToString() ?? DBNull.Value.ToString(),
                    //Gender = Convert.ToInt32(sdr[4].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[4].ToString()),
                    Email = sdr[5].ToString() ?? DBNull.Value.ToString(),
                    Phone = sdr[6].ToString() ?? DBNull.Value.ToString(),
                    //DOB = Convert.ToDateTime(sdr[7].ToString() ?? DBNull.Value.ToString()),
                    //ProfilePicture = sdr[8].ToString() ?? DBNull.Value.ToString(),
                    Credits = Convert.ToDouble(sdr[9].ToString() ?? DBNull.Value.ToString()),
                    LeaveDaysLeft = float.Parse(sdr[10].ToString()),
                    MCDaysLeft = float.Parse(sdr[11].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[12].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[13].ToString() ?? DBNull.Value.ToString()),
                    UserStatus = Convert.ToInt32(sdr[14].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[14].ToString()),
                    RoleID = Convert.ToInt32(sdr[15].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[15].ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[16].ToString() ?? DBNull.Value.ToString()),
                    IDNumber = sdr[17].ToString() ?? DBNull.Value.ToString(),
                };
                finalListToReturn.Add(dl);
            }
            return finalListToReturn;
        }
    }
    public ICollection<User> GetAvailableWorkers(int currentJobID = 0, string[] workingDates = null, string start = "", string end = "", bool onlyDrivers = false)
    {
        if (workingDates == null || start == "" || end == "")
        {
            return GetAll(null, null, null, new List<int> { UserRoles.Driver.GetHashCode(), UserRoles.Worker.GetHashCode() });
        }

        List<User> finalListToReturn = new List<User>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };

            string queryWorkingDates = "";
            if (workingDates != null)
            {
                queryWorkingDates = string.Join(",", workingDates.Select(d => $"'{Convert.ToDateTime(d).ToString("yyyy-MM-dd")}'"));
            }
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT [ID],[Username],[Password],[Name],[Gender], " +
                                    "[Email],[Phone],[DOB],[ProfilePicture],[Credits], " +
                                    "[LeaveDaysLeft],[MCDaysLeft],[CreatedDate],[UpdatedDate], " +
                                    "[UserStatus],[RoleID],[SoftDelete],[IDNumber],[IsValid] " +
                                    "FROM ( " +
                                    "SELECT u.[ID],u.[Username],u.[Password],u.[Name],u.[Gender],u.[Email],u.[Phone], " +
                                    "u.[DOB],u.[ProfilePicture],u.[Credits],u.[LeaveDaysLeft],u.[MCDaysLeft], " +
                                    "u.[CreatedDate],u.[UpdatedDate],u.[UserStatus],u.[RoleID],u.[SoftDelete],u.[IDNumber], " +
                                    "STRING_AGG( CASE " +
                                    "WHEN (j.[WorkingDate] IS NULL " +
                                    "OR ( j.[ID]=@JobID " +
                                    //"OR ( cast(j.[WorkingDate] AS date)!=@WorkingDate " +
                                    //"OR ( cast(j.[WorkingDate] AS date)=@WorkingDate " +
                                    $"OR ( cast(j.[WorkingDate] AS date) NOT IN ({queryWorkingDates}) " +
                                    $"OR ( cast(j.[WorkingDate] AS date) IN ({queryWorkingDates}) " +
                                    "AND " +
                                    "( @Start " +
                                    "NOT BETWEEN REPLACE(REPLACE(Convert(varchar(5),t.[StartTime],108), '12:', '24:'), '00:', '12:') " +
                                    "and REPLACE(REPLACE(Convert(varchar(5),t.[EndTime],108), '12:', '24:'), '00:', '12:') ) " +
                                    "AND " +
                                    "( @End " +
                                    "NOT BETWEEN REPLACE(REPLACE(Convert(varchar(5),t.[StartTime],108), '12:', '24:'), '00:', '12:') " +
                                    "and REPLACE(REPLACE(Convert(varchar(5),t.[EndTime],108), '12:', '24:'), '00:', '12:') ) " +
                                    "AND ( REPLACE(REPLACE(Convert(varchar(5),t.[StartTime],108), '12:', '24:'), '00:', '12:') " +
                                    "NOT BETWEEN @Start " +
                                    "and @End ) " +
                                    "AND ( REPLACE(REPLACE(Convert(varchar(5),t.[EndTime],108), '12:', '24:'), '00:', '12:') " +
                                    "NOT BETWEEN @Start " +
                                    "and @End ) ) ) ) )" +
                                    "THEN 1 ELSE 0 END, ',') AS [IsValid] " +
                                    "FROM [dbo].[AvaChem_User] AS u " +
                                    "LEFT JOIN [AvaChem_UserJob] AS uj ON uj.[UserID]=u.[ID] " +
                                    "LEFT JOIN [AvaChem_Job] AS j ON uj.[JobID]=j.[ID] " +
                                    "LEFT JOIN [AvaChem_Trip] AS t ON j.[ID]=t.[JobID] " +
                                    "WHERE u.[SoftDelete]=0 " +
                                    (onlyDrivers ? "AND u.[RoleID] IN (@Driver) " : "AND u.[RoleID] IN (@Driver,@Worker) ") +
                                    "AND ( j.[ID] IS NULL " +
                                    "OR ( j.[ID] IS NOT NULL " +
                                    "AND uj.[SoftDelete]=0 " +
                                    "AND j.[SoftDelete]=0 " +
                                    "AND ( t.[ID] IS NULL OR ( t.[ID] IS NOT NULL AND t.[SoftDelete]=0 ) ) ) )" +
                                    "GROUP BY u.[ID],u.[Username],u.[Password],u.[Name],u.[Gender],u.[Email],u.[Phone],u.[DOB],u.[ProfilePicture],u.[Credits],u.[LeaveDaysLeft],u.[MCDaysLeft],u.[CreatedDate],u.[UpdatedDate],u.[UserStatus],u.[RoleID],u.[SoftDelete],u.[IDNumber] ) AS R " +
                                    "WHERE R.[IsValid] NOT LIKE '%0%' " +
                                    "GROUP BY [ID],[Username],[Password],[Name],[Gender],[Email],[Phone],[DOB],[ProfilePicture],[Credits],[LeaveDaysLeft],[MCDaysLeft],[CreatedDate],[UpdatedDate],[UserStatus],[RoleID],[SoftDelete],[IDNumber],[IsValid]";
            //sqlCommand.CommandText =
            //    "SELECT [ID],[Username],[Password],[Name],[Gender],[Email],[Phone],[DOB],[ProfilePicture],[Credits],[LeaveDaysLeft],[MCDaysLeft],[CreatedDate],[UpdatedDate],[UserStatus],[RoleID],[SoftDelete],[IDNumber],[IsValid] FROM " +
            //     "( SELECT u.[ID],u.[Username],u.[Password],u.[Name],u.[Gender],u.[Email],u.[Phone],u.[DOB],u.[ProfilePicture],u.[Credits],u.[LeaveDaysLeft],u.[MCDaysLeft],u.[CreatedDate],u.[UpdatedDate],u.[UserStatus],u.[RoleID],u.[SoftDelete],u.[IDNumber]" +
            //            ",STRING_AGG(CASE WHEN (j.[WorkingDate] IS NULL " +
            //                                    "OR ( j.[ID]=@JobID OR ( cast(j.[WorkingDate] AS date)!=@WorkingDate " +
            //                                        "OR ( cast(j.[WorkingDate] AS date)=@WorkingDate AND " +
            //                                                "(@Start NOT BETWEEN " +
            //                                                        "REPLACE(REPLACE(Convert(varchar(5),t.[StartTime],108), '12:', '24:'), '00:', '12:') and REPLACE(REPLACE(Convert(varchar(5),t.[EndTime],108), '12:', '24:'), '00:', '12:') ) " +
            //                                                "AND (@End NOT BETWEEN " +
            //                                                        "REPLACE(REPLACE(Convert(varchar(5),t.[StartTime],108), '12:', '24:'), '00:', '12:') and REPLACE(REPLACE(Convert(varchar(5),t.[EndTime],108), '12:', '24:'), '00:', '12:') ) )" +
            //                                                "AND (REPLACE(REPLACE(Convert(varchar(5), t.[StartTime], 108), '12:', '24:'), '00:', '12:') NOT BETWEEN @Start and @End)" +
            //                                                "AND (REPLACE(REPLACE(Convert(varchar(5), t.[EndTime], 108), '12:', '24:'), '00:', '12:')) NOT BETWEEN '14:33' and '22:38') ) ) " +
            //                         "THEN 1 ELSE 0 END, ',') AS [IsValid] " +
            //        "FROM [dbo].[AvaChem_User] AS u " +
            //        "LEFT JOIN [AvaChem_UserJob] AS uj ON uj.[UserID]=u.[ID] " +
            //        "LEFT JOIN [AvaChem_Job] AS j ON uj.[JobID]=j.[ID] " +
            //        "LEFT JOIN [AvaChem_Trip] AS t ON j.[ID]=t.[JobID]" +
            //        "WHERE u.[SoftDelete]=0 AND ( j.[ID] IS NULL " +
            //                                        "OR ( j.[ID] IS NOT NULL " +
            //                                                "AND uj.[SoftDelete]=0 AND j.[SoftDelete]=0 AND ( t.[ID] IS NULL OR ( t.[ID] IS NOT NULL AND t.[SoftDelete]=0 ) ) ) ) " +
            //        "GROUP BY u.[ID],u.[Username],u.[Password],u.[Name],u.[Gender],u.[Email],u.[Phone],u.[DOB],u.[ProfilePicture],u.[Credits],u.[LeaveDaysLeft],u.[MCDaysLeft],u.[CreatedDate],u.[UpdatedDate],u.[UserStatus],u.[RoleID],u.[SoftDelete],u.[IDNumber] ) AS R " +
            //    "WHERE R.[IsValid] NOT LIKE '%0%' " +
            //    "GROUP BY [ID],[Username],[Password],[Name],[Gender],[Email],[Phone],[DOB],[ProfilePicture],[Credits],[LeaveDaysLeft],[MCDaysLeft],[CreatedDate],[UpdatedDate],[UserStatus],[RoleID],[SoftDelete],[IDNumber],[IsValid]";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@Driver", UserRoles.Driver.GetHashCode());
            if (!onlyDrivers) sqlCommand.Parameters.AddWithValue("@Worker", UserRoles.Worker.GetHashCode());
            sqlCommand.Parameters.AddWithValue("@JobID", currentJobID);
            //sqlCommand.Parameters.AddWithValue("@WorkingDate", Convert.ToDateTime(workingDate).ToString("yyyy-MM-dd"));
            sqlCommand.Parameters.AddWithValue("@Start", start.Contains("01:") ? "01:00" : Convert.ToDateTime(start).AddHours(-1).ToString("HH:mm").Replace("12:", "24:").Replace("00:", "12:"));
            sqlCommand.Parameters.AddWithValue("@End", end.Contains("23:") ? "23:59" : Convert.ToDateTime(end).AddHours(1).ToString("HH:mm").Replace("12:", "24:").Replace("00:", "12:"));
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                User dl = new User()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Username = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    Password = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    Name = sdr[3].ToString() ?? DBNull.Value.ToString(),
                    //Gender = Convert.ToInt32(sdr[4].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[4].ToString()),
                    Email = sdr[5].ToString() ?? DBNull.Value.ToString(),
                    Phone = sdr[6].ToString() ?? DBNull.Value.ToString(),
                    //DOB = Convert.ToDateTime(sdr[7].ToString() ?? DBNull.Value.ToString()),
                    //ProfilePicture = sdr[8].ToString() ?? DBNull.Value.ToString(),
                    Credits = Convert.ToDouble(sdr[9].ToString() ?? DBNull.Value.ToString()),
                    LeaveDaysLeft = float.Parse(sdr[10].ToString()),
                    MCDaysLeft = float.Parse(sdr[11].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[12].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[13].ToString() ?? DBNull.Value.ToString()),
                    UserStatus = Convert.ToInt32(sdr[14].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[14].ToString()),
                    RoleID = Convert.ToInt32(sdr[15].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[15].ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[16].ToString() ?? DBNull.Value.ToString()),
                    IDNumber = sdr[17].ToString() ?? DBNull.Value.ToString(),
                };
                finalListToReturn.Add(dl);
            }
            return finalListToReturn;
        }
    }
    public int CountAll()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT COUNT(*) FROM [dbo].[AvaChem_User] WHERE [SoftDelete]=0 AND [RoleID] NOT IN (@Admin)";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@Admin", UserRoles.SuperAdmin.GetHashCode());
            int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
            return count;
        }
    }
    public User UpdatePasswored(int ID, string password)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_User] SET [Password]=@Password,[UpdatedDate]=@UpdatedDate WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            sqlCommand.Parameters.AddWithValue("@Password", password);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
        return null;
    }
    public User UpdateUser(User AvaChemUser)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_User] " +
                "SET [Username]=@Username,[Password]=@Password,[Name]=@Name,[Email]=@Email,[Phone]=@Phone,[DOB]=@DOB,[Credits]=@Credits,[LeaveDaysLeft]=@LeaveDaysLeft,[MCDaysLeft]=@MCDaysLeft,[UpdatedDate]=@UpdatedDate,[UserStatus]=@UserStatus,[RoleID]=@RoleID,[SoftDelete]=@SoftDelete,[IDNumber]=@IDNumber WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", AvaChemUser.ID == 0 ? 0 : AvaChemUser.ID);
            sqlCommand.Parameters.AddWithValue("@Username", AvaChemUser.Username ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Password", AvaChemUser.Password ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Name", AvaChemUser.Name ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Email", AvaChemUser.Email ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Phone", AvaChemUser.Phone ?? DBNull.Value.ToString());
            //sqlCommand.Parameters.AddWithValue("@DOB", AvaChemUser.DOB.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@DOB", DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Credits", AvaChemUser.Credits.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@LeaveDaysLeft", AvaChemUser.LeaveDaysLeft);
            sqlCommand.Parameters.AddWithValue("@MCDaysLeft", AvaChemUser.MCDaysLeft);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@UserStatus", AvaChemUser.UserStatus == 0 ? 0 : AvaChemUser.UserStatus);
            sqlCommand.Parameters.AddWithValue("@RoleID", AvaChemUser.RoleID == 0 ? 0 : AvaChemUser.RoleID);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemUser.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@IDNumber", AvaChemUser.IDNumber ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return AvaChemUser;
        }
    }
    public User UpdateUserPartial(User AvaChemUser)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_User] " +
                "SET [Username]=@Username,[Password]=@Password,[Name]=@Name,[Email]=@Email,[Phone]=@Phone,[UpdatedDate]=@UpdatedDate WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", AvaChemUser.ID == 0 ? 0 : AvaChemUser.ID);
            sqlCommand.Parameters.AddWithValue("@Username", AvaChemUser.Username ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Password", AvaChemUser.Password ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Name", AvaChemUser.Name ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Email", AvaChemUser.Email ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Phone", AvaChemUser.Phone ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return GetUsersByFirstVariable(AvaChemUser.ID);
        }
    }
    public void UpdateCredits(int uid, double credits, UpdateCreditsType type)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_User] SET [UpdatedDate]=@UpdatedDate" +
                                        (type.ToString() == UpdateCreditsType.Overwrite.ToString()
                                                            ? ",[Credits]=@Credits"
                                                            : type.ToString() == UpdateCreditsType.Increase.ToString()
                                                            ? ",[Credits]+=@Credits"
                                                            : type.ToString() == UpdateCreditsType.Decrease.ToString()
                                                            ? ",[Credits]-=@Credits"
                                                            : "")
                                        + " WHERE [ID]=@ID";

            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@Credits", credits);
            sqlCommand.Parameters.AddWithValue("@ID", uid);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
    }

    private string GetLeaveDaysFieldName(string o, double? leaveDaysLeft = null, double? MCDaysLeft = null)
    {
        string fieldUpdate = leaveDaysLeft + "" == "" ? "" : $",[LeaveDaysLeft]{o}@LeaveDaysLeft";
        fieldUpdate += MCDaysLeft + "" == "" ? "" : $",[MCDaysLeft]{o}@MCDaysLeft";
        return fieldUpdate;
    }
    public User UpdateLeaveDays(int uid, UpdateLeaveDaysType type, float? leaveDaysLeft = null, float? MCDaysLeft = null)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_User] SET [UpdatedDate]=@UpdatedDate" +
                                        (type.ToString() == UpdateLeaveDaysType.Overwrite.ToString()
                                                            ? $"{GetLeaveDaysFieldName("", leaveDaysLeft, MCDaysLeft)}"
                                                            : type.ToString() == UpdateLeaveDaysType.Increase.ToString()
                                                            ? $"{GetLeaveDaysFieldName("+=", leaveDaysLeft, MCDaysLeft)}"
                                                            : type.ToString() == UpdateLeaveDaysType.Decrease.ToString()
                                                            ? $"{GetLeaveDaysFieldName("-=", leaveDaysLeft, MCDaysLeft)}"
                                                            : "")
                                        + " WHERE [ID]=@ID";

            sqlCommand.Parameters.Clear();
            if (leaveDaysLeft + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@LeaveDaysLeft", leaveDaysLeft);
            }
            if (MCDaysLeft + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@MCDaysLeft", MCDaysLeft);
            }
            sqlCommand.Parameters.AddWithValue("@ID", uid);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
        return null;
    }
    public User UpdateUserSoftDelete(int ID)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE[dbo].[AvaChem_User] SET [SoftDelete]=1,[UpdatedDate]=@UpdatedDate WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
        return null;
    }
    public bool DeletebyFirstVariable(int ID)
    {
        int count = 0;
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "DELETE FROM [dbo].[AvaChem_User] WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            if (count == 1)
            {
                return true;
            }
        }
        return false;
    }
    //public bool CheckEmailExists(string email)
    //{
    //    int count = 0;
    //    using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
    //    {
    //        SqlCommand sqlCommand = new SqlCommand()
    //        {
    //            Connection = sqlConnection
    //        };
    //        sqlConnection.Open();
    //        sqlCommand.CommandText = "SELECT Count([Email]) as CountEmail FROM [dbo].[AvaChem_User] WHERE [SoftDelete]=0 AND [Email]=@email"; sqlCommand.Parameters.Clear();
    //        sqlCommand.Parameters.AddWithValue("@email", email);
    //        SqlDataReader sdr = sqlCommand.ExecuteReader();
    //        while (sdr.Read())
    //        {
    //            count = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString());
    //        }
    //    }
    //    return count > 0;
    //}
    public bool CheckUsernameExists(string username)
    {
        int count = 0;
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT Count([Username]) as CountUsername FROM [dbo].[AvaChem_User] WHERE [SoftDelete]=0 AND [Username]=@Username"; sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@Username", username);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                count = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString());
            }
        }
        return count > 0;
    }

    public bool IsDriver(int ID)
    {
        int count = 0;
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT Count([ID]) as CountID FROM [dbo].[AvaChem_User] WHERE [SoftDelete]=0 AND [ID]=@ID AND [RoleID]=@Driver";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            sqlCommand.Parameters.AddWithValue("@Driver", UserRoles.Driver.GetHashCode());
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                count = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString());
            }
        }
        return count > 0;
    }
}

public enum UpdateCreditsType
{
    Increase = 1,
    Decrease = 2,
    Overwrite = 3
}
public enum UpdateLeaveDaysType
{
    Increase = 1,
    Decrease = 2,
    Overwrite = 3
}