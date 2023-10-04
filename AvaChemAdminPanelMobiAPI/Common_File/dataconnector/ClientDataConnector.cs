using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
public class ClientDataConnector
{
    public ClientDataConnector()
    {
    }
    public Client CreateClient(Client AvaChemClient)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "INSERT INTO [dbo].[AvaChem_Client]([CompanyName],[ContactNamePrimary],[ContactDetailsPrimary],[ContactDetailsSecondary],[ContactNameSecondary],[SoftDelete],[CreatedDate],[UpdatedDate],[Location])Values(@CompanyName,@ContactNamePrimary,@ContactDetailsPrimary,@ContactDetailsSecondary,@ContactNameSecondary,@SoftDelete,@CreatedDate,@UpdatedDate,@Location)";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", AvaChemClient.ID == 0 ? 0 : AvaChemClient.ID);
            sqlCommand.Parameters.AddWithValue("@CompanyName", AvaChemClient.CompanyName ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@ContactNamePrimary", AvaChemClient.ContactNamePrimary ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@ContactDetailsPrimary", AvaChemClient.ContactDetailsPrimary ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@ContactDetailsSecondary", AvaChemClient.ContactDetailsSecondary ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@ContactNameSecondary", AvaChemClient.ContactNameSecondary ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemClient.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@Location", AvaChemClient.Location ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return GetClientByLastInsertedID();
        }
    }
    public Client GetClientByFirstVariable(int ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT [ID],[CompanyName],[ContactNamePrimary],[ContactDetailsPrimary],[ContactDetailsSecondary],[ContactNameSecondary],[SoftDelete],[CreatedDate],[UpdatedDate],[Location] FROM [dbo].[AvaChem_Client] WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new Client()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    CompanyName = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    ContactNamePrimary = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    ContactDetailsPrimary = sdr[3].ToString() ?? DBNull.Value.ToString(),
                    ContactDetailsSecondary = sdr[4].ToString() ?? DBNull.Value.ToString(),
                    ContactNameSecondary = sdr[5].ToString() ?? DBNull.Value.ToString(),
                    SoftDelete = Convert.ToBoolean(sdr[6].ToString() ?? DBNull.Value.ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[7].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[8].ToString() ?? DBNull.Value.ToString()),
                    Location = sdr[9].ToString() ?? DBNull.Value.ToString(),
                };
            }
            return null;
        }
    }
    public Client GetClientByLastInsertedID()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[CompanyName],[ContactNamePrimary],[ContactDetailsPrimary],[ContactDetailsSecondary],[ContactNameSecondary],[SoftDelete],[CreatedDate],[UpdatedDate],[Location] FROM [dbo].[AvaChem_Client] WHERE [ID]=(SELECT MAX(ID) FROM [AvaChem_Client]) AND [SoftDelete]=0";
            sqlCommand.Parameters.Clear();
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new Client()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    CompanyName = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    ContactNamePrimary = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    ContactDetailsPrimary = sdr[3].ToString() ?? DBNull.Value.ToString(),
                    ContactDetailsSecondary = sdr[4].ToString() ?? DBNull.Value.ToString(),
                    ContactNameSecondary = sdr[5].ToString() ?? DBNull.Value.ToString(),
                    SoftDelete = Convert.ToBoolean(sdr[6].ToString() ?? DBNull.Value.ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[7].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[8].ToString() ?? DBNull.Value.ToString()),
                    Location = sdr[9].ToString() ?? DBNull.Value.ToString(),
                };
            }
            return null;
        }
    }
    public List<Client> GetAll(int? page = null, int? per_page = null, string search = "")
    {
        List<Client> finalListToReturn = new List<Client>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT [ID],[CompanyName],[ContactNamePrimary],[ContactDetailsPrimary],[ContactDetailsSecondary],[ContactNameSecondary],[SoftDelete],[CreatedDate],[UpdatedDate],[Location] " +
                                        "FROM [dbo].[AvaChem_Client] WHERE [SoftDelete]=0 " +
                                        (search + "" == "" ? ""
                                            : "AND ( [CompanyName] Like @Search " +
                                                                  "OR [ContactNamePrimary] Like @Search " +
                                                                  "OR [ContactDetailsPrimary] Like @Search " +
                                                                  "OR [ContactDetailsSecondary] Like @Search " +
                                                                  "OR [ContactNameSecondary] Like @Search ) ") +
                                         ((page + "" == "" || per_page + "" == "") ? "ORDER BY [ID] DESC" : "ORDER BY [ID] DESC OFFSET (@Offset) ROWS FETCH NEXT @PerPage ROWS ONLY");
            sqlCommand.Parameters.Clear();
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
                Client dl = new Client()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    CompanyName = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    ContactNamePrimary = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    ContactDetailsPrimary = sdr[3].ToString() ?? DBNull.Value.ToString(),
                    ContactDetailsSecondary = sdr[4].ToString() ?? DBNull.Value.ToString(),
                    ContactNameSecondary = sdr[5].ToString() ?? DBNull.Value.ToString(),
                    SoftDelete = Convert.ToBoolean(sdr[6].ToString() ?? DBNull.Value.ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[7].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[8].ToString() ?? DBNull.Value.ToString()),
                    Location = sdr[9].ToString() ?? DBNull.Value.ToString(),
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
            sqlCommand.CommandText = "SELECT COUNT(*) FROM [dbo].[AvaChem_Client] WHERE [SoftDelete]=0";
            int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
            return count;
        }
    }
    public Client UpdateClient(Client AvaChemClient)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_Client] SET [CompanyName]=@CompanyName,[ContactNamePrimary]=@ContactNamePrimary,[ContactDetailsPrimary]=@ContactDetailsPrimary,[ContactDetailsSecondary]=@ContactDetailsSecondary,[ContactNameSecondary]=@ContactNameSecondary,[SoftDelete]=@SoftDelete,[UpdatedDate]=@UpdatedDate,[Location]=@Location WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", AvaChemClient.ID == 0 ? 0 : AvaChemClient.ID);
            sqlCommand.Parameters.AddWithValue("@CompanyName", AvaChemClient.CompanyName ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@ContactNamePrimary", AvaChemClient.ContactNamePrimary ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@ContactDetailsPrimary", AvaChemClient.ContactDetailsPrimary ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@ContactDetailsSecondary", AvaChemClient.ContactDetailsSecondary ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@ContactNameSecondary", AvaChemClient.ContactNameSecondary ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemClient.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@Location", AvaChemClient.Location ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return AvaChemClient;
        }
    }
    public void UpdateClientSoftDelete(int ID)
    {

        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_Client] SET [SoftDelete]=1,[UpdatedDate]=@UpdatedDate WHERE [ID]=@ID";
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
            sqlCommand.CommandText = "DELETE FROM [dbo].[AvaChem_Client] WHERE [ID]=@ID";
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