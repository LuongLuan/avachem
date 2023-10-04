using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
public class StatusDataConnector
{
    public StatusDataConnector()
    {
    }
    public Status CreateStatus(Status AvaChemStatus)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "INSERT INTO [dbo].[AvaChem_Status]([Name],[CName],[SoftDelete],[Color])Values(@Name,@CName,@SoftDelete,@Color)";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@Name", AvaChemStatus.Name ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@CName", AvaChemStatus.CName ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemStatus.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Color", AvaChemStatus.Color ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return GetStatusByLastInsertedID();
        }
    }
    public Status GetStatusByFirstVariable(int ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[Name],[CName],[SoftDelete],[Color] FROM [dbo].[AvaChem_Status] WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new Status()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Name = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    CName = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    SoftDelete = Convert.ToBoolean(sdr[3].ToString() ?? DBNull.Value.ToString()),
                    Color = sdr[4].ToString() ?? DBNull.Value.ToString()
                };
            }
            return null;
        }
    }
    public Status GetStatusByLastInsertedID()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[Name],[CName],[SoftDelete],[Color] FROM [dbo].[AvaChem_Status] WHERE [ID]=(SELECT MAX(ID) FROM [AvaChem_Status]) AND [SoftDelete]=0";
            sqlCommand.Parameters.Clear();
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new Status()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Name = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    CName = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    SoftDelete = Convert.ToBoolean(sdr[3].ToString() ?? DBNull.Value.ToString()),
                    Color = sdr[4].ToString() ?? DBNull.Value.ToString()
                };
            }
            return null;
        }
    }
    public ICollection<Status> GetAll()
    {
        List<Status> finalListToReturn = new List<Status>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[Name],[CName],[SoftDelete],[Color] FROM [dbo].[AvaChem_Status] WHERE [SoftDelete]=0";
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                Status dl = new Status()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Name = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    CName = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    SoftDelete = Convert.ToBoolean(sdr[3].ToString() ?? DBNull.Value.ToString()),
                    Color = sdr[4].ToString() ?? DBNull.Value.ToString()
                };
                finalListToReturn.Add(dl);
            }
            return finalListToReturn;
        }
    }
    public Status GetStatusByName(string name)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[Name],[CName],[SoftDelete],[Color] FROM [dbo].[AvaChem_Status] WHERE [Name]=@Name";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@Name", name);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new Status()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Name = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    CName = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    SoftDelete = Convert.ToBoolean(sdr[3].ToString() ?? DBNull.Value.ToString()),
                    Color = sdr[4].ToString() ?? DBNull.Value.ToString()
                };
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
            sqlCommand.CommandText = "SELECT COUNT(*) FROM [dbo].[AvaChem_Status]";
            int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
            return count;
        }
    }
    public Status UpdateStatus(Status AvaChemStatus)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_Status]SET[Name]=@Name,[CName]=@CName,[SoftDelete]=@SoftDelete,[Color]=@Color WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@Name", AvaChemStatus.Name ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@CName", AvaChemStatus.CName ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemStatus.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Color", AvaChemStatus.Color ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return AvaChemStatus;
        }
    }
    public void UpdateStatusSoftDelete(int ID)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE[dbo].[AvaChem_Status] SET [SoftDelete]=1 WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
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
            sqlCommand.CommandText = "DELETE FROM [dbo].[AvaChem_Status] WHERE [ID]=@ID";
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