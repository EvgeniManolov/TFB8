﻿namespace TFB8.Controllers
{
    using MySql.Data.MySqlClient;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using TFB8.Models;

    public class DisciplineController : ApiController
    {
        string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;


        // GET api/<controller>
        [HttpGet()]
        public IHttpActionResult Get()
        {
            IHttpActionResult result = null;
            List<Discipline> list = new List<Discipline>();
           
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                using (MySqlCommand command = new MySqlCommand("SELECT disciplineId, disciplineName, professorName FROM tfb8.discipline", con))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var discipline = new Discipline();
                        int disciplineId = (int)reader["disciplineId"];
                        string disciplineName = reader["disciplineName"] as string;
                        string professorName = reader["professorName"] as string;
                        discipline.DisciplineId = disciplineId;
                        discipline.DisciplineName = disciplineName;
                        discipline.ProfessorName = professorName;
                        list.Add(discipline);
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

        // GET api/<controller>/id
        [HttpGet()]
        public IHttpActionResult Get(int id)
        {
            IHttpActionResult result;
            List<Discipline> list = new List<Discipline>();
            Discipline discipline = new Discipline();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string sqlCommand =
                    "SELECT disciplineId, disciplineName, professorName FROM tfb8.discipline where disciplineId = " +
                    id;
                using (MySqlCommand command = new MySqlCommand(sqlCommand, con))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int disciplineId = (int)reader["disciplineId"];
                        string disciplineName = reader["disciplineName"] as string;
                        string professorName = reader["professorName"] as string;
                        discipline.DisciplineId = disciplineId;
                        discipline.DisciplineName = disciplineName;
                        discipline.ProfessorName = professorName;
                    }
                }

                con.Close();
            }

            if (discipline == null)
            {
                result = Content(HttpStatusCode.NotFound, "Missing discipline with this id");
            }
            else
            {
                result = Ok(discipline);
            }

            return result;
        }


        // POST api/<controller>
        [HttpPost()]
        public IHttpActionResult Post(Discipline discipline)
        {
            IHttpActionResult result = null;

            if (string.IsNullOrEmpty(discipline.DisciplineName))
            {
                result = Content(HttpStatusCode.BadRequest, "Discipline name is requered!");
            }
            else if (string.IsNullOrEmpty(discipline.ProfessorName))
            {
                result = Content(HttpStatusCode.BadRequest, "Professor name is requered!");
            }
            else
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    try
                    {
                        using (MySqlCommand command = new MySqlCommand(
                            "INSERT into tfb8.discipline values (default, @DisciplineName, @ProfessorName)", con))
                        {
                            command.Parameters.Add(new MySqlParameter("DisciplineName", discipline.DisciplineName));
                            command.Parameters.Add(new MySqlParameter("ProfessorName", discipline.ProfessorName));
                            command.ExecuteNonQuery();
                        }


                        result = Content(HttpStatusCode.Created, "Successfully created discipline!");
                    }
                    catch
                    {
                        result = NotFound();
                    }
                }
            }

            return result;
        }

        [HttpPut()]
        public IHttpActionResult Put(int id, Discipline discipline)
        {
            IHttpActionResult result = null;
            if (string.IsNullOrEmpty(discipline.DisciplineName))
            {
                result = Content(HttpStatusCode.BadRequest, "Discipline name is requered!");
            }
            else if (id == 0 || discipline is null)
            {
                result = NotFound();
            }
            else
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    try
                    {
                        using (MySqlCommand command = new MySqlCommand(
                            "UPDATE  tfb8.discipline set disciplinename =  @DisciplineName, professorName = @ProfessorName where disciplineid = @DisciplineId", con))
                        {
                            command.Parameters.Add(new MySqlParameter("DisciplineName", discipline.DisciplineName));
                            command.Parameters.Add(new MySqlParameter("ProfessorName", discipline.ProfessorName));
                            command.Parameters.Add(new MySqlParameter("DisciplineId", id));
                            command.ExecuteNonQuery();
                        }
                    }
                    catch
                    {
                        result = NotFound();
                    }
                    result = Ok(discipline);
                }

            }

            return result;
        }

        // DELETE api/<controller>/5
        [HttpDelete()]
        public IHttpActionResult Delete(int id)
        {
            IHttpActionResult result = null;

            if (id != 0)
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    try
                    {
                        using (MySqlCommand command = new MySqlCommand(
                            "DELETE from tfb8.discipline where disciplineid = @DisciplineId", con))
                        {
                            command.Parameters.Add(new MySqlParameter("DisciplineId", id));
                            command.ExecuteNonQuery();
                        }
                    }
                    catch
                    {
                        result = NotFound();
                    }
                    result = Ok();
                }
            }
            else
            {
                result = NotFound();
            }

            return result;
        }
    }
}
