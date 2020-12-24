using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TFB8.Models;

namespace TFB8.Controllers
{
    public class SemesterController : ApiController
    {
        string connectionString =
           @"server=localhost;userid=emanolov;password=test;database=tfb8";

        // GET api/<controller>
        [HttpGet()]
        public IHttpActionResult Get()
        {
            IHttpActionResult result = null;
            List<Semester> list = new List<Semester>();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                using (MySqlCommand command = new MySqlCommand(
                    "select s.semesterid, s.name, s.startdate, s.enddate, GROUP_CONCAT(d.disciplineName) as disciplines " +
                     "from tfb8.semester as s " +
                        "left join tfb8.semesterdisciplines sd on sd.semesterid = s.semesterid " +
                        "left join tfb8.discipline d on d.disciplineid = sd.disciplineid", con))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var semester = new Semester();
                        int semesterId = (int)reader["SemesterId"];
                        DateTime startDate = (DateTime)reader["StartDate"];
                        DateTime endDate = (DateTime)reader["EndDate"];
                        string name = (string)reader["Name"];
                        string disciplines = (string)reader["Disciplines"];
                        semester.SemesterId = semesterId;
                        semester.StartDate = startDate;
                        semester.Name = name;
                        semester.EndDate = endDate;
                        semester.DisciplinesAsString = disciplines;
                        list.Add(semester);
                    }
                }

                con.Close();
            }

            if (list.Count > 0)
            {
                result = Ok(list);
            }
            else
            {
                result = NotFound();
            }

            return result;
        }
    }
}
