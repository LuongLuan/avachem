using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
public class LeaveDataConnector
{
    public LeaveDataConnector()
    {
    }
    public Leave CreateLeave(Leave AvaChemLeave)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "INSERT INTO [dbo].[AvaChem_Leave]([StartedDate],[EndedDate],[NumDays],[Remarks],[ProofImage],[ReasonID],[StatusID],[UserID],[CreatedDate],[UpdatedDate],[SoftDelete])Values(@StartedDate,@EndedDate,@NumDays,@Remarks,@ProofImage,@ReasonID,@StatusID,@UserID,@CreatedDate,@UpdatedDate,@SoftDelete)";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@StartedDate", AvaChemLeave.StartedDate.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@EndedDate", AvaChemLeave.EndedDate.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@NumDays", AvaChemLeave.NumDays == 0 ? 0 : AvaChemLeave.NumDays);
            sqlCommand.Parameters.AddWithValue("@Remarks", AvaChemLeave.Remarks ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@ProofImage", AvaChemLeave.ProofImage ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@ReasonID", AvaChemLeave.ReasonID == 0 ? 0 : AvaChemLeave.ReasonID);
            sqlCommand.Parameters.AddWithValue("@StatusID", AvaChemLeave.StatusID == 0 ? 0 : AvaChemLeave.StatusID);
            sqlCommand.Parameters.AddWithValue("@UserID", AvaChemLeave.UserID == 0 ? 0 : AvaChemLeave.UserID);
            sqlCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemLeave.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return GetLeaveByLastInsertedID();
        }
    }
    public Leave GetLeaveByFirstVariable(int ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[StartedDate],[EndedDate],[NumDays],[Remarks],[ProofImage],[ReasonID],[StatusID],[UserID],[CreatedDate],[UpdatedDate],[SoftDelete] FROM [dbo].[AvaChem_Leave] WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new Leave()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    StartedDate = Convert.ToDateTime(sdr[1].ToString() ?? DBNull.Value.ToString()),
                    EndedDate = Convert.ToDateTime(sdr[2].ToString() ?? DBNull.Value.ToString()),
                    NumDays = float.Parse(sdr[3].ToString()) == 0 ? 0 : float.Parse(sdr[3].ToString()),
                    Remarks = sdr[4].ToString() ?? DBNull.Value.ToString(),
                    ProofImage = sdr[5].ToString() ?? DBNull.Value.ToString(),
                    ReasonID = Convert.ToInt32(sdr[6].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[6].ToString()),
                    StatusID = Convert.ToInt32(sdr[7].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[7].ToString()),
                    UserID = Convert.ToInt32(sdr[8].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[8].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[9].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[10].ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[11].ToString() ?? DBNull.Value.ToString())
                };
            }
            return null;
        }
    }
    public Leave GetLeaveByLastInsertedID()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[StartedDate],[EndedDate],[NumDays],[Remarks],[ProofImage],[ReasonID],[StatusID],[UserID],[CreatedDate],[UpdatedDate],[SoftDelete] FROM [dbo].[AvaChem_Leave] WHERE [ID]=(SELECT MAX(ID) FROM [AvaChem_Leave]) AND [SoftDelete]=0";
            sqlCommand.Parameters.Clear();
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new Leave()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    StartedDate = Convert.ToDateTime(sdr[1].ToString() ?? DBNull.Value.ToString()),
                    EndedDate = Convert.ToDateTime(sdr[2].ToString() ?? DBNull.Value.ToString()),
                    NumDays = float.Parse(sdr[3].ToString()) == 0 ? 0 : float.Parse(sdr[3].ToString()),
                    Remarks = sdr[4].ToString() ?? DBNull.Value.ToString(),
                    ProofImage = sdr[5].ToString() ?? DBNull.Value.ToString(),
                    ReasonID = Convert.ToInt32(sdr[6].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[6].ToString()),
                    StatusID = Convert.ToInt32(sdr[7].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[7].ToString()),
                    UserID = Convert.ToInt32(sdr[8].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[8].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[9].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[10].ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[11].ToString() ?? DBNull.Value.ToString())
                };
            }
            return null;
        }
    }
    public List<T> GetAll<T>(int? page = null, int? per_page = null, string search = "",
                                int? type = null, string from = "", string to = "",
                                bool? includeUser = false, int? uid = null,
                                int? month = null, int? year = null, int? sort = null) where T : Leave
    {
        List<T> finalListToReturn = new List<T>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            bool shouldIncludeUser = includeUser == true || search + "" != "";
            sqlCommand.CommandText = "SELECT l.[ID],l.[StartedDate],l.[EndedDate],l.[NumDays],l.[Remarks],l.[ProofImage],l.[ReasonID],l.[StatusID],l.[UserID],l.[CreatedDate],l.[UpdatedDate],l.[SoftDelete] " +
                                        (shouldIncludeUser == false ? "" : ",u.[Name],u.[IDNumber],u.[LeaveDaysLeft],u.[MCDaysLeft] ") +
                                        "FROM [dbo].[AvaChem_Leave] as l " +
                                        (shouldIncludeUser == false ? "" : "LEFT JOIN [AvaChem_User] AS u ON l.[UserID]=u.[ID] ") +
                                        "WHERE l.[SoftDelete]=0 " + (shouldIncludeUser == false ? "" : "AND u.[SoftDelete]=0 ") +
                                        (search + "" == "" ? ""
                                            : "AND ( u.[Name] Like @Search ) ") +
                                        (type + "" == "" ? ""
                                                         : type == LeaveProgressTypes.Upcoming.GetHashCode()
                                                         ? "AND l.[EndedDate]>=@Now "
                                                         : type == LeaveProgressTypes.Past.GetHashCode()
                                                         ? "AND l.[EndedDate]<@Now "
                                                         : type == LeaveProgressTypes.Pending.GetHashCode()
                                                         ? "AND l.[StatusID]=@PendingStatusID "
                                                         : type == LeaveProgressTypes.Completed.GetHashCode()
                                                         ? "AND l.[StatusID] IN (@ApprovedStatusID,@RejectedStatusID) "
                                                         : "") +
                                        ((from + "" == "" && to + "" == "") ? ""
                                                         : "AND ( l.[StartedDate] BETWEEN @BetweenStart AND @BetweenEnd " +
                                                                        "OR l.[EndedDate] BETWEEN @BetweenStart AND @BetweenEnd ) ") +
                                        (uid + "" == "" ? "" : "AND l.[UserID]=@UserID ") +

                                        ((month + "" == "0" || month + "" == "") ? "" : "AND (MONTH(l.[StartedDate])=@Month OR MONTH(l.[EndedDate])=@Month) ") +
                                        ((year + "" == "0" || year + "" == "") ? "" : "AND (YEAR(l.[StartedDate])=@Year OR YEAR(l.[EndedDate])=@Year) ") +
                                        (sort + "" == "0" ? "ORDER BY l.[ID] DESC" : "ORDER BY l.[StartedDate] " + (sort == 1 ? "ASC" : "DESC")) +
                                        ((page + "" == "" || per_page + "" == "")
                                            ? ""
                                            : " OFFSET (@Offset) ROWS FETCH NEXT @PerPage ROWS ONLY");
            sqlCommand.Parameters.Clear();
            if (search + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@Search", $"%{search}%");
            }
            if (type + "" != "" && Enum.IsDefined(typeof(LeaveProgressTypes), type))
            {
                if (type == LeaveProgressTypes.Upcoming.GetHashCode() || type == LeaveProgressTypes.Past.GetHashCode())
                {
                    sqlCommand.Parameters.AddWithValue("@Now", DateTime.Now.AddHours(8));
                }
                if (type == LeaveProgressTypes.Pending.GetHashCode())
                {
                    sqlCommand.Parameters.AddWithValue("@PendingStatusID", Statuses.Pending.GetHashCode());
                }
                if (type == LeaveProgressTypes.Completed.GetHashCode())
                {
                    sqlCommand.Parameters.AddWithValue("@ApprovedStatusID", Statuses.Approved.GetHashCode());
                    sqlCommand.Parameters.AddWithValue("@RejectedStatusID", Statuses.Rejected.GetHashCode());
                }
            }
            if (from + "" != "" || to + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@BetweenStart", from + "" != "" ? Convert.ToDateTime(from).ToString("yyyy-MM-dd") : Convert.ToDateTime(to).AddDays(-1).ToString("yyyy-MM-dd"));
                sqlCommand.Parameters.AddWithValue("@BetweenEnd", to + "" != "" ? Convert.ToDateTime(to).ToString("yyyy-MM-dd") : Convert.ToDateTime(from).AddDays(1).ToString("yyyy-MM-dd"));
            }
            if (uid + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@UserID", uid);
            }
            if (month + "" != "0" && month + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@Month", month);
            }
            if (year + "" != "0" && year + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@Year", year);
            }
            if (page + "" != "" && per_page + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@Offset", (page - 1) * per_page);
                sqlCommand.Parameters.AddWithValue("@PerPage", per_page);
            }
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                LeaveTableView dl = new LeaveTableView()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    StartedDate = Convert.ToDateTime(sdr[1].ToString() ?? DBNull.Value.ToString()),
                    EndedDate = Convert.ToDateTime(sdr[2].ToString() ?? DBNull.Value.ToString()),
                    NumDays = float.Parse(sdr[3].ToString()) == 0 ? 0 : float.Parse(sdr[3].ToString()),
                    Remarks = sdr[4].ToString() ?? DBNull.Value.ToString(),
                    ProofImage = sdr[5].ToString() ?? DBNull.Value.ToString(),
                    ReasonID = Convert.ToInt32(sdr[6].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[6].ToString()),
                    StatusID = Convert.ToInt32(sdr[7].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[7].ToString()),
                    UserID = Convert.ToInt32(sdr[8].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[8].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[9].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[10].ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[11].ToString() ?? DBNull.Value.ToString())
                };
                if (shouldIncludeUser)
                {
                    dl.UName = sdr[12].ToString() ?? DBNull.Value.ToString();
                    dl.UIDNumber = sdr[13].ToString() ?? DBNull.Value.ToString();
                    dl.ULeaveDaysLeft = Convert.ToDouble(sdr[14].ToString()) == 0 ? 0 : Convert.ToDouble(sdr[14].ToString());
                    dl.UMCDaysLeft = Convert.ToDouble(sdr[15].ToString()) == 0 ? 0 : Convert.ToDouble(sdr[15].ToString());
                }
                finalListToReturn.Add(dl as T);
            }
            return finalListToReturn;
        }
    }
    public List<T> GetByParams<T>(string[] workingDates, int? uid = null) where T : Leave
    {
        List<T> finalListToReturn = new List<T>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            string queryWorkingDates = "";
            foreach (string workingDate in workingDates)
            {
                queryWorkingDates += $" AND '{Convert.ToDateTime(workingDate).ToString("yyyy-MM-dd")}' " +
                                        $"BETWEEN l.[StartedDate] AND l.[EndedDate] ";
            }
            sqlCommand.CommandText = "SELECT l.[ID],l.[StartedDate],l.[EndedDate],l.[NumDays],l.[Remarks],l.[ProofImage],l.[ReasonID],l.[StatusID],l.[UserID],l.[CreatedDate],l.[UpdatedDate],l.[SoftDelete] " +
                                        "FROM [dbo].[AvaChem_Leave] as l " +
                                        "WHERE l.[SoftDelete]=0" +
                                         queryWorkingDates +
                                        (uid + "" == "" ? "" : "AND l.[UserID]=@UserID ") +
                                        "ORDER BY l.[ID] DESC";
            sqlCommand.Parameters.Clear();
            //sqlCommand.Parameters.AddWithValue("@WorkingDate", Convert.ToDateTime(workingDate).ToString("yyyy-MM-dd"));
            if (uid + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@UserID", uid);
            }
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                LeaveTableView dl = new LeaveTableView()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    StartedDate = Convert.ToDateTime(sdr[1].ToString() ?? DBNull.Value.ToString()),
                    EndedDate = Convert.ToDateTime(sdr[2].ToString() ?? DBNull.Value.ToString()),
                    NumDays = float.Parse(sdr[3].ToString()) == 0 ? 0 : float.Parse(sdr[3].ToString()),
                    Remarks = sdr[4].ToString() ?? DBNull.Value.ToString(),
                    ProofImage = sdr[5].ToString() ?? DBNull.Value.ToString(),
                    ReasonID = Convert.ToInt32(sdr[6].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[6].ToString()),
                    StatusID = Convert.ToInt32(sdr[7].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[7].ToString()),
                    UserID = Convert.ToInt32(sdr[8].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[8].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[9].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[10].ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[11].ToString() ?? DBNull.Value.ToString())
                };
                finalListToReturn.Add(dl as T);
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
            sqlCommand.CommandText = "SELECT COUNT(*) FROM [dbo].[AvaChem_Leave] WHERE [SoftDelete]=0";
            int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
            return count;
        }
    }
    public Leave UpdateLeave(Leave AvaChemLeave)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_Leave] SET [StartedDate]=@StartedDate,[EndedDate]=@EndedDate,[NumDays]=@NumDays,[Remarks]=@Remarks,[ProofImage]=@ProofImage,[ReasonID]=@ReasonID,[StatusID]=@StatusID,[UserID]=@UserID,[UpdatedDate]=@UpdatedDate,[SoftDelete]=@SoftDelete WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", AvaChemLeave.ID == 0 ? 0 : AvaChemLeave.ID);
            sqlCommand.Parameters.AddWithValue("@StartedDate", AvaChemLeave.StartedDate.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@EndedDate", AvaChemLeave.EndedDate.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@NumDays", AvaChemLeave.NumDays == 0 ? 0 : AvaChemLeave.NumDays);
            sqlCommand.Parameters.AddWithValue("@Remarks", AvaChemLeave.Remarks ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@ProofImage", AvaChemLeave.ProofImage ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@ReasonID", AvaChemLeave.ReasonID == 0 ? 0 : AvaChemLeave.ReasonID);
            sqlCommand.Parameters.AddWithValue("@StatusID", AvaChemLeave.StatusID == 0 ? 0 : AvaChemLeave.StatusID);
            sqlCommand.Parameters.AddWithValue("@UserID", AvaChemLeave.UserID == 0 ? 0 : AvaChemLeave.UserID);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemLeave.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return AvaChemLeave;
        }
    }
    public void UpdateLeaveImageByID(int ID, string imageUrl)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_Leave] SET [ProofImage]=@ProofImage WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            sqlCommand.Parameters.AddWithValue("@ProofImage", imageUrl);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
    }
    public Leave UpdateLeaveSoftDelete(int ID)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE[dbo].[AvaChem_Leave] SET [SoftDelete]=1,[UpdatedDate]=@UpdatedDate WHERE [ID]=@ID ";
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
            sqlCommand.CommandText = "DELETE FROM [dbo].[AvaChem_Leave] WHERE [ID]=@ID";
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
    public bool CheckUserValid(int ID, int uid)
    {
        int count = 0;
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT Count([UserID]) as CountUser FROM [dbo].[AvaChem_Leave] " +
                                        "WHERE [SoftDelete]=0 AND [ID]=@ID AND [UserID]=@UserID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
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