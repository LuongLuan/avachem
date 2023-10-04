using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
public class LogDataConnector
{
    public LogDataConnector()
    {
    }
    public Log CreateLog(Log AvaChemLog)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "INSERT INTO [dbo].[AvaChem_Log]([ActionName],[ActionLocation],[ActionType],[ActionMeta],[ByUserID],[CreatedDate],[SoftDelete])Values(@ActionName,@ActionLocation,@ActionType,@ActionMeta,@ByUserID,@CreatedDate,@SoftDelete)";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ActionName", AvaChemLog.ActionName ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@ActionLocation", AvaChemLog.ActionLocation ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@ActionType", AvaChemLog.ActionType == 0 ? 0 : AvaChemLog.ActionType);
            sqlCommand.Parameters.AddWithValue("@ActionMeta", AvaChemLog.ActionMeta ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@ByUserID", AvaChemLog.ByUserID == 0 ? 0 : AvaChemLog.ByUserID);
            sqlCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemLog.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return GetLogByLastInsertedID();
        }
    }
    public Log GetLogByFirstVariable(int ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[ActionName],[ActionLocation],[ActionType],[ActionMeta],[ByUserID],[CreatedDate],[SoftDelete] FROM [dbo].[AvaChem_Log] WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new Log()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    ActionName = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    ActionLocation = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    ActionType = Convert.ToInt32(sdr[3].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[3].ToString()),
                    ActionMeta = sdr[4].ToString() ?? DBNull.Value.ToString(),
                    ByUserID = Convert.ToInt32(sdr[5].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[5].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[6].ToString().ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[7].ToString() ?? DBNull.Value.ToString())
                };
            }
            return null;
        }
    }
    public Log GetLogByLastInsertedID()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT * FROM [dbo].[AvaChem_Log] WHERE [ID]=(SELECT MAX(ID) FROM [AvaChem_Log]) AND [SoftDelete]=0";
            sqlCommand.Parameters.Clear();
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new Log()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    ActionName = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    ActionLocation = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    ActionType = Convert.ToInt32(sdr[3].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[3].ToString()),
                    ActionMeta = sdr[4].ToString() ?? DBNull.Value.ToString(),
                    ByUserID = Convert.ToInt32(sdr[5].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[5].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[6].ToString().ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[7].ToString() ?? DBNull.Value.ToString())
                };
            }
            return null;
        }
    }
    public ICollection<Log> GetAll()
    {
        List<Log> finalListToReturn = new List<Log>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[ActionName],[ActionLocation],[ActionType],[ActionMeta],[ByUserID],[CreatedDate],[SoftDelete]FROM [dbo].[AvaChem_Log]";
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                Log dl = new Log()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    ActionName = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    ActionLocation = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    ActionType = Convert.ToInt32(sdr[3].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[3].ToString()),
                    ActionMeta = sdr[4].ToString() ?? DBNull.Value.ToString(),
                    ByUserID = Convert.ToInt32(sdr[5].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[5].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[6].ToString().ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[7].ToString() ?? DBNull.Value.ToString())
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
            sqlCommand.CommandText = "SELECT COUNT(*) FROM [dbo].[AvaChem_Log]";
            int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
            return count;
        }
    }
    public Log UpdateLog(Log AvaChemLog)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_Log]SET[ActionName]=@ActionName,[ActionLocation]=@ActionLocation,[ActionType]=@ActionType,[ActionMeta]=@ActionMeta,[ByUserID]=@ByUserID,[SoftDelete]=@SoftDelete WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", AvaChemLog.ID == 0 ? 0 : AvaChemLog.ID);
            sqlCommand.Parameters.AddWithValue("@ActionName", AvaChemLog.ActionName ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@ActionLocation", AvaChemLog.ActionLocation ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@ActionType", AvaChemLog.ActionType == 0 ? 0 : AvaChemLog.ActionType);
            sqlCommand.Parameters.AddWithValue("@ActionMeta", AvaChemLog.ActionMeta ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@ByUserID", AvaChemLog.ByUserID == 0 ? 0 : AvaChemLog.ByUserID);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemLog.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return AvaChemLog;
        }
    }
    public void UpdateLogSoftDelete(int ID)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE[dbo].[AvaChem_Log] SET [SoftDelete]=1 WHERE [ID]=@ID";
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
            sqlCommand.CommandText = "DELETE FROM [dbo].[AvaChem_Log] WHERE [ID]=@ID";
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