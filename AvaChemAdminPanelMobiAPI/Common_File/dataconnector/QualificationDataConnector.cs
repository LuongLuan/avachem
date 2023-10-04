using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
public class QualificationDataConnector
{
    public QualificationDataConnector()
    {
    }
    public Qualification CreateQualification(Qualification AvaChemQualification)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "INSERT INTO [dbo].[AvaChem_Qualification]([Name],[DateObtained],[ExpiryDate],[UserID],[SoftDelete])Values(@Name,@DateObtained,@ExpiryDate,@UserID,@SoftDelete)";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@Name", AvaChemQualification.Name ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@DateObtained", AvaChemQualification.DateObtained.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@ExpiryDate", AvaChemQualification.ExpiryDate.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@UserID", AvaChemQualification.UserID == 0 ? 0 : AvaChemQualification.UserID);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemQualification.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return GetQualificationByLastInsertedID();
        }
    }
    public Qualification GetQualificationByFirstVariable(int ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[Name],[DateObtained],[ExpiryDate],[UserID],[SoftDelete] FROM [dbo].[AvaChem_Qualification] WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new Qualification()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Name = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    DateObtained = Convert.ToDateTime(sdr[2].ToString() ?? DBNull.Value.ToString()),
                    ExpiryDate = Convert.ToDateTime(sdr[3].ToString() ?? DBNull.Value.ToString()),
                    UserID = Convert.ToInt32(sdr[4].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[4].ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[5].ToString() ?? DBNull.Value.ToString())
                };
            }
            return null;
        }
    }
    public Qualification GetQualificationByLastInsertedID()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[Name],[DateObtained],[ExpiryDate],[UserID],[SoftDelete] FROM [dbo].[AvaChem_Qualification] WHERE [ID]=(SELECT MAX(ID) FROM [AvaChem_Qualification]) AND [SoftDelete]=0";
            sqlCommand.Parameters.Clear();
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new Qualification()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Name = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    DateObtained = Convert.ToDateTime(sdr[2].ToString() ?? DBNull.Value.ToString()),
                    ExpiryDate = Convert.ToDateTime(sdr[3].ToString() ?? DBNull.Value.ToString()),
                    UserID = Convert.ToInt32(sdr[4].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[4].ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[5].ToString() ?? DBNull.Value.ToString())
                };
            }
            return null;
        }
    }
    public ICollection<Qualification> GetAll()
    {
        List<Qualification> finalListToReturn = new List<Qualification>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[Name],[DateObtained],[ExpiryDate],[UserID],[SoftDelete]FROM [dbo].[AvaChem_Qualification] WHERE [SoftDelete]=0";
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                Qualification dl = new Qualification()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Name = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    DateObtained = Convert.ToDateTime(sdr[2].ToString() ?? DBNull.Value.ToString()),
                    ExpiryDate = Convert.ToDateTime(sdr[3].ToString() ?? DBNull.Value.ToString()),
                    UserID = Convert.ToInt32(sdr[4].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[4].ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[5].ToString() ?? DBNull.Value.ToString())
                };
                finalListToReturn.Add(dl);
            }
            return finalListToReturn;
        }
    }

    public List<QualificationWithStatus> GetByUserID(int uid)
    {
        List<QualificationWithStatus> finalListToReturn = new List<QualificationWithStatus>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT [ID],[Name],[DateObtained],[ExpiryDate],[UserID],[SoftDelete],DATEDIFF(DAY,GETDATE(),[ExpiryDate]) AS DaysLeft " +
                                        "FROM [dbo].[AvaChem_Qualification] WHERE [SoftDelete]=0 AND [UserID]=@UserID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@UserID", uid);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                QualificationWithStatus dl = new QualificationWithStatus()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Name = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    DateObtained = Convert.ToDateTime(sdr[2].ToString() ?? DBNull.Value.ToString()),
                    ExpiryDate = Convert.ToDateTime(sdr[3].ToString() ?? DBNull.Value.ToString()),
                    UserID = Convert.ToInt32(sdr[4].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[4].ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[5].ToString() ?? DBNull.Value.ToString()),
                    DaysLeft = Convert.ToInt32(sdr[6].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[6].ToString())
                };
                finalListToReturn.Add(dl);
            }
            return finalListToReturn;
        }
    }
    public List<QualificationWithStatus> GetExpires(int? uid = null)
    {
        List<QualificationWithStatus> finalListToReturn = new List<QualificationWithStatus>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT [ID],[Name],[DateObtained],[ExpiryDate],[UserID],[SoftDelete],DATEDIFF(DAY,GETDATE(),[ExpiryDate]) AS DaysLeft " +
                                        "FROM [dbo].[AvaChem_Qualification] WHERE [SoftDelete]=0 AND DATEDIFF(DAY,GETDATE(),[ExpiryDate])<=30 " +
                                        (uid + "" == "" ? "" : "AND [UserID]=@UserID");
            sqlCommand.Parameters.Clear();
            if (uid + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@UserID", uid);
            }

            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                QualificationWithStatus dl = new QualificationWithStatus()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Name = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    DateObtained = Convert.ToDateTime(sdr[2].ToString() ?? DBNull.Value.ToString()),
                    ExpiryDate = Convert.ToDateTime(sdr[3].ToString() ?? DBNull.Value.ToString()),
                    UserID = Convert.ToInt32(sdr[4].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[4].ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[5].ToString() ?? DBNull.Value.ToString()),
                    DaysLeft = Convert.ToInt32(sdr[6].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[6].ToString())
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
            sqlCommand.CommandText = "SELECT COUNT(*) FROM [dbo].[AvaChem_Qualification]";
            int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
            return count;
        }
    }
    public Qualification UpdateQualification(Qualification AvaChemQualification)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_Qualification] SET " +
                                        (AvaChemQualification.Name + "" == "" ? "" : "[Name]=@Name, ") +
                                        (AvaChemQualification.DateObtained + "" == "" ? "" : "[DateObtained]=@DateObtained, ") +
                                        (AvaChemQualification.ExpiryDate + "" == "" ? "" : "[ExpiryDate]=@ExpiryDate, ") +
                                        (AvaChemQualification.UserID + "" == "" ? "" : "[UserID]=@UserID, ") +
                                        (AvaChemQualification.SoftDelete + "" == "" ? "" : "[SoftDelete]=@SoftDelete ") +
                                        "WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", AvaChemQualification.ID == 0 ? 0 : AvaChemQualification.ID);
            sqlCommand.Parameters.AddWithValue("@Name", AvaChemQualification.Name ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@DateObtained", AvaChemQualification.DateObtained.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@ExpiryDate", AvaChemQualification.ExpiryDate.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@UserID", AvaChemQualification.UserID == 0 ? 0 : AvaChemQualification.UserID);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemQualification.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return AvaChemQualification;
        }
    }
    public Qualification UpdateQualificationSoftDelete(int ID)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE[dbo].[AvaChem_Qualification] SET [SoftDelete]=1 WHERE [ID]=@ID";
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
            sqlCommand.CommandText = "DELETE FROM [dbo].[AvaChem_Qualification] WHERE [ID]=@ID";
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
    public bool CheckBelongToThisUser(int ID, int uid)
    {
        int count = 0;
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT COUNT([ID]) as CountID FROM [AvaChem_Qualification] " +
                                        "WHERE [SoftDelete]=0 AND [ID]=@ID AND [UserID]=@UserID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            sqlCommand.Parameters.AddWithValue("@UserID", uid);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                count = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString());
            }
        }
        return count > 0;
    }
}