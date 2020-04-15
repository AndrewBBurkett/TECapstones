using Capstone.Models;
using System.Collections.Generic;
using System;
using System.Text;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class CampgroungSqlDAO : ICampgroundDAO
    {
        private string connectionString;
        public CampgroungSqlDAO(string dbConnection)
        {
            connectionString = dbConnection;
        }
        public IList<Campground> GetCampground(int parkWant)
        {
           List<Campground> list = new List<Campground>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "select * from campground where campground_id = @campground_id";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@campground_id", parkWant);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Campground campground = new Campground();
                        campground.CampgroundId = Convert.ToInt32(rdr["campground_id"]);
                        campground.ParkId = Convert.ToInt32(rdr["park_id"]);
                        campground.Name = Convert.ToString(rdr["name"]);
                        campground.OpenFrom = Convert.ToInt32(rdr["open_from_mm"]);
                        campground.OpenTo = Convert.ToInt32(rdr["open_to_mm"]);
                        campground.DailyFee = Convert.ToInt32(rdr["daily_fee"]);
                        list.Add(campground);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return list;
        }
    }
}
