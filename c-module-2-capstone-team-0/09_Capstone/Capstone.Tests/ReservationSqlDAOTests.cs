using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;

namespace Capstone.Tests
{
    [TestClass]
    public class ReservationSqlDAOTests
    {
        private TransactionScope transaction = null;
        private string connectionString = "Server=.\\SqlExpress;Database=npcampground;Trusted_Connection=True;";
        private int newcampground_id;
        private int newpark_id;
        private int newsite_id;
        private int newreservation_id;


        [TestInitialize]
        public void SetupDatabase()
        {
            // Start a transaction, so we can roll back when we are finished with this test
            transaction = new TransactionScope();

            // Open Setup.Sql and read in the script to be executed
            string setupSQL;
            using (StreamReader rdr = new StreamReader("setup.sql"))
            {
                setupSQL = rdr.ReadToEnd();
            }

            // Connect to the DB and execute the script
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(setupSQL, conn);
                SqlDataReader rdr = cmd.ExecuteReader();

                // Get the result (new Campground id) and save it for use later in a test.
                if (rdr.Read())
                {
                    newreservation_id = Convert.ToInt32(rdr["reservation_id"]);
                    newsite_id = Convert.ToInt32(rdr["site_id"]);
                }
            }
        }

        [TestCleanup]
        public void CleanupDatabase()
        {
            // Rollback the transaction to get our good data back
            transaction.Dispose();
        }
        [TestMethod]
        public void MakeReservationTests()
        {

            // Arrange
           ReservationSqlDAO dao = new ReservationSqlDAO(connectionString);
            Reservation r1 = new Reservation();
            r1.SiteId = newsite_id;
            r1.Name = "Test Family";
            r1.FromDate = DateTime.Parse("2021-06-15");
            r1.ToDate = DateTime.Parse("2021-06-30");
            r1.CreateDate = DateTime.Now;

            //@site_id, 'Asimov Family', '2020-06-15','2020-06-30', '2020-05-01'



            //Act
            int reservation_id = dao.MakeReservation(r1);

            //Assert

            // At least 1 Campground
            Assert.IsTrue(reservation_id > 0);
        }
       

    }
}