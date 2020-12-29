using Microsoft.VisualStudio.TestTools.UnitTesting;
using TFB8.Models;

namespace TFB8.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Controllers;
    using Interfaces;
    using Services;

    using System.Configuration;

    [TestClass]
    public class DisciplineTest
    {
        string connectionString = "server=localhost;database=tfb8;userid=emanolov;password=test;";

        [TestMethod]
        public void TestCreateDiscpiline()
        {
            var disciplineService = new DisciplineService(connectionString);
            Discipline discipline = new Discipline() { DisciplineName = "DisciplineName2", ProfessorName = "ProfessorName2" };
            disciplineService.CreateDiscipline(discipline);
            List<Discipline> disciplines = disciplineService.GetAllDisciplines().ToList().Where(x =>
                x.DisciplineName == discipline.DisciplineName && x.ProfessorName == discipline.ProfessorName).ToList();
            int expectedResult = 1;
            Assert.AreEqual(expectedResult, disciplines.Count);
        }

        [TestMethod]
        public void TestDeleteDiscpiline()
        {
            var disciplineService = new DisciplineService(connectionString);
            Discipline discipline = new Discipline() { DisciplineName = "DisciplineName33", ProfessorName = "ProfessorName33" };
            disciplineService.CreateDiscipline(discipline);
            Discipline createdDiscipline = disciplineService.GetAllDisciplines().ToList().Where(x =>
                x.DisciplineName == discipline.DisciplineName && x.ProfessorName == discipline.ProfessorName).First();
            disciplineService.DeleteDiscipline(createdDiscipline.DisciplineId);
            List<Discipline> disciplinesAfterDelete = disciplineService.GetAllDisciplines().ToList().Where(x =>
                x.DisciplineName == discipline.DisciplineName && x.ProfessorName == discipline.ProfessorName).ToList();
            int expectedResult = 0;
            Assert.AreEqual(expectedResult, disciplinesAfterDelete.Count);
        }
    }
}
