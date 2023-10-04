using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
public class UserOTDataConnector
{
    public UserOTDataConnector()
    {
    }
    public object CreateUserOT(UserOT AvaChemUserOT)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "INSERT INTO [dbo].[AvaChem_UserOT]([UserID],[OT_ID],[CreatedDate],[UpdatedDate],[SoftDelete])Values(@UserID,@OT_ID,@CreatedDate,@UpdatedDate,@SoftDelete)";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", AvaChemUserOT.ID == 0 ? 0 : AvaChemUserOT.ID);
            sqlCommand.Parameters.AddWithValue("@UserID", AvaChemUserOT.UserID == 0 ? 0 : AvaChemUserOT.UserID);
            sqlCommand.Parameters.AddWithValue("@OT_ID", AvaChemUserOT.OT_ID == 0 ? 0 : AvaChemUserOT.OT_ID);
            sqlCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemUserOT.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return GetUserOTByLastInsertedID();
        }
    }
    public UserOT GetUserOTByFirstVariable(int ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[UserID],[OT_ID],[CreatedDate],[UpdatedDate],[SoftDelete] FROM [dbo].[AvaChem_UserOT] WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new UserOT()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    UserID = Convert.ToInt32(sdr[1].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[1].ToString()),
                    OT_ID = Convert.ToInt32(sdr[2].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[2].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[3].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[4].ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[5].ToString() ?? DBNull.Value.ToString())
                };
            }
            return null;
        }
    }
    public UserOT GetUserOTByLastInsertedID()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT * FROM [dbo].[AvaChem_UserOT] WHERE [ID]=(SELECT MAX(ID) FROM [AvaChem_UserOT]) AND [SoftDelete]=0";
            sqlCommand.Parameters.Clear();
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new UserOT()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    UserID = Convert.ToInt32(sdr[1].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[1].ToString()),
                    OT_ID = Convert.ToInt32(sdr[2].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[2].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[3].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[4].ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[5].ToString() ?? DBNull.Value.ToString())
                };
            }
            return null;
        }
    }
    public ICollection<UserOT> GetAll()
    {
        List<UserOT> finalListToReturn = new List<UserOT>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[UserID],[OT_ID],[CreatedDate],[UpdatedDate],[SoftDelete]FROM [dbo].[AvaChem_UserOT]";
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                UserOT dl = new UserOT()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    UserID = Convert.ToInt32(sdr[1].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[1].ToString()),
                    OT_ID = Convert.ToInt32(sdr[2].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[2].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[3].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[4].ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[5].ToString() ?? DBNull.Value.ToString())
                };
                finalListToReturn.Add(dl);
            }
            return finalListToReturn;
        }
    }
    public ICollection<OT_DTO> GetOTsByParams(int? uid = null, int? page = null, int? per_page = null,
                                        int? type = null, int? month = null, int? year = null, string search = "")
    {
        List<OT_DTO> finalListToReturn = new List<OT_DTO>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT OT.[ID],OT.[OTNumber],OT.[StatusID],OT.[WorkerStartedTime],OT.[WorkerEndedTime],OT.[DriverStartedTime],OT.[DriverEndedTime],OT.[JobName],OT.[JobID],OT.[CreatedDate],OT.[UpdatedDate],OT.[SoftDelete],OT.[UserID],OT.[JobNumber],OT.[DriverID]" +
                                        ",s.[ID],s.[Name],s.[CName],s.[SoftDelete],s.[Color]" +
                                        ",j.[WorkingDate],j.[Name] AS jName" +
                                        //(uid + "" != "0" && uid + "" != "" ? "" : ",u.[ID],u.[Username],u.[Password],u.[Name],u.[Gender],u.[Email],u.[Phone],u.[DOB],u.[ProfilePicture],u.[Credits],u.[LeaveDaysLeft],u.[MCDaysLeft],u.[CreatedDate],u.[UpdatedDate],u.[UserStatus],u.[RoleID],u.[SoftDelete],u.[IDNumber]") +
                                        " FROM [AvaChem_UserOT] AS uOT " +
                                        "LEFT JOIN [AvaChem_OT] AS OT ON uOT.[OT_ID]=OT.[ID] " +
                                        "LEFT JOIN [AvaChem_Status] AS s ON OT.[StatusID]=s.[ID] " +
                                        //"LEFT JOIN [AvaChem_Job] AS j ON OT.[JobID]=j.[ID] " +

                                        "LEFT JOIN [AvaChem_Trip] AS t ON OT.[JobNumber]=t.[JobNumber] " +
                                        "LEFT JOIN [AvaChem_Job] AS j ON t.[JobID]=j.[ID] " +
                                        (search + "" == "" ? ""
                                                           : "LEFT JOIN [AvaChem_User] AS u ON uOT.[UserID]=u.[ID] ") +
                                        //(uid + "" != "0" && uid + "" != "" ? "" : "LEFT JOIN [AvaChem_User] AS u ON uOT.[UserID]=u.[ID] ") +
                                        "WHERE uOT.[SoftDelete]=0 AND OT.[SoftDelete]=0 " +
                                        (uid + "" != "0" && uid + "" != "" ? "AND uOT.[UserID]=@UserID " : "") +
                                        (type + "" == "" ? ""
                                                    : type == OTProgressTypes.Pending.GetHashCode()
                                                    ? "AND OT.[StatusID]=@PendingStatusID "
                                                    : type == OTProgressTypes.Approved.GetHashCode()
                                                    ? "AND OT.[StatusID]=@ApprovedStatusID "
                                                    : type == OTProgressTypes.Rejected.GetHashCode()
                                                    ? "AND OT.[StatusID]=@RejectedStatusID "
                                                    : type == OTProgressTypes.Completed.GetHashCode()
                                                    ? "AND OT.[StatusID] IN (@ApprovedStatusID,@RejectedStatusID) "
                                                    : "") +
                                        ((month + "" == "0" || month + "" == "") ? "" : "AND MONTH(OT.[CreatedDate])=@Month ") +
                                        ((year + "" == "0" || year + "" == "") ? "" : "AND YEAR(OT.[CreatedDate])=@Year ") +
                                        (search + "" == "" ? ""
                                                           : "AND ( u.[Name] Like @Search OR u.[Email] Like @Search ) ") +
                                        "GROUP BY OT.[ID],OT.[OTNumber],OT.[StatusID],OT.[WorkerStartedTime],OT.[WorkerEndedTime],OT.[DriverStartedTime],OT.[DriverEndedTime],OT.[JobName],OT.[JobID],OT.[CreatedDate],OT.[UpdatedDate],OT.[SoftDelete],OT.[UserID],OT.[JobNumber],OT.[DriverID],s.[ID],s.[Name],s.[CName],s.[SoftDelete],s.[Color],j.[WorkingDate],j.[Name] " +
                                        ((page + "" == "" || per_page + "" == "") ? "ORDER BY OT.[ID] DESC" : "ORDER BY OT.[ID] DESC OFFSET (@Offset) ROWS FETCH NEXT @PerPage ROWS ONLY");
            sqlCommand.Parameters.Clear();
            if (uid + "" != "0" && uid + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@UserID", uid);
            }

            if (type + "" != "" && Enum.IsDefined(typeof(OTProgressTypes), type))
            {
                if (type == OTProgressTypes.Pending.GetHashCode())
                {
                    sqlCommand.Parameters.AddWithValue("@PendingStatusID", Statuses.Pending.GetHashCode());
                }
                else if (type == OTProgressTypes.Approved.GetHashCode())
                {
                    sqlCommand.Parameters.AddWithValue("@ApprovedStatusID", Statuses.Approved.GetHashCode());
                }
                else if (type == OTProgressTypes.Rejected.GetHashCode())
                {
                    sqlCommand.Parameters.AddWithValue("@RejectedStatusID", Statuses.Rejected.GetHashCode());
                }
                else if (type == OTProgressTypes.Completed.GetHashCode())
                {
                    sqlCommand.Parameters.AddWithValue("@ApprovedStatusID", Statuses.Approved.GetHashCode());
                    sqlCommand.Parameters.AddWithValue("@RejectedStatusID", Statuses.Rejected.GetHashCode());
                }
            }
            if (month + "" != "0" && month + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@Month", month);
            }
            if (year + "" != "0" && year + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@Year", year);
            }
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
                OT_DTO dl = new OT_DTO()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    OTNumber = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    StatusID = Convert.ToInt32(sdr[2].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[2].ToString()),
                    WorkerStartedTime = Convert.ToDateTime(sdr[3].ToString() ?? DBNull.Value.ToString()),
                    WorkerEndedTime = Convert.ToDateTime(sdr[4].ToString() ?? DBNull.Value.ToString()),
                    DriverStartedTime = Convert.ToDateTime(sdr[5].ToString() ?? DBNull.Value.ToString()),
                    DriverEndedTime = Convert.ToDateTime(sdr[6].ToString() ?? DBNull.Value.ToString()),
                    //JobName = sdr[7].ToString() ?? DBNull.Value.ToString(),
                    CreatedDate = Convert.ToDateTime(sdr[9].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[10].ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[11].ToString() ?? DBNull.Value.ToString()),
                    UserID = Convert.ToInt32(sdr[12].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[12].ToString()),
                    JobNumber = sdr[13].ToString() ?? DBNull.Value.ToString(),
                    DriverID = Convert.ToInt32(sdr[14].ToString()),
                    Status = new Status
                    {
                        ID = Convert.ToInt32(sdr[15].ToString()),
                        Name = sdr[16].ToString() ?? DBNull.Value.ToString(),
                        CName = sdr[17].ToString() ?? DBNull.Value.ToString(),
                        SoftDelete = Convert.ToBoolean(sdr[18].ToString() ?? DBNull.Value.ToString()),
                        Color = sdr[19].ToString() ?? DBNull.Value.ToString()
                    },
                };
                if (sdr[13] + "" != "" && sdr[20] + "" != "")
                {
                    //dl.JobID = Convert.ToInt32(sdr[8].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[8].ToString());
                    dl.JobWorkingDate = Convert.ToDateTime(sdr[20].ToString() ?? DBNull.Value.ToString());
                }
                //if (sdr[13] + "" != "" && sdr[20] + "" != "")
                //{
                //    dl.JobName = sdr[20].ToString() ?? DBNull.Value.ToString();
                //}
                //if (!(uid + "" != "0" && uid + "" != ""))
                //{
                //    User user = new User
                //    {
                //        ID = Convert.ToInt32(sdr[21].ToString()),
                //        Username = sdr[22].ToString() ?? DBNull.Value.ToString(),
                //        Password = "",
                //        Name = sdr[24].ToString() ?? DBNull.Value.ToString(),
                //        //Gender = Convert.ToInt32(sdr[25].ToString()),
                //        Email = sdr[26].ToString() ?? DBNull.Value.ToString(),
                //        Phone = sdr[27].ToString() ?? DBNull.Value.ToString(),
                //        DOB = Convert.ToDateTime(sdr[28].ToString() ?? DBNull.Value.ToString()),
                //        //ProfilePicture = sdr[29].ToString() ?? DBNull.Value.ToString(),
                //        Credits = Convert.ToDouble(sdr[30].ToString() ?? DBNull.Value.ToString()),
                //        LeaveDaysLeft = float.Parse(sdr[31].ToString()),
                //        MCDaysLeft = float.Parse(sdr[32].ToString()),
                //        CreatedDate = Convert.ToDateTime(sdr[33].ToString() ?? DBNull.Value.ToString()),
                //        UpdatedDate = Convert.ToDateTime(sdr[34].ToString() ?? DBNull.Value.ToString()),
                //        UserStatus = Convert.ToInt32(sdr[35].ToString()),
                //        RoleID = Convert.ToInt32(sdr[36].ToString()),
                //        SoftDelete = Convert.ToBoolean(sdr[37].ToString() ?? DBNull.Value.ToString()),
                //        IDNumber = sdr[38].ToString() ?? DBNull.Value.ToString(),
                //    };
                //    dl.User = user;
                //}
                finalListToReturn.Add(dl);
            }
            return finalListToReturn;
        }
    }
    public ICollection<UserWithUserOT_DTO> GetUsersByOT_ID(int OT_ID)
    {
        List<UserWithUserOT_DTO> finalListToReturn = new List<UserWithUserOT_DTO>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT u.[ID],u.[Username],u.[Password],u.[Name],u.[Gender],u.[Email],u.[Phone],u.[DOB],u.[ProfilePicture],u.[Credits],u.[LeaveDaysLeft],u.[MCDaysLeft],u.[CreatedDate],u.[UpdatedDate],u.[UserStatus],u.[RoleID],u.[SoftDelete],u.[IDNumber],uOT.[ID] AS uOT_ID " +
                                        "FROM [AvaChem_UserOT] AS uOT " +
                                        "LEFT JOIN [AvaChem_User] AS u ON uOT.[UserID]=u.[ID] " +
                                        "WHERE uOT.[SoftDelete]=0 AND u.[SoftDelete]=0 AND u.[RoleID] NOT IN (@Admin) AND uOT.[OT_ID]=@OT_ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@Admin", UserRoles.SuperAdmin.GetHashCode());
            sqlCommand.Parameters.AddWithValue("@OT_ID", OT_ID);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                UserWithUserOT_DTO dl = new UserWithUserOT_DTO()
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
                    UserOT_ID = Convert.ToInt32(sdr[18].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[18].ToString()),
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
            sqlCommand.CommandText = "SELECT COUNT(*) FROM [dbo].[AvaChem_UserOT]";
            int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
            return count;
        }
    }
    public UserOT UpdateUserOT(UserOT AvaChemUserOT)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_UserOT]SET[ID]=@ID,[UserID]=@UserID,[OT_ID]=@OT_ID,[UpdatedDate]=@UpdatedDate,[SoftDelete]=@SoftDelete WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", AvaChemUserOT.ID == 0 ? 0 : AvaChemUserOT.ID);
            sqlCommand.Parameters.AddWithValue("@UserID", AvaChemUserOT.UserID == 0 ? 0 : AvaChemUserOT.UserID);
            sqlCommand.Parameters.AddWithValue("@OT_ID", AvaChemUserOT.OT_ID == 0 ? 0 : AvaChemUserOT.OT_ID);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemUserOT.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return AvaChemUserOT;
        }
    }
    public void UpdateUserOTSoftDelete(int ID)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE[dbo].[AvaChem_UserOT] SET [SoftDelete]=1,[UpdatedDate]=@UpdatedDate WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
    }
    public void UpdateSoftDeleteByOT_ID(int OT_ID)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_UserOT] SET [SoftDelete]=1,[UpdatedDate]=@UpdatedDate WHERE [OT_ID]=@OT_ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@OT_ID", OT_ID);
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
            sqlCommand.CommandText = "DELETE FROM [dbo].[AvaChem_UserOT] WHERE [ID]=@ID";
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

    public bool CheckHasThisUser(int OT_ID, int uid)
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
            sqlCommand.CommandText = "SELECT COUNT(uOT.[UserID]) as CountUser FROM [AvaChem_UserOT] AS uOT " +
                                        "WHERE uOT.[SoftDelete]=0 AND uOT.[OT_ID]=@OT_ID AND uOT.[UserID]=@UserID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@OT_ID", OT_ID);
            sqlCommand.Parameters.AddWithValue("@UserID", uid);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                count = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString());
            }
        }
        return count > 0;
    }
}