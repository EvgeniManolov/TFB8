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
                        "left join tfb8.discipline d on d.disciplineid = sd.disciplineid group by s.semesterid", con))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var semester = new Semester();
                        int semesterId = (int)reader["SemesterId"];
                        DateTime startDate = (DateTime)reader["StartDate"];
                        DateTime endDate = (DateTime)reader["EndDate"];
                        string name = (string)reader["Name"];
                        string disciplines = "";
                        if (reader["Disciplines"] != null && reader["Disciplines"] != DBNull.Value)
                        {
                            disciplines = (string)reader["Disciplines"];
                        }
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


        // POST api/<controller>
        [HttpPost()]
        public IHttpActionResult Post(Semester semester)
        {
            IHttpActionResult result = null;

            if (semester is null)
            {
                result = NotFound();
            }
            else
            {
                var disciplines = semester.DisciplinesAsString
                    .Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    try
                    {
                        using (MySqlCommand command = new MySqlCommand(
                            "INSERT into tfb8.semester values (default, @Name, @StartDate, @EndDate)", con))
                        {
                            command.Parameters.Add(new MySqlParameter("Name", semester.Name));
                            command.Parameters.Add(new MySqlParameter("StartDate", semester.StartDate));
                            command.Parameters.Add(new MySqlParameter("EndDate", semester.EndDate));
                            command.ExecuteNonQuery();
                            var lastId = command.LastInsertedId;

                            foreach (string discipline in disciplines)
                            {
                                using (MySqlCommand com = new MySqlCommand(
                                    "INSERT into tfb8.semesterdisciplines values (@SemesterId, (Select disciplineid from tfb8.discipline where UPPER(disciplinename) = @Discipline))", con))
                                {
                                    com.Parameters.Add(new MySqlParameter("SemesterId", lastId));
                                    com.Parameters.Add(new MySqlParameter("Discipline", discipline));
                                    com.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    catch
                    {
                        result = NotFound();
                    }
                }
                result = Ok(semester);
            }

            return result;
        }
        
    }
}
