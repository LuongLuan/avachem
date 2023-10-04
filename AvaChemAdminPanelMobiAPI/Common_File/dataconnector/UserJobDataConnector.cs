using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
public class UserJobDataConnector
{
    public UserJobDataConnector()
    {
    }
    public UserJob CreateUserJob(UserJob AvaChemUserJob)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "INSERT INTO [dbo].[AvaChem_UserJob]([UserID],[JobID],[CreatedDate],[UpdatedDate],[SoftDelete])Values(@UserID,@JobID,@CreatedDate,@UpdatedDate,@SoftDelete)";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@UserID", AvaChemUserJob.UserID == 0 ? 0 : AvaChemUserJob.UserID);
            sqlCommand.Parameters.AddWithValue("@JobID", AvaChemUserJob.JobID == 0 ? 0 : AvaChemUserJob.JobID);
            sqlCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemUserJob.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return GetUserJobByLastInsertedID();
        }
    }
    public UserJob GetUserJobByFirstVariable(int ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[UserID],[JobID],[CreatedDate],[UpdatedDate],[SoftDelete] FROM [dbo].[AvaChem_UserJob] WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new UserJob()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    UserID = Convert.ToInt32(sdr[1].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[1].ToString()),
                    JobID = Convert.ToInt32(sdr[2].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[2].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[3].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[4].ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[5].ToString() ?? DBNull.Value.ToString())
                };
            }
            return null;
        }
    }
    public UserJob GetUserJobByLastInsertedID()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT * FROM [dbo].[AvaChem_UserJob] WHERE [ID]=(SELECT MAX(ID) FROM [AvaChem_UserJob]) AND [SoftDelete]=0";
            sqlCommand.Parameters.Clear();
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new UserJob()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    UserID = Convert.ToInt32(sdr[1].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[1].ToString()),
                    JobID = Convert.ToInt32(sdr[2].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[2].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[3].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[4].ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[5].ToString() ?? DBNull.Value.ToString())
                };
            }
            return null;
        }
    }
    public ICollection<UserJob> GetAll()
    {
        List<UserJob> finalListToReturn = new List<UserJob>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[UserID],[JobID],[CreatedDate],[UpdatedDate],[SoftDelete]FROM [dbo].[AvaChem_UserJob]";
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                UserJob dl = new UserJob()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    UserID = Convert.ToInt32(sdr[1].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[1].ToString()),
                    JobID = Convert.ToInt32(sdr[2].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[2].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[3].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[4].ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[5].ToString() ?? DBNull.Value.ToString())
                };
                finalListToReturn.Add(dl);
            }
            return finalListToReturn;
        }
    }
    public ICollection<Job> GetJobsByParams(int uid, int? page = null, int? per_page = null,
                                                string search = "", int? type = null, string from = "", string to = "")
    {
        List<Job> finalListToReturn = new List<Job>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT j.[ID],j.[WorkingDate],j.[JobNumber],j.[Name],j.[AdminRemarks],c.[Location]," +
                                        "j.[CreatedDate],j.[UpdatedDate],j.[SoftDelete],j.[ClientID],j.[ReportName],j.[InvoiceNo] " +
                                        "FROM [AvaChem_UserJob] AS uJ LEFT JOIN [AvaChem_Job] AS j ON uJ.[JobID]=j.[ID] " +
                                        "LEFT JOIN [dbo].[AvaChem_Trip] AS t ON j.[ID] = t.[JobID] " +
                                        "LEFT JOIN [dbo].[AvaChem_Client] AS c ON j.[ClientID] = c.[ID] " +
                                        "WHERE j.[SoftDelete]=0 AND uJ.[SoftDelete]=0 AND uJ.[UserID]=@UserID " +
                                        (search + "" == "" ? ""
                                                           : "AND ( j.[JobNumber] Like @Search OR j.[Name] Like @Search OR c.[Location] Like @Search ) ") +
                                        (type + "" == "" ? ""
                                                         : type == JobProgressTypes.Pending.GetHashCode()
                                                         ? "AND ( t.[ID] IS NULL OR " +
                                                         "( t.[ID] IS NOT NULL AND t.[SoftDelete]=0" +
                                                         "AND EXISTS ( SELECT 1 FROM [dbo].[AvaChem_Trip] " +
                                                         "WHERE [JobID] = j.[ID] AND " +
                                                         "( ( [CustomerSignatureImage] IS NULL OR [CustomerSignatureImage]='' ) OR [WorkerStartedTime] IS NULL OR [WorkerEndedTime] IS NULL ) AND [SoftDelete]=0 ) ) )"
                                                         : type == JobProgressTypes.Completed.GetHashCode()
                                                         ? "AND t.[ID] IS NOT NULL AND t.[SoftDelete]=0" +
                                                         "AND NOT EXISTS ( SELECT 1 FROM [dbo].[AvaChem_Trip] " +
                                                         "WHERE [JobID] = j.[ID] AND " +
                                                         "( ( [CustomerSignatureImage] IS NULL OR [CustomerSignatureImage]='' ) OR [WorkerStartedTime] IS NULL OR [WorkerEndedTime] IS NULL ) AND [SoftDelete]=0 ) "
                                                         : "") +
                                        ((from + "" == "" && to + "" == "") ? ""
                                                         : "AND j.[WorkingDate] BETWEEN @BetweenStart " +
                                                                        "and @BetweenEnd ") +
                                        "GROUP BY j.[ID],j.[WorkingDate],j.[JobNumber],j.[Name],j.[AdminRemarks],c.[Location],j.[CreatedDate],j.[UpdatedDate],j.[SoftDelete],j.[ClientID],j.[ReportName],j.[InvoiceNo] " +
                                        ((page + "" == "" || per_page + "" == "") ? "ORDER BY j.[ID] DESC" : "ORDER BY j.[ID] DESC OFFSET (@Offset) ROWS FETCH NEXT @PerPage ROWS ONLY");
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@UserID", uid);
            if (search + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@Search", $"%{search}%");
            }
            if (from + "" != "" || to + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@BetweenStart", from + "" != "" ? Convert.ToDateTime(from).ToString("yyyy-MM-dd") : Convert.ToDateTime(to).AddDays(-1).ToString("yyyy-MM-dd"));
                sqlCommand.Parameters.AddWithValue("@BetweenEnd", to + "" != "" ? Convert.ToDateTime(to).ToString("yyyy-MM-dd") : Convert.ToDateTime(from).AddDays(1).ToString("yyyy-MM-dd"));
            }
            if (page + "" != "" && per_page + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@Offset", (page - 1) * per_page);
                sqlCommand.Parameters.AddWithValue("@PerPage", per_page);
            }
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                Job dl = new Job()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    WorkingDate = Convert.ToDateTime(sdr[1].ToString() ?? DBNull.Value.ToString()),
                    //JobNumber = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    Name = sdr[3].ToString() ?? DBNull.Value.ToString(),
                    AdminRemarks = sdr[4].ToString() ?? DBNull.Value.ToString(),
                    Location = sdr[5].ToString() ?? DBNull.Value.ToString(),
                    CreatedDate = Convert.ToDateTime(sdr[6].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[7].ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[8].ToString() ?? DBNull.Value.ToString()),
                    ClientID = Convert.ToInt32(sdr[9].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[9].ToString()),
                    ReportName = sdr[10].ToString() ?? DBNull.Value.ToString(),
                    InvoiceNo = sdr[11].ToString() ?? DBNull.Value.ToString(),
                };
                finalListToReturn.Add(dl);
            }
            return finalListToReturn;
        }
    }
    public ICollection<UserWithUserJobDTO> GetUsersByJobID(int jobID)
    {
        List<UserWithUserJobDTO> finalListToReturn = new List<UserWithUserJobDTO>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT u.[ID],u.[Username],u.[Password],u.[Name],u.[Gender],u.[Email],u.[Phone],u.[DOB],u.[ProfilePicture],u.[Credits],u.[LeaveDaysLeft],u.[MCDaysLeft],u.[CreatedDate],u.[UpdatedDate],u.[UserStatus],u.[RoleID],u.[SoftDelete],u.[IDNumber],uJ.[ID] AS uJ_ID " +
                                        "FROM [AvaChem_UserJob] AS uJ LEFT JOIN [AvaChem_User] AS u ON uJ.[UserID]=u.[ID] " +
                                        "WHERE u.[SoftDelete]=0 AND uJ.[SoftDelete]=0 AND u.[RoleID] IN (@Driver,@Worker) AND uJ.[JobID]=@JobID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@Driver", UserRoles.Driver.GetHashCode());
            sqlCommand.Parameters.AddWithValue("@Worker", UserRoles.Worker.GetHashCode());
            sqlCommand.Parameters.AddWithValue("@JobID", jobID);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                UserWithUserJobDTO dl = new UserWithUserJobDTO()
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
                    UserJobID = Convert.ToInt32(sdr[18].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[18].ToString()),
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
            sqlCommand.CommandText = "SELECT COUNT(*) FROM [dbo].[AvaChem_UserJob]";
            int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
            return count;
        }
    }
    public UserJob UpdateUserJob(UserJob AvaChemUserJob)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_UserJob]SET[UserID]=@UserID,[JobID]=@JobID,[UpdatedDate]=@UpdatedDate,[SoftDelete]=@SoftDelete WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", AvaChemUserJob.ID == 0 ? 0 : AvaChemUserJob.ID);
            sqlCommand.Parameters.AddWithValue("@UserID", AvaChemUserJob.UserID == 0 ? 0 : AvaChemUserJob.UserID);
            sqlCommand.Parameters.AddWithValue("@JobID", AvaChemUserJob.JobID == 0 ? 0 : AvaChemUserJob.JobID);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemUserJob.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return AvaChemUserJob;
        }
    }
    public void UpdateUserJobSoftDelete(int ID)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE[dbo].[AvaChem_UserJob] SET [SoftDelete]=1,[UpdatedDate]=@UpdatedDate WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

        }
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
            sqlCommand.CommandText = "DELETE FROM [dbo].[AvaChem_UserJob] WHERE [ID]=@ID";
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

    public bool CheckHasThisUser(int jobID, int uid)
    {
        int count = 0;
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT COUNT(uJ.[UserID]) as CountUser FROM [AvaChem_UserJob] AS uJ " +
                                        "WHERE uJ.[SoftDelete]=0 AND uJ.[JobID]=@JobID AND uj.[UserID]=@UserID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@JobID", jobID);
            sqlCommand.Parameters.AddWithValue("@UserID", uid);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                count = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString());
            }
        }
        return count > 0;
    }
    public JobLiteDTO GetJobByThisUser(int uid, int? jobID = null)
    {
        if (jobID + "" == "" || jobID == 0) return null;
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT j.[ID],j.[WorkingDate],j.[JobNumber],j.[Name] " +
                                        "FROM [AvaChem_UserJob] AS uJ LEFT JOIN [AvaChem_Job] AS j ON uJ.[JobID]=j.[ID] " +
                                        "WHERE uJ.[SoftDelete]=0 AND uJ.[JobID]=@JobID AND uj.[UserID]=@UserID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@JobID", jobID);
            sqlCommand.Parameters.AddWithValue("@UserID", uid);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new JobLiteDTO()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    // WorkingDate = Convert.ToDateTime(sdr[1].ToString() ?? DBNull.Value.ToString()),
                    //JobNumber = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    Name = sdr[3].ToString() ?? DBNull.Value.ToString()
                };
            }
            return null;
        }
    }
    public List<JobLiteDTO> GetJobsByDate(int uid, string from = "", string to = "")
    {
        List<JobLiteDTO> finalListToReturn = new List<JobLiteDTO>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT j.[ID],j.[WorkingDate],j.[JobNumber],j.[Name] " +
                                        "FROM [AvaChem_UserJob] AS uJ LEFT JOIN [AvaChem_Job] AS j ON uJ.[JobID]=j.[ID] " +
                                        "WHERE j.[SoftDelete]=0 AND uJ.[SoftDelete]=0 AND uJ.[UserID]=@UserID " +
                                        ((from + "" == "" && to + "" == "") ? ""
                                                         : "AND j.[WorkingDate] BETWEEN @BetweenStart " +
                                                                        "and @BetweenEnd");

            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@UserID", uid);
            if (from + "" != "" || to + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@BetweenStart", from + "" != "" ? Convert.ToDateTime(from).ToString("yyyy-MM-dd") : Convert.ToDateTime(to).AddDays(-1).ToString("yyyy-MM-dd"));
                sqlCommand.Parameters.AddWithValue("@BetweenEnd", to + "" != "" ? Convert.ToDateTime(to).ToString("yyyy-MM-dd") : Convert.ToDateTime(from).AddDays(1).ToString("yyyy-MM-dd"));
            }
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                JobLiteDTO dl = new JobLiteDTO()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    // WorkingDate = Convert.ToDateTime(sdr[1].ToString() ?? DBNull.Value.ToString()),
                    //JobNumber = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    Name = sdr[3].ToString() ?? DBNull.Value.ToString()
                };
                finalListToReturn.Add(dl);
            }
            return finalListToReturn;
        }
    }
}