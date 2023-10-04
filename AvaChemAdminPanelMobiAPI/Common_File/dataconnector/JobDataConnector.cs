using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
public class JobDataConnector
{
    public JobDataConnector()
    {
    }
    public Job CreateJob(Job AvaChemJob)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "INSERT INTO [dbo].[AvaChem_Job]" +
                                        "([WorkingDate],[Name],[AdminRemarks],[Location],[CreatedDate],[UpdatedDate],[SoftDelete],[ClientID],[ReportName],[JobNumber],[InvoiceNo])" +
                                        "Values(@WorkingDate,@Name,@AdminRemarks,@Location,@CreatedDate,@UpdatedDate,@SoftDelete,@ClientID,@ReportName,@JobNumber,@InvoiceNo)";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@WorkingDate", AvaChemJob.WorkingDate.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Name", AvaChemJob.Name ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@AdminRemarks", AvaChemJob.AdminRemarks ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Location", AvaChemJob.Location ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemJob.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@ClientID", AvaChemJob.ClientID == 0 ? 0 : AvaChemJob.ClientID);
            sqlCommand.Parameters.AddWithValue("@ReportName", DBNull.Value.ToString());
            //sqlCommand.Parameters.AddWithValue("@JobNumber", AvaChemJob.JobNumber ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@JobNumber", DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@InvoiceNo", AvaChemJob.InvoiceNo ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return GetJobByLastInsertedID();
        }
    }
    public Job GetJobByFirstVariable(int ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT j.[ID],j.[WorkingDate],j.[JobNumber],j.[Name],j.[AdminRemarks],c.[Location]," +
                                                            "j.[CreatedDate],j.[UpdatedDate],j.[SoftDelete],j.[ClientID],j.[ReportName],j.[InvoiceNo] " +
                                                            "FROM [dbo].[AvaChem_Job] AS j " +
                                                            "LEFT JOIN [dbo].[AvaChem_Client] AS c ON j.[ClientID] = c.[ID] " +
                                                            "WHERE j.[ID]=@ID";

            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
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
                return dl;
            }
            return null;
        }
    }
    public Job GetByJobNumber(string jobNumber)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT j.[ID],j.[WorkingDate],j.[JobNumber],j.[Name],j.[AdminRemarks],c.[Location]," +
                                                            "j.[CreatedDate],j.[UpdatedDate],j.[SoftDelete],j.[ClientID],j.[ReportName],j.[InvoiceNo] " +
                                                            "FROM [dbo].[AvaChem_Job] AS j " +
                                                            "LEFT JOIN [dbo].[AvaChem_Client] AS c ON j.[ClientID] = c.[ID] " +
                                                            "INNER JOIN [dbo].[AvaChem_Trip] AS t ON j.[ID] = t.[JobID] AND t.[JobNumber]=@JobNumber";

            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@JobNumber", jobNumber);
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
                return dl;
            }
            return null;
        }
    }
    public Job GetJobByLastInsertedID()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT j.[ID],j.[WorkingDate],j.[JobNumber],j.[Name],j.[AdminRemarks],c.[Location]," +
                                                            "j.[CreatedDate],j.[UpdatedDate],j.[SoftDelete],j.[ClientID],j.[ReportName],j.[InvoiceNo] " +
                                                            "FROM [dbo].[AvaChem_Job] AS j " +
                                                            "LEFT JOIN [dbo].[AvaChem_Client] AS c ON j.[ClientID] = c.[ID] " +
                                                            "WHERE j.[ID]=(SELECT MAX(ID) FROM [AvaChem_Job]) AND j.[SoftDelete]=0";
            sqlCommand.Parameters.Clear();
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
                return dl;
            }
            return null;
        }
    }
    public ICollection<Job> GetAll(int? page = null, int? per_page = null, string search = "",
                                    int? type = null, int? month = null, int? year = null
                                    //string from = "", string to = "",
                                    )
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
                                        "FROM [dbo].[AvaChem_Job] AS j " +
                                        "LEFT JOIN [dbo].[AvaChem_Trip] AS t ON j.[ID] = t.[JobID] " +
                                        "LEFT JOIN [dbo].[AvaChem_Client] AS c ON j.[ClientID] = c.[ID] " +
                                        "WHERE j.[SoftDelete]=0 " +
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
                                        //((from + "" == "" && to + "" == "") ? ""
                                        //                 : "AND j.[WorkingDate] BETWEEN @BetweenStart " +
                                        //                                "AND @BetweenEnd ") +
                                        ((month + "" == "0" || month + "" == "") ? "" : "AND MONTH(j.[WorkingDate])=@Month ") +
                                        ((year + "" == "0" || year + "" == "") ? "" : "AND YEAR(j.[WorkingDate])=@Year ") +
                                        "GROUP BY j.[ID],j.[WorkingDate],j.[JobNumber],j.[Name],j.[AdminRemarks],c.[Location],j.[CreatedDate],j.[UpdatedDate],j.[SoftDelete],j.[ClientID],j.[ReportName],j.[InvoiceNo] " +
                                        ((page + "" == "" || per_page + "" == "") ? "ORDER BY j.[ID] DESC" : "ORDER BY j.[ID] DESC OFFSET (@Offset) ROWS FETCH NEXT @PerPage ROWS ONLY");
            sqlCommand.Parameters.Clear();
            if (search + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@Search", $"%{search}%");
            }
            //if (from + "" != "" || to + "" != "")
            //{
            //    sqlCommand.Parameters.AddWithValue("@BetweenStart", from + "" != "" ? Convert.ToDateTime(from).ToString("yyyy-MM-dd") : Convert.ToDateTime(to).AddDays(-1).ToString("yyyy-MM-dd"));
            //    sqlCommand.Parameters.AddWithValue("@BetweenEnd", to + "" != "" ? Convert.ToDateTime(to).ToString("yyyy-MM-dd") : Convert.ToDateTime(from).AddDays(1).ToString("yyyy-MM-dd"));
            //}
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
    public ICollection<Job> GetByClientID(int clientID)
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
                                        "FROM [dbo].[AvaChem_Job] AS j " +
                                        "LEFT JOIN [dbo].[AvaChem_Client] AS c ON j.[ClientID] = c.[ID] " +
                                        "WHERE j.[SoftDelete]=0 AND j.[ClientID]=@ClientID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ClientID", clientID);
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
    public Job GetByReportName(string ReportName)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT j.[ID],j.[WorkingDate],j.[JobNumber],j.[Name],j.[AdminRemarks],c.[Location]," +
                                                            "j.[CreatedDate],j.[UpdatedDate],j.[SoftDelete],j.[ClientID],j.[ReportName],j.[InvoiceNo] " +
                                                            "FROM [dbo].[AvaChem_Job] AS j " +
                                                            "LEFT JOIN [dbo].[AvaChem_Client] AS c ON j.[ClientID] = c.[ID] " +
                                                            "WHERE j.[ReportName]=@ReportName";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ReportName", ReportName);
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
                return dl;
            }
            return null;
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
            sqlCommand.CommandText = "SELECT COUNT(*) FROM [dbo].[AvaChem_Job] WHERE [SoftDelete]=0";
            int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
            return count;
        }
    }
    public Job UpdateJob(Job AvaChemJob)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_Job] " +
                                        "SET [WorkingDate]=@WorkingDate,[Name]=@Name,[AdminRemarks]=@AdminRemarks,[Location]=@Location,[UpdatedDate]=@UpdatedDate," +
                                        "[SoftDelete]=@SoftDelete,[ClientID]=@ClientID,[JobNumber]=@JobNumber,[InvoiceNo]=@InvoiceNo " +
                                        "WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", AvaChemJob.ID == 0 ? 0 : AvaChemJob.ID);
            sqlCommand.Parameters.AddWithValue("@WorkingDate", AvaChemJob.WorkingDate.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Name", AvaChemJob.Name ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@AdminRemarks", AvaChemJob.AdminRemarks ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Location", AvaChemJob.Location ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemJob.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@ClientID", AvaChemJob.ClientID == 0 ? 0 : AvaChemJob.ClientID);
            //sqlCommand.Parameters.AddWithValue("@JobNumber", AvaChemJob.JobNumber ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@JobNumber", DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@InvoiceNo", AvaChemJob.InvoiceNo ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return AvaChemJob;
        }
    }
    public string UpdateReportName(int ID, string reportName)
    {
        //string prefix = $"INV-{DateTime.Now.Year}-";
        //string reportName = $"{prefix}00{jobNumber}";

        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_Job] SET [ReportName]=@ReportName,[UpdatedDate]=@UpdatedDate WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            sqlCommand.Parameters.AddWithValue("@ReportName", reportName);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            int rowsAffected = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            if (rowsAffected > 0)
            {
                return reportName;
            }
        }
        return "";
    }
    public void UpdateJobSoftDelete(int ID)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE[dbo].[AvaChem_Job] SET [SoftDelete]=1,[UpdatedDate]=@UpdatedDate WHERE [ID]=@ID";
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
            sqlCommand.CommandText = "DELETE FROM [dbo].[AvaChem_Job] WHERE [ID]=@ID";
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
    public bool CheckIsCompleted(int ID)
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
            sqlCommand.CommandText = "SELECT j.[ID],j.[WorkingDate],j.[JobNumber],j.[Name],j.[AdminRemarks],c.[Location]," +
                                        "j.[CreatedDate],j.[UpdatedDate],j.[SoftDelete],j.[ClientID],j.[ReportName],j.[InvoiceNo] " +
                                        "FROM [dbo].[AvaChem_Job] AS j " +
                                        "LEFT JOIN [dbo].[AvaChem_Trip] AS t ON j.[ID] = t.[JobID] " +
                                        "LEFT JOIN [dbo].[AvaChem_Client] AS c ON j.[ClientID] = c.[ID] " +
                                        "WHERE j.[SoftDelete]=0 AND j.[ID]=@ID " +
                                        "AND t.[ID] IS NOT NULL AND t.[SoftDelete]=0" +
                                                         "AND NOT EXISTS ( SELECT 1 FROM [dbo].[AvaChem_Trip] " +
                                                         "WHERE [JobID] = j.[ID] AND " +
                                                         "( ( [CustomerSignatureImage] IS NULL OR [CustomerSignatureImage]='' ) OR [WorkerStartedTime] IS NULL OR [WorkerEndedTime] IS NULL OR [JobNumber] IS NULL ) AND [SoftDelete]=0 ) " +
                                        "GROUP BY j.[ID],j.[WorkingDate],j.[JobNumber],j.[Name],j.[AdminRemarks],c.[Location],j.[CreatedDate],j.[UpdatedDate],j.[SoftDelete],j.[ClientID],j.[ReportName],j.[InvoiceNo]";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                count = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString());
            }
        }
        return count > 0;
    }
}