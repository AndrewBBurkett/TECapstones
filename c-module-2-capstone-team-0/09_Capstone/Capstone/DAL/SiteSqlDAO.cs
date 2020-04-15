using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class SiteSqlDAO : ISiteDAO
    {
        private string connectionString;
        public SiteSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<Site> TopFiveSites(int campgroundId, DateTime arrival, DateTime departure)
        {
            IList<Site> top5Sites = new List<Site>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "Select top 5 * from site join campground on site.campground_id = campground.campground_id left join reservation on reservation.site_id = site.site_id where site.site_id not in (select s.site_id from site as s join campground c on s.campground_id = c.campground_id left JOIN reservation r on r.site_id = s.site_id where NOT((from_date > @departure or to_date < @arrival) and @arrival not between from_date and to_date and @departure not between from_date and to_date))And campground.campground_id = 1";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@campgroundid", campgroundId);
                    cmd.Parameters.AddWithValue("@arrival", arrival);
                    cmd.Parameters.AddWithValue("@departure", departure);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Site site = new Site();
                        site.CampgroundId = campgroundId;
                        site.DailyFee = Convert.ToDecimal(rdr["daily_fee"]);
                        site.Utilities = Convert.ToBoolean(rdr["utilities"]);
                        site.SiteId = Convert.ToInt32(rdr["site_id"]);
                        site.Accessible = Convert.ToBoolean(rdr["accessible"]);
                        site.MaxOccupancy = Convert.ToInt32(rdr["max_occupancy"]);
                        site.MaxRvLength = Convert.ToInt32(rdr["max_rv_length"]);
                        site.SiteNumber = Convert.ToInt32(rdr["site_number"]);
                        top5Sites.Add(site);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return top5Sites;

        }
    }
}
