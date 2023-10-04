using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

public class VehicleDataConnector
{
    public VehicleDataConnector()
    {
    }
    public Vehicle CreateVehicle(Vehicle AvaChemVehicle)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "INSERT INTO [dbo].[AvaChem_Vehicle]([Model],[Number],[SoftDelete],[CreatedDate],[UpdatedDate])Values(@Model,@Number,@SoftDelete,@CreatedDate,@UpdatedDate)";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@Model", AvaChemVehicle.Model ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Number", AvaChemVehicle.Number ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemVehicle.SoftDelete.ToString() ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return GetVehicleByLastInsertedID();
        }
    }
    public Vehicle GetVehicleByFirstVariable(int ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[Model],[Number],[SoftDelete] FROM [dbo].[AvaChem_Vehicle] WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new Vehicle()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Model = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    Number = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    SoftDelete = Convert.ToBoolean(sdr[3].ToString() ?? DBNull.Value.ToString()),
                };
            }
            return null;
        }
    }
    public Vehicle GetVehicleByLastInsertedID()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[Model],[Number],[SoftDelete] FROM [dbo].[AvaChem_Vehicle] WHERE [ID]=(SELECT MAX(ID) FROM [AvaChem_Vehicle]) AND [SoftDelete]=0";
            sqlCommand.Parameters.Clear();
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                return new Vehicle()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Model = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    Number = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    SoftDelete = Convert.ToBoolean(sdr[3].ToString() ?? DBNull.Value.ToString()),
                };
            }
            return null;
        }
    }
    public List<Vehicle> GetAll(int? page = null, int? per_page = null, string search = "")
    {
        List<Vehicle> finalListToReturn = new List<Vehicle>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            // bool shouldIncludeUser = includeUser == true || search + "" != "";
            sqlCommand.CommandText = "SELECT v.[ID],v.[Model],v.[Number],v.[SoftDelete] " +
                                        // (shouldIncludeUser == false ? "" : ",u.[Name],u.[IDNumber] ") +
                                        "FROM [dbo].[AvaChem_Vehicle] AS v " +
                                        // (shouldIncludeUser == false ? "" : "LEFT JOIN [AvaChem_User] AS u ON v.[UserID]=u.[ID] ") +
                                        // "WHERE v.[SoftDelete]=0 " + (shouldIncludeUser == false ? "" : "AND u.[SoftDelete]=0 ") +
                                        "WHERE v.[SoftDelete]=0 " +
                                        (search + "" == "" ? ""
                                            : "AND ( v.[Number] Like @Search OR v.[Model] Like @Search ) ") +
                                        // (uid + "" == "" ? "" : "AND v.[UserID]=@UserID ") +
                                        ((page + "" == "" || per_page + "" == "") ? "ORDER BY v.[ID] DESC" : "ORDER BY v.[ID] DESC OFFSET (@Offset) ROWS FETCH NEXT @PerPage ROWS ONLY");
            sqlCommand.Parameters.Clear();
            if (search + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@Search", $"%{search}%");
            }
            // if (uid + "" != "")
            // {
            //     sqlCommand.Parameters.AddWithValue("@UserID", uid);
            // }
            if (page + "" != "" && per_page + "" != "")
            {
                sqlCommand.Parameters.AddWithValue("@Offset", (page - 1) * per_page);
                sqlCommand.Parameters.AddWithValue("@PerPage", per_page);
            }
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                Vehicle dl = new Vehicle()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Model = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    Number = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    SoftDelete = Convert.ToBoolean(sdr[3].ToString() ?? DBNull.Value.ToString()),
                    // UserID = Convert.ToInt32(sdr[4].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[4].ToString())
                };
                // if (shouldIncludeUser)
                // {
                //     dl.UName = sdr[5].ToString() ?? DBNull.Value.ToString();
                //     dl.UIDNumber = sdr[6].ToString() ?? DBNull.Value.ToString();
                // }
                finalListToReturn.Add(dl);
            }
            return finalListToReturn;
        }
    }
    // public ICollection<Vehicle> GetVehiclesByUserID(int uid)
    // {
    //     List<Vehicle> finalListToReturn = new List<Vehicle>();
    //     string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
    //     using (SqlConnection sqlConnection = new SqlConnection(connectionString))
    //     {
    //         SqlCommand sqlCommand = new SqlCommand()
    //         {
    //             Connection = sqlConnection
    //         };
    //         sqlConnection.Open(); sqlCommand.CommandText = "SELECT [ID],[Model],[Number],[SoftDelete],[UserID]FROM [dbo].[AvaChem_Vehicle] WHERE [UserID]=@UserID AND [SoftDelete]=0";
    //         sqlCommand.Parameters.Clear();
    //         sqlCommand.Parameters.AddWithValue("@UserID", uid);
    //         SqlDataReader sdr = sqlCommand.ExecuteReader();
    //         while (sdr.Read())
    //         {
    //             Vehicle dl = new Vehicle()
    //             {
    //                 ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
    //                 Model = sdr[1].ToString() ?? DBNull.Value.ToString(),
    //                 Number = sdr[2].ToString() ?? DBNull.Value.ToString(),
    //                 SoftDelete = Convert.ToBoolean(sdr[3].ToString() ?? DBNull.Value.ToString()),
    //                 UserID = Convert.ToInt32(sdr[4].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[4].ToString())
    //             };
    //             finalListToReturn.Add(dl);
    //         }
    //         return finalListToReturn;
    //     }
    // }
    public ICollection<Vehicle> GetAvailableVehicles(int currentJobID = 0, string[] workingDates = null, string start = "", string end = "")
    {
        if (workingDates == null || start == "" || end == "")
        {
            return GetAll();
        }

        List<Vehicle> finalListToReturn = new List<Vehicle>();
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };

            string queryWorkingDates = "";
            if (workingDates != null)
            {
                queryWorkingDates = string.Join(",", workingDates.Select(d => $"'{Convert.ToDateTime(d).ToString("yyyy-MM-dd")}'"));
            }
            sqlConnection.Open();
            sqlCommand.CommandText = "SELECT [ID],[Model],[Number],[SoftDelete],[IsValid] " +
                                    "FROM ( " +
                                    "SELECT v.[ID],v.[Model],v.[Number],v.[SoftDelete], " +
                                    "STRING_AGG( CASE " +
                                    "WHEN (j.[WorkingDate] IS NULL " +
                                    "OR ( j.[ID]=@JobID " +
                                    //"OR ( cast(j.[WorkingDate] AS date)!=@WorkingDate " +
                                    //"OR ( cast(j.[WorkingDate] AS date)=@WorkingDate " +
                                    $"OR ( cast(j.[WorkingDate] AS date) NOT IN ({queryWorkingDates}) " +
                                    $"OR ( cast(j.[WorkingDate] AS date) IN ({queryWorkingDates}) " +
                                    "AND " +
                                    "( @Start " +
                                    "NOT BETWEEN REPLACE(REPLACE(Convert(varchar(5),t.[StartTime],108), '12:', '24:'), '00:', '12:') " +
                                    "and REPLACE(REPLACE(Convert(varchar(5),t.[EndTime],108), '12:', '24:'), '00:', '12:') ) " +
                                    "AND " +
                                    "( @End " +
                                    "NOT BETWEEN REPLACE(REPLACE(Convert(varchar(5),t.[StartTime],108), '12:', '24:'), '00:', '12:') " +
                                    "and REPLACE(REPLACE(Convert(varchar(5),t.[EndTime],108), '12:', '24:'), '00:', '12:') ) " +
                                    "AND ( REPLACE(REPLACE(Convert(varchar(5),t.[StartTime],108), '12:', '24:'), '00:', '12:') " +
                                    "NOT BETWEEN @Start " +
                                    "and @End ) " +
                                    "AND ( REPLACE(REPLACE(Convert(varchar(5),t.[EndTime],108), '12:', '24:'), '00:', '12:') " +
                                    "NOT BETWEEN @Start " +
                                    "and @End ) ) ) ) )" +
                                    "THEN 1 ELSE 0 END, ',') AS [IsValid] " +
                                    "FROM [dbo].[AvaChem_Vehicle] AS v " +
                                    "LEFT JOIN [AvaChem_JobVehicle] AS jV ON jV.[VehicleID]=v.[ID] " +
                                    "LEFT JOIN [AvaChem_Job] AS j ON jV.[JobID]=j.[ID] " +
                                    "LEFT JOIN [AvaChem_Trip] AS t ON j.[ID]=t.[JobID]" +
                                    "WHERE v.[SoftDelete]=0 " +
                                    "AND ( j.[ID] IS NULL " +
                                    "OR ( j.[ID] IS NOT NULL " +
                                    "AND jV.[SoftDelete]=0 " +
                                    "AND j.[SoftDelete]=0 " +
                                    "AND ( t.[ID] IS NULL OR ( t.[ID] IS NOT NULL AND t.[SoftDelete]=0 ) ) ) )" +
                                    "GROUP BY v.[ID],v.[Model],v.[Number],v.[SoftDelete] ) AS R " +
                                    "WHERE R.[IsValid] NOT LIKE '%0%' " +
                                    "GROUP BY [ID],[Model],[Number],[SoftDelete],[IsValid]";
            //sqlCommand.CommandText =
            //    "SELECT [ID],[Model],[Number],[SoftDelete],[IsValid] FROM " +
            //     "( SELECT v.[ID],v.[Model],v.[Number],v.[SoftDelete]" +
            //            ",STRING_AGG(CASE WHEN (j.[WorkingDate] IS NULL " +
            //                                    "OR ( j.[ID]=@JobID OR ( cast(j.[WorkingDate] AS date)!=@WorkingDate " +
            //                                        "OR ( cast(j.[WorkingDate] AS date)=@WorkingDate AND " +
            //                                                "(@Start NOT BETWEEN " +
            //                                                        "REPLACE(REPLACE(Convert(varchar(5),t.[StartTime],108), '12:', '24:'), '00:', '12:') and REPLACE(REPLACE(Convert(varchar(5),t.[EndTime],108), '12:', '24:'), '00:', '12:') ) " +
            //                                                "AND (@End NOT BETWEEN " +
            //                                                        "REPLACE(REPLACE(Convert(varchar(5),t.[StartTime],108), '12:', '24:'), '00:', '12:') and REPLACE(REPLACE(Convert(varchar(5),t.[EndTime],108), '12:', '24:'), '00:', '12:') ) )" +
            //                                                "AND (REPLACE(REPLACE(Convert(varchar(5), t.[StartTime], 108), '12:', '24:'), '00:', '12:') NOT BETWEEN @Start and @End)" +
            //                                                "AND (REPLACE(REPLACE(Convert(varchar(5), t.[EndTime], 108), '12:', '24:'), '00:', '12:')) NOT BETWEEN '14:33' and '22:38') ) ) " +
            //                         "THEN 1 ELSE 0 END, ',') AS [IsValid] " +
            //        "FROM [dbo].[AvaChem_Vehicle] AS v " +
            //        "LEFT JOIN [AvaChem_JobVehicle] AS jV ON jV.[VehicleID]=v.[ID] " +
            //        "LEFT JOIN [AvaChem_Job] AS j ON jV.[JobID]=j.[ID] " +
            //        "LEFT JOIN [AvaChem_Trip] AS t ON j.[ID]=t.[JobID]" +
            //        "WHERE v.[SoftDelete]=0 AND ( j.[ID] IS NULL " +
            //                                        "OR ( j.[ID] IS NOT NULL " +
            //                                                "AND jV.[SoftDelete]=0 AND j.[SoftDelete]=0 AND ( t.[ID] IS NULL OR ( t.[ID] IS NOT NULL AND t.[SoftDelete]=0 ) ) ) ) " +
            //        "GROUP BY v.[ID],v.[Model],v.[Number],v.[SoftDelete] ) AS R " +
            //    "WHERE R.[IsValid] NOT LIKE '%0%' " +
            //    "GROUP BY [ID],[Model],[Number],[SoftDelete],[IsValid]";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@JobID", currentJobID);
            //sqlCommand.Parameters.AddWithValue("@WorkingDate", Convert.ToDateTime(workingDate).ToString("yyyy-MM-dd"));
            var x = Convert.ToDateTime(start).AddHours(-1).ToString("HH:mm").Replace("12:", "24:").Replace("00:", "12:");
            var y = Convert.ToDateTime(end).AddHours(1).ToString("HH:mm").Replace("12:", "24:").Replace("00:", "12:");
            sqlCommand.Parameters.AddWithValue("@Start", start.Contains("01:") ? "01:00" : Convert.ToDateTime(start).AddHours(-1).ToString("HH:mm").Replace("12:", "24:").Replace("00:", "12:"));
            sqlCommand.Parameters.AddWithValue("@End", end.Contains("23:") ? "23:59" : Convert.ToDateTime(end).AddHours(1).ToString("HH:mm").Replace("12:", "24:").Replace("00:", "12:"));
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            while (sdr.Read())
            {
                Vehicle dl = new Vehicle()
                {
                    ID = Convert.ToInt32(sdr[0].ToString()) == 0 ? 0 : Convert.ToInt32(sdr[0].ToString()),
                    Model = sdr[1].ToString() ?? DBNull.Value.ToString(),
                    Number = sdr[2].ToString() ?? DBNull.Value.ToString(),
                    SoftDelete = Convert.ToBoolean(sdr[3].ToString() ?? DBNull.Value.ToString()),
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
            sqlCommand.CommandText = "SELECT COUNT(*) FROM [dbo].[AvaChem_Vehicle] WHERE [SoftDelete]=0";
            int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
            return count;
        }
    }
    public Vehicle UpdateVehicle(Vehicle AvaChemVehicle)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE [dbo].[AvaChem_Vehicle]SET[Model]=@Model,[Number]=@Number,[SoftDelete]=@SoftDelete,[UpdatedDate]=@UpdatedDate WHERE [ID]=@ID";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@ID", AvaChemVehicle.ID == 0 ? 0 : AvaChemVehicle.ID);
            sqlCommand.Parameters.AddWithValue("@Model", AvaChemVehicle.Model ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@Number", AvaChemVehicle.Number ?? DBNull.Value.ToString());
            sqlCommand.Parameters.AddWithValue("@SoftDelete", AvaChemVehicle.SoftDelete.ToString() ?? DBNull.Value.ToString());
            // sqlCommand.Parameters.AddWithValue("@UserID", AvaChemVehicle.UserID == 0 ? 0 : AvaChemVehicle.UserID);
            sqlCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return AvaChemVehicle;
        }
    }
    public Vehicle UpdateVehicleSoftDelete(int ID)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand()
            {
                Connection = sqlConnection
            };
            sqlConnection.Open();
            sqlCommand.CommandText = "UPDATE[dbo].[AvaChem_Vehicle] SET [SoftDelete]=1,[UpdatedDate]=@UpdatedDate WHERE [ID]=@ID";
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
            sqlCommand.CommandText = "DELETE FROM [dbo].[AvaChem_Vehicle] WHERE [ID]=@ID";
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