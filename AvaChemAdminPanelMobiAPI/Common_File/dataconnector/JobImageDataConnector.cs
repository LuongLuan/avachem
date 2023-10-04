using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
public class JobImageDataConnector
{
    public JobImageDataConnector()
    {
    }
    public JobImage CreateJobImage(JobImage AvaChemJobImage)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "INSERT INTO [dbo].[AvaChem_JobImage]([ImageUrl],[Type],[TripID],[SoftDelete])Values(@ImageUrl,@Type,@TripID,@SoftDelete)";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", AvaChemJobImage.ID == 0 ? 0 : AvaChemJobImage.ID);
            sqlCommand.Parameters.AddWithValue("@ImageUrl", AvaChemJobImage.ImageUrl ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Type", AvaChemJobImage.Type ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@TripID", AvaChemJobImage.TripID == 0 ? 0 : AvaChemJobImage.TripID);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemJobImage.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return GetJobImageByLastInsertedID();
        }
    }
    public JobImage GetJobImageByFirstVariable(int ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[ImageUrl],[Type],[TripID],[SoftDelete] FROM [dbo].[AvaChem_JobImage] WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new JobImage()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    ImageUrl = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    Type = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    TripID = Convert.ToInt32(sdr[3].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[3].ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[4].ToString() ?? DBNull.Value.ToString())
                };
            }
            return null;
        }
    }
    public JobImage GetJobImageByLastInsertedID()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[ImageUrl],[Type],[TripID],[SoftDelete] FROM [dbo].[AvaChem_JobImage] WHERE [ID]=(SELECT MAX(ID) FROM [AvaChem_JobImage]) AND [SoftDelete]=0";
            sqlCommand.Parameters.Clear();
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new JobImage()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    ImageUrl = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    Type = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    TripID = Convert.ToInt32(sdr[3].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[3].ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[4].ToString() ?? DBNull.Value.ToString())
                };
            }
            return null;
        }
    }
    public ICollection<JobImage> GetAll()
    {
        List<JobImage> finalListToReturn = new List<JobImage>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[ImageUrl],[Type],[TripID],[SoftDelete]FROM [dbo].[AvaChem_JobImage]";
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                JobImage dl = new JobImage()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    ImageUrl = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    Type = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    TripID = Convert.ToInt32(sdr[3].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[3].ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[4].ToString() ?? DBNull.Value.ToString())
                };
                finalListToReturn.Add(dl);
            }
            return finalListToReturn;
        }
    }
    public ICollection<JobImage> GetByTripID(int TripID)
    {
        List<JobImage> finalListToReturn = new List<JobImage>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[ImageUrl],[Type],[TripID],[SoftDelete]FROM [dbo].[AvaChem_JobImage] WHERE [TripID]=@TripID AND [SoftDelete]=0";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@TripID", TripID);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                JobImage dl = new JobImage()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    ImageUrl = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    Type = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    TripID = Convert.ToInt32(sdr[3].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[3].ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[4].ToString() ?? DBNull.Value.ToString())
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
            sqlCommand.CommandText = "SELECT COUNT(*) FROM [dbo].[AvaChem_JobImage]";
            int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
            return count;
        }
    }
    public JobImage UpdateJobImage(JobImage AvaChemJobImage)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_JobImage] SET [ImageUrl]=@ImageUrl,[Type]=@Type,[TripID]=@TripID,[SoftDelete]=@SoftDelete WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", AvaChemJobImage.ID == 0 ? 0 : AvaChemJobImage.ID);
            sqlCommand.Parameters.AddWithValue("@ImageUrl", AvaChemJobImage.ImageUrl ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Type", AvaChemJobImage.Type ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@TripID", AvaChemJobImage.TripID == 0 ? 0 : AvaChemJobImage.TripID);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemJobImage.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return AvaChemJobImage;
        }
    }
    public JobImage UpdateJobImageSoftDelete(int ID)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE[dbo].[AvaChem_JobImage] SET [SoftDelete]=1 WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
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
            sqlCommand.CommandText = "DELETE FROM [dbo].[AvaChem_JobImage] WHERE [ID]=@ID";
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