using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ParkSqlDao : IParkDAO
    {
        private string connectionString;
        public ParkSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<Park> GetParks()
        {
            List<Park> list = new List<Park>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "select * from park";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Park park = new Park();
                        park.ParkId = Convert.ToInt32(rdr["park_id"]);
                        park.Name = Convert.ToString(rdr["name"]);
                        park.Location = Convert.ToString(rdr["location"]);
                        park.EstablishDate = Convert.ToDateTime(rdr["establish_date"]);
                        park.Area = Convert.ToInt32(rdr["area"]);
                        park.Visitors = Convert.ToInt32(rdr["visitors"]);
                        park.Description = Convert.ToString(rdr["description"]);
                        list.Add(park);
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
