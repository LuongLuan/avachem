using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
public class JobVehicleDataConnector
{
    public JobVehicleDataConnector()
    {
    }
    public JobVehicle CreateJobVehicle(JobVehicle AvaChemJobVehicle)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "INSERT INTO [dbo].[AvaChem_JobVehicle]([JobID],[VehicleID],[CreatedDate],[UpdatedDate],[SoftDelete])Values(@JobID,@VehicleID,@CreatedDate,@UpdatedDate,@SoftDelete)";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@JobID", AvaChemJobVehicle.JobID == 0 ? 0 : AvaChemJobVehicle.JobID);
            sqlCommand.Parameters.AddWithValue("@VehicleID", AvaChemJobVehicle.VehicleID == 0 ? 0 : AvaChemJobVehicle.VehicleID);
            sqlCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemJobVehicle.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return GetJobVehicleByLastInsertedID();
        }
    }
    public JobVehicle GetJobVehicleByFirstVariable(int ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[JobID],[VehicleID],[CreatedDate],[UpdatedDate],[SoftDelete] FROM [dbo].[AvaChem_JobVehicle] WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new JobVehicle()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    JobID = Convert.ToInt32(sdr[1].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[1].ToString()),
                    VehicleID = Convert.ToInt32(sdr[2].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[2].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[3].ToString().ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[4].ToString().ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[5].ToString() ?? DBNull.Value.ToString())
                };
            }
            return null;
        }
    }
    public JobVehicle GetJobVehicleByLastInsertedID()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[JobID],[VehicleID],[CreatedDate],[UpdatedDate],[SoftDelete] FROM [dbo].[AvaChem_JobVehicle] WHERE [ID]=(SELECT MAX(ID) FROM [AvaChem_JobVehicle]) AND [SoftDelete]=0";
            sqlCommand.Parameters.Clear();
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new JobVehicle()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    JobID = Convert.ToInt32(sdr[1].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[1].ToString()),
                    VehicleID = Convert.ToInt32(sdr[2].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[2].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[3].ToString().ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[4].ToString().ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[5].ToString() ?? DBNull.Value.ToString())
                };
            }
            return null;
        }
    }
    public ICollection<JobVehicle> GetAll()
    {
        List<JobVehicle> finalListToReturn = new List<JobVehicle>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT [ID],[JobID],[VehicleID],[CreatedDate],[UpdatedDate],[SoftDelete] FROM [dbo].[AvaChem_JobVehicle] WHERE [SoftDelete]=0";
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                JobVehicle dl = new JobVehicle()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    JobID = Convert.ToInt32(sdr[1].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[1].ToString()),
                    VehicleID = Convert.ToInt32(sdr[2].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[2].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[3].ToString().ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[4].ToString().ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[5].ToString() ?? DBNull.Value.ToString())
                };
                finalListToReturn.Add(dl);
            }
            return finalListToReturn;
        }
    }
    public ICollection<VehicleWithJobVehicleDTO> GetVehiclesByParams(int jobID, int? page = null, int? per_page = null)
    {
        List<VehicleWithJobVehicleDTO> finalListToReturn = new List<VehicleWithJobVehicleDTO>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT v.[ID],v.[Model],v.[Number],v.[SoftDelete],v.[CreatedDate],v.[UpdatedDate],jV.[ID] AS jVID " +
                                        //",j.[WorkingDate],j.[JobNumber],j.[Name] AS jName " +
                                        "FROM [AvaChem_JobVehicle] AS jV " +
                                        "LEFT JOIN [AvaChem_Vehicle] AS v ON jV.[VehicleID]=v.[ID] " +
                                        //"LEFT JOIN [AvaChem_Job] AS j ON jV.[JobID]=j.[ID] " +
                                        "WHERE jV.[SoftDelete]=0 AND v.[SoftDelete]=0 AND jV.[JobID]=@JobID " +
                                        ((page + "" == "" || per_page + "" == "") ? "ORDER BY v.[ID] DESC" : "ORDER BY v.[ID] DESC OFFSET (@Offset) ROWS FETCH NEXT @PerPage ROWS ONLY");
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@JobID", jobID);
            if (page + "" != "" && per_page + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@Offset", (page - 1) * per_page);
                sqlCommand.Parameters.AddWithValue("@PerPage", per_page);
            }
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                VehicleWithJobVehicleDTO dl = new VehicleWithJobVehicleDTO()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Model = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    Number = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    SoftDelete = Convert.ToBoolean(sdr[3].ToString() ?? DBNull.Value.ToString()),
                    //CreatedDate = Convert.ToDateTime(sdr[4].ToString().ToString() ?? DBNull.Value.ToString()),
                    //UpdatedDate = Convert.ToDateTime(sdr[5].ToString().ToString() ?? DBNull.Value.ToString()),
                    JobVehicleID = Convert.ToInt32(sdr[6].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[6].ToString()),
                };
                finalListToReturn.Add(dl);
            }
            return finalListToReturn;
        }
    }
    public int CountAll()
    {
        List<JobVehicle> finalListToReturn = new List<JobVehicle>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT COUNT(*) FROM [dbo].[AvaChem_JobVehicle] WHERE [SoftDelete]=0";
            int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
            return count;
        }
    }
    public JobVehicle UpdateJobVehicle(JobVehicle AvaChemJobVehicle)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_JobVehicle] SET [JobID]=@JobID,[VehicleID]=@VehicleID,[UpdatedDate]=@UpdatedDate,[SoftDelete]=@SoftDelete WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", AvaChemJobVehicle.ID == 0 ? 0 : AvaChemJobVehicle.ID);
            sqlCommand.Parameters.AddWithValue("@JobID", AvaChemJobVehicle.JobID == 0 ? 0 : AvaChemJobVehicle.JobID);
            sqlCommand.Parameters.AddWithValue("@VehicleID", AvaChemJobVehicle.VehicleID == 0 ? 0 : AvaChemJobVehicle.VehicleID);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemJobVehicle.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return AvaChemJobVehicle;
        }
    }
    public void UpdateJobVehicleSoftDelete(int ID)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE[dbo].[AvaChem_JobVehicle] SET [SoftDelete]=1,[UpdatedDate]=@UpdatedDate WHERE [ID]=@ID";
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
            sqlCommand.CommandText = "DELETE FROM [dbo].[AvaChem_JobVehicle] WHERE [ID]=@ID";
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