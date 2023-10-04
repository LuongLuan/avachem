using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

public class DescriptionDataConnector
{
    public DescriptionDataConnector()
    {
    }
    public Description CreateDescription(Description AvaChemDescription)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "INSERT INTO [dbo].[AvaChem_Description]([Content],[SoftDelete],[CreatedDate],[UpdatedDate])Values(@Content,@SoftDelete,@CreatedDate,@UpdatedDate)";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@Content", AvaChemDescription.Content ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemDescription.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return GetDescriptionByLastInsertedID();
        }
    }
    public Description GetDescriptionByFirstVariable(int ID)
    {
        ArrayList finalListToReturn = new ArrayList();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[Content],[SoftDelete],[CreatedDate],[UpdatedDate] FROM [dbo].[AvaChem_Description] WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new Description()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Content = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    SoftDelete = Convert.ToBoolean(sdr[2].ToString() ?? DBNull.Value.ToString()),
                    //CreatedDate = Convert.ToDateTime(sdr[3].ToString().ToString() ?? DBNull.Value.ToString()),
                    //UpdatedDate = Convert.ToDateTime(sdr[4].ToString().ToString() ?? DBNull.Value.ToString())
                };
            }
            return null;
        }
    }
    public Description GetDescriptionByLastInsertedID()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[Content],[SoftDelete],[CreatedDate],[UpdatedDate] FROM [dbo].[AvaChem_Description] WHERE [ID]=(SELECT MAX(ID) FROM [AvaChem_Description]) AND [SoftDelete]=0";
            sqlCommand.Parameters.Clear();
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new Description()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Content = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    SoftDelete = Convert.ToBoolean(sdr[2].ToString() ?? DBNull.Value.ToString()),
                    //CreatedDate = Convert.ToDateTime(sdr[3].ToString().ToString() ?? DBNull.Value.ToString()),
                    //UpdatedDate = Convert.ToDateTime(sdr[4].ToString().ToString() ?? DBNull.Value.ToString())
                };
            }
            return null;
        }
    }
    public ICollection<Description> GetAll()
    {
        List<Description> finalListToReturn = new List<Description>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[Content],[SoftDelete],[CreatedDate],[UpdatedDate] FROM [dbo].[AvaChem_Description] WHERE [SoftDelete]=0";
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                Description dl = new Description()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Content = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    SoftDelete = Convert.ToBoolean(sdr[2].ToString() ?? DBNull.Value.ToString()),
                    //CreatedDate = Convert.ToDateTime(sdr[3].ToString().ToString() ?? DBNull.Value.ToString()),
                    //UpdatedDate = Convert.ToDateTime(sdr[4].ToString().ToString() ?? DBNull.Value.ToString())
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
            sqlCommand.CommandText = "SELECT COUNT(*) FROM [dbo].[AvaChem_Description] WHERE [SoftDelete]=0";
            int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
            return count;
        }
    }
    public Description UpdateDescription(Description AvaChemDescription)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_Description] SET [Content]=@Content,[SoftDelete]=@SoftDelete,[UpdatedDate]=@UpdatedDate WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@Content", AvaChemDescription.Content ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemDescription.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return AvaChemDescription;
        }
    }
    public Description UpdateDescriptionSoftDelete(int ID)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE[dbo].[AvaChem_Description] SET [SoftDelete]=1,[UpdatedDate]=@UpdatedDate WHERE [ID]=@ID"; sqlCommand.Parameters.Clear();
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
            sqlCommand.CommandText = "DELETE FROM [dbo].[AvaChem_Description] WHERE [ID]=@ID";
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