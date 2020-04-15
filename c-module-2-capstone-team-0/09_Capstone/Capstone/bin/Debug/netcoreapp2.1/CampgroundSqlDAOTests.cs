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
    public class CampgroundSqlDAOTests
    {
        private TransactionScope transaction = null;
        private string connectionString = "Server=.\\SqlExpress;Database=campground;Trusted_Connection=True;";
        private int newcampground_id;

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
                    newcampground_id = Convert.ToInt32(rdr["newcampground_id"]);
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
        public void GetAllCampgoundsTests()
        {

            // Arrange
            CampgroungSqlDAO dao = new CampgroungSqlDAO(connectionString);

            //Act
            IList<Campground> Project = dao.GetCampground(newcampground_id);

            //Assert

            // At least 1 Project
            Assert.IsTrue(Project.Count > 0);
        }
//        [TestMethod]
//        public void AssignEmployeeToProjectTests()
//        {

//            ProjectSqlDAO dao = new ProjectSqlDAO(connectionString);



//            dao.AssignEmployeeToProject(1, 1);
//        }

//        [TestMethod]
//        public void CreateNewProject()
//        {
//            //int EmployeeId;
//            //int projectId;
//            // Arrange
//            ProjectSqlDAO dao = new ProjectSqlDAO(connectionString);
//            //EmployeeId = newEmployeeNumber;
//            //projectId = newProjectNumber;
//            //Act
//            // IList<Project> Project = dao.CreateProject(newProjectNumber);

//            //Assert

//            // At least 1 Employee
//            // Assert.IsTrue(Project.Count > 0);
//        }
//    }

}
}