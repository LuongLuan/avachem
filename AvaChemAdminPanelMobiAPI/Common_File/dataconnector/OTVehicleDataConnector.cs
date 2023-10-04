using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
public class OTVehicleDataConnector
{
    public OTVehicleDataConnector()
    {
    }
    public OTVehicle CreateOTVehicle(OTVehicle AvaChemOTVehicle)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "INSERT INTO [dbo].[AvaChem_OTVehicle]([OT_ID],[VehicleID],[SoftDelete])Values(@OT_ID,@VehicleID,@SoftDelete)";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@OT_ID", AvaChemOTVehicle.OT_ID);
            sqlCommand.Parameters.AddWithValue("@VehicleID", AvaChemOTVehicle.VehicleID);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemOTVehicle.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return GetOTVehicleByLastInsertedID();
        }
    }
    public OTVehicle GetOTVehicleByFirstVariable(int ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[OT_ID],[VehicleID],[SoftDelete] FROM [dbo].[AvaChem_OTVehicle] WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new OTVehicle()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    OT_ID = Convert.ToInt32(sdr[1].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[1].ToString()),
                    VehicleID = Convert.ToInt32(sdr[2].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[2].ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[3].ToString() ?? DBNull.Value.ToString())
                };
            }
            return null;
        }
    }
    public OTVehicle GetOTVehicleByLastInsertedID()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[OT_ID],[VehicleID],[SoftDelete] FROM [dbo].[AvaChem_OTVehicle] WHERE [ID]=(SELECT MAX(ID) FROM [AvaChem_OTVehicle]) AND [SoftDelete]=0";
            sqlCommand.Parameters.Clear();
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new OTVehicle()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    OT_ID = Convert.ToInt32(sdr[1].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[1].ToString()),
                    VehicleID = Convert.ToInt32(sdr[2].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[2].ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[3].ToString() ?? DBNull.Value.ToString())
                };
            }
            return null;
        }
    }
    public ICollection<OTVehicle> GetAll()
    {
        List<OTVehicle> finalListToReturn = new List<OTVehicle>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT [ID],[OT_ID],[VehicleID],[SoftDelete] FROM [dbo].[AvaChem_OTVehicle] WHERE [SoftDelete]=0";
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                OTVehicle dl = new OTVehicle()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    OT_ID = Convert.ToInt32(sdr[1].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[1].ToString()),
                    VehicleID = Convert.ToInt32(sdr[2].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[2].ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[3].ToString() ?? DBNull.Value.ToString())
                };
                finalListToReturn.Add(dl);
            }
            return finalListToReturn;
        }
    }
    public ICollection<VehicleWithOTVehicleDTO> GetVehiclesByParams(int OT_ID, int? page = null, int? per_page = null)
    {
        List<VehicleWithOTVehicleDTO> finalListToReturn = new List<VehicleWithOTVehicleDTO>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT v.[ID],v.[Model],v.[Number],v.[SoftDelete],v.[CreatedDate],v.[UpdatedDate],oV.[ID] AS oVID " +
                                        "FROM [AvaChem_OTVehicle] AS oV " +
                                        "LEFT JOIN [AvaChem_Vehicle] AS v ON oV.[VehicleID]=v.[ID] " +
                                        "WHERE oV.[SoftDelete]=0 AND v.[SoftDelete]=0 AND oV.[OT_ID]=@OT_ID " +
                                        ((page + "" == "" || per_page + "" == "") ? "ORDER BY v.[ID] DESC" : "ORDER BY v.[ID] DESC OFFSET (@Offset) ROWS FETCH NEXT @PerPage ROWS ONLY");
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@OT_ID", OT_ID);
            if (page + "" != "" && per_page + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@Offset", (page - 1) * per_page);
                sqlCommand.Parameters.AddWithValue("@PerPage", per_page);
            }
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                VehicleWithOTVehicleDTO dl = new VehicleWithOTVehicleDTO()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Model = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    Number = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    SoftDelete = Convert.ToBoolean(sdr[3].ToString() ?? DBNull.Value.ToString()),
                    //CreatedDate = Convert.ToDateTime(sdr[4].ToString().ToString() ?? DBNull.Value.ToString()),
                    //UpdatedDate = Convert.ToDateTime(sdr[5].ToString().ToString() ?? DBNull.Value.ToString()),
                    OTVehicleID = Convert.ToInt32(sdr[6].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[6].ToString()),
                };
                finalListToReturn.Add(dl);
            }
            return finalListToReturn;
        }
    }
    public int CountAll()
    {
        List<OTVehicle> finalListToReturn = new List<OTVehicle>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT COUNT(*) FROM [dbo].[AvaChem_OTVehicle] WHERE [SoftDelete]=0";
            int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
            return count;
        }
    }
    public OTVehicle UpdateOTVehicle(OTVehicle AvaChemOTVehicle)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_OTVehicle] SET [OT_ID]=@OT_ID,[VehicleID]=@VehicleID,[SoftDelete]=@SoftDelete WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", AvaChemOTVehicle.ID);
            sqlCommand.Parameters.AddWithValue("@OT_ID", AvaChemOTVehicle.OT_ID);
            sqlCommand.Parameters.AddWithValue("@VehicleID", AvaChemOTVehicle.VehicleID);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemOTVehicle.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return AvaChemOTVehicle;
        }
    }
    public void UpdateOTVehicleSoftDelete(int ID)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE[dbo].[AvaChem_OTVehicle] SET [SoftDelete]=1 WHERE [ID]=@ID";
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
            sqlCommand.CommandText = "DELETE FROM [dbo].[AvaChem_OTVehicle] WHERE [ID]=@ID";
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