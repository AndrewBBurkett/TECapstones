using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ReservationSqlDAO : IReservationDAO
    {
        private string connectionString;
        public ReservationSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        //public int id = 0;
        public int MakeReservation(Reservation reservation)
        {
            //int id = 0;
            //try
            //{
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "insert into reservation(site_id, name, from_date, to_date) values(@siteid, @name, @fromdate, @todate); select @@identity;";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@siteid", reservation.SiteId);
                    cmd.Parameters.AddWithValue("@name", reservation.Name);
                    cmd.Parameters.AddWithValue("@fromdate", reservation.FromDate);
                    cmd.Parameters.AddWithValue("@todate", reservation.ToDate);
                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    return id;
                }
            //}
            //catch (SqlException ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //return id;
        }
    }
}
