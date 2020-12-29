using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TFB8.Services;
using TFB8.Models;
using System.Linq;

namespace TFB8.Tests
{
    /// <summary>
    /// Summary description for SemesterTest
    /// </summary>
    [TestClass]
    public class SemesterTest
    {
        public SemesterTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;
        string connectionString = "server=localhost;database=tfb8;userid=test;password=test;";

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            var semesterService = new SemesterService(connectionString);
            var disciplineService = new DisciplineService(connectionString);
            //Create discipline
            Discipline discipline = new Discipline() { DisciplineName = "DisciplineName2", ProfessorName = "ProfessorName2" };
            disciplineService.CreateDiscipline(discipline);
            Discipline createdDiscipline = disciplineService.GetAllDisciplines().ToList().Where(x =>
                x.DisciplineName == discipline.DisciplineName && x.ProfessorName == discipline.ProfessorName).First();

            Semester semester = new Semester()
            {
                Name = "Semester11121",
                EndDate = DateTime.Now,
                StartDate = DateTime.Now,
                Disciplines = new List<Discipline>() { discipline }
            };

            semesterService.CreateSemester(semester);
            List<Semester> semesters = semesterService.GetAllSemesters().ToList().Where(x =>
                x.Name == semester.Name).ToList();
            int expectedResult = 1;

            Assert.AreEqual(expectedResult, semesters.Count);
        }
    }
}
