using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;

namespace ProjectDBTests
{
    [TestClass]
    public class ParkSqlDAOTests
    {
        private TransactionScope transaction = null;
        private string connectionString = "Server=.\\SqlExpress;Database=campground;Trusted_Connection=True;";
        private int newpark_id;

        [TestInitialize]
        public void SetupDatabase()
        {
            // Start a transaction, so we can roll back when we are finished with this test
            transaction = new TransactionScope();

            // Open Setup.Sql and read in the script to be executed
            string setupSQL;
            using (StreamReader rdr = new StreamReader("Setup.sql"))
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
                    newpark_id = Convert.ToInt32(rdr["newpark_id"]);
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
        public void GetParksTest()
        {

            // Arrange
            ParkSqlDao dao = new ParkSqlDao(connectionString);

            //Act
            IList<Park> parks = dao.GetParks();

            //Assert

            // At least 1 Project
            Assert.IsTrue(parks.Count > 0);
        }
    }
}
