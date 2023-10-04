using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

public class UserRoleDataConnector
{
    public UserRoleDataConnector()
    {
    }
    public UserRole CreateUserRole(UserRole AvaChemUserRole)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "INSERT INTO [dbo].[AvaChem_UserRole]([RoleName],[SoftDelete],[CRoleName])Values(@RoleName,@SoftDelete,@CRoleName)";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@RoleName", AvaChemUserRole.RoleName ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemUserRole.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@CRoleName", AvaChemUserRole.CRoleName ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return GetUserRoleByLastInsertedID();
        }
    }
    public UserRole GetUserRoleByFirstVariable(int ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[RoleName],[SoftDelete],[CRoleName] FROM [dbo].[AvaChem_UserRole] WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new UserRole()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    RoleName = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    SoftDelete = Convert.ToBoolean(sdr[2].ToString() ?? DBNull.Value.ToString()),
                    CRoleName = sdr[3].ToString() ?? DBNull.Value.ToString()
                };
            }
            return null;
        }
    }
    public UserRole GetUserRoleByLastInsertedID()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[RoleName],[SoftDelete],[CRoleName] FROM [dbo].[AvaChem_UserRole] WHERE [ID]=(SELECT MAX(ID) FROM [AvaChem_UserRole]) AND [SoftDelete]=0";
            sqlCommand.Parameters.Clear();
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new UserRole()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    RoleName = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    SoftDelete = Convert.ToBoolean(sdr[2].ToString() ?? DBNull.Value.ToString()),
                    CRoleName = sdr[3].ToString() ?? DBNull.Value.ToString()
                };
            }
            return null;
        }
    }
    public ICollection<UserRole> GetAll(int currentRole)
    {
        List<UserRole> finalListToReturn = new List<UserRole>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT [ID],[RoleName],[SoftDelete],[CRoleName] FROM [dbo].[AvaChem_UserRole] WHERE [SoftDelete]=0 AND " +
                                        (currentRole == UserRoles.HR.GetHashCode() ? "[ID] NOT IN (@SuperAdmin,@OverallAdmin,@CreditAdmin,@HR,@OTAdmin)"
                                                                                   : currentRole == UserRoles.SuperAdmin.GetHashCode()
                                                                                   ? "[ID] NOT IN (@SuperAdmin)"
                                                                                   : "[ID]=0");
            sqlCommand.Parameters.Clear();
            if (currentRole == UserRoles.HR.GetHashCode() || currentRole == UserRoles.SuperAdmin.GetHashCode())
                sqlCommand.Parameters.AddWithValue("@SuperAdmin", UserRoles.SuperAdmin.GetHashCode());
            if (currentRole == UserRoles.HR.GetHashCode())
            {
                sqlCommand.Parameters.AddWithValue("@OverallAdmin", UserRoles.OverallAdmin.GetHashCode());
                sqlCommand.Parameters.AddWithValue("@CreditAdmin", UserRoles.CreditAdmin.GetHashCode());
                sqlCommand.Parameters.AddWithValue("@HR", UserRoles.HR.GetHashCode());
                sqlCommand.Parameters.AddWithValue("@OTAdmin", UserRoles.OTAdmin.GetHashCode());
            }
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                UserRole dl = new UserRole()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    RoleName = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    SoftDelete = Convert.ToBoolean(sdr[2].ToString() ?? DBNull.Value.ToString()),
                    CRoleName = sdr[3].ToString() ?? DBNull.Value.ToString()
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
            sqlCommand.CommandText = "SELECT COUNT(*) FROM [dbo].[AvaChem_UserRole]";
            int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
            return count;
        }
    }
    public UserRole UpdateUserRole(UserRole AvaChemUserRole)
    {
        int count = 0;
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_UserRole]SET[RoleName]=@RoleName,[SoftDelete]=@SoftDelete,[CRoleName]=@CRoleName WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@RoleName", AvaChemUserRole.RoleName ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemUserRole.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@CRoleName", AvaChemUserRole.CRoleName ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            if (count > 0)
            {
                return AvaChemUserRole;
            }
        }
        return null;
    }
    public void UpdateUserRoleSoftDelete(int ID)
    {
        int count = 0;
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE[dbo].[AvaChem_UserRole] SET [SoftDelete]=1 WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            if (count == 1)
            {
                return;
            }
        }
        return;
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
            sqlCommand.CommandText = "DELETE FROM [dbo].[AvaChem_UserRole] WHERE [ID]=@ID";
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
