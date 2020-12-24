namespace TFB8.Controllers
{
    using MySql.Data.MySqlClient;
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using TFB8.Models;

    public class DisciplineController : ApiController
    {
        string connectionString =
            @"server=localhost;userid=emanolov;password=test;database=tfb8";

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
                result = NotFound();
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
            IHttpActionResult ret = null;

            if (discipline is null)
            {
                ret = NotFound();
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

                        ret = Created<Discipline>(Request.RequestUri +
                                                  discipline.DisciplineId.ToString(),
                            discipline);
                    }
                    catch
                    {
                        ret = NotFound();
                    }
                }
            }

            return ret;
        }

        [HttpPut()]
        public IHttpActionResult Put(int id, Discipline discipline)
        {
            IHttpActionResult ret = null;

            if (id == 0 || discipline is null)
            {
                ret = NotFound();
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
                        ret = NotFound();
                    }
                    ret = Ok(discipline);
                }

            }

            return ret;
        }
    }
}
