using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
public class JobDescriptionDataConnector
{
    public JobDescriptionDataConnector()
    {
    }
    public JobDescription CreateJobDescription(JobDescription AvaChemJobDescription)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "INSERT INTO [dbo].[AvaChem_JobDescription]([JobID],[DescriptionID],[CreatedDate],[UpdatedDate],[SoftDelete])Values(@JobID,@DescriptionID,@CreatedDate,@UpdatedDate,@SoftDelete)";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@JobID", AvaChemJobDescription.JobID == 0 ? 0 : AvaChemJobDescription.JobID);
            sqlCommand.Parameters.AddWithValue("@DescriptionID", AvaChemJobDescription.DescriptionID == 0 ? 0 : AvaChemJobDescription.DescriptionID);
            sqlCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemJobDescription.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return GetJobDescriptionByLastInsertedID();
        }
    }
    public JobDescription GetJobDescriptionByFirstVariable(int ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[JobID],[DescriptionID],[CreatedDate],[UpdatedDate],[SoftDelete] FROM [dbo].[AvaChem_JobDescription] WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new JobDescription()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    JobID = Convert.ToInt32(sdr[1].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[1].ToString()),
                    DescriptionID = Convert.ToInt32(sdr[2].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[2].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[3].ToString().ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[4].ToString().ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[5].ToString() ?? DBNull.Value.ToString())
                };
            }
            return null;
        }
    }
    public JobDescription GetJobDescriptionByLastInsertedID()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT * FROM [dbo].[AvaChem_JobDescription] WHERE [ID]=(SELECT MAX(ID) FROM [AvaChem_JobDescription]) AND [SoftDelete]=0";
            sqlCommand.Parameters.Clear();
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new JobDescription()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    JobID = Convert.ToInt32(sdr[1].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[1].ToString()),
                    DescriptionID = Convert.ToInt32(sdr[2].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[2].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[3].ToString().ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[4].ToString().ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[5].ToString() ?? DBNull.Value.ToString())
                };
            }
            return null;
        }
    }
    public ICollection<JobDescription> GetAll()
    {
        List<JobDescription> finalListToReturn = new List<JobDescription>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[JobID],[DescriptionID],[CreatedDate],[UpdatedDate],[SoftDelete]FROM [dbo].[AvaChem_JobDescription] WHERE [SoftDelete]=0";
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                JobDescription dl = new JobDescription()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    JobID = Convert.ToInt32(sdr[1].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[1].ToString()),
                    DescriptionID = Convert.ToInt32(sdr[2].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[2].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[3].ToString().ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[4].ToString().ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[5].ToString() ?? DBNull.Value.ToString())
                };
                finalListToReturn.Add(dl);
            }
            return finalListToReturn;
        }
    }
    public ICollection<JobDescriptionWithContent> GetByJobID(int jobID)
    {
        List<JobDescriptionWithContent> finalListToReturn = new List<JobDescriptionWithContent>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT jd.[ID],jd.[JobID],jd.[DescriptionID],jd.[CreatedDate],jd.[UpdatedDate],jd.[SoftDelete],d.[Content] " +
                                    "FROM [dbo].[AvaChem_JobDescription] AS jd " +
                                    "LEFT JOIN [dbo].[AvaChem_Description] AS d ON jd.[DescriptionID] = d.[ID] " +
                                    "WHERE jd.[SoftDelete]=0 AND jd.[JobID]=@JobID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@JobID", jobID);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                JobDescriptionWithContent dl = new JobDescriptionWithContent()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    JobID = Convert.ToInt32(sdr[1].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[1].ToString()),
                    DescriptionID = Convert.ToInt32(sdr[2].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[2].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[3].ToString().ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[4].ToString().ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[5].ToString() ?? DBNull.Value.ToString()),
                    Content = sdr[6].ToString() ?? DBNull.Value.ToString(),
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
            sqlCommand.CommandText = "SELECT COUNT(*) FROM [dbo].[AvaChem_JobDescription] WHERE [SoftDelete]=0";
            int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
            return count;
        }
    }
    public JobDescription UpdateJobDescription(JobDescription AvaChemJobDescription)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_JobDescription] SET [JobID]=@JobID,[DescriptionID]=@DescriptionID,[UpdatedDate]=@UpdatedDate,[SoftDelete]=@SoftDelete WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@JobID", AvaChemJobDescription.JobID == 0 ? 0 : AvaChemJobDescription.JobID);
            sqlCommand.Parameters.AddWithValue("@DescriptionID", AvaChemJobDescription.DescriptionID == 0 ? 0 : AvaChemJobDescription.DescriptionID);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemJobDescription.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return AvaChemJobDescription;
        }
    }
    public JobDescription UpdateJobDescriptionSoftDelete(int ID)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE[dbo].[AvaChem_JobDescription] SET [SoftDelete]=1,[UpdatedDate]=@UpdatedDate WHERE [ID]=@ID"; sqlCommand.Parameters.Clear();
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
            sqlCommand.CommandText = "DELETE FROM [dbo].[AvaChem_JobDescription] WHERE [ID]=@ID";
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