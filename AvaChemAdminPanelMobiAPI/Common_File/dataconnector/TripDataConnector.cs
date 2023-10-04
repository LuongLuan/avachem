using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
public class TripDataConnector
{
    public TripDataConnector()
    {
    }
    public Trip CreateTrip(Trip AvaChemTrip)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "INSERT INTO [dbo].[AvaChem_Trip]([StartTime],[EndTime],[SoftDelete],[JobID],[CreatedDate],[UpdatedDate])Values(@StartTime,@EndTime,@SoftDelete,@JobID,@CreatedDate,@UpdatedDate)";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@StartTime", AvaChemTrip.StartTime.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@EndTime", AvaChemTrip.EndTime.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemTrip.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@JobID", AvaChemTrip.JobID == 0 ? 0 : AvaChemTrip.JobID);


            //sqlCommand.Parameters.AddWithValue("@WorkerStartedTime", AvaChemTrip.WorkerStartedTime.ToString() ?? DBNull.Value.ToString());
            //sqlCommand.Parameters.AddWithValue("@WorkerEndedTime", AvaChemTrip.WorkerEndedTime.ToString() ?? DBNull.Value.ToString());
            //sqlCommand.Parameters.AddWithValue("@Remarks", AvaChemTrip.Remarks ?? DBNull.Value.ToString());
            //sqlCommand.Parameters.AddWithValue("@CustomerSignatureImage", AvaChemTrip.CustomerSignatureImage ?? DBNull.Value.ToString());

            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return GetTripByLastInsertedID();
        }
    }
    public Trip GetTripByFirstVariable(int ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[StartTime],[EndTime],[WorkerStartedTime],[WorkerEndedTime],[Remarks],[CustomerSignatureImage],[CreatedDate],[UpdatedDate],[SoftDelete],[JobID],[JobNumber] FROM [dbo].[AvaChem_Trip] WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                Trip dl = new Trip()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    StartTime = Convert.ToDateTime(sdr[1].ToString().ToString() ?? DBNull.Value.ToString()),
                    EndTime = Convert.ToDateTime(sdr[2].ToString().ToString() ?? DBNull.Value.ToString()),
                    //WorkerStartedTime = Convert.ToDateTime(sdr[3].ToString().ToString() ?? DBNull.Value.ToString()),
                    //WorkerEndedTime = Convert.ToDateTime(sdr[4].ToString().ToString() ?? DBNull.Value.ToString()),
                    Remarks = sdr[5].ToString() ?? DBNull.Value.ToString(),
                    CustomerSignatureImage = sdr[6].ToString() ?? DBNull.Value.ToString(),
                    CreatedDate = Convert.ToDateTime(sdr[7].ToString().ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[8].ToString().ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[9].ToString() ?? DBNull.Value.ToString()),
                    JobID = Convert.ToInt32(sdr[10].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[10].ToString()),
                    JobNumber = sdr[11].ToString() ?? DBNull.Value.ToString(),
                };
                if (sdr[3] + "" != "")
                {
                    dl.WorkerStartedTime = Convert.ToDateTime(sdr[3].ToString());
                }
                if (sdr[4] + "" != "")
                {
                    dl.WorkerEndedTime = Convert.ToDateTime(sdr[4].ToString());
                }
                return dl;
            }
            return null;
        }
    }
    public Trip GetTripByLastInsertedID()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[StartTime],[EndTime],[WorkerStartedTime],[WorkerEndedTime],[Remarks],[CustomerSignatureImage],[CreatedDate],[UpdatedDate],[SoftDelete],[JobID],[JobNumber] FROM [dbo].[AvaChem_Trip] WHERE [ID]=(SELECT MAX(ID) FROM [AvaChem_Trip]) AND [SoftDelete]=0";
            sqlCommand.Parameters.Clear();
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                Trip dl = new Trip()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    StartTime = Convert.ToDateTime(sdr[1].ToString().ToString() ?? DBNull.Value.ToString()),
                    EndTime = Convert.ToDateTime(sdr[2].ToString().ToString() ?? DBNull.Value.ToString()),
                    //WorkerStartedTime = Convert.ToDateTime(sdr[3].ToString().ToString() ?? DBNull.Value.ToString()),
                    //WorkerEndedTime = Convert.ToDateTime(sdr[4].ToString().ToString() ?? DBNull.Value.ToString()),
                    Remarks = sdr[5].ToString() ?? DBNull.Value.ToString(),
                    CustomerSignatureImage = sdr[6].ToString() ?? DBNull.Value.ToString(),
                    CreatedDate = Convert.ToDateTime(sdr[7].ToString().ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[8].ToString().ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[9].ToString() ?? DBNull.Value.ToString()),
                    JobID = Convert.ToInt32(sdr[10].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[10].ToString()),
                    JobNumber = sdr[11].ToString() ?? DBNull.Value.ToString(),
                };
                if (sdr[3] + "" != "")
                {
                    dl.WorkerStartedTime = Convert.ToDateTime(sdr[3].ToString());
                }
                if (sdr[4] + "" != "")
                {
                    dl.WorkerEndedTime = Convert.ToDateTime(sdr[4].ToString());
                }
                return dl;
            }
            return null;
        }
    }
    public ICollection<Trip> GetAll()
    {
        List<Trip> finalListToReturn = new List<Trip>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[StartTime],[EndTime],[WorkerStartedTime],[WorkerEndedTime],[Remarks],[CustomerSignatureImage],[CreatedDate],[UpdatedDate],[SoftDelete],[JobID],[JobNumber] FROM [dbo].[AvaChem_Trip] WHERE [SoftDelete]=0";
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                Trip dl = new Trip()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    StartTime = Convert.ToDateTime(sdr[1].ToString().ToString() ?? DBNull.Value.ToString()),
                    EndTime = Convert.ToDateTime(sdr[2].ToString().ToString() ?? DBNull.Value.ToString()),
                    //WorkerStartedTime = Convert.ToDateTime(sdr[3].ToString().ToString() ?? DBNull.Value.ToString()),
                    //WorkerEndedTime = Convert.ToDateTime(sdr[4].ToString().ToString() ?? DBNull.Value.ToString()),
                    Remarks = sdr[5].ToString() ?? DBNull.Value.ToString(),
                    CustomerSignatureImage = sdr[6].ToString() ?? DBNull.Value.ToString(),
                    CreatedDate = Convert.ToDateTime(sdr[7].ToString().ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[8].ToString().ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[9].ToString() ?? DBNull.Value.ToString()),
                    JobID = Convert.ToInt32(sdr[10].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[10].ToString()),
                    JobNumber = sdr[11].ToString() ?? DBNull.Value.ToString(),
                };
                if (sdr[3] + "" != "")
                {
                    dl.WorkerStartedTime = Convert.ToDateTime(sdr[3].ToString());
                }
                if (sdr[4] + "" != "")
                {
                    dl.WorkerEndedTime = Convert.ToDateTime(sdr[4].ToString());
                }
                finalListToReturn.Add(dl);
            }
            return finalListToReturn;
        }
    }
    public ICollection<Trip> GetByJobID(int jobID)
    {
        List<Trip> finalListToReturn = new List<Trip>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[StartTime],[EndTime],[WorkerStartedTime],[WorkerEndedTime],[Remarks],[CustomerSignatureImage],[CreatedDate],[UpdatedDate],[SoftDelete],[JobID],[JobNumber] FROM [dbo].[AvaChem_Trip] WHERE [JobID]=@JobID AND [SoftDelete]=0";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@JobID", jobID);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                Trip dl = new Trip()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    StartTime = Convert.ToDateTime(sdr[1].ToString().ToString() ?? DBNull.Value.ToString()),
                    EndTime = Convert.ToDateTime(sdr[2].ToString().ToString() ?? DBNull.Value.ToString()),
                    //WorkerStartedTime = Convert.ToDateTime(sdr[3].ToString().ToString() ?? DBNull.Value.ToString()),
                    //WorkerEndedTime = Convert.ToDateTime(sdr[4].ToString().ToString() ?? DBNull.Value.ToString()),
                    Remarks = sdr[5].ToString() ?? DBNull.Value.ToString(),
                    CustomerSignatureImage = sdr[6].ToString() ?? DBNull.Value.ToString(),
                    CreatedDate = Convert.ToDateTime(sdr[7].ToString().ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[8].ToString().ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[9].ToString() ?? DBNull.Value.ToString()),
                    JobID = Convert.ToInt32(sdr[10].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[10].ToString()),
                    JobNumber = sdr[11].ToString() ?? DBNull.Value.ToString(),
                };
                if (sdr[3] + "" != "")
                {
                    dl.WorkerStartedTime = Convert.ToDateTime(sdr[3].ToString());
                }
                if (sdr[4] + "" != "")
                {
                    dl.WorkerEndedTime = Convert.ToDateTime(sdr[4].ToString());
                }
                finalListToReturn.Add(dl);
            }
            return finalListToReturn;
        }
    }
    public int CountByJobID(int jobID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT COUNT([ID]) FROM [dbo].[AvaChem_Trip] WHERE [JobID]=@JobID AND [SoftDelete]=0";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@JobID", jobID);
            int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
            return count;
        }
    }
    public int CountAll()
    {
        List<Trip> finalListToReturn = new List<Trip>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT COUNT(*) FROM [dbo].[AvaChem_Trip] WHERE [SoftDelete]=0";
            int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
            return count;
        }
    }
    public Trip UpdateTrip(Trip AvaChemTrip)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_Trip] SET [StartTime]=@StartTime,[EndTime]=@EndTime,[Remarks]=@Remarks,[UpdatedDate]=@UpdatedDate,[SoftDelete]=@SoftDelete,[JobID]=@JobID,[JobNumber]=@JobNumber" +
                                    (AvaChemTrip.CustomerSignatureImage + "" == "" ? "" : ",[CustomerSignatureImage]=@CustomerSignatureImage") +
                                    (AvaChemTrip.WorkerStartedTime + "" == "" ? "" : ",[WorkerStartedTime]=@WorkerStartedTime") +
                                    (AvaChemTrip.WorkerEndedTime + "" == "" ? "" : ",[WorkerEndedTime]=@WorkerEndedTime") +
                                    " WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", AvaChemTrip.ID == 0 ? 0 : AvaChemTrip.ID);
            sqlCommand.Parameters.AddWithValue("@StartTime", AvaChemTrip.StartTime.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@EndTime", AvaChemTrip.EndTime.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Remarks", AvaChemTrip.Remarks ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemTrip.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@JobID", AvaChemTrip.JobID == 0 ? 0 : AvaChemTrip.JobID);
            sqlCommand.Parameters.AddWithValue("@JobNumber", AvaChemTrip.JobNumber ?? DBNull.Value.ToString());
            if (AvaChemTrip.CustomerSignatureImage + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@CustomerSignatureImage", AvaChemTrip.CustomerSignatureImage.ToString() ?? DBNull.Value.ToString());
            }
            if (AvaChemTrip.WorkerStartedTime + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@WorkerStartedTime", AvaChemTrip.WorkerStartedTime.ToString() ?? DBNull.Value.ToString());
            }
            if (AvaChemTrip.WorkerEndedTime + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@WorkerEndedTime", AvaChemTrip.WorkerEndedTime.ToString() ?? DBNull.Value.ToString());
            }

            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return AvaChemTrip;
        }
    }
    public Trip UpdateTripPartial(Trip AvaChemTrip)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_Trip] SET [JobNumber]=@JobNumber,[Remarks]=@Remarks,[CustomerSignatureImage]=@CustomerSignatureImage,[UpdatedDate]=@UpdatedDate" +
                                    (AvaChemTrip.WorkerStartedTime + "" == "" ? "" : ",[WorkerStartedTime]=@WorkerStartedTime") +
                                    (AvaChemTrip.WorkerEndedTime + "" == "" ? "" : ",[WorkerEndedTime]=@WorkerEndedTime") +
                                    " WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", AvaChemTrip.ID == 0 ? 0 : AvaChemTrip.ID);
            sqlCommand.Parameters.AddWithValue("@JobNumber", AvaChemTrip.JobNumber ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Remarks", AvaChemTrip.Remarks ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@CustomerSignatureImage", AvaChemTrip.CustomerSignatureImage ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

            if (AvaChemTrip.WorkerStartedTime + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@WorkerStartedTime", AvaChemTrip.WorkerStartedTime.ToString() ?? DBNull.Value.ToString());
            }
            if (AvaChemTrip.WorkerEndedTime + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@WorkerEndedTime", AvaChemTrip.WorkerEndedTime.ToString() ?? DBNull.Value.ToString());
            }
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return GetTripByFirstVariable(AvaChemTrip.ID);
        }
    }
    public void UpdateSignatureByID(int ID, string imgUrl)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_Trip] SET [CustomerSignatureImage]=@CustomerSignatureImage,[UpdatedDate]=@UpdatedDate WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            sqlCommand.Parameters.AddWithValue("@CustomerSignatureImage", imgUrl);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

        }
    }
    public Trip UpdateTripSoftDelete(int ID)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_Trip] SET [SoftDelete]=1,[UpdatedDate]=@UpdatedDate WHERE [ID]=@ID";
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
            sqlCommand.CommandText = "DELETE FROM [dbo].[AvaChem_Trip] WHERE [ID]=@ID";
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