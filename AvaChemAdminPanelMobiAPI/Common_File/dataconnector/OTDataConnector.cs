using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
public class OTDataConnector
{
    public OTDataConnector()
    {
    }
    public OT CreateOT(OT AvaChemOT)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "INSERT INTO [dbo].[AvaChem_OT]([StatusID],[WorkerStartedTime],[WorkerEndedTime],[DriverStartedTime],[DriverEndedTime],[JobName],[JobID],[CreatedDate],[UpdatedDate],[SoftDelete],[UserID],[JobNumber],[DriverID])Values(@StatusID,@WorkerStartedTime,@WorkerEndedTime,@DriverStartedTime,@DriverEndedTime,@JobName,@JobID,@CreatedDate,@UpdatedDate,@SoftDelete,@UserID,@JobNumber,@DriverID)";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", AvaChemOT.ID == 0 ? 0 : AvaChemOT.ID);
            sqlCommand.Parameters.AddWithValue("@StatusID", AvaChemOT.StatusID == 0 ? 0 : AvaChemOT.StatusID);
            sqlCommand.Parameters.AddWithValue("@WorkerStartedTime", AvaChemOT.WorkerStartedTime.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@WorkerEndedTime", AvaChemOT.WorkerEndedTime.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@DriverStartedTime", AvaChemOT.DriverStartedTime.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@DriverEndedTime", AvaChemOT.DriverEndedTime.ToString() ?? DBNull.Value.ToString());
            //sqlCommand.Parameters.AddWithValue("@JobName", AvaChemOT.JobName ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@JobName", DBNull.Value.ToString());
            //sqlCommand.Parameters.AddWithValue("@JobID", AvaChemOT.JobID);
            sqlCommand.Parameters.AddWithValue("@JobID", 0);
            sqlCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemOT.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@UserID", AvaChemOT.UserID == 0 ? 0 : AvaChemOT.UserID);
            sqlCommand.Parameters.AddWithValue("@JobNumber", AvaChemOT.JobNumber.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@DriverID", AvaChemOT.DriverID);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return GetOTByLastInsertedID();
        }
    }
    public OT GetOTByFirstVariable(int ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT [ID],[OTNumber],[StatusID],[WorkerStartedTime],[WorkerEndedTime],[DriverStartedTime],[DriverEndedTime],[JobName],[JobID],[CreatedDate],[UpdatedDate],[SoftDelete],[UserID],[JobNumber],[DriverID] FROM [dbo].[AvaChem_OT] WHERE [ID]=@ID and [SoftDelete]=0";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new OT()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    OTNumber = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    StatusID = Convert.ToInt32(sdr[2].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[2].ToString()),
                    WorkerStartedTime = Convert.ToDateTime(sdr[3].ToString() ?? DBNull.Value.ToString()),
                    WorkerEndedTime = Convert.ToDateTime(sdr[4].ToString() ?? DBNull.Value.ToString()),
                    DriverStartedTime = Convert.ToDateTime(sdr[5].ToString() ?? DBNull.Value.ToString()),
                    DriverEndedTime = Convert.ToDateTime(sdr[6].ToString() ?? DBNull.Value.ToString()),
                    //JobName = sdr[7].ToString() ?? DBNull.Value.ToString(),
                    //JobID = Convert.ToInt32(sdr[8].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[9].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[10].ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[11].ToString() ?? DBNull.Value.ToString()),
                    UserID = Convert.ToInt32(sdr[12].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[12].ToString()),
                    JobNumber = sdr[13].ToString() ?? DBNull.Value.ToString(),
                    DriverID = Convert.ToInt32(sdr[14].ToString()),
                };
            }
            return null;
        }
    }
    public OT GetOTByLastInsertedID()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT [ID],[OTNumber],[StatusID],[WorkerStartedTime],[WorkerEndedTime],[DriverStartedTime],[DriverEndedTime],[JobName],[JobID],[CreatedDate],[UpdatedDate],[SoftDelete],[UserID],[JobNumber],[DriverID] FROM [dbo].[AvaChem_OT] WHERE [ID]=(SELECT MAX(ID) FROM [AvaChem_OT]) AND [SoftDelete]=0";
            sqlCommand.Parameters.Clear();
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new OT()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    OTNumber = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    StatusID = Convert.ToInt32(sdr[2].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[2].ToString()),
                    WorkerStartedTime = Convert.ToDateTime(sdr[3].ToString() ?? DBNull.Value.ToString()),
                    WorkerEndedTime = Convert.ToDateTime(sdr[4].ToString() ?? DBNull.Value.ToString()),
                    DriverStartedTime = Convert.ToDateTime(sdr[5].ToString() ?? DBNull.Value.ToString()),
                    DriverEndedTime = Convert.ToDateTime(sdr[6].ToString() ?? DBNull.Value.ToString()),
                    //JobName = sdr[7].ToString() ?? DBNull.Value.ToString(),
                    //JobID = Convert.ToInt32(sdr[8].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[9].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[10].ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[11].ToString() ?? DBNull.Value.ToString()),
                    UserID = Convert.ToInt32(sdr[12].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[12].ToString()),
                    JobNumber = sdr[13].ToString() ?? DBNull.Value.ToString(),
                    DriverID = Convert.ToInt32(sdr[14].ToString()),
                };
            }
            return null;
        }
    }
    public ICollection<OT> GetAll(int? page = null, int? per_page = null, string search = "",
                                            int? type = null, int? month = null, int? year = null, int? sort = null)
    {
        List<OT> finalListToReturn = new List<OT>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT o.[ID],o.[OTNumber],o.[StatusID],o.[WorkerStartedTime],o.[WorkerEndedTime],o.[DriverStartedTime],o.[DriverEndedTime],o.[JobName],o.[JobID],o.[CreatedDate],o.[UpdatedDate],o.[SoftDelete],o.[UserID],o.[JobNumber],o.[DriverID] " +
                                        "FROM [dbo].[AvaChem_OT] AS o " +
                                        "LEFT JOIN [AvaChem_Trip] AS t ON o.[JobNumber]=t.[JobNumber] " +
                                        "LEFT JOIN [AvaChem_Job] AS j ON t.[JobID]=j.[ID] " +
                                        "WHERE o.[SoftDelete]=0 " +
                                        (search + "" == "" ? ""
                                                           : "AND ( o.[OTNumber] Like @Search OR o.[JobName] Like @Search ) ") +
                                        (type + "" == "" ? ""
                                                         : type == OTProgressTypes.Pending.GetHashCode()
                                                         ? "AND o.[StatusID]=@PendingStatusID "
                                                         : type == OTProgressTypes.Approved.GetHashCode()
                                                         ? "AND o.[StatusID]=@ApprovedStatusID "
                                                         : type == OTProgressTypes.Rejected.GetHashCode()
                                                         ? "AND o.[StatusID]=@RejectedStatusID "
                                                         : "") +
                                        ((month + "" == "0" || month + "" == "") ? "" : "AND MONTH(COALESCE(j.[WorkingDate], o.[CreatedDate]))=@Month ") +
                                        ((year + "" == "0" || year + "" == "") ? "" : "AND YEAR(COALESCE(j.[WorkingDate], o.[CreatedDate]))=@Year ") +
                                        // ((from + "" == "" && to + "" == "") ? ""
                                        //                  : "AND j.[WorkingDate] BETWEEN @BetweenStart " +
                                        //                                 "and @BetweenEnd ") +
                                        (sort + "" == "0" ? "ORDER BY o.[ID] DESC" : "ORDER BY COALESCE(j.[WorkingDate], o.[CreatedDate]) " + (sort == 1 ? "ASC" : "DESC")) +
                                        ((page + "" == "" || per_page + "" == "")
                                            ? ""
                                            : " OFFSET (@Offset) ROWS FETCH NEXT @PerPage ROWS ONLY");
            sqlCommand.Parameters.Clear();
            if (search + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@Search", $"%{search}%");
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
                OT dl = new OT()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    OTNumber = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    StatusID = Convert.ToInt32(sdr[2].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[2].ToString()),
                    WorkerStartedTime = Convert.ToDateTime(sdr[3].ToString() ?? DBNull.Value.ToString()),
                    WorkerEndedTime = Convert.ToDateTime(sdr[4].ToString() ?? DBNull.Value.ToString()),
                    DriverStartedTime = Convert.ToDateTime(sdr[5].ToString() ?? DBNull.Value.ToString()),
                    DriverEndedTime = Convert.ToDateTime(sdr[6].ToString() ?? DBNull.Value.ToString()),
                    //JobName = sdr[7].ToString() ?? DBNull.Value.ToString(),
                    //JobID = Convert.ToInt32(sdr[8].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[9].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[10].ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[11].ToString() ?? DBNull.Value.ToString()),
                    UserID = Convert.ToInt32(sdr[12].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[12].ToString()),
                    JobNumber = sdr[13].ToString() ?? DBNull.Value.ToString(),
                    DriverID = Convert.ToInt32(sdr[14].ToString()),
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
            sqlCommand.CommandText = "SELECT COUNT(*) FROM [dbo].[AvaChem_OT] WHERE [SoftDelete]=0";
            int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
            return count;
        }
    }
    public OT UpdateOT(OT AvaChemOT)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_OT]SET [StatusID]=@StatusID,[WorkerStartedTime]=@WorkerStartedTime,[WorkerEndedTime]=@WorkerEndedTime,[DriverStartedTime]=@DriverStartedTime,[DriverEndedTime]=@DriverEndedTime,[JobName]=@JobName,[JobID]=@JobID,[UpdatedDate]=@UpdatedDate,[SoftDelete]=@SoftDelete,[JobNumber]=@JobNumber,[DriverID]=@DriverID WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", AvaChemOT.ID == 0 ? 0 : AvaChemOT.ID);
            sqlCommand.Parameters.AddWithValue("@StatusID", AvaChemOT.StatusID == 0 ? 0 : AvaChemOT.StatusID);
            sqlCommand.Parameters.AddWithValue("@WorkerStartedTime", AvaChemOT.WorkerStartedTime.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@WorkerEndedTime", AvaChemOT.WorkerEndedTime.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@DriverStartedTime", AvaChemOT.DriverStartedTime.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@DriverEndedTime", AvaChemOT.DriverEndedTime.ToString() ?? DBNull.Value.ToString());
            //sqlCommand.Parameters.AddWithValue("@JobName", AvaChemOT.JobName ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@JobName", DBNull.Value.ToString());
            //sqlCommand.Parameters.AddWithValue("@JobID", AvaChemOT.JobID);
            sqlCommand.Parameters.AddWithValue("@JobID", 0);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemOT.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@JobNumber", AvaChemOT.JobNumber ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@DriverID", AvaChemOT.DriverID);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return AvaChemOT;
        }
    }
    public OT UpdateOTStatus(int ID, int StatusID)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_OT] SET [StatusID]=@StatusID,[UpdatedDate]=@UpdatedDate WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            sqlCommand.Parameters.AddWithValue("@StatusID", StatusID);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return this.GetOTByFirstVariable(ID);
        }
    }
    public void UpdateOTSoftDelete(int ID)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE[dbo].[AvaChem_OT] SET [SoftDelete]=1,[UpdatedDate]=@UpdatedDate WHERE [ID]=@ID";
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
            sqlCommand.CommandText = "DELETE FROM [dbo].[AvaChem_OT] WHERE [ID]=@ID";
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
}