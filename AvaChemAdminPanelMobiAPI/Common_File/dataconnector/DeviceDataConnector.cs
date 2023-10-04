using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
public class DeviceDataConnector
{
    public DeviceDataConnector()
    {
    }
    public Device CreateDevice(Device AvaChemDevice)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "INSERT INTO [dbo].[AvaChem_Device]([FCMToken],[DevID],[Model],[NumFailedNotif],[CreatedDate],[UpdatedDate],[SoftDelete],[UserID])Values(@FCMToken,@DevID,@Model,@NumFailedNotif,@CreatedDate,@UpdatedDate,@SoftDelete,@UserID)";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@FCMToken", AvaChemDevice.FCMToken ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@DevID", AvaChemDevice.DevID ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Model", AvaChemDevice.Model ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@NumFailedNotif", AvaChemDevice.NumFailedNotif == 0 ? 0 : AvaChemDevice.NumFailedNotif);
            sqlCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemDevice.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@UserID", AvaChemDevice.UserID == 0 ? 0 : AvaChemDevice.UserID);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return GetDeviceByLastInsertedID();
        }
    }
    public Device GetDeviceByFirstVariable(int ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[FCMToken],[DevID],[Model],[NumFailedNotif],[CreatedDate],[UpdatedDate],[SoftDelete],[UserID] FROM [dbo].[AvaChem_Device] WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new Device()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    FCMToken = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    DevID = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    Model = sdr[3].ToString() ?? DBNull.Value.ToString(),
                    NumFailedNotif = Convert.ToInt32(sdr[4].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[4].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[5].ToString().ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[6].ToString().ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[7].ToString() ?? DBNull.Value.ToString()),
                    UserID = Convert.ToInt32(sdr[8].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[8].ToString()),
                };
            }
            return null;
        }
    }
    public Device GetDeviceByLastInsertedID()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[FCMToken],[DevID],[Model],[NumFailedNotif],[CreatedDate],[UpdatedDate],[SoftDelete],[UserID] FROM [dbo].[AvaChem_Device] WHERE [ID]=(SELECT MAX(ID) FROM [AvaChem_Device]) AND [SoftDelete]=0";
            sqlCommand.Parameters.Clear();
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new Device()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    FCMToken = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    DevID = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    Model = sdr[3].ToString() ?? DBNull.Value.ToString(),
                    NumFailedNotif = Convert.ToInt32(sdr[4].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[4].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[5].ToString().ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[6].ToString().ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[7].ToString() ?? DBNull.Value.ToString()),
                    UserID = Convert.ToInt32(sdr[8].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[8].ToString())
                };
            }
            return null;
        }
    }
    public List<Device> GetAll(int? uid = null)
    {
        List<Device> finalListToReturn = new List<Device>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT [ID],[FCMToken],[DevID],[Model],[NumFailedNotif],[CreatedDate],[UpdatedDate],[SoftDelete],[UserID] " +
                                        "FROM [dbo].[AvaChem_Device] WHERE [SoftDelete]=0 " +
                                        (uid + "" == "" ? "" : "AND [UserID]=@UserID AND [NumFailedNotif]<2 AND ([FCMToken] IS NOT NULL AND [FCMToken]!='')");
            sqlCommand.Parameters.Clear();
            if (uid + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@UserID", uid);
            }
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                Device dl = new Device()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    FCMToken = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    DevID = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    Model = sdr[3].ToString() ?? DBNull.Value.ToString(),
                    NumFailedNotif = Convert.ToInt32(sdr[4].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[4].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[5].ToString().ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[6].ToString().ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[7].ToString() ?? DBNull.Value.ToString()),
                    UserID = Convert.ToInt32(sdr[8].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[8].ToString()),
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
            sqlCommand.CommandText = "SELECT COUNT(*) FROM [dbo].[AvaChem_Device] WHERE [SoftDelete]=0";
            int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
            return count;
        }
    }
    public Device UpdateDevice(Device AvaChemDevice)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_Device] SET [FCMToken]=@FCMToken,[DevID]=@DevID,[Model]=@Model,[NumFailedNotif]=@NumFailedNotif,[UpdatedDate]=@UpdatedDate,[SoftDelete]=@SoftDelete,[UserID]=@UserID WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", AvaChemDevice.ID == 0 ? 0 : AvaChemDevice.ID);
            sqlCommand.Parameters.AddWithValue("@FCMToken", AvaChemDevice.FCMToken ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@DevID", AvaChemDevice.DevID ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Model", AvaChemDevice.Model ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@NumFailedNotif", AvaChemDevice.NumFailedNotif == 0 ? 0 : AvaChemDevice.NumFailedNotif);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemDevice.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@UserID", AvaChemDevice.UserID == 0 ? 0 : AvaChemDevice.UserID);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return AvaChemDevice;
        }
    }
    public void UpdateNumFailedNotif(int ID, int numFailedNotif, UpdateNumFailedNotifType type)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_Device] SET [UpdatedDate]=@UpdatedDate" +
                                        (type.ToString() == UpdateNumFailedNotifType.Overwrite.ToString()
                                                            ? ",[NumFailedNotif]=@NumFailedNotif"
                                                            : type.ToString() == UpdateNumFailedNotifType.Increase.ToString()
                                                            ? ",[NumFailedNotif]+=@NumFailedNotif"
                                                            : "")
                                        + " WHERE [ID]=@ID";

            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@NumFailedNotif", numFailedNotif);
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

            Device dev = GetDeviceByFirstVariable(ID);
            if (dev != null && dev.NumFailedNotif > 1) UpdateDeviceSoftDelete(dev.ID);
        }
    }
    public void UpdateDeviceSoftDelete(int ID)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_Device] SET [SoftDelete]=1,[UpdatedDate]=@UpdatedDate WHERE [ID]=@ID";
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
            sqlCommand.CommandText = "DELETE FROM [dbo].[AvaChem_Device] WHERE [ID]=@ID";
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

    public bool CheckFCMExists(int uid, string fcm)
    {
        int count = 0;
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT Count([UserID]) as CountUserID FROM [dbo].[AvaChem_Device] WHERE [SoftDelete]=0 AND [UserID]=@UserID AND [FCMToken]=@FCMToken";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@UserID", uid);
            sqlCommand.Parameters.AddWithValue("@FCMToken", fcm);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                count = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString());
            }
        }
        return count > 0;
    }
}
public enum UpdateNumFailedNotifType
{
    Increase = 1,
    Overwrite = 2
}