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
    public class SiteSqlDAOTests
    {
        private TransactionScope transaction = null;
        private string connectionString = "Server=.\\SqlExpress;Database=npcampground;Trusted_Connection=True;";
        private int newsite_id;
        private int newcampground_id;

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

                // Get the result (new Department id) and save it for use later in a test.
                if (rdr.Read())
                {
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
        public void TopFiveSitesTest()
        {
            ////int campgroundId, DateTime arrival, DateTime departure
            //// Arrange
            SiteSqlDAO dao = new SiteSqlDAO(connectionString);
            //Site s1 = new Site();
            //s1.CampgroundId = newcampground_id;
            //s1. = DateTime.Parse("2021-06-15");



            //Act
            IList<Site> site = dao.TopFiveSites(newcampground_id, DateTime.Parse("2020-06-15"),DateTime.Parse("2021-07-05"));

            //Assert

            // At least 1 Project
            Assert.IsTrue(site != null) ;
        }
    }
}
//public int SiteId { get; set; }
//public int CampgroundId { get; set; }
//public int SiteNumber { get; set; }
//public int MaxOccupancy { get; set; }
//public bool Accessible { get; set; }
//public int MaxRvLength { get; set; }
//public bool Utilities { get; set; }
//public decimal DailyFee { get; set; }
