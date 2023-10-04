using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
public class CreditDataConnector
{
    public CreditDataConnector()
    {
    }
    public Credit CreateCredit(Credit AvaChemCredit)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "INSERT INTO [dbo].[AvaChem_Credit]([Amount],[Description],[UserID],[CreatedDate],[UpdatedDate],[SoftDelete])Values(@Amount,@Description,@UserID,@CreatedDate,@UpdatedDate,@SoftDelete)";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", AvaChemCredit.ID == 0 ? 0 : AvaChemCredit.ID);
            sqlCommand.Parameters.AddWithValue("@Amount", AvaChemCredit.Amount == 0 ? 0 : AvaChemCredit.Amount);
            sqlCommand.Parameters.AddWithValue("@Description", AvaChemCredit.Description ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@UserID", AvaChemCredit.UserID == 0 ? 0 : AvaChemCredit.UserID);
            sqlCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemCredit.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return GetCreditByLastInsertedID();
        }
    }
    public Credit GetCreditByFirstVariable(int ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[Amount],[Description],[UserID],[CreatedDate],[UpdatedDate],[SoftDelete] FROM [dbo].[AvaChem_Credit] WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new Credit()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Amount = Convert.ToDouble(sdr[1].ToString()) == 0 ? 0 : Convert.ToDouble(sdr[1].ToString()),
                    Description = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    UserID = Convert.ToInt32(sdr[3].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[3].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[4].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[5].ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[6].ToString() ?? DBNull.Value.ToString())
                };
            }
            return null;
        }
    }
    public Credit GetCreditByLastInsertedID()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT * FROM [dbo].[AvaChem_Credit] WHERE [ID]=(SELECT MAX(ID) FROM [AvaChem_Credit]) AND [SoftDelete]=0";
            sqlCommand.Parameters.Clear();
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new Credit()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Amount = Convert.ToDouble(sdr[1].ToString()) == 0 ? 0 : Convert.ToDouble(sdr[1].ToString()),
                    Description = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    UserID = Convert.ToInt32(sdr[3].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[3].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[4].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[5].ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[6].ToString() ?? DBNull.Value.ToString())
                };
            }
            return null;
        }
    }
    public CreditLogsAndCount<T> GetAll<T>(int? page = null, int? per_page = null, string search = "",
                                string from = "", string to = "", bool? includeUser = false, int? uid = null,
                                int? month = null, int? year = null, int? sort = null) where T : Credit
    {
        CreditLogsAndCount<T> result = new CreditLogsAndCount<T>();
        List<T> finalListToReturn = new List<T>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            bool shouldIncludeUser = includeUser == true || search + "" != "";
            sqlCommand.CommandText = "SELECT c.[ID],c.[Amount],c.[Description],c.[UserID],c.[CreatedDate],c.[UpdatedDate],c.[SoftDelete] " +
                                        (shouldIncludeUser == false ? "" : ",u.[Name],u.[IDNumber],u.[Credits] ") +
                                        ",Count(*) OVER() AS TotalCount " +
                                        "FROM [dbo].[AvaChem_Credit] AS c " +
                                        (shouldIncludeUser == false ? "" : "LEFT JOIN [AvaChem_User] AS u ON c.[UserID]=u.[ID] ") +
                                        "WHERE c.[SoftDelete]=0 " + (shouldIncludeUser == false ? "" : "AND u.[SoftDelete]=0 ") +
                                        (search + "" == "" ? ""
                                            : "AND ( u.[Name] Like @Search ) ") +
                                        ((from + "" == "" && to + "" == "") ? ""
                                                         : "AND c.[CreatedDate] BETWEEN @BetweenStart AND @BetweenEnd ") +
                                        (uid + "" == "" ? "" : "AND c.[UserID]=@UserID ") +

                                        ((month + "" == "0" || month + "" == "") ? "" : "AND MONTH(c.[CreatedDate])=@Month ") +
                                        ((year + "" == "0" || year + "" == "") ? "" : "AND YEAR(c.[CreatedDate])=@Year ") +
                                        (sort + "" == "0" ? "ORDER BY c.[ID] DESC" : "ORDER BY c.[ID] " + (sort == 1 ? "ASC" : "DESC")) +
                                        ((page + "" == "" || per_page + "" == "")
                                            ? ""
                                            : " OFFSET (@Offset) ROWS FETCH NEXT @PerPage ROWS ONLY");
            sqlCommand.Parameters.Clear();
            if (search + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@Search", $"%{search}%");
            }
            if (from + "" != "" || to + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@BetweenStart", from + "" != "" ? Convert.ToDateTime(from).ToString("yyyy-MM-dd") : Convert.ToDateTime(to).AddDays(-1).ToString("yyyy-MM-dd"));
                sqlCommand.Parameters.AddWithValue("@BetweenEnd", to + "" != "" ? Convert.ToDateTime(to).ToString("yyyy-MM-dd") : Convert.ToDateTime(from).AddDays(1).ToString("yyyy-MM-dd"));
            }
            if (uid + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@UserID", uid);
            }
            if (month + "" != "0" && month + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@Month", month);
            }
            if (year + "" != "0" && year + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@Year", year);
            }
            if (page + "" != "" && per_page + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@Offset", (page - 1) * per_page);
                sqlCommand.Parameters.AddWithValue("@PerPage", per_page);
            }
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                CreditTableView dl = new CreditTableView()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Amount = Convert.ToDouble(sdr[1].ToString()) == 0 ? 0 : Convert.ToDouble(sdr[1].ToString()),
                    Description = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    UserID = Convert.ToInt32(sdr[3].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[3].ToString()),
                    CreatedDate = Convert.ToDateTime(sdr[4].ToString() ?? DBNull.Value.ToString()),
                    UpdatedDate = Convert.ToDateTime(sdr[5].ToString() ?? DBNull.Value.ToString()),
                    SoftDelete = Convert.ToBoolean(sdr[6].ToString() ?? DBNull.Value.ToString())
                };
                if (shouldIncludeUser)
                {
                    dl.UName = sdr[7].ToString() ?? DBNull.Value.ToString();
                    dl.UIDNumber = sdr[8].ToString() ?? DBNull.Value.ToString();
                    dl.UCredits = Convert.ToDouble(sdr[9].ToString()) == 0 ? 0 : Convert.ToDouble(sdr[9].ToString());
                    result.Count = Convert.ToInt32(sdr[10].ToString());
                }
                else
                {
                    result.Count = Convert.ToInt32(sdr[7].ToString());
                }
                finalListToReturn.Add(dl as T);
            }

            result.Credits = finalListToReturn;
            return result;
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
            sqlCommand.CommandText = "SELECT COUNT(*) FROM [dbo].[AvaChem_Credit] WHERE [SoftDelete]=0";
            int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
            return count;
        }
    }
    public Credit UpdateCredit(Credit AvaChemCredit)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_Credit] SET [Amount]=@Amount,[Description]=@Description,[UserID]=@UserID,[UpdatedDate]=@UpdatedDate,[SoftDelete]=@SoftDelete WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", AvaChemCredit.ID == 0 ? 0 : AvaChemCredit.ID);
            sqlCommand.Parameters.AddWithValue("@Amount", AvaChemCredit.Amount == 0 ? 0 : AvaChemCredit.Amount);
            sqlCommand.Parameters.AddWithValue("@Description", AvaChemCredit.Description ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@UserID", AvaChemCredit.UserID == 0 ? 0 : AvaChemCredit.UserID);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemCredit.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return AvaChemCredit;
        }
    }
    public Credit UpdateCreditSoftDelete(int ID)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE[dbo].[AvaChem_Credit] SET [SoftDelete]=1,[UpdatedDate]=@UpdatedDate WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
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
            sqlCommand.CommandText = "DELETE FROM [dbo].[AvaChem_Credit] WHERE [ID]=@ID";
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